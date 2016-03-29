namespace SAHL.Services.DocumentManager.Utils.FileSystemWriter
{
    public interface IFileSystemWriter
    {
        bool DoesFileExist(string path);

        bool DoesFolderExist(string folderPath);

        void WriteFile(byte[] file, string filePath);

        void CreateDirectoryForFile(string filePath);

        void GrantFullControlPermissions(string filePath, string userAccount);
    }
}