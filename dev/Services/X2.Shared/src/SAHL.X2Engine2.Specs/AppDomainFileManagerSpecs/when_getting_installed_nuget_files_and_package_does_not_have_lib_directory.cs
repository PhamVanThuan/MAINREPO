using System.Collections.Generic;
using System.IO.Abstractions;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.X2Engine2.Node.AppDomain;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.AppDomainFileManagerSpecs
{
    internal class when_getting_installed_nuget_files_and_package_does_not_have_lib_directory : WithFakes
    {
        private static AutoMocker<AppDomainFileManager> autoMocker;
        private static List<string> files;
        private static string nugetPackagePath;
        private static string[] directories;
        private static string[] nugetSubFolders;
        private static string[] packageFiles;
        private static string dllFile1;
        private static string dllFile2;

        private Establish context = () =>
        {
            dllFile1 = @"C:\PackageDirectory\";
            nugetSubFolders = new string[] { };
            directories = new string[] { @"C:\PackageDirectory" };
            autoMocker = new NSubstituteAutoMocker<AppDomainFileManager>();
            autoMocker.Get<IFileSystem>().Directory.WhenToldTo(x => x.GetDirectories(Param.IsAny<string>(), "*", System.IO.SearchOption.TopDirectoryOnly))
                .Return(directories);
            autoMocker.Get<IFileSystem>().Directory.WhenToldTo(x => x.Exists(directories[0] + "\\lib")).Return(false);
            autoMocker.Get<IFileSystem>().Directory.WhenToldTo(x => x.GetDirectories(directories[0] + "\\lib")).Return(nugetSubFolders);
        };

        private Because of = () =>
        {
            files = autoMocker.ClassUnderTest.GetInstalledNuGetFiles(nugetPackagePath);
        };

        private It should_not_continue_if_there_is_no_lib_directory = () =>
        {
            autoMocker.Get<IFileSystem>().Directory.WasNotToldTo(x => x.GetDirectories(Param.IsAny<string>()));
        };
    }
}