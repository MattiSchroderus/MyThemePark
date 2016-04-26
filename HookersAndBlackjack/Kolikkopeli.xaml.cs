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
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HookersAndBlackjack
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Kolikkopeli : Page
    {
        //settings
        private KPeli kolikkopeli;
        Player player;

        public static double CanvasWidth; //canvas W for wheels
        public static double CanvasHeight; //canvas H for wheels

        /// <summary>
        /// Kolikkopeli
        /// </summary>
        public Kolikkopeli()
        {
            this.InitializeComponent();
            //kolikkopeli setup
            CanvasWidth = MyCanvas.Width;
            CanvasHeight = MyCanvas.Height;
            kolikkopeli = new KPeli(MyCanvas, textBlock_Money, textBlock_Log, button_Play, slider_Bet,button_Double);
            kolikkopeli.ChangePath(0);
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is Player)
            {
                player = (Player)e.Parameter;
                //Ui element setups
                textBlock_Log.Text = "Player: " + player.Name + "\n" + textBlock_Log.Text;
                textBlock_Money.Text = "Money: " + player.Money.ToString();
                slider_Bet.Maximum = player.Money; //slider max to player.Money
            }
            base.OnNavigatedTo(e);
        }

        /// <summary>
        /// Play button click
        /// </summary>
        private void button_Play_Click(object sender, RoutedEventArgs e)
        {
            int bet = int.Parse(textBlock_Bet.Text); //Get bet
            button_Double.IsEnabled = false; //Disable Double button

            //Check if player has enough money
            if (player.Money >= bet && bet != 0)
            {
                button_Play.IsEnabled = false; //Disable play-button
                kolikkopeli.Reset(); //Reset kolikkopeli-settings
                kolikkopeli.Play(bet, player); //play
            }
            //...if not enough money
            else
            {
                kolikkopeli.LogAdd("Not enough money!", textBlock_Log); //Add text to LogTextBlock
            }
        }

        /// <summary>
        /// Back button click
        /// </summary>
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

        /// <summary>
        /// Double button click
        /// </summary>
        private void button_Double_Click(object sender, RoutedEventArgs e)
        {
            kolikkopeli.Double(); //Double
        }

        /// <summary>
        /// Language Select click
        /// </summary>
        private void languageButton_Click(object sender, RoutedEventArgs e)
        {
            //if Suomi
            if (sender.Equals(languageButton_Suomi))
            {
                //Collapse media buttons
                muteButton.Visibility = Visibility.Collapsed;
                volumeText.Visibility = Visibility.Collapsed;
                volumeSlider.Visibility = Visibility.Collapsed;
                mediaElement.Pause();
                //Change colors
                bg_gradient_top.Color = Color.FromArgb(255, 199, 101, 3);
                bg_gradient_mid.Color = Color.FromArgb(255, 199, 101, 3);
                bg_gradient_bot.Color = Colors.Black;
                canvas_gradient_bot.Color = Color.FromArgb(255, 243, 156, 51);
                canvas_gradient_middle.Color = Color.FromArgb(255, 161, 103, 33);
                canvas_gradient_top.Color = Color.FromArgb(255, 0, 0, 0);
                kolikkopeli.ChangePath(0); //Change wheel images
                image_Prizes.Source = new BitmapImage(new Uri(BaseUri, ("/Assets/taulu.png"))); //Change imageboard image
                title.Visibility = Visibility.Visible;
                title2.Visibility = Visibility.Collapsed;
            }
            //else Dank
            else
            {
                //Enable media buttons
                muteButton.Visibility = Visibility.Visible;
                volumeText.Visibility = Visibility.Visible;
                volumeSlider.Visibility = Visibility.Visible;
                mediaElement.Play();
                //Change colors
                bg_gradient_top.Color = Color.FromArgb(255, 253, 137, 253);
                bg_gradient_mid.Color = Color.FromArgb(200, 20, 255, 255);
                bg_gradient_bot.Color = Color.FromArgb(150, 0, 255, 100);
                canvas_gradient_top.Color = Color.FromArgb(200, 224, 0, 200);
                canvas_gradient_middle.Color = Color.FromArgb(150, 225, 100, 200);
                canvas_gradient_bot.Color = Color.FromArgb(150, 0, 255, 255);
                kolikkopeli.ChangePath(1); //Change wheel images
                image_Prizes.Source = new BitmapImage(new Uri(BaseUri, ("/Assets/dank/taulu.png"))); //Change imageboard image
                title.Visibility = Visibility.Collapsed;
                title2.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Mute button click
        /// </summary>
        private void muteButton_Click(object sender, RoutedEventArgs e)
        {
            mediaElement.Pause(); //Pause music
        }

        /// <summary>
        /// Volume slider moving
        /// </summary>
        private void volumeSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if(kolikkopeli == null) { }
            else
            {
                mediaElement.Volume = volumeSlider.Value; //Changes music volume to Slider value
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            player.Money += 50;
            slider_Bet.Maximum = player.Money;
        }
    }
}
