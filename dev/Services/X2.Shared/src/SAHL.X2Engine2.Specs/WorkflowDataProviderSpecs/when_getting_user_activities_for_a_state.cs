using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
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
    public class when_getting_user_activities_for_a_state : WithFakes
    {
        static AutoMocker<WorkflowDataProvider> automocker = new NSubstituteAutoMocker<WorkflowDataProvider>();
        private static IReadOnlySqlRepository readOnlySqlRepository;
        private static ActivityDataModel activityDataModel1;
        private static ActivityDataModel activityDataModel2;
        private static IEnumerable<ActivityDataModel> activities;
        private static IEnumerable<ActivityDataModel> results;
        private static int stateID;

        Establish context = () =>
        {
            stateID = 999;
            activityDataModel1 = Helper.GetActivityDataModel();
            activityDataModel2 = Helper.GetActivityDataModel();
            activityDataModel2.Name = "Second Activity";
            activityDataModel2.ID = 99;
            activities = new[] { activityDataModel1, activityDataModel2 };
            readOnlySqlRepository = MockRepositoryProvider.GetReadOnlyRepository();
            readOnlySqlRepository.WhenToldTo(x => x.Select<ActivityDataModel>(Param.IsAny<UserActivitiesForStateSqlStatement>())).Return(activities);
        };

        Because of = () =>
        {
            results = automocker.ClassUnderTest.GetUserActivitiesForState(stateID);
        };

        It should_return_a_list_of_activity_data_models_for_the_state_provided = () =>
        {
            results.ShouldBeTheSameAs(activities);
        };

        It should_add_it_to_the_cache = () =>
            {
                automocker.Get<ICache>().WasToldTo(x => x.AddItem<IEnumerable<ActivityDataModel>>(Param.IsAny<string>(), activities));
            };

        It should_use_the_state_id_provided = () =>
        {
        };
    }
}