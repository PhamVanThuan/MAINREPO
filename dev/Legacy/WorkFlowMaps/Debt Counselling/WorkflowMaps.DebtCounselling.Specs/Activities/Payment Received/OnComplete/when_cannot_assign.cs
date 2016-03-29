using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Payment_Received.OnComplete
{
    [Subject("Activity => Payment_Received => OnComplete")]
    internal class when_cannot_assign : WorkflowSpecDebtCounselling
    {
        private static IWorkflowAssignment wfa;
        private static string message;
        private static bool result;

        private Establish context = () =>
        {
            result = true;
            workflowData.DebtCounsellingKey = 1;
            ((InstanceDataStub)instanceData).ID = 1;
            wfa = An<IWorkflowAssignment>();
            wfa.WhenToldTo(x => x.AssignDebtCounsellingCaseForGroupOrLoadBalance(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<long>(),
                Param.IsAny<int>(), Param.IsAny<SAHL.Common.Globals.WorkflowRoleTypes>(), Param.IsAny<string>(), Param.IsAny<List<string>>(),
                Param.IsAny<bool>(), Param.IsAny<bool>())).Return(false);
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(wfa);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Payment_Received(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}