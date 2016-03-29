using System;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Services.DocumentManager.Managers.DocumentFile;
using SAHL.Services.DocumentManager.Utils.FileSystemReader;
using SAHL.Services.DocumentManager.Utils.FileSystemWriter;

namespace SAHL.Services.DocumentManager.Specs.Managers.DocumentFile.WriteFileToFolder
{
    public class when_folder_does_exist : WithFakes
    {
        private static DocumentFileManager manager;
        private static ISystemMessageCollection result;
        private static IFileSystemWriter fileSystemWriter;
        private static IFileSystemReader fileSystemReader;
        private static byte[] document;
        private static string fileName;
        private static string userAccount;
        private static string baseFolder;
        private static string filePath;
        private static DateTime documentDate;
        private static string documentString;

        private Establish context = () =>
        {
            fileSystemWriter = An<IFileSystemWriter>();
            manager = new DocumentFileManager(fileSystemWriter, fileSystemReader);
            documentString = @"SGVsbG8=";
            document = new byte[documentString.Length * sizeof(char)];
            Buffer.BlockCopy(documentString.ToCharArray(), 0, document, 0, document.Length);
            fileName = "{01234-4567-8945-4568}";
            baseFolder = "C:\\ClientFiles";
            userAccount = "SAHL\\TestUser";
            documentDate = new DateTime(2014, 8, 22);
            fileSystemWriter.WhenToldTo(x => x.DoesFolderExist(baseFolder)).Return(true);
            filePath = string.Format("{0}\\{1}\\{2}\\{3}", baseFolder, documentDate.Year, documentDate.ToString("MM"), fileName);
        };

        private Because of = () =>
        {
            result = manager.WriteFileToDatedFolder(document, fileName, baseFolder, userAccount, documentDate);
        };

        private It should_check_if_folder_exists = () =>
        {
            fileSystemWriter.WasToldTo(x => x.DoesFolderExist(baseFolder));
        };

        private It should_create_the_directory_for_the_new_file = () =>
        {
            fileSystemWriter.WasToldTo(x => x.CreateDirectoryForFile(filePath));
        };

        private It should_write_the_file_to_the_folder_location_specified = () =>
        {
            fileSystemWriter.WasToldTo(x => x.WriteFile(document, filePath));
        };

        private It should_set_the_permissions_on_the_file_for_the_given_user = () =>
        {
            fileSystemWriter.WasToldTo(x => x.GrantFullControlPermissions(filePath, userAccount));
        };

        private It should_return_no_error_messages = () =>
        {
            result.ErrorMessages().ShouldBeEmpty();
        };
    }
}