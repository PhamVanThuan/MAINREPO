using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.DebtCounselling;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.DebtCounselling.Specs.Activities._5days.OnComplete
{
    [Subject("Activity => 5_Days => OnComplete")]
    internal class when_5_days_and_assignment_fails : WorkflowSpecDebtCounselling
    {
        private static bool result;
        private static string message;

        private static IWorkflowAssignment workflowAssignment;
        private static IDebtCounselling debtcounsellingClient;

        private Establish context = () =>
        {
            debtcounsellingClient = An<IDebtCounselling>();
            domainServiceLoader.RegisterMockForType<IDebtCounselling>(debtcounsellingClient);

            result = true;

            workflowAssignment = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(workflowAssignment);
            var client = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(client);
            workflowAssignment.WhenToldTo(x => x.AssignDebtCounsellingCaseForGroupOrLoadBalance(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<long>(),
                Param.IsAny<int>(), Param.IsAny<SAHL.Common.Globals.WorkflowRoleTypes>(), Param.IsAny<string>(), Param.IsAny<List<string>>(),
                Param.IsAny<bool>(), Param.IsAny<bool>())).Return(false);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_5days(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_Post_973_Financial_Transaction = () =>
        {
            debtcounsellingClient.WasToldTo(x => x.RollbackTransaction((IDomainMessageCollection)messages, workflowData.DebtCounsellingKey));
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}