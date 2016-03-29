using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using WorkflowMaps.Valuations.Specs;
using X2DomainService.Interface.Origination;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Valuations.States.ParentCaseArchive.OnEnter
{
    [Subject("State => ParentCaseArchive => OnEnter")]
    internal class when_parent_case_archive : WorkflowSpecValuations
    {
        private static bool result;
        private static IWorkflowAssignment assignmentClient;
        private static IValuations valuationClient;
        private static List<string> expectedRoles;

        private Establish context = () =>
        {
            assignmentClient = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(assignmentClient);
            valuationClient = An<IValuations>();
            domainServiceLoader.RegisterMockForType<IValuations>(valuationClient);

            expectedRoles = new List<string>();
            expectedRoles.Add("Valuations Administrator D");
            expectedRoles.Add("Valuations Manager D");
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_ParentCaseArchive(instanceData, workflowData, paramsData, messages);
        };

        private It send_email_to_consultants = () =>
        {
            valuationClient.WasToldTo(x => x.SendEmailToAllOpenApplicationConsultantsForValuationComplete((IDomainMessageCollection)messages, workflowData.ValuationKey, workflowData.ApplicationKey));
        };

        private It should_deactivate_users_for_instance = () =>
        {
            assignmentClient.WasToldTo(x => x.DeActiveUsersForInstanceAndProcess((IDomainMessageCollection)messages, instanceData.ID, workflowData.ApplicationKey, expectedRoles, SAHL.Common.Globals.Process.Origination));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}