using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using System;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.Document_Check.OnComplete
{
    [Subject("Activity => Document_Check => OnComplete")]
    public class when_document_check_completed : WorkflowSpecPersonalLoans
    {
        private static bool result;
        private static IWorkflowAssignment workflowAssignment;
        private static string message;

        private Establish context = () =>
        {
            message = String.Empty;
            workflowAssignment = An<IWorkflowAssignment>();

            workflowAssignment.WhenToldTo(x => x.ReactivateUserOrRoundRobinForWorkflowRoleAssignment(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<GenericKeyTypes>(), Param.IsAny<WorkflowRoleTypes>(), Param.IsAny<int>(), Param.IsAny<long>(), Param.IsAny<RoundRobinPointers>())).Return("");

            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(workflowAssignment);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Document_Check(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_perform_assignment = () =>
        {
            workflowAssignment.WasToldTo(x => x.ReactivateUserOrRoundRobinForWorkflowRoleAssignment(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<GenericKeyTypes>(), Param.IsAny<WorkflowRoleTypes>(), Param.IsAny<int>(), Param.IsAny<long>(), Param.IsAny<RoundRobinPointers>()));
        };

        private It should_update_message_argument = () =>
        {
            message.ShouldEqual("");
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}