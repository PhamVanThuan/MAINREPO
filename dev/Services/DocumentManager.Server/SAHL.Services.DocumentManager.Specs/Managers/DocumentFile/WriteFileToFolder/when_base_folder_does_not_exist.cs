using System;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Services.DocumentManager.Managers.DocumentFile;
using SAHL.Services.DocumentManager.Utils.FileSystemReader;
using SAHL.Services.DocumentManager.Utils.FileSystemWriter;

namespace SAHL.Services.DocumentManager.Specs.Managers.DocumentFile.WriteFileToFolder
{
    public class when_base_folder_does_not_exist : WithFakes
    {
        private static DocumentFileManager manager;
        private static ISystemMessageCollection result;
        private static IFileSystemWriter fileSystemWriter;
        private static IFileSystemReader fileSystemReader;
        private static byte[] document;
        private static string fileName;
        private static string baseFolder;
        private static string username;
        private static DateTime documentDate;
        private static string documentString = @"bXliYXNlNjRzdHJpbmc=";

        private Establish context = () =>
        {
            fileSystemWriter = An<IFileSystemWriter>();
            fileSystemReader = An<IFileSystemReader>();
            manager = new DocumentFileManager(fileSystemWriter, fileSystemReader);

            document = new byte[documentString.Length * sizeof(char)];
            Buffer.BlockCopy(documentString.ToCharArray(), 0, document, 0, document.Length);
            fileName = "{123456-456789-78945-65466}";
            baseFolder = "C:\\ClientFiles";
            username = "SAHL\\Zorro";
            documentDate = DateTime.Now;
            fileSystemWriter.WhenToldTo(x => x.DoesFolderExist(baseFolder)).Return(false);
        };

        private Because of = () =>
        {
            result = manager.WriteFileToDatedFolder(document, fileName, baseFolder, username, documentDate);
        };

        private It should_check_if_the_base_folder_exists = () =>
        {
            fileSystemWriter.WasToldTo(x => x.DoesFolderExist(baseFolder));
        };

        private It should_add_a_error_message_if_folder_does_not_exist = () =>
        {
            result.ErrorMessages().First().Message.ShouldEqual(String.Format("Folder '{0}' does not exist.", baseFolder));
        };
    }
}