using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.IO;
using HtmlAgilityPack;
using System.Windows;
using System.Text.RegularExpressions;

namespace BionicleHeroesModManager.Networking
{
    //Hurr Durr Object orientation 
    public class Mod
    {
        public string ImageURL { get; set; }
        public string BigImageURL { get; set; } = String.Empty;
        public string Title { get; set; }
        public string URL { get; private set; }
        public string Description { get; set; } = String.Empty;
        public Mod(string img, string name, string url) => (ImageURL, Title, URL) = (img, name, url);


    }
    public class Tool
    {
        public string ImageURL { get; set; }
        public string BigImageURL { get; set; } = String.Empty;
        public string Title { get; set; }
        public string DownloadURL { get; private set; }
        public string Description { get; set; } = String.Empty;
        public Tool(string img, string name, string url) => (ImageURL, Title, DownloadURL) = (img, name, url);


    }
    internal class Scraper
    {
        public static List<Mod> Mods = new List<Mod>();

        public static async Task<string> CallUrl(string fullUrl)
        {
            HttpClient client = new HttpClient();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13;
            client.DefaultRequestHeaders.Accept.Clear();
            var response = client.GetStringAsync(fullUrl);
            return await response;
        }
        //God have mercy
        public static async Task DownloadImages(Mod m)
        {
            HttpClient client = new HttpClient();

            if (m.BigImageURL != String.Empty)
            {

                var bigpath = Path.Join(Directory.GetCurrentDirectory(), $"/ImageCache/{Regex.Replace(m.Title, @"[^a-zA-Z]", String.Empty)}BIG.jpg").Replace(@"\\", @"\");
                if (!File.Exists(bigpath))
                {
                    var bigImage = await client.GetAsync(m.BigImageURL);
                    await File.WriteAllBytesAsync(bigpath, await bigImage.Content.ReadAsByteArrayAsync());
                    m.BigImageURL = Path.GetFullPath(bigpath);
                }
                else
                    m.BigImageURL = Path.GetFullPath(bigpath);
            }


            var path = Path.Join(Directory.GetCurrentDirectory(), $"/ImageCache/{Regex.Replace(m.Title, @"[^a-zA-Z]", String.Empty)}.jpg").Replace(@"\\", @"\");
            if (!File.Exists(path))
            {
                var thumbnail = await client.GetAsync(m.ImageURL);
                await File.WriteAllBytesAsync(path, await thumbnail.Content.ReadAsByteArrayAsync());
                m.ImageURL = Path.GetFullPath(path);
            }
            else
                m.ImageURL = Path.GetFullPath(path);

        }
        //Fuck me sideways
        public static async Task<List<Mod>> ParseModPageHTML(string HTML)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(HTML);
            var AllModItems = htmlDoc.DocumentNode.SelectNodes("//*[@id=\"modsbrowse\"]/div[2]/div/div[2]/div[*]/a").ToList();
            foreach (var item in AllModItems)
            {
                var modName = item.Attributes["title"].Value;
                var href = item.Attributes["href"].Value;
                var modImageLocation = item.FirstChild.Attributes["src"].Value;
                Mod m = new Mod(modImageLocation, modName, href);
                await DownloadImages(m);
                Mods.Add(m);
            }
            return Mods;
        }
        public static async Task ParseDetailedModPageAsync(string HTML, Mod c)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(HTML);
            var ModDesc = htmlDoc.DocumentNode.SelectNodes("/html/body/div[1]/div/div[3]/div[1]/div[2]/div/div").Descendants("p").ToList();
            var AllPossibleImages = htmlDoc.DocumentNode.SelectNodes("//*[@id=\"articlesbrowse\"]/div[2]/div/div[2]/div[2]");
            if (AllPossibleImages != null)
            {
                var BigImgs = AllPossibleImages.Descendants("img");
                if (BigImgs.First() != null && c.BigImageURL == String.Empty)
                {
                    c.BigImageURL = BigImgs.First().Attributes["src"].Value;
                    await DownloadImages(c);
                }
            }    
            if (c.Description == String.Empty)
            {
                foreach (var item in ModDesc)
                    c.Description += ModDesc.First().InnerText;
            }

        }

    }
}
