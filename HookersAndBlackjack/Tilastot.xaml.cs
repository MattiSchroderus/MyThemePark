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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HookersAndBlackjack
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Tilastot : Page
    {
        Player player;
        ObservableCollection<Player> players = new ObservableCollection<Player>();
        public Tilastot()
        {
            this.InitializeComponent();


            try
            {
                string[] lines = File.ReadAllLines("Assets/Players.txt");
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

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            player = ((sender as ComboBox).SelectedItem as Player);
            MoneyBox.Text = "Money: " + player.Money.ToString();
            ChipBox.Text = "Chips: " + player.Chips.ToString();
        }
    }
}
