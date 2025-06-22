using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public class Trap:IGameObject
    {
        public PictureBox ObjectPictureBox { get; set; }
        public bool IsActive { get; set; }

        public Trap()
        {
            IsActive = true;
            ObjectPictureBox = new PictureBox
            {
                Size = new Size(91, 68),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
        }
        public void InitializeObject()
        {
            // Tuzakları başlatmak için kullanılan kod
            // Örneğin, resim ataması ve rastgele konum belirleme
            Image[] trapImages = {
                Properties.Resources.tuzakAlev,
                Properties.Resources.tuzakKapan,
                Properties.Resources.tuzakDiken
            };
            Random rnd = new Random();
            ObjectPictureBox.Image = trapImages[rnd.Next(trapImages.Length)];
            ObjectPictureBox.Location = GetRandomLocation();
        }
        public void ClearObject()
        {
            // Tuzakları temizlemek için kullanılan kod
            ObjectPictureBox.Visible = false;
            IsActive = false;
        }
        private Point GetRandomLocation()
        {
            // Rastgele konum üretme kodunuzu buraya ekleyin
            List<Point> availableLocations = TrapsLocations();
            Random rnd = new Random();
            int index = rnd.Next(availableLocations.Count);
            return availableLocations[index];
        }
        public static List<Point> TrapsLocations()
        {
            List<Point> gridLocations = new List<Point>();
            for (int x = 242; x <= 1309; x += 97)
            {
                for (int y = 166; y <= 314; y += 74)
                {
                    gridLocations.Add(new Point(x, y));
                }
            }
            return gridLocations;
            
        }
        public void UpdatePosition()
        {
            throw new NotImplementedException();
        }

    }
}
    


