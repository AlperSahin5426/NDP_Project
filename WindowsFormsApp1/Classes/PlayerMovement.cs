using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public class PlayerMover
    {
        private GameState gameState;

        public PlayerMover(GameState gameState)
        {
            // Oyun durumunu PlayerMover sınıfına başlatma
            this.gameState = gameState;
        }

        private Point MoveUp(Point currentLocation, IEnumerable<Point> gridLocations)
        {
            return gridLocations.Where(p => p.X == currentLocation.X && p.Y < currentLocation.Y)
                                .OrderByDescending(p => p.Y)
                                .FirstOrDefault();
        }

        private Point MoveLeft(Point currentLocation, IEnumerable<Point> gridLocations)
        {
            return gridLocations.Where(p => p.Y == currentLocation.Y && p.X < currentLocation.X)
                                .OrderByDescending(p => p.X)
                                .FirstOrDefault();
        }

        private Point MoveDown(Point currentLocation, IEnumerable<Point> gridLocations)
        {
            return gridLocations.Where(p => p.X == currentLocation.X && p.Y > currentLocation.Y)
                                .OrderBy(p => p.Y)
                                .FirstOrDefault();
        }

        private Point MoveRight(Point currentLocation, IEnumerable<Point> gridLocations)
        {
            return gridLocations.Where(p => p.Y == currentLocation.Y && p.X > currentLocation.X)
                                .OrderBy(p => p.X)
                                .FirstOrDefault();
        }

        public Point GetNextPosition(Point currentLocation, Keys direction)
        {
            // Oyuncunun bir sonraki pozisyonunu hesaplama
            Point nextLocation = currentLocation;
            var gridLocations = gameState.GetGridLocations(); // Oyun ızgarasının konumlarını al

            switch (direction)
            {
                case Keys.W:
                    nextLocation = MoveUp(currentLocation, gridLocations);
                    break;
                case Keys.A:
                    nextLocation = MoveLeft(currentLocation, gridLocations);
                    break;
                case Keys.S:
                    nextLocation = MoveDown(currentLocation, gridLocations);
                    break;
                case Keys.D:
                    nextLocation = MoveRight(currentLocation, gridLocations);
                    break;
            }

            // Eğer bir sonraki konum geçerliyse onu, değilse mevcut konumu döndür
            return nextLocation != Point.Empty ? nextLocation : currentLocation;
        }
    }
}
