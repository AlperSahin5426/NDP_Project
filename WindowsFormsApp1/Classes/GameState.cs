using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace WindowsFormsApp1
{
    public class GameState
    {
        // Oyunun yaşam (can) sayısı
        public int Lives { get; set; }
        private Random rnd = new Random();
        // Oyunun skoru
        public int Score { get; set; }
        // Oyunun mevcut seviyesi
        public int Level { get; set; }
        // Geçen süre
        public TimeSpan TimeElapsed { get; set; }
        // Oyuncunun ismi
        public string PlayerName { get; set; }
        // Oyunun duraklatılıp duraklatılmadığını kontrol eden flag
        public bool isPaused = false;
        // Gizemli kutu için zamanlayıcı
        private Timer mysteryBoxTimer;
        // Can güncellemesi için delegate
        private Action<int> updateLivesAction;
        // Oyunu duraklatmak için action
        private Action pauseGameAction;
        // Mevcut formun adı
        public string CurrentForm { get; set; }
        // Oyunu devam ettirmek için action
        private Action resumeGameAction;

        // Yapıcı metot, başlangıç değerlerini ayarlar ve actionları alır
        public GameState(Action<int> updateLives, Action pauseGame, Action resumeGame)
        {
            Lives = 3;
            Score = 0;
            Level = 1;
            TimeElapsed = TimeSpan.Zero;
            updateLivesAction = updateLives;
            pauseGameAction = pauseGame;
            resumeGameAction = resumeGame;
        }

        // Oyunu duraklatır
        public void PauseGame()
        {
            pauseGameAction?.Invoke();
        }

        // Oyunu devam ettirir
        public void ResumeGame()
        {
            resumeGameAction?.Invoke();
        }

        // Bir sonraki seviyeye geçer
        public void NextLevel()
        {
            Level++;
            Lives++; // Her yeni seviyede can 1 artar
                     // Not: Süre ve puan gibi diğer değerlerin güncellenmesi oyunun kurallarına bağlıdır.
        }

        // Oyunun mevcut durumunu tutar
        public GameState CurrentGameState { get; private set; }

        // Izgaradaki konumları döndürür
        public List<Point> GetGridLocations()
        {
            List<Point> gridLocations = new List<Point>();
            for (int x = 145; x <= 1406; x += 97)
            {
                for (int y = 166; y <= 314; y += 74)
                {
                    gridLocations.Add(new Point(x, y));
                }
            }
            return gridLocations;
        }
    }
}
