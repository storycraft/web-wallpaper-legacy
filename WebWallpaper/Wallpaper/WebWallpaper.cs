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

        public ConfigEntry DefaultConfig { get; }
        public ConfigEntry ProgramConfig { get; protected set; }

        public ThreadManager ThreadManager { get; }

        private InputThread InputThread { get; set; }
        private BrowserThread BrowserThread { get; set; }
        private RenderThread WallpaperThread { get; set; }

        public InputManager InputManager { get; }
        public WallpaperRenderer WallpaperRenderer { get; }
        public BrowserManager BrowserManager { get; }

        public DataStorage DataStorage { get; }

        public WebWallpaper(ConfigEntry defaultConfig) : this()
        {
            DefaultConfig = defaultConfig;
        }

        protected WebWallpaper()
        {
            DataStorage = new DataStorage()
            {
                DataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "web-wallpaper")
            };
            ThreadManager = new ThreadManager();
            InputManager = new InputManager();
            BrowserManager = new BrowserManager();
            WallpaperRenderer = new WallpaperRenderer(BrowserManager.RenderTarget);
        }

        public void Start()
        {
            LoadOrDefaultConfigAsync().ContinueWith((Task<ConfigEntry> loadTask) =>
            {
                ProgramConfig = loadTask.Result;
                Logger.Log("Config loaded successfully");

                SaveConfigAsync().ContinueWith((Task saveTask) => {
                    Logger.Log("Config saved");
                });

                WallpaperRenderer.Initialize(ProgramConfig);

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

            BrowserManager.InitBrowser(ProgramConfig);
        }
        #endregion

        #region Input update
        public void UpdateInput()
        {
            Logger.Log("Input thread started");

            while (InputThread.Started)
            {
                InputManager.UpdateInput();
            }
        }
        #endregion

        #region Config
        private async Task<ConfigEntry> LoadOrDefaultConfigAsync()
        {
            try
            {
                byte[] data = await DataStorage.Get("config.json");
                JObject rawConfig = JObject.Parse(Encoding.UTF8.GetString(data));

                ConfigEntry entry = new ConfigEntry()
                {
                    startURL = rawConfig.Value<string>("startURL"),
                    renderEnabled = rawConfig.Value<bool>("renderEnabled"),
                    handleMovement = rawConfig.Value<bool>("handleMovement"),
                    clickEnabled = rawConfig.Value<bool>("clickEnabled")
                };

                return entry;
            }
            catch (Exception e)
            {
                Logger.Warn("Failed to read config.json " + e.Message);
                Logger.Log("Use default instead");
            }

            return DefaultConfig;
        }

        private async Task SaveConfigAsync()
        {
            try
            {
                JObject obj = new JObject
                {
                    ["startURL"] = ProgramConfig.startURL,
                    ["renderEnabled"] = ProgramConfig.renderEnabled,
                    ["handleMovement"] = ProgramConfig.handleMovement,
                    ["clickEnabled"] = ProgramConfig.clickEnabled
                };

                await DataStorage.Set("config.json", Encoding.UTF8.GetBytes(obj.ToString()));
            } catch (Exception e)
            {
                Logger.Error("Cannot save config " + e);
            }
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
