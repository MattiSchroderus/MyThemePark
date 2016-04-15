using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HookersAndBlackjack.Model
{
    class Player
    {
        private string Name { get; set; }
        /// <summary>
        /// 1 = Human, 4 = House, 2 = Easy, 3 = Hard
        /// </summary>
        public int AI { get; set; }
        public int Money { get; set; }
        public int Chips { get; set; }
        public int Loses { get; set; }
        public int Games { get; set; }
        public int Loans { get; set; }

        public Player(string name, int ai)
        {
            Name = name;
            AI = ai;
        }
    }


}