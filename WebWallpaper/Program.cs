using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebWallpaper.Config;
using WebWallpaper.Log;

namespace WebWallpaper
{
    public class Program
    {
        static void Main(string[] args)
        {
            ConfigEntry defaultConfig = new ConfigEntry() {
                startURL = "https://www.google.com",
                renderEnabled = true,
                handleMovement = true,
                clickEnabled = false
            };

            Logger.Log("WebWallpaper started");

            using (var webWallpaper = new WebWallpaper.Wallpaper.WebWallpaper(defaultConfig))
            {
                webWallpaper.Start();
            }
        }
    }
}
