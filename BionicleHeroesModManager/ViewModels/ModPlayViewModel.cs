using BionicleHeroesModManager.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Concurrency;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BionicleHeroesModManager.ViewModels
{
    public class ModPlayViewModel : ViewModelBase
    {
        public ICommand DownloadBaseGame { get; }

        public ObservableCollection<ModViewModel> Mods { get; } = new();
        private MainWindowViewModel MainWindowViewModel;
        private bool _collectionEmpty;

        public bool CollectionEmpty
        {
            get => _collectionEmpty;
            set => this.RaiseAndSetIfChanged(ref _collectionEmpty, value);
        }

        private async void LoadMods()
        {
            var mods = (await Mod.LoadCachedAsync()).Select(x => new ModViewModel(x));

            foreach (var mod in mods)
            {
                Mods.Add(mod);
            }

            foreach (var mod in Mods.ToList())
            {
                await mod.LoadImage();
            }
        }
        public ModPlayViewModel(MainWindowViewModel mw)
        {
            MainWindowViewModel = mw;
            DownloadBaseGame = ReactiveCommand.Create(() =>
            {
                var x = Mod.DownloadBaseGame();
                x.DownloadProgressChanged += X_DownloadProgressChanged;
                x.DownloadFileCompleted += X_DownloadFileCompleted;
                mw.IsWorking = true;
                mw.CurrentBgTask = "Downloading...";
            });
            this.WhenAnyValue(x => x.Mods.Count).Subscribe(x => CollectionEmpty = x == 0);
            RxApp.MainThreadScheduler.Schedule(LoadMods);
        }

        private async void X_DownloadFileCompleted(object? sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            MainWindowViewModel.IsWorking = false;
            Mod m = new Mod("Modders Edition", "A ready to go version of Bionicle Heroes, that allows for quick and easy modification.", "http://localhost:5000/img.png", "");
            m.IsDownloaded = true;
            var vm = new ModViewModel(m);
            await vm.LoadImage();
            await vm.SaveToDiskAsync();
            Mods.Add(vm);

            MainWindowViewModel.IsWorking = true;
            MainWindowViewModel.IsIndeterminate = true;
            MainWindowViewModel.CurrentBgTask = "Unzipping Files...";


            Task t = Mod.SetupAndBaseGame();
            await t.ContinueWith((e) => {
                MainWindowViewModel.CurrentBgTask = "Verifying Files...";
                Mod.VerifyGameFile("./Mods/BH_Modders", "./Mods/BH_Modders/verification");
            });
        }

        private void X_DownloadProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e) =>
            MainWindowViewModel.ProgressBarPercentage = e.ProgressPercentage;

    }
}
