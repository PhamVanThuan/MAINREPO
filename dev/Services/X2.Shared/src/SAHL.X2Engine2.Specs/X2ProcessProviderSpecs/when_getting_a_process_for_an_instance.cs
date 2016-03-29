using System;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.X2;
using SAHL.Core.X2.AppDomain;
using SAHL.X2Engine2.Providers;

namespace SAHL.X2Engine2.Specs.X2ProcessProviderSpecs
{
    public class when_getting_a_process_for_an_instance : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<X2ProcessProvider> autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2ProcessProvider>();
        private static long instanceID;
        private static InstanceDataModel instance;
        private static WorkFlowDataModel workflow;
        private static ProcessDataModel processDataModel;
        private static IX2Process process;

        Establish context = () =>
        {
            instanceID = 123465789L;
            instance = new InstanceDataModel(instanceID, 1, null, "InstanceName", "Subject", "WorkflowProvider", 1, @"SAHL\ClintonS", DateTime.Now, null, null, null, @"SAHL\ClintonS", null, null, null, null);
            processDataModel = new ProcessDataModel(1, null, "Process", "1", new byte[] { }, DateTime.Now, "1.0", "configFile", string.Empty, true);
            workflow = new WorkFlowDataModel(1, processDataModel.ID, null, "Name", DateTime.Now, "X2DataTable", "ApplicationKey", 1, "Test", 2);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetInstanceDataModel(instanceID)).Return(instance);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetWorkflow(Param.IsAny<InstanceDataModel>())).Return(workflow);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetProcessById(Param.IsAny<int>())).Return(processDataModel);
        };

        Because of = () =>
        {
            process = autoMocker.ClassUnderTest.GetProcessForInstance(instanceID);
        };

        It should_get_the_instance_data_model_from_the_workflow_data_provider = () =>
        {
            autoMocker.Get<IWorkflowDataProvider>().WasToldTo(x => x.GetInstanceDataModel(instanceID));
        };

        It should_get_the_workflow_for_instance_data_given_by_the_workflow_data_provider = () =>
        {
            autoMocker.Get<IWorkflowDataProvider>().WasToldTo(x => x.GetWorkflow(instance));
        };

        It should_get_process_for_the_given_workflow = () =>
        {
            autoMocker.Get<IWorkflowDataProvider>().WasToldTo(x => x.GetProcessById(workflow.ProcessID));
        };

        It should_instantiate_the_process = () =>
        {
            autoMocker.Get<IProcessInstantiator>().WasToldTo(x => x.GetProcess(processDataModel.ID));
        };

        It should_return_the_instantiated_process = () =>
        {
            process.ShouldNotBeNull();
        };
    }
}