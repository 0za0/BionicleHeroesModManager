using BionicleHeroesModManager.Models;
using DynamicData;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BionicleHeroesModManager.ViewModels
{
    public class ModDownloadViewModel : ViewModelBase
    {
        private string? _searchText;
        public ObservableCollection<ModViewModel> AllMods { get; } = new();
        public ObservableCollection<ModViewModel> SearchResults { get; } = new();
        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => this.RaiseAndSetIfChanged(ref _isBusy, value);
        }
        public string? SearchText
        {
            get => _searchText;
            set => this.RaiseAndSetIfChanged(ref _searchText, value);
        }
        private CancellationTokenSource? _cancellationTokenSource;
        public ModDownloadViewModel(MainWindowViewModel mw)
        {
            InitModsList();

            this.WhenAnyValue(x => x.SearchText)
                .Throttle(TimeSpan.FromMilliseconds(400))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(DoSearch!);


        }
        //TODO: Change this to be proper
        private async void DoSearch(string s)
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = _cancellationTokenSource.Token;

            SearchResults.Clear();

            if (!string.IsNullOrWhiteSpace(s))
            {
                var mods = AllMods.Where(x => x.Mod.ModTitle.ToLower().Contains(s.ToLower())).ToList();

                foreach (var mod in mods)
                {
                    var vm = new ModViewModel(mod.Mod);

                    SearchResults.Add(vm);
                }
                if (!cancellationToken.IsCancellationRequested)
                {
                    LoadCovers(cancellationToken);
                }
            }
            else
            {
                SearchResults.AddRange(AllMods);
            }
        }
        
        //TODO: Change this to be proper!
        public async void InitModsList()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = _cancellationTokenSource.Token;
            IsBusy = true;
            SearchResults.Clear();
            var mods = await Mod.GetAllMods();
            foreach (var mod in mods)
            {
                var vm = new ModViewModel(mod);

                AllMods.Add(vm);
                SearchResults.Add(vm);
            }

            LoadCovers(cancellationToken);
            IsBusy = false;
        }
        private async void LoadCovers(CancellationToken cancellationToken)
        {
            foreach (var mod in SearchResults.ToList())
            {
                await mod.LoadImage();

            }
        }
    }
}
