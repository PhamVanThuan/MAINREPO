using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.TileModelGenerator
{
    public interface IFileManager
    {
        bool DoesFileExist(string path);
        void SaveNewFile(string data, string path);
    }

    public class FileManager : IFileManager
    {
        public bool DoesFileExist(string path)
        {
            return File.Exists(path);
        }

        public void SaveNewFile(string data, string path)
        {
            string directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            File.WriteAllText(path, data);
        }
    }
}
