using AndantinoBot;
using AndantinoBot.Agent;
using AndantinoBot.Game;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AndantinoGUI
{
    public partial class Form1 : Form
    {
        public Andantino Game { get; set; }
        public int StoneSize = 16;

        public AndantinoAgent Agent { get; set; }
        public HexCoordinate? NextPlay { get; set; }
        public int HoveredChainIndex { get; set; }
        public ChainRowCollection HoveredChainRow { get; set; }

        public Stack<long> ElapsedTimeHistory { get; }
        public Stopwatch AgentStopwatch { get; }

        public Form1()
        {
            InitializeComponent();
            hg_board.RenderHexagon += OnRenderHexagon;
            hg_board.ClickHexagon += OnClickHexagon;

            Game = new Andantino();
            Agent = new AndantinoAgent(new TestHeuristic());

            lb_black_chains.MouseMove += (sender, args) => OnHoverChain(sender, args, Game.BlackChains);
            lb_white_chains.MouseMove += (sender, args) => OnHoverChain(sender, args, Game.WhiteChains);
            lb_black_chains.MouseLeave += (sender, args) => OnHoverChain(sender, args, Game.BlackChains);
            lb_white_chains.MouseLeave += (sender, args) => OnHoverChain(sender, args, Game.WhiteChains);

            ElapsedTimeHistory = new Stack<long>();
            AgentStopwatch = new Stopwatch();
        }

        private void OnClickHexagon(HexCoordinate c)
        {
            if (Game.IsGameOver)
            {
                return;
            }

            if (Game.GetValidPlacements().Contains(c))
            {
                Game.PlaceStone(c);
                NextPlay = null;

                UpdateRender();
            }

            (new TestHeuristic()).Evaluate(Game);
        }

        private void OnUndoClick(object sender, EventArgs e)
        {
            NextPlay = null;

            if (!Game.CanUndo)
                return;

            Game.UndoLastMove();
            ElapsedTimeHistory.Pop();
            UpdateRender();
        }

        private void OnNextMoveClick(object sender, EventArgs e)
        {
            NextPlay = Agent.GetNextPlay(Game);
            UpdateRender();
        }

        private void OnAutoplayClick(object sender, EventArgs e)
        {
            Game.PlaceStone(NextPlay ?? Agent.GetNextPlay(Game));
            NextPlay = null;
            UpdateRender();
        }

        private void OnRenderHexagon(Graphics g, HexCoordinate c)
        {
            var backgroundColor = Color.LightGray;
            if (HoveredChainRow != null)
            {
                if (HoveredChainRow.Contains(HoveredChainIndex, c))
                {
                    backgroundColor = Color.Magenta;
                }
                else if (HoveredChainRow.GetChainStartExtension(HoveredChainIndex, out var q, out var r) && q == c.Q && r == c.R)
                {
                    backgroundColor = Color.DarkMagenta;
                }
                else if (HoveredChainRow.GetChainEndExtension(HoveredChainIndex, out q, out r) && q == c.Q && r == c.R)
                {
                    backgroundColor = Color.DarkMagenta;
                }
            }

            if (backgroundColor == Color.LightGray)
            {
                if (NextPlay != null && c.Equals(NextPlay)) // Render background for the move that was selected by the agent
                {
                    backgroundColor = Color.Orange;
                }
                else if (Game.GetValidPlacements().Contains(c)) // Render background for valid moves
                {
                    backgroundColor = Color.DarkGray;
                }
            }

            // Render Background
            g.FillRectangle(new SolidBrush(backgroundColor), g.ClipBounds);

            if (Game[c] != Player.None) // Render placed stones
            {
                var stoneColor = Game[c] == Player.Black ? Color.Black : Color.White;
                g.FillEllipse(new SolidBrush(stoneColor), -StoneSize / 2, -StoneSize / 2, StoneSize, StoneSize);
            }
            else if (hg_board.HoveredHexagon.Equals(c)) // Render fake stone for hovering
            {
                var pen = new Pen(Game.ActivePlayer == Player.Black ? Color.Black : Color.White, 2);
                pen.DashStyle = DashStyle.Dash;
                g.DrawEllipse(pen, -StoneSize / 2, -StoneSize / 2, StoneSize, StoneSize);
            }

            if(NextPlay != null && c.Equals(NextPlay) && !c.Equals(hg_board.HoveredHexagon))
            {
                g.DrawString(hg_board.GetHannahNotationString(c), new Font("Arial", 10, FontStyle.Bold), new SolidBrush(Color.Black), Point.Empty, hg_board.TextFormat);
            }
        }

        private void OnHoverChain(object sender, EventArgs e, ChainCollection collection)
        {
            var listBox = (ListBox)sender;

            var hoverPoint = listBox.PointToClient(Cursor.Position);
            int index = listBox.IndexFromPoint(hoverPoint);
            if (index < 0)
            {
                HoveredChainRow = null;
                return;
            }

            foreach (var chainRow in collection.AllChains)
            {
                if(index < chainRow.ChainCount)
                {
                    HoveredChainRow = chainRow;
                    HoveredChainIndex = index;
                    break;
                }

                index -= chainRow.ChainCount;
            }

            UpdateRender(false);
        }

        private void UpdateRender(bool refillChains = true)
        {
            // Redraw the hexagon grid
            hg_board.Invalidate();

            // Update active player and winner text
            l_active_player.Text = Game.ActivePlayer.ToString();
            l_winner.Text = Game.Winner.ToString();

            // Enable\Disable undo button
            b_undo.Enabled = Game.CanUndo;

            // Display chains
            if(refillChains)
            {
                FillChainListBox(lb_black_chains, Game.BlackChains);
                FillChainListBox(lb_white_chains, Game.WhiteChains);
            }
        }

        private void FillChainListBox(ListBox listBox, ChainCollection collection)
        {
            listBox.Items.Clear();

            foreach(var chainRow in collection.AllChains)
            {
                for(var i = 0; i < chainRow.ChainCount; i++)
                {
                    chainRow.GetChainStart(i, out var qStart, out var rStart);
                    chainRow.GetChainEnd(i, out var qEnd, out var rEnd);

                    listBox.Items.Add($"{chainRow.Direction}: ({qStart}, {rStart}) to ({qEnd}, {rEnd})");
                }
            }
        }
    }
}
