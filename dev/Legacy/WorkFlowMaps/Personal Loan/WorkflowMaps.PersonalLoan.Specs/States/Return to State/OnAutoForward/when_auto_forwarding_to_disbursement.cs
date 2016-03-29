using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using System;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.PersonalLoan.Specs.States.Return_to_State.OnAutoForward
{
    [Subject("State => Return_to_State => OnAutoForward")]
    internal class when_auto_forwarding_to_disbursement : WorkflowSpecPersonalLoans
    {
        protected static IWorkflowAssignment workflowAssignment;
        protected static string result = String.Empty;
        protected static string previousState = "Disbursement";
        protected static WorkflowRoleTypes workflowRoleType = WorkflowRoleTypes.PLSupervisorD;
        protected static RoundRobinPointers roundRobinPointer = RoundRobinPointers.PLSupervisor;

        private Establish context = () =>
        {
            workflowAssignment = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(workflowAssignment);
            workflowData.PreviousState = previousState;
        };

        private Because of = () =>
        {
            result = workflow.GetForwardStateName_Return_to_State(instanceData, workflowData, paramsData, messages);
        };

        private It should_assign_case_to_personal_loans_supervisor = () =>
        {
            workflowAssignment.WasToldTo(x => x.ReactivateUserOrRoundRobinForWorkflowRoleAssignment((IDomainMessageCollection)messages, SAHL.Common.Globals.GenericKeyTypes.WorkflowRoleType, workflowRoleType, workflowData.ApplicationKey, instanceData.ID, roundRobinPointer));
        };

        private It should_return_disbursement_as_the_previous_state = () =>
        {
            result.ShouldEqual(previousState);
        };
    }
}