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
        public double LocationX { get; set; }
        public double LocationY { get; set; }
        Random rand = new Random();

        public Wheel()
        {
            this.InitializeComponent();

            Width = 80;
            Height = 141;

            image.Source = new BitmapImage(new Uri(this.BaseUri, ("/Assets/9.png")));
        }

        /// <summary>
        /// Rullan pyöräytys, 8 = ei voittoa
        /// </summary>
        public int Spin()
        {
            Random rand = new Random();
            int numero = rand.Next(0, 11);
            if (numero < 3)
            {
                return Winning();
            }
            else return Losing();
        }

        private int Losing()
        {
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
        //winning %
        private int Winning()
        {
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

        public void SetLocation()
        {
            SetValue(Canvas.LeftProperty, LocationX);
            SetValue(Canvas.TopProperty, LocationY);
        }

        public void ImageChange(int imagenumber)
        {
            image.Source = new BitmapImage(new Uri(BaseUri, ("/Assets/" + imagenumber.ToString() + ".png")));
        }
        
        /// <summary>
        /// DoubleWheelSpin, 3=win, 8=lose
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
    }
}
