using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebWallpaper.Wallpaper.Window;

namespace WebWallpaper.Wallpaper
{
    public abstract class BrowserFactory<T>
    {

        public bool Ready { get; protected set; }

        public BrowserFactory()
        {
            Ready = false;
        }

        public abstract void Init();

        public abstract void Shutdown();

        public T CreateBrowser(BrowserSettings settings)
        {
            return CreateBrowser(settings, "");
        }

        public abstract T CreateBrowser(BrowserSettings settings, string url);

    }
}
