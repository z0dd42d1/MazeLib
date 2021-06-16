using MazeLib.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeLib.TileMapAlgorithms
{
    public class DepthFirst : MazeGenAlgorithmBase
    {
        private MazeCoordinate downright;
        private MazeCoordinate upperleft;
        private Stack<MazeCoordinate> pointStack = new Stack<MazeCoordinate>();
        private List<MazeCoordinate> possibleWays = new List<MazeCoordinate>();

        private MazeCoordinate entrance;
        
        public DepthFirst(int randomSeed = -1) : base(randomSeed)
        {
        }

        public override string GetName()
        {
            return this.GetType().Name;
        }

        internal override IEnumerable<MazeTransformationStep> InternalGenerateMazeFullSize()
        {
            InitializeMaze();
            entrance = new MazeCoordinate(upperleft.X + 1 + Random.Next(this.CurrentMaze.GetWidth() - 2), upperleft.Y);

            pointStack.Push(entrance);

            Nullable<MazeCoordinate> newCorridor;

            while (pointStack.Count() != 0)
            {
                newCorridor = FindWay();

                if (newCorridor == null)
                {
                    newCorridor = pointStack.Pop(); // One step back
                    if (newCorridor != null && CurrentMaze.WouldChangeMazeFieldType(newCorridor.Value, MazeFieldType.Corridor))
                        yield return CurrentMaze.SetMazeTypeOnPos(newCorridor.Value, MazeFieldType.Corridor);
                }
                else
                {
                    pointStack.Push(newCorridor.Value);
                    yield return CurrentMaze.SetMazeTypeOnPos(newCorridor.Value, MazeFieldType.Corridor);
                }
            }

            yield return TryPlaceEntrance();
            yield return TryPlaceExit();
        }

        private Nullable<MazeCoordinate> FindWay()
        {
            MazeCoordinate akt = pointStack.Peek();
            possibleWays.Clear();

            // four possible directions
            possibleWays.Add(new MazeCoordinate(akt.X, akt.Y - 1));
            possibleWays.Add(new MazeCoordinate(akt.X + 1, akt.Y));
            possibleWays.Add(new MazeCoordinate(akt.X - 1, akt.Y));
            possibleWays.Add(new MazeCoordinate(akt.X, akt.Y + 1));

            int i = Random.Next(possibleWays.Count);

            while (possibleWays.Count != 0)
            {
                if (IsValidWay(possibleWays[i], akt))
                {
                    return possibleWays[i];
                }
                else
                {
                    possibleWays.Remove(possibleWays[i]);
                    if (possibleWays.Count != 0)
                        i = Random.Next(possibleWays.Count);
                }
            }
            return null;
        }

        private bool IsValidWay(MazeCoordinate point, MazeCoordinate comeFrom)
        {
            if (!this.CurrentMaze.IsPointInMaze(point)) return false;

            // Already something other than a wall?
            if (this.CurrentMaze.GetMazeTypeOnPos(point) != MazeFieldType.Wall)
                return false;

            // Make sure we do not create a way through a wall.
            MazeCoordinate up = new MazeCoordinate(point.X, point.Y - 1);
            MazeCoordinate right = new MazeCoordinate(point.X + 1, point.Y);
            MazeCoordinate down = new MazeCoordinate(point.X, point.Y + 1);
            MazeCoordinate left = new MazeCoordinate(point.X - 1, point.Y);

            MazeCoordinate[] pointsToCheck = { up, right, down, left };

            for (int i = 0; i < pointsToCheck.Length; i++)
            {
                if (!pointsToCheck[i].Equals(comeFrom))
                {
                    if (CurrentMaze.GetMazeTypeOnPos(pointsToCheck[i]) == MazeFieldType.Corridor) return false;
                }
            }

            return true;
        }

        public override IEnumerable<MazeTransformationStep> InitializeMaze()
        {
            downright = new MazeCoordinate(CurrentMaze.GetWidth() - 1, 0);
            upperleft = new MazeCoordinate(0, CurrentMaze.GetHeight() - 1);

            CurrentMaze.OverrideAllMazeFields(MazeFieldType.Wall);
            return null;
        }

        internal override MazeTransformationStep TryPlaceEntrance()
        {
            // the entrance is placed during maze generation. This algorithm doesn't not need an extra handling.
            return this.CurrentMaze.SetMazeTypeOnPos(entrance, MazeFieldType.Entrance);
        }

        internal override MazeTransformationStep TryPlaceExit()
        {
            List<MazeCoordinate> possibleExits = new List<MazeCoordinate>();

            // collect all possible exits
            for (int i = CurrentMaze.GetWidth() - 2; i >= 1; i--)
            {
                possibleExits.Add(new MazeCoordinate(i, downright.Y));
            }

            // check in random order if exit is valid
            while (possibleExits.Count > 0)
            {
                var possibleExit = possibleExits.ElementAt(Random.Next(possibleExits.Count));

                if (CurrentMaze.GetMazeTypeOnPos(possibleExit.X, possibleExit.Y + 1) == MazeFieldType.Corridor)
                {
                    return CurrentMaze.SetMazeTypeOnPos(possibleExit, MazeFieldType.Exit);
                }
                else
                {
                    possibleExits.Remove(possibleExit);
                }
            }

            // TODO Maybe not the best way to handle this, but the maze is useless without valid exit.
            throw new Exception("Could not find a suitable exit position.");
        }
    }
}