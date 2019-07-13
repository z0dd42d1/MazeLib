using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeLib.Base
{
    public struct MazeCoordinate : IEquatable<MazeCoordinate>
    {
        public int x;
        public int y;

        public MazeCoordinate(int x, int y) : this()
        {
            this.x = x;
            this.y = y;
        }

        public override bool Equals(object obj)
        {
            if (obj is MazeCoordinate)
            {
                var o = (MazeCoordinate)obj;
                return
                    o.x == this.x &&
                    o.y == this.y;
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
              o.x == this.x &&
              o.y == this.y;
        }

        /// <summary>
        /// Checks if this coordinate is directly adjacent to the passed tile
        /// on the horizontal or vertical axis.
        /// </summary>
        public bool IsAdjacentTo(MazeCoordinate coordinate)
        {
            // Directly adjacent to each other on x-Axis OR on y-Axis.
            return (this.x == coordinate.x && Math.Abs(this.y - coordinate.y) == 1) ||
                (this.y == coordinate.y && Math.Abs(this.x - coordinate.x) == 1)
                ;
        }

        /// <summary>
        /// This method returns an array with 4 coordinates which
        /// represent the direct vertical and horizontal adjacent MazeCoordinates.
        /// </summary>
        public static MazeCoordinate[] GetHorizontalVerticalAdjacentCoordinates(MazeCoordinate coordinateCenter)
        {
            var array = new MazeCoordinate[4];
            array[0] = new MazeCoordinate(coordinateCenter.x, coordinateCenter.y - 1); // up
            array[1] = new MazeCoordinate(coordinateCenter.x + 1, coordinateCenter.y); // right
            array[2] = new MazeCoordinate(coordinateCenter.x - 1, coordinateCenter.y); // left
            array[3] = new MazeCoordinate(coordinateCenter.x, coordinateCenter.y + 1); // down
            return array;
        }

        /// <summary>
        /// This method returns an array with 8 coordinates which
        /// represent all adjacent MazeCoordinates.
        /// </summary>
        public static MazeCoordinate[] GetAllAdjacentCoordinates(MazeCoordinate coordinateCenter)
        {
            return new MazeCoordinate[]{
                new MazeCoordinate(coordinateCenter.x, coordinateCenter.y - 1), // up
                new MazeCoordinate(coordinateCenter.x + 1, coordinateCenter.y), // right
                new MazeCoordinate(coordinateCenter.x - 1, coordinateCenter.y), // left
                new MazeCoordinate(coordinateCenter.x, coordinateCenter.y + 1), // down
                new MazeCoordinate(coordinateCenter.x + 1, coordinateCenter.y - 1), // up right
                new MazeCoordinate(coordinateCenter.x - 1, coordinateCenter.y - 1), // up left
                new MazeCoordinate(coordinateCenter.x + 1, coordinateCenter.y + 1), // down right
                new MazeCoordinate(coordinateCenter.x - 1, coordinateCenter.y + 1), // down left
        };
        }
    }
}