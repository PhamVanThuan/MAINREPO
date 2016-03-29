using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.DocumentManager.Utils.FileSystemWriter;
using System.IO.Abstractions;

namespace SAHL.Services.DocumentManager.Specs.Utils
{
    public class when_checking_existing_folder : WithFakes
    {
        private static FileSystemWriter fileSystemWriter;
        private static IFileSystem fileSystem;
        private static bool result;
        private static string folderPath;

        Establish context = () =>
        {
            fileSystem = An<IFileSystem>();
            fileSystemWriter = new FileSystemWriter(fileSystem);
            folderPath = "C:\\temp";
            fileSystemWriter.WhenToldTo(x => x.DoesFolderExist(folderPath)).Return(true);
        };

        Because of = () =>
        {
            result = fileSystemWriter.DoesFolderExist(folderPath);
        };

        It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}
