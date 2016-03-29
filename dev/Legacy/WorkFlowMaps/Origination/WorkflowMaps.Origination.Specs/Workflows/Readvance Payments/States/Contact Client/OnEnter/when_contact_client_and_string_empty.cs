using Machine.Fakes;
using Machine.Specifications;
using Rhino.Mocks;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ReadvancePayments.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.States.Contact_Client.OnEnter
{
    [Subject("State => Contact_Client => OnEnter")]
    internal class when_contact_client_and_string_empty : WorkflowSpecReadvancePayments
    {
        private static bool result;
        private static IWorkflowAssignment workflowAssignment;
        private static string adUserName;

        private Establish context = () =>
        {
            result = false;
            ((InstanceDataStub)instanceData).SourceInstanceID = 1;
            adUserName = @"SAHL\FLAppProcUser";
            workflowAssignment = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(workflowAssignment);
            workflowAssignment.Expect(x => x.GetUsersWhoWorkedOnThisCaseInOtherMapsWhereUserIsStillActiveAndInSameOrgStruct((IDomainMessageCollection)messages, (long)instanceData.SourceInstanceID, "FL Processor D", 157, out adUserName)).OutRef("");
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Contact_Client(instanceData, workflowData, paramsData, messages);
        };

        private It should_not_reassign_case_user_by_process = () =>
        {
            workflowAssignment.WasNotToldTo(x => x.ReassignCaseToUserByProcess((IDomainMessageCollection)messages, instanceData.ID, workflowData.ApplicationKey, adUserName, 157, 857, "Contact Client", SAHL.Common.Globals.Process.Origination));
        };

        private It should_reactive_user_or_round_robin_for_oskey_by_process = () =>
        {
            workflowAssignment.WasToldTo(x => x.ReactiveUserOrRoundRobinForOSKeyByProcess((IDomainMessageCollection)messages, "FL Processor D", workflowData.ApplicationKey, 157, instanceData.ID, "Contact Client", SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.FLProcessor));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}