using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public class Soldier
    {
        // Asker nesnesinin PictureBox'ı ve aktiflik durumu
        public PictureBox SoldierPictureBox { get; set; }
        public bool IsActive { get; set; }
        private List<PictureBox> enemySoldiers = new List<PictureBox>();

        public Soldier()
        {
            // Soldier sınıfı başlatıldığında varsayılan ayarlar
            IsActive = true; // Asker varsayılan olarak aktif
            SoldierPictureBox = new PictureBox
            {
                Size = new Size(91, 68), // PictureBox boyutunu ayarlama
                SizeMode = PictureBoxSizeMode.StretchImage // Resmin uygun şekilde gerilmesini sağlama
            };
        }
       
    }

}
