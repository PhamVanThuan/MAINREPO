using System;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.X2.Messages;
using SAHL.Core.Services;

namespace SAHL.X2Engine2.Specs.X2RequestInterrogatorSpecs
{
    public class when_getting_a_workflow_for_a_new_instance : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<X2RequestInterrogator> autoMocker;

        private static X2CreateInstanceRequest createInstanceRequest;
        private static X2Workflow workflow;
        private static string processName;
        private static string workflowName;
        private static IServiceRequestMetadata serviceRequestMetadata;

        private Establish context = () =>
        {
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2RequestInterrogator>();

            processName = "processName";
            workflowName = "workflowName";
            serviceRequestMetadata = new ServiceRequestMetadata();
            serviceRequestMetadata[ServiceRequestMetadata.HEADER_USERNAME] = "UserName";

            createInstanceRequest = new X2CreateInstanceRequest(Guid.NewGuid(), "activityName", processName, workflowName, serviceRequestMetadata, false);
        };

        private Because of = () =>
        {
            workflow = autoMocker.ClassUnderTest.GetRequestWorkflow(createInstanceRequest);
        };

        private It should_return_the_workflow_for_the_given_request = () =>
        {
            workflow.WorkflowName.ShouldEqual(createInstanceRequest.WorkflowName);
        };

        private It should_return_the_workflow_with_the_process_for_the_given_request = () =>
        {
            workflow.ProcessName.ShouldEqual(createInstanceRequest.ProcessName);
        };
    }
}