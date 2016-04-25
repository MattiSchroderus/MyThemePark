using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        // Tämä tarkistaa mikä halutun kortin todennäköisyys
        public void RiskMeter()
        {
            int u = 0;
            int count = Hand.Count;
            foreach (Kortti k in Hand)
            {
                u += k.Number;
            }

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
