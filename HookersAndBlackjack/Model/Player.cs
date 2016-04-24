using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Player(string name)
        {
            Name = name;
            Chips = 0;
        }
    }


}