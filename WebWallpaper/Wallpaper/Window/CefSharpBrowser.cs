using CefSharp;
using CefSharp.OffScreen;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebWallpaper.Wallpaper.Window
{
    public class CefSharpBrowser : IBrowserVirtual
    {

        private string lastSetURL;

        public string CurrentURL
        {
            get
            {
                return Browser.Address;
            }

            set
            {
                if (Started)
                    Browser.Load(value);

                lastSetURL = value;
            }
        }
        
        public bool Started { get; private set; }
        public bool Ready { get => Browser.IsBrowserInitialized; }

        private ChromiumWebBrowser Browser { get; set; }

        public Size Size
        {
            get
            {
                return Browser.Size;
            }

            set
            {
                Browser.Size = value;
            }
        }

        public CefSharpBrowser(BrowserSettings settings)
        {
            CefSharp.BrowserSettings browserSettings = new CefSharp.BrowserSettings();

            if (settings.SkipCorsCheck)
            {
                browserSettings.FileAccessFromFileUrls = CefState.Enabled;
                browserSettings.UniversalAccessFromFileUrls = CefState.Enabled;
                browserSettings.WebSecurity = CefState.Enabled;
            }

            Browser = new ChromiumWebBrowser("", browserSettings);
        }

        public CefSharpBrowser()
        {
            Browser = new ChromiumWebBrowser();
        }

        public void Start()
        {
            if (Started)
                Stop();
            Started = true;

            while (!Browser.IsBrowserInitialized) { System.Threading.Thread.Sleep(1); }

            Browser.SetZoomLevel(0.0);

            Browser.Load(lastSetURL);
        }

        public void Stop()
        {
            Browser.Stop();
        }

        public Bitmap GetRenderData()
        {
            if (!Started)
                return null;

            return Browser.ScreenshotOrNull(PopupBlending.Blend);
        }

        public async Task<Bitmap> GetScreenshot()
        {
            if (!Started)
                return null;

            return await Browser.ScreenshotAsync();
        }
    }
}
