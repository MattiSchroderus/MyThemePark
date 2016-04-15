using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using Windows.UI.Popups;

namespace HookersAndBlackjack.Model
{
    class Table
    {
        // Kokeillaan voinko luoda jo BlackjackMenussa ton olion ja passata sitä sitten tänne.
        public int PackNumber { get; set; }
        public int StakeSize { get; set; }
        private Random rand = new Random();

        // Dummy pelaaja
        public Foe Dummy = new Foe();
        // Pelaaja lista
        public List<Foe> PlayerList = new List<Foe>();

        // DebugMessage on tässä debuggausta varten. Mietin että tätä voisi käyttää
        // palauttamaan stringin erillistä Debug luokkaa varten. 
        public string DebugMessage { get; set; }

        // Jokainen maa on oma lista
        private List<Kortti> Spades = new List<Kortti>();
        private List<Kortti> Hearts = new List<Kortti>();
        private List<Kortti> Diamonds = new List<Kortti>();
        private List<Kortti> Clubs = new List<Kortti>();
        // Pakka on myös lista, johon myöhemmin muut listat lisätään.
        private List<Kortti> Pack = new List<Kortti>();
        /* Käsi. 
        Tämä siirretään Foe luokkaan. Tähän listaan pääsee käsiksi
        Foe luokan olion metodeilla. Tämä tehdään näin jotta
        Foe luokan oliot voidaan tunkea listaan, jolloin pelaajien määrää
        voidaan hallita erikseen BlackjackMenu:sta.
        */
        private List<Kortti> Hand = new List<Kortti>();
        // Ylimääräinen kortti. Sekoitusta varten.
        private Kortti T = new Kortti();

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

        // Jakaa kortit
        public void Deal()
        {
            Spades.Clear();
            Hearts.Clear();
            Diamonds.Clear();
            Clubs.Clear();
            Pack.Clear();
            Hand.Clear();

            DebugMessage = "";

            //Spades
            for (ushort i = 0; i < 13; i++)
            {
                Kortti kortti = new Kortti("♠", (ushort)(i + 1));
                Spades.Add(kortti);
            }
            //Hertz
            for (ushort i = 0; i < 13; i++)
            {
                Kortti kortti = new Kortti("♥", (ushort)(i + 1));
                Hearts.Add(kortti);
            }
            //Diamonds
            for (ushort i = 0; i < 13; i++)
            {
                Kortti kortti = new Kortti("♦", (ushort)(i + 1));
                Diamonds.Add(kortti);
            }
            //Clubs
            for (ushort i = 0; i < 13; i++)
            {
                Kortti kortti = new Kortti("♣", (ushort)(i + 1));
                Clubs.Add(kortti);
            }

            //Lisätään kaikki maat pakkaan ja kerrotaan menu ikkunan PackNumberilla
            for (ushort i = 0; i < PackNumber; i++)
            {
                Pack.AddRange(Spades);
                Pack.AddRange(Hearts);
                Pack.AddRange(Diamonds);
                Pack.AddRange(Clubs);
            }

            // Fisher-Yates sekoitus
            ushort n = (ushort)Pack.Count;
            while (n > 1)
            {
                n--;
                int k = rand.Next(n + 1);
                T = Pack[k];
                Pack[k] = Pack[n];
                Pack[n] = T;
            }
            // Korttien siirto käteen
            ushort x = (ushort)Pack.Count;
            for (n = 0; n < 2; n++)
            {
                x--;
                Hand.Add(Pack[x]);
                Pack.RemoveAt(x);
            }
            // Printtaus DebugMessageen. Mä ehkä haluan luoda tästä erillisen luokan.
            DebugMessage += "Pack count: " + Pack.Count + "\n";
            DebugMessage += "Hand count: " + Hand.Count + "\n";

            foreach (Kortti k in Hand)
            {
                DebugMessage += k.ToString();
            }
        }

        // Tämä ei kuulu tänne. Siirretään Foe luokkaan.
        public void Hit()
        {
            // Muokkaa tätä siten, että pöydän pakasta voidaan ottaa kortteja
            // ja lisätä niitä pelaajan pakkaan.
            DebugMessage = "";
            ushort x;
            try
            {
                x = (ushort)(Pack.Count - 1);
                Hand.Add(Pack[x]);
                Pack.RemoveAt(x);
            }
            catch
            {
                DebugMessage += "Could not edit lists.\n";
                DebugMessage += "Printing Hand and Pack count > \n";
            }

            // Kaikki tästä alaspäin toimii niinkuin pitääkin. 
            try
            {
                // Printtaus DebugMessageen. Mä ehkä haluan luoda tästä erillisen luokan.
                DebugMessage += "Pack count: " + Pack.Count + "\n";
                DebugMessage += "Hand count: " + Hand.Count + "\n";
            }
            catch
            {
                DebugMessage += "Something wrong with DebugMessage.\n";
            }

            try
            {
                foreach (Kortti k in Hand)
                {
                    DebugMessage += k.ToString();
                }
            }
            catch
            {
                DebugMessage = "Could not print cards.\n";
            }

        }

        // Tämä tarkistaa onko yhteen laskettu arvo yli 21.
        // Ei kuulu tänne. Siirretään Foe luokkaan.
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

        public Table() { }
        public Table(int packnumber, int stacksize)
        {
            PackNumber = packnumber;
            StakeSize = stacksize;
        }
    }
}