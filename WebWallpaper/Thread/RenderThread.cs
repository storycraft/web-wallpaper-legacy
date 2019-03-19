using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebWallpaper.Thread
{
    public class RenderThread : WorkerThread
    {
        public RenderThread(Action method) : base(method)
        {
        }
    }
}
