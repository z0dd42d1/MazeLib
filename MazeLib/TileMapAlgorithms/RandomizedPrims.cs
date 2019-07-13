﻿using MazeLib.Base;
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
        private MazeCoordinate entrance;

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

                // If the wall does not divide two cells
                if (!checkIfWallDividesTwoCorridors(temp))
                {
                    VisitedList.Add(temp);

                    // make the wall a passage
                    this.currentMaze.SetMazeTypeOnPos(temp, MazeFieldType.Corridor);
                    WallsList.Remove(temp);

                    addSourroundingWallsToWallList(temp);
                }
                else
                {
                    WallsList.Remove(temp);
                }
            }

            TryPlaceExit();
            TryPlaceEntrance();
        }

        public override string GetName()
        {
            return this.GetType().Name;
        }

        public override void InitializeMaze()
        {
            currentMaze.OverrideAllMazeFields(MazeFieldType.Wall);
        }

        private void addSourroundingWallsToWallList(MazeCoordinate akt)
        {
            MazeCoordinate[] temp = new MazeCoordinate[8];

            temp[0] = new MazeCoordinate(akt.x, akt.y - 1); // hoch
            temp[3] = new MazeCoordinate(akt.x + 1, akt.y); // rechts
            temp[4] = new MazeCoordinate(akt.x - 1, akt.y); // links
            temp[5] = new MazeCoordinate(akt.x, akt.y + 1); // runter

            for (int i = 0; i < temp.Length; i++)
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
            var temp = new MazeCoordinate[4];
            // will the new coordinate create a path?
            temp[0] = new MazeCoordinate(akt.x, akt.y - 1); // hoch
            temp[1] = new MazeCoordinate(akt.x + 1, akt.y); // rechts
            temp[2] = new MazeCoordinate(akt.x - 1, akt.y); // links
            temp[3] = new MazeCoordinate(akt.x, akt.y + 1); // runter

            int TargetWallAlignsWithCorridors = 0;
            foreach (MazeCoordinate c in temp)
            {
                if (this.currentMaze.GetMazeTypeOnPos(c) == MazeFieldType.Corridor)
                    if (akt.x == c.x || akt.y == c.y)
                    {
                        TargetWallAlignsWithCorridors++;
                    }
            }

            // direct connection
            if (TargetWallAlignsWithCorridors > 2) return true;

            temp = new MazeCoordinate[8];

            temp[0] = new MazeCoordinate(akt.x, akt.y - 1); // hoch
            temp[1] = new MazeCoordinate(akt.x + 1, akt.y - 1); // hoch rechts
            temp[2] = new MazeCoordinate(akt.x - 1, akt.y - 1); // hoch links
            temp[3] = new MazeCoordinate(akt.x + 1, akt.y); // rechts
            temp[4] = new MazeCoordinate(akt.x - 1, akt.y); // links
            temp[5] = new MazeCoordinate(akt.x, akt.y + 1); // runter
            temp[6] = new MazeCoordinate(akt.x + 1, akt.y + 1); // runter rechts
            temp[7] = new MazeCoordinate(akt.x - 1, akt.y + 1); // runter links

            int CorridorFieldsInCloseProximity = 0;
            HashSet<MazeCoordinate> inProximity = new HashSet<MazeCoordinate>();
            foreach (MazeCoordinate c in temp)
            {
                if (currentMaze.GetMazeTypeOnPos(c) != MazeFieldType.Wall)
                {
                    CorridorFieldsInCloseProximity++;
                    inProximity.Add(c);
                }
            }
            // This value can be reduced to create more narrow sideways, maybe make it configurable ?
            if (CorridorFieldsInCloseProximity >= 3)
            {
                return true;
            }

            if (CorridorFieldsInCloseProximity == 2)
            {
                // check if the corridors are adjacent each other then its ok.
                foreach (MazeCoordinate c in inProximity)
                {
                    var subset = inProximity.Where(x => x != c).AsEnumerable();
                    bool allAdjacent = subset.All(x => x.IsAdjacentTo(c));
                    if (allAdjacent)
                    {
                        return false;
                    }
                }
                return true;
            }

            if (TargetWallAlignsWithCorridors > 1 && CorridorFieldsInCloseProximity >= 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void AddToWallList(MazeCoordinate akt)
        {
            // TODO Refactor

            MazeCoordinate[] temp = new MazeCoordinate[8];

            temp[0] = new MazeCoordinate(akt.x, akt.y - 1); // hoch
            temp[3] = new MazeCoordinate(akt.x + 1, akt.y); // rechts
            temp[4] = new MazeCoordinate(akt.x - 1, akt.y); // links
            temp[5] = new MazeCoordinate(akt.x, akt.y + 1); // runter

            for (int i = 0; i < temp.Length; i++)
            {
                if (this.currentMaze.IsPointInMaze(temp[i]))

                    if (!VisitedList.Contains(temp[i]))// schon Teil des Labyrinths?
                    {
                        if (!WallsList.Contains(temp[i]))//Noch nicht auf der Liste?
                        {
                            WallsList.Add(temp[i]);
                        }
                    }
            }
        }
    }
}