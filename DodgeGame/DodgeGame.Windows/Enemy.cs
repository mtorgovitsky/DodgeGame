using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace DodgeGame
{
    public class Enemy
    {
        //Enemies quantity
        private const int ENEMIES = 10;

        //Enemy initial picture path from the Assets of the project
        private const string ENEMY_IMG_PATH = "ms-appx:///Assets/enemy.png";

        //Image hight and width constant
        private const double IMG_WIDTH_HEIGHT = 100;

        //Constant number for the enemies coordinates randomization
        private const int ENEMY_OFFSET = 1500;

        //Enemies array
        private Image[] _enemies;

        //Enemies class constructor
        public Enemy()
        {
            //Array by size of the enemies (see constant "ENEMIES")
            _enemies = new Image[ENEMIES];
        }

        //Get property for the enemy array member
        public Image[] Enemies
        {
            get
            {
                return _enemies;
            }            
        }
        
        //Set the image of the enemy
        public Image SetImage(int index, Canvas canv)
        {
            Random Rand = new Random();

            Image img = new Image();

            BitmapImage src = new BitmapImage(new Uri(ENEMY_IMG_PATH));

            img.SetValue(Canvas.TopProperty, (Rand.Next(ENEMY_OFFSET)));
            img.SetValue(Canvas.LeftProperty, (Rand.Next(ENEMY_OFFSET)));
            img.Source = src;
            img.Height = IMG_WIDTH_HEIGHT;
            img.Width = IMG_WIDTH_HEIGHT;
            canv.Children.Add(img);

            _enemies[index] = img;
            return img;
        }
    }
}
