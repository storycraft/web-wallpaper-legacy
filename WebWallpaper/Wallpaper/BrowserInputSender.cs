using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebWallpaper.Config;
using WebWallpaper.Input;
using WebWallpaper.Wallpaper.Window;

namespace WebWallpaper.Wallpaper
{
    public class BrowserInputSender : IDisposable
    {
        
        public InputManager InputManager { get; }

        public IBrowserVirtual Browser { get; }

        public bool SimulateMouseMove { get; set; }
        public bool SimulateMouseDown { get; set; }
        public bool SimulateMouseUp { get; set; }
        public bool SimulateMouseWheel { get; set; }

        public BrowserInputSender(InputManager inputManager, IBrowserVirtual browser)
        {
            InputManager = inputManager;
            Browser = browser;

            SimulateMouseMove = false;
            SimulateMouseDown = false;
            SimulateMouseUp = false;
            SimulateMouseWheel = false;
            
            InputManager.OnMouseMove += OnMouseMove;
            InputManager.OnMouseDown += OnMouseDown;
            InputManager.OnMouseUp += OnMouseUp;
            InputManager.OnMouseWheel += OnMouseWheel;
        }

        protected virtual void OnMouseMove(MouseInputState state)
        {
            if (!SimulateMouseMove)
                return;

            Browser.SimulateMouseMove(state.mouseX, state.mouseY);
        }

        protected virtual void OnMouseDown(MouseInputState state)
        {
            if (!SimulateMouseDown)
                return;

            if (state.PressedButton.HasFlag(MouseInputState.MouseButtonType.LEFT))
            {
                Browser.SimulateMouseDown(state.mouseX, state.mouseY, false);
            }
            else if (state.PressedButton.HasFlag(MouseInputState.MouseButtonType.RIGHT))
            {
                Browser.SimulateMouseDown(state.mouseX, state.mouseY, true);
            }
        }

        protected virtual void OnMouseUp(MouseInputState state)
        {
            if (!SimulateMouseUp)
                return;

            if (state.PressedButton.HasFlag(MouseInputState.MouseButtonType.LEFT))
            {
                Browser.SimulateMouseUp(state.mouseX, state.mouseY, false);
            }
            else if (state.PressedButton.HasFlag(MouseInputState.MouseButtonType.RIGHT))
            {
                Browser.SimulateMouseUp(state.mouseX, state.mouseY, true);
            }
        }

        protected virtual void OnMouseWheel(MouseInputState state)
        {
            if (!SimulateMouseWheel)
                return;

            Browser.SimulateMouseWheel(state.mouseX, state.mouseY, state.wheelDeltaX, state.wheelDeltaY);
        }

        public void Dispose()
        {
            InputManager.OnMouseMove -= OnMouseMove;
            InputManager.OnMouseDown -= OnMouseDown;
            InputManager.OnMouseUp -= OnMouseUp;
            InputManager.OnMouseWheel -= OnMouseWheel;
        }
    }
}
