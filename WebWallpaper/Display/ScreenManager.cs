using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        public ScreenManager()
        {

        }
    }
}
