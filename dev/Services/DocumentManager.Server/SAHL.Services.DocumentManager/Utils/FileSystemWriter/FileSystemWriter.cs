using System.IO;
using System.IO.Abstractions;
using System.Security.AccessControl;

namespace SAHL.Services.DocumentManager.Utils.FileSystemWriter
{
    public class FileSystemWriter : IFileSystemWriter
    {
        private IFileSystem fileSystem;

        public FileSystemWriter(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public bool DoesFileExist(string path)
        {
            return fileSystem.File.Exists(path);
        }

        public bool DoesFolderExist(string folderPath)
        {
            return fileSystem.Directory.Exists(folderPath);
        }

        public void CreateDirectoryForFile(string filePath)
        {
            fileSystem.Directory.CreateDirectory(fileSystem.Path.GetDirectoryName(filePath));
        }

        public void WriteFile(byte[] file, string filePath)
        {
            using (Stream fileStream = fileSystem.File.Create(filePath))
            {
                fileStream.Write(file, 0, file.Length);
                fileStream.Close();
            }
        }

        public void GrantFullControlPermissions(string filePath, string userAccount)
        {
            FileSecurity fileSecurity = fileSystem.File.GetAccessControl(filePath);
            fileSecurity.AddAccessRule(new FileSystemAccessRule(userAccount, FileSystemRights.FullControl, AccessControlType.Allow));
            fileSystem.File.SetAccessControl(filePath, fileSecurity);
        }
    }
}