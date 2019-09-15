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
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.hg_board = new AndantinoGUI.HexagonalGrid();
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
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.hg_board);
            this.flowLayoutPanel1.Controls.Add(this.flowLayoutPanel2);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1160, 637);
            this.flowLayoutPanel1.TabIndex = 0;
            this.flowLayoutPanel1.WrapContents = false;
            // 
            // hg_board
            // 
            this.hg_board.Location = new System.Drawing.Point(3, 3);
            this.hg_board.Name = "hg_board";
            this.hg_board.Radius = 9;
            this.hg_board.Size = new System.Drawing.Size(739, 614);
            this.hg_board.TabIndex = 1;
            this.hg_board.TextFont = new System.Drawing.Font("Arial", 8F);
            stringFormat1.Alignment = System.Drawing.StringAlignment.Center;
            stringFormat1.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;
            stringFormat1.LineAlignment = System.Drawing.StringAlignment.Center;
            stringFormat1.Trimming = System.Drawing.StringTrimming.Character;
            this.hg_board.TextFormat = stringFormat1;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.groupBox1);
            this.flowLayoutPanel2.Controls.Add(this.groupBox2);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(748, 3);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(357, 614);
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
            this.groupBox1.Size = new System.Drawing.Size(342, 82);
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
            this.groupBox2.Size = new System.Drawing.Size(342, 134);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Actions";
            // 
            // b_autoplay
            // 
            this.b_autoplay.Location = new System.Drawing.Point(215, 61);
            this.b_autoplay.Name = "b_autoplay";
            this.b_autoplay.Size = new System.Drawing.Size(121, 36);
            this.b_autoplay.TabIndex = 2;
            this.b_autoplay.Text = "Autoplay";
            this.b_autoplay.UseVisualStyleBackColor = true;
            this.b_autoplay.Click += new System.EventHandler(this.OnAutoplayClick);
            // 
            // b_next_move
            // 
            this.b_next_move.Location = new System.Drawing.Point(215, 19);
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 661);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "Form1";
            this.Text = "Andantino";
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private HexagonalGrid hg_board;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label l_winner;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label l_active_player;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button b_undo;
        private System.Windows.Forms.Button b_autoplay;
        private System.Windows.Forms.Button b_next_move;
    }
}

