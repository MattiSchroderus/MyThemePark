using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HookersAndBlackjack.Model
{
    public class Wheel
    {
        /// <summary>
        /// Rullan pyöräytys = random numero, images = kuvien määrä rullassa
        /// </summary>
        public int Spin(int images)
        {
            Random rand = new Random();
            int numero = rand.Next(1, (images + 1));
            if (0 < numero && numero < 11)
            {
                return 5;
            }
            else if (10 < numero && numero < 19)
            {
                return 4;
            }
            else if (18 < numero && numero < 24)
            {
                return 3;
            }
            else if (24 < numero && numero < 28)
            {
                return 2;
            }
            else if (numero == 28)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}

//voittotaulukko
/*
3 Wheels        Double
1. 25x	1/28    1-3 = 10/28
2. 10x	3/28    4-5 = 18/28
3. 5x   6/28    
4. 2x	8/28    
5. 1x	10/28   
0-10-18-24-27-28
*/
