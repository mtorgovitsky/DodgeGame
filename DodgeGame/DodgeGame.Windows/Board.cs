using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace DodgeGame
{
    class Board
    {
        //Canvas member
        private Canvas _cvsMain;
        public Canvas CvsMain
        {
            get { return _cvsMain; }
            set { _cvsMain = value; }
        }

        //Image member
        private Image _imgPlayer;
        public Image imgPlayer
        {
            get { return _imgPlayer; }
            set { _imgPlayer = value; }
        }

        //Constructor
        public Board (Canvas cnvInit, Image imgInit)
        {
            _cvsMain = cnvInit;
            _imgPlayer = imgInit;
            PlayerStep = PLAYER_STEP_EASY;
            EnemyStep = ENEMY_STEP_EASY;
    }

        //Constants for PLAYER moving speed
        public const double PLAYER_STEP_EASY = 3.5;
        public const double PLAYER_STEP_MEDIUM = 6;
        public const double PLAYER_STEP_HARD = 10;

        //Constants for ENEMY moving speed
        public const double ENEMY_STEP_EASY = 1;
        public const double ENEMY_STEP_MEDIUM = 2;
        public const double ENEMY_STEP_HARD = 3;

        private double _playerStep;
        
        public double EnemyStep { get; set; }

        public double PlayerStep
        {
            get
            {
                return _playerStep;
            }

            set
            {
                _playerStep = value;
            }
        }

        //Getting the image object top coordinate
        public double GetControlTop(Image imgParam)
        {
            return Canvas.GetTop(imgParam);
        }
        //Getting the image object left coordinate
        public double GetControlLeft(Image imgParam)
        {
            return Canvas.GetLeft(imgParam);
        }

        //Moving elements on the canvas
        public void Move(UIElement uiElement, DependencyProperty dependencyProperty, double newPosition)
        {
            uiElement.SetValue(dependencyProperty, newPosition);
        }

        //Image is in the range of another image
        public bool ImageTouching(Image player, Image enemy)
        {
            if (player == null || enemy == null)
            {
                return false;
            }

            double gapLeft = Math.Abs(Canvas.GetLeft(enemy) - Canvas.GetLeft(player));
            bool col1 = gapLeft < enemy.ActualWidth;

            double gapTop = Math.Abs(Canvas.GetTop(enemy) - Canvas.GetTop(player));
            bool col2 = gapTop < enemy.ActualHeight;

            return col1 && col2;
        }
    }
}
