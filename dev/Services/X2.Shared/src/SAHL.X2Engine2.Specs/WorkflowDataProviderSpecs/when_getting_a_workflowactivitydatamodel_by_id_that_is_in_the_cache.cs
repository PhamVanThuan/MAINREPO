using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Caching;
using SAHL.Core.Data.Models.X2;
using SAHL.X2Engine2.Providers;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.WorkflowDataProviderSpecs
{
    public class when_getting_a_workflowactivitydatamodel_by_id_that_is_in_the_cache : WithFakes
    {
        static AutoMocker<WorkflowDataProvider> automocker = new NSubstituteAutoMocker<WorkflowDataProvider>();
        private static WorkFlowActivityDataModel workflowActivityDataModel;
        private static WorkFlowActivityDataModel returnModel;

        Establish context = () =>
        {
            workflowActivityDataModel = new WorkFlowActivityDataModel(1, 1, "wf activity", 2, 2, 1, 9);

            automocker.Get<ICacheKeyGenerator>().WhenToldTo(x => x.GetKey<IKeyedCacheKey, WorkFlowActivityDataModel>(Param.IsAny<IKeyedCacheKey>())).Return("Key");
            automocker.Get<ICache>().WhenToldTo(x => x.Contains("Key")).Return(true);
            automocker.Get<ICache>().WhenToldTo(x => x.GetItem<WorkFlowActivityDataModel>("Key")).Return(workflowActivityDataModel);
        };

        Because of = () =>
        {
            returnModel = automocker.ClassUnderTest.GetWorkflowActivityDataModelById(1);
        };

        It should_get_it_from_the_cache = () =>
        {
            automocker.Get<ICache>().WasToldTo(x => x.GetItem<WorkFlowActivityDataModel>("Key"));
        };

        It should_return_the_correct_instance = () =>
        {
            returnModel.ID.ShouldEqual(1);
        };
    }
}