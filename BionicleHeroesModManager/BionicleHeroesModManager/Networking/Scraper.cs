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
using System.Collections.Generic;
namespace BionicleHeroesModManager.Networking
{
    record Mod(string ImageURL, string Title);
    internal class Scraper
    {
        public static object Messagebox { get; private set; }

        public static async Task<string> CallUrl(string fullUrl)
        {
            HttpClient client = new HttpClient();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13;
            client.DefaultRequestHeaders.Accept.Clear();
            var response = client.GetStringAsync(fullUrl);
            return await response;
        }

        public static void ParseHTML(string HTML)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(HTML);
            var AllModItems = htmlDoc.DocumentNode.SelectNodes("//*[@id=\"modsbrowse\"]/div[2]/div/div[2]").Descendants("a");


            List<Mod> Mods = new List<Mod>();

            foreach (var item in AllModItems.Skip(0).Where(x => x.InnerText != String.Empty))
            {
                var x = item.Descendants("img");
                MessageBox.Show(String.Join('\n', x.Select(y => y.Attributes["src"].Value.ToString())));
            }
        }

    }
}
