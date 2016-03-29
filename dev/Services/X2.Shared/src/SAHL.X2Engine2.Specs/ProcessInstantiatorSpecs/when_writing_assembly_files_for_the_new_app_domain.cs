using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.X2;
using SAHL.Core.X2.AppDomain;
using SAHL.X2Engine2.Node.AppDomain;
using SAHL.X2Engine2.Providers;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.ProcessInstantiatorSpecs
{
    public class when_writing_assembly_files_for_the_new_app_domain : WithFakes
    {
        private static AutoMocker<ProcessInstantiator> autoMocker = new NSubstituteAutoMocker<ProcessInstantiator>();
        private static int processId = 9;
        private static IX2Process process;
        private static IAppDomainProcessProxy proxy;
        private static string applicationName = "applicationName";
        private static string packageName = "packageName";
        private static string packageVersion = "packageVersion";
        private static AppDomainSetup appDomainSetup;
        private static ProcessDataModel processDataModel;
        private static ProcessAssemblyNugetInfoDataModel nugetInfo;
        private static List<string> NugetFiles = new List<string>();
        private static IEnumerable<ProcessAssemblyDataModel> assemblies;
        private static byte[] dllData1;
        private static byte[] dllData2;
        private static string fileName1;
        private static string fileName2;
        private static string destinationPath1;
        private static string destinationPath2;
        private static string directoryFullPath;

        private Establish context = () =>
        {
            dllData1 = new byte[] { };
            dllData2 = new byte[] { };
            fileName1 = "File1.dll";
            fileName2 = "File2.dll";
            NugetFiles.Add("File1.dll");
            directoryFullPath = @"c:\directoryFullPath\";
            destinationPath1 = string.Format(@"c:\destinationPath\{0}\", fileName1);
            destinationPath2 = string.Format(@"c:\destinationPath\{0}\", fileName2);
            process = An<IX2Process>();
            proxy = An<IAppDomainProcessProxy>();
            appDomainSetup = new AppDomainSetup();
            appDomainSetup.ApplicationName = applicationName;
            nugetInfo = new ProcessAssemblyNugetInfoDataModel(processId, 1, packageName, packageVersion);
            assemblies = new[] {
                new ProcessAssemblyDataModel(1, null, fileName1, dllData1),
                new ProcessAssemblyDataModel(1, null, fileName2, dllData2),
            };
            processDataModel = new ProcessDataModel(processId, null, applicationName, string.Empty, new byte[] { }, DateTime.Now, string.Empty, ",", string.Empty, true);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetProcessById(processId)).Return(processDataModel);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetProcessAssemblyNuGetInfoByProcessId(processId))
                .Return(new List<ProcessAssemblyNugetInfoDataModel>(new ProcessAssemblyNugetInfoDataModel[] { nugetInfo }));
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetProcessAssemblies(Param.IsAny<int>())).Return(assemblies);
            autoMocker.Get<IAppDomainProcessProxyFactory>().WhenToldTo(x => x.Create(null)).Return(proxy);
            autoMocker.Get<IAppDomainFileManager>().WhenToldTo(x => x.GetInstalledNuGetFiles(Param.IsAny<string>())).Return(NugetFiles);
            autoMocker.Get<IFileSystem>().Path.WhenToldTo(x => x.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "dotnetX2Processes\\1"))
                .Return(directoryFullPath);
            autoMocker.Get<IFileSystem>().Path.WhenToldTo(x => x.Combine(Param.IsAny<string>(), assemblies.First().DLLName))
                .Return(destinationPath1);
            autoMocker.Get<IFileSystem>().Path.WhenToldTo(x => x.Combine(Param.IsAny<string>(), assemblies.Last().DLLName))
                .Return(destinationPath2);
        };

        private Because of = () =>
        {
            process = autoMocker.ClassUnderTest.GetProcess(processId);
        };

        private It should_use_the_app_domain_file_manager_write_the_first_dll = () =>
        {
            autoMocker.Get<IAppDomainFileManager>().WasToldTo(x => x.WriteFile(assemblies.First().DLLData, destinationPath1));
        };

        private It should_use_the_app_domain_file_manager_write_the_last_dll = () =>
        {
            autoMocker.Get<IAppDomainFileManager>().WasToldTo(x => x.WriteFile(assemblies.Last().DLLData, destinationPath2));
        };

        private It should_load_each_assembly_using_the_proxy_loader = () =>
        {
            proxy.WasToldTo(x => x.LoadAssembly(Param.IsAny<string>())).Times(3);
        };
    }
}