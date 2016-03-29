using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.DocumentManager.Utils.FileSystemReader;
using System.IO.Abstractions;

namespace SAHL.Services.DocumentManager.Specs.Utils.Reader
{
    public class when_reading_a_file : WithFakes
    {
        private static IFileSystem fileSystem;
        private static FileSystemReader fileStreamReader;
        private static string filePath;
        private static byte[] documentByteData, expectedDocumentByteData;

        Establish context = () =>
        {
            filePath = "C:\\temp\\somefile.txt";
            fileSystem = An<IFileSystem>();
            fileStreamReader = new FileSystemReader(fileSystem);
            expectedDocumentByteData = new byte[] { 1, 22, 25, 8 };
            fileSystem.File.WhenToldTo(x => x.ReadAllBytes(filePath)).Return(expectedDocumentByteData);
        };

        Because of = () =>
        {
            documentByteData = fileStreamReader.ReadFile(filePath);
        };

        It should_return_document_byte_array = () =>
        {
            documentByteData.ShouldNotBeNull();
        };
    }
}
