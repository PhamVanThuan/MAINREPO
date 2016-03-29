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
    public class when_getting_a_statedatamodel_by_statename_and_workflowname_that_is_in_the_cache : WithFakes
    {
        static AutoMocker<WorkflowDataProvider> automocker = new NSubstituteAutoMocker<WorkflowDataProvider>();
        private static StateDataModel stateDataModel;
        private static StateDataModel returnedModel;

        private Establish context = () =>
        {
            automocker.Get<ICacheKeyGenerator>().WhenToldTo(x => x.GetKey<INamedCacheKey, StateDataModel>(Param.IsAny<INamedCacheKey>())).Return("Key");
            stateDataModel = new StateDataModel(1, 1, "State4", 2, true, null, null, null, Guid.NewGuid());
            automocker.Get<ICache>().WhenToldTo(x => x.Contains("Key")).Return(true);
            automocker.Get<ICache>().WhenToldTo(x => x.GetItem<StateDataModel>("Key")).Return(stateDataModel);
        };

        private Because of = () =>
        {
            returnedModel = automocker.ClassUnderTest.GetStateDataModel("State4", "workflowName");
        };

        private It should_return_the_correct_instance = () =>
        {
            returnedModel.ID.ShouldEqual(1);
        };

        It should_return_the_item_from_the_cache = () =>
            {
                automocker.Get<ICache>().WasToldTo(x => x.GetItem<StateDataModel>("Key"));
            };
    }
}