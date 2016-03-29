using System.Collections.Generic;
using System.IO.Abstractions;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.X2Engine2.Node.AppDomain;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.AppDomainFileManagerSpecs
{
    public class when_getting_installed_nuget_files_and_no_directories_exits_in_the_nuget_package_path : WithFakes
    {
        private static AutoMocker<AppDomainFileManager> autoMocker;
        private static List<string> files;
        private static string nugetPackagePath;

        private Establish context = () =>
        {
            autoMocker = new NSubstituteAutoMocker<AppDomainFileManager>();
            autoMocker.Get<IFileSystem>().Directory.WhenToldTo(x => x.GetDirectories(Param.IsAny<string>(), "*", System.IO.SearchOption.TopDirectoryOnly))
                .Return(new string[] { });
        };

        private Because of = () =>
        {
            files = autoMocker.ClassUnderTest.GetInstalledNuGetFiles(nugetPackagePath);
        };

        private It should_not_continue_looking_for_dlls = () =>
        {
            autoMocker.Get<IFileSystem>().Directory.WasNotToldTo(x => x.GetDirectories(Param.IsAny<string>()));
        };

        private It should_return_an_empty_list_of_files = () =>
        {
            files.Count.ShouldEqual(0);
        };
    }
}