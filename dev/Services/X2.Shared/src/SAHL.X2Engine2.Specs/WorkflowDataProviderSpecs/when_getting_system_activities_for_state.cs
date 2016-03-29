using System;
using System.Collections.Generic;
using System.Linq;
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
    public class when_getting_system_activities_for_state : WithFakes
    {
        static AutoMocker<WorkflowDataProvider> automocker = new NSubstituteAutoMocker<WorkflowDataProvider>();
        private static IReadOnlySqlRepository readOnlySqlRepository;
        private static List<ActivityDataModel> Activities;
        private static int stateId = 1;
        private static ActivityDataModel returnModel;

        private Establish context = () =>
            {
                Activities = new List<ActivityDataModel>();
                Activities.Add(new ActivityDataModel(1, 1, "name", 1, 1, 2, false, 1, 1, "activityMessage", null, null, null, "chainedActivityName", 1, Guid.NewGuid()));
                automocker.Get<ICache>().WhenToldTo(x => x.GetItem<IEnumerable<ActivityDataModel>>("Key")).Return(Activities);
                readOnlySqlRepository = MockRepositoryProvider.GetReadOnlyRepository();

                readOnlySqlRepository.WhenToldTo(x => x.Select<ActivityDataModel>(Param.IsAny<SystemActivitiesForStateSqlStatement>())).Return(Activities);
            };

        private Because of = () =>
            {
                returnModel = automocker.ClassUnderTest.GetSystemActivitiesForState(stateId).First();
            };

        It should_add_them_to_the_cache = () =>
            {
                automocker.Get<ICache>().WasToldTo(x => x.AddItem<IEnumerable<ActivityDataModel>>(Param.IsAny<string>(), Activities));
            };

        private It should_return_system_activities_coming_from_that_state = () =>
            {
                returnModel.ID.ShouldEqual(1);
            };
    }
}