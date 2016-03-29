using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Caching;
using SAHL.Core.Data;
using SAHL.Core.Testing.Providers;
using SAHL.X2Engine2.Providers;
using SAHL.X2Engine2.ViewModels;
using SAHL.X2Engine2.ViewModels.SqlStatement;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;
using System.Collections.Generic;
using SAHL.Core.Testing.Fakes;

namespace SAHL.X2Engine2.Specs.WorkflowDataProviderSpecs
{
    public class when_getting_supported_workflows_for_processes : WithFakes
    {
        private static AutoMocker<WorkflowDataProvider> automocker = new NSubstituteAutoMocker<WorkflowDataProvider>();
        private static IReadOnlySqlRepository readOnlySqlRepository;
        private static IEnumerable<string> configuredProcessNames;
        private static ProcessWorkflowViewModel process;
        private static IEnumerable<ProcessWorkflowViewModel> returnedModels;

        Establish context = () =>
            {
                configuredProcessNames = new List<string>(new string[] { "processName" });
                readOnlySqlRepository = MockRepositoryProvider.GetReadOnlyRepository();
                process = new ProcessWorkflowViewModel("processName","workflowName");
                readOnlySqlRepository.WhenToldTo(x => x.Select<ProcessWorkflowViewModel>(Param.IsAny<SupportedWorkflowsSqlStatement>())).Return(new List<ProcessWorkflowViewModel>(new ProcessWorkflowViewModel[] { process }));
            };

        Because of = () =>
            {
                returnedModels = automocker.ClassUnderTest.GetSupportedWorkflows(configuredProcessNames);
            };

        It should_add_them_to_the_cache = () =>
            {
                automocker.Get<ICache>().WasToldTo(x => x.AddItem<IEnumerable<ProcessWorkflowViewModel>>(Param.IsAny<string>(), returnedModels));
            };

        It should_return_supported_workflows = () =>
            {
                returnedModels.ShouldContain<ProcessWorkflowViewModel>(x => x.ProcessName == "processName" && x.WorkflowName == "workflowName");
            };
    }
}
