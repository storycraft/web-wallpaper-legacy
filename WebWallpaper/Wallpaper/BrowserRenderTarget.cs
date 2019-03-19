using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebWallpaper.Wallpaper.Render;

namespace WebWallpaper.Wallpaper
{
    public class BrowserRenderTarget : IRenderTarget
    {

        public BrowserManager BrowserManager { get; }

        public BrowserRenderTarget(BrowserManager manager)
        {
            BrowserManager = manager;
        }

        public bool CanDraw
        {
            get
            {
                if (!BrowserManager.Ready || !BrowserManager.Browser.Ready)
                {
                    return false;
                }

                return true;
            }
        }

        public void Draw(WallpaperRenderer renderer, Graphics graphics)
        {
            Size wallpaperSize = renderer.WallpaperSize;
            if (!BrowserManager.Browser.Size.Equals(wallpaperSize))
            {
                BrowserManager.Browser.Size = wallpaperSize;
            }

            Bitmap bitmap = BrowserManager.Browser.GetRenderData();

            if (bitmap == null)
                return;

            graphics.DrawImage(bitmap, renderer.WallpaperOffset);
        }
    }
}
