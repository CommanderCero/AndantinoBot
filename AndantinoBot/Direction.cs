using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndantinoBot
{
    public enum Direction
    {
        Q,
        R,
        S
    }

    public static class DirectionExtensions
    {
        public static HexCoordinate ToHex(this Direction d)
        {
            switch(d)
            {
                case Direction.Q: return HexCoordinate.East;
                case Direction.R: return HexCoordinate.SouthEast;
                case Direction.S: return HexCoordinate.NorthEast;
            }

            throw new Exception("Unknown Direction");
        }
    }
}
