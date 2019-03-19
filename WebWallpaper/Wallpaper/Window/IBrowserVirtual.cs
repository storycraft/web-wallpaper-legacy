using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebWallpaper.Wallpaper.Window
{
    public interface IBrowserVirtual
    {
        
        string CurrentURL { get; }

        Size Size { get; set; }

        bool Started { get; }
        bool Ready { get; }

        void Start();
        void Stop();

        //Nullable
        Bitmap GetLastRendered();

    }
}
