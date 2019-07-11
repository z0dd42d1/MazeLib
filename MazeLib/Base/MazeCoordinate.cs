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
    }
}