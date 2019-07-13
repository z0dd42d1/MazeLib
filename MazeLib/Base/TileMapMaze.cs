using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeLib.Base
{
    public class TileMapMaze : IMaze
    {
        private readonly int height;
        private readonly int width;
        private readonly MazeField[][] mazefield;
        private Action<MazeTransformationStep> drawCallback;

        public int GetWidth()
        {
            return width;
        }

        public int GetHeight()
        {
            return height;
        }

        public TileMapMaze(int width, int height)
        {
            this.width = width;
            this.height = height;

            mazefield = new MazeField[width][];
            for (int w = 0; w < width; w++)
            {
                mazefield[w] = new MazeField[height];
            }
        }

        public MazeFieldType GetMazeTypeOnPos(MazeCoordinate coordinate)
        {
            return GetMazeTypeOnPos(coordinate.x, coordinate.y);
        }

        public MazeFieldType GetMazeTypeOnPos(int x, int y)
        {
            return mazefield[x][y].type;
        }

        public MazeTransformationStep SetMazeTypeOnPos(MazeCoordinate coordinate, MazeFieldType type)
        {
            return SetMazeTypeOnPos(coordinate.x, coordinate.y, type);
        }

        internal void SetDrawCallback(Action<MazeTransformationStep> drawCallback)
        {
            this.drawCallback = drawCallback;
        }

        public bool WouldChangeMazeFieldType(MazeTransformationStep step)
        {
            return WouldChangeMazeFieldType(step.coordinate, step.typeAfterTransform);
        }

        public bool WouldChangeMazeFieldType(int x, int y, MazeFieldType typeAfter)
        {
            return mazefield[x][y].type != typeAfter;
        }

        public bool WouldChangeMazeFieldType(MazeCoordinate coordinate, MazeFieldType typeAfter)
        {
            return mazefield[coordinate.x][coordinate.y].type != typeAfter;
        }

        public MazeTransformationStep SetMazeTypeOnPos(int x, int y, MazeFieldType type)
        {
            var step = new MazeTransformationStep() { coordinate = new MazeCoordinate() { x = x, y = y }, typeAfterTransform = type };

            mazefield[x][y].type = type;
            this.drawCallback?.Invoke(step);

            return step;
        }

        public void TransformMaze(MazeTransformationStep step)
        {
            mazefield[step.coordinate.x][step.coordinate.y].type = step.typeAfterTransform;
        }

        public void OverrideAllMazeFields(MazeFieldType defaultType = MazeFieldType.Corridor)
        {
            for (int w = 0; w < width; w++)
            {
                for (int h = 0; h < height; h++)
                {
                    mazefield[w][h].type = defaultType;
                }
            }
        }

        internal void MakeRectangle(MazeCoordinate upperleft, MazeCoordinate downright, MazeFieldType defaultType = MazeFieldType.Wall)
        {
            int i;
            for (i = 0; i < this.height; i++)
            { // Left / Right walls
                this.SetMazeTypeOnPos(upperleft.x, upperleft.y - i, defaultType);
                this.SetMazeTypeOnPos(downright.x, upperleft.y - i, defaultType);
            }

            for (i = 0; i < this.width; i++)
            { // Top / Bottom walls
                this.SetMazeTypeOnPos(upperleft.x + i, upperleft.y, defaultType);
                this.SetMazeTypeOnPos(upperleft.x + i, downright.y, defaultType);
            }
        }

        /// <summary>
        /// Determines if the coordinate is within the maze walls.
        /// </summary>
        /// <returns>Returns true if point is inside the maze. Returns false when outside or directly on the outer wall.</returns>
        internal bool IsPointInMaze(MazeCoordinate point)
        {
            // Out of the maze?
            if (point.x <= 0 || point.x >= width - 1)
                return false;

            if (point.y >= height - 1 || point.y <= 0)
                return false;

            return true;
        }
    }
}