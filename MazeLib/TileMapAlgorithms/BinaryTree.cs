using MazeLib.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeLib.MazeGenAlgos
{
    public class BinaryTree : MazeGenAlgorithmBase
    {
        private Random random = new Random();
        private MazeCoordinate downright;
        private MazeCoordinate upperleft;

        private IEnumerable<MazeTransformationStep> InternalGenerateMazeFullSize()
        {
            this.SetCurrentMaze(GetInitializedMaze());

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
        }

        public override string GetName()
        {
            return this.GetType().Name;
        }

        public override IList<MazeTransformationStep> GenerateMazeFullSize()
        {
            return InternalGenerateMazeFullSize().ToList();
        }

        public override TileMapMaze GetInitializedMaze()
        {
            TileMapMaze maze = new TileMapMaze(currentMaze.GetWidth(), currentMaze.GetHeight());
            maze.OverrideAllMazeFields();
            downright = new MazeCoordinate(maze.GetWidth() - 1, 0);
            upperleft = new MazeCoordinate(0, maze.GetHeight() - 1);
            maze.MakeRectangle(upperleft, downright);
            return maze;
        }

        internal override MazeTransformationStep PlaceEntrance()
        {
            throw new NotImplementedException();
        }

        internal override MazeTransformationStep PlaceExit()
        {
            throw new NotImplementedException();
        }
    }
}