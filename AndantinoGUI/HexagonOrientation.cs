using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndantinoGUI
{
    public class HexagonOrientation
    {
        // F0 to F3 are elements of a matrix used for converting from axial coordinates to pixel positions
        // I have no idea how it works, but for now I can ignore this fact as I just need to copy some code into the HexagonGridLayout class without understanding the math
        public double F0 { get; set; }
        public double F1 { get; set; }
        public double F2 { get; set; }
        public double F3 { get; set; }

        // F0 to F3 are elements of a matrix used for converting from pixel positions to axial coordinates
        // This is the inverse of the F-Matrix
        public double B0 { get; set; }
        public double B1 { get; set; }
        public double B2 { get; set; }
        public double B3 { get; set; }

        public double StartAngle { get; set; }

        public HexagonOrientation(double f0, double f1, double f2, double f3, double b0, double b1, double b2, double b3, double startAngle)
        {
            F0 = f0;
            F1 = f1;
            F2 = f2;
            F3 = f3;
            B0 = b0;
            B1 = b1;
            B2 = b2;
            B3 = b3;
            StartAngle = startAngle;
        }
    }
}
