using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebWallpaper.Thread
{
    public class ThreadManager
    {

        private List<WorkerThread> ThreadList { get; } = new List<WorkerThread>();
        private ThreadAllocator Allocator { get; }

        public ThreadManager()
        {
            Allocator = CreateAllocator();
        }

        protected virtual ThreadAllocator CreateAllocator()
        {
            return new ThreadAllocator();
        }

        public void Run(WorkerThread thread)
        {
            thread.Run(Allocator);
            ThreadList.Add(thread);
        }

        public void Stop(WorkerThread thread)
        {
            thread.Stop();
            ThreadList.Remove(thread);
        }

        public bool IsDead(WorkerThread thread)
        {
            return !thread.Started;
        }

        public void StopAll()
        {
            foreach (WorkerThread thread in ThreadList)
            {
                if (!IsDead(thread))
                    thread.Stop();
            }

            ThreadList.Clear();
        }

    }
}
