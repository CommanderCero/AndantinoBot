using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndantinoBot
{
    // TODO For Performance reasons: https://docs.microsoft.com/de-de/dotnet/csharp/write-safe-efficient-code Reduce size to less than 4 Bytes or pass this class only with the in argument and ref return
    public readonly struct HexCoordinate : IEquatable<HexCoordinate>
    {
        public int Q { get; } // west and east axis
        public int R { get; } // northwest and southeast axis
        public int S => -Q - R; // northeast and southwest axis

        public HexCoordinate(int q, int r)
        {
            Q = q;
            R = r;
        }

        public int Distance(HexCoordinate o)
        {
            return (Math.Abs(Q - o.Q) + Math.Abs(R - o.R) + Math.Abs(S - o.S)) / 2;
        }

        public int Length()
        {
            return Distance(new HexCoordinate(0, 0));
        }

        public IEnumerable<HexCoordinate> GetNeighbors()
        {
            var neighbors = new HexCoordinate[HexDirections.Length];
            for(var i = 0; i < HexDirections.Length; i++)
            {
                neighbors[i] = HexDirections[i] + this;
            }

            return neighbors;
        }

        public override int GetHashCode()
        {
            return Q ^ (R << 8);
        }

        public override string ToString()
        {
            return $"({Q},{R})";
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            var other = (HexCoordinate)obj;
            return Equals(other);
        }

        public bool Equals(HexCoordinate other)
        {
            return other.Q == Q && other.R == R;
        }

        #region Operators
        public static HexCoordinate operator +(HexCoordinate left, HexCoordinate right)
        {
            return new HexCoordinate(left.Q + right.Q, left.R + right.R);
        }

        public static HexCoordinate operator -(HexCoordinate left, HexCoordinate right)
        {
            return new HexCoordinate(left.Q - right.Q, left.R - right.R);
        }

        public static HexCoordinate operator *(HexCoordinate left, int scalar)
        {
            return new HexCoordinate(left.Q * scalar, left.R * scalar);
        }
        #endregion

        #region Static Defines
        public static HexCoordinate West { get; } = new HexCoordinate(-1, 0);
        public static HexCoordinate NorthWest { get; } = new HexCoordinate(0, -1);
        public static HexCoordinate NorthEast { get; } = new HexCoordinate(1, -1);
        public static HexCoordinate East { get; } = new HexCoordinate(1, 0);
        public static HexCoordinate SouthEast { get; } = new HexCoordinate(0, 1);
        public static HexCoordinate SouthWest { get; } = new HexCoordinate(-1, 1);

        public static HexCoordinate[] HexDirections = new HexCoordinate[]
        {
            West, NorthWest, NorthEast,
            East, SouthEast, SouthWest
        };
        #endregion
    }
}
