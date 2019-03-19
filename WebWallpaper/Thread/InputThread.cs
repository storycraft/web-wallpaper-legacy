using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebWallpaper.Thread
{
    public class InputThread : WorkerThread
    {
        public InputThread(Action method) : base(method)
        {
        }
    }
}
