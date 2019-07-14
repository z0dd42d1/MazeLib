using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeLib.Base
{
    public struct MazeCoordinate : IEquatable<MazeCoordinate>
    {
        private int x;
        private int y;

        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }

        public MazeCoordinate(int x, int y) : this()
        {
            this.X = x;
            this.Y = y;
        }

        public override bool Equals(object obj)
        {
            if (obj is MazeCoordinate)
            {
                var o = (MazeCoordinate)obj;
                return
                    o.X == this.X &&
                    o.Y == this.Y;
            }
            return false;
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash = (hash * 7) + x.GetHashCode();
            hash = (hash * 7) + y.GetHashCode();
            return hash;
        }

        public static bool operator ==(MazeCoordinate left, MazeCoordinate right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(MazeCoordinate left, MazeCoordinate right)
        {
            return !(left == right);
        }

        public bool Equals(MazeCoordinate o)
        {
            return
              o.X == this.X &&
              o.Y == this.Y;
        }

        /// <summary>
        /// Checks if this coordinate is directly adjacent to the passed tile
        /// on the horizontal or vertical axis.
        /// </summary>
        public bool IsAdjacentTo(MazeCoordinate coordinate)
        {
            // Directly adjacent to each other on x-Axis OR on y-Axis.
            return (this.X == coordinate.X && Math.Abs(this.Y - coordinate.Y) == 1) ||
                (this.Y == coordinate.Y && Math.Abs(this.X - coordinate.X) == 1)
                ;
        }

        /// <summary>
        /// This method returns an array with 4 coordinates which
        /// represent the direct vertical and horizontal adjacent MazeCoordinates.
        /// </summary>
        public static MazeCoordinate[] GetHorizontalVerticalAdjacentCoordinates(MazeCoordinate coordinateCenter)
        {
            var array = new MazeCoordinate[4];
            array[0] = new MazeCoordinate(coordinateCenter.X, coordinateCenter.Y - 1); // up
            array[1] = new MazeCoordinate(coordinateCenter.X + 1, coordinateCenter.Y); // right
            array[2] = new MazeCoordinate(coordinateCenter.X - 1, coordinateCenter.Y); // left
            array[3] = new MazeCoordinate(coordinateCenter.X, coordinateCenter.Y + 1); // down
            return array;
        }

        /// <summary>
        /// This method returns an array with 8 coordinates which
        /// represent all adjacent MazeCoordinates.
        /// </summary>
        public static MazeCoordinate[] GetAllAdjacentCoordinates(MazeCoordinate coordinateCenter)
        {
            return new MazeCoordinate[]{
                new MazeCoordinate(coordinateCenter.X, coordinateCenter.Y - 1), // up
                new MazeCoordinate(coordinateCenter.X + 1, coordinateCenter.Y), // right
                new MazeCoordinate(coordinateCenter.X - 1, coordinateCenter.Y), // left
                new MazeCoordinate(coordinateCenter.X, coordinateCenter.Y + 1), // down
                new MazeCoordinate(coordinateCenter.X + 1, coordinateCenter.Y - 1), // up right
                new MazeCoordinate(coordinateCenter.X - 1, coordinateCenter.Y - 1), // up left
                new MazeCoordinate(coordinateCenter.X + 1, coordinateCenter.Y + 1), // down right
                new MazeCoordinate(coordinateCenter.X - 1, coordinateCenter.Y + 1), // down left
        };
        }
    }
}