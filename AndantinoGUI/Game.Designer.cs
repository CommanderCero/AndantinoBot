namespace AndantinoGUI
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.Drawing.StringFormat stringFormat1 = new System.Drawing.StringFormat();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.l_active_player = new System.Windows.Forms.Label();
            this.l_winner = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.b_autoplay = new System.Windows.Forms.Button();
            this.b_next_move = new System.Windows.Forms.Button();
            this.b_undo = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cb_show_move_score = new System.Windows.Forms.CheckBox();
            this.hg_board = new AndantinoGUI.HexagonalGrid();
            this.lb_black_chains = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lb_white_chains = new System.Windows.Forms.ListBox();
            this.flowLayoutPanel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.groupBox1);
            this.flowLayoutPanel2.Controls.Add(this.groupBox2);
            this.flowLayoutPanel2.Controls.Add(this.groupBox3);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(1066, 12);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(306, 637);
            this.flowLayoutPanel2.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.l_active_player);
            this.groupBox1.Controls.Add(this.l_winner);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(10);
            this.groupBox1.Size = new System.Drawing.Size(293, 82);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Informations";
            // 
            // l_active_player
            // 
            this.l_active_player.AutoSize = true;
            this.l_active_player.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.l_active_player.Location = new System.Drawing.Point(145, 23);
            this.l_active_player.Name = "l_active_player";
            this.l_active_player.Size = new System.Drawing.Size(52, 20);
            this.l_active_player.TabIndex = 4;
            this.l_active_player.Text = "White";
            // 
            // l_winner
            // 
            this.l_winner.AutoSize = true;
            this.l_winner.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.l_winner.Location = new System.Drawing.Point(93, 52);
            this.l_winner.Name = "l_winner";
            this.l_winner.Size = new System.Drawing.Size(48, 20);
            this.l_winner.TabIndex = 3;
            this.l_winner.Text = "None";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(13, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Winner:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(13, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Active Player:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.b_autoplay);
            this.groupBox2.Controls.Add(this.b_next_move);
            this.groupBox2.Controls.Add(this.b_undo);
            this.groupBox2.Location = new System.Drawing.Point(3, 91);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(293, 108);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Actions";
            // 
            // b_autoplay
            // 
            this.b_autoplay.Location = new System.Drawing.Point(166, 61);
            this.b_autoplay.Name = "b_autoplay";
            this.b_autoplay.Size = new System.Drawing.Size(121, 36);
            this.b_autoplay.TabIndex = 2;
            this.b_autoplay.Text = "Autoplay";
            this.b_autoplay.UseVisualStyleBackColor = true;
            this.b_autoplay.Click += new System.EventHandler(this.OnAutoplayClick);
            // 
            // b_next_move
            // 
            this.b_next_move.Location = new System.Drawing.Point(166, 19);
            this.b_next_move.Name = "b_next_move";
            this.b_next_move.Size = new System.Drawing.Size(121, 36);
            this.b_next_move.TabIndex = 1;
            this.b_next_move.Text = "Next Move";
            this.b_next_move.UseVisualStyleBackColor = true;
            this.b_next_move.Click += new System.EventHandler(this.OnNextMoveClick);
            // 
            // b_undo
            // 
            this.b_undo.Enabled = false;
            this.b_undo.Location = new System.Drawing.Point(6, 19);
            this.b_undo.Name = "b_undo";
            this.b_undo.Size = new System.Drawing.Size(121, 36);
            this.b_undo.TabIndex = 0;
            this.b_undo.Text = "Undo";
            this.b_undo.UseVisualStyleBackColor = true;
            this.b_undo.Click += new System.EventHandler(this.OnUndoClick);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cb_show_move_score);
            this.groupBox3.Location = new System.Drawing.Point(3, 205);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(293, 100);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Debugging";
            // 
            // cb_show_move_score
            // 
            this.cb_show_move_score.AutoSize = true;
            this.cb_show_move_score.Location = new System.Drawing.Point(7, 19);
            this.cb_show_move_score.Name = "cb_show_move_score";
            this.cb_show_move_score.Size = new System.Drawing.Size(170, 17);
            this.cb_show_move_score.TabIndex = 0;
            this.cb_show_move_score.Text = "Show Move-Heuristic Ranking";
            this.cb_show_move_score.UseVisualStyleBackColor = true;
            // 
            // hg_board
            // 
            this.hg_board.Location = new System.Drawing.Point(321, 12);
            this.hg_board.Name = "hg_board";
            this.hg_board.Radius = 9;
            this.hg_board.Size = new System.Drawing.Size(739, 637);
            this.hg_board.TabIndex = 1;
            this.hg_board.TextFont = new System.Drawing.Font("Arial", 8F);
            stringFormat1.Alignment = System.Drawing.StringAlignment.Center;
            stringFormat1.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;
            stringFormat1.LineAlignment = System.Drawing.StringAlignment.Center;
            stringFormat1.Trimming = System.Drawing.StringTrimming.Character;
            this.hg_board.TextFormat = stringFormat1;
            // 
            // lb_black_chains
            // 
            this.lb_black_chains.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_black_chains.FormattingEnabled = true;
            this.lb_black_chains.ItemHeight = 20;
            this.lb_black_chains.Location = new System.Drawing.Point(12, 39);
            this.lb_black_chains.Name = "lb_black_chains";
            this.lb_black_chains.Size = new System.Drawing.Size(303, 284);
            this.lb_black_chains.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(130, 24);
            this.label3.TabIndex = 3;
            this.label3.Text = "Black Chains";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 332);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(133, 24);
            this.label4.TabIndex = 5;
            this.label4.Text = "White Chains";
            // 
            // lb_white_chains
            // 
            this.lb_white_chains.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_white_chains.FormattingEnabled = true;
            this.lb_white_chains.ItemHeight = 20;
            this.lb_white_chains.Location = new System.Drawing.Point(12, 359);
            this.lb_white_chains.Name = "lb_white_chains";
            this.lb_white_chains.Size = new System.Drawing.Size(303, 284);
            this.lb_white_chains.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1384, 661);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lb_white_chains);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lb_black_chains);
            this.Controls.Add(this.hg_board);
            this.Controls.Add(this.flowLayoutPanel2);
            this.Name = "Form1";
            this.Text = "Andantino";
            this.flowLayoutPanel2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label l_active_player;
        private System.Windows.Forms.Label l_winner;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button b_autoplay;
        private System.Windows.Forms.Button b_next_move;
        private System.Windows.Forms.Button b_undo;
        private HexagonalGrid hg_board;
        private System.Windows.Forms.ListBox lb_black_chains;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox lb_white_chains;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox cb_show_move_score;
    }
}

