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

            if (width < Constants.MINIMUM_MAZE_SIZE || height < Constants.MINIMUM_MAZE_SIZE)
                throw new Exception($"A valid maze cannot subceed the limit of {Constants.MINIMUM_MAZE_SIZE} in height or width.");

            mazefield = new MazeField[width][];
            for (int w = 0; w < width; w++)
            {
                mazefield[w] = new MazeField[height];
            }
        }

        public MazeFieldType GetMazeTypeOnPos(MazeCoordinate coordinate)
        {
            return GetMazeTypeOnPos(coordinate.X, coordinate.Y);
        }

        public bool IsCorridor(MazeCoordinate coordinate)
        {
            return GetMazeTypeOnPos(coordinate) == MazeFieldType.Corridor;
        }

        public MazeFieldType GetMazeTypeOnPos(int x, int y)
        {
            return mazefield[x][y].type;
        }

        public MazeTransformationStep SetMazeTypeOnPos(MazeCoordinate coordinate, MazeFieldType type)
        {
            return SetMazeTypeOnPos(coordinate.X, coordinate.Y, type);
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
            return mazefield[coordinate.X][coordinate.Y].type != typeAfter;
        }

        public MazeTransformationStep SetMazeTypeOnPos(int x, int y, MazeFieldType type)
        {
            var step = new MazeTransformationStep() { coordinate = new MazeCoordinate() { X = x, Y = y }, typeAfterTransform = type };

            mazefield[x][y].type = type;
            this.drawCallback?.Invoke(step);

            return step;
        }

        public void TransformMaze(MazeTransformationStep step)
        {
            mazefield[step.coordinate.X][step.coordinate.Y].type = step.typeAfterTransform;
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

        internal IEnumerable<MazeTransformationStep> MakeRectangle(MazeCoordinate upperleft, MazeCoordinate downright, MazeFieldType defaultType = MazeFieldType.Wall)
        {
            int i;
            for (i = 0; i < this.height; i++)
            { // Left / Right walls
                yield return this.SetMazeTypeOnPos(upperleft.X, upperleft.Y - i, defaultType);
                yield return this.SetMazeTypeOnPos(downright.X, upperleft.Y - i, defaultType);
            }

            for (i = 0; i < this.width; i++)
            { // Top / Bottom walls
                yield return this.SetMazeTypeOnPos(upperleft.X + i, upperleft.Y, defaultType);
                yield return this.SetMazeTypeOnPos(upperleft.X + i, downright.Y, defaultType);
            }
        }

        /// <summary>
        /// Determines if the coordinate is within the maze walls.
        /// </summary>
        /// <returns>Returns true if point is inside the maze. Returns false when outside or directly on the outer wall.</returns>
        internal bool IsPointInMaze(MazeCoordinate point)
        {
            // Out of the maze?
            if (point.X <= 0 || point.X >= width - 1)
                return false;

            if (point.Y >= height - 1 || point.Y <= 0)
                return false;

            return true;
        }
    }
}