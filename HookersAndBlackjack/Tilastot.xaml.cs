using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using HookersAndBlackjack.Model;
using System.Diagnostics;
using System.Collections.ObjectModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HookersAndBlackjack
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Tilastot : Page
    {
        public Tilastot()
        {
            this.InitializeComponent();
            string data = "";
            try
            {
                string[] lines = File.ReadAllLines("Assets/players.txt"); //get lines from players.txt
                foreach (string pair in lines) //pair == {name} {money}
                {
                    int position = pair.IndexOf(" "); //name ends in " "
                    if (position < 0) { continue; }
                    else
                    {
                        string name = pair.Substring(0, position); //name ends in " "
                        nameText.Text += Environment.NewLine + name;
                        string moneychips = pair.Substring(position + 1); //money and chips are rest
                        int position2 = moneychips.IndexOf(" "); //money ends in " "
                        if (position2 < 0) { continue; }
                        else
                        {
                            int money = int.Parse(moneychips.Substring(0, position2));
                            int chips = int.Parse(moneychips.Substring(position2 + 1));
                            moneyText.Text += Environment.NewLine + money.ToString();
                            chipText.Text += Environment.NewLine + chips.ToString();
                        }
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Debug.WriteLine("error: File Not Found");
            }


        }
        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            // get root frame (which show pages)
            Frame rootFrame = Window.Current.Content as Frame;
            // did we get it correctly
            if (rootFrame == null) return;
            // navigate back if possible
            if (rootFrame.CanGoBack)
            {
                rootFrame.GoBack();
            }
        }
    }
}
