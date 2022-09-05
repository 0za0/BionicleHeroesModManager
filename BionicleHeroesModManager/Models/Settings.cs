using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BionicleHeroesModManager.Models
{
    public class Settings
    {
        public bool UseDxWND { get; set; }
        public string DxWNDPath { get; set; }

        public bool UseDgVoodoo { get; set; }
        public string DgVoodooPath { get; set; }

        public bool CheckForUpdatesOnStartup { get; set; }
        private string ConfigPath = "./Config/Config.cfg";

        //ReadConfig
        //WriteConfig
        //Download and Setup DXWnd or DgVoodoo
        //Config Parsers for both ... yay
    }
}
