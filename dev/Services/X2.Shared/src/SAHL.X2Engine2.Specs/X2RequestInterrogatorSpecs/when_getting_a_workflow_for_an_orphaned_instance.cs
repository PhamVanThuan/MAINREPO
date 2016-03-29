using System;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Providers;
using SAHL.Core.Services;

namespace SAHL.X2Engine2.Specs.X2RequestInterrogatorSpecs
{
    public class when_getting_a_workflow_for_an_orphaned_instance : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<X2RequestInterrogator> autoMocker;

        private static X2Workflow requestWorkflow;
        private static X2RequestForExistingInstance existingInstanceRequest;
        private static InstanceDataModel nullInstanceDataModel;
        private static IServiceRequestMetadata serviceRequestMetadata;

        private Establish context = () =>
        {
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2RequestInterrogator>();
            nullInstanceDataModel = null;
            serviceRequestMetadata = new ServiceRequestMetadata();
            serviceRequestMetadata[ServiceRequestMetadata.HEADER_USERNAME] = "UserName";
            existingInstanceRequest = new X2RequestForExistingInstance(Guid.NewGuid(), Param.IsAny<long>(), X2RequestType.UserComplete, serviceRequestMetadata, "Activity", false, null);

            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetInstanceDataModel(Param.IsAny<long>())).Return(nullInstanceDataModel);
        };

        Because of = () =>
        {
            requestWorkflow = autoMocker.ClassUnderTest.GetRequestWorkflow(existingInstanceRequest);
        };

        It should_not_find_the_instance_in_the_database = () =>
        {
            autoMocker.Get<IWorkflowDataProvider>().WasToldTo(x => x.GetInstanceDataModel(existingInstanceRequest.InstanceId));
        };

        It should_return_a_workflow_with_a_blank_workflow_name_for_the_given_request = () =>
        {
            requestWorkflow.WorkflowName.ShouldEqual("");
        };

        It should_return_a_workflow_with_a_blank_process_name_for_the_given_request = () =>
        {
            requestWorkflow.ProcessName.ShouldEqual("");
        };
    }
}