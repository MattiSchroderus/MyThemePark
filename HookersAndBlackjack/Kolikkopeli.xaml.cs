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
        private KPeli kolikkopeli;
        Player player = new Player("testaaja", 1);


        public static double CanvasWidth;
        public static double CanvasHeight;

        //Voitetut rahat jotka siirretään tilille
        int winnings = 0;
        //Player player = "valittu pelaaja classi";

        public Kolikkopeli()
        {
            this.InitializeComponent();

            CanvasWidth = MyCanvas.Width;
            CanvasHeight = MyCanvas.Height;
            kolikkopeli = new KPeli(MyCanvas);

            //testirahaa
            player.Money = 200;
        }


        // Play napin painaminen, pelin aloitus
        private void button_Play_Click(object sender, RoutedEventArgs e)
        {
            int bet = int.Parse(textBlock_Bet.Text);

            //tarkistus, riittääkö rahat
            if (kolikkopeli.CheckBet(bet, player) == true)
            {
                //jos, niin:
                //Rulluen pyöräytys
                int combination = kolikkopeli.Play(); //--> rullien combinaatio

                //Voitot/Häviöt
                winnings = kolikkopeli.CheckWin(combination, bet);
            }
            //jos ei:
            else
            {
                textBlock_Log.Text += "\nError: Not enough money!";
            }
        }

        //Back-napin painaminen
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
