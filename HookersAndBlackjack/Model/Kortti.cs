using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
namespace HookersAndBlackjack.Model
{
    class Kortti
    {
        public string Suit { get; }
        public ushort Number { get; }

        public Kortti() { }

        public Kortti(string suit, ushort number)
        {
            Suit = suit;
            Number = number;
        }

        public override string ToString()
        {
            return "Suit: " + Suit + ", Number: " + Number + "\n";
        }
    }
}