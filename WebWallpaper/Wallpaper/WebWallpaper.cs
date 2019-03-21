using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebWallpaper.Config;
using WebWallpaper.Data;
using WebWallpaper.Input;
using WebWallpaper.Log;
using WebWallpaper.Thread;
using WebWallpaper.Wallpaper.Render;

namespace WebWallpaper.Wallpaper
{
    public class WebWallpaper : IDisposable
    {

        public ConfigManager ConfigManager { get; }

        public ThreadManager ThreadManager { get; }

        private System.Threading.Thread MainThread { get; }

        private InputThread InputThread { get; set; }
        private BrowserThread BrowserThread { get; set; }
        private RenderThread WallpaperThread { get; set; }

        public InputManager InputManager { get; }
        public WallpaperRenderer WallpaperRenderer { get; }
        public BrowserManager BrowserManager { get; }

        public DataStorage DataStorage { get; }

        public WebWallpaper(ConfigEntry defaultConfig)
        {
            MainThread = System.Threading.Thread.CurrentThread;

            DataStorage = new DataStorage()
            {
                DataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "web-wallpaper")
            };
            ConfigManager = new ConfigManager(DataStorage, defaultConfig);
            ThreadManager = new ThreadManager();
            InputManager = new InputManager();
            BrowserManager = new BrowserManager(InputManager);
            WallpaperRenderer = new WallpaperRenderer(this);
        }

        public void Start()
        {
            ConfigManager.LoadConfigAsync().ContinueWith((Task loadTask) =>
            {
                Logger.Log("Config loaded successfully");

                ConfigManager.SaveConfigAsync().ContinueWith((Task saveTask) => {
                    Logger.Log("Config saved");
                });

                WallpaperRenderer.Initialize();

                ThreadManager.Run(InputThread = new InputThread(UpdateInput));
                ThreadManager.Run(BrowserThread = new BrowserThread(BrowserInit));
                ThreadManager.Run(WallpaperThread = new RenderThread(RenderInit));
            });

            Application.Run();
        }

        #region Renderering
        public void RenderInit()
        {
            Logger.Log("Rendering thread started");

            WallpaperRenderer.Run();
        }
        #endregion

        #region Browser
        public void BrowserInit()
        {
            Logger.Log("Browser thread started");

            BrowserManager.InitBrowser(ConfigManager);
        }
        #endregion

        #region Input update
        public void UpdateInput()
        {
            Logger.Log("Input thread started");

            InputManager.Listen();
        }
        #endregion

        public void Dispose()
        {
            WallpaperRenderer.Dispose();
            BrowserManager.Dispose();
            ThreadManager.StopAll();
        }
    }
}
