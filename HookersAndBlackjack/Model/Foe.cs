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
    /// Foe luokka toimii place holderina lopullista pelaaja/vastustaja luokkaa varten.
    /// Tänne kokeellinen tavara.
    /// </summary>
    class Foe
    {
        public List<Kortti> Hand = new List<Kortti>();
        public bool Intelligence { get; set; }
        public int Uhkarohkeus { get; set; }
        public string vastaus { get; set; }
        private int Risk { get; set; }

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
            if(Uhkarohkeus < Risk)
            {
                vastaus = "Pass";
            }
            else if(Uhkarohkeus > Risk)
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

        // Tätä voi käyttää kun halutaan antaa infoa käyttäkälle popup ikkunassa.
        // Vois laittaa erilliseen luokkaan.
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

        // Konstruktorit
        public Foe () { }
        public Foe (bool intelligence, int uhkarohkeus)
        {
            Intelligence = intelligence;
            Uhkarohkeus = uhkarohkeus;
        }
    }
}
