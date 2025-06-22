using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public abstract class BaseLevel : Form, IGameState
    {

        protected GameState gameState;
        protected Timer gameTimer;
        // Ortak metotlar ve özellikler...
        protected abstract void InitializeLevel();
        protected abstract void UpdateUI();

        public abstract void UpdateLives(int newLives);
        public abstract void PauseGame();
        public abstract void ResumeGame();
    }
}
