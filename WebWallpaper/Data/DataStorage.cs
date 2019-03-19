using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebWallpaper.Data
{
    public class DataStorage : DiskStorage<byte[]>
    {

        public string DataDir { get; set; }
        public int BufferSize { get; set; }

        public DataStorage()
        {
            BufferSize = 4194304/*4 MB*/;
        }

        public virtual string GetFilePath(string key)
        {
            return Path.Combine(DataDir, key);
        }

        public override async Task<byte[]> Get(string key)
        {
            Directory.CreateDirectory(DataDir);
            using (FileStream fileStream = new FileStream(GetFilePath(key), FileMode.Open))
            {
                using (MemoryStream memStream = new MemoryStream())
                {
                    fileStream.Position = 0;

                    int bufferSize = BufferSize;
                    long length = fileStream.Length;

                    await fileStream.CopyToAsync(memStream);

                    return memStream.ToArray();
                }
            }
        }

        public override async Task Set(string key, byte[] value)
        {
            Directory.CreateDirectory(DataDir);
            using (FileStream fileStream = new FileStream(GetFilePath(key), FileMode.Create))
            {
                fileStream.Position = 0;

                await fileStream.WriteAsync(value, 0, value.Length);
            }
        }
    }
}
