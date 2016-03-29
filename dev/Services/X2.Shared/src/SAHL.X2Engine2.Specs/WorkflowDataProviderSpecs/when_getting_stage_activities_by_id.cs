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
    public class when_getting_stage_activities_by_id : WithFakes
    {
        static AutoMocker<WorkflowDataProvider> automocker = new NSubstituteAutoMocker<WorkflowDataProvider>();
        private static IReadOnlySqlRepository readOnlySqlRepository;
        private static List<StageActivityDataModel> Activities;
        private static int activityId = 1;
        private static StageActivityDataModel returnModel;

        private Establish context = () =>
        {
            Activities = new List<StageActivityDataModel>();
            Activities.Add(new StageActivityDataModel(1, 1, 54, 568));
            readOnlySqlRepository = MockRepositoryProvider.GetReadOnlyRepository();

            readOnlySqlRepository.WhenToldTo(x => x.Select<StageActivityDataModel>(Param.IsAny<StageActivityByActivitySqlStatement>())).Return(Activities);
        };

        private Because of = () =>
        {
            returnModel = automocker.ClassUnderTest.GetStageActivities(activityId).First();
        };

        It should_add_it_to_the_cache = () =>
            {
                automocker.Get<ICache>().WasToldTo(x => x.AddItem(Param.IsAny<string>(), Activities));
            };

        private It should_return_system_activities_coming_from_that_state = () =>
        {
            returnModel.ID.ShouldEqual(1);
        };
    }
}