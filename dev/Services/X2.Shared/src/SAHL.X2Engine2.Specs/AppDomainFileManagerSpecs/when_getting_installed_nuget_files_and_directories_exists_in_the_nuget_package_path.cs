using System.Collections.Generic;
using System.IO.Abstractions;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.X2Engine2.Node.AppDomain;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.AppDomainFileManagerSpecs
{
    public class when_getting_installed_nuget_files_and_directories_exists_in_the_nuget_package_path : WithFakes
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
            dllFile1 = @"C:\PackageDirectory\lib\net40\file1.dll";
            dllFile2 = @"C:\PackageDirectory\lib\net40\file1.cs";
            nugetSubFolders = new string[] { @"C:\PackageDirectory\lib\net40", @"C:\PackageDirectory\lib\net35" };
            directories = new string[] { @"C:\PackageDirectory" };
            packageFiles = new string[] { dllFile1, dllFile2 };
            autoMocker = new NSubstituteAutoMocker<AppDomainFileManager>();
            autoMocker.Get<IFileSystem>().Directory.WhenToldTo(x => x.GetDirectories(Param.IsAny<string>(), "*", System.IO.SearchOption.TopDirectoryOnly))
                .Return(directories);
            autoMocker.Get<IFileSystem>().Directory.WhenToldTo(x => x.GetDirectories(directories[0] + "\\lib")).Return(nugetSubFolders);
            autoMocker.Get<IFileSystem>().Directory.WhenToldTo(x => x.GetFiles(@"C:\PackageDirectory\lib\net40", "*", System.IO.SearchOption.AllDirectories))
                .Return(packageFiles);
            autoMocker.Get<IFileSystem>().Path.WhenToldTo(x => x.GetExtension(dllFile1)).Return(".dll");
            autoMocker.Get<IFileSystem>().Path.WhenToldTo(x => x.GetExtension(dllFile2)).Return(".cs");
            autoMocker.Get<IFileSystem>().Directory.WhenToldTo(x => x.Exists(directories[0] + "\\lib")).Return(true);
        };

        private Because of = () =>
        {
            files = autoMocker.ClassUnderTest.GetInstalledNuGetFiles(nugetPackagePath);
        };

        private It should_continue_looking_for_dlls = () =>
        {
            autoMocker.Get<IFileSystem>().Directory.WasToldTo(x => x.GetDirectories(Param.IsAny<string>()));
        };

        private It should_copy_only_the_latest_version_of_the_framework = () =>
        {
            autoMocker.Get<IFileSystem>().Directory.WasToldTo(x => x.GetFiles(@"C:\PackageDirectory\lib\net40", "*", System.IO.SearchOption.AllDirectories));
        };

        private It should_not_copy_older_framework_versions = () =>
        {
            autoMocker.Get<IFileSystem>().Directory.WasNotToldTo(x => x.GetFiles(@"C:\PackageDirectory\lib\net35", "*", System.IO.SearchOption.AllDirectories));
        };

        private It should_only_add_the_dll_files_from_the_package = () =>
        {
            files.ShouldNotContain(dllFile2);
        };

        private It should_return_a_list_of_the_dll_files = () =>
        {
            files.ShouldContain(dllFile1);
            files.Count.ShouldEqual(1);
        };
    }
}