using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Providers;
using System;
using SAHL.Core.Services;

namespace SAHL.X2Engine2.Specs.X2RequestInterrogatorSpecs
{
    public class when_getting_a_workflow_for_an_external_activity_with_activating_instance : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<X2RequestInterrogator> autoMocker;

        private static IX2ExternalActivityRequest x2ExternalActivityRequest;
        private static X2Workflow workflow;
        private static WorkFlowDataModel workflowDataModel;
        private static ProcessDataModel processDataModel;
        private static IServiceRequestMetadata metadata;

        private Establish context = () =>
        {
            metadata = An<IServiceRequestMetadata>();
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2RequestInterrogator>();
            workflow = new X2Workflow("process", "workflowname");
            workflowDataModel = new WorkFlowDataModel(1, 2, null, "workflow", DateTime.Now, "x2.x2data.workflow", "key", 1, "subject", null);
            processDataModel = new ProcessDataModel(workflowDataModel.ProcessID, null, "process", "v1.0", null, DateTime.Now, "v1.0", String.Empty, "3", false);
            x2ExternalActivityRequest = new X2ExternalActivityRequest(Guid.NewGuid(), -1, 1, 1, 1, 1000, metadata);

            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetWorkflowById(x2ExternalActivityRequest.WorkflowId)).Return(workflowDataModel);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetProcessById(workflowDataModel.ProcessID)).Return(processDataModel);
        };

        private Because of = () =>
        {
            workflow = autoMocker.ClassUnderTest.GetRequestWorkflow(x2ExternalActivityRequest);
        };

        private It should_return_the_workflow_for_the_request = () =>
        {
            workflow.ProcessName.ShouldEqual(processDataModel.Name);
            workflow.WorkflowName.ShouldEqual(workflowDataModel.Name);
        };
    }
}