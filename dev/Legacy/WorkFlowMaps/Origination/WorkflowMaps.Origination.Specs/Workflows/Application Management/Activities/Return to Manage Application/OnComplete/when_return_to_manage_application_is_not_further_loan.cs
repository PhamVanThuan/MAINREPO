using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Return_to_Manage_Application.OnComplete
{
    [Subject("Activity => Return_to_Manage_Application => OnComplete")]
    internal class when_return_to_manage_application_is_not_further_loan : WorkflowSpecApplicationManagement
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
            workflowData.IsFL = false;
            assignmentClient = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(assignmentClient);

            expectedDynamicRoles = new List<string>
            {
                "Branch Consultant D",
                "Branch Admin D",
                "Branch Manager D",
                "New Business Processor D",
                "New Business Supervisor D",
                "FL Processor D",
                "FL Supervisor D",
                "FL Manager D",
            };
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Return_to_Manage_Application(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_archive_v3_itc_for_application = () =>
        {
            commonClient.WasToldTo(x => x.ArchiveV3ITCForApplication((IDomainMessageCollection)messages, workflowData.ApplicationKey));
        };

        private It should_create_new_revision_for_application = () =>
        {
            commonClient.WasToldTo(x => x.CreateNewRevision((IDomainMessageCollection)messages, workflowData.ApplicationKey));
        };

        private It should_deactivate_users_for_instance = () =>
        {
            assignmentClient.WasToldTo(x => x.DeActiveUsersForInstanceAndProcess((IDomainMessageCollection)messages, instanceData.ID, workflowData.ApplicationKey, expectedDynamicRoles, SAHL.Common.Globals.Process.Origination));
        };

        private It should_reactivate_user_or_round_robin_for_new_business_processors = () =>
        {
            assignmentClient.WasToldTo(x => x.ReactiveUserOrRoundRobinForOSKeyByProcess((IDomainMessageCollection)messages, "New Business Processor D", workflowData.ApplicationKey, 106, instanceData.ID,
                                        "Manage Application", SAHL.Common.Globals.Process.Origination,
                                                            (int)SAHL.Common.Globals.RoundRobinPointers.NewBusinessProcessor));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}