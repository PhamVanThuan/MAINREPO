using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Caching;
using SAHL.Core.Data;
using SAHL.X2Engine2.Providers;

using SAHL.X2Engine2.ViewModels;
using SAHL.X2Engine2.ViewModels.SqlStatement;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;
using SAHL.Core.Testing.Providers;
using SAHL.Core.Testing.Fakes;

namespace SAHL.X2Engine2.Specs.WorkflowDataProviderSpecs
{
    public class when_getting_a_list_of_processes : WithFakes
    {
        static AutoMocker<WorkflowDataProvider> automocker = new NSubstituteAutoMocker<WorkflowDataProvider>();
        private static IReadOnlySqlRepository readOnlySqlRepository;
        private static ProcessViewModel process;
        private static IEnumerable<ProcessViewModel> returnedModels;
        static IEnumerable<string> configuredProcessNames;

        Establish context = () =>
        {
            configuredProcessNames = new List<string>(new string[] { "processName" });
            readOnlySqlRepository = MockRepositoryProvider.GetReadOnlyRepository();
            process = new ProcessViewModel(1, "processName");
            readOnlySqlRepository.WhenToldTo(x => x.Select<ProcessViewModel>(Param.IsAny<ConfiguredProcessesSqlStatement>())).Return(new List<ProcessViewModel>(new ProcessViewModel[] { process }));
        };

        Because of = () =>
        {
            returnedModels = automocker.ClassUnderTest.GetConfiguredProcesses(configuredProcessNames);
        };

        It should_add_them_to_the_cache = () =>
        {
            automocker.Get<ICache>().WasToldTo(x => x.AddItem<IEnumerable<ProcessViewModel>>(Param.IsAny<string>(), returnedModels));
        };

        It should_return_the_correct_instance = () =>
        {
            returnedModels.ShouldContain<ProcessViewModel>(x => x.ProcessName == "processName" && x.ProcessID == 1);
        };
    }
}