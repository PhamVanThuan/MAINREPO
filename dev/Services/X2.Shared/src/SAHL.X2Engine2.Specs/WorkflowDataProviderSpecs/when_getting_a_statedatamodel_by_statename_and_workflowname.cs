using System;
using Machine.Fakes;
using Machine.Specifications;
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
    public class when_getting_a_statedatamodel_by_statename_and_workflowname : WithFakes
    {
        static AutoMocker<WorkflowDataProvider> automocker = new NSubstituteAutoMocker<WorkflowDataProvider>();
        private static IReadOnlySqlRepository readOnlySqlRepository;
        private static StateDataModel stateDataModel;
        private static StateDataModel returnedModel;

        private Establish context = () =>
        {
            readOnlySqlRepository = MockRepositoryProvider.GetReadOnlyRepository();
            stateDataModel = new StateDataModel(1, 1, "State4", 2, true, null, null, null, Guid.NewGuid());
            readOnlySqlRepository.WhenToldTo(x => x.SelectOne<StateDataModel>(Param.IsAny<StateByAutoForwardAndWorkflowNameSqlStatement>())).Return(stateDataModel);
        };

        private Because of = () =>
        {
            returnedModel = automocker.ClassUnderTest.GetStateDataModel("State4", "workflowName");
        };

        private It should_return_the_correct_instance = () =>
        {
            returnedModel.ID.ShouldEqual(1);
        };
    }
}