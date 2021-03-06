﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HookersAndBlackjack.Model
{
    /// <summary>
    /// Kolikkopeli
    /// </summary>
    public class KPeli
    {
        //Wheels
        Wheel wheel1 = new Wheel() { LocationX = 0 , LocationY = 0};
        Wheel wheel2 = new Wheel() { LocationX = 86, LocationY = 0};
        Wheel wheel3 = new Wheel() { LocationX = 172, LocationY = 0};
        Wheel wheelDouble = new Wheel() { LocationX = 260, LocationY = 0};
        //Wheel list
        List<Wheel> wheels = new List<Wheel>();

        int combination = 0;
        public int winnings = 0;
        int wheelnumber = 0;
        Random rnd = new Random();
        List<Player> players = new List<Player>();
        
        //Timer settings
        int timesTicked = 1;
        int timesToTick = 3;
        DispatcherTimer dispatcherTimer;

        //KPeli settings
        private Canvas canvas;
        int bet;
        Player player;
        TextBlock tbMoney;
        TextBlock tbLog;
        Button button_play;
        Button button_Double;
        Slider slider_Bet;
        
        //Double settings
        private int doublebool;
        private Player playertemp;

        /// <summary>
        /// KPeli
        /// </summary>
        public KPeli(Canvas canvas, TextBlock tbMoney, TextBlock tbLog, Button button_play,Slider slider_Bet,Button button_Double)
        {
            //UI elements
            this.canvas = canvas;
            this.tbMoney = tbMoney;
            this.tbLog = tbLog;
            this.button_play = button_play;
            this.slider_Bet = slider_Bet;
            this.button_Double = button_Double;

            //Add wheels to List
            wheels.Add(wheel1);
            wheels.Add(wheel2);
            wheels.Add(wheel3);
            
            //Set wheel locations
            foreach(Wheel wheel in wheels)
            {
                wheel.SetLocation();
            }
            wheelDouble.SetLocation();
            
            //Add wheels to canvas
            foreach (Wheel wheel in wheels)
            {
                canvas.Children.Add(wheel);
            }
            canvas.Children.Add(wheelDouble);    
        }

        /// <summary>
        /// Play-method
        /// </summary>
        public void Play(int bet, Player player)
        {
            //player, bet
            this.bet = bet;
            this.player = player;
            

            //Change images --> [?]
            foreach (Wheel wheel in wheels)
            {
                wheel.ImageChange(9);
            }
            
            DispatcherTimerSetup(); //Setup Timer
        }

        /// <summary>
        /// Play-timerSetup
        /// </summary>
        public void DispatcherTimerSetup()
        {
            //Timer setups
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 400);
            Debug.WriteLine("dispatcherTimer.IsEnabled = " + dispatcherTimer.IsEnabled + "\n");
            Debug.WriteLine("Calling dispatcherTimer.Start()\n");
            
            //get combination
            combination = wheel1.Spin();
            Debug.WriteLine(combination.ToString());
            
            //start timer
            dispatcherTimer.Start();
            Debug.WriteLine("dispatcherTimer.IsEnabled = " + dispatcherTimer.IsEnabled + "\n");
        }

        /// <summary>
        /// Play-Timer ticks
        /// </summary>
        void dispatcherTimer_Tick(object sender, object e)
        {
            wheels[wheelnumber].ImageChange(int.Parse(combination.ToString().Substring(wheelnumber,1))); //Change images

            //next wheel, next tick
            wheelnumber++;
            timesTicked++;

            //Final tick
            if (timesTicked > timesToTick)
            {
                //stop timer
                dispatcherTimer.Stop();
                Debug.WriteLine("dispatcherTimer.IsEnabled = " + dispatcherTimer.IsEnabled + "\n");
                
                winnings = CheckWin(bet); //Get winnings

                player.Money += winnings; //Add winnings to player.Money

                TextMoney(tbMoney); //Money textblock = player.Money
                //Add log texts
                if (winnings > 0)
                {
                    LogAdd("You won " + winnings.ToString(),tbLog);
                    button_Double.IsEnabled = true;
                }
                else
                {
                    LogAdd("You lost " + ((-1) * winnings).ToString(),tbLog);
                }
                //Ui settings
                button_play.IsEnabled = true;
                slider_Bet.Maximum = player.Money;
                UpdatePlayerData(); //Save player money to players.txt
            }
        }
        /// <summary>
        /// Player data saver
        /// </summary>
        private async void UpdatePlayerData()
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

        /// <summary>
        /// Reset KPeli settings
        /// </summary>
        public void Reset()
        {
            combination = 0;
            timesTicked = 1;
            timesToTick = 3;
            wheelnumber = 0;
            winnings = 0;
            wheelDouble.ImageChange(9); //wheel images --> [?]
        }

        /// <summary>
        /// Change imagepaths
        /// </summary>
        internal void ChangePath(int pathnumber)
        {
            foreach(Wheel wheel in wheels)
            {
                wheel.ChangePath(pathnumber);
            }
            wheelDouble.ChangePath(pathnumber);
        }

        /// <summary>
        /// Change MoneyTextBlock text --> player.Money
        /// </summary>
        public void TextMoney(TextBlock tb)
        {
            tb.Text = "Money: " + player.Money.ToString();
        }

        /// <summary>
        /// Add text to LogTextBlock
        /// </summary>
        public void LogAdd(string text,TextBlock tbLog)
        {
            tbLog.Text = text + Environment.NewLine + tbLog.Text;
        }

        /// <summary>
        /// Double, 1=yes 0=no
        /// </summary>
        public void Double()
        {
            wheelDouble.ImageChange(9);
            DispatcherTimerSetupDouble();
        }
        
        /// <summary>
        /// Double Timer setup
        /// </summary>
        public void DispatcherTimerSetupDouble()
        {
            //timer setups
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_TickDouble;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 400);
            Debug.WriteLine("dispatcherTimer.IsEnabled = " + dispatcherTimer.IsEnabled + "\n");
            Debug.WriteLine("Calling dispatcherTimer.Start()\n");
            
            doublebool = wheel1.DoubleSpin(); //Get double, 1=win, 0=lose

            //start timer
            dispatcherTimer.Start();
            Debug.WriteLine("dispatcherTimer.IsEnabled = " + dispatcherTimer.IsEnabled + "\n");
        }

        /// <summary>
        /// Double Timer ticks
        /// </summary>
        void dispatcherTimer_TickDouble(object sender, object e)
        {
            wheelDouble.ImageChange(doublebool); //Change doublewheel image

            //stop timer
            dispatcherTimer.Stop();
            Debug.WriteLine("dispatcherTimer.IsEnabled = " + dispatcherTimer.IsEnabled + "\n");

            //Add winnings to player.Money
            if(doublebool == 3)
            {
                player.Money -= winnings; //takes old winnings from player.Money
                winnings = winnings*2; //doubles winnigns
            }
            else
            {
                winnings = -(winnings);
            }

            player.Money += winnings; //Add new winnings to player.Money
            TextMoney(tbMoney); //Add text to MoneyTextBlock

            //Add text to LogTextBlock
            if (winnings > 0)
            {
                LogAdd("You won " + winnings.ToString(), tbLog);
            }
            else
            {
                LogAdd("You lost " + ((-1) * winnings).ToString(), tbLog);
                button_Double.IsEnabled = false;
            }
            //ui element settings
            slider_Bet.Maximum = player.Money;
            UpdatePlayerData();
        }

        /// <summary>
        /// Checks how much player won
        /// </summary>
        public int CheckWin(int bet)
        {
            switch (combination)
            {
                case 111: return bet * 10;
                case 222: return bet * 5;
                case 333: return bet * 4;
                case 444: return bet * 2;
                case 555: return bet;
                default: return -(bet);
            }
        }
        /// <summary>
        /// gets player data from players.txt
        /// </summary>
        public void PlayerRefresh()
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
    }
}