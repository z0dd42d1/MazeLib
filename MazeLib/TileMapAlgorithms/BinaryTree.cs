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
            /*
             * In the wall added version, for each vertex add a wall segment leading down or right, but not both.
             */

            for (int y = 1; y < this.GetMazeHeight() - 2; y += 2)
            {
                for (int x = 1; x < this.GetMazeWidth() - 2; x += 2)
                {
                    switch (random.Next(2))
                    {
                        case 0: // new wall down
                            if (downright.x - x > 2) // don't draw new wall downward on the outer edge
                            {
                                yield return this.currentMaze.SetMazeTypeOnPos(upperleft.x + x + 1, upperleft.y - y, MazeFieldType.Wall);
                                if (upperleft.y - y > 2) // don't close corridors to the outer walls
                                {
                                    yield return this.currentMaze.SetMazeTypeOnPos(upperleft.x + x + 1, upperleft.y - y - 1, MazeFieldType.Wall);
                                }
                            }
                            break;

                        case 1:
                            if (upperleft.y - y > 2) // don't draw new wall horizontal on the outer edge
                            {
                                yield return this.currentMaze.SetMazeTypeOnPos(upperleft.x + x, upperleft.y - (y + 1), MazeFieldType.Wall);
                                if (downright.x - x > 2) // don't close corridors to the outer walls
                                {
                                    yield return this.currentMaze.SetMazeTypeOnPos(upperleft.x + x + 1, upperleft.y - (y + 1), MazeFieldType.Wall);
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

        public override void InitializeMaze()
        {
            this.currentMaze.OverrideAllMazeFields();
            downright = new MazeCoordinate(this.currentMaze.GetWidth() - 1, 0);
            upperleft = new MazeCoordinate(0, this.currentMaze.GetHeight() - 1);
            this.currentMaze.MakeRectangle(upperleft, downright);
        }

        internal override MazeTransformationStep TryPlaceEntrance()
        {
            // no need to check, the nature of the algorithm will guarantee that all positions on this side are valid entrances.
            return this.currentMaze.SetMazeTypeOnPos(random.Next(1, currentMaze.GetWidth() - 2), 0, MazeFieldType.Entrance);
        }

        internal override MazeTransformationStep TryPlaceExit()
        {
            List<MazeCoordinate> possibleExits = new List<MazeCoordinate>();

            // collect all possible exits
            for (int y = currentMaze.GetHeight() - 2; y >= currentMaze.GetHeight() / 2; y--)
            {
                possibleExits.Add(new MazeCoordinate(0, y));
            }

            // check in random order if exit is valid
            while (possibleExits.Count > 0)
            {
                var possibleExit = possibleExits.ElementAt(random.Next(possibleExits.Count));

                if (this.currentMaze.GetMazeTypeOnPos(1, possibleExit.y) == MazeFieldType.Corridor)
                {
                    return this.currentMaze.SetMazeTypeOnPos(0, possibleExit.y, MazeFieldType.Exit);
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