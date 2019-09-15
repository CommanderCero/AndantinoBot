using AndantinoGUI;

namespace GridIterationTest
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
            this.hg_board = new AndantinoGUI.HexagonalGrid();
            this.c_iteration_type = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // hg_board
            // 
            this.hg_board.Location = new System.Drawing.Point(12, 12);
            this.hg_board.Name = "hg_board";
            this.hg_board.Radius = 9;
            this.hg_board.Size = new System.Drawing.Size(866, 594);
            this.hg_board.TabIndex = 0;
            this.hg_board.TextFont = new System.Drawing.Font("Arial", 8F);
            stringFormat1.Alignment = System.Drawing.StringAlignment.Center;
            stringFormat1.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;
            stringFormat1.LineAlignment = System.Drawing.StringAlignment.Center;
            stringFormat1.Trimming = System.Drawing.StringTrimming.Character;
            this.hg_board.TextFormat = stringFormat1;
            // 
            // c_iteration_type
            // 
            this.c_iteration_type.DisplayMember = "Q-First";
            this.c_iteration_type.FormattingEnabled = true;
            this.c_iteration_type.Items.AddRange(new object[] {
            "Q-First",
            "R-First",
            "S-First"});
            this.c_iteration_type.Location = new System.Drawing.Point(899, 12);
            this.c_iteration_type.Name = "c_iteration_type";
            this.c_iteration_type.Size = new System.Drawing.Size(121, 21);
            this.c_iteration_type.TabIndex = 1;
            this.c_iteration_type.SelectedIndexChanged += new System.EventHandler(this.ComboBox1_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1043, 631);
            this.Controls.Add(this.c_iteration_type);
            this.Controls.Add(this.hg_board);
            this.Name = "Form1";
            this.Text = "GridIterationTest";
            this.ResumeLayout(false);

        }

        #endregion

        private HexagonalGrid hg_board;
        private System.Windows.Forms.ComboBox c_iteration_type;
    }
}

