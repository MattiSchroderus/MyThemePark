﻿using System;
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
                for (int i = 0; i < House.PlayerList.Count; i++)
                {
                    House.PlayerList[i].Checker();
                }
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

        private void Go_Click(object sender, RoutedEventArgs e)
        {
            // Tämä vapauttaa rotaattorin
            signal.Release();
        }

        private void Hit_Click(object sender, RoutedEventArgs e)
        {
            // Tämä muuttaa ListBufferin arvoa.
            ListBuffer = "Hit";
            // Debug viestejä:
            DebugScreen.Text += "Hit stuff\n";
            // Tämä vapauttaa rotaattorin
            signal.Release();
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

        private Table Rotator(Table House, string DebugStr)
        {
            Task task0 = Task.Run(() => Waiter());
            task0.Wait();
            // Luo yhden pelaajan. Loput on botteja
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

            // Tsekkaus looppi
            int xeon = House.PlayerList.Count;
            for (int i = 0; i < xeon; i++)
            {
                House.PlayerList[i].RiskMeter(House.PackNumber);
                // Voitaisiin myös tehä niin että pelaajien kohdalla
                // uhkarohkeus on null. silloin switchi siirtyisi
                // default kohtaan ja suorittaisi siellä olevat komennot
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
                                b = false;
                                break;
                            default:
                                break;
                        }
                    }
                }
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
            House.DebugMessage = DebugStr;
            return House;
        }

        private async void Waiter()
        {
            await signal.WaitAsync();
        }
    }
}