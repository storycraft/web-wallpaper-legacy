using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WebWallpaper.Native;
using WebWallpaper.Wallpaper.Render;

namespace WebWallpaper.Wallpaper
{
    public class BrowserRenderTarget : IRenderTarget
    {

        private volatile Bitmap lastRenderData;

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

        public bool Draw(WallpaperRenderer renderer, IntPtr hdc, IntPtr memDc)
        {
            Size wallpaperSize = renderer.ScreenManager.WallpaperSize;

            if (!BrowserManager.Browser.Size.Equals(wallpaperSize))
            {
                BrowserManager.Browser.Size = wallpaperSize;
            }

            Bitmap bitmap = BrowserManager.Browser.GetRenderData();

            if (bitmap == null)
            {
                if (lastRenderData != null)
                    bitmap = lastRenderData;
                else
                {
                    return false;
                }
            } else
            {
                lastRenderData?.Dispose();
            }

            IntPtr hBitmap = bitmap.GetHbitmap();

            NativeWin32.SelectObject(memDc, hBitmap);

            Point offset = renderer.ScreenManager.WallpaperOffset;

            bool flag = NativeWin32.BitBlt(hdc,
                offset.X, offset.Y,
                wallpaperSize.Width, wallpaperSize.Height,
                memDc,
                0, 0,
                NativeWin32.TernaryRasterOperations.SRCCOPY);

            NativeWin32.DeleteObject(hBitmap);
            
            lastRenderData = bitmap;

            return flag;
        }
    }
}
