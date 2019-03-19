using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebWallpaper.Data
{
    public abstract class DiskStorage<T> : IStorage<T>
    {

        public abstract Task<T> Get(string key);
        public abstract Task Set(string key, T value);

    }
}
