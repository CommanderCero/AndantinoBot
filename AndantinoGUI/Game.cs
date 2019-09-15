using AndantinoBot;
using AndantinoBot.Agent;
using AndantinoBot.Game;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

        public Form1()
        {
            InitializeComponent();
            hg_board.RenderHexagon += OnRenderHexagon;
            hg_board.ClickHexagon += OnClickHexagon;

            Game = new Andantino();
            Agent = new AndantinoAgent(new TestHeuristic());
        }

        private void OnClickHexagon(HexCoordinate c)
        {
            if (Game.IsGameOver)
            {
                return;
            }

            if(Game.GetValidPlacements().Contains(c))
            {
                Game.PlaceStone(c);
                NextPlay = null;

                l_active_player.Text = Game.ActivePlayer.ToString();
                l_winner.Text = Game.Winner.ToString();

                hg_board.Invalidate();
            }

            b_undo.Enabled = true;
        }

        private void OnUndoClick(object sender, EventArgs e)
        {
            NextPlay = null;

            if (!Game.CanUndo)
                return;

            Game.UndoLastMove();
            b_undo.Enabled = Game.CanUndo;
            UpdateRender();
        }

        private void OnRenderHexagon(Graphics g, HexCoordinate c)
        {
            // Render background for valid moves
            if(NextPlay != null && c.Equals(NextPlay))
            {
                g.FillRectangle(new SolidBrush(Color.Orange), g.ClipBounds);
            }
            else if(Game.GetValidPlacements().Contains(c))
            {
                g.FillRectangle(new SolidBrush(Color.DarkGray), g.ClipBounds);
            }

            if (Game[c] != Player.None)
            {
                var stoneColor = Game[c] == Player.Black ? Color.Black : Color.White;
                g.FillEllipse(new SolidBrush(stoneColor), -StoneSize / 2, -StoneSize / 2, StoneSize, StoneSize);
            }
            else if(hg_board.HoveredHexagon.Equals(c))
            {
                var pen = new Pen(Game.ActivePlayer == Player.Black ? Color.Black : Color.White, 2);
                pen.DashStyle = DashStyle.Dash;
                g.DrawEllipse(pen, -StoneSize / 2, -StoneSize / 2, StoneSize, StoneSize);
            }
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
            b_undo.Enabled = true;
            UpdateRender();
        }

        private void UpdateRender()
        {
            hg_board.Invalidate();

            l_active_player.Text = Game.ActivePlayer.ToString();
            l_winner.Text = Game.Winner.ToString();
        }
    }
}
