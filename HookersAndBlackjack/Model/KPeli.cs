using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HookersAndBlackjack.Model
{
    public class KPeli
    {
        Wheel wheel1 = new Wheel() { LocationX = 0 , LocationY = 0};
        Wheel wheel2 = new Wheel() { LocationX = 86, LocationY = 0};
        Wheel wheel3 = new Wheel() { LocationX = 172, LocationY = 0};
        Wheel wheelDouble = new Wheel() { LocationX = 260, LocationY = 0};
        List<Wheel> wheels = new List<Wheel>();
        int combination = 0;
        int timesTicked = 1;
        int timesToTick = 3;
        Random rnd = new Random();
        DispatcherTimer dispatcherTimer;
        int wheelnumber = 0;
        public int winnings = 0;
        int bet;
        Player player;
        TextBlock tbMoney;
        TextBlock tbLog;
        Button button_play;
        Button button_Double;
        Slider slider_Bet;

        private Canvas canvas;
        private int doublebool;

        //MediaElement mediaElement = new MediaElement();

        public KPeli(Canvas canvas, TextBlock tbMoney, TextBlock tbLog, Button button_play,Slider slider_Bet,Button button_Double)
        {
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

        public void Play(int bet, Player player)
        {
            this.bet = bet;
            this.player = player;
            

            //Change images --> [?]
            foreach (Wheel wheel in wheels)
            {
                wheel.ImageChange(9);
            }

            DispatcherTimerSetup();
        }

        //Timer
        public void DispatcherTimerSetup()
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 400);
            Debug.WriteLine("dispatcherTimer.IsEnabled = " + dispatcherTimer.IsEnabled + "\n");
            Debug.WriteLine("Calling dispatcherTimer.Start()\n");
            //combination
            combination = wheel1.Spin();
            
            //start timer
            dispatcherTimer.Start();
            Debug.WriteLine("dispatcherTimer.IsEnabled = " + dispatcherTimer.IsEnabled + "\n");
        }

        //Timer tick
        void dispatcherTimer_Tick(object sender, object e)
        {
            //kuvan vaihto
            wheels[wheelnumber].ImageChange(int.Parse(combination.ToString().Substring(wheelnumber,1)));

            wheelnumber++;
            timesTicked++;

            if (timesTicked > timesToTick)
            {
                //stop timer
                dispatcherTimer.Stop();
                Debug.WriteLine("dispatcherTimer.IsEnabled = " + dispatcherTimer.IsEnabled + "\n");
                LogAdd(combination.ToString(), tbLog);

                //Winnings
                winnings = CheckWin(bet);

                //Log tekstit

                player.Money += winnings;
                TextMoney(tbMoney);

                if (winnings > 0)
                {
                    LogAdd("You won " + winnings.ToString(),tbLog);
                    button_Double.IsEnabled = true;
                }
                else
                {
                    LogAdd("You lost " + ((-1) * winnings).ToString(),tbLog);
                }
                button_play.IsEnabled = true;
                slider_Bet.Maximum = player.Money;

            }
        }

        public void Reset()
        {
            combination = 0;
            timesTicked = 1;
            timesToTick = 3;
            wheelnumber = 0;
            winnings = 0;
            wheelDouble.ImageChange(9);

        }

        public void TextMoney(TextBlock tb)
        {
            tb.Text = "Money: " + player.Money.ToString();
        }

        public void LogAdd(string text,TextBlock tbLog)
        {
            tbLog.Text = text + Environment.NewLine + tbLog.Text;
        }

        /// <summary>
        /// Tuplaus, palauttaa 1=onnistui 0=epäonnistui
        /// </summary>
        public void Double()
        {
            wheelDouble.ImageChange(9);
            DispatcherTimerSetupDouble();
        }
        
        //Timer
        public void DispatcherTimerSetupDouble()
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_TickDouble;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 400);
            Debug.WriteLine("dispatcherTimer.IsEnabled = " + dispatcherTimer.IsEnabled + "\n");
            Debug.WriteLine("Calling dispatcherTimer.Start()\n");
            //True/false
            doublebool = wheel1.DoubleSpin();

            //start timer
            dispatcherTimer.Start();
            Debug.WriteLine("dispatcherTimer.IsEnabled = " + dispatcherTimer.IsEnabled + "\n");
        }

        //Timer tick
        void dispatcherTimer_TickDouble(object sender, object e)
        {
            //kuvan vaihto
            wheelDouble.ImageChange(doublebool);

            //stop timer
            dispatcherTimer.Stop();
            Debug.WriteLine("dispatcherTimer.IsEnabled = " + dispatcherTimer.IsEnabled + "\n");
            LogAdd(doublebool.ToString(), tbLog);

            //Winnings
            if(doublebool == 3)
            {
                player.Money -= winnings;
                winnings = winnings*2;
            }
            else
            {
                winnings = -(winnings);
            }

            //Log tekstit

            player.Money += winnings;
            TextMoney(tbMoney);

            if (winnings > 0)
            {
                LogAdd("You won " + winnings.ToString(), tbLog);
            }
            else
            {
                LogAdd("You lost " + ((-1) * winnings).ToString(), tbLog);
                button_Double.IsEnabled = false;
            }
            slider_Bet.Maximum = player.Money;
        }

        /// <summary>
        /// Tarkistaa voitot, palauttaa bet * x
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
    }
}