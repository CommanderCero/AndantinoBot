namespace ZobrisHashingTest
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
            System.Drawing.StringFormat stringFormat2 = new System.Drawing.StringFormat();
            this.hg_board_1 = new AndantinoGUI.HexagonalGrid();
            this.hg_board_2 = new AndantinoGUI.HexagonalGrid();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // hg_board_1
            // 
            this.hg_board_1.Location = new System.Drawing.Point(12, 12);
            this.hg_board_1.Name = "hg_board_1";
            this.hg_board_1.Radius = 9;
            this.hg_board_1.Size = new System.Drawing.Size(760, 593);
            this.hg_board_1.TabIndex = 0;
            this.hg_board_1.TextFont = new System.Drawing.Font("Arial", 8F);
            stringFormat1.Alignment = System.Drawing.StringAlignment.Center;
            stringFormat1.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;
            stringFormat1.LineAlignment = System.Drawing.StringAlignment.Center;
            stringFormat1.Trimming = System.Drawing.StringTrimming.Character;
            this.hg_board_1.TextFormat = stringFormat1;
            // 
            // hg_board_2
            // 
            this.hg_board_2.Location = new System.Drawing.Point(778, 12);
            this.hg_board_2.Name = "hg_board_2";
            this.hg_board_2.Radius = 9;
            this.hg_board_2.Size = new System.Drawing.Size(760, 593);
            this.hg_board_2.TabIndex = 1;
            this.hg_board_2.TextFont = new System.Drawing.Font("Arial", 8F);
            stringFormat2.Alignment = System.Drawing.StringAlignment.Center;
            stringFormat2.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;
            stringFormat2.LineAlignment = System.Drawing.StringAlignment.Center;
            stringFormat2.Trimming = System.Drawing.StringTrimming.Character;
            this.hg_board_2.TextFormat = stringFormat2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 627);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(139, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Search next error";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1492, 662);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.hg_board_2);
            this.Controls.Add(this.hg_board_1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private AndantinoGUI.HexagonalGrid hg_board_1;
        private AndantinoGUI.HexagonalGrid hg_board_2;
        private System.Windows.Forms.Button button1;
    }
}

