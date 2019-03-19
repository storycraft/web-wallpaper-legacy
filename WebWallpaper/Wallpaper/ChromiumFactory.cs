using CefSharp;
using CefSharp.OffScreen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebWallpaper.Wallpaper.Window;

namespace WebWallpaper.Wallpaper
{
    public class ChromiumFactory : BrowserFactory<CefSharpBrowser>
    {

        private static ChromiumFactory instance;

        public static ChromiumFactory Instance
        {
            get
            {
                if (instance == null)
                    instance = new ChromiumFactory();

                return instance;
            }
        }

        private ChromiumFactory() : base()
        {

        }

        public override CefSharpBrowser CreateBrowser(Window.BrowserSettings settings, string url)
        {
            if (!Ready)
                return null;

            return new CefSharpBrowser(settings)
            {
                CurrentURL = url
            };
        }

        public override void Init()
        {
            if (Ready)
                Shutdown();
            Ready = true;

            CefSettings cefSettings = new CefSettings()
            {
                WindowlessRenderingEnabled = true,
                MultiThreadedMessageLoop = true
            };

            cefSettings.CefCommandLineArgs.Add("autoplay-policy", "no-user-gesture-required");

            cefSettings.EnableAudio();

            Cef.Initialize(cefSettings);
        }

        public override void Shutdown()
        {
            if (!Ready)
                return;
            Ready = false;

            Cef.Shutdown();
        }
    }
}
