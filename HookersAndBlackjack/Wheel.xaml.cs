using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
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
        public double LocationZ { get; set; }




        public Wheel()
        {
            this.InitializeComponent();

            Width = 80;
            Height = 141;
        }

        /// <summary>
        /// Rullan pyöräytys = random numero, images = kuvien määrä rullassa
        /// </summary>
        public int Spin()
        {
            //kuvan vaihto
            Random rand = new Random();
            int numero = rand.Next(1, (200 + 1));
            if (0 < numero && numero < 11)
            {
                //kuvan vaihto
                return 5;
            }
            else if (10 < numero && numero < 19)
            {
                //kuvan vaihto
                return 4;
            }
            else if (18 < numero && numero < 24)
            {
                //kuvan vaihto
                return 3;
            }
            else if (24 < numero && numero < 28)
            {
                //kuvan vaihto
                return 2;
            }
            else if (numero == 28)
            {
                //kuvan vaihto
                return 1;
            }
            else
            {
                //kuvan vaihto
                return 0;
            }
        }
    }
}

//voittotaulukko
/*
3 Wheels        Double
1. 25x	1/28    1-3 = 10/28
2. 10x	3/28    4-5 = 18/28
3. 5x   6/28    
4. 2x	8/28    
5. 1x	10/28   
0-10-18-24-27-28
*/
