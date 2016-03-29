using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System;
using System.Collections.Generic;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Valuations.Specs.Activities.Review_Valuation_Required.OnComplete
{
    [Subject("Activity => Review_Valuation_Required => OnComplete")]
    internal class when_review_valuation_required_on_manager_worklist : WorkflowSpecValuations
    {
        private static ICommon client;
        private static IWorkflowAssignment workflowAssignmentClient;
        private static string message;
        private static bool result;
        private static List<string> roles;

        private Establish context = () =>
        {
            roles = new List<string>
            {
                "Valuations Administrator D",
	            "Valuations Manager D"
            };
            workflowAssignmentClient = An<IWorkflowAssignment>();
            message = String.Empty;
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(workflowAssignmentClient);
            domainServiceLoader.RegisterMockForType<ICommon>(client);
            result = false;
            workflowData.OnManagerWorkList = true;
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Review_Valuation_Required(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_deactivate_users_for_instance = () =>
        {
            workflowAssignmentClient.WasToldTo(x => x.DeActiveUsersForInstanceAndProcess((IDomainMessageCollection)messages, 0, 0, roles, SAHL.Common.Globals.Process.Origination));
        };

        private It should_set_on_manager_worklist_data_property_to_false = () =>
        {
            workflowData.OnManagerWorkList.ShouldBeFalse();
        };

        private It should_reassign_to_previous_valuation_user_or_round_robin = () =>
        {
            workflowAssignmentClient.WasToldTo(x => x.ReassignToPreviousValuationsUserIfExistsElseRoundRobin((IDomainMessageCollection)messages,
                "Valuations Administrator D", 111, 0, "Valuations", 0, "Valuation Review Request",
                 (int)SAHL.Common.Globals.RoundRobinPointers.ValuationsAdministrator));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}