//using Machine.Fakes;
//using Machine.Specifications;
//using SAHL.Common.Collections.Interfaces;
//using WorkflowMaps.ApplicationManagement.Specs;
//using WorkflowMaps.Specs.Common;
//using X2DomainService.Interface.Common;
//using X2DomainService.Interface.Origination;

//namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Request_Resolved.OnComplete
//{
//    [Subject("Activity => Request_Resolved => OnComplete")]
//    internal class when_requet_resolved_where_is_not_fl_and_do_credit_score_check_fails : WorkflowSpecApplicationManagement
//    {
//        static bool result;
//        static string message;
//        static ICommon common;
//        static IApplicationCapture appCap;

//        Establish context = () =>
//        {
//            result = false;
//            message = string.Empty;
//            workflowData.IsFL = false;
//            workflowData.ApplicationKey = 1;
//            ((ParamsDataStub)paramsData).ADUserName = "ADUserNameTest";
//            ((ParamsDataStub)paramsData).IgnoreWarning = true;

//            common = An<ICommon>();
//            domainServiceLoader.RegisterMockForType<ICommon>(common);

//            appCap = An<IApplicationCapture>();
//            domainServiceLoader.RegisterMockForType<IApplicationCapture>(appCap);
//        };

//        Because of = () =>
//        {
//            result = workflow.OnCompleteActivity_Request_Resolved(instanceData, workflowData, paramsData, messages, ref message);
//        };

//        It should_check_branch_submit_application_rules = () =>
//        {
//            appCap.WasToldTo(x => x.CheckBranchSubmitApplicationRules((IDomainMessageCollection)messages, workflowData.ApplicationKey, paramsData.IgnoreWarning));
//        };

//        It should_return_false = () =>
//        {
//            result.ShouldBeFalse();
//        };
//    }
//}