using StoryWallpaper;
using StoryWallpaper.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebWallpaper.Config;
using WebWallpaper.Input;
using WebWallpaper.Log;

namespace WebWallpaper.Wallpaper.Render
{
    public class WallpaperRenderer : IDisposable
    {

        public ConfigManager ConfigManager { get; private set; }

        public IRenderTarget RenderTarget { get; set; }

        public bool Initialized { get; private set; }

        public bool Running { get; private set; }

        private bool renderEnabled;
        public bool RenderEnabled
        {
            get => renderEnabled;

            set
            {
                renderEnabled = value;
                
                if (!value)
                {
                    DesktopTool.UpdateWallpaper();
                }
            }
        }

        public Size WallpaperSize
        {
            get
            {
                return Screen.PrimaryScreen.Bounds.Size;
            }
        }

        public Point WallpaperOffset
        {
            get
            {
                Rectangle rect = SystemInformation.VirtualScreen;
                return new Point(-rect.Left, -rect.Top);
            }
        }

        public WallpaperRenderer(IRenderTarget target)
        {
            Initialized = false;
            Running = false;

            RenderTarget = target;
        }

        public void Initialize(ConfigManager configManager)
        {
            if (Initialized) return;
            Initialized = true;

            ConfigManager = configManager;

            renderEnabled = ConfigManager.CurrentConfig.renderEnabled;

            // pre spawn worker to draw faster
            if (HandleUtil.NeedSeparation)
            {
                HandleUtil.SpawnWorker();
            }
        }

        public int Run()
        {
            Running = true;

            try
            {
                var graphics = DesktopTool.GetWallpaperGraphics();

                while (Running)
                {
                    if (renderEnabled && RenderTarget != null && RenderTarget.CanDraw)
                    {
                        RenderTarget.Draw(this, graphics);
                    }
                }
            } catch (Exception e)
            {
                Logger.Error("Wallpaper rendering failed retrying after 1 seconds " + e);
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
