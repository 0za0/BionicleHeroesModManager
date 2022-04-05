using BionicleHeroesModManager.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BionicleHeroesModManager.View
{
    /// <summary>
    /// Interaction logic for ModDetails.xaml
    /// </summary>
    public partial class ModDetails : Window
    {
        public Mod CurrentMod { get; private set; }
        public ModDetails(Mod mod)
        {
            CurrentMod = mod;
            InitializeComponent();
            ModLabel.Content = CurrentMod.Title;
        }
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {


            //https://www.moddb.com/games/bionicle-heroes/
            var response = await Scraper.CallUrl($"https://www.moddb.com{CurrentMod.URL}");
            //html/body/div[1]/div/div[3]/div[1]/div[2]/div/div/p
            Scraper.ParseDetailedModPage(response, CurrentMod);
            DescriptionText.Text = CurrentMod.Description;

        }
    }
}
