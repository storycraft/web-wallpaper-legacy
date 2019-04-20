using StoryWallpaper;
using StoryWallpaper.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
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

        public IRenderTarget RenderTarget { get; set; }

        public bool Initialized { get; private set; }

        public bool Running { get; private set; }

        public Bindable<bool> RenderEnabled { get; }

        public Bindable<bool> VSyncEnabled { get; }

        public long LastRender { get; private set; }

        public long FPS { get; private set; }

        public WallpaperRenderer(WebWallpaper webWallpaper)
        {
            Initialized = false;
            Running = false;

            WebWallpaper = webWallpaper;
            ScreenManager = new ScreenManager();

            Logger.Log("Screen Info: [size=" + ScreenManager.WallpaperSize.Width + " * " + ScreenManager.WallpaperSize.Height + ", refresh_rate=" + ScreenManager.WallpaperRefreshRate + "]");

            RenderTarget = WebWallpaper.BrowserManager.RenderTarget;

            RenderEnabled = webWallpaper.ConfigManager.CurrentConfig.RenderEnabled;
            VSyncEnabled = webWallpaper.ConfigManager.CurrentConfig.VSyncMode;

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

            Graphics graphics = DesktopTool.GetWallpaperGraphics();

            IntPtr hdc = graphics.GetHdc();
            IntPtr memDc = NativeWin32.CreateCompatibleDC(hdc);

            LastRender = DateTime.Now.Ticks;

            try
            {
                while (Running)
                {

                    if (RenderEnabled.Value && RenderTarget != null && RenderTarget.CanDraw)
                    {
                        RenderTarget.Draw(this, hdc, memDc);
                    }

                    long now = DateTime.Now.Ticks;

                    FPS = (now - LastRender) / 1000;
                    LastRender = now;

                    if (VSyncEnabled.Value)
                        System.Threading.Thread.Sleep(1000 / ScreenManager.WallpaperRefreshRate);
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
