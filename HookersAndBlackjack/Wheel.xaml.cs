using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HookersAndBlackjack
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Wheel : Page
    {
        //Wheel settings
        public double LocationX { get; set; }
        public double LocationY { get; set; }
        Random rand = new Random();
        //image path
        private string path = "/Assets/";

        /// <summary>
        /// Wheel setup
        /// </summary>
        public Wheel()
        {
            this.InitializeComponent();
            //W,H
            Width = 80;
            Height = 141;
            //default image = [?]
            image.Source = new BitmapImage(new Uri(this.BaseUri, ("/Assets/9.png")));
        }

        /// <summary>
        /// Get wheel combination
        /// </summary>
        public int Spin()
        {
            Random rand = new Random();
            int numero = rand.Next(0, 11);
            if (numero < 3)
            {
                return Winning(); //if random number < 3, player wins
            }
            else return Losing(); //else player loses
        }

        /// <summary>
        /// Player loses
        /// </summary>
        /// <returns></returns>
        private int Losing()
        {
            //Get random combination XYZ, Z!=X
            int number;
            string comb = "";
            for (int i = 1; i < 3; i++)
            {
                number = rand.Next(1, 6);
                comb += number.ToString();
            }

            for (int i = 1; i < 2; i++)
            {
                number = rand.Next(1, 6);
                if (comb.Contains(number.ToString()) == true)
                {
                    i--;
                }
                else
                {
                    comb += number.ToString();
                }
            }
            return int.Parse(comb);
        }
        

        /// <summary>
        /// Player wins
        /// </summary>
        /// <returns></returns>
        private int Winning()
        {
            //winnings
            int numero = rand.Next(0, 101);
            if (numero < 21) //25% --> 1x
            {
                return 555;
            }
            else if (numero < 56) //30% --> 2x
            {
                return 444;
            }
            else if (numero < 76) //20% --> 4x
            {
                return 333;
            }
            else if (numero < 91) //15% --> 5x
            {
                return 222;
            }
            else //10% --> 10x
            {
                return 111;
            }
        }

        /// <summary>
        /// Set wheel location on canvas
        /// </summary>
        public void SetLocation()
        {
            SetValue(Canvas.LeftProperty, LocationX);
            SetValue(Canvas.TopProperty, LocationY);
        }

        /// <summary>
        /// Change wheel image
        /// </summary>
        /// <param name="imagenumber"></param>
        public void ImageChange(int imagenumber)
        {
            image.Source = new BitmapImage(new Uri(BaseUri, (path + imagenumber.ToString() + ".png")));
        }
        
        /// <summary>
        /// Double, 3=win, 8=lose
        /// </summary>
        /// <returns></returns>
        public int DoubleSpin()
        {
            int numero = rand.Next(0, 11);
            if (numero < 4)
            {
                return 3;
            }
            else
            {
                return 8;
            }
        }
        /// <summary>
        /// Change imagepaths, 1=dank, 0=normal
        /// </summary>
        /// <param name="pathnumber"></param>
        public void ChangePath(int pathnumber)
        {
            if(pathnumber == 1)
            {
                path = "/Assets/dank/";
            }
            else
            {
                path = "/Assets/";
            }
        }
    }
}
