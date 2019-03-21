using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebWallpaper.Bind;

namespace WebWallpaper.Config
{
    public class ConfigEntry
    {
        public Bindable<string> StartURL { get; }
        public Bindable<bool> RenderEnabled { get; }
        public Bindable<bool> HandleMovement { get; }
        public Bindable<bool> ClickEnabled { get; }

        public ConfigEntry(string startURL = "", bool renderEnabled = false, bool handleMovement = false, bool clickEnabled = false)
        {
            StartURL = new Bindable<string>(startURL);
            RenderEnabled = new Bindable<bool>(renderEnabled);
            HandleMovement = new Bindable<bool>(handleMovement);
            ClickEnabled = new Bindable<bool>(clickEnabled);

        }
    }

}
