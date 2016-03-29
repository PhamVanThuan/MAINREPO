using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Payment_Received.OnComplete
{
    [Subject("Activity => Payment_Received => OnComplete")]
    internal class when_can_assign : WorkflowSpecDebtCounselling
    {
        private static IWorkflowAssignment wfa;
        private static string message;
        private static bool result;
        private static List<string> inclusionStates = new List<string> { "Recoveries Proposal Decision", "Decision on Proposal", "Payment Review" };

        private Establish context = () =>
            {
                result = false;
                workflowData.DebtCounsellingKey = 1;
                workflowData.CourtCase = false;
                ((InstanceDataStub)instanceData).ID = 1;
                wfa = An<IWorkflowAssignment>();
                wfa.WhenToldTo(x => x.AssignDebtCounsellingCaseForGroupOrLoadBalance(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<long>(),
                    Param.IsAny<int>(), Param.IsAny<SAHL.Common.Globals.WorkflowRoleTypes>(), Param.IsAny<string>(), Param.IsAny<List<string>>(),
                    Param.IsAny<bool>(), Param.IsAny<bool>())).Return(true);
                domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(wfa);
            };

        private Because of = () =>
            {
                result = workflow.OnCompleteActivity_Payment_Received(instanceData, workflowData, paramsData, messages, ref message);
            };

        private It should_return_true = () =>
            {
                result.ShouldBeTrue();
            };

        private It should_assign_or_load_balance_the_case_to_the_debt_counselling_supervisor = () =>
            {
                wfa.WasToldTo(x => x.AssignDebtCounsellingCaseForGroupOrLoadBalance((IDomainMessageCollection)messages, instanceData.ID, workflowData.DebtCounsellingKey,
                        SAHL.Common.Globals.WorkflowRoleTypes.DebtCounsellingSupervisorD, "Payment Received", inclusionStates, true, workflowData.CourtCase));
            };
    }
}