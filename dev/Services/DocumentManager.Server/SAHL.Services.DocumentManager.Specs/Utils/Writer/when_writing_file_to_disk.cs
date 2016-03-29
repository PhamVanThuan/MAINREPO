using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.DocumentManager.Utils.FileSystemWriter;
using System.IO;
using System.IO.Abstractions;

namespace SAHL.Services.DocumentManager.Specs.Utils.Writer
{
    public class when_writing_file_to_disk : WithFakes
    {
        private static FileSystemWriter fileSystemWriter;
        private static IFileSystem fileSystem;
        private static string filePath;
        private static byte[] data;
        private static Stream fs;

        private Establish context = () =>
        {
            data = new byte[] { 2, 12, 48 };
            fileSystem = An<IFileSystem>();
            fileSystemWriter = new FileSystemWriter(fileSystem);
            filePath = "C:\\fileSystemTest\\somefile.txt";
            Directory.CreateDirectory("c:\\fileSystemTest");
            fs = File.Create(filePath);
            fileSystem.File.WhenToldTo(x => x.Create(filePath)).Return(fs);
        };

        private Because of = () =>
        {
            fileSystemWriter.WriteFile(data, filePath);
        };

        private It should_write_the_file_to_write_to = () =>
        {
            fileSystem.File.WasToldTo(x => x.Create(filePath));
        };

        private Cleanup after = () =>
        {
            fs.Close();
            File.Delete(filePath);
            Directory.Delete("c:\\fileSystemTest");
        };
    }
}