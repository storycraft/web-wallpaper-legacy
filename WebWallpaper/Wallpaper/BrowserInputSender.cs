using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebWallpaper.Config;
using WebWallpaper.Input;

namespace WebWallpaper.Wallpaper
{
    public class BrowserInputSender
    {
        
        public InputManager InputManager { get; }

        public bool SimulateMouseMove { get; set; }
        public bool SimulateMouseClick { get; set; }
        public bool SimulateMouseDown { get; set; }
        public bool SimulateMouseUp { get; set; }
        public bool SimulateMouseWheel { get; set; }

        public BrowserInputSender(InputManager inputManager)
        {
            InputManager = inputManager;

            SimulateMouseMove = false;
            SimulateMouseClick = false;
            SimulateMouseDown = false;
            SimulateMouseUp = false;
            SimulateMouseWheel = false;
        }

        protected virtual void OnMouseMove()
        {

        }
    }
}
