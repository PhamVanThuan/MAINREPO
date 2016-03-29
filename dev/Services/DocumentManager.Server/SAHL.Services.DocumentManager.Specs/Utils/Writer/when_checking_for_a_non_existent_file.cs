using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.DocumentManager.Utils.FileSystemWriter;
using System.IO.Abstractions;

namespace SAHL.Services.DocumentManager.Specs.Utils
{
    public class when_checking_for_a_non_existent_file : WithFakes
    {
        private static FileSystemWriter fileSystemWriter;
        private static IFileSystem fileSystem;
        private static bool result;
        private static string filePath;

        Establish context = () =>
        {
            fileSystem = An<IFileSystem>();
            fileSystemWriter = new FileSystemWriter(fileSystem);
            filePath = "C:\\temp\\somefile.txt";
            fileSystemWriter.WhenToldTo(x => x.DoesFileExist(filePath)).Return(false);
        };

        Because of = () =>
        {
            result = fileSystemWriter.DoesFileExist(filePath);
        };

        It should_return_true = () =>
        {
            result.ShouldBeFalse();
        };
    }
}
