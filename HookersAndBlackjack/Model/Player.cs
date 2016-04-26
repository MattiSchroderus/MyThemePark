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
        public int Money { get; set; }
        public int Chips { get; set; }
        public int Loses { get; set; }
        public int Games { get; set; }
        public int Loans { get; set; }

        // Foe luokasta
        private List<Kortti> Hand = new List<Kortti>();
        public bool Intelligence { get; set; }
        public int Uhkarohkeus { get; set; }
        private int Risk { get; set; }
        public string vastaus { get; set; }

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

        public int CardTotal()
        {
            int u = 0;
            foreach (Kortti k in Hand)
            {
                u += k.Number;
            }
            return u;
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

        private async void Popup()
        {
            MessageDialog messageDialog = new MessageDialog(
                "You thought your DDOS attack would succeed, it did not." +
                "You thought you could beat us, you can not." +
                "You can only hope to contain us, and fail");
            messageDialog.Commands.Add(new UICommand("Ok"));
            await messageDialog.ShowAsync();
        }

        public override string ToString()
        {
            string str = "";
            try
            {
                foreach (Kortti k in Hand)
                {
                    str += k.ToString();
                }
            }
            catch (Exception ex)
            {
                str = "Exception in player tostring()" + ex.ToString();
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