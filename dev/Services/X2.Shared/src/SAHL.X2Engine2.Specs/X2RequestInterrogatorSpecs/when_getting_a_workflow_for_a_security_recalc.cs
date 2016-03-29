using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Providers;
using System;
using SAHL.Core.Services;

namespace SAHL.X2Engine2.Specs.X2RequestInterrogatorSpecs
{
    public class when_getting_a_workflow_for_a_security_recalc : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<X2RequestInterrogator> autoMocker;

        private static IX2RequestForSecurityRecalc x2RequestForSecurityRecalc;
        private static X2Workflow workflow;
        private static WorkFlowDataModel workflowDataModel;
        private static ProcessDataModel processDataModel;
        private static InstanceDataModel instance;
        private static IServiceRequestMetadata metadata;

        private Establish context = () =>
        {
            metadata = An<IServiceRequestMetadata>();
            instance = new InstanceDataModel(1, 1, null, "name", "subject", "workflowProvider", 1, "CreatorADUserName", DateTime.Now, null, null, null, null, null, null, null, null);
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<X2RequestInterrogator>();
            workflow = new X2Workflow("process", "workflowname");
            workflowDataModel = new WorkFlowDataModel(1, 2, null, "workflow", DateTime.Now, "x2.x2data.workflow", "key", 1, "subject", null);
            processDataModel = new ProcessDataModel(workflowDataModel.ProcessID, null, "process", "v1.0", null, DateTime.Now, "v1.0", String.Empty, "3", false);
            x2RequestForSecurityRecalc = new X2RequestForSecurityRecalc(Guid.NewGuid(), -1, metadata);

            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetInstanceDataModel(x2RequestForSecurityRecalc.InstanceId)).Return(instance);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetWorkflowById(instance.WorkFlowID)).Return(workflowDataModel);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetProcessById(workflowDataModel.ProcessID)).Return(processDataModel);
        };

        private Because of = () =>
        {
            workflow = autoMocker.ClassUnderTest.GetRequestWorkflow(x2RequestForSecurityRecalc);
        };

        private It should_return_the_workflow_for_the_request = () =>
        {
            workflow.ProcessName.ShouldEqual(processDataModel.Name);
            workflow.WorkflowName.ShouldEqual(workflowDataModel.Name);
        };
    }
}