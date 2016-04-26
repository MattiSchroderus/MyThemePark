using HookersAndBlackjack.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
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
    public sealed partial class Pankki : Page
    {
        Player player;
        public Pankki()
        {
            this.InitializeComponent();
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is Player)
            {
                player = (Player)e.Parameter;
                //Ui element setups
                RefreshData(); //updates player data to UI
            }
            base.OnNavigatedTo(e);
        }
        /// <summary>
        /// repay button click
        /// </summary>
        private async void repayButton_Click(object sender, RoutedEventArgs e)
        {
            if (player.Money >= 50)
            {
                player.Money -= 50;
                RefreshData();
            }
            else
            {
                MessageDialog messageDialog = new MessageDialog("Not enough money");
                messageDialog.Commands.Add(new UICommand("Ok (feelsbadman)"));
                await messageDialog.ShowAsync();
            }
        }
        /// <summary>
        /// buy money button click
        /// </summary>
        private void buyMoneyButton_Click(object sender, RoutedEventArgs e)
        {
            player.Money += 50;
            RefreshData();
        }
        /// <summary>
        /// buy chips button click
        /// </summary>
        private async void buyChipsButton_Click(object sender, RoutedEventArgs e)
        {
            if (player.Money >= 50)
            {
                player.Money -= 50;
                player.Chips += 100;
                RefreshData();
            }
            else
            {
                MessageDialog messageDialog = new MessageDialog("Not enough money");
                messageDialog.Commands.Add(new UICommand("Ok (feelsbadman)"));
                await messageDialog.ShowAsync();
            }
        }
        private void RefreshData()
        {
            playerTitleText.Text = "Player: " + player.Name;
            moneyBox.Text = "Money: " + player.Money.ToString();
            chipBox.Text = "Chips: " + player.Chips.ToString();
        }
    }
}
