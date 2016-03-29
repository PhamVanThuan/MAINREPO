using System;
using System.Collections.Generic;
using System.Linq;
using SAHL.Core.SystemMessages;
using SAHL.Tools.DecisionTree.TestRunner.Interfaces;
using SAHL.Tools.DecisionTree.TestRunner.Lib.TestResult;

namespace SAHL.Tools.DecisionTree.TestRunner.Lib.Assertions
{
    public class AssertionManager : IAssertionManager
    {
        private IAssertionFactory assertionFactory;

        public AssertionManager(IAssertionFactory assertionFactory)
        {
            this.assertionFactory = assertionFactory;
        }
       
        public SubtreeExpectationResult AssertSubtreeTestPassed(List<string> calledSubtrees, ISubtreeExpectation expectation)
        {
            bool passed = true;

            var expectedSubtreeName = Utilities.StripInvalidChars(expectation.SubtreeName);
            expectedSubtreeName = Utilities.LowerFirstLetter(expectedSubtreeName);

            var subtreeAssertion = assertionFactory.GetCollectionAssertion(expectation.Assertion);
            passed = subtreeAssertion.Assert(calledSubtrees, expectedSubtreeName);

            var result = new SubtreeExpectationResult(expectedSubtreeName, expectation.Assertion, calledSubtrees, passed);
            return result;
        }

        public TestMessageResult AssertMessageTestPassed(List<ISystemMessage> messages, IOutputMessage outputMessage)
        {
            bool passed = true;

            var messageCollection = messages.Where(x => Utilities.IsMessageOfSeverity(x, outputMessage.ExpectedMessageSeverity)).Select(y=>y.Message).ToList();
            var messageAssertion = assertionFactory.GetCollectionAssertion(outputMessage.Assertion);
            passed = messageAssertion.Assert(messageCollection, outputMessage.ExpectedMessage);
           
            var testResult = new TestMessageResult(outputMessage, messages, passed);
            return testResult;
        }

        public TestOutputResult AssertOutputTestPassed(ITestOutput testOutput, object expected, object actual, Type outputType)
        {
            bool passed = true;

            var actualValue = Convert.ChangeType(actual, outputType);
            var expectedValue = Convert.ChangeType(expected, outputType);

            var outputAssertion = assertionFactory.GetAssertion(testOutput.Assertion);
            passed = outputAssertion.Assert(expectedValue, actualValue, outputType);

            var result = new TestOutputResult(testOutput, expectedValue.ToString(), actualValue.ToString(), passed);
            return result;
        }
    }
}
