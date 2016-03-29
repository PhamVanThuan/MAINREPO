using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.DocumentManager.Managers.DocumentFile;
using SAHL.Services.DocumentManager.Utils.FileSystemReader;
using SAHL.Services.DocumentManager.Utils.FileSystemWriter;
using System;

namespace SAHL.Services.DocumentManager.Specs.Managers.DocumentFile.ReadFileFromFolder
{
    public class when_file_does_not_exist : WithFakes
    {
        static DocumentFileManager documentFileManager;
        static IFileSystemWriter fileSystemWriter;
        static IFileSystemReader fileSystemReader;
        static string result, fileName, baseFolder, path;
        static DateTime documentDate;

        Establish context = () =>
        {
            fileSystemWriter = An<IFileSystemWriter>();
            fileSystemReader = An<IFileSystemReader>();
            documentFileManager = new DocumentFileManager(fileSystemWriter, fileSystemReader);

            fileName = "{01234-4567-8945-4568}";
            baseFolder = "C:\\ClientFiles";
            documentDate = new DateTime(2014, 2, 22);
            path = string.Format("{0}\\{1}\\{2}\\{3}", baseFolder, documentDate.Year, documentDate.ToString("MM"), fileName);

            fileSystemWriter.WhenToldTo(x => x.DoesFileExist(path)).Return(false);
        };

        Because of = () =>
        {
            result = documentFileManager.ReadFileFromDatedFolderAsBase64(fileName, baseFolder, documentDate);
        };

        It should_check_if_file_exists = () =>
        {
            fileSystemWriter.WasToldTo(x => x.DoesFileExist(path));
        };

        It should_not_read_file_from_folder = () =>
        {
            fileSystemReader.WasNotToldTo(x => x.ReadFile(path));
        };

        It should_not_return_the_file_contents = () =>
        {
            (string.IsNullOrEmpty(result)).ShouldBeTrue();
        };
    }
}
