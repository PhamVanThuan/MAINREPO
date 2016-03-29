using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using X2DomainService.Interface.DebtCounselling;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Court_Application_Withdrawn.OnComplete
{
    [Subject("Activity => Court_Application_Withdrawn => OnComplete")]
    internal class when_court_application_and_assignment_fails : WorkflowSpecDebtCounselling
    {
        private static bool result;
        private static string message;
        private static IWorkflowAssignment workflowAssignment;
        private static IDebtCounselling client;

        private Establish context = () =>
        {
            result = true;
            message = string.Empty;
            client = An<IDebtCounselling>();
            workflowAssignment = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IDebtCounselling>(client);
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(workflowAssignment);
            client.WhenToldTo(x => x.UpdateHearingDetailStatusToInactive(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>())).Return(true);
            workflowAssignment.WhenToldTo(x => x.AssignDebtCounsellingCaseForGroupOrLoadBalance(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<long>(),
                Param.IsAny<int>(), Param.IsAny<SAHL.Common.Globals.WorkflowRoleTypes>(), Param.IsAny<string>(), Param.IsAny<List<string>>(),
                Param.IsAny<bool>(), Param.IsAny<bool>())).Return(false);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Court_Application_Withdrawn(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}