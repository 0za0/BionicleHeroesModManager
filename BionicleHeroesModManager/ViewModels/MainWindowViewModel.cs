using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace BionicleHeroesModManager.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<ModViewModel> Mods { get; } = new();
        public MainWindowViewModel()
        {
            Mods.Add(new());
            Mods.Add(new());
            Mods.Add(new());
            Mods.Add(new());
            Mods.Add(new());
        }
    }
}
