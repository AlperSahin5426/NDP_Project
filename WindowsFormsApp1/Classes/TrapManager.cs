using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public class TrapManager
    {
        // Tuzakların saklandığı liste
        private List<Trap> traps = new List<Trap>();
        // Kullanılmış konumları saklayan HashSet
        private HashSet<Point> usedLocations = new HashSet<Point>();
        private Random rnd = new Random();

        // Tuzaklara dışarıdan erişim sağlamak için özellik
        public List<Trap> Traps => traps;


        // Oyun başlangıcında tuzakları başlatma
        public void InitializeTraps()
        {
            // Tuzak resimleri
            Image[] trapImages = {
                Properties.Resources.tuzakAlev,
                Properties.Resources.tuzakKapan,
                Properties.Resources.tuzakDiken
            };

            // Tuzakları rastgele konumlara yerleştirme
            for (int i = 0; i < 10; i++)
            {
                PictureBox trapPictureBox = new PictureBox
                {
                    Size = new Size(91, 68),
                    Image = trapImages[rnd.Next(trapImages.Length)],
                    SizeMode = PictureBoxSizeMode.StretchImage
                };

                Point location = GetRandomLocation();
                if (location != Point.Empty)
                {
                    trapPictureBox.Location = location;
                    Trap trap = new Trap
                    {
                        ObjectPictureBox = trapPictureBox,
                        IsActive = true
                    };

                    traps.Add(trap);
                }
            }
        }
        // Mevcut tüm bombaları temizleme
        public void ClearBomb()
        {
            foreach (var bomb in traps.ToList()) // Listenin bir kopyası üzerinden yineleme yapın
            {
                bomb.ObjectPictureBox.Visible = false; // Bombayı görünmez yap
                bomb.IsActive = false; // Bombayı devre dışı bırak
                traps.Remove(bomb); // Listeden çıkar
            }
        }


        // Bombaları rastgele yerleştirme
        public void DropBombs()
        {
            Image bombImage = Properties.Resources.bombapng;

            ClearBomb();
            for (int i = 0; i < 10; i++)
            {
                PictureBox bombPictureBox = new PictureBox
                {
                    Size = new Size(91, 68),
                    Image = bombImage,
                    SizeMode = PictureBoxSizeMode.StretchImage
                };

                Point location = GetRandomLocation();
                if (location != Point.Empty)
                {
                    bombPictureBox.Location = location;
                    Trap trap = new Trap
                    {
                        ObjectPictureBox = bombPictureBox,
                        IsActive = true
                    };

                    traps.Add(trap);
                }
            }
        }
        // Rastgele bir konum seçme
        private Point GetRandomLocation()
        {
            List<Point> availableLocations = Trap.TrapsLocations()
                                                  .Except(usedLocations)
                                                  .ToList();

            if (availableLocations.Count == 0)
            {
                return Point.Empty; // Tüm konumlar kullanıldıysa boş bir konum döndür
            }

            int index = rnd.Next(availableLocations.Count);
            Point selectedLocation = availableLocations[index];
            usedLocations.Add(selectedLocation); // Seçilen konumu kullanılmış olarak işaretle

            return selectedLocation;
        }

        public  Point GetRandomLocation(Random rnd)
        {
            List<Point> gridLocations = new List<Point>();
            for (int x = 242; x <= 1309; x += 97)
            {
                for (int y = 166; y <= 314; y += 74)
                {
                    gridLocations.Add(new Point(x, y));
                }
            }
           
            int index = rnd.Next(gridLocations.Count);
            return gridLocations[index];
        }
        public void CheckTraps(PictureBox playerPictureBox, Action gameOverAction)
        {
            // Tüm tuzakları tek tek kontrol et
            foreach (var trap in traps)
            {
                // Eğer tuzak aktifse ve oyuncunun PictureBox'ı tuzağın PictureBox'ı ile çakışıyorsa
                if (trap.IsActive && playerPictureBox.Bounds.IntersectsWith(trap.ObjectPictureBox.Bounds))
                {
                    // Tuzağı devre dışı bırak ve görünmez yap
                    trap.IsActive = false;
                    trap.ObjectPictureBox.Visible = false;

                    // Oyun bitirme işlevini çağır
                    gameOverAction.Invoke();
                    break; // Bir tuzak bulunduğunda döngüyü durdur
                }
            }
        }

    }
}
