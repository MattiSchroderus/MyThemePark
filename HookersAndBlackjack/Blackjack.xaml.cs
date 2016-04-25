using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
// Tsekkaa näiden tarpeellisuus
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
        private AutoResetEvent autoResetEvent;
        private SemaphoreSlim signal = new SemaphoreSlim(0, 1);

        private Table House = new Table();
        public Random rand = new Random();
        private bool DebugBool = false;
        // Luo taski joka seuraa ListBufferia
        public string ListBuffer = "";
        public string DebugStr = "";

        // Konstruktori tapahtuu ensin
        public Blackjack()
        {
            this.InitializeComponent();
            // House.Start();
            House.Dummy.Intelligence = false;
        }

        // Tapahtuu konstruktorin jälkeen. Tämän jälkeen tulee muut.
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
                House.Checker();
            }
            catch
            {
                DebugScreen.Text += "Could not deal\n";
            }
            base.OnNavigatedTo(e);
            House.tester = false;
            DebugScreen.Text += "" + House.tester;
            try
            {
                Task t = Task.Factory.StartNew(() => Rotator(House, DebugStr));
                DebugScreen.Text += "" + House.tester;
            }
            catch(Exception ex)
            {
                DebugScreen.Text += "Exception occured: " + ex.ToString();
            }
            DebugScreen.Text += "Kokeilu";
            DebugScreen.Text += DebugStr;
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
            ListBuffer = "Hit";
            DebugScreen.Text += DebugStr;
            //autoResetEvent.Set();
            signal.Release();
            // Nämä on ehkä turhia:
            /*
            House.Hit();
            House.Checker();
            */
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

        // Tämä on asyncroninen tehtävä joka odottaa että ListBuffer muuttuja
        // muuttuu. Kun ListBuffer muuttuu se ottaa uuden arvon ja lähettää
        // sen eteen päin. Changer muutettu ListBufferiksi.
        // TaskMasteri on ehkä turha.
        private void TaskMaster()
        {
            throw new NotImplementedException();
        }

        private async void Rotator(Table House, string DebugStr)
        {
            DebugStr += "Imma thread now\n";
            autoResetEvent = new AutoResetEvent(false);
            // Taaskin intelligence arvon voisi tiputtaa ja tehdä jostain
            // kokonais luvusta pelaaja arvon. Vaikka nullista.
            Foe p = new Foe(true, 0);
            House.PlayerList.Add(p);
            for (int i = 0; i < 4; i++)
            {
                Foe w = new Foe(false, 10);
                House.PlayerList.Add(w);
            }
            // Fisher-Yates sekoitus
            ushort n = (ushort)House.PlayerList.Count;
            while (n > 1)
            {
                n--;
                int k = rand.Next(n + 1);
                House.Dummy = House.PlayerList[k];
                House.PlayerList[k] = House.PlayerList[n];
                House.PlayerList[n] = House.Dummy;
            }
            // Checkkaus looppi
            foreach (Foe f in House.PlayerList)
            {
                // Voitaisiin myös tehä niin että pelaajien kohdalla
                // uhkarohkeus on null. silloin switchi siirtyisi
                // default kohtaan ja suorittaisi siellä olevat komennot
                if (f.Intelligence == true)
                {
                    House.tester = true;
                    DebugStr += "Dem players\n";
                    // Pupup on yks tapa kysyä pelaajan inputtia.
                    // Tosin omasta mielestä tämä ei ole niin hieno
                    // Tapa ottaa inputtia, koska se pysäyttää loopin.
                    // House.Popup();
                    //autoResetEvent.WaitOne();
                    // Tämän pitäisi odotella nappulan painallusta
                    await signal.WaitAsync();
                    DebugStr += "Back in dreads\n";
                    switch (ListBuffer)
                    {
                        case "Hit":
                            DebugStr += "Tap dat thread\n";
                            House.Hit();
                            House.Checker();
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    switch (f.Uhkarohkeus)
                    {
                        case 0:
                            House.Pass();
                            break;
                        case 10:
                            House.Hit();
                            break;
                        default:
                            // Tänne pelaajan jutut. Tai varsinaisesti ottaen
                            // tänne tulee pelaajan valmiiksi valitsema metodi.
                            // Jos tuon metodin voisi vaikka tallentaa.
                            break;
                    }
                }
            }
            House.DebugMessage = DebugStr;
        }

        private async void Waiter()
        {

        }

        private void Pass_Click(object sender, RoutedEventArgs e)
        {
            // Tämä muuttaa ListBufferin arvoa.
            ListBuffer = "Hit";
            // Tämä vapauttaa rotaattorin
            signal.Release();
            // Debug viestejä:
            DebugScreen.Text += "Pass has been pressed\n";
        }
    }
}