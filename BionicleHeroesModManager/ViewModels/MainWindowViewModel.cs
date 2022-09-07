using BionicleHeroesModManager.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
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
        private string? _currentBgTask;
        public string? CurrentBgTask
        {
            get => _currentBgTask;
            set => this.RaiseAndSetIfChanged(ref _currentBgTask, value);
        }

        //Shows the Progress Stuff or not
        private bool _isWorking;
        public bool IsWorking
        {
            get => _isWorking;
            set => this.RaiseAndSetIfChanged(ref _isWorking, value);
        }

        //Percentage Bar
        private int _progressBarPercentage;
        public int ProgressBarPercentage
        {
            get => _progressBarPercentage;
            set => this.RaiseAndSetIfChanged(ref _progressBarPercentage, value);
        }
       


        public ObservableCollection<ModTabItem> Tabs { get; } = new();



        public MainWindowViewModel()
        {
            IsWorking = false;
            CurrentBgTask = "Idle";
            //According to stackoverflow its fine to just pass a reference this way, this is so all of them can change the ProgressBar bottom left corner
            Tabs.Add(new("Play", new ModPlayViewModel(this)));
            Tabs.Add(new("Download", new ModDownloadViewModel(this)));
            Tabs.Add(new("Settings", new SettingsViewModel()));
        }

        
    }
}
