using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebWallpaper.Native;

namespace WebWallpaper.Display
{
    public class ScreenManager
    {

        public Size WallpaperSize
        {
            get
            {
                return Screen.PrimaryScreen.Bounds.Size;
            }
        }

        public Point WallpaperOffset
        {
            get
            {
                Rectangle rect = SystemInformation.VirtualScreen;
                return new Point(-rect.Left, -rect.Top);
            }
        }

        public int WallpaperRefreshRate
        {
            get;
        }

        public ScreenManager()
        {
            NativeWin32.DEVMODE devMode = new NativeWin32.DEVMODE();
            if (!NativeWin32.EnumDisplaySettings(Screen.PrimaryScreen.DeviceName, NativeWin32.ENUM_CURRENT_SETTINGS, ref devMode))
            {
                WallpaperRefreshRate = 250;
            } else
            {
                WallpaperRefreshRate = devMode.dmDisplayFrequency;
            }
        }
    }
}
