using System.IO;
using System.IO.Abstractions;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.X2Engine2.Node.AppDomain;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.AppDomainFileManagerSpecs
{
    public class when_writing_a_file_and_the_file_does_not_exist : WithFakes
    {
        private static AutoMocker<AppDomainFileManager> autoMocker;
        private static byte[] data;
        private static string pathToWriteTo;
        private static string folderPath;
        private static Stream stream;

        Establish context = () =>
        {
            pathToWriteTo = @"c:\pathToWriteTo";
            folderPath = @"c:\folderPath";
            data = new byte[] { };
            stream = An<Stream>();
            autoMocker = new NSubstituteAutoMocker<AppDomainFileManager>();
            autoMocker.Get<IFileSystem>().Path.WhenToldTo(x => x.GetDirectoryName(folderPath)).Return(folderPath);
            autoMocker.Get<IFileSystem>().Directory.WhenToldTo(x => x.Exists(folderPath)).Return(true);
            autoMocker.Get<IFileSystem>().File.WhenToldTo(x => x.Exists(pathToWriteTo)).Return(false);
            autoMocker.Get<IFileSystem>().File.WhenToldTo(x => x.Create(pathToWriteTo)).Return(stream);
        };

        Because of = () =>
        {
            autoMocker.ClassUnderTest.WriteFile(data, pathToWriteTo);
        };

        It should_write_the_data_to_stream_that_was_created = () =>
        {
            stream.WasToldTo(x => x.Write(data, 0, data.Length));
        };

        It should_close_the_file_stream_once_completed = () =>
        {
            stream.WasToldTo(x => x.Close());
        };
    }
}