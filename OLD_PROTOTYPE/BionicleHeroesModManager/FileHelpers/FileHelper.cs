using System.IO;

namespace BionicleHeroesModManager.FileHelpers
{
    internal static class FileHelper
    {
        public static void CreateImageCache()
        {
            if (!Directory.Exists("./ImageCache"))
            {
                Directory.CreateDirectory("./ImageCache");
            }
        }
    }
}
