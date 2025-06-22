using DevExpress.UIAutomation;
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
    public partial class frmLevel3 : Form
    {
        // Oyun özelliklerinin ve durumlarının tanımlanması
        private GameState gameState;
        private List<Trap> traps = new List<Trap>();
        private Timer gameTimer;
        private List<Soldier> enemySoldiers = new List<Soldier>();
        private int elapsedTime = 0;
        private Keys lastKeyPressed = Keys.None;
        private PlayerMover playerMover;
        private TrapManager trap_3 = new TrapManager();
        private Random rnd = new Random();
        private GameState CurrentGameState;
        private bool isRunning = false;
        public string PlayerName { get; set; }
        private bool levelTransitioning = false;

        private TrapManager trapManager = new TrapManager();
        public int Score { get; set; }
        public frmLevel3(GameState gameState)
        {

            InitializeGame(gameState);
            InitializeGameTimer();

            

        }
        // Oyunun başlatılması ve zamanlayıcıların ayarlanması
        private void InitializeGame(GameState gameState)
        {
            InitializeComponent();
            this.gameState = gameState;
            elapsedTime = (int)gameState.TimeElapsed.TotalSeconds;
            InitializeTimers();
            UpdateUI();
            InitializeLevel3();
        }

        private void InitializeTimers()
        {
            InitializeGameTimer();
            InitializeMysteryTimer();
            InitializeSoldierTimer();
            InitializeMoveTimer();
        }

        private void InitializeGameTimer()
        {
            gameTimer = new Timer
            {
                Interval = 1000 // 1 saniye
            };
            gameTimer.Tick += timer1_Tick; // EventHandler ekle
            gameTimer.Start(); // Timer'ı başlat
        }

        private void InitializeMysteryTimer()
        {
            if (!mysteryTimer.Enabled)
            {
                mysteryTimer.Interval = 10000; // 10 saniye
                mysteryTimer.Tick += mysteryTimer_Tick;
                mysteryTimer.Start();
            }
        }

        private void InitializeSoldierTimer()
        {
            if (!SoldierTimer.Enabled)
            {
                SoldierTimer = new Timer
                {
                    Interval = 2000 // 2 saniye
                };
                SoldierTimer.Tick += SoldierTimer_Tick;
                SoldierTimer.Start();
            }
        }

        private void InitializeMoveTimer()
        {
            if (!MoveTimer.Enabled)
            {
                MoveTimer = new Timer
                {
                    Interval = 1000 // 1 saniye
                };
                MoveTimer.Tick += MoveTimer_Tick;
                MoveTimer.Start();
            }
        }

        private void InitializeLevel3()
        {
            // Oyunun yeni seviye için başlangıç durumlarını ayarlayın
            lblCan.Text = gameState.Lives.ToString();
            lblSeviye.Text = gameState.Level.ToString();
            playerMover = new PlayerMover(gameState);
            // Timer'ı alınan süreyle başlat
            gameTimer = new Timer();
            gameTimer.Interval = 1000;
            gameTimer.Tick += timer1_Tick;
            gameTimer.Start();
            elapsedTime = (int)gameState.TimeElapsed.TotalSeconds;
        }

        private void UpdateUI()
        {
            // Oyun durumu bileşenlerini güncelle
            lblCan.Text = gameState.Lives.ToString();
            lblTimer.Text = TimeSpan.FromSeconds(elapsedTime).ToString(@"mm\:ss");
            // Ve diğer UI bileşenlerini güncelle
        }

        
        private void frmLevel3_KeyDown(object sender, KeyEventArgs e)
        {
            // Klavye olayları için işleyici
            // Oyunun duraklatılması, karakter hareketleri ve tuzak kontrolleri
            
            if (e.KeyCode == Keys.P)
            {
                TogglePause();
            }
            if (gameState.isPaused || e.KeyCode == lastKeyPressed) return;

            if (e.KeyCode == Keys.W || e.KeyCode == Keys.A || e.KeyCode == Keys.S || e.KeyCode == Keys.D)
            {
                Point currentLocation = new Point(picturePlayer.Left, picturePlayer.Top);
                Point nextLocation = playerMover.GetNextPosition(currentLocation, e.KeyCode);
                picturePlayer.Left = nextLocation.X;
                picturePlayer.Top = nextLocation.Y;

                lastKeyPressed = e.KeyCode;
            }
            picturePlayer.BringToFront();
            CheckTraps();
            if (picturePlayer.Bounds.IntersectsWith(pictureFinish.Bounds))
            {
                FinishAndReturnMainMenu();
            }
        }
        private void TogglePause()
        {
            if (gameState.isPaused)
            {
                gameState.isPaused = false;
                UpdateGameStatus("Game Resumed");
                StartTimers();
            }
            else
            {
                gameState.isPaused = true;
                UpdateGameStatus("Game Stopped");
                StopTimers();
            }
        }
        private void PauseGame()
        {
            // Zamanlayıcıları durdur
            gameTimer.Stop();
            mysteryTimer.Stop();
            gameState.isPaused = true;
            lblGameStatus.Text = "Game Stopped";
        }
        private void UpdateGameStatus(string status)
        {
            // Oyun durum metnini güncelle
            lblGameStatus.Text = status;
        }
        private void StartTimers()
        {
            // Zamanlayıcıları başlat
            gameTimer.Start();
            mysteryTimer.Start();
        }

        private void StopTimers()
        {
            // Zamanlayıcıları durdur
            gameTimer.Stop();
            mysteryTimer.Stop();
        }

        private void ResumeGame()
        {
            // Zamanlayıcıları başlat
            gameTimer.Start();
            mysteryTimer.Start();
            gameState.isPaused = false;
            lblGameStatus.Text = "Game Resumed";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Her saniye çağrılan zamanlayıcı olayı. Oyun süresini artırır ve günceller.
            elapsedTime++;
            lblTimer.Text = TimeSpan.FromSeconds(elapsedTime).ToString(@"mm\:ss");
        }

        private void UpdateLives(int newLives)
        {
            // Oyuncunun can bilgisini günceller. Can 0'a düşerse oyunu sonlandırır.
            lblCan.Text = newLives.ToString();
            if (newLives <= 0)
            {
                timer1.Stop();
                mysteryTimer.Stop();
                MessageBox.Show("Game Over!");
                this.Close();
                this.Dispose();
            }
        }

        private void FinishAndReturnMainMenu()
        {
            // Oyunu bitirip ana menüye dönme işlevini gerçekleştirir.
            if (levelTransitioning) return;

            StopAllGameTimers();
            levelTransitioning = true;
            SaveCurrentGameState();
            WriteScoreToFile();
            CloseAndOpenMainMenu();
        }

        private void StopAllGameTimers()
        {
            // Tüm oyun zamanlayıcılarını durdurur.
            gameTimer.Stop();
            SoldierTimer.Stop();
            MoveTimer.Stop();
        }

        private void SaveCurrentGameState()
        {
            // Mevcut oyun durumunu kaydeder.
            CurrentGameState = new GameState(UpdateLives, PauseGame, ResumeGame)
            {
                Lives = gameState.Lives,
                TimeElapsed = TimeSpan.FromSeconds(elapsedTime),
                Level = 3
            };
        }

        private void WriteScoreToFile()
        {
            // Skoru bir dosyaya yazar.
            int score = CalculateScore();
            using (StreamWriter writer = new StreamWriter("skor.txt", true))
            {
                writer.WriteLine($"Oyuncu Adı: {PlayerName} Skor: {score}");
            }
        }

        private int CalculateScore()
        {
            // Skor hesaplamasını gerçekleştirir.
            int elapsedTimeInSeconds = (int)CurrentGameState.TimeElapsed.TotalSeconds;
            return CurrentGameState.Lives * 500 + (1000 - elapsedTimeInSeconds);
        }

        private void CloseAndOpenMainMenu()
        {
            // Formu kapatır ve ana menüyü açar.
            this.Close();
            this.Dispose();
            FrmMainMenu mainMenu = new FrmMainMenu();
            mainMenu.Show();
        }

        private void CheckTraps()
        {
            // Tuzakları kontrol eder ve çarpışma durumunda işlemleri gerçekleştirir.
            foreach (var soldier in enemySoldiers)
            {
                if (soldier.IsActive && picturePlayer.Bounds.IntersectsWith(soldier.SoldierPictureBox.Bounds))
                {
                    DeactivateSoldier(soldier);
                    gameState.Lives--;
                    UpdateLivesUI();
                    if (gameState.Lives <= 0)
                    {
                        EndGame();
                        break;
                    }
                }
            }
        }

        private void DeactivateSoldier(Soldier soldier)
        {
            // Askeri devre dışı bırakır.
            soldier.IsActive = false;
            soldier.SoldierPictureBox.Visible = false;
        }

        private void UpdateLivesUI()
        {
            // Can bilgisini kullanıcı arayüzünde günceller.
            lblCan.Text = gameState.Lives.ToString();
        }


        private void EndGame()
        {
            gameTimer.Stop();
            TogglePause(); // Optionally pause other processes
            MessageBox.Show("Game Over!");
            CloseAndDisposeForm();
        }

        private void CloseAndDisposeForm()
        {
            this.Dispose();
            this.Close();
        }

        private void CreateSoldiers()
        {
            // Oyun duraklatılmışsa asker oluşturmayı durdurur.
            if (gameState.isPaused) return;

            // Yeni bir düşman askeri oluşturur ve oyun alanına ekler.
            Soldier enemySoldier = CreateEnemySoldier();
            this.Controls.Add(enemySoldier.SoldierPictureBox);
            enemySoldier.SoldierPictureBox.BringToFront();
            enemySoldiers.Add(enemySoldier);
        }

        private Soldier CreateEnemySoldier()
        {
            // Rastgele bir konumda yeni bir düşman askeri oluşturur.
            Random rnd = new Random();
            Point randomLocation = trap_3.GetRandomLocation(rnd);

            return new Soldier
            {
                SoldierPictureBox = new PictureBox
                {
                    Size = new Size(91, 68),
                    Image = Properties.Resources.asker,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Location = randomLocation
                }
            };
        }

        private void SoldierTimer_Tick(object sender, EventArgs e)
        {
            // Belirli aralıklarla asker oluşturur.
            CreateSoldiers();
        }

        private void MoveTimer_Tick(object sender, EventArgs e)
        {
            // Oyun duraklatılmışsa askerleri hareket ettirmez.
            if (gameState.isPaused) return;

            // Aktif askerleri hareket ettirir.
            foreach (var soldier in enemySoldiers.ToList())
            {
                if (!soldier.IsActive) continue;

                Point currentLocation = new Point(soldier.SoldierPictureBox.Left, soldier.SoldierPictureBox.Top);
                // Askerin yeni konumunu belirler. Eğer sınıra ulaşırsa askeri kaldırır.
                Point nextLocation = Trap.TrapsLocations().Where(p => p.Y == currentLocation.Y && p.X < currentLocation.X)
                                                .OrderByDescending(p => p.X)
                                                .FirstOrDefault();

                if (nextLocation == Point.Empty || soldier.SoldierPictureBox.Left <= 242)
                {
                    // Asker sınıra ulaştıysa kaldırır.
                    this.Controls.Remove(soldier.SoldierPictureBox);
                    soldier.SoldierPictureBox.Dispose();
                    soldier.IsActive = false;
                }
                else
                {
                    // Askeri yeni konumuna taşır.
                    soldier.SoldierPictureBox.Left = nextLocation.X;
                    soldier.SoldierPictureBox.Top = nextLocation.Y;
                }
            }
            // Tuzakları kontrol eder.
            CheckTraps();
        }

        private void ApplyMysteryBoxEffect()
        {
            if (isRunning)
            {
                isRunning = false;
                return;
            }
            int chance = rnd.Next(100); // 0 ile 99 arasında rastgele bir sayı üretir

            if (chance < 80) // %80 ihtimal
            {
                gameState.Lives++; // Can ekle
                isRunning = true;
            }
            else // %20 ihtimal
            {
                gameState.Lives--; // Can azalt
                lblCan.Text = gameState.Lives.ToString();
                if (gameState.Lives <= 0)
                {
                    timer1.Stop();
                    mysteryTimer.Stop();
                    TogglePause();
                    MessageBox.Show("Game Over!");
                    this.Close();
                    this.Dispose();
                }
            }
            lblCan.Text = gameState.Lives.ToString(); // Güncellenmiş can sayısını etikete yaz
            isRunning = true;
        }
        private void frmLevel3_Load(object sender, EventArgs e)
        {
            // Form yüklendiğinde yapılacak işlemler
            // Oyuncu adı ve skorun güncellenmesi
            
            lblOyuncuAd.Text = PlayerName;
            lblScore.Text = Score.ToString();
        }

        private void frmLevel3_KeyUp(object sender, KeyEventArgs e)
        {
            // Tuş bırakma olayı için işleyici
            // Son basılan tuşun sıfırlanması
            
            lastKeyPressed = Keys.None;
        }

        private void mysteryTimer_Tick(object sender, EventArgs e)
        {
            // Gizemli kutu zamanlayıcısı için işleyici
            // ApplyMysteryBoxEffect metodunun çağrılması
            
            ApplyMysteryBoxEffect();
        }
    }
}
