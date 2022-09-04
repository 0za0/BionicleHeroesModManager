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

        public ModViewModel()
        {
            SetupMod = ReactiveCommand.Create(() => { Debug.Write("Pushed"); });
            PlayMod = ReactiveCommand.Create(() => { Debug.Write("Pushed"); });
        }
        public ICommand SetupMod { get; }
        public ICommand PlayMod { get; }

    }
}
