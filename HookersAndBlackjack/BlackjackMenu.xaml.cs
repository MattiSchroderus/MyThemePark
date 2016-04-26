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
    public sealed partial class BlackjackMenu : Page
    {
        public Player player;

        public BlackjackMenu()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is Player)
            {
                player = (Player)e.Parameter;
                //Ui element setups
                DebugScreen.Text = "Player: " + player.Name + "\n" + DebugScreen.Text;
            }
            base.OnNavigatedTo(e);
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            Table House = new Table();
            DebugScreen.Text += PackNumber.SelectedValue.ToString();
            DebugScreen.Text += StakeSize.SelectedItem.ToString();
            try
            {
                House.PackNumber = int.Parse(PackNumber.SelectedItem.ToString());
                House.StakeSize = int.Parse(StakeSize.SelectedItem.ToString());
                House.PlayerList.Add(player);

                // Lisää ja navigoi uudelle sivulle. Tiedetään että toimii.
                // Laitetaan tänne jotta debugaus toimii järjestelmällisesti.
                this.Frame.Navigate(typeof(Blackjack), House);
            }
            catch
            {
                DebugScreen.Text += "Error while trying to save values from ComboBoxes\n";
            }
        }
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
       
    }
}