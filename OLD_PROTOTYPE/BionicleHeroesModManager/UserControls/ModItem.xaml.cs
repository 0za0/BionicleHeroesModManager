using BionicleHeroesModManager.Networking;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

namespace BionicleHeroesModManager.UserControls
{
    /// <summary>
    /// Interaction logic for ModItem.xaml
    /// </summary>
    public partial class ModItem : UserControl
    {
        public Mod Mod { get; private set; }
        public string ModTitle { get; private set; } = "Test Text";
        public ImageSource ModImg { get; private set; }
        public ModItem(string imageURL, string Name,Mod m)
        {
            this.Mod = m;
            this.DataContext = this;
            BitmapImage image = new BitmapImage(new Uri(imageURL, UriKind.Absolute));
            ModImg = image;
            ModTitle = Name;
            InitializeComponent();
        }


    }
}
