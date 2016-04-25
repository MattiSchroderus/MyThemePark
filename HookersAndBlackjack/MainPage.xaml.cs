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

        public MainPage()
        {
            this.InitializeComponent();

            PlayerRefresh();
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

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            player = ((sender as ComboBox).SelectedItem as Player);
            if(player != null)
            {
            MoneyBox.Text = "Money: " + player.Money.ToString();
            ChipBox.Text = "Chips: " + player.Chips.ToString();
            }
            else { Debug.WriteLine("Error: combobox error"); }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            newProfileStackPanel.Visibility = Visibility.Visible;
        }

        private void newProfileTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            newProfileTextBox.Text = "";
        }

        private async void newProfileButton_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog messageDialog = new MessageDialog("");
            messageDialog.Commands.Add(new UICommand("Ok"));
            if (newProfileTextBox.Text != "" && newProfileTextBox.Text != "Profile name...")
            {
                Player newplayer = new Player(newProfileTextBox.Text);
                await AddPlayer(newplayer);
                PlayerRefresh();
                messageDialog.Content = "Profile " + newProfileTextBox.Text + " created";
                await messageDialog.ShowAsync();
            }
            else
            {
                messageDialog.Content = "Error: Give profile name";
                await messageDialog.ShowAsync();
            }
        }

        /// <summary>
        /// Refresh playerdata from players.txt
        /// </summary>
        public void PlayerRefresh()
        {
            try
            {
                players.Clear();
                string[] lines = File.ReadAllLines("Assets/players.txt");
                foreach (string pair in lines)
                {
                    int position = pair.IndexOf(" ");
                    if (position < 0) { continue; }
                    else
                    {
                        string name = pair.Substring(0, position);
                        int money = int.Parse(pair.Substring(position + 1));
                        players.Add(player = new Player(name));
                        player.Money = money;
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Debug.WriteLine("error: File Not Found");
            }
        }
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

                //write sring to created file
                await FileIO.WriteTextAsync(file, text);

                //get assets folder
                StorageFolder appInstalledFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
                StorageFolder assetsFolder = await appInstalledFolder.GetFolderAsync("Assets");

                //move file from public folder to assets
                await file.MoveAsync(assetsFolder, "players.txt", NameCollisionOption.ReplaceExisting);

            }
            catch (Exception ex)
            {
                //Console.WriteLine("Some exception happened!");
                Debug.WriteLine(ex.ToString() + ":Addplayer error");
            }
        }
    }
}