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
                browserSettings.WebSecurity = CefState.Disabled;
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

            Browser.SetZoomLevel(1.0);

            Browser.Load(lastSetURL);
        }

        public void Stop()
        {
            Browser.Stop();
        }

        public Bitmap GetRenderData()
        {
            if (!Ready)
                return null;

            return Browser.ScreenshotOrNull(PopupBlending.Blend);
        }

        public async Task<Bitmap> GetScreenshot()
        {
            if (!Ready)
                return null;

            return await Browser.ScreenshotAsync();
        }

        public void SimulateMouseMove(int x, int y)
        {
            if (!Ready)
                return;

            Browser.GetBrowserHost().SendMouseMoveEvent(new MouseEvent(x, y, CefEventFlags.None), false);
        }

        public void SimulateMouseClick(int x, int y, bool right)
        {
            if (!Ready)
                return;

            SimulateMouseDown(x, y, right);
            SimulateMouseUp(x, y, right);
        }

        public void SimulateMouseDown(int x, int y, bool right)
        {
            if (!Ready)
                return;

            Browser.GetBrowserHost().SendMouseClickEvent(new MouseEvent(x, y, CefEventFlags.None), right ? MouseButtonType.Right : MouseButtonType.Left, false, 1);
        }

        public void SimulateMouseUp(int x, int y, bool right)
        {
            if (!Ready)
                return;

            Browser.GetBrowserHost().SendMouseClickEvent(new MouseEvent(x, y, CefEventFlags.None), right ? MouseButtonType.Right : MouseButtonType.Left, true, 1);
        }

        public void SimulateMouseWheel(int x, int y, int deltaX, int deltaY)
        {
            if (!Ready)
                return;

            Browser.GetBrowserHost().SendMouseWheelEvent(new MouseEvent(x, y, CefEventFlags.None), deltaX, deltaY);
        }
    }
}
