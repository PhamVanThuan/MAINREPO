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
    public class when_getting_workflow_activities_for_a_state : WithFakes
    {
        static AutoMocker<WorkflowDataProvider> automocker = new NSubstituteAutoMocker<WorkflowDataProvider>();
        private static IReadOnlySqlRepository readOnlySqlRepository;
        private static List<WorkFlowActivityDataModel> Activities;
        private static int stateId = 1;
        private static WorkFlowActivityDataModel returnModel;

        Establish context = () =>
        {
            Activities = new List<WorkFlowActivityDataModel>();
            Activities.Add(new WorkFlowActivityDataModel(1, 1, "wfActivitiy", 2, 10, 5, null));
            readOnlySqlRepository = MockRepositoryProvider.GetReadOnlyRepository();

            readOnlySqlRepository.WhenToldTo(x => x.Select<WorkFlowActivityDataModel>(Param.IsAny<WorkflowActivitiesForStateSqlStatement>())).Return(Activities);
        };

        Because of = () =>
        {
            returnModel = automocker.ClassUnderTest.GetWorkflowActivitiesForState(stateId).First();
        };

        It should_add_them_to_the_cache = () =>
        {
            automocker.Get<ICache>().WasToldTo(x => x.AddItem<IEnumerable<WorkFlowActivityDataModel>>(Param.IsAny<string>(), Activities));
        };

        It should_return_system_activities_coming_from_that_state = () =>
        {
            returnModel.ID.ShouldEqual(1);
        };
    }
}