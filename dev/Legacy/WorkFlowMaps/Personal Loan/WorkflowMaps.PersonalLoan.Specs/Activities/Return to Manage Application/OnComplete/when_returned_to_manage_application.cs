using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.Return_to_Manage_Application.OnComplete
{
    [Subject("Activity => Return_to_Manage_Application => OnComplete")]
    internal class when_returned_to_manage_application : WorkflowSpecPersonalLoans
    {
        private static bool result;
        private static string message;
        private static IWorkflowAssignment client;

        private Establish context = () =>
        {
            workflowData.ApplicationKey = 1;
            ((InstanceDataStub)instanceData).ID = 1;
            result = false;
            client = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType(client);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Return_to_Manage_Application(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };

        private It should_round_robin_or_reactivate_the_personal_loan_admin = () =>
        {
            client.WasToldTo(x => x.ReactivateUserOrRoundRobinForWorkflowRoleAssignment((IDomainMessageCollection)messages, SAHL.Common.Globals.GenericKeyTypes.WorkflowRoleType,
                SAHL.Common.Globals.WorkflowRoleTypes.PLConsultantD, workflowData.ApplicationKey, instanceData.ID, SAHL.Common.Globals.RoundRobinPointers.PLConsultant));
        };
    }
}