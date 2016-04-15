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
using Windows.UI.Popups;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

using HookersAndBlackjack.Model;

namespace HookersAndBlackjack
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Blackjack : Page
    {
        private Table House = new Table();
        private bool DebugBool = false;

        public Blackjack()
        {
            this.InitializeComponent();
            // House.Start();
            House.Dummy.Intelligence = false;
            for (int i = 0; i < 2; i++)
            {
                // Jos laittaisit loopin Foe luokan sisään. Jokainen pelaaja 
                // on listassa. Lista sekoitetaan <- Pelaaja järjestys muuttuu
                // Kun lista menee pelaajan kohalle
                Table h;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is Table)
            { DebugScreen.Text += "Jotain Tapahtuu"; }
            Table house = (Table)e.Parameter;
            House.PackNumber = house.PackNumber;
            House.StakeSize = house.StakeSize;
            try
            {
                House.Deal();
                house.Checker();
            }
            catch
            {
                DebugScreen.Text += "Could not deal\n";
            }
            base.OnNavigatedTo(e);
        }
        // Deal buttoni on vain debugausta varten
        private void Deal_Click(object sender, RoutedEventArgs e)
        {
            House.Deal();
            House.Checker();
        }

        private void DebugButton_Click(object sender, RoutedEventArgs e)
        {
            if (DebugBool == false)
            {
                try
                {
                    DebugScreen.Text = House.DebugMessage;
                    DebugBool = true;
                }
                catch
                {
                    DebugScreen.Text = "No Data";
                    DebugBool = true;
                }
            }
            else
            {
                DebugScreen.Text = "";
                DebugBool = false;
            }
        }

        private void Hit_Click(object sender, RoutedEventArgs e)
        {
            House.Hit();
            House.Checker();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // get root frame (which shows pages)
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