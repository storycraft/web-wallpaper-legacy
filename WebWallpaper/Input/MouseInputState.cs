using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebWallpaper.Input
{
    public struct MouseInputState
    {
        public int mouseX;
        public int mouseY;

        public int wheelDeltaX;
        public int wheelDeltaY;

        public MouseButtonType PressedButton;

        public enum MouseButtonType
        {
            LEFT = 0x01,
            RIGHT = 0x02,
            ALL = 0x03
        }
    }
}
