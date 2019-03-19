using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebWallpaper.Input
{
    public class InputManager
    {

        public MouseInputState Mouse { get; }

        public delegate void MouseEventArgs(MouseInputState state);

        public event MouseEventArgs OnMouseClick;
        public event MouseEventArgs OnMouseDown;
        public event MouseEventArgs OnMouseMove;

        public InputManager()
        {
            Mouse = new MouseInputState()
            {
                mouseDown = false,
                mouseX = 0,
                mouseY = 0
            };
        }

        public void UpdateInput()
        {

        }
    }
}
