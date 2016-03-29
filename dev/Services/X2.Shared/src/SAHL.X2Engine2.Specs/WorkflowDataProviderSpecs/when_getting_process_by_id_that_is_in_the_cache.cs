using System;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Caching;
using SAHL.Core.Data.Models.X2;
using SAHL.X2Engine2.Providers;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.WorkflowDataProviderSpecs
{
    public class when_getting_process_by_id_that_is_in_the_cache : WithFakes
    {
        static AutoMocker<WorkflowDataProvider> automocker = new NSubstituteAutoMocker<WorkflowDataProvider>();
        private static ProcessDataModel processDataModel;
        private static ProcessDataModel returnModel;

        private Establish context = () =>
        {
            processDataModel = new ProcessDataModel(1, 1, "processName", "", new byte[] { }, DateTime.Now, "", ",", "", true);

            automocker.Get<ICacheKeyGenerator>().WhenToldTo(x => x.GetKey<IKeyedCacheKey, ProcessDataModel>(Param.IsAny<IKeyedCacheKey>())).Return("Key");
            automocker.Get<ICache>().WhenToldTo(x => x.Contains("Key")).Return(true);
            automocker.Get<ICache>().WhenToldTo(x => x.GetItem<ProcessDataModel>("Key")).Return(processDataModel);
        };

        private Because of = () =>
        {
            returnModel = automocker.ClassUnderTest.GetProcessById(1);
        };

        It should_get_it_from_the_cache = () =>
        {
            automocker.Get<ICache>().WasToldTo(x => x.GetItem<ProcessDataModel>("Key"));
        };

        private It should_return_the_correct_instance = () =>
        {
            returnModel.ID.ShouldEqual(1);
        };
    }
}