using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.DashboardViewGenerator
{
    public interface IFileManager
    {
        bool DoesFileExist(string path);
        void SaveNewFile(string data, string path);
    }

    public class FileManager : IFileManager
    {
        Options _options;

        public FileManager(Options options)
        {
            this._options = options;
        }

        public bool DoesFileExist(string path)
        {
            return File.Exists(Path.Combine(_options.Output,path));
        }

        public void SaveNewFile(string data, string path)
        {
            path = Path.Combine(_options.Output, path).minifyToLast();
            string directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            File.WriteAllText(path, data);
        }
    }
}
