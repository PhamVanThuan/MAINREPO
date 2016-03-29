using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Court_Order_Granted.OnComplete
{
    [Subject("Activity => Court_Order_Granted => OnComplete")]
    internal class when_court_order_granted_and_assignment_fails : WorkflowSpecDebtCounselling
    {
        private static IWorkflowAssignment wfa;
        private static bool result;
        private static string message;

        private Establish context = () =>
        {
            result = true;
            wfa = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(wfa);
            wfa.WhenToldTo(x => x.AssignDebtCounsellingCaseForGroupOrLoadBalance(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<long>(),
                Param.IsAny<int>(), Param.IsAny<SAHL.Common.Globals.WorkflowRoleTypes>(), Param.IsAny<string>(),
                Param.IsAny<List<string>>(), Param.IsAny<bool>(), Param.IsAny<bool>())).Return(false);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Court_Order_Granted(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}