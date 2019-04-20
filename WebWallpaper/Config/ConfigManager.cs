using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebWallpaper.Data;
using WebWallpaper.Log;

namespace WebWallpaper.Config
{
    public class ConfigManager
    {

        public ConfigEntry DefaultConfig { get; }
        public ConfigEntry CurrentConfig { get; }

        public DataStorage ConfigStorage { get; }

        public ConfigManager(DataStorage configStorage, ConfigEntry defaultConfig)
        {
            DefaultConfig = defaultConfig;
            ConfigStorage = configStorage;

            CurrentConfig = new ConfigEntry();
        }

        public async Task LoadConfigAsync()
        {
            try
            {
                byte[] data = await ConfigStorage.Get("config.json");
                JObject rawConfig = JObject.Parse(Encoding.UTF8.GetString(data));

                CurrentConfig.StartURL.Value = rawConfig.Value<string>("startURL");
                CurrentConfig.RenderEnabled.Value = rawConfig.Value<bool>("renderEnabled");
                CurrentConfig.HandleMovement.Value = rawConfig.Value<bool>("handleMovement");
                CurrentConfig.ClickEnabled.Value = rawConfig.Value<bool>("clickEnabled");
                CurrentConfig.VSyncMode.Value = rawConfig.Value<bool>("vsyncEnabled");
            }
            catch (Exception e)
            {
                Logger.Warn("Failed to read config.json " + e.Message);
                Logger.Log("Use default instead");

                setDefault();
            }
        }

        public async Task SaveConfigAsync()
        {
            try
            {
                JObject obj = new JObject
                {
                    ["startURL"] = CurrentConfig.StartURL.Value,
                    ["renderEnabled"] = CurrentConfig.RenderEnabled.Value,
                    ["handleMovement"] = CurrentConfig.HandleMovement.Value,
                    ["clickEnabled"] = CurrentConfig.ClickEnabled.Value,
                    ["vsyncEnabled"] = CurrentConfig.VSyncMode.Value
                };

                await ConfigStorage.Set("config.json", Encoding.UTF8.GetBytes(obj.ToString()));
            }
            catch (Exception e)
            {
                Logger.Error("Cannot save config " + e);
            }
        }

        public void setDefault()
        {
            CurrentConfig.StartURL.Value = DefaultConfig.StartURL.Value;
            CurrentConfig.RenderEnabled.Value = DefaultConfig.RenderEnabled.Value;
            CurrentConfig.HandleMovement.Value = DefaultConfig.HandleMovement.Value;
            CurrentConfig.ClickEnabled.Value = DefaultConfig.ClickEnabled.Value;
            CurrentConfig.VSyncMode.Value = DefaultConfig.VSyncMode.Value;
        }
        
    }
}
