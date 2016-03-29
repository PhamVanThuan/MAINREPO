using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using WorkflowMaps.Credit.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Origination;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.States.Archive_Declined_by_Credit.OnEnter
{
    [Subject("State => Archive_Declined_by_Credit => OnEnter")]
    internal class when_archive_declined_by_credit_and_is_resub : WorkflowSpecCredit
    {
        private static bool result;
        private static IWorkflowAssignment assignment;
        private static ICredit credit;
        private static List<string> dys;

        private Establish context = () =>
        {
            result = false;
            workflowData.IsResub = true;
            dys = new List<string>() { "Credit Underwriter D",
                "Credit Supervisor D",
                "Credit Manager D",
                "Credit Exceptions D" };
            ((InstanceDataStub)instanceData).ID = 1;
            workflowData.ApplicationKey = 2;
            assignment = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(assignment);
            credit = An<ICredit>();
            domainServiceLoader.RegisterMockForType<ICredit>(credit);
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Archive_Declined_by_Credit(instanceData, workflowData, paramsData, messages);
        };

        private It should_send_resub_mail = () =>
        {
            credit.WasToldTo(x => x.SendResubMail((IDomainMessageCollection)messages, workflowData.ApplicationKey));
        };

        private It should_deactive_user_for_instance_and_process = () =>
        {
            assignment.WasToldTo(x => x.DeActiveUsersForInstanceAndProcess((IDomainMessageCollection)messages,
                instanceData.ID,
                workflowData.ApplicationKey,
                dys,
                SAHL.Common.Globals.Process.Origination));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}