using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Caching;
using SAHL.Core.Data.Models.X2;
using SAHL.X2Engine2.Providers;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.WorkflowDataProviderSpecs
{
    public class when_getting_a_workflowdatamodel_by_id_and_its_in_the_cache : WithFakes
    {
        static AutoMocker<WorkflowDataProvider> automocker = new NSubstituteAutoMocker<WorkflowDataProvider>();
        private static WorkFlowDataModel workflowDataModel;
        private static WorkFlowDataModel returnModel;
        private static int workflowID;

        Establish context = () =>
        {
            workflowDataModel = Helper.GetWorkflowDataModel();
            workflowID = workflowDataModel.ID;
            automocker.Get<ICacheKeyGenerator>().WhenToldTo(x => x.GetKey<IKeyedCacheKey, WorkFlowDataModel>(Param.IsAny<IKeyedCacheKey>())).Return("Key");
            automocker.Get<ICache>().WhenToldTo(x => x.Contains("Key")).Return(true);
            automocker.Get<ICache>().WhenToldTo(x => x.GetItem<WorkFlowDataModel>("Key")).Return(workflowDataModel);
        };

        Because of = () =>
        {
            returnModel = automocker.ClassUnderTest.GetWorkflowById(workflowID);
        };

        It should_return_the_correct_instance = () =>
        {
            returnModel.ShouldBeTheSameAs(workflowDataModel);
        };

        It should_get_it_from_the_cache = () =>
            {
                automocker.Get<ICache>().WasToldTo(x => x.GetItem<WorkFlowDataModel>("Key"));
            };
    }
}