using StoryWallpaper;
using StoryWallpaper.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebWallpaper.Bind;
using WebWallpaper.Config;
using WebWallpaper.Display;
using WebWallpaper.Input;
using WebWallpaper.Log;
using WebWallpaper.Native;

namespace WebWallpaper.Wallpaper.Render
{
    public class WallpaperRenderer : IDisposable
    {

        public WebWallpaper WebWallpaper { get; }

        public ScreenManager ScreenManager { get; }

        public IRenderTarget RenderTarget { get => WebWallpaper.BrowserManager.RenderTarget; }

        public bool Initialized { get; private set; }

        public bool Running { get; private set; }

        public Bindable<bool> RenderEnabled { get; }

        public WallpaperRenderer(WebWallpaper webWallpaper)
        {
            Initialized = false;
            Running = false;

            WebWallpaper = webWallpaper;
            ScreenManager = new ScreenManager();

            RenderEnabled = webWallpaper.ConfigManager.CurrentConfig.RenderEnabled;

            RenderEnabled.OnChange += OnRenderModeChange;
        }

        private void OnRenderModeChange(bool oldValue, bool newValue)
        {
            if (!newValue)
            {
                DesktopTool.UpdateWallpaper();
            }
        }

        public void Initialize()
        {
            if (Initialized) return;
            Initialized = true;

            // pre spawn worker to draw faster
            if (HandleUtil.NeedSeparation)
            {
                HandleUtil.SpawnWorker();
            }
        }

        public int Run()
        {
            if (Running)
                return -1;

            Running = true;

            var graphics = DesktopTool.GetWallpaperGraphics();

            var hdc = graphics.GetHdc();
            var memDc = NativeWin32.CreateCompatibleDC(hdc);

            try
            {
                while (Running)
                {
                    if (RenderEnabled.Value && RenderTarget != null && RenderTarget.CanDraw)
                    {
                        RenderTarget.Draw(this, hdc, memDc);
                    }
                }
            } catch (Exception e)
            {
                Logger.Error("Wallpaper rendering failed retrying after 1 seconds " + e);

                NativeWin32.DeleteObject(memDc);

                System.Threading.Thread.Sleep(1000);

                if (Running)
                    Run();
            }

            return 0;
        }

        public void Dispose()
        {
            Running = false;
            DesktopTool.UpdateWallpaper();
        }
    }
}
