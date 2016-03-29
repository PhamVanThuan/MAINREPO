using System.Configuration;
using SAHL.Core.SystemMessages;
using SAHL.Services.DocumentManager.Utils.FileSystemReader;
using SAHL.Services.DocumentManager.Utils.FileSystemWriter;
using System;
using SAHL.Services.DocumentManager.Utils.PdfConverter;

namespace SAHL.Services.DocumentManager.Managers.DocumentFile
{
    public class DocumentFileManager : IDocumentFileManager
    {
        private IFileSystemWriter fileSystemWriter;
        private IFileSystemReader fileSystemReader;

        public DocumentFileManager(IFileSystemWriter fileSystemWriter, IFileSystemReader fileSystemReader)
        {
            this.fileSystemWriter = fileSystemWriter;
            this.fileSystemReader = fileSystemReader;
        }

        public ISystemMessageCollection WriteFileToDatedFolder(byte[] document, string fileName, string baseFolder, string userAccount, DateTime documentDate)
        {
            var messages = SystemMessageCollection.Empty();

            if (!fileSystemWriter.DoesFolderExist(baseFolder))
            {
                messages.AddMessage(new SystemMessage(String.Format("Folder '{0}' does not exist.", baseFolder), SystemMessageSeverityEnum.Error));
                return messages;
            }
            string filePath = String.Format("{0}\\{1}\\{2}\\{3}", baseFolder, documentDate.Year, documentDate.ToString("MM"), fileName);
            fileSystemWriter.CreateDirectoryForFile(filePath);
            fileSystemWriter.WriteFile(document, filePath);
            fileSystemWriter.GrantFullControlPermissions(filePath, userAccount);

            return messages;
        }

        public string ReadFileFromDatedFolderAsBase64(string fileName, string baseFolder, DateTime documentDate)
        {
            string filePath = string.Format("{0}\\{1}\\{2}\\{3}", baseFolder, documentDate.Year, documentDate.ToString("MM"), fileName);
            string fileContents = string.Empty;

            var documentHostName = ConfigurationManager.AppSettings.Get("DocumentHostName") ?? "sahl-ds02";
            filePath = filePath.Replace("sahl-ds02\\losscontrol$", string.Format("{0}\\losscontrol$", documentHostName));

            if (fileSystemWriter.DoesFileExist(filePath))
            {
                var fileByteArray = fileSystemReader.ReadFile(filePath);

                fileByteArray = TransformTifFormat(fileByteArray);

                fileContents = Convert.ToBase64String(fileByteArray);
            }

            return fileContents;
        }

        private byte[] TransformTifFormat(byte[] fileByteArray)
        {
            if (IsTifFileFormat(fileByteArray))
            {
                var pdfConverter = new PdfConverter();
                return pdfConverter.ConvertImageToPdf(fileByteArray);
            }
            return fileByteArray;
        }

        private static bool IsTifFileFormat(byte[] readFile)
        {
            return readFile[0] == 0x49 &&
                   readFile[1] == 0x49 &&
                   readFile[2] == 0x2A;
        }
    }
}