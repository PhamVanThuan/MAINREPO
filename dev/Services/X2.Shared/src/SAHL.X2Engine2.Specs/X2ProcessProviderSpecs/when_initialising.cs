using System.Collections.Generic;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.X2.AppDomain;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Node.Providers;
using SAHL.X2Engine2.Providers;
using SAHL.X2Engine2.ViewModels;

namespace SAHL.X2Engine2.Specs.X2ProcessProviderSpecs
{
    public class when_initialising : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<X2ProcessProvider> autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2ProcessProvider>();
        private static IEnumerable<X2Process> configuredProcesses;
        private static IEnumerable<X2Workflow> workflows;
        private static IEnumerable<ProcessViewModel> processModel;
        private static ProcessViewModel processViewModel;
        private static IEnumerable<string> configuredProcessNames;
        private static X2Process process;
        private static X2Workflow workflow;

        Establish context = () =>
            {
                workflow = new X2Workflow("Process", "Workflow");
                workflows = new[] { workflow };
                processViewModel = new ProcessViewModel(1, "Process");
                processModel = new[] { processViewModel };
                process = new X2Process("Process", workflows);
                configuredProcesses = new[] { process };
                configuredProcessNames = configuredProcesses.Select(x => x.ProcessName);
                autoMocker.Get<IX2NodeConfigurationProvider>().WhenToldTo(x => x.GetAvailableProcesses()).Return(configuredProcesses);
                autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetConfiguredProcesses(Param.IsAny<IEnumerable<string>>())).Return(processModel);
            };

        Because of = () =>
            {
                autoMocker.ClassUnderTest.Initialise();
            };

        It should_preload_all_processes_its_configured_to_support = () =>
            {
                autoMocker.Get<IProcessInstantiator>().WasToldTo(x => x.GetProcess(processViewModel.ProcessID));
            };

        It should_get_the_configured_processes_from_the_workflow_data_provider = () =>
            {
                autoMocker.Get<IWorkflowDataProvider>().WasToldTo(x => x.GetConfiguredProcesses(Arg.Is<IEnumerable<string>>(i => i.First().ToString().Equals(configuredProcessNames.First().ToString()))));
            };
    }
}