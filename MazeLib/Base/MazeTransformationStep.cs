using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable CA1051, CA1815

namespace MazeLib.Base
{
    public struct MazeTransformationStep : IEquatable<MazeTransformationStep>
    {
        public MazeFieldType typeAfterTransform;
        public MazeCoordinate coordinate;

        public override bool Equals(object obj)
        {
            if (obj is MazeTransformationStep)
            {
                var o = (MazeTransformationStep)obj;
                return
                    o.coordinate.Equals(this.coordinate) &&
                    o.typeAfterTransform == this.typeAfterTransform;
            }
            return false;
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash = (hash * 7) + coordinate.GetHashCode();
            hash = (hash * 7) + typeAfterTransform.GetHashCode();
            return hash;
        }

        public static bool operator ==(MazeTransformationStep left, MazeTransformationStep right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(MazeTransformationStep left, MazeTransformationStep right)
        {
            return !(left == right);
        }

        public bool Equals(MazeTransformationStep o)
        {
            return o.coordinate.Equals(this.coordinate) &&
                   o.typeAfterTransform == this.typeAfterTransform;
        }
    }
}