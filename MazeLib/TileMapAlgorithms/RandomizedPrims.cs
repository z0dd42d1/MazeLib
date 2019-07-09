using MazeLib.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MazeLib.TileMapAlgorithms
{
    /*
    * This algorithm is a randomized version of Prim's algorithm. Start with a
    * grid full of walls. Pick a cell, mark it as part of the maze. Add the
    * walls of the cell to the wall list. While there are walls in the list:
    * Pick a random wall from the list. If the cell on the opposite side isn't
    * in the maze yet: Make the wall a passage and mark the cell on the
    * opposite side as part of the maze. Add the neighboring walls of the cell
    * to the wall list. If the cell on the opposite side already was in the
    * maze, remove the wall from the list.
    */

    // TODO translate german code commenting
    public class RandomizedPrims : MazeGenAlgorithmBase
    {
        private Random random = new Random();

        private MazeCoordinate upperleft;
        private MazeCoordinate downright;

        private MazeCoordinate entrance;
        private MazeCoordinate exit;

        private HashSet<MazeCoordinate> VisitedList;
        private HashSet<MazeCoordinate> WallsList;

        public override IList<MazeTransformationStep> GenerateMazeFullSize()
        {
            InitializeMaze();

            return InternalGenerateMazeFullSize().ToList();
        }

        private IEnumerable<MazeTransformationStep> InternalGenerateMazeFullSize()
        {
            this.currentMaze.OverrideAllMazeFields(MazeFieldType.Wall);

            VisitedList = new HashSet<MazeCoordinate>();
            WallsList = new HashSet<MazeCoordinate>();

            // Pick a cell and mark it as part of the maze
            entrance = new MazeCoordinate(random.Next(1, this.currentMaze.GetWidth() - 2), this.currentMaze.GetHeight() - 2);

            VisitedList.Add(entrance);
            yield return this.currentMaze.SetMazeTypeOnPos(entrance, MazeFieldType.Corridor);

            //add the walls to the wall list
            AddToWallList(entrance);

            int rand;
            MazeCoordinate temp;
            while (WallsList.Count > 0) // while there are walls
            {
                // Pick random wall
                rand = random.Next(WallsList.Count);
                temp = WallsList.ElementAt(rand);

                // choose direction to check first
                bool horizontal = random.Next(1) == 0 ? true : false;

                // If the wall does not devides two cells
                if (!checkIfWallDividesTwoCorridors(temp))
                {
                    VisitedList.Add(temp);

                    this.currentMaze.SetMazeTypeOnPos(temp, MazeFieldType.Corridor);
                    WallsList.Remove(temp);

                    addSourroundingWallsToWallList(temp);
                }
                else
                {
                    WallsList.Remove(temp);
                }
            }

            // createEntranceExit();
        }

        public override string GetName()
        {
            return this.GetType().Name;
        }

        public override void InitializeMaze()
        {
            downright = new MazeCoordinate(currentMaze.GetWidth() - 1, 0);
            upperleft = new MazeCoordinate(0, currentMaze.GetHeight() - 1);

            currentMaze.OverrideAllMazeFields(MazeFieldType.Wall);
        }

        internal override MazeTransformationStep PlaceEntrance()
        {
            throw new NotImplementedException();
        }

        internal override MazeTransformationStep PlaceExit()
        {
            throw new NotImplementedException();
        }

        private void addSourroundingWallsToWallList(MazeCoordinate akt)
        {
            MazeCoordinate[] temp = new MazeCoordinate[4];

            temp[0] = new MazeCoordinate(akt.x, akt.y - 1); // hoch
            temp[1] = new MazeCoordinate(akt.x + 1, akt.y); // rechts
            temp[2] = new MazeCoordinate(akt.x - 1, akt.y); // links
            temp[3] = new MazeCoordinate(akt.x, akt.y + 1); // runter

            for (int i = 0; i < 4; i++)
            {
                if (this.currentMaze.IsPointInMaze(temp[i]))
                    if (!VisitedList.Contains(temp[i])) // Noch kein teil des Labyrinths?
                        if (!WallsList.Contains(temp[i]))
                        { //Noch nicht in der Liste?
                            WallsList.Add(temp[i]);
                        }
            }
        }

        private bool checkIfWallDividesTwoCorridors(MazeCoordinate akt)
        {
            // checkt ob mehr als eine Zelle ein Hallway ist, wenn ja gibt er false zur�ck
            // wenn nein true

            int zaehler = 0;
            MazeCoordinate[] temp = new MazeCoordinate[8];

            // TODO dont use new, get them from the maze array directly
            temp[0] = new MazeCoordinate(akt.x, akt.y - 1); // hoch
            temp[1] = new MazeCoordinate(akt.x + 1, akt.y - 1); // hoch rechts
            temp[2] = new MazeCoordinate(akt.x - 1, akt.y - 1); // hoch links
            temp[3] = new MazeCoordinate(akt.x + 1, akt.y); // rechts
            temp[4] = new MazeCoordinate(akt.x - 1, akt.y); // links
            temp[5] = new MazeCoordinate(akt.x, akt.y + 1); // runter
            temp[6] = new MazeCoordinate(akt.x + 1, akt.y + 1); // runter rechts
            temp[7] = new MazeCoordinate(akt.x - 1, akt.y + 1); // runter links

            for (int i = 0; i < temp.Length; i++)
            {
                if (VisitedList.Contains(temp[i])) zaehler++;
            }

            return zaehler >= 3;
        }

        private void AddToWallList(MazeCoordinate akt)
        {
            //
            //Pr�fe alle 4 M�glichkeiten und added sie zu der wall-list wenn sie den bedienungen gen�gen
            //
            MazeCoordinate[] temp = new MazeCoordinate[4];

            temp[0] = new MazeCoordinate(akt.x, akt.y - 1); // hoch
            temp[1] = new MazeCoordinate(akt.x + 1, akt.y); // rechts
            temp[2] = new MazeCoordinate(akt.x - 1, akt.y); // links
            temp[3] = new MazeCoordinate(akt.x, akt.y + 1); // runter

            for (int i = 0; i < 4; i++)
            {
                if (this.currentMaze.IsPointInMaze(temp[i]))

                    //if (!VisitedList.Contains(temp[i]))// schon Teil des Labyrinths?
                    //{
                    if (!WallsList.Contains(temp[i]))//Noch nicht auf der Liste?
                    {
                        WallsList.Add(temp[i]);
                    }

                //}
            }
        }
    }
}