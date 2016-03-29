using System.IO;
using System.IO.Abstractions;

namespace SAHL.Services.DocumentManager.Utils.FileSystemReader
{
    public class FileSystemReader : IFileSystemReader
    {
        private IFileSystem fileSystem;

        public FileSystemReader(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public byte[] ReadFile(string path)
        {
            return fileSystem.File.ReadAllBytes(path);
        }
    }
}
