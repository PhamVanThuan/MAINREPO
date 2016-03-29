using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Caching;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.X2;
using SAHL.X2Engine2.Providers;

using SAHL.X2Engine2.ViewModels.SqlStatement;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;
using SAHL.Core.Testing.Providers;
using SAHL.Core.Testing.Fakes;

namespace SAHL.X2Engine2.Specs.WorkflowDataProviderSpecs
{
    internal class when_getting_configured_process_assemblies : WithFakes
    {
        static AutoMocker<WorkflowDataProvider> automocker = new NSubstituteAutoMocker<WorkflowDataProvider>();
        private static IReadOnlySqlRepository readOnlySqlRepository;
        private static ProcessAssemblyDataModel processAssembly;
        private static IEnumerable<ProcessAssemblyDataModel> returnedModels;
        static int processId = 1;

        Establish context = () =>
        {
            readOnlySqlRepository = MockRepositoryProvider.GetReadOnlyRepository();
            processAssembly = new ProcessAssemblyDataModel(1, processId, null, "dllName", new byte[] { });
            readOnlySqlRepository.WhenToldTo(x => x.Select<ProcessAssemblyDataModel>(Arg.Is<ProcessAssembliesByProcessIdSqlStatement>(y => y.ProcessID == processId))).Return(new List<ProcessAssemblyDataModel>(new ProcessAssemblyDataModel[] { processAssembly }));
        };

        Because of = () =>
        {
            returnedModels = automocker.ClassUnderTest.GetProcessAssemblies(processId);
        };

        It should_add_them_to_the_cache = () =>
        {
            automocker.Get<ICache>().WasToldTo(x => x.AddItem<IEnumerable<ProcessAssemblyDataModel>>(Param.IsAny<string>(), returnedModels));
        };

        It should_return_the_correct_instance = () =>
        {
            returnedModels.ShouldContain<ProcessAssemblyDataModel>(x => x.ID == 1 && x.ProcessID == processId);
        };
    }
}