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
            var thumbnail = await client.GetAsync(m.ImageURL);
            if (m.BigImageURL != String.Empty)
            {
                var bigImage = await client.GetAsync(m.BigImageURL);
                var bigpath = Path.Join(Directory.GetCurrentDirectory(), $"/ImageCache/{Regex.Replace(m.Title, @"[^a-zA-Z]", String.Empty)}BIG.jpg").Replace(@"\\", @"\");
                if (!File.Exists(bigpath))
                    await File.WriteAllBytesAsync(bigpath, await bigImage.Content.ReadAsByteArrayAsync());
                m.BigImageURL = Path.GetFullPath(bigpath);
            }


            var path = Path.Join(Directory.GetCurrentDirectory(), $"/ImageCache/{Regex.Replace(m.Title, @"[^a-zA-Z]", String.Empty)}.jpg").Replace(@"\\", @"\");
            if (!File.Exists(path))
                await File.WriteAllBytesAsync(path, await thumbnail.Content.ReadAsByteArrayAsync());
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
            var ModBigImg = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"articlesbrowse\"]/div[2]/div/div[2]/div[2]/*").Descendants("img").First();
            if (ModBigImg != null)
                c.BigImageURL = ModBigImg.Attributes["src"].Value;
            //await DownloadImages(c);
            ////*[@id="articlesbrowse"]/div[2]/div/div[2]/div[2]/h2/a/img
            ////*[@id="articlesbrowse"]/div[2]/div/div[2]/div[2]/p[2]/a/img
            ////*[@id="articlesbrowse"]/div[2]/div/div[2]/div[2]/div[1]/p/a/img
            if (c.Description == String.Empty)
            {
                foreach (var item in ModDesc)
                    c.Description += ModDesc.First().InnerText;
            }

        }

    }
}
