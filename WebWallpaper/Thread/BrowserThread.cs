using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebWallpaper.Thread
{
    public class BrowserThread : WorkerThread
    {
        public BrowserThread(Action method) : base(method)
        {
        }
    }
}
