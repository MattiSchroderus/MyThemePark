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
    }
}
