using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;

namespace BionicleHeroesModManager.ViewModels
{
    public class ModTabItem
    {
        public string Header { get; }
        public ViewModelBase Content { get; }
        public ModTabItem(string Header, ViewModelBase vm)
        {
            this.Header = Header;
            this.Content = vm;
        }

    }
    public class MainWindowViewModel : ViewModelBase
    {
        public ICommand DownloadHeroes { get; }

        public ObservableCollection<ModViewModel> Mods { get; } = new();
        public ObservableCollection<ModTabItem> Tabs { get; } = new();

        private bool _collectionEmpty;

        public bool CollectionEmpty
        {
            get => _collectionEmpty;
            set => this.RaiseAndSetIfChanged(ref _collectionEmpty, value);
        }
        public MainWindowViewModel()
        {
            DownloadHeroes = ReactiveCommand.Create(() => { Debug.WriteLine("Cmon Man"); });
            this.WhenAnyValue(x => x.Mods.Count).Subscribe(x => CollectionEmpty = x == 0);
            
            Tabs.Add(new("Play", new ModPlayViewModel()));
            Tabs.Add(new("Download", new ModDownloadViewModel()));
            Tabs.Add(new("Settings", new SettingsViewModel()));

            Mods.Add(new());
            Mods.Add(new());
            Mods.Add(new());
            Mods.Add(new());
            Mods.Add(new());
        }
    }
}
