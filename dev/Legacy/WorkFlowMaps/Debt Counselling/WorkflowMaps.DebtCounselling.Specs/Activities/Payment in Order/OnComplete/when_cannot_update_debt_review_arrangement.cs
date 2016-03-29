using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using X2DomainService.Interface.DebtCounselling;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Payment_in_Order.OnComplete
{
    [Subject("Activity => Payment_in_Order => OnComplete")]
    internal class when_cannot_update_debt_review_arrangement : WorkflowSpecDebtCounselling
    {
        private static IDebtCounselling client;
        private static IWorkflowAssignment wfa;
        private static bool result;
        private static string message;

        private Establish context = () =>
            {
                result = true;
                client = An<IDebtCounselling>();
                wfa = An<IWorkflowAssignment>();
                client.WhenToldTo(x => x.UpdateDebtCounsellingDebtReviewArrangement(Param.IsAny<IDomainMessageCollection>(),
                    Param.IsAny<int>(), Param.IsAny<string>())).Return(false);
                domainServiceLoader.RegisterMockForType<IDebtCounselling>(client);
                domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(wfa);
            };

        private Because of = () =>
            {
                result = workflow.OnCompleteActivity_Payment_in_Order(instanceData, workflowData, paramsData, messages, ref message);
            };

        private It should_return_false = () =>
            {
                result.ShouldBeFalse();
            };

        private It should_not_try_assign_the_case = () =>
            {
                wfa.WasNotToldTo(x => x.AssignDebtCounsellingCaseForGroupOrLoadBalance(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<long>(),
                    Param.IsAny<int>(), Param.IsAny<SAHL.Common.Globals.WorkflowRoleTypes>(), Param.IsAny<string>(), Param.IsAny<List<string>>(),
                    Param.IsAny<bool>(), Param.IsAny<bool>()));
            };
    }
}