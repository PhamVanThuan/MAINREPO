using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Accept.OnComplete
{
    [Subject("Activity => Accept => OnComplete")]
    internal class when_accept_and_assignment_fails : WorkflowSpecDebtCounselling
    {
        private static bool result;
        private static string message;
        private static IWorkflowAssignment workflowAssignment;
        private static List<string> statesToExclude = new List<string> { "Debt Review Approved", "Bond Exclusions", "Bond Exclusions Arrears" };

        private Establish context = () =>
        {
            result = true;
            message = string.Empty;

            workflowAssignment = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(workflowAssignment);
            workflowAssignment.WhenToldTo(x => x.AssignDebtCounsellingCaseForGroupOrLoadBalance(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<long>(),
                Param.IsAny<int>(), Param.IsAny<SAHL.Common.Globals.WorkflowRoleTypes>(), Param.IsAny<string>(), Param.IsAny<List<string>>(),
                Param.IsAny<bool>(), Param.IsAny<bool>())).Return(false);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Accept(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}