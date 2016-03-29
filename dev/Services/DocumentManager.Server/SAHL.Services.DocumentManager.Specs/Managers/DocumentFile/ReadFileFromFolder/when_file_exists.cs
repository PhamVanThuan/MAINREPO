using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.DocumentManager.Managers.DocumentFile;
using SAHL.Services.DocumentManager.Utils.FileSystemReader;
using SAHL.Services.DocumentManager.Utils.FileSystemWriter;
using System;

namespace SAHL.Services.DocumentManager.Specs.Managers.DocumentFile.ReadFileFromFolder
{
    public class when_file_exists : WithFakes
    {
        private static DocumentFileManager documentFileManager;
        private static IFileSystemWriter fileSystemWriter;
        private static IFileSystemReader fileSystemReader;
        private static string result, fileName, baseFolder, path;
        private static DateTime documentDate;
        private static byte[] fileContentByteArray;

        private Establish context = () =>
        {
            fileSystemWriter = An<IFileSystemWriter>();
            fileSystemReader = An<IFileSystemReader>();
            documentFileManager = new DocumentFileManager(fileSystemWriter, fileSystemReader);

            fileName = "{01234-4567-8945-4568}";
            baseFolder = "C:\\ClientFiles";
            fileContentByteArray = new byte[] { 0, 22, 12, 34, 55 };
            documentDate = new DateTime(2014, 2, 22);
            path = string.Format("{0}\\{1}\\{2}\\{3}", baseFolder, documentDate.Year, documentDate.ToString("MM"), fileName);

            fileSystemWriter.WhenToldTo(x => x.DoesFileExist(path)).Return(true);
            fileSystemReader.WhenToldTo(x => x.ReadFile(path)).Return(fileContentByteArray);
        };

        private Because of = () =>
        {
            result = documentFileManager.ReadFileFromDatedFolderAsBase64(fileName, baseFolder, documentDate);
        };

        private It should_check_if_file_exists = () =>
        {
            fileSystemWriter.WasToldTo(x => x.DoesFileExist(path));
        };

        private It should_read_file_from_folder = () =>
        {
            fileSystemReader.WasToldTo(x => x.ReadFile(path));
        };

        private It should_return_file_content_as_base_64_string = () =>
        {
            (string.IsNullOrEmpty(result)).ShouldBeFalse();
        };
    }
}