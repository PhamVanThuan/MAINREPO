using SAHL.Core.SystemMessages;
using System;

namespace SAHL.Services.DocumentManager.Managers.DocumentFile
{
    public interface IDocumentFileManager
    {
        ISystemMessageCollection WriteFileToDatedFolder(byte[] document, string fileName, string folder, string userAccount, DateTime documentDate);

        string ReadFileFromDatedFolderAsBase64(string fileName, string folder, DateTime documentDate);
    }
}