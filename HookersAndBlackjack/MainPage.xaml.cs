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
            
            try
            {
                string[] lines = File.ReadAllLines("Assets/Players.txt");
                foreach(string pair in lines)
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
            MoneyBox.Text = "Money: " + player.Money.ToString();
            ChipBox.Text = "Chips: " + player.Chips.ToString();
        }
    }
}