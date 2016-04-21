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
using Windows.UI;

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
            TextMoney();
        }

        
        // Play napin painaminen, pelin aloitus
        private void button_Play_Click(object sender, RoutedEventArgs e)
        {
            int bet = int.Parse(textBlock_Bet.Text);

            //tarkistus, riittääkö rahat
            if (player.Money >= bet && bet != 0)
            {
                //jos, niin:
                //Rulluen pyöräytys
                int combination = kolikkopeli.Play(); //--> rullien combinaatio

                //Voitot/Häviöt
                winnings = kolikkopeli.CheckWin(combination, bet);
                player.Money += winnings;
                TextMoney();
                if (winnings > 0)
                {
                    LogAdd("You won " + winnings.ToString());
                }
                else
                {
                    LogAdd("You lost " + ((-1) * winnings).ToString());
                }
            }
            //jos ei:
            else
            {
                LogAdd("Not enough money!");
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

        private void LogAdd(string text)
        {
            textBlock_Log.Text = text + Environment.NewLine + textBlock_Log.Text;
        }

        private void TextMoney()
        {
            textBlock_Money.Text = "Money: " + player.Money.ToString();
        }

        //Teemavalitsin
        private void languageButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender.Equals(languageButton_Suomi))
            {
                muteButton.Visibility = Visibility.Collapsed;
                volumeText.Visibility = Visibility.Collapsed;
                volumeSlider.Visibility = Visibility.Collapsed;
                mediaElement.Pause();
                bg_gradient_top.Color = Color.FromArgb(255, 199, 101, 3);
                bg_gradient_mid.Color = Color.FromArgb(255, 199, 101, 3);
                bg_gradient_bot.Color = Colors.Black;
                canvas_gradient_bot.Color = Color.FromArgb(255, 243, 156, 51);
                canvas_gradient_middle.Color = Color.FromArgb(255, 161, 103, 33);
                canvas_gradient_top.Color = Color.FromArgb(255, 0, 0, 0);
            }
            else
            {
                muteButton.Visibility = Visibility.Visible;
                volumeText.Visibility = Visibility.Visible;
                volumeSlider.Visibility = Visibility.Visible;
                mediaElement.Play();
                bg_gradient_top.Color = Color.FromArgb(255, 253, 137, 253);
                bg_gradient_mid.Color = Color.FromArgb(200, 20, 255, 255);
                bg_gradient_bot.Color = Color.FromArgb(150, 0, 255, 100);
                canvas_gradient_top.Color = Color.FromArgb(200, 224, 0, 200);
                canvas_gradient_middle.Color = Color.FromArgb(150, 225, 100, 200);
                canvas_gradient_bot.Color = Color.FromArgb(150, 0, 255, 255);
            }
        }

        private void muteButton_Click(object sender, RoutedEventArgs e)
        {
            mediaElement.Pause();
        }

        //volume sliderin arvon muuttaminen
        private void volumeSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if(kolikkopeli == null)
            {

            }
            else
            {
                mediaElement.Volume = volumeSlider.Value;
            }
        }
    }

}
