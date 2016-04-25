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
using System.Diagnostics;
using System.Collections.ObjectModel;
using Windows.UI.Popups;
using Windows.Storage;
using System.Threading.Tasks;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace HookersAndBlackjack
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Player player;
        ObservableCollection<Player> players = new ObservableCollection<Player>();
        List<Player> playerlist = new List<Player>();

        public MainPage()
        {
            this.InitializeComponent();

            PlayerRefresh();
            Debug.WriteLine(Windows.ApplicationModel.Package.Current.InstalledLocation.Path + "---------------------------------------");
        }

        private void KolikkopeliButton_Click(object sender, RoutedEventArgs e)
        {
            // Lisää ja navigoi uudelle sivulle.
            this.Frame.Navigate(typeof(Kolikkopeli));
        }

        private void BlackjackButton_Click(object sender, RoutedEventArgs e)
        {
            // lisää ja navigoi uudelle sivulle.
            this.Frame.Navigate(typeof(BlackjackMenu));
        }

        private void TilastoButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Tilastot));
        }

        /// <summary>
        /// when combobox selection is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            player = ((sender as ComboBox).SelectedItem as Player); //player is combobox selection
            //if selection is player
            if (player != null)
            {
            MoneyBox.Text = "Money: " + player.Money.ToString();
            ChipBox.Text = "Chips: " + player.Chips.ToString();
            }
            else { Debug.WriteLine("Error: combobox error"); } //if not
        }
        /// <summary>
        /// New... button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newButton_Click(object sender, RoutedEventArgs e)
        {
            newProfileStackPanel.Visibility = Visibility.Visible; //show new profile block, button
        }
        /// <summary>
        /// when new profile text box is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newProfileTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            newProfileTextBox.Text = ""; //erase block text
        }

        /// <summary>
        /// NewProfile Button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void newProfileButton_Click(object sender, RoutedEventArgs e)
        {
            //create new message dialog box
            MessageDialog messageDialog = new MessageDialog("");
            messageDialog.Commands.Add(new UICommand("Ok"));
            //check if new profile name is empty or default text
            if (newProfileTextBox.Text != "" && newProfileTextBox.Text != "Profile name...") //if not
            {
                //Adds players to temp playerlist
                playerlist.Clear();
                foreach (Player pla in players)
                {
                    playerlist.Add(pla);
                }
                int findbool = FindPlayer();
                //checks if profile name uses spaces
                if(newProfileTextBox.Text.Contains(" ") == true)
                {
                    messageDialog.Content = "Error: No names with spaces";
                    await messageDialog.ShowAsync();
                }
                //checks if profile name taken
                else if (findbool == 1) //if not
                {
                    Player newplayer = new Player(newProfileTextBox.Text); //new player
                    await AddPlayer(newplayer); //adds new player to players.txt
                    PlayerRefresh(); //refresh playerlist
                    messageDialog.Content = "Profile " + newProfileTextBox.Text + " created"; //show message
                    await messageDialog.ShowAsync();
                    newProfileStackPanel.Visibility = Visibility.Collapsed;
                }
                else //if yes
                {
                    messageDialog.Content = "Error: Name taken";
                    await messageDialog.ShowAsync();
                }
            }
            else //if yes
            {
                //show error
                messageDialog.Content = "Error: Give profile name";
                await messageDialog.ShowAsync();
            }
        }
        /// <summary>
        /// Checks if profile name used
        /// </summary>
        /// <returns></returns>
        private int FindPlayer()
        {
            try
            {
                //Checks if name in playerlist
                int index = playerlist.FindIndex(f => f.Name == newProfileTextBox.Text);
                if (index == -1)
                {
                    return 1;
                }
                else return 0;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return 1;
            }
        }

        /// <summary>
        /// Refresh playerdata from players.txt
        /// </summary>
        public void PlayerRefresh()
        {
            try
            {
                players.Clear(); //clears old playerlist
                string[] lines = File.ReadAllLines("Assets/players.txt"); //get lines from players.txt
                foreach (string pair in lines) //pair == {name} {money}
                {
                    int position = pair.IndexOf(" "); //name ends in " "
                    if (position < 0) { continue; }
                    else
                    {
                        string name = pair.Substring(0, position); //name ends in " "
                        int money = int.Parse(pair.Substring(position + 1)); //money is rest
                        players.Add(player = new Player(name)); //Add player to players list
                        player.Money = money; //give money to player
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Debug.WriteLine("error: File Not Found");
            }
        }
        /// <summary>
        /// Add player to players.txt
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public async Task AddPlayer(Player player)
        {
            try
            {
                //create file in public folder
                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                StorageFile file = await storageFolder.CreateFileAsync("temp.txt", CreationCollisionOption.ReplaceExisting);

                //Text
                string text = File.ReadAllText("Assets/players.txt");
                text += Environment.NewLine + player.Name + " " + player.Money.ToString();

                //write string to created file
                await FileIO.WriteTextAsync(file, text);

                //get Assets folder
                StorageFolder appInstalledFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
                StorageFolder assetsFolder = await appInstalledFolder.GetFolderAsync("Assets");

                //move file from public folder to Assets folder
                await file.MoveAsync(assetsFolder, "players.txt", NameCollisionOption.ReplaceExisting);
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Some exception happened!");
                Debug.WriteLine(ex.ToString() + ":Addplayer error");
            }
        }
        /// <summary>
        /// Delete... button click , deletes selected profile
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog messageDialog = new MessageDialog("Delete profile '" + player.Name + "'?");
            messageDialog.Commands.Add(new UICommand("YES") {Id = 0 });
            messageDialog.Commands.Add(new UICommand("NO") { Id = 1 });
            messageDialog.DefaultCommandIndex = 0;
            messageDialog.CancelCommandIndex = 1;
            var result = await messageDialog.ShowAsync();
            if (result.Label == "YES")
            {
                playerlist.Clear();
                foreach (Player pla in players)
                {
                    playerlist.Add(pla);
                }
                int index = playerlist.FindIndex(f => f.Name == player.Name);
                playerlist.RemoveAt(index);
                string data = "";
                foreach (Player player in playerlist)
                {
                    data += player.Name + " " + player.Money + Environment.NewLine;
                }
                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                StorageFile file = await storageFolder.CreateFileAsync("temp.txt", CreationCollisionOption.ReplaceExisting);

                //write string to created file
                await FileIO.WriteTextAsync(file, data);

                //get Assets folder
                StorageFolder appInstalledFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
                StorageFolder assetsFolder = await appInstalledFolder.GetFolderAsync("Assets");

                //move file from public folder to Assets folder
                await file.MoveAsync(assetsFolder, "players.txt", NameCollisionOption.ReplaceExisting);
                PlayerRefresh();
            }
            else { }
        }
    }
}