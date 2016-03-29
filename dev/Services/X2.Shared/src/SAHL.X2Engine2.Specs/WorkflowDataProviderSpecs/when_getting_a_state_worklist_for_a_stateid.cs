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
    public class when_getting_a_state_worklist_for_a_stateid : WithFakes
    {
        static AutoMocker<WorkflowDataProvider> automocker = new NSubstituteAutoMocker<WorkflowDataProvider>();
        private static IReadOnlySqlRepository readOnlySqlRepository;
        private static List<StateWorkListDataModel> StateWorkLists;
        private static int stateId = 1;
        private static StateWorkListDataModel returnModel;

        private Establish context = () =>
            {
                readOnlySqlRepository = MockRepositoryProvider.GetReadOnlyRepository();
                StateWorkLists = new List<StateWorkListDataModel>();
                returnModel = new StateWorkListDataModel(1, 10, 12);
                StateWorkLists.Add(returnModel);
                readOnlySqlRepository.WhenToldTo(x => x.Select<StateWorkListDataModel>(Param.IsAny<StateWorkListForStateSqlStatement>())).Return(StateWorkLists);
            };

        private Because of = () =>
            {
                returnModel = automocker.ClassUnderTest.GetStateWorkList(stateId).First();
            };

        It should_add_the_item_to_the_cache = () =>
            {
                automocker.Get<ICache>().WasToldTo(x => x.AddItem<IEnumerable<StateWorkListDataModel>>(Param.IsAny<string>(), StateWorkLists));
            };

        private It should_return_system_activities_coming_from_that_state = () =>
            {
                returnModel.ID.ShouldEqual(1);
            };
    }
}