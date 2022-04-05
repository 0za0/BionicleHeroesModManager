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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BionicleHeroesModManager.FileHelpers;
using BionicleHeroesModManager.Networking;
using BionicleHeroesModManager.UserControls;
using BionicleHeroesModManager.View;

namespace BionicleHeroesModManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            FileHelper.CreateImageCache();

            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string data = await Scraper.CallUrl("https://www.moddb.com/games/bionicle-heroes/mods");
            var mods = await Scraper.ParseModPageHTML(data);
            for (int i = 0; i < mods.Count; i++)
            {
                //uff
                var item = mods[i];
                ModItem mi = new ModItem(item.ImageURL, item.Title,item);
                mi.MouseLeftButtonDown += ModItem_Click;
                MainStackPanel.Children.Add(mi);
            }
        }

        private void ModItem_Click(object sender, MouseButtonEventArgs e)
        {
            var mo = sender as ModItem;
            ModDetails md = new ModDetails(mo.Mod);
            md.ShowDialog();
        }
    }
}
