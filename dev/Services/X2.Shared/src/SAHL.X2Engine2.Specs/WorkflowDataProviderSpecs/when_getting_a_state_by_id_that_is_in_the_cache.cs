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
    public class when_getting_a_state_by_id_that_is_in_the_cache : WithFakes
    {
        static AutoMocker<WorkflowDataProvider> automocker = new NSubstituteAutoMocker<WorkflowDataProvider>();
        private static StateDataModel stateDataModel;
        private static StateDataModel returnedModel;

        Establish context = () =>
        {
            stateDataModel = new StateDataModel(10, 1, "state", 1, false, null, null, null, Guid.NewGuid());
            automocker.Get<ICacheKeyGenerator>().WhenToldTo(x => x.GetKey<IKeyedCacheKey, StateDataModel>(Param.IsAny<IKeyedCacheKey>())).Return("Key");
            automocker.Get<ICache>().WhenToldTo(x => x.Contains("Key")).Return(true);
            automocker.Get<ICache>().WhenToldTo(x => x.GetItem<StateDataModel>("Key")).Return(stateDataModel);
        };

        Because of = () =>
        {
            returnedModel = automocker.ClassUnderTest.GetStateById(10);
        };

        It should_return_the_correct_instance = () =>
        {
            returnedModel.ShouldBeTheSameAs(stateDataModel);
        };

        It should_use_the_cache_to_return_the_item = () =>
            {
                automocker.Get<ICache>().WasToldTo(x => x.GetItem<StateDataModel>("Key"));
            };
    }
}