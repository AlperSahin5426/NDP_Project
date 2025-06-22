namespace WindowsFormsApp1
{
    partial class FrmMainMenu
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
            this.btnStart = new System.Windows.Forms.Button();
            this.btnViewScoreBoard = new System.Windows.Forms.Button();
            this.lblKeyInfo = new System.Windows.Forms.Label();
            this.lblAd = new System.Windows.Forms.Label();
            this.txtOyuncuAdi = new System.Windows.Forms.TextBox();
            this.lstViewRates = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.SpringGreen;
            this.btnStart.Location = new System.Drawing.Point(316, 138);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(218, 44);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Oyunu Başlat";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnViewScoreBoard
            // 
            this.btnViewScoreBoard.BackColor = System.Drawing.Color.Turquoise;
            this.btnViewScoreBoard.Location = new System.Drawing.Point(316, 204);
            this.btnViewScoreBoard.Name = "btnViewScoreBoard";
            this.btnViewScoreBoard.Size = new System.Drawing.Size(218, 44);
            this.btnViewScoreBoard.TabIndex = 2;
            this.btnViewScoreBoard.Text = "En İyi 5 Skoru Görüntüle";
            this.btnViewScoreBoard.UseVisualStyleBackColor = false;
            this.btnViewScoreBoard.Click += new System.EventHandler(this.btnViewScoreBoard_Click);
            // 
            // lblKeyInfo
            // 
            this.lblKeyInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblKeyInfo.Location = new System.Drawing.Point(582, 33);
            this.lblKeyInfo.Name = "lblKeyInfo";
            this.lblKeyInfo.Size = new System.Drawing.Size(197, 113);
            this.lblKeyInfo.TabIndex = 3;
            // 
            // lblAd
            // 
            this.lblAd.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblAd.Location = new System.Drawing.Point(105, 80);
            this.lblAd.Name = "lblAd";
            this.lblAd.Size = new System.Drawing.Size(205, 34);
            this.lblAd.TabIndex = 4;
            this.lblAd.Text = "Oyuncu Adı :";
            // 
            // txtOyuncuAdi
            // 
            this.txtOyuncuAdi.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtOyuncuAdi.Location = new System.Drawing.Point(316, 80);
            this.txtOyuncuAdi.Name = "txtOyuncuAdi";
            this.txtOyuncuAdi.Size = new System.Drawing.Size(218, 38);
            this.txtOyuncuAdi.TabIndex = 5;
            // 
            // lstViewRates
            // 
            this.lstViewRates.BackColor = System.Drawing.Color.OldLace;
            this.lstViewRates.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lstViewRates.FormattingEnabled = true;
            this.lstViewRates.ItemHeight = 24;
            this.lstViewRates.Location = new System.Drawing.Point(112, 273);
            this.lstViewRates.Name = "lstViewRates";
            this.lstViewRates.Size = new System.Drawing.Size(635, 196);
            this.lstViewRates.TabIndex = 6;
            this.lstViewRates.Visible = false;
            // 
            // FrmMainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Khaki;
            this.ClientSize = new System.Drawing.Size(816, 504);
            this.Controls.Add(this.lstViewRates);
            this.Controls.Add(this.txtOyuncuAdi);
            this.Controls.Add(this.lblAd);
            this.Controls.Add(this.lblKeyInfo);
            this.Controls.Add(this.btnViewScoreBoard);
            this.Controls.Add(this.btnStart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(791, 456);
            this.Name = "FrmMainMenu";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ana Menü";
            this.Enter += new System.EventHandler(this.btnStart_Click);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnViewScoreBoard;
        private System.Windows.Forms.Label lblKeyInfo;
        private System.Windows.Forms.Label lblAd;
        private System.Windows.Forms.TextBox txtOyuncuAdi;
        private System.Windows.Forms.ListBox lstViewRates;
    }
}

