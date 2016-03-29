using System.Collections.Generic;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Caching;
using SAHL.Core.Data.Models.X2;
using SAHL.X2Engine2.Providers;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.WorkflowDataProviderSpecs
{
    internal class when_getting_configured_process_assemblies_that_are_in_the_cache : WithFakes
    {
        static AutoMocker<WorkflowDataProvider> automocker = new NSubstituteAutoMocker<WorkflowDataProvider>();
        private static ProcessAssemblyDataModel processAssembly;
        private static List<ProcessAssemblyDataModel> returnedModels = new List<ProcessAssemblyDataModel>();
        static int processId = 1;

        Establish context = () =>
        {
            processAssembly = new ProcessAssemblyDataModel(1, processId, null, "dllName", new byte[] { });
            returnedModels.Add(processAssembly);

            automocker.Get<ICacheKeyGenerator>().WhenToldTo(x => x.GetKey<IKeyedCacheKey, IEnumerable<ProcessAssemblyDataModel>>(Param.IsAny<IKeyedCacheKey>())).Return("Key");
            automocker.Get<ICache>().WhenToldTo(x => x.Contains("Key")).Return(true);
            automocker.Get<ICache>().WhenToldTo(x => x.GetItem<IEnumerable<ProcessAssemblyDataModel>>("Key")).Return(returnedModels);
        };

        Because of = () =>
        {
            returnedModels = automocker.ClassUnderTest.GetProcessAssemblies(processId).ToList();
        };

        It should_get_them_from_the_cache = () =>
        {
            automocker.Get<ICache>().WasToldTo(x => x.GetItem<IEnumerable<ProcessAssemblyDataModel>>("Key"));
        };

        It should_return_the_correct_instance = () =>
        {
            returnedModels.ShouldContain<ProcessAssemblyDataModel>(x => x.ID == 1 && x.ProcessID == processId);
        };
    }
}