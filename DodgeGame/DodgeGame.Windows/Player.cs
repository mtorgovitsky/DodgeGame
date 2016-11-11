using Windows.UI.Xaml.Controls;

namespace DodgeGame
{
    class Player : Board
    {
        //Constructor for Player
        public Player(Canvas cnvInit, Image imgInit) : base(cnvInit, imgInit)
        {
        }

        //Moving the player by arrow keys in keyboard
        public void Move()
        {
            //Top and left properties
            double imgTop;
            double imgLeft;

            //Arrow Up pushed
            if (MainPage.KeyUp)
            {
                imgTop = GetControlTop(this.imgPlayer);
                if (imgTop - PlayerStep >= 0)
                {
                    base.Move(this.imgPlayer, Canvas.TopProperty, imgTop - PlayerStep);
                }
            }

            //Arrow Down pushed
            if (MainPage.KeyDown)
            {
                imgTop = GetControlTop(this.imgPlayer);
                if (imgTop + this.imgPlayer.ActualHeight < this.CvsMain.ActualHeight)
                {
                    base.Move(this.imgPlayer, Canvas.TopProperty, imgTop + PlayerStep);
                }
            }

            //Arrow Right pushed
            if (MainPage.KeyRight)
            {
                imgLeft = GetControlLeft(this.imgPlayer);
                if (imgLeft + this.imgPlayer.ActualWidth <= this.CvsMain.ActualWidth)
                {
                    base.Move(this.imgPlayer, Canvas.LeftProperty, imgLeft + PlayerStep);

                }
            }

            //Arrow Left pushed
            if (MainPage.KeyLeft)
            {
                imgLeft = GetControlLeft(this.imgPlayer);
                if (imgLeft - PlayerStep >= 0)
                {
                    base.Move(this.imgPlayer, Canvas.LeftProperty, imgLeft - PlayerStep);
                }
            }
        }
    }
}
