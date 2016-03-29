using System.IO.Abstractions;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.X2Engine2.Node.AppDomain;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.AppDomainFileManagerSpecs
{
    public class when_asked_to_copy_an_xml_file_that_does_not_exist_in_the_destination_path : WithFakes
    {
        private static AutoMocker<AppDomainFileManager> autoMocker;
        private static string destinationPath;
        private static string sourcePath;
        private static IFileSystem fileSystem;

        private Establish context = () =>
        {
            destinationPath = @"C:\destinationPath\";
            sourcePath = @"C:\sourcePath\";
            autoMocker = new NSubstituteAutoMocker<AppDomainFileManager>();
            autoMocker.Get<IFileSystem>().Path.WhenToldTo(x => x.GetExtension(destinationPath)).Return(".xml");
            autoMocker.Get<IFileSystem>().File.WhenToldTo(x => x.Exists(destinationPath)).Return(false);
        };

        private Because of = () =>
        {
            autoMocker.ClassUnderTest.CopyFile(sourcePath, destinationPath);
        };

        private It should_copy_the_file = () =>
        {
            autoMocker.Get<IFileSystem>().File.WasToldTo(x => x.Copy(sourcePath, destinationPath));
        };
    }
}