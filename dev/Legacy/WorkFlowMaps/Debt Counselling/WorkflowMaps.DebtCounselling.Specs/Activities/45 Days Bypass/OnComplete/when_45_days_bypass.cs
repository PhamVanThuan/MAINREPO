//using Machine.Specifications;

//namespace WorkflowMaps.DebtCounselling.Specs.Activities._45_Days_Bypass.OnComplete
//{
//    [Subject("Activity => 45_Days_Bypass => OnComplete")] // AutoGenerated
//    internal class when_45_days_bypass : WorkflowSpecDebtCounselling
//    {
//        static bool result;
//        Establish context = () =>
//        {
//            result = false;
//        };

//        Because of = () =>
//        {
//            string message = string.Empty;
//            result = workflow.OnCompleteActivity_45_Days_Bypass(instanceData, workflowData,paramsData, messages, ref message);
//        };

//        It should_return_true = () =>
//        {
//            result.ShouldBeTrue();
//        };
//    }
//}