using HookersAndBlackjack.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
        List<Player> players = new List<Player>();
        private Player playertemp;

        public Pankki()
        {
            this.InitializeComponent();
        }

        private async void backButton_Click(object sender, RoutedEventArgs e)
        {
            //save players data to playrs.txt
            await UpdatePlayerData(); //saves player data
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
        private async Task UpdatePlayerData()
        {
            try
            {
                PlayerRefresh(); //makes list of players

                //create file in public folder

                int index = players.FindIndex(f => f.Name == player.Name); //find player index
                players.RemoveAt(index); //remove player from list
                //adds other players to player list
                foreach (Player pla in players)
                {
                    players.Add(pla);
                }
                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                StorageFile file = await storageFolder.CreateFileAsync("temp.txt", CreationCollisionOption.ReplaceExisting);

                string text = ""; //Text
                foreach (Player player in players)
                {
                    text += Environment.NewLine + player.Name + " " + player.Money.ToString() + " " + player.Chips.ToString(); //add old players to text string
                }
                text += Environment.NewLine + player.Name + " " + player.Money.ToString() + " " + player.Chips.ToString(); //add players new data to text string
                //write string to created file
                await FileIO.WriteTextAsync(file, text); //write playerdata to file
                //get Assets folder
                StorageFolder appInstalledFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
                StorageFolder assetsFolder = await appInstalledFolder.GetFolderAsync("Assets");
                await file.MoveAsync(assetsFolder, "players.txt", NameCollisionOption.ReplaceExisting); //move file from public folder to Assets folder
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Some exception happened!");
                Debug.WriteLine(ex.ToString() + ":Updateplayerdata error");
            }
        }

        private void PlayerRefresh()
        {
            try
            {
                players.Clear(); //clears old playerlist
                string[] lines = File.ReadAllLines("Assets/players.txt"); //get lines from players.txt
                foreach (string pair in lines) //pair == {name} {money + chips}
                {
                    int position = pair.IndexOf(" "); //name ends in " "
                    if (position < 0) { continue; }
                    else
                    {
                        string name = pair.Substring(0, position); //name ends in " "
                        string moneychips = pair.Substring(position + 1); //money and chips are rest
                        int position2 = moneychips.IndexOf(" "); //moneychips == {money} {chips}
                        if (position2 < 0) { continue; }
                        else
                        {
                            int money = int.Parse(moneychips.Substring(0, position2));
                            int chips = int.Parse(moneychips.Substring(position2 + 1));
                            players.Add(playertemp = new Player(name)); //Add player to players list
                            playertemp.Money = money; //give money to player
                            playertemp.Chips = chips;
                        }
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Debug.WriteLine("error: File Not Found");
            }
        }

        private async void chipsToMoneyButton_Click(object sender, RoutedEventArgs e)
        {
            if (player.Chips >= 100)
            {
                player.Money += 50;
                player.Chips -= 100;
                RefreshData();
            }
            else
            {
                MessageDialog messageDialog = new MessageDialog("Not enough chips");
                messageDialog.Commands.Add(new UICommand("Ok"));
                await messageDialog.ShowAsync();
            }
        }
    }
}
