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

        public override string GetName()
        {
            return this.GetType().Name;
        }

        public override IList<MazeTransformationStep> GenerateMazeFullSize()
        {
            InitializeMaze();

            return InternalGenerateMazeFullSize().ToList();
        }

        private IEnumerable<MazeTransformationStep> InternalGenerateMazeFullSize()
        {
            entrance = new MazeCoordinate(upperleft.x + 1 + random.Next(this.currentMaze.GetWidth() - 2), upperleft.y);

            pointStack.Push(entrance);

            Nullable<MazeCoordinate> newCorridor;

            while (pointStack.Count() != 0)
            {
                newCorridor = FindWay();

                if (newCorridor == null)
                {
                    newCorridor = pointStack.Pop(); // One step back
                    if (newCorridor != null)
                        yield return currentMaze.SetMazeTypeOnPos(newCorridor.Value, MazeFieldType.Corridor);
                }
                else
                {
                    pointStack.Push(newCorridor.Value);
                    yield return currentMaze.SetMazeTypeOnPos(newCorridor.Value, MazeFieldType.Corridor);
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
            possibleWays.Add(new MazeCoordinate(akt.x, akt.y - 1));
            possibleWays.Add(new MazeCoordinate(akt.x + 1, akt.y));
            possibleWays.Add(new MazeCoordinate(akt.x - 1, akt.y));
            possibleWays.Add(new MazeCoordinate(akt.x, akt.y + 1));

            int i = random.Next(possibleWays.Count);

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
                        i = random.Next(possibleWays.Count);
                }
            }
            return null;
        }

        private bool IsValidWay(MazeCoordinate point, MazeCoordinate comeFrom)
        {
            if (!this.currentMaze.IsPointInMaze(point)) return false;

            // Already something other than a wall?
            if (this.currentMaze.GetMazeTypeOnPos(point) != MazeFieldType.Wall)
                return false;

            // Make sure we do not create a way through a wall.
            MazeCoordinate up = new MazeCoordinate(point.x, point.y - 1);
            MazeCoordinate right = new MazeCoordinate(point.x + 1, point.y);
            MazeCoordinate down = new MazeCoordinate(point.x, point.y + 1);
            MazeCoordinate left = new MazeCoordinate(point.x - 1, point.y);

            MazeCoordinate[] pointsToCheck = { up, right, down, left };

            for (int i = 0; i < pointsToCheck.Length; i++)
            {
                if (!pointsToCheck[i].Equals(comeFrom))
                {
                    if (currentMaze.GetMazeTypeOnPos(pointsToCheck[i]) == MazeFieldType.Corridor) return false;
                }
            }

            return true;
        }

        public override void InitializeMaze()
        {
            downright = new MazeCoordinate(currentMaze.GetWidth() - 1, 0);
            upperleft = new MazeCoordinate(0, currentMaze.GetHeight() - 1);

            currentMaze.OverrideAllMazeFields(MazeFieldType.Wall);
        }

        internal override MazeTransformationStep TryPlaceEntrance()
        {
            // the entrance is placed during maze generation. This algorithm doesn't not need an extra handling.
            return this.currentMaze.SetMazeTypeOnPos(entrance, MazeFieldType.Entrance);
        }

        internal override MazeTransformationStep TryPlaceExit()
        {
            List<MazeCoordinate> possibleExits = new List<MazeCoordinate>();

            // collect all possible exits
            for (int i = currentMaze.GetWidth() - 2; i >= 1; i--)
            {
                possibleExits.Add(new MazeCoordinate(i, downright.y));
            }

            // check in random order if exit is valid
            while (possibleExits.Count > 0)
            {
                var possibleExit = possibleExits.ElementAt(random.Next(possibleExits.Count));

                if (currentMaze.GetMazeTypeOnPos(possibleExit.x, possibleExit.y + 1) == MazeFieldType.Corridor)
                {
                    return currentMaze.SetMazeTypeOnPos(possibleExit, MazeFieldType.Exit);
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