using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Caching;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.X2Engine2.Providers;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.WorkflowDataProviderSpecs
{
    public class when_getting_a_workflowactivitydatamodel_by_id : WithFakes
    {
        static AutoMocker<WorkflowDataProvider> automocker = new NSubstituteAutoMocker<WorkflowDataProvider>();
        private static IReadOnlySqlRepository readOnlySqlRepository;
        private static WorkFlowActivityDataModel workflowActivityDataModel;
        private static WorkFlowActivityDataModel returnModel;

        Establish context = () =>
        {
            readOnlySqlRepository = MockRepositoryProvider.GetReadOnlyRepository();
            workflowActivityDataModel = new WorkFlowActivityDataModel(1, 1, "wf activity", 2, 2, 1, 9);
            readOnlySqlRepository.WhenToldTo(x => x.SelectOne<WorkFlowActivityDataModel>(Param.IsAny<string>(), Param.IsAny<object>())).Return(workflowActivityDataModel);
        };

        Because of = () =>
        {
            returnModel = automocker.ClassUnderTest.GetWorkflowActivityDataModelById(1);
        };

        It should_add_them_to_the_cache = () =>
        {
            automocker.Get<ICache>().WasToldTo(x => x.AddItem<WorkFlowActivityDataModel>(Param.IsAny<string>(), returnModel));
        };

        It should_return_the_correct_instance = () =>
        {
            returnModel.ID.ShouldEqual(1);
        };
    }
}