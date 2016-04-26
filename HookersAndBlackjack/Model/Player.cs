using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace HookersAndBlackjack.Model
{
    /// <summary>
    /// 1 = Human, 4 = House, 2 = Easy, 3 = Hard
    /// </summary>
    public class Player
    {
        public string Name { get; set; }
        public int AI { get; set; }
        public int Money { get; set; }
        public int Chips { get; set; }
        public int Loses { get; set; }
        public int Games { get; set; }
        public int Loans { get; set; }

        // Foe luokasta
        private List<Kortti> Hand = new List<Kortti>();
        public bool Intelligence { get; set; }
        public int Uhkarohkeus { get; set; }
        public string vastaus { get; set; }
        private int Risk { get; set; }

        // Valmistelua player luokkaa varten
        public void HandClear()
        {
            Hand.Clear();
        }

        public void AddCard(Kortti k)
        {
            Hand.Add(k);
        }

        public int CardCount()
        {
            return Hand.Count;
        }

        // Tämä tarkistaa mikä on halutun kortin todennäköisyys
        public void RiskMeter(int packNumber)
        {
            double u = 0;
            foreach (Kortti k in Hand)
            {
                u += k.Number;
            }
            u = 21 - u;
            u = Math.Pow((packNumber * 4), u) - Hand.Count;
            u = u / ((52 * packNumber) - Hand.Count);
            u = u * 100;
            Risk = (int)u;
        }

        // Tämä selvittää minkä vaihtoehdon botti ottaa tällä uhkarohkeus tasolla
        public void Thinking()
        {
            if (Uhkarohkeus < Risk)
            {
                vastaus = "Pass";
            }
            else
            {
                vastaus = "Hit";
            }
        }

        // Tämä tarkistaa onko yhteen laskettu arvo yli 21.
        public void Checker()
        {
            int u = 0;
            foreach (Kortti k in Hand)
            {
                u += k.Number;
            }
            if (u > 21)
            {
                Popup();
            }
        }

        // Tätä voi käyttää kun halutaan antaa infoa popup ikkunassa.
        public async void Popup()
        {
            // create the message dialog and set its content
            var messageDialog = new MessageDialog(
                "You thought your DDOS attack would succeed, it did not." +
                "You thought you could beat us, you can not." +
                "You can only hope to contain us, and fail.");

            messageDialog.Title = "Title";

            // add commands and set their callbacks; both buttons use the same callback function
            messageDialog.Commands.Add(new UICommand(
                "Ok",
                new UICommandInvokedHandler(this.CommandInvokedHandler)));
            messageDialog.Commands.Add(new UICommand(
                "Cancel",
                new UICommandInvokedHandler(this.CommandInvokedHandler)));

            // set the command that will be invoked by default
            messageDialog.DefaultCommandIndex = 0;
            // set the command to be invoked when escape is pressed
            messageDialog.DefaultCommandIndex = 1;
            await messageDialog.ShowAsync();
        }

        //Kuuluu Popup metodiin.
        private void CommandInvokedHandler(IUICommand command)
        {
            // Display message showing the label of the command that was invoked
            Debug.WriteLine("The '" + command.Label + "' command has been selected.");
        }

        public override string ToString()
        {
            string str = "";
            foreach (Kortti k in Hand)
            {
                str += k.ToString();
            }
            return str;
        }

        public Player(string name)
        {
            Name = name;
            Chips = 0;
        }
    }


}