using AndantinoBot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndantinoGUI
{
    // Most of the code in this class was taken from https://www.redblobgames.com/grids/hexagons/ and https://www.redblobgames.com/grids/hexagons/implementation.html#pixel-to-hex
    public class HexagonGridLayout
    {
        public HexagonOrientation Orientation { get; set; }

        /// <summary>
        /// The length of the 6 sides. You can specify x and y to squash or stretch the hexagon.
        /// This is NOT the width or height of the hexagon, unless the starting angle of the orientation is a multiple of 60 degrees
        /// </summary>
        public PointF SideLength { get; set; }

        /// <summary>
        /// The center of the hexagon with q = 0, r = 0 and s = 0
        /// </summary>
        public PointF Origin { get; set; }

        public HexagonGridLayout(HexagonOrientation orientation, PointF sideLength, PointF origin)
        {
            Orientation = orientation;
            SideLength = sideLength;
            Origin = origin;
        }

        public PointF HexToPixel(HexCoordinate h)
        {
            // For more readable code we create a alias
            var o = Orientation;

            var x = (o.F0 * h.Q + o.F1 * h.R) * SideLength.X;
            var y = (o.F2 * h.Q + o.F3 * h.R) * SideLength.Y;

            return new PointF((float)x + Origin.X, (float)y + Origin.Y);
        }

        public FractionalHex PixelToHex(PointF p)
        {
            // For more readable code we create a alias
            var o = Orientation;

            var x = (p.X - Origin.X) / SideLength.X;
            var y = (p.Y - Origin.Y) / SideLength.Y;

            var q = o.B0 * x + o.B1 * y;
            var r = o.B2 * x + o.B3 * y;

            return new FractionalHex(q, r);
        }

        public PointF HexCornerOffset(int corner)
        {
            var angle = 2.0 * Math.PI * (Orientation.StartAngle + corner) / 6;
            return new PointF(SideLength.X * (float)Math.Cos(angle), SideLength.Y * (float)Math.Sin(angle));
        }

        public PointF[] GetHexCorners(HexCoordinate cord)
        {
            var corners = new PointF[6];
            var center = HexToPixel(cord);
            for(var i = 0; i < corners.Length; i++)
            {
                var offset = HexCornerOffset(i);
                corners[i] = new PointF(center.X + offset.X, center.Y + offset.Y);
            }

            return corners;
        }
    }
}
