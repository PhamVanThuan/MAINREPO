//using Machine.Specifications;

//namespace WorkflowMaps.Valuations.Specs.States.Archive_Valuation_Clone.OnExit
//{
//    [Subject("State => Archive_Valuation_Clone => OnExit")] // AutoGenerated
//    internal class when_archive_valuation_clone : WorkflowSpecValuations
//    {
//        static bool result;
//        Establish context = () =>
//        {
//            result = false;
//        };

//        Because of = () =>
//        {
//            result = workflow.OnExit_Archive_Valuation_Clone(instanceData, workflowData, paramsData, messages);
//        };

//        It should_return_true = () =>
//        {
//            result.ShouldBeTrue();
//        };
//    }
//}