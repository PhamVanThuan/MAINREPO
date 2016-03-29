using System.Collections.Generic;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Caching;
using SAHL.X2Engine2.Providers;
using SAHL.X2Engine2.ViewModels;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.WorkflowDataProviderSpecs
{
    public class when_getting_supported_workflows_for_processes_that_are_in_the_cache : WithFakes
    {
        static AutoMocker<WorkflowDataProvider> automocker = new NSubstituteAutoMocker<WorkflowDataProvider>();
        private static ProcessWorkflowViewModel process;
        private static List<ProcessWorkflowViewModel> returnedModels = new List<ProcessWorkflowViewModel>();
        static IEnumerable<string> configuredProcessNames;

        Establish context = () =>
        {
            configuredProcessNames = new List<string>(new string[] { "processName"});
            process = new ProcessWorkflowViewModel("processName", "workflowName");
            returnedModels.Add(process);

            automocker.Get<ICacheKeyGenerator>().WhenToldTo(x => x.GetKey<INamedCacheKey, IEnumerable<ProcessWorkflowViewModel>>(Param.IsAny<INamedCacheKey>())).Return("Key");
            automocker.Get<ICache>().WhenToldTo(x => x.Contains("Key")).Return(true);
            automocker.Get<ICache>().WhenToldTo(x => x.GetItem<IEnumerable<ProcessWorkflowViewModel>>("Key")).Return(returnedModels);
        };

        Because of = () =>
        {
            returnedModels = automocker.ClassUnderTest.GetSupportedWorkflows(configuredProcessNames).ToList();
        };

        It should_get_them_from_the_cache = () =>
        {
            automocker.Get<ICache>().WasToldTo(x => x.GetItem<IEnumerable<ProcessWorkflowViewModel>>("Key"));
        };

        It should_return_the_correct_process = () =>
        {
            returnedModels.ShouldContain<ProcessWorkflowViewModel>(x => x.ProcessName == "processName" && x.WorkflowName == "workflowName");
        };
    }
}
