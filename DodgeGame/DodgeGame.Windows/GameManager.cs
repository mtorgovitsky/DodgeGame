using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace DodgeGame
{
    class GameManager : Board
    {
        //Enemy timer
        public DispatcherTimer _enemyTimer;

        //Exploding picture path from the Assets of the project
        private const string EXPLODE_IMAGE_PATH = "ms-appx:///Assets/boom.gif";

        //Game over You lost text message
        private string GAMEOVER_YOU_LOST = "G A M E  O V E R" + "\n  Y O U  L O S T";
        
        //Game over You won text message
        private string GAMEOVER_YOU_WON = "G A M E  O V E R" + "\n   Y O U  W O N";

        //Enemy class member
        private Enemy _enemy;

        //======Constructor creating array of enemies and filling it with the enemies======//
        public GameManager(Canvas cnvInit, Image imgInit) : base(cnvInit, imgInit)
        {
            _enemy = new Enemy();
            
            for (int i = 0; i < _enemy.Enemies.Length; i++)
            {                
                var enemy = _enemy.SetImage(i, cnvInit);
                for (int j = 0; j < i; j++)
                {
                    if ((ImageTouching(enemy, _enemy.Enemies[j])) || (ImageTouching(enemy, imgInit)))
                        {
                        enemy.Visibility = Visibility.Collapsed;
                        _enemy.Enemies[i] = null;
                        i--;
                    }
                }
            }

            _enemyTimer = new DispatcherTimer();
            _enemyTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            _enemyTimer.Tick += EnemyTimer_Tick;
        }
        
        //Starting the enemy movement and check uf there's enemies left
        //if not - stop the game.
        private void EnemyTimer_Tick(object sender, object e)
        {
            double imgTop = GetControlTop(imgPlayer);
            double imgLeft = GetControlLeft(imgPlayer);
            for (int i = 0; i < _enemy.Enemies.Length; i++)
            {
                if (ImageTouching(imgPlayer, _enemy.Enemies[i]))
                {
                    var textBlock = new TextBlock();
                    textBlock.Text = GAMEOVER_YOU_LOST;
                    textBlock.FontSize = 70;
                    textBlock.Loaded += TextBlock_Loaded;
                    base.CvsMain.Children.Add(textBlock);
                    _enemyTimer.Stop();
                    MainPage.PlayerTimer.Stop();

                    DisplayBoom(imgPlayer, new Uri(EXPLODE_IMAGE_PATH), removeBoom: false);

                    Window.Current.CoreWindow.KeyDown -= MainPage.CoreWindow_KeyDown;
                    Window.Current.CoreWindow.KeyUp -= MainPage.CoreWindow_KeyUp;

                    break;
                }
                else
                {
                    SetEnemy(imgPlayer, _enemy.Enemies[i]);
                }
            }

            for (int i = 0; i < _enemy.Enemies.Length; i++)
            {
                for (int j = i + 1; j < _enemy.Enemies.Length; j++)
                {
                    bool crash = ImageTouching(_enemy.Enemies[i], _enemy.Enemies[j]);
                    if (crash)
                    {
                        _enemy.Enemies[i].Visibility = Visibility.Collapsed;
                        _enemy.Enemies[j].Visibility = Visibility.Collapsed;

                        DisplayBoom(_enemy.Enemies[i], new Uri(EXPLODE_IMAGE_PATH));
                        DisplayBoom(_enemy.Enemies[j], new Uri(EXPLODE_IMAGE_PATH));

                        _enemy.Enemies[i] = null;
                        _enemy.Enemies[j] = null;

                        if (EndGame())
                        {
                            _enemyTimer.Stop();
                            MainPage.PlayerTimer.Stop();
                            MainPage.KeyDown = false;
                            MainPage.KeyLeft = false;
                            MainPage.KeyRight = false;
                            MainPage.KeyUp = false;


                            var textBlock = new TextBlock();
                            textBlock.Text = GAMEOVER_YOU_WON;
                            textBlock.FontSize = 70;
                            textBlock.Loaded += TextBlock_Loaded;
                            base.CvsMain.Children.Add(textBlock);
                        }
                        break;
                    }
                }
            }
        }

        //Text box to display the result message (end of the game)
        private void TextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            TextBlock textBlock = (TextBlock)sender;
            Canvas.SetTop(textBlock, (base.CvsMain.ActualHeight - textBlock.ActualHeight) / 2);
            Canvas.SetLeft(textBlock, (base.CvsMain.ActualWidth - textBlock.ActualWidth) / 2);
        }

        //Boolean flag for knowing if tha game is over
        private bool EndGame()
        {
            int enemiesCounter = 0;
            for (int i = 0; i < _enemy.Enemies.Length; i++)
            {
                if (_enemy.Enemies[i] != null)
                {
                    enemiesCounter++;
                }
            }
            return enemiesCounter <= 1;
        }

        //Display image of the enemy exploding
        private async void DisplayBoom(Image deadImage, Uri boomUri, bool removeBoom = true)
        {
            BitmapImage boom = new BitmapImage(boomUri);
            Image imgBoom = new Image();
            imgBoom.Source = boom;
            imgBoom.SetValue(Canvas.TopProperty, deadImage.GetValue(Canvas.TopProperty));
            imgBoom.SetValue(Canvas.LeftProperty, deadImage.GetValue(Canvas.LeftProperty));
            CvsMain.Children.Add(imgBoom);
            if (removeBoom)
            {
                await Task.Delay(1000);
                CvsMain.Children.Remove(imgBoom);
            }
        }

        //Moving enemy image
        public void SetEnemy(Image imgPlayer, Image imgEnemy)
        {
            if (imgEnemy == null)
            {
                return;
            }

            double topEnemy = GetControlTop(imgEnemy);
            double leftEnemy = GetControlLeft(imgEnemy);
            double topPlayer = GetControlTop(imgPlayer);
            double leftPlayer = GetControlLeft(imgPlayer);

            if (leftEnemy != leftPlayer)
            {
                double stepLeft = Math.Abs(leftEnemy - leftPlayer) > EnemyStep ? EnemyStep : Math.Abs(leftEnemy - leftPlayer);

                if (leftEnemy > leftPlayer)
                    leftEnemy = leftEnemy - stepLeft;
                else
                    leftEnemy = leftEnemy + stepLeft;
            }

            if (topEnemy != topPlayer)
            {
                double stepTop = Math.Abs(topEnemy - topPlayer) > EnemyStep ? EnemyStep : Math.Abs(topEnemy - topPlayer);

                if (topEnemy > topPlayer)
                    topEnemy = topEnemy - stepTop;
                else
                    topEnemy = topEnemy + stepTop;
            }

            Move(imgEnemy, Canvas.TopProperty, topEnemy);
            Move(imgEnemy, Canvas.LeftProperty, leftEnemy);
        }

        //Start enemy movement
        public void StartEnemy()
        {
            _enemyTimer.Start();
        }

        //Stop enemy movement
        public void StopEnemy()
        {
            _enemyTimer.Stop();
        }

        public void KeyFlagsFalse()
        {
            //Flags init for the new game
            MainPage.KeyDown = false;
            MainPage.KeyLeft = false;
            MainPage.KeyUp = false;
            MainPage.KeyRight = false;
        }
    }
}

