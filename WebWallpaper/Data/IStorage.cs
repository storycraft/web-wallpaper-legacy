using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebWallpaper.Data
{
    
    // Do async IO work
    public interface IStorage<T>
    {
        Task<T> Get(string key);
        Task Set(string key, T value);
    }
}
