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

        public ConfigEntry DefaultConfig { get; set; }
        public ConfigEntry CurrentConfig { get; set; }

        public DataStorage ConfigStorage { get; }

        public ConfigManager(DataStorage configStorage, ConfigEntry defaultConfig)
        {
            DefaultConfig = defaultConfig;
            ConfigStorage = configStorage;
        }

        public async Task LoadConfigAsync()
        {
            try
            {
                byte[] data = await ConfigStorage.Get("config.json");
                JObject rawConfig = JObject.Parse(Encoding.UTF8.GetString(data));

                ConfigEntry entry = new ConfigEntry()
                {
                    startURL = rawConfig.Value<string>("startURL"),
                    renderEnabled = rawConfig.Value<bool>("renderEnabled"),
                    handleMovement = rawConfig.Value<bool>("handleMovement"),
                    clickEnabled = rawConfig.Value<bool>("clickEnabled")
                };

                CurrentConfig = entry;
            }
            catch (Exception e)
            {
                Logger.Warn("Failed to read config.json " + e.Message);
                Logger.Log("Use default instead");

                CurrentConfig = DefaultConfig;
            }
        }

        public async Task SaveConfigAsync()
        {
            try
            {
                JObject obj = new JObject
                {
                    ["startURL"] = CurrentConfig.startURL,
                    ["renderEnabled"] = CurrentConfig.renderEnabled,
                    ["handleMovement"] = CurrentConfig.handleMovement,
                    ["clickEnabled"] = CurrentConfig.clickEnabled
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
            CurrentConfig = DefaultConfig;
        }
        
    }
}
