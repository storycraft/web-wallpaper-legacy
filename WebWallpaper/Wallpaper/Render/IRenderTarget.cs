using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebWallpaper.Wallpaper.Render
{
    public interface IRenderTarget
    {

        bool CanDraw { get; }

        bool Draw(WallpaperRenderer renderer, IntPtr hdc, IntPtr memDc);

    }
}
