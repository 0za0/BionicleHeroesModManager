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
        public ObservableCollection<ModTabItem> Tabs { get; } = new();
       
        public MainWindowViewModel()
        {
            Tabs.Add(new("Play", new ModPlayViewModel()));
            Tabs.Add(new("Download", new ModDownloadViewModel()));
            Tabs.Add(new("Settings", new SettingsViewModel()));
        }
    }
}
