using AndantinoBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndantinoGUI
{
    public class FractionalHex
    {
        public double Q { get; }
        public double R { get; }
        public double S => -Q - R;

        public FractionalHex(double q, double r)
        {
            Q = q;
            R = r;
        }

        public HexCoordinate Round()
        {
            var q = (int)Math.Round(Q);
            var r = (int)Math.Round(R);
            var s = (int)Math.Round(S);

            var q_diff = Math.Abs(q - Q);
            var r_diff = Math.Abs(r - R);
            var s_diff = Math.Abs(s - S);
            if (q_diff > r_diff && q_diff > s_diff)
            {
                q = -r - s;
            }
            else if (r_diff > s_diff)
            {
                r = -q - s;
            }

            return new HexCoordinate(q, r);
        }
    }
}
