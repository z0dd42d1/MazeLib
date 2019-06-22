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
    }
}