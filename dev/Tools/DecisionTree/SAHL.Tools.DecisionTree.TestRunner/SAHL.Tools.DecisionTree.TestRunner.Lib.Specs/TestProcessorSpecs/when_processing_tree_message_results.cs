using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Tools.DecisionTree.TestRunner.Interfaces;

using SAHL.Tools.DecisionTree.TestRunner.Lib.TestResult;
using SAHL.Tools.DecisionTree.TestRunner.Lib.Specs.Fakes;

namespace SAHL.Tools.DecisionTree.TestRunner.Lib.Specs
{
    public class when_processing_tree_message_results_with_contains_assertion : WithFakes
    {
        private static TestProcessorFactory factory;
        private static TestProcessor processor;
        private static List<IOutputMessage> outputMessages;
        private static ISystemMessageCollection actualMessages;
        private static ISystemMessage actualMessage;
        private static string messageText;
        private static List<ITestResult> result;

        private Establish context = () =>
        {
            factory = new TestProcessorFactory();
            processor = factory.Processor;

            messageText = "Applicant must be older than 18 years";

            outputMessages = new List<IOutputMessage>
            {
                new OutputMessage(messageText, "should contain", "Warning Messages"),
                new OutputMessage("This message does not exist", "should contain", "Warning Messages")
            };
            actualMessages = SystemMessageCollection.Empty();
            actualMessage = new SystemMessage(messageText, SystemMessageSeverityEnum.Warning);
            actualMessages.AddMessage(actualMessage);
        };

        private Because of = () =>
        {
            result = processor.ProcessDecisionTreeMessageResults(actualMessages, outputMessages);
        };

        private It should_call_the_assertion_manager_for_the_first_test = () =>
        {
            factory.assertionManager.WasToldTo(x => x.AssertMessageTestPassed(
                Param<List<ISystemMessage>>.Matches(y => y.Contains(actualMessage)), outputMessages[0]));
        };

        private It should_call_the_assertion_manager_for_the_second_test = () =>
        {
            factory.assertionManager.WasToldTo(x => x.AssertMessageTestPassed(
                Param<List<ISystemMessage>>.Matches(y => y.Contains(actualMessage)), outputMessages[1]));
        };

        private It should_return_a_result_for_each_assertion = () =>
        {
            result.Count.ShouldEqual(2);
        };
    }
}