using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.Origination;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Request_Resolved.OnComplete
{
    [Subject("Activity => Request_Resolved => OnComplete")]
    internal class when_request_resolved_where_is_not_fl_and_do_credit_score_check_passes : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static string message;
        private static ICommon common;
        private static IApplicationCapture appCap;
        private static IWorkflowAssignment workflowAssignment;

        private Establish context = () =>
        {
            result = false;
            message = string.Empty;
            workflowData.IsFL = false;
            workflowData.ApplicationKey = 1;
            ((ParamsDataStub)paramsData).ADUserName = "ADUserNameTest";
            ((ParamsDataStub)paramsData).IgnoreWarning = true;

            common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(common);
            common.WhenToldTo(x => x.CheckApplicationMinimumIncomeRules(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<bool>())).Return(true);

            appCap = An<IApplicationCapture>();
            appCap.WhenToldTo(x => x.CheckBranchSubmitApplicationRules(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<bool>()))
                .Return(true);
            domainServiceLoader.RegisterMockForType<IApplicationCapture>(appCap);

            workflowAssignment = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(workflowAssignment);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Request_Resolved(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_check_application_minimum_income_rules = () =>
        {
            common.WasToldTo(x => x.CheckApplicationMinimumIncomeRules(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<bool>()));
        };

        private It should_check_branch_submit_application_rules = () =>
        {
            appCap.WasToldTo(x => x.CheckBranchSubmitApplicationRules((IDomainMessageCollection)messages, workflowData.ApplicationKey, paramsData.IgnoreWarning));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}