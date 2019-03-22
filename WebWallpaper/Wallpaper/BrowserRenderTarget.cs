using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WebWallpaper.Native;
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

        public void Draw(WallpaperRenderer renderer, IntPtr hdc, IntPtr memDc)
        {
            Point offset = renderer.ScreenManager.WallpaperOffset;
            Size wallpaperSize = renderer.ScreenManager.WallpaperSize;

            if (!BrowserManager.Browser.Size.Equals(wallpaperSize))
            {
                BrowserManager.Browser.Size = wallpaperSize;
            }

            using (Bitmap bitmap = BrowserManager.Browser.GetRenderData())
            {
                if (bitmap == null)
                    return;

                IntPtr hBitmap = bitmap.GetHbitmap();

                NativeWin32.SelectObject(memDc, hBitmap);

                NativeWin32.BitBlt(hdc,
                    offset.X, offset.Y,
                    wallpaperSize.Width, wallpaperSize.Height,
                    memDc,
                    0, 0,
                    NativeWin32.TernaryRasterOperations.SRCCOPY);

                NativeWin32.DeleteObject(hBitmap);

                //Graphics.FromHdcInternal(hdc).DrawImage(bitmap, renderer.WallpaperOffset);
            }
        }
    }
}
