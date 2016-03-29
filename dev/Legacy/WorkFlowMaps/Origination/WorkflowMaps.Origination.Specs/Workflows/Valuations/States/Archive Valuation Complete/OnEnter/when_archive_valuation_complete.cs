using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using WorkflowMaps.Valuations.Specs;
using X2DomainService.Interface.Origination;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Valuations.States.Archive_Valuation_Complete.OnEnter
{
    [Subject("State => Archive_Valuation_Complete => OnEnter")]
    internal class when_archive_valuation_complete : WorkflowSpecValuations
    {
        private static IValuations client;
        private static IWorkflowAssignment assignmentClient;
        private static bool result;
        private static List<string> expectedRoles;

        private Establish context = () =>
        {
            client = An<IValuations>();
            assignmentClient = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IValuations>(client);
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(assignmentClient);

            expectedRoles = new List<string>();
            expectedRoles.Add("Valuations Administrator D");
            expectedRoles.Add("Valuations Manager D");
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Archive_Valuation_Complete(instanceData, workflowData, paramsData, messages);
        };

        private It should_set_valuation_status_to_complete = () =>
        {
            client.WasToldTo(x => x.SetValuationStatus((IDomainMessageCollection)messages, workflowData.ValuationKey, (int)SAHL.Common.Globals.ValuationStatuses.Complete));
        };

        private It should_make_valuation_active = () =>
        {
            client.WasToldTo(x => x.SetValuationActiveAndSave((IDomainMessageCollection)messages, workflowData.ValuationKey));
        };

        private It send_email_to_consultants = () =>
        {
            client.WasToldTo(x => x.SendEmailToAllOpenApplicationConsultantsForValuationComplete
                                        ((IDomainMessageCollection)messages, workflowData.ValuationKey, workflowData.ApplicationKey));
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