using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class FrmMainMenu : Form
    {
        private ScoreLoader scoreLoader = new ScoreLoader();
        public FrmMainMenu()
        {
            InitializeComponent();
            // Ana menü için klavye bilgilerini etikette gösterme
            lblKeyInfo.Location = new Point(this.Width - lblKeyInfo.Width - 10, 10);
            lblKeyInfo.Text = "W, A, S, D: Hareket\n  P: Duraklat";
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            // "Başlat" butonuna tıklanınca oyun formunun açılması
            if (!IsValidPlayerName(txtOyuncuAdi.Text))
            {
                MessageBox.Show("Oyuncu Adı Girilmesi Zorunludur!");
                return;
            }

            GameForm frm = new GameForm
            {
                PlayerName = txtOyuncuAdi.Text // Oyuncu adını GameForm'a aktarma
            };
            frm.Show();
            this.Hide(); // Ana menü formunu gizleme
        }

        private bool IsValidPlayerName(string playerName)
        {
            // Oyuncu adının geçerli olup olmadığını kontrol etme
            return !String.IsNullOrWhiteSpace(playerName);
        }

        private void btnViewScoreBoard_Click(object sender, EventArgs e)
        {
            // "Skor Tablosunu Görüntüle" butonu işlevi
            string filePath = "skor.txt"; // Skor dosyasının yolu
            var topScores = scoreLoader.LoadTopScores(filePath); // Skorları yükleme
            lstViewRates.Items.Clear(); // Listeyi temizleme
            lstViewRates.Visible = true; // Skor listesini görünür yapma

            foreach (var score in topScores)
            {
                // Skorları listeye ekleme
                lstViewRates.Items.Add($"{score.PlayerName} - {score.PlayerScore}");
            }
        }
    }
}
