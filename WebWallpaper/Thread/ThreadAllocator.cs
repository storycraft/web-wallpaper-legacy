using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebWallpaper.Thread
{
    public class ThreadAllocator
    {

        public virtual System.Threading.Thread Allocate(Action method)
        {
            var thread = new System.Threading.Thread(new System.Threading.ThreadStart(method));
            return thread;
        }

    }
}
