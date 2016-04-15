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
using HookersAndBlackjack.Model;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HookersAndBlackjack
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Kolikkopeli : Page
    {
        KPeli kolikkopeli = new KPeli();

        //Voitetut rahat jotka siirretään tilille
        int winnings = 0;
        //Player player = "valittu pelaaja classi";

        public Kolikkopeli()
        {
            this.InitializeComponent();
        }

        // Play napin painaminen, pelin aloitus
        private void button_Play_Click(object sender, RoutedEventArgs e)
        {
            int bet = int.Parse(textBlock_Bet.Text);

            //tarkistus, riittääkö rahat
            if (kolikkopeli.bet(int.Parse(textBlock_Bet.Text)) == true)
            {

                //Rulluen pyöräytys
                int comb = kolikkopeli.Play(); //--> rullien combinaatio

                //Voitot/Häviöt
                winnings = kolikkopeli.Win(comb, bet);

                //Kuvien vaihto
                /*
                Images(int.Parse(comb.ToString().Substring(0,1)), image1);
                Images(int.Parse(comb.ToString().Substring(1,2)), image2);
                Images(int.Parse(comb.ToString().Substring(2,3)), image3);
                */
            }
            else
            {
                textBlock_Log.Text += "\nError: Not enough money!";
            }
        }

        /*
        //Kuvarullan kuvien vaihto
        private void Images(int combination, Image img)
        {
            switch (combination)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                default:
            }
        }
        */

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            // get root frame (which show pages)
            Frame rootFrame = Window.Current.Content as Frame;
            // did we get it correctly
            if (rootFrame == null) return;
            // navigate back if possible
            if (rootFrame.CanGoBack)
            {
                rootFrame.GoBack();
            }
        }
        //Tuplaus
        /*
                //button_Double.isPressed())
                {
                    if(kolikkopeli.Double() == 1)
                    {
                        player.TakeMoney(winnings);
                        winnings = winnings * 2;
                        //player.AddMoney(winnings);
                    }
                    else
                    {
                        player.TakeMoney(winnings)
                    }
                }
                    */

    }

}
