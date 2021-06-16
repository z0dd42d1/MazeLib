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

    public class RandomizedPrims : MazeGenAlgorithmBase
    {
        private MazeCoordinate entrance;

        private HashSet<MazeCoordinate> VisitedList;
        private HashSet<MazeCoordinate> WallsList;
        
        public RandomizedPrims(int randomSeed = -1) : base(randomSeed)
        {
        }

        internal override IEnumerable<MazeTransformationStep> InternalGenerateMazeFullSize()
        {
            InitializeMaze();

            VisitedList = new HashSet<MazeCoordinate>();
            WallsList = new HashSet<MazeCoordinate>();

            // Pick a cell and mark it as part of the maze
            entrance = new MazeCoordinate(Random.Next(1, this.CurrentMaze.GetWidth() - 2), this.CurrentMaze.GetHeight() - 2);

            VisitedList.Add(entrance);
            yield return this.CurrentMaze.SetMazeTypeOnPos(entrance, MazeFieldType.Corridor);

            //add the walls to the wall list
            AddToWallList(entrance);

            int rand;
            MazeCoordinate temp;
            while (WallsList.Count > 0) // while there are walls
            {
                // Pick random wall
                rand = Random.Next(WallsList.Count);
                temp = WallsList.ElementAt(rand);

                // If the wall does not divide two cells
                if (!checkIfWallDividesTwoCorridors(temp))
                {
                    VisitedList.Add(temp);

                    // make the wall a passage
                    yield return this.CurrentMaze.SetMazeTypeOnPos(temp, MazeFieldType.Corridor);
                    WallsList.Remove(temp);

                    addSourroundingWallsToWallList(temp);
                }
                else
                {
                    WallsList.Remove(temp);
                }
            }

            yield return TryPlaceExit();
            yield return TryPlaceEntrance();
        }

        public override string GetName()
        {
            return this.GetType().Name;
        }

        public override IEnumerable<MazeTransformationStep> InitializeMaze()
        {
            CurrentMaze.OverrideAllMazeFields(MazeFieldType.Wall);
            return null;
        }

        private void addSourroundingWallsToWallList(MazeCoordinate target)
        {
            MazeCoordinate[] temp = MazeCoordinate.GetHorizontalVerticalAdjacentCoordinates(target);

            for (int i = 0; i < temp.Length; i++)
            {
                if (this.CurrentMaze.IsPointInMaze(temp[i]))
                    if (!VisitedList.Contains(temp[i]))
                        if (!WallsList.Contains(temp[i]))
                        {
                            WallsList.Add(temp[i]);
                        }
            }
        }

        private bool checkIfWallDividesTwoCorridors(MazeCoordinate target)
        {
            MazeCoordinate[] temp = MazeCoordinate.GetHorizontalVerticalAdjacentCoordinates(target);

            int TargetWallAlignsWithCorridors = 0;
            foreach (MazeCoordinate c in temp)
            {
                if (this.CurrentMaze.GetMazeTypeOnPos(c) == MazeFieldType.Corridor)
                    if (target.X == c.X || target.Y == c.Y)
                    {
                        TargetWallAlignsWithCorridors++;
                    }
            }

            // direct connection
            if (TargetWallAlignsWithCorridors > 2) return true;

            temp = MazeCoordinate.GetAllAdjacentCoordinates(target);

            int CorridorFieldsInCloseProximity = 0;
            HashSet<MazeCoordinate> inProximity = new HashSet<MazeCoordinate>();
            foreach (MazeCoordinate c in temp)
            {
                if (CurrentMaze.GetMazeTypeOnPos(c) != MazeFieldType.Wall)
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
                // check if the corridors are adjacent each other then its OK.
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
            MazeCoordinate[] temp = MazeCoordinate.GetHorizontalVerticalAdjacentCoordinates(akt);

            for (int i = 0; i < temp.Length; i++)
            {
                if (this.CurrentMaze.IsPointInMaze(temp[i]))

                    if (!VisitedList.Contains(temp[i]))
                    {
                        if (!WallsList.Contains(temp[i]))
                        {
                            WallsList.Add(temp[i]);
                        }
                    }
            }
        }
    }
}