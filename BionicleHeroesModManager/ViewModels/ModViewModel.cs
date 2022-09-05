using Avalonia.Media.Imaging;
using BionicleHeroesModManager.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BionicleHeroesModManager.ViewModels
{
    public class ModViewModel : ViewModelBase
    {
        private Mod _mod;

        public Mod? Mod
        {
            get => _mod;
            set => this.RaiseAndSetIfChanged(ref _mod, value);
        }
        private Bitmap? _modImage;

        public Bitmap? ModImage
        {
            get => _modImage;
            private set => this.RaiseAndSetIfChanged(ref _modImage, value);
        }

        public ModViewModel(Mod m )
        {
            this._mod = m;
            SetupMod = ReactiveCommand.Create(() => { Debug.Write("Pushed"); });
            PlayMod = ReactiveCommand.Create(() => { Debug.Write("Pushed"); });
        }
        public async Task LoadImage()
        {
            await using (var imageStream = await _mod.LoadModImageAsync())
            {
                ModImage = await Task.Run(() => Bitmap.DecodeToWidth(imageStream, 400));
            }
        }

        public ICommand SetupMod { get; }
        public ICommand PlayMod { get; }
        public ICommand DownloadMod { get; }
        public bool IsDownloaded { get;  set; }

    }
}
