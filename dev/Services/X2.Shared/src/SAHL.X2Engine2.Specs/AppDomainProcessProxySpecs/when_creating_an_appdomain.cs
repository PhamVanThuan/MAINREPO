using System;
using System.IO.Abstractions;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models.X2;
using SAHL.X2Engine2.Node.AppDomain;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.AppDomainProcessProxySpecs
{
    public class when_creating_an_appdomain : WithFakes
    {
        private static AutoMocker<AppDomainFactory> autoMocker;
        private static ProcessDataModel processDataModel;
        private static System.AppDomain appDomain;
        private static string baseDirectory;
        private static string configFileName;
        private static string configFileFullPath;
        private static string configFileNewFullPath;
        private static string appName;
        private static string directoryFullPath;
        private static string directory;

        private Establish context = () =>
        {
            directoryFullPath = "";
            directory = "PrivateBinPath";
            baseDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
            autoMocker = new NSubstituteAutoMocker<AppDomainFactory>();
            processDataModel = new ProcessDataModel(1, null, "Process", "Version", new byte[] { }, DateTime.Now, "1.0.0.0", "config", string.Empty, true);
            configFileName = string.Format("{0}.config", processDataModel.Name);
            configFileFullPath = @"C:\Path\";
            configFileNewFullPath = @"C:\FullPath\";
            autoMocker.Get<IFileSystem>().WhenToldTo(x => x.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, configFileName)).Return(configFileFullPath);
            autoMocker.Get<IFileSystem>().WhenToldTo(x => x.Path.Combine(directoryFullPath, configFileName)).Return(configFileNewFullPath);
            appName = string.Format("X2ProxyLoader-{0}", processDataModel.ID);
        };

        private Because of = () =>
        {
            appDomain = autoMocker.ClassUnderTest.Create(processDataModel, directory, directoryFullPath);
        };

        private It should_create_an_app_domain = () =>
        {
            appDomain.ShouldNotBeNull();
        };

        private It should_create_the_app_domain_with_the_name_provided = () =>
        {
            appDomain.FriendlyName.ShouldEqual(appName);
        };

        private It should_set_the_base_diretory_to_the_current_domain_base_directory = () =>
        {
            appDomain.BaseDirectory.ShouldEqual(baseDirectory);
        };

        private It should_set_the_path_to_the_private_binaries = () =>
        {
            appDomain.RelativeSearchPath.ShouldEqual(directory);
        };

        private It should_set_the_path_to_the_configuration_file = () =>
        {
            appDomain.SetupInformation.ConfigurationFile.ShouldEndWith(configFileNewFullPath);
        };

        private It should_set_the_private_binaries_bin_path_probe = () =>
        {
            appDomain.SetupInformation.PrivateBinPathProbe.ShouldEqual("noprobe");
        };
    }
}