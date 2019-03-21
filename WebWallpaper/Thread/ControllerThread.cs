using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebWallpaper.Thread
{
    public class ControllerThread : WorkerThread
    {
        public ControllerThread(Action method) : base(method)
        {
        }
    }
}
