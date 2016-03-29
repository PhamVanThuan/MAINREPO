using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.X2;
using SAHL.X2Engine2.Providers;

using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;
using SAHL.Core.Testing.Providers;
using SAHL.Core.Testing.Fakes;

namespace SAHL.X2Engine2.Specs.WorkflowDataProviderSpecs
{
    public class when_getting_the_workflow_name_for_an_instance : WithFakes
    {
        static AutoMocker<WorkflowDataProvider> automocker = new NSubstituteAutoMocker<WorkflowDataProvider>();
        private static IReadOnlySqlRepository readOnlySqlRepository;
        private static WorkFlowDataModel workflow;
        private static string workflowName;
        private static InstanceDataModel instance;
        private static Dictionary<string, object> parameters = new Dictionary<string, object>();

        Establish context = () =>
        {
            readOnlySqlRepository = MockRepositoryProvider.GetReadOnlyRepository();
            instance = Helper.GetInstanceDataModel(1234567L);
            workflow = Helper.GetWorkflowDataModel();
            parameters.Add("PrimaryKey", instance.WorkFlowID);
            readOnlySqlRepository.WhenToldTo(x => x.SelectOne<WorkFlowDataModel>(Param.IsAny<string>(), Param.IsAny<object>())).Return(workflow);
        };

        Because of = () =>
        {
            workflowName = automocker.ClassUnderTest.GetWorkflowName(instance);
        };

        It should_return_the_workflow_name = () =>
        {
            workflowName.ShouldEqual(workflow.Name);
        };

        It should_use_the_workflow_id_from_the_instance_provided = () =>
        {
            readOnlySqlRepository.WasToldTo(x => x.SelectOne<WorkFlowDataModel>(Param.IsAny<string>(), Arg.Is<object>(anonymousObject => anonymousObject.CheckValue(parameters))));
        };
    }
}