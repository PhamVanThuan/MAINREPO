using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.DocumentManager.Utils.FileSystemWriter;
using System.IO.Abstractions;

namespace SAHL.Services.DocumentManager.Specs.Utils.Writer
{
    public class when_creating_directory_for_a_file : WithFakes
    {
        private static FileSystemWriter fileSystemWriter;
        private static IFileSystem fileSystem;
        private static string filePath;

        Establish context = () =>
        {
            fileSystem = An<IFileSystem>();
            fileSystemWriter = new FileSystemWriter(fileSystem);
            filePath = "C:\\temp_dir\\somefile.txt";
        };

        Because of = () =>
        {
            fileSystemWriter.CreateDirectoryForFile(filePath);
        };

        It should_create_the_directory_from_the_file_path = () =>
        {
            fileSystem.Directory.WasToldTo(x => x.CreateDirectory(fileSystem.Path.GetDirectoryName(filePath)));
        };
    }
}
