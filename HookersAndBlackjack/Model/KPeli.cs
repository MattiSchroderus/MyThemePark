using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HookersAndBlackjack.Model
{
    public class KPeli
    {
        public Wheel rulla { get; set; }

        /// <summary>
        /// Palauttaa 3 rullan numerosarjan esim. 134
        /// </summary>
        public int Play()
        {
            //Spin Wheels
            int numero1 = rulla.Spin(200);
            int numero2 = rulla.Spin(200);
            int numero3 = rulla.Spin(200);

            //Check combinations
            if (numero1 == numero2 && numero3 == numero1 | numero1 != 0)
            {
                switch (numero1 + numero2 + numero3)
                {
                    case 3: return 111;
                    case 6: return 222;
                    case 9: return 333;
                    case 12: return 444;
                    case 15: return 555;
                    default: return 0;
                }
            }
            else
            {
                return int.Parse(numero1.ToString() + numero2.ToString() + numero3.ToString());
            }
        }


        /// <summary>
        /// Panoksen asettaminen
        /// </summary>
        internal bool bet(int v)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Tuplaus, palauttaa 1=onnistui 0=epäonnistui
        /// </summary>
        public void Double()
        {
            /*
            int rnd = Rand(1, 101);
            if(rnd < 25)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        */
        }

        /// <summary>
        /// Tarkistaa voitot, palauttaa bet * x
        /// </summary>
        public int Win(int numero, int bet)
        {
            switch (numero)
            {
                case 111: return bet * 25;
                case 222: return bet * 10;
                case 333: return bet * 5;
                case 444: return bet * 2;
                case 555: return bet;
                default: return bet - (2 * bet);
            }
        }
    }
}

//voittotaulukko
/*
1. 25x	1/28
2. 10x	3/28
3. 5x   6/28
4. 2x	8/28
5. 1x	10/28
Double 24%
0-10-18-24-27-28
*/
