using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Return_Processor_Credit.OnComplete
{
    [Subject("Activity => Return_Processor_Credit => OnComplete")]
    internal class when_return_processor_credit_is_further_loan : WorkflowSpecApplicationManagement
    {
        private static ICommon commonClient;
        private static IWorkflowAssignment assignmentClient;
        private static List<string> expectedDynamicRoles;
        private static bool result;
        private static string message;

        private Establish context = () =>
        {
            commonClient = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(commonClient);
            workflowData.IsFL = true;
            assignmentClient = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(assignmentClient);

            expectedDynamicRoles = new List<string>
            {
                "Branch Consultant D",
                "Branch Admin D",
                "Branch Manager D",
                "New Business Processor D",
                "FL Processor D",
                "FL Supervisor D",
                "FL Manager D",
            };
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Return_Processor_Credit(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_deactivate_users_for_instance = () =>
        {
            assignmentClient.WasToldTo(x => x.DeActiveUsersForInstanceAndProcess((IDomainMessageCollection)messages, instanceData.ID, workflowData.ApplicationKey, expectedDynamicRoles, SAHL.Common.Globals.Process.Origination));
        };

        private It should_reactivate_user_or_round_robin_for_fl_processors = () =>
        {
            assignmentClient.WasToldTo(x => x.ReactiveUserOrRoundRobinForOSKeyByProcess((IDomainMessageCollection)messages, "FL Processor D", workflowData.ApplicationKey, 157,
                   instanceData.ID, "Manage Application", SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.FLProcessor));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}