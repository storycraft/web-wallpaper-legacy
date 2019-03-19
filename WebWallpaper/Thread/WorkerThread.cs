using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebWallpaper.Thread
{
    public class WorkerThread
    {

        public bool Started { get; private set; }

        protected Action Method { get; }
        protected System.Threading.Thread Thread { get; set; }

        public WorkerThread(Action method)
        {
            Method = method;
        }

        public void Run(ThreadAllocator allocator)
        {
            if (Started)
            {
                throw new ThreadAlreadyStartedException();
            }
            Started = true;

            Thread = allocator.Allocate(Method);

            Thread.Start();
        }

        public void Stop()
        {
            if (!Started)
            {
                throw new ThreadNotStartedException();
            }
            Started = false;

            if (Thread.IsAlive)
                Thread.Abort();
        }
    }
}
