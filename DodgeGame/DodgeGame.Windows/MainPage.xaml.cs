using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace DodgeGame
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static DispatcherTimer PlayerTimer;

        //Classes of the game
        private Board boardMain;
        private static Player playerMain;
        private GameManager gameManager;

        //Keys flags
        public static new bool KeyUp { get; set; }
        public static new bool KeyDown { get; set; }
        public static bool KeyLeft { get; set; }
        public static bool KeyRight { get; set; }

        public MainPage()
        {
            this.InitializeComponent();

            //Board class instance
            boardMain = new Board(cnvCanvas, imgMain);
            
            //Player class instance
            playerMain = new Player(cnvCanvas, imgMain);

            //GameManager class instance
            gameManager = new GameManager(cnvCanvas, imgMain);

            //Starting the player timer
            PlayerTimer = new DispatcherTimer();
            PlayerTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            PlayerTimer.Tick += PlayerTick;
           

            //Key DOWN and Key UP event handling
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            Window.Current.CoreWindow.KeyUp += CoreWindow_KeyUp;
        }

        //Player tick event
        public static void PlayerTick(object sender, object e)
        {
            playerMain.Move();
        }

        //Change the flags by KeyUp event
        public static void CoreWindow_KeyUp(CoreWindow sender, KeyEventArgs args)
        {
            if (args.VirtualKey == VirtualKey.Right)
            {
                KeyRight = false;
            }
            if (args.VirtualKey == VirtualKey.Left)
            {
                KeyLeft = false;
            }
            if (args.VirtualKey == VirtualKey.Up)
            {
                KeyUp = false;
            }
            if (args.VirtualKey == VirtualKey.Down)
            {
                KeyDown = false;
            }
        }

        //Change the flags by KeyDown event
        public static void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            if (args.VirtualKey == VirtualKey.Up)
            {
                KeyUp = true;
            }
            else if (args.VirtualKey == VirtualKey.Down)
            {
                KeyDown = true;
            }
            else if (args.VirtualKey == VirtualKey.Right)
            {
                KeyRight = true;
            }
            else if (args.VirtualKey == VirtualKey.Left)
            {
                KeyLeft = true;
            }
        }

        //Exit the game
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }

        //Start the game with the medium level
        private void btnEasy_Click(object sender, RoutedEventArgs e)
        {
            StartGameWithLevel(Board.ENEMY_STEP_EASY, Board.PLAYER_STEP_EASY);
        }

        //Start the game with the Medium level
        private void btnMedium_Click(object sender, RoutedEventArgs e)
        {
            StartGameWithLevel(Board.ENEMY_STEP_MEDIUM, Board.PLAYER_STEP_MEDIUM);
        }

        //Start the game with the hard level
        private void btnHard_Click(object sender, RoutedEventArgs e)
        {
            StartGameWithLevel(Board.ENEMY_STEP_HARD, Board.PLAYER_STEP_HARD);
        }

        //Pause the game
        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            PlayerTimer.Stop();
            gameManager.StopEnemy();
        }

        //New game starting
        private void btnNewGame_Click(object sender, RoutedEventArgs e)
        {
            //Set Key flags to false to stop player from moving.
            gameManager.KeyFlagsFalse();

            //Stop Player and Enemy movement even when user push the arrow keys
            PlayerTimer.Stop();
            gameManager.StopEnemy();


            //New Game
            this.Frame.Navigate(typeof(DodgeGame.MainPage));
        }

        //Starting the game with level that user choosed
        private void StartGameWithLevel(double enemyStep, double playerStep)
        {
            boardMain.EnemyStep = enemyStep;
            playerMain.PlayerStep = playerStep;
            gameManager.EnemyStep = enemyStep;

            PlayerTimer.Start();
            
            gameManager.StartEnemy();
            btnEasy.Visibility = Visibility.Collapsed;
            btnMedium.Visibility = Visibility.Collapsed;
            btnHard.Visibility = Visibility.Collapsed;
        }

        //Continuing after stop
        private void btnContinue_Click(object sender, RoutedEventArgs e)
        {
            PlayerTimer.Start();
            gameManager.StartEnemy();
        }
    }
}
