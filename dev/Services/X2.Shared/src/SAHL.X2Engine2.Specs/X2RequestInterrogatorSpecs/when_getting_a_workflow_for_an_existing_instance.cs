using System;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Providers;
using SAHL.Core.Services;

namespace SAHL.X2Engine2.Specs.X2RequestInterrogatorSpecs
{
    public class when_getting_a_workflow_for_an_existing_instance : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<X2RequestInterrogator> autoMocker;

        private static X2Workflow requestWorkflow;
        private static X2RequestForExistingInstance existingInstanceRequest;
        private static InstanceDataModel instanceDataModel;
        private static WorkFlowDataModel workflowDataModel;
        private static ProcessDataModel processDataModel;
        private static IServiceRequestMetadata serviceRequestMetadata;

        private Establish context = () =>
        {
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2RequestInterrogator>();
            instanceDataModel = Helper.GetInstanceDataModel(1234567L);
            serviceRequestMetadata = new ServiceRequestMetadata();
            serviceRequestMetadata[ServiceRequestMetadata.HEADER_USERNAME] = "UserName";
            existingInstanceRequest = new X2RequestForExistingInstance(Guid.NewGuid(), instanceDataModel.ID, X2RequestType.UserComplete, serviceRequestMetadata, "Activity", false);
            workflowDataModel = Helper.GetWorkflowDataModel();
            processDataModel = new ProcessDataModel(1, null, "ProcessName", "Version", new byte[] { }, DateTime.Now, "1", "config", string.Empty, true);
            workflowDataModel.ProcessID = processDataModel.ID;
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetInstanceDataModel(Param.IsAny<long>())).Return(instanceDataModel);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetWorkflow(Param.IsAny<InstanceDataModel>())).Return(workflowDataModel);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetProcessById(Param.IsAny<int>())).Return(processDataModel);
        };

        Because of = () =>
        {
            requestWorkflow = autoMocker.ClassUnderTest.GetRequestWorkflow(existingInstanceRequest);
        };

        It should_get_the_instance_from_the_database = () =>
        {
            autoMocker.Get<IWorkflowDataProvider>().WasToldTo(x => x.GetInstanceDataModel(existingInstanceRequest.InstanceId));
        };

        It should_get_the_workflow_from_the_database = () =>
        {
            autoMocker.Get<IWorkflowDataProvider>().WasToldTo(x => x.GetWorkflow(instanceDataModel));
        };

        It should_get_the_process_from_the_database = () =>
        {
            autoMocker.Get<IWorkflowDataProvider>().WasToldTo(x => x.GetProcessById(workflowDataModel.ProcessID));
        };

        It should_return_the_workflow_for_the_given_request = () =>
        {
            requestWorkflow.WorkflowName.ShouldEqual(workflowDataModel.Name);
        };

        It should_return_the_workflow_with_the_process_for_the_given_request = () =>
        {
            requestWorkflow.ProcessName.ShouldEqual(processDataModel.Name);
        };
    }
}