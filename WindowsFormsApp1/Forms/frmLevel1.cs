using DevExpress.Utils.VisualEffects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace WindowsFormsApp1
{
    public partial class GameForm : Form
    {
        // Oyun ögelerinin ve durumlarının tanımlanması
        private List<Trap> traps = new List<Trap>();
        private Timer gameTimer;
        // private bool isPaused = false; // Oyunun duraklatılma durumunu tutar (şu anda kullanılmıyor)
        private PlayerMover playerMover;
        private Random rnd = new Random();
        private int elapsedTime = 0; // Oyun süresini tutar
        private GameState CurrentGameState; // Mevcut oyun durumunu tutar
        private bool levelTransitioning = false; // Seviye geçiş durumunu kontrol eder
        private Timer mysteryBoxTimer = new Timer(); // Gizemli kutu zamanlayıcısı
        public string PlayerName { get; set; } // Oyuncu adını saklar
        private Keys lastKeyPressed = Keys.None; // Son basılan tuşu saklar
        private TrapManager trapManager = new TrapManager(); // Tuzak yöneticisi


        public GameForm()
        {
            InitializeComponent();
            InitializeGame();
            lblSeviye.Text = CurrentGameState.Level.ToString();
        }
        private void InitializeGame()
        {
            // Oyun başlangıcında temel ayarların yapılması
            // Klavye olayları ve zamanlayıcıların ayarlanması
            this.KeyDown += new KeyEventHandler(GameForm_KeyDown);
            this.KeyPreview = true;

            

            mysteryBoxTimer = new Timer();
            
            gameTimer = new Timer();
            
            SetupGameState();
            SetupTimers();

            AddTrapsToForm();
            UpdateUI();
        }
        private void SetupGameState()
        {
            CurrentGameState = new GameState(UpdateLives, PauseGame, ResumeGame);
            playerMover = new PlayerMover(CurrentGameState);
            trapManager.InitializeTraps();
        }

        private void SetupTimers()
        {
            gameTimer.Interval = 1000; // 1 second
            gameTimer.Tick += new EventHandler(timer1_Tick);
            gameTimer.Start();

            mysteryBoxTimer.Interval = 10000; // 10 seconds
            mysteryBoxTimer.Tick += MysteryTimer_Tick;
            mysteryBoxTimer.Start();
        }

        private void AddTrapsToForm()
        {
            // Tuzakların forma eklenmesi
            foreach (var trap in trapManager.Traps)
            {
                this.Controls.Add(trap.ObjectPictureBox);
                trap.ObjectPictureBox.BringToFront();
            }
        }
        private void UpdateUI()
        {
            // Kullanıcı arayüzünün güncellenmesi
            // Can, seviye, zaman gibi bilgilerin gösterilmesi
            lblCan.Text = CurrentGameState.Lives.ToString();
            lblSeviye.Text = CurrentGameState.Level.ToString();
            lblTimer.Text = CurrentGameState.TimeElapsed.ToString(@"mm\:ss");
        }

        private void UpdateLives(int newLives)
        {
            // Oyuncunun canının güncellenmesi
            // Eğer can 0'a düşerse, oyunu bitirme işlemleri
            lblCan.Text = newLives.ToString();
            if (newLives <= 0)
            {
                timer1.Stop();
                mysteryBoxTimer.Stop();
                TogglePause();
                MessageBox.Show("Game Over!");
                this.Close();
                this.Dispose();
            }
        }   
        private void StopTimer()
        {
            levelTransitioning = true;
            mysteryBoxTimer.Stop();
            mysteryBoxTimer.Dispose();
            gameTimer.Stop();
        }
        private void CreateNewForm()
        {
            var nextLevelForm = new frmLevel2(CurrentGameState);
            nextLevelForm.Score = CalculatedScore();
            nextLevelForm.PlayerName = lblOyuncuAd.Text;
            nextLevelForm.Show();
            this.Close(); // Mevcut formu kapat
            gameTimer.Dispose();
            mysteryBoxTimer.Dispose();
            this.Dispose();
        }
        private void GoToNextLevel()
        {
            // Bir sonraki seviyeye geçiş yapılması
            // Zamanlayıcıların durdurulması ve mevcut oyun durumunun kaydedilmesi
            
            if (levelTransitioning) return;
            StopTimer();
            

            CurrentGameState.Level++; // Seviyeyi artır
            CurrentGameState.Lives++; 
                    CurrentGameState = new GameState(UpdateLives,ResumeGame,PauseGame)
            {
                TimeElapsed = TimeSpan.FromSeconds(elapsedTime), // Mevcut süreyi sakla
            };
            traps.Clear();


            // Yeni seviye formunu oluştur ve göster

            CreateNewForm();
        }
        private int CalculatedScore()
        {
            int remainingLives = CurrentGameState.Lives;
            int elapsedTimeInSeconds = (int)CurrentGameState.TimeElapsed.TotalSeconds; // TimeSpan'i int'e dönüştür
            int score =remainingLives * 500 + (1000 - elapsedTimeInSeconds);
            return score;
        }
        private void CheckTraps()
        {
            // Tuzakların kontrol edilmesi
            // Oyuncu tuzaklara çarptıysa GameOver fonksiyonunun çağrılması
            
            trapManager.CheckTraps(picturePlayer, GameOver);
        }
        private void GameOver()
        {
            // Oyunun bitirilmesi
            // Canın azaltılması ve 0 olup olmadığının kontrol edilmesi
            
            CurrentGameState.Lives--;
            lblCan.Text = CurrentGameState.Lives.ToString();
            if (CurrentGameState.Lives <= 0)
            {
                // Oyunu bitir
                gameTimer.Stop();
                TogglePause();
                MessageBox.Show("Game Over!");
                this.Close();
                this.Dispose();
            }
        }
        private void PlayerMovement( KeyEventArgs e)
        {
            if (e.KeyCode == Keys.P)
            {
                TogglePause();
            }
            if (CurrentGameState.isPaused || e.KeyCode == lastKeyPressed) return;
            if (e.KeyCode == Keys.W || e.KeyCode == Keys.A || e.KeyCode == Keys.S || e.KeyCode == Keys.D)
            {
                Point currentLocation = new Point(picturePlayer.Left, picturePlayer.Top);
                Point nextLocation = playerMover.GetNextPosition(currentLocation, e.KeyCode);
                picturePlayer.Left = nextLocation.X;
                picturePlayer.Top = nextLocation.Y;

                PlayerMoved();
                lastKeyPressed = e.KeyCode;
            }
            picturePlayer.BringToFront();
            if (picturePlayer.Bounds.IntersectsWith(pictureFinish.Bounds))
            {
                GoToNextLevel();
            }
        }
        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            PlayerMovement(e);
        }
        private void TogglePause()
        {
            if (CurrentGameState.isPaused)
            {
                CurrentGameState.ResumeGame();
            }
            else
            {
                CurrentGameState.PauseGame();
            }
        }
        private void PauseGame()
        {
            // Zamanlayıcıları durdur
            gameTimer.Stop();
            mysteryBoxTimer.Stop();
            CurrentGameState.isPaused = true;
            lblGameStatus.Text = "Game Stopped";
        }

        private void ResumeGame()
        {
            // Zamanlayıcıları başlat
            gameTimer.Start();
            mysteryBoxTimer.Start();
            CurrentGameState.isPaused = false;
             lblGameStatus.Text = "Game Resumed";
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            elapsedTime++;
            lblTimer.Text = TimeSpan.FromSeconds(elapsedTime).ToString(@"mm\:ss");
        }

        private void GameForm_Load(object sender, EventArgs e)
        {
            lblOyuncuAd.Text = PlayerName;
            this.KeyDown += new KeyEventHandler(GameForm_KeyDown);
            this.KeyUp += new KeyEventHandler(this.GameForm_KeyUp);

        }
        private void PlayerMoved()
        {
            CheckTraps(); // Tuzakları kontrol et
        }

        private void MysteryTimer_Tick(object sender, EventArgs e)
        {
            ApplyMysteryBoxEffect();
        }
        private void ApplyMysteryBoxEffect()
        {
            int chance = rnd.Next(100);
            if (CurrentGameState.isPaused) return;
            if (chance < 80) // %80 ihtimal
            {
                CurrentGameState.Lives++; // Can ekle
            }
            else // %20 ihtimal
            {
                CurrentGameState.Lives--; // Can azalt
                
                
            }
            UpdateLives(CurrentGameState.Lives);
        }
        private void GameForm_KeyUp(object sender, KeyEventArgs e)
        {
            lastKeyPressed = Keys.None;
        }
    }
}


