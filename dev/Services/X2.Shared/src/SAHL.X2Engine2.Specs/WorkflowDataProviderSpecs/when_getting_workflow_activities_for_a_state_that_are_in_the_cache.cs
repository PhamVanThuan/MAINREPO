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
    public class when_getting_workflow_activities_for_a_state_that_are_in_the_cache : WithFakes
    {
        static AutoMocker<WorkflowDataProvider> automocker = new NSubstituteAutoMocker<WorkflowDataProvider>();
        private static List<WorkFlowActivityDataModel> activities;
        private static int stateId = 1;
        private static WorkFlowActivityDataModel returnModel;

        Establish context = () =>
        {
            activities = new List<WorkFlowActivityDataModel>();
            activities.Add(new WorkFlowActivityDataModel(1, 1, "wfActivitiy", 2, 10, 5, null));

            automocker.Get<ICacheKeyGenerator>().WhenToldTo(x => x.GetKey<IKeyedCacheKey, IEnumerable<WorkFlowActivityDataModel>>(Param.IsAny<IKeyedCacheKey>())).Return("Key");
            automocker.Get<ICache>().WhenToldTo(x => x.Contains("Key")).Return(true);
            automocker.Get<ICache>().WhenToldTo(x => x.GetItem<IEnumerable<WorkFlowActivityDataModel>>("Key")).Return(activities);
        };

        Because of = () =>
        {
            returnModel = automocker.ClassUnderTest.GetWorkflowActivitiesForState(stateId).First();
        };

        It should_get_them_from_the_cache = () =>
        {
            automocker.Get<ICache>().WasToldTo(x => x.GetItem<IEnumerable<WorkFlowActivityDataModel>>("Key"));
        };

        It should_return_system_activities_coming_from_that_state = () =>
        {
            returnModel.ID.ShouldEqual(1);
        };
    }
}