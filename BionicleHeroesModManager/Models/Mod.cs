using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO.Compression;
using Avalonia.Markup.Xaml.Templates;


namespace BionicleHeroesModManager.Models
{
    public class Mod
    {
        private static HttpClient s_httpClient = new();

        public Mod(string modTitle, string modDescription, string imageURL, string downloadURL)
        {
            ModTitle = modTitle;
            ModDescription = modDescription;
            ImageURL = imageURL;
            DownloadURL = downloadURL;
            ModPath = $"./Mods/{ModTitle}";
        }

        public string ModTitle { get; set; }
        public string ModDescription { get; set; }
        public string ImageURL { get; set; }
        public string DownloadURL { get; set; }
        public bool IsDownloaded { get; set; }
        public string ModPath { get; private set; }
        private string CachePath => $"./Cache/{ModTitle}";


        public Stream SaveModImageBitmapStream()
        {
            return File.OpenWrite(CachePath + ".bmp");
        }
        public async Task<Stream> LoadModImageAsync()
        {
            if (File.Exists(CachePath + ".bmp"))
            {
                return File.OpenRead(CachePath + ".bmp");
            }
            else
            {
                var data = await s_httpClient.GetByteArrayAsync(ImageURL);
                return new MemoryStream(data);
            }
        }

        public async Task SaveAsync()
        {
            if (!Directory.Exists("./Cache"))
            {
                Directory.CreateDirectory("./Cache");
            }

            using (var fs = File.OpenWrite(CachePath))
            {
                await SaveToStreamAsync(this, fs);
            }
        }

        private static async Task SaveToStreamAsync(Mod data, Stream stream)
        {
            await JsonSerializer.SerializeAsync(stream, data).ConfigureAwait(false);
        }

        public static async Task<Mod> LoadFromStream(Stream stream)
        {
            return (await JsonSerializer.DeserializeAsync<Mod>(stream).ConfigureAwait(false))!;
        }

        public static async Task<IEnumerable<Mod>> LoadCachedAsync()
        {
            if (!Directory.Exists("./Cache"))
            {
                Directory.CreateDirectory("./Cache");
            }

            var results = new List<Mod>();

            foreach (var file in Directory.EnumerateFiles("./Cache"))
            {
                if (!string.IsNullOrWhiteSpace(new DirectoryInfo(file).Extension)) continue;

                await using var fs = File.OpenRead(file);
                results.Add(await Mod.LoadFromStream(fs).ConfigureAwait(false));
            }

            return results;
        }

        public static async Task<IEnumerable<Mod>> GetAllMods()
        {
            var m = await s_httpClient.GetFromJsonAsync<List<Mod>>("http://localhost:5000/mod.json");
            return m;
        }

        public static async Task<IEnumerable<Mod>> SearchAsync(string searchTerm)
        {
            var m = await s_httpClient.GetFromJsonAsync<List<Mod>>("http://localhost:5000/mod.json");
            return m.Where(x => x.ModTitle.Any(c => searchTerm.Contains(c)));

        }

        // TODO: Change to HTTPClient
        public static WebClient DownloadMod(Mod mod)
        {
            WebClient webClient = new WebClient();
            webClient.DownloadFileAsync(new Uri(mod.DownloadURL), mod.ModPath);
            return webClient;
        }
        
        // TODO: Change to HTTPClient
        public static WebClient DownloadBaseGame()
        {
            WebClient webClient = new WebClient();
            if (!Directory.Exists("./Mods/BH_Modders"))
            {
                Directory.CreateDirectory("./Mods/BH_Modders");
            }
            webClient.DownloadFileAsync(new Uri("http://localhost:5000/mods/base/Modders.zip"), "./Mods/BH_Modders/Modders.zip");
            return webClient;
        }

        public static Task SetupAndBaseGame()
        {
            return Task.Run(() => ZipFile.ExtractToDirectory("./Mods/BH_Modders/Modders.zip", "./Mods/BH_Modders"));
        }

        public static async void VerifyGameFile(string gamePath, string verificationFilePath)
        {
        }

    }
}
