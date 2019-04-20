using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebWallpaper.Wallpaper.Window
{
    public interface IBrowserVirtual
    {
        
        string CurrentURL { get; }

        Size Size { get; set; }

        bool Started { get; }
        bool Ready { get; }

        bool ShouldDraw { get; set; }

        void Start();
        void Stop();

        void SimulateMouseMove(int x, int y);
        void SimulateMouseClick(int x, int y, bool right);
        void SimulateMouseDown(int x, int y, bool right);
        void SimulateMouseUp(int x, int y, bool right);
        void SimulateMouseWheel(int x, int y, int deltaX, int deltaY);

        void OpenDevTools();
        void CloseDevTools();

        //Nullable
        Bitmap GetRenderData();
        Task<Bitmap> GetScreenshot();

    }
}
