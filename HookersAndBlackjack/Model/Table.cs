﻿using System;
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
        public bool Waiter { get; set; }
        public bool tester { get; set; }
        public int Returner { get; set; }
        private Random rand = new Random();

        // Pelaaja lista
        public List<Player> PlayerList = new List<Player>();

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
        // Ylimääräinen kortti. Sekoitusta varten.
        private Kortti T = new Kortti();

        // Jakaa kortit
        public void Deal()
        {
            Spades.Clear();
            Hearts.Clear();
            Diamonds.Clear();
            Clubs.Clear();
            Pack.Clear();
            for (int i = 0; i < PlayerList.Count; i++)
            {
                PlayerList[i].HandClear();
            }
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
            int n = Pack.Count;
            while (n > 1)
            {
                --n;
                int k = rand.Next(n + 1);
                T = Pack[k];
                Pack[k] = Pack[n];
                Pack[n] = T;
            }

            //Korttien siirto pelaajille
            for(int i = 0; i < PlayerList.Count; i++)
            {
                // Korttien siirto käteen
                int x = Pack.Count;
                for (n = 0; n < 2; n++)
                {
                    x--;
                    PlayerList[i].AddCard(Pack[x]);
                    Pack.RemoveAt(x);
                }
            }

            // Printtaus DebugMessageen. Mä ehkä haluan luoda tästä erillisen luokan.
            DebugMessage += "Pack count: " + Pack.Count + "\n";
            for (int i = 0; i < PlayerList.Count; i ++)
            {
                if (PlayerList[i].Intelligence == true) PlayerList[i].Checker();
                DebugMessage += "Foe#" + i + " hand count: " + PlayerList[i].CardCount() + "\n";
                DebugMessage += PlayerList[i].ToString();
            }
        }

        // Hit metodi vaatii kokonaisluvun, joka kertoo kuka käskee tekeen mitä.
        public void Hit(int playerNumber)
        {
            // Muokkaa tätä siten, että pöydän pakasta voidaan ottaa kortteja
            // ja lisätä niitä pelaajan pakkaan.
            try
            {
                int x;
                x = (Pack.Count - 1);
                PlayerList[playerNumber].AddCard(Pack[x]);
                Pack.RemoveAt(x);
                DebugMessage += "Hit";
            }
            catch
            {
                DebugMessage += "Could not edit lists.\n";
                DebugMessage += "Printing Hand and Pack count > \n";
            }

            try
            {
                DebugMessage = "";
                // Printtaus DebugMessageen. Mä ehkä haluan luoda tästä erillisen luokan.
                DebugMessage += "Pack count: " + Pack.Count + "\n";
                for (int i = 0; i < PlayerList.Count; i++)
                {
                    if (PlayerList[i].Intelligence == true)
                    {
                        PlayerList[i].Checker();
                        DebugMessage += "Player" + " hand count: " + PlayerList[i].CardCount() + "\n";
                        DebugMessage += PlayerList[i].ToString();
                    }
                    else
                    {
                        DebugMessage += "Foe#" + i + " hand count: " + PlayerList[i].CardCount() + "\n";
                        DebugMessage += PlayerList[i].ToString();
                    }
                }
            }
            catch(Exception ex)
            {
                DebugMessage += "Could not print cards and call Checker()\n";
                DebugMessage += ex.ToString() + "\n";
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