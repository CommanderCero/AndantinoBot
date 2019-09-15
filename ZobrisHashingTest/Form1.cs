using AndantinoBot;
using AndantinoBot.Game;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZobrisHashingTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            currentMap = new Player[Andantino.BOARD_WIDTH, Andantino.BOARD_WIDTH];

            hg_board_1.RenderHexagon += Hg_board_RenderHexagon;
            hg_board_2.RenderHexagon += Hg_board_RenderHexagon2;
        }

        private void Hg_board_RenderHexagon(Graphics g, HexCoordinate c)
        {
            if (ErrorMap1 == null)
                return;

            if (ErrorMap1[c.R + Andantino.BOARD_RADIUS, c.Q + Andantino.BOARD_RADIUS] != Player.None)
                g.FillRectangle(new SolidBrush(ErrorMap1[c.R + Andantino.BOARD_RADIUS, c.Q + Andantino.BOARD_RADIUS] == Player.Black ? Color.Black : Color.White), g.ClipBounds);
        }

        private void Hg_board_RenderHexagon2(Graphics g, HexCoordinate c)
        {
            if (ErrorMap2 == null)
                return;

            if (ErrorMap2[c.R + Andantino.BOARD_RADIUS, c.Q + Andantino.BOARD_RADIUS] != Player.None)
                g.FillRectangle(new SolidBrush(ErrorMap2[c.R + Andantino.BOARD_RADIUS, c.Q + Andantino.BOARD_RADIUS] == Player.Black ? Color.Black : Color.White), g.ClipBounds);
        }

        private Dictionary<long, Player[,]> errorMaps = new Dictionary<long, Player[,]>();
        private HashSet<long> occupiedHashcodes = new HashSet<long>();
        private Dictionary<long, int> errorCount = new Dictionary<long, int>();
        public Player[,] currentMap;
        public long currentHashCode;
        public Player[,] ErrorMap1;
        public Player[,] ErrorMap2;

        private void Button1_Click(object sender, EventArgs e)
        {
            while (true)
            {
                int r = -9;
                int q = Math.Max(-9 - r, -9);
                var carryOver = true;
                while (carryOver)
                {
                    carryOver = false;
                    var curR = r + Andantino.BOARD_RADIUS;
                    var curQ = q + Andantino.BOARD_RADIUS;
                    if(currentMap[curR, curQ] != Player.None)
                        currentHashCode ^= Andantino.randomNumbers[currentMap[curR, curQ]][curR, curQ];
                    if (currentMap[curR, curQ] == Player.None)
                    {
                        currentMap[curR, curQ] = Player.White;
                        currentHashCode ^= Andantino.randomNumbers[Player.White][curR, curQ];
                    }
                    else if (currentMap[curR, curQ] == Player.White)
                    {
                        currentMap[curR, curQ] = Player.Black;
                        currentHashCode ^= Andantino.randomNumbers[Player.Black][curR, curQ];
                    }
                    else
                    {
                        currentMap[curR, curQ] = Player.None;
                        carryOver = true;
                    }

                    q++;
                    if(q > Math.Min(9 - r, 9))
                    {
                        r++;
                        q = Math.Max(-9 - r, -9);
                    }
                }
                if(!errorCount.ContainsKey(currentHashCode % 20000000))
                {
                    errorCount.Add(currentHashCode % 20000000, 0);
                }
                errorCount[currentHashCode % 20000000] += 1;
                if(errorCount[currentHashCode % 20000000] >= 4)
                {
                    MessageBox.Show($"{currentHashCode}: {currentHashCode % 20000000} {errorCount[currentHashCode % 20000000]}");
                }
                if(occupiedHashcodes.Contains(currentHashCode % 20000000))
                {
                    if (errorMaps.ContainsKey(currentHashCode % 20000000))
                    {
                        ErrorMap1 = errorMaps[currentHashCode % 20000000];
                        ErrorMap2 = currentMap;
                        break;
                    }
                    else
                    {
                        errorMaps.Add(currentHashCode % 20000000, CopyMap(currentMap));
                    }
                }
                else
                {
                    occupiedHashcodes.Add(currentHashCode % 20000000);
                }
            }

            hg_board_1.Invalidate();
            hg_board_2.Invalidate();
        }

        private Player[,] CopyMap(Player[,] game)
        {
            var copy = new Player[Andantino.BOARD_WIDTH, Andantino.BOARD_WIDTH];
            Array.Copy(game, 0, copy, 0, copy.Length);

            return copy;
        }
    }
}
