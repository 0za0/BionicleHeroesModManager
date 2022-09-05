using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BionicleHeroesModManager.ViewModels
{
    public class ModPlayViewModel : ViewModelBase
    {
        public ICommand DownloadHeroes { get; }

        public ObservableCollection<ModViewModel> Mods { get; } = new();

        private bool _collectionEmpty;

        public bool CollectionEmpty
        {
            get => _collectionEmpty;
            set => this.RaiseAndSetIfChanged(ref _collectionEmpty, value);
        }
        public ModPlayViewModel()
        {
            DownloadHeroes = ReactiveCommand.Create(() => { Debug.WriteLine("Cmon Man"); });
             this.WhenAnyValue(x => x.Mods.Count).Subscribe(x => CollectionEmpty = x == 0);
          
            //Mods.Add(new());
            //Mods.Add(new());
            //Mods.Add(new());
            //Mods.Add(new());
            //Mods.Add(new());
        
        }
    }
}
