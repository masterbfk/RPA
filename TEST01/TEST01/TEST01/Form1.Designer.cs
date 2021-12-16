namespace TEST01
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Glabel = new System.Windows.Forms.Label();
            this.Clabel = new System.Windows.Forms.Label();
            this.msglbl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Glabel
            // 
            this.Glabel.AutoSize = true;
            this.Glabel.Location = new System.Drawing.Point(12, 22);
            this.Glabel.Name = "Glabel";
            this.Glabel.Size = new System.Drawing.Size(57, 13);
            this.Glabel.TabIndex = 0;
            this.Glabel.Text = "Girdi dizini:";
            // 
            // Clabel
            // 
            this.Clabel.AutoSize = true;
            this.Clabel.Location = new System.Drawing.Point(12, 54);
            this.Clabel.Name = "Clabel";
            this.Clabel.Size = new System.Drawing.Size(56, 13);
            this.Clabel.TabIndex = 1;
            this.Clabel.Text = "Çıktı dizini:";
            // 
            // msglbl
            // 
            this.msglbl.AutoSize = true;
            this.msglbl.Location = new System.Drawing.Point(36, 90);
            this.msglbl.Name = "msglbl";
            this.msglbl.Size = new System.Drawing.Size(38, 13);
            this.msglbl.TabIndex = 2;
            this.msglbl.Text = "Mesaj:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(546, 122);
            this.Controls.Add(this.msglbl);
            this.Controls.Add(this.Clabel);
            this.Controls.Add(this.Glabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TEST01";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Glabel;
        private System.Windows.Forms.Label Clabel;
        private System.Windows.Forms.Label msglbl;
    }
}

