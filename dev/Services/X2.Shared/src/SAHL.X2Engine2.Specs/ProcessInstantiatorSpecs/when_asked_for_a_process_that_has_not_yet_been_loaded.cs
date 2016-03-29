using System;
using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.X2;
using SAHL.Core.X2.AppDomain;
using SAHL.X2Engine2.Node.AppDomain;
using SAHL.X2Engine2.Providers;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.ProcessInstantiatorSpecs
{
    public class when_asked_for_a_process_that_has_not_yet_been_loaded : WithFakes
    {
        static AutoMocker<ProcessInstantiator> autoMocker = new NSubstituteAutoMocker<ProcessInstantiator>();
        static int processId = 9;
        static IX2Process process;
        static IAppDomainProcessProxy proxy;
        static string applicationName = "applicationName";
        static string packageName = "packageName";
        static string packageVersion = "packageVersion";
        static AppDomainSetup appDomainSetup;
        static ProcessDataModel processDataModel;
        static ProcessAssemblyNugetInfoDataModel nugetInfo;
        static List<string> NugetFiles = new List<string>();

        Establish context = () =>
        {
            NugetFiles.Add("File1.dll");
            process = An<IX2Process>();
            proxy = An<IAppDomainProcessProxy>();
            appDomainSetup = new AppDomainSetup();
            appDomainSetup.ApplicationName = applicationName;
            nugetInfo = new ProcessAssemblyNugetInfoDataModel(processId, 1, packageName, packageVersion);
            processDataModel = new ProcessDataModel(processId, null, applicationName, "", new byte[] { }, DateTime.Now, "", ",", string.Empty, true);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetProcessById(processId)).Return(processDataModel);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetProcessAssemblyNuGetInfoByProcessId(processId)).Return(new List<ProcessAssemblyNugetInfoDataModel>(new ProcessAssemblyNugetInfoDataModel[] { nugetInfo }));
            autoMocker.Get<IAppDomainProcessProxyFactory>().WhenToldTo(x => x.Create(null)).Return(proxy);
            autoMocker.Get<IAppDomainFileManager>().WhenToldTo(x => x.GetInstalledNuGetFiles(Param.IsAny<string>())).Return(NugetFiles);
        };

        Because of = () =>
        {
            process = autoMocker.ClassUnderTest.GetProcess(processId);
        };

        It should_load_the_process = () =>
            {
                autoMocker.Get<IWorkflowDataProvider>().WasToldTo(x => x.GetProcessById(processId));
            };

        It should_load_process_assemblies = () =>
            {
                autoMocker.Get<IWorkflowDataProvider>().WasToldTo(x => x.GetProcessAssemblyNuGetInfoByProcessId(processId));
            };

        It should_install_the_nuget_packages = () =>
            {
                autoMocker.Get<INugetRetriever>().WasToldTo(x => x.InstallPackage(packageName, Param.IsAny<string>(), Arg.Any<string>()));
            };

        It should_create_the_appDomain = () =>
            {
                autoMocker.Get<IAppDomainFactory>().WasToldTo(x => x.Create(Arg.Is<ProcessDataModel>(y => y.ID == processId), Arg.Any<string>(), Arg.Any<string>()));
            };

        It should_create_the_proxy_class = () =>
            {
                autoMocker.Get<IAppDomainProcessProxyFactory>().WasToldTo(x => x.Create(Arg.Any<System.AppDomain>()));
            };

        It should_write_the_config_file_into_the_appdomain_basedirectory = () =>
            {
                autoMocker.Get<IAppDomainFileManager>().WasToldTo(x => x.WriteConfigFile(Param.IsAny<string>(), Param.IsAny<string>()));
            };

        It should_copy_all_the_files_from_nuget_to_the_appdomain_basedirectory = () =>
            {
                autoMocker.Get<IAppDomainFileManager>().WasToldTo(x => x.CopyFile(Param.IsAny<string>(), Param.IsAny<string>()));
            };

        It should_load_all_the_assemblies_into_the_new_domain = () =>
            {
                proxy.WasToldTo(x => x.LoadAssembly(Arg.Any<string>()));
            };

        It should_call_getprocess_on_the_new_proxy = () =>
            {
                proxy.WasToldTo(x => x.GetProcess(applicationName));
            };

        It should_add_the_process_to_the_cache = () =>
            {
                autoMocker.Get<IAppDomainProcessProxyCache>().WasToldTo(x => x.AddProxy(processId, proxy));
            };

        It should_instantiate_the_process = () =>
            {
                process.ShouldNotBeNull();
            };
    }
}