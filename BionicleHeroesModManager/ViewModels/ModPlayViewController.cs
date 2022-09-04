using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BionicleHeroesModManager.ViewModels
{
    public class ModPlayViewController : ViewModelBase
    {
        public ObservableCollection<ModViewModel> Mods { get; } = new();
        private bool _collectionEmpty;

        public bool CollectionEmpty
        {
            get => _collectionEmpty;
            set => this.RaiseAndSetIfChanged(ref _collectionEmpty, value);
        }

        public ModPlayViewController()
        {
            this.WhenAnyValue(x => x.Mods.Count)
                    .Subscribe(x => CollectionEmpty = x == 0);
            //Mods.Add(new());
            //Mods.Add(new());
            //Mods.Add(new());
            //Mods.Add(new());
            //Mods.Add(new());
        }
    }
}
