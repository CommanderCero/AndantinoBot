using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AndantinoBot;

namespace AndantinoGUI
{
    public delegate void RenderHexagonHandler(Graphics g, HexCoordinate c);
    public delegate void ClickHexagonHandler(HexCoordinate c);

    public class HexagonalGrid : Panel
    {
        public int Radius { get; } = 9;

        public event RenderHexagonHandler RenderHexagon;
        public event ClickHexagonHandler ClickHexagon;

        public HexCoordinate? HoveredHexagon { get; private set; }

        public Pen BorderPen { get; set; } = new Pen(Color.Black);
        public Font TextFont { get; set; } = new Font("Arial", 8);
        public StringFormat TextFormat { get; set; } = new StringFormat();

        // This class is actually overkill, as we only have pointy hexagons. But it doesn't hurt to make it a little bit more generic...
        private readonly HexagonGridLayout GridLayout;

        public HexagonalGrid()
        {
            // For faster drawing
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                        ControlStyles.UserPaint |
                        ControlStyles.AllPaintingInWmPaint, true);

            // Define our Hexagonal grid to consist of regular pointy hexagons
            GridLayout = new HexagonGridLayout(
                new HexagonOrientation(
                        Math.Sqrt(3), Math.Sqrt(3) / 2, 0, 3.0 / 2.0,
                        Math.Sqrt(3) / 3, -1.0 / 3.0, 0, 2.0 / 3.0,
                        0.5),
                new PointF(20, 20),
                new PointF(Width / 2, Height / 2)
            );

            ResizeRedraw = true;
            TextFormat.LineAlignment = StringAlignment.Center;
            TextFormat.Alignment = StringAlignment.Center;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            var mousePosition = e.Location;
            var newHoveredHexagon = GridLayout.PixelToHex(e.Location).Round();
            if (newHoveredHexagon.Length() > Radius)
            {
                HoveredHexagon = null;
                Invalidate();
            }
            else if (HoveredHexagon == null || !HoveredHexagon.Equals(newHoveredHexagon))
            {
                HoveredHexagon = newHoveredHexagon;
                Invalidate();
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            if(HoveredHexagon != null)
            {
                ClickHexagon?.Invoke((HexCoordinate)HoveredHexagon);
            }
        }

        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);
            GridLayout.Origin = new PointF(Width / 2, Height / 2);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            for (int q = -Radius; q <= Radius; q++)
            {
                int r1 = Math.Max(-Radius, -q - Radius);
                int r2 = Math.Min(Radius, -q + Radius);
                for (int r = r1; r <= r2; r++)
                {
                    var cord = new HexCoordinate(q, r);
                    PaintHexagon(e.Graphics, cord);
                }
            }

            // Print the coordinates of the hovered hexagon
            if(HoveredHexagon != null)
            {
                e.Graphics.DrawString($"Havannah Notation: {GetHannahNotationString(HoveredHexagon.Value)}", new Font("Artial", 12), new SolidBrush(Color.Black), 0, 0);
                e.Graphics.DrawString($"Coordinates: {HoveredHexagon.Value.ToString()}", new Font("Artial", 12), new SolidBrush(Color.Black), 0, 20);
            }
        }

        public void PaintHexagon(Graphics g, HexCoordinate cord)
        {
            var center = GridLayout.HexToPixel(cord);
            var corners = GridLayout.GetHexCorners(cord);

            // Restrict the drawing area only to the current hexagon
            var path = new GraphicsPath();
            path.AddPolygon(corners);
            g.SetClip(path);
            // Translate the graphics so we treat the center of the hexagon as coordinate (0,0)
            g.TranslateTransform(center.X, center.Y);

            // Fill the background
            g.FillRectangle(new SolidBrush(Color.LightGray), g.ClipBounds);

            // Let subscribers render the hexagon content
            RenderHexagon?.Invoke(g, cord);

            // Print the coordinates
            //g.DrawString($"{cord.Q}, {cord.R}", TextFont, TextBrush, 0, 0, TextFormat);

            // Remove the clipping and translation
            g.ResetClip();
            g.ResetTransform();

            // Draw the border
            // TODO While hovering there is still a black line visible
            g.DrawPolygon(BorderPen, GridLayout.GetHexCorners(cord));
        }

        public string GetHannahNotationString(HexCoordinate c)
        {
            var sLetter = char.ConvertFromUtf32(65 + (Radius - c.S));
            var rNumber = c.R + Radius + 1;

            return $"({rNumber}, {sLetter})";
        }
    }
}
