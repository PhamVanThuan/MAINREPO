using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.Send_Documents.OnComplete
{
    [Subject("Activity => Send_Documents => OnComplete")]
    internal class when_send_documents : WorkflowSpecPersonalLoans
    {
        private static bool result;
        private static IWorkflowAssignment wfa;
        private static string message = String.Empty;

        private Establish context = () =>
        {
            result = false;
            wfa = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(wfa);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Send_Documents(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };

        private It should_round_robin_assign_the_personal_loans_administrator = () =>
        {
            wfa.WasToldTo(x => x.ReactivateUserOrRoundRobinForWorkflowRoleAssignment((IDomainMessageCollection)messages, SAHL.Common.Globals.GenericKeyTypes.WorkflowRoleType,
                SAHL.Common.Globals.WorkflowRoleTypes.PLAdminD, workflowData.ApplicationKey, instanceData.ID, SAHL.Common.Globals.RoundRobinPointers.PLAdmin));
        };
    }
}