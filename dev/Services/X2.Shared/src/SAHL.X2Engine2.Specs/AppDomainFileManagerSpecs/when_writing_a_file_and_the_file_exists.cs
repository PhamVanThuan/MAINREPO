using System.IO.Abstractions;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.X2Engine2.Node.AppDomain;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.AppDomainFileManagerSpecs
{
    public class when_writing_a_file_and_the_file_does_exists : WithFakes
    {
        private static AutoMocker<AppDomainFileManager> autoMocker;
        private static byte[] data;
        private static string pathToWriteTo;
        private static string folderPath;

        private Establish context = () =>
        {
            pathToWriteTo = @"c:\pathToWriteTo";
            folderPath = @"c:\folderPath";
            data = new byte[] { };
            autoMocker = new NSubstituteAutoMocker<AppDomainFileManager>();
            autoMocker.Get<IFileSystem>().Path.WhenToldTo(x => x.GetDirectoryName(folderPath)).Return(folderPath);
            autoMocker.Get<IFileSystem>().Directory.WhenToldTo(x => x.Exists(folderPath)).Return(true);
            autoMocker.Get<IFileSystem>().File.WhenToldTo(x => x.Exists(pathToWriteTo)).Return(true);
        };

        private Because of = () =>
        {
            autoMocker.ClassUnderTest.WriteFile(data, pathToWriteTo);
        };

        private It should_not_create_the_file_to_write_to = () =>
        {
            autoMocker.Get<IFileSystem>().File.WasNotToldTo(x => x.Create(pathToWriteTo));
        };
    }
}