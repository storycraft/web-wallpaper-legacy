using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebWallpaper.Config;
using WebWallpaper.Wallpaper.Window;

namespace WebWallpaper.Wallpaper
{
    public class BrowserManager : IDisposable
    {
        public BrowserRenderTarget RenderTarget { get; }

        public bool Ready { get; private set; }

        public CefSharpBrowser Browser { get; private set; }

        public BrowserManager()
        {
            RenderTarget = new BrowserRenderTarget(this);
        }

        #region Browser Thread
        public void InitBrowser(ConfigManager configManager)
        {
            if (Ready)
                return;

            ChromiumFactory.Instance.Init();

            Browser = ChromiumFactory.Instance.CreateBrowser(new BrowserSettings() { SkipCorsCheck = true }, configManager.CurrentConfig.startURL);

            Browser.Start();
            Ready = true;
        }
        #endregion

        public void Dispose()
        {
            Browser.Stop();
            ChromiumFactory.Instance.Shutdown();

            Ready = false;
        }
    }
}
