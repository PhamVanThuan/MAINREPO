using Machine.Specifications;
using WorkflowMaps.Credit.Specs;
using X2DomainService.Interface.WorkflowAssignment;
using System.Collections.Generic;
using Machine.Fakes;
using WorkflowMaps.Specs.Common;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.States.Archive_Resub_Remain.OnEnter
{
    /*[Subject("State => Archive_Resub_Remain => OnEnter")]
    internal class when_archive_resub_remain : WorkflowSpecCredit
    {
        static bool result;
        static IWorkflowAssignment assignment;
        static List<string> dys;

        Establish context = () =>
        {
            result = false;
            dys = new List<string>() { "Credit Underwriter D",
                "Credit Supervisor D",
                "Credit Manager D",
                "Credit Exceptions D" };
            ((InstanceDataStub)instanceData).ID = 1;
            workflowData.ApplicationKey = 2;
            assignment = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(assignment);
        };

        Because of = () =>
        {
            result = workflow.OnEnter_Archive_Resub_Remain(instanceData, workflowData, paramsData, messages);
        };

        It should_deactive_user_for_instance_and_process = () =>
        {
            assignment.WasToldTo(x => x.DeActiveUsersForInstanceAndProcess((IDomainMessageCollection)messages,
                instanceData.ID,
                workflowData.ApplicationKey,
                dys,
                SAHL.Common.Globals.Process.Origination));
        };

        It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }*/
}
