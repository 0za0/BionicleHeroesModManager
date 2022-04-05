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
        public string BigImageURL { get; set; }
        public string Title { get; set; }
        public string URL { get; private set; }
        public string Description { get; set; }
        public Mod(string img, string name, string url) => (ImageURL, Title, URL) = (img, name, url);


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
        public static async Task<string> DownloadImages(Mod m)
        {
            HttpClient client = new HttpClient();
            var resp = await client.GetAsync(m.ImageURL);
            var path = Path.Join(Directory.GetCurrentDirectory(), $"/ImageCache/{Regex.Replace(m.Title, @"[^a-zA-Z]", String.Empty)}.jpg").Replace(@"\\", @"\");
            if (!File.Exists(path))
                await File.WriteAllBytesAsync(path, await resp.Content.ReadAsByteArrayAsync());
            return Path.GetFullPath(path);
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
                m.ImageURL = await DownloadImages(m);
                Mods.Add(m);
            }
            return Mods;
        }
        public static void ParseDetailedModPage(string HTML, Mod c)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(HTML);
            var ModDesc = htmlDoc.DocumentNode.SelectNodes("/html/body/div[1]/div/div[3]/div[1]/div[2]/div/div").Descendants("p").ToList();
            //var ModBigImg = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"articlesbrowse\"]/div[2]/div/div[2]/div[2]/").Descendants("img").ToList();
            foreach (var item in ModDesc)
            {
                c.Description += ModDesc.First().InnerText;
            }
        }

    }
}
