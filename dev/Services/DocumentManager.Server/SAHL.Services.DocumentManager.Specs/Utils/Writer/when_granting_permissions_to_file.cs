using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.DocumentManager.Utils.FileSystemWriter;
using System.IO.Abstractions;
using System.Security.AccessControl;

namespace SAHL.Services.DocumentManager.Specs.Utils.Writer
{
    public class when_granting_permissions_to_file : WithFakes
    {
        private static FileSystemWriter fileSystemWriter;
        private static IFileSystem fileSystem;
        private static string filePath;
        private static FileSecurity fileSecurity;

        Establish context = () =>
        {
            fileSecurity = new FileSecurity();
            fileSystem = An<IFileSystem>();
            fileSystemWriter = new FileSystemWriter(fileSystem);
            filePath = "C:\\temp\\somefile.txt";
            fileSystem.File.WhenToldTo(x => x.GetAccessControl(filePath)).Return(fileSecurity);
        };

        Because of = () =>
        {
            fileSystemWriter.GrantFullControlPermissions(filePath, "Guest");
        };

        It should_the_access_control_permissions = () =>
        {
            fileSystem.File.WasToldTo(x => x.SetAccessControl(filePath, fileSecurity));
        };
    }
}
