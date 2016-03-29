using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using WorkflowMaps.ApplicationManagement.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.Origination;
using X2DomainService.Interface.WorkflowAssignment;


namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Feedback_on_Query.OnComplete
{
    public class when_feed_back_on_query_and_CheckSubmissionRules_fail : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static bool expectedResult;
        private static string message;
        private static ICommon common;
        private static IWorkflowAssignment assignment;
        private static IApplicationCapture appCap;
        private static List<string> dys;



        private Establish context = () =>
        {
            result = true;
            expectedResult = false;
            message = string.Empty;
            workflowData.IsFL = false;
            workflowData.ApplicationKey = 1;
            ((ParamsDataStub)paramsData).IgnoreWarning = true;
            ((InstanceDataStub)instanceData).ID = 2;
            dys = new List<string>() {SAHL.Common.ApplicationCapture.WorkflowRoles.BranchConsultantD,
                SAHL.Common.ApplicationCapture.WorkflowRoles.BranchAdminD,
                SAHL.Common.ApplicationCapture.WorkflowRoles.BranchManagerD};

            common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(common);
            common.WhenToldTo(x => x.CheckApplicationMinimumIncomeRules(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<bool>())).Return(true);

            assignment = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(assignment);

            appCap = An<IApplicationCapture>();
            appCap.WhenToldTo(x => x.CheckBranchSubmitApplicationRules(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<bool>()))
                .Return(expectedResult);
            domainServiceLoader.RegisterMockForType<IApplicationCapture>(appCap);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Feedback_on_Query(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_check_application_minimum_income_rules = () =>
        {
            common.WasToldTo(x => x.CheckApplicationMinimumIncomeRules(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<bool>()));
        };

        private It should_check_branch_submit_application_rules = () =>
        {
            appCap.WasToldTo(x => x.CheckBranchSubmitApplicationRules((IDomainMessageCollection)messages, workflowData.ApplicationKey, paramsData.IgnoreWarning));
        };

        private It should_not_perform_pricing_for_risk = () =>
        {
            common.WasNotToldTo(x => x.PricingForRisk((IDomainMessageCollection)messages, workflowData.ApplicationKey));
        };
    }
}
