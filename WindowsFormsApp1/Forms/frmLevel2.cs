using DevExpress.Utils.DragDrop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class frmLevel2 : Form 
    {
        private GameState gameState;
        private List<Trap> traps = new List<Trap>();
        private Timer gameTimer;
        private Keys lastKeyPressed = Keys.None;
        private  PlayerMover playerMover;
        private int elapsedTime = 0;
        public GameState CurrentGameState;
        public TrapManager trap = new TrapManager();
        public string PlayerName { get; set; }
        private bool levelTransitioning = false;
        public int Score { get; set; }
        private bool isPaused = false;
        private bool isRunning = false;
        private bool isSecond = false;
        private Random rnd = new Random();

        public frmLevel2(GameState gameState)
        {
            InitializeComponent();
            InitializeGameState(gameState);
            InitializePlayerMover();
            InitializeTimers();


        }
        private void InitializeGameState(GameState gameState)
        {
            this.gameState = gameState;
            elapsedTime = (int)gameState.TimeElapsed.TotalSeconds;
            UpdateUI();
            InitializeLevel2();
            CurrentGameState = new GameState(UpdateLives, PauseGame, ResumeGame);
        }

        private void InitializePlayerMover()
        {
            playerMover = new PlayerMover(CurrentGameState);
        }

        private void InitializeTimers()
        {
            InitializeMysteryTimer();
            InitializeBombTimer();
        }
        private void InitializeBombTimer()
        {
            if (!BombTimer.Enabled)
            {
                BombTimer.Interval = 3000;
                BombTimer.Tick += BombTimer_Tick;
                BombTimer.Start();
            }
        }

        private void InitializeMysteryTimer()
        {
            if (!mysteryTimer.Enabled)
            {
                mysteryTimer.Interval = 10000;
                mysteryTimer.Tick += mysteryTimer_Tick;
                mysteryTimer.Start();
            }
        }
       
        private void UpdateLives(int newLives)
        {
            lblCan.Text = newLives.ToString();
            if (newLives <= 0)
            {
                TogglePause();
                BombTimer.Stop();
                timer1.Stop();
                mysteryTimer.Stop();
                MessageBox.Show("Game Over!");
                this.Close();
                this.Dispose();
            }
        }


        private void StopAndDisposeTimers()
        {
            BombTimer.Stop();
            BombTimer.Dispose();
            mysteryTimer.Stop();
            mysteryTimer.Dispose();
            gameTimer.Stop();
            gameTimer.Dispose();
        }

        private void PrepareForLevelTransition()
        {
            if (levelTransitioning) return;

            levelTransitioning = true;
            ClearBombs();
            CurrentGameState = CreateNewGameState();
        }

        private GameState CreateNewGameState()
        {
            return new GameState(UpdateLives, PauseGame, ResumeGame)
            {
                Lives = gameState.Lives + 1,
                TimeElapsed = TimeSpan.FromSeconds(elapsedTime),
                Level = 3,
                CurrentForm = "Form2"
            };
        }

        private int CalculateScore()
        {
            int remainingLives = gameState.Lives;
            int elapsedTimeInSeconds = (int)gameState.TimeElapsed.TotalSeconds;
            return remainingLives * 500 + (1000 - elapsedTimeInSeconds);
        }

        private void ShowNextLevelForm()
        {
            var nextLevelForm = new frmLevel3(CurrentGameState)
            {
                Score = CalculateScore(),
                PlayerName = lblOyuncuAd.Text
            };
            nextLevelForm.Show();
        }

        private void CloseCurrentForm()
        {
            this.Dispose();
            this.Close();
        }

        public void GoToNextLevel()
        {
            StopAndDisposeTimers();
            PrepareForLevelTransition();

            if (levelTransitioning)
            {
                ShowNextLevelForm();
                CloseCurrentForm();
            }
        }

        private void UpdateGameStatus()
        {
            lblCan.Text = gameState.Lives.ToString();
            lblTimer.Text = FormatElapsedTime();
        }
        private string FormatElapsedTime()
        {
            return TimeSpan.FromSeconds(elapsedTime).ToString(@"mm\:ss");
        }
        private void IncrementElapsedTime()
        {
            elapsedTime++;
            lblTimer.Text = FormatElapsedTime();
        }

        private void UpdateUI()
        {
            // Oyun durumu bileşenlerini güncelle
            UpdateGameStatus();
            // Ve diğer UI bileşenlerini güncelle
        }
        private void InitializeLevel2()
        {
            // Oyunun yeni seviye için başlangıç durumlarını ayarlayın
            // Örneğin:
            lblCan.Text = gameState.Lives.ToString();
            lblSeviye.Text = gameState.Level.ToString();
            // Timer'ı alınan süreyle başlat
            if(isSecond)
            {
                isSecond = false;
                return;
            }
            gameTimer = new Timer();
            gameTimer.Interval = 1000;
            gameTimer.Tick += new EventHandler(timer1_Tick);
            gameTimer.Start();
            elapsedTime = (int)gameState.TimeElapsed.TotalSeconds;
            isSecond = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Her saniye çağrılan zamanlayıcı olayı. Oyun süresini artırır.
            IncrementElapsedTime();
        }

        private void FrmLevel2_KeyDown(object sender, KeyEventArgs e)
        {
            // Level 2 formunda tuşa basıldığında çağrılan olay.
            // P tuşuna basıldığında oyunu duraklatır veya devam ettirir.
            if (e.KeyCode == Keys.P)
            {
                TogglePause();
            }

            // Eğer oyun duraklatılmışsa veya aynı tuş tekrar basılmışsa işlem yapmaz.
            if (isPaused || e.KeyCode == lastKeyPressed) return;

            // WASD tuşlarına basıldığında oyuncu hareketini kontrol eder.
            if (e.KeyCode == Keys.W || e.KeyCode == Keys.A || e.KeyCode == Keys.S || e.KeyCode == Keys.D)
            {
                Point currentLocation = new Point(picturePlayer.Left, picturePlayer.Top);
                Point nextLocation = playerMover.GetNextPosition(currentLocation, e.KeyCode);
                picturePlayer.Left = nextLocation.X;
                picturePlayer.Top = nextLocation.Y;

                // Bombaları kontrol eder.
                CheckBombs();
                // Son basılan tuşu günceller.
                lastKeyPressed = e.KeyCode;
            }

            // Oyuncu resmini en öne getirir.
            picturePlayer.BringToFront();
            // Eğer oyuncu finish noktasına ulaştıysa bir sonraki seviyeye geçer.
            if (picturePlayer.Bounds.IntersectsWith(pictureFinish.Bounds))
            {
                GoToNextLevel();
            }
        }

        private void frmLevel2_Load(object sender, EventArgs e)
        {
            // Form yüklendiğinde çağrılan olay.
            // Oyuncu adı ve skoru etiketlere yazdırılır.
            lblOyuncuAd.Text = PlayerName;
            lblScore.Text = Score.ToString();
        }

        private void HandleBombCollision(Trap trap)
        {
            // Bomba ile çarpışma durumunda çağrılan metod.
            // Oyuncunun canını azaltır ve tuzak (trap) nesnesini devre dışı bırakır.
            gameState.Lives--;
            lblCan.Text = gameState.Lives.ToString();
            DeactivateAndHideTrap(trap);

            // Eğer oyuncunun canı 0'a düşerse oyunu bitirir.
            if (gameState.Lives <= 0)
            {
                EndGame();
            }
        }


        private void DeactivateAndHideTrap(Trap trap)
        {
            trap.ObjectPictureBox.Visible = false; // Make the bomb invisible
            trap.IsActive = false; // Deactivate the bomb
        }

        private void EndGame()
        {
            StopAllTimers();
            MessageBox.Show("Game Over!");
            CloseForm();
        }

        private void StopAllTimers()
        {
            gameTimer.Stop();
            BombTimer.Stop();
            timer1.Stop();
        }

        private void CloseForm()
        {
            this.Dispose();
            this.Close(); // Close the form if no lives are left
        }

        private void CheckBombs()
        {
            foreach (var trap in traps)
            {
                if (trap.IsActive && picturePlayer.Bounds.IntersectsWith(trap.ObjectPictureBox.Bounds))
                {
                    HandleBombCollision(trap);
                    if (gameState.Lives <= 0) return;
                    break;
                }
            }
        }

        private PictureBox CreateBombPictureBox()
        {
            Image bombImage = Properties.Resources.bombapng;
            return new PictureBox
            {
                Size = new Size(91, 68), // Assuming this is the size of your grid
                Image = bombImage,
                SizeMode = PictureBoxSizeMode.StretchImage
            };
        }

        private void DropBombs()
        {
            Random rnd = new Random();
            ClearBombs();

            for (int i = 0; i < 10; i++) // Assuming you want to drop 10 bombs
            {
                PictureBox bombPictureBox = CreateBombPictureBox();
                bombPictureBox.Location = trap.GetRandomLocation(rnd);

                traps.Add(new Trap { ObjectPictureBox = bombPictureBox, IsActive = true });
                this.Controls.Add(bombPictureBox);
                bombPictureBox.BringToFront();
            }
        }

        private void ClearBombs()
        {
            foreach (var bomb in traps)
            {
                this.Controls.Remove(bomb.ObjectPictureBox);
                bomb.ObjectPictureBox.Dispose(); // This is important to free resources
            }
            traps.Clear(); // Clear the list after removing bombs from the form
        }

        private void TogglePause()
        {
            if (isPaused)
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
            BombTimer.Stop();
            mysteryTimer.Stop();
            timer1.Stop();
            isPaused = true;
            lblGameStatus.Text = "Game Stopped";
        }

        private void ResumeGame()
        {
            // Zamanlayıcıları başlat
            gameTimer.Start();
            BombTimer.Start(); // Bomba timer'ını başlat
            mysteryTimer.Start();
            isPaused = false;
            lblGameStatus.Text = "Game Resumed";
        }
       
        private void BombTimer_Tick(object sender, EventArgs e)
        {
            if (!isPaused) // Oyun duraklatılmışsa bomba düşürme ve kontrol etme işlemlerini yapma
            {
                DropBombs(); // Yeni bombaları yerleştir
                CheckBombs(); // Bombaları kontrol et
            }
        }
        private void ApplyMysteryBoxEffect()
        {
            int chance = rnd.Next(100); // 0 ile 99 arasında rastgele bir sayı üretir
            if (isRunning)
            {
                isRunning = false;
                return;
            }

            if (chance < 80) // %80 ihtimal
            {
                gameState.Lives++; // Can ekle
                lblCan.Text = gameState.Lives.ToString();
            }
            else // %20 ihtimal
            {
                gameState.Lives--; // Can azalt
                lblCan.Text = gameState.Lives.ToString();
                if (gameState.Lives <= 0)
                {
                    timer1.Stop();
                    mysteryTimer.Stop();
                    MessageBox.Show("Game Over!");
                    this.Close();
                    this.Dispose();
                }
            }
            lblCan.Text = gameState.Lives.ToString(); // Güncellenmiş can sayısını etikete yaz
            isRunning = true;

        }
        private void mysteryTimer_Tick(object sender, EventArgs e)
        {
            ApplyMysteryBoxEffect();
        }

        private void FrmLevel2_KeyUp(object sender, KeyEventArgs e)
        {
            lastKeyPressed = Keys.None;
        }

        private void lblSeviye_Click(object sender, EventArgs e)
        {

        }
    }
}
