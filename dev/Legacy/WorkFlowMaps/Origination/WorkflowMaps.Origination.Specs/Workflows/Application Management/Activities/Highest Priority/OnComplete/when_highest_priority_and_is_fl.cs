using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using WorkflowMaps.ApplicationManagement.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.Origination;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Highest_Priority.OnComplete
{
    [Subject("Activity => Highest_Priority => OnComplete")]
    internal class when_highest_priority_and_is_fl : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static string message;
        private static string expectedAssignedTo;
        private static IFL fl;
        private static IWorkflowAssignment assignment;
        private static ICommon common;
        private static List<string> dys;

        private Establish context = () =>
        {
            result = false;
            message = string.Empty;
            workflowData.IsFL = true;
            expectedAssignedTo = "AssignedToTest";
            dys = new List<string>(){"FL Processor D",
                "FL Supervisor D",
                "FL Manager D"};
            workflowData.ApplicationKey = 1;
            ((InstanceDataStub)instanceData).ID = 2;
            ((ParamsDataStub)paramsData).ADUserName = "ADUserNameTest";

            fl = An<IFL>();
            domainServiceLoader.RegisterMockForType<IFL>(fl);

            assignment = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(assignment);

            assignment.WhenToldTo(x => x.RoundRobinAndAssignOtherFLCases((IDomainMessageCollection)messages, workflowData.ApplicationKey, dys[0], 157, instanceData.ID, "QA", (int)SAHL.Common.Globals.RoundRobinPointers.FLProcessor))
                .Return(expectedAssignedTo);

            common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(common);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Highest_Priority(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_deactive_users_for_instance_and_process = () =>
        {
            assignment.WasToldTo(x => x.DeActiveUsersForInstanceAndProcess((IDomainMessageCollection)messages,
                instanceData.ID,
                workflowData.ApplicationKey,
                dys,
                SAHL.Common.Globals.Process.Origination));
        };

        private It should_round_robin_assign_other_fl_cases = () =>
        {
            assignment.WasToldTo(x => x.RoundRobinAndAssignOtherFLCases((IDomainMessageCollection)messages, workflowData.ApplicationKey, dys[0], 157, instanceData.ID, "QA", (int)SAHL.Common.Globals.RoundRobinPointers.FLProcessor));
        };

        private It should_add_account_memo_message_on_reciept_of_application = () =>
        {
            fl.WasToldTo(x => x.AddAccountMemoMessageOnReceiptOfApplication((IDomainMessageCollection)messages,
                workflowData.ApplicationKey,
                paramsData.ADUserName,
                expectedAssignedTo));
        };

        private It should_update_assigned_user_in_idm = () =>
        {
            common.WasToldTo(x => x.UpdateAssignedUserInIDM((IDomainMessageCollection)messages,
                workflowData.ApplicationKey,
                workflowData.IsFL,
                instanceData.ID,
                "Application Management"));
        };

        private It should_return_true = () =>
        {
            result.ShouldEqual(true);
        };
    }
}