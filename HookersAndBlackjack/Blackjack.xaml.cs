using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        private SemaphoreSlim signal = new SemaphoreSlim(0, 1);

        private Table House = new Table();
        public Random rand = new Random();
        public Player Dummy = new Player("Dummy");
        private bool DebugBool = false;
        // Luo taski joka seuraa ListBufferia
        public string ListBuffer = "";

        // Konstruktori tapahtuu ensin
        public Blackjack()
        {
            this.InitializeComponent();
        }

        // Tapahtuu konstruktorin jälkeen. Tämän jälkeen tulee muut.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Table house = (Table)e.Parameter;
            House.PackNumber = house.PackNumber;
            House.StakeSize = house.StakeSize;
            House.PlayerList.AddRange(house.PlayerList);
            try
            {
                Task t = Task.Run(() => Rotator(House));
            }
            catch(Exception ex)
            {
                DebugScreen1.Text += "Exception occured: " + ex.ToString();
            }
        }

        // Deal buttoni on vain debugausta varten
        private void Deal_Click(object sender, RoutedEventArgs e)
        {
            House.Deal();
        }

        private void DebugButton_Click(object sender, RoutedEventArgs e)
        {
            if (DebugBool == false)
            {
                try
                {
                    DebugScreen1.Text = House.DebugMessage;
                    DebugBool = true;
                }
                catch
                {
                    DebugScreen1.Text = "No Data";
                    DebugBool = true;
                }
            }
            else
            {
                DebugScreen1.Text = "";
                DebugBool = false;
            }
        }

        // Lisää yhden kortin pelaajalle
        private void Hit_Click(object sender, RoutedEventArgs e)
        {
            // Tämä muuttaa ListBufferin arvoa.
            ListBuffer = "Hit";
            // Tämä vapauttaa rotaattorin
            signal.Release();
        }

        // Lopettaa pelaajan vuoron
        private void Pass_Click(object sender, RoutedEventArgs e)
        {
            // Tämä muuttaa ListBufferin arvoa.
            ListBuffer = "Pass";
            // Tämä vapauttaa rotaattorin
            signal.Release();
        }

        // Palauttaa aiemmalle sivulle
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

        private Table Rotator(Table House)
        {
            // Botit
            for (int i = 0; i < 3; i++)
            {
                Player w = new Player("");
                w.Intelligence = false;
                w.Uhkarohkeus = 20;
                House.PlayerList.Add(w);
            }

            // Korttien jako
            House.Deal();

            // Fisher-Yates sekoitus
            int n = House.PlayerList.Count;
            while (n > 1)
            {
                n--;
                int k = rand.Next(n + 1);
                Dummy = House.PlayerList[k];
                House.PlayerList[k] = House.PlayerList[n];
                House.PlayerList[n] = Dummy;
            }

            // Tsekkaus looppi
            for (int i = 0; i < House.PlayerList.Count; i++)
            {
                // Pelaajan looppi
                if (House.PlayerList[i].Intelligence == true)
                {
                    // Tämä luo loopin joka odottaa että pelaaja lopettaa vuoronsa
                    bool b = true;
                    while (b == true)
                    {
                        // Tämän pitäisi odotella nappulan painallusta
                        Task task = Task.Run(() => Waiter());
                        task.Wait();
                        switch (ListBuffer)
                        {
                            case "Hit":
                                House.DebugMessage += "Tap dat thread\n";
                                House.Hit(i);
                                ListBuffer = "";
                                break;
                            case "Pass":
                                ListBuffer = "";
                                b = false;
                                break;
                            default:
                                ListBuffer = "";
                                break;
                        }
                    }
                }

                // Bottien looppi
                else
                {
                    bool b = true;
                    while (b == true)
                    {
                        House.PlayerList[i].RiskMeter(House.PackNumber);
                        House.PlayerList[i].Thinking();
                        switch (House.PlayerList[i].vastaus)
                        {
                            case "Hit":
                                House.Hit(i);
                                House.DebugMessage += "Foe#" + i + " used hit\n";
                                break;
                            case "Pass":
                                House.DebugMessage += "Foe#" + i + " used pass\n";
                                b = false;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            int q = 0;
            int x = 0;
            int z = 0;
            for (int i = 0; i < House.PlayerList.Count; i++)
            {
                q = House.PlayerList[i].CardTotal();
                if (q > z)
                {
                    x = i;
                    z = q;
                }
            }
            Winner(x);
            // Aika varma että tätä ei tarvita.
            return House;
        }

        private async void Winner(int x)
        {
            MessageDialog messageDialog;
            if (House.PlayerList[x].Intelligence == true)
            {
                 messageDialog = new MessageDialog("And the winner is Player");
            }
            else messageDialog = new MessageDialog("And the winner is Foe#" + x);
            messageDialog.Commands.Add(new UICommand("Ok"));
            await messageDialog.ShowAsync();
        }

        // Tätä tarvitaan odotteluun
        private async void Waiter()
        {
            await signal.WaitAsync();
        }
    }
}