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
    public class when_getting_a_state_worklist_for_a_stateid_that_is_in_the_cache : WithFakes
    {
        static AutoMocker<WorkflowDataProvider> automocker = new NSubstituteAutoMocker<WorkflowDataProvider>();
        private static List<StateWorkListDataModel> StateWorkLists;
        private static int stateId = 1;
        private static StateWorkListDataModel returnModel;

        private Establish context = () =>
            {
                StateWorkLists = new List<StateWorkListDataModel>();
                returnModel = new StateWorkListDataModel(1, 10, 12);
                StateWorkLists.Add(returnModel);
                automocker.Get<ICacheKeyGenerator>().WhenToldTo(x => x.GetKey<IKeyedCacheKey, IEnumerable<StateWorkListDataModel>>(Param.IsAny<IKeyedCacheKey>())).Return("Key");
                automocker.Get<ICache>().WhenToldTo(x => x.Contains("Key")).Return(true);
                automocker.Get<ICache>().WhenToldTo(x => x.GetItem<IEnumerable<StateWorkListDataModel>>("Key")).Return(StateWorkLists);
            };

        private Because of = () =>
            {
                returnModel = automocker.ClassUnderTest.GetStateWorkList(stateId).First();
            };

        It get_it_from_the_cache = () =>
            {
                automocker.Get<ICache>().WasToldTo(x => x.GetItem<IEnumerable<StateWorkListDataModel>>("Key"));
            };

        private It should_return_system_activities_coming_from_that_state = () =>
            {
                returnModel.ID.ShouldEqual(1);
            };
    }
}