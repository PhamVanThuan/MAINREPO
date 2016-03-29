using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using WorkflowMaps.ReadvancePayments.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities.Payment_Prepared.OnComplete
{
    [Subject("Activity => Payment_Prepared => OnComplete")]
    internal class when_payment_prepared_return_true : WorkflowSpecReadvancePayments
    {
        private static bool result;
        private static string message;
        private static IWorkflowAssignment workflowAssignment;
        private static List<string> dys;

        private Establish context = () =>
        {
            result = false;
            message = string.Empty;
            ((InstanceDataStub)instanceData).ID = 1;
            workflowData.ApplicationKey = 1;
            dys = new List<string>() {"FL Processor D",
                "FL Supervisor D",
                "FL Manager D",};
            workflowAssignment = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(workflowAssignment);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Payment_Prepared(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_DeActive_Users_For_Instance_And_Process = () =>
        {
            workflowAssignment.DeActiveUsersForInstanceAndProcess((IDomainMessageCollection)messages, instanceData.ID, workflowData.ApplicationKey, dys, SAHL.Common.Globals.Process.Origination);
        };

        private It should_Reactive_User_Or_Round_Robin_For_OS_Keys_By_Process = () =>
        {
            workflowAssignment.ReactiveUserOrRoundRobinForOSKeysByProcess((IDomainMessageCollection)messages, "FL Supervisor D", workflowData.ApplicationKey, null, instanceData.ID, "Disburse Funds", SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.FLSupervisorDisburseFunds);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}