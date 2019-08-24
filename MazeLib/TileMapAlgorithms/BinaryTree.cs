using MazeLib.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeLib.TileMapAlgorithms
{
    public class BinaryTree : MazeGenAlgorithmBase
    {
        private MazeCoordinate downright;
        private MazeCoordinate upperleft;

        internal override IEnumerable<MazeTransformationStep> InternalGenerateMazeFullSize()
        {
            foreach (var step in InitializeMaze())
            {
                yield return step;
            }
            /*
             * In the wall added version, for each vertex add a wall segment leading down or right, but not both.
             */

            for (int y = 1; y < this.GetMazeHeight() - 2; y += 2)
            {
                for (int x = 1; x < this.GetMazeWidth() - 2; x += 2)
                {
                    switch (Random.Next(2))
                    {
                        case 0: // new wall down
                            if (downright.X - x > 2) // don't draw new wall downward on the outer edge
                            {
                                yield return this.CurrentMaze.SetMazeTypeOnPos(upperleft.X + x + 1, upperleft.Y - y, MazeFieldType.Wall);
                                if (upperleft.Y - y > 2) // don't close corridors to the outer walls
                                {
                                    yield return this.CurrentMaze.SetMazeTypeOnPos(upperleft.X + x + 1, upperleft.Y - y - 1, MazeFieldType.Wall);
                                }
                            }
                            break;

                        case 1:
                            if (upperleft.Y - y > 2) // don't draw new wall horizontal on the outer edge
                            {
                                yield return this.CurrentMaze.SetMazeTypeOnPos(upperleft.X + x, upperleft.Y - (y + 1), MazeFieldType.Wall);
                                if (downright.X - x > 2) // don't close corridors to the outer walls
                                {
                                    yield return this.CurrentMaze.SetMazeTypeOnPos(upperleft.X + x + 1, upperleft.Y - (y + 1), MazeFieldType.Wall);
                                }
                            }
                            break;

                        default:
                            break;
                    }
                }
            }

            yield return TryPlaceEntrance();
            yield return TryPlaceExit();
        }

        public override string GetName()
        {
            return this.GetType().Name;
        }

        public override IEnumerable<MazeTransformationStep> InitializeMaze()
        {
            this.CurrentMaze.OverrideAllMazeFields();
            downright = new MazeCoordinate(this.CurrentMaze.GetWidth() - 1, 0);
            upperleft = new MazeCoordinate(0, this.CurrentMaze.GetHeight() - 1);

            foreach (var step in this.CurrentMaze.MakeRectangle(upperleft, downright))
            {
                yield return step;
            }
        }

        internal override MazeTransformationStep TryPlaceEntrance()
        {
            // no need to check, the nature of the algorithm will guarantee that all positions on this side are valid entrances.
            return this.CurrentMaze.SetMazeTypeOnPos(Random.Next(1, CurrentMaze.GetWidth() - 2), 0, MazeFieldType.Entrance);
        }

        internal override MazeTransformationStep TryPlaceExit()
        {
            List<MazeCoordinate> possibleExits = new List<MazeCoordinate>();

            // collect all possible exits
            for (int y = CurrentMaze.GetHeight() - 2; y >= CurrentMaze.GetHeight() / 2; y--)
            {
                possibleExits.Add(new MazeCoordinate(0, y));
            }

            // check in random order if exit is valid
            while (possibleExits.Count > 0)
            {
                var possibleExit = possibleExits.ElementAt(Random.Next(possibleExits.Count));

                if (this.CurrentMaze.GetMazeTypeOnPos(1, possibleExit.Y) == MazeFieldType.Corridor)
                {
                    return this.CurrentMaze.SetMazeTypeOnPos(0, possibleExit.Y, MazeFieldType.Exit);
                }
                else
                {
                    possibleExits.Remove(possibleExit);
                }
            }

            throw new Exception("Could not place a valid exit.");
        }
    }
}