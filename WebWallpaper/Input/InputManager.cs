using EventHook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebWallpaper.Log;

namespace WebWallpaper.Input
{
    public class InputManager
    {

        public MouseInputState internalMouseState;
        public MouseInputState Mouse { get => internalMouseState; }

        public bool Listening { get; set; }

        private MouseWatcher mouseWatcher;

        public delegate void MouseEventArgs(MouseInputState state);
        
        public event MouseEventArgs OnMouseDown;
        public event MouseEventArgs OnMouseUp;
        public event MouseEventArgs OnMouseMove;
        public event MouseEventArgs OnMouseWheel;

        public InputManager()
        {
            internalMouseState = new MouseInputState()
            {
                mouseX = 0,
                mouseY = 0
            };

            Listening = false;
        }

        public void Listen()
        {
            if (Listening)
                return;

            Listening = true;

            using (var hookFactory = new EventHookFactory())
            {
                mouseWatcher = hookFactory.GetMouseWatcher();
                mouseWatcher.Start();
                mouseWatcher.OnMouseInput += (s, e) =>
                {
                    try
                    {
                        internalMouseState.mouseX = e.Point.x;
                        internalMouseState.mouseY = e.Point.y;

                        if (e.Message == EventHook.Hooks.MouseMessages.WM_MOUSEMOVE)
                        {
                            OnMouseMove?.Invoke(Mouse);
                        }
                        else if (e.Message == EventHook.Hooks.MouseMessages.WM_LBUTTONDOWN)
                        {
                            internalMouseState.PressedButton |= MouseInputState.MouseButtonType.LEFT;
                            OnMouseDown?.Invoke(Mouse);
                        }
                        else if (e.Message == EventHook.Hooks.MouseMessages.WM_RBUTTONDOWN)
                        {
                            internalMouseState.PressedButton |= MouseInputState.MouseButtonType.RIGHT;
                            OnMouseDown?.Invoke(Mouse);
                        }
                        else if (e.Message == EventHook.Hooks.MouseMessages.WM_LBUTTONUP)
                        {
                            OnMouseUp?.Invoke(Mouse);
                            internalMouseState.PressedButton ^= MouseInputState.MouseButtonType.LEFT;
                        }
                        else if (e.Message == EventHook.Hooks.MouseMessages.WM_RBUTTONUP)
                        {
                            OnMouseUp?.Invoke(Mouse);
                            internalMouseState.PressedButton ^= MouseInputState.MouseButtonType.RIGHT;
                        }
                        else if (e.Message == EventHook.Hooks.MouseMessages.WM_MOUSEWHEEL)
                        {
                            OnMouseWheel?.Invoke(Mouse);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("Error on input update " + ex);
                    }
                };
            }

            while (Listening) ;

            mouseWatcher.Stop();
        }
    }
}
