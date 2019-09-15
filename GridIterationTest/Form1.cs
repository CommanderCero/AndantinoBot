using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AndantinoBot;
using AndantinoGUI;

namespace GridIterationTest
{
    public partial class Form1 : Form
    {
        public Dictionary<HexCoordinate, int> iterationLookup = new Dictionary<HexCoordinate, int>();

        public Form1()
        {
            InitializeComponent();

            hg_board.RenderHexagon += Hg_board_RenderHexagon;

            c_iteration_type.SelectedItem = c_iteration_type.Items[0];
            QFirst();
        }

        private void Hg_board_RenderHexagon(Graphics g, HexCoordinate c)
        {
            if (!iterationLookup.ContainsKey(c))
                return;

            StringFormat format = new StringFormat();
            format.LineAlignment = StringAlignment.Center;
            format.Alignment = StringAlignment.Center;

            g.DrawString(iterationLookup[c].ToString(), new Font("Arial", 12), new SolidBrush(Color.Black), 0, 0, format);
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(c_iteration_type.SelectedItem.ToString())
            {
                case "Q-First": QFirst();break;
                case "R-First": RFirst();break;
                case "S-First": SFirst();break;
            }

            hg_board.Invalidate();
        }

        private void QFirst()
        {
            iterationLookup = new Dictionary<HexCoordinate, int>();

            int count = 0;
            for (var r = -9; r <= 9; r++)
            {
                var q_start = Math.Max(-9 - r, -9);
                var q_end = Math.Min(9 - r, 9);
                for (var q = q_start; q <= q_end; q++)
                {
                    count++;
                    iterationLookup.Add(new HexCoordinate(q, r), count);
                }
            }
        }

        private void RFirst()
        {
            iterationLookup = new Dictionary<HexCoordinate, int>();

            int count = 0;
            for (int q = -9; q <= 9; q++)
            {
                int r1 = Math.Max(-9, -q - 9);
                int r2 = Math.Min(9, -q + 9);
                for (int r = r1; r <= r2; r++)
                {
                    count++;
                    iterationLookup.Add(new HexCoordinate(q, r), count);
                }
            }
        }

        private void SFirst()
        {
            iterationLookup = new Dictionary<HexCoordinate, int>();

            int count = 0;
            for (int i = -9; i <= 9; i++)
            {
                var q_start = Math.Max(-9, i - 9);
                var r_start = Math.Min(9, i + 9);
                for(var x = 0; x <= r_start - q_start; x++)
                {
                    count++;
                    iterationLookup.Add(new HexCoordinate(q_start + x, r_start - x), count);
                }
            }
        }
    }
}
