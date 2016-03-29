using System;
using System.Collections.Generic;
using System.Linq;
using SAHL.Core.SystemMessages;
using SAHL.Tools.DecisionTree.TestRunner.Interfaces;
using SAHL.Tools.DecisionTree.TestRunner.Lib.Assertions;
using SAHL.Tools.DecisionTree.TestRunner.Lib.TestResult;
using SAHL.Tools.DecisionTree.TestRunner.Lib.Interfaces;

namespace SAHL.Tools.DecisionTree.TestRunner.Lib
{
    public class TestProcessor : ITestProcessor
    {
        private IAssertionManager assertionManager;

        public TestProcessor(IAssertionManager assertionManager)
        {
            this.assertionManager = assertionManager;
        }

        public List<ITestResult> ProcessDecisionTreeOutputResults(dynamic treeVariablesObject, List<ITestOutput> expectedTestOutputs, dynamic enumerations)
        {
            var testResults = new List<ITestResult>();

            dynamic actualTreeOutputs = treeVariablesObject.outputs;
            foreach (var expectedOutput in expectedTestOutputs)
            {
                var outputValueName = Utilities.StripInvalidChars(expectedOutput.Name);
                var outputPropertyToCompare = actualTreeOutputs.GetType().GetProperty(outputValueName);
                var actualOutputValue = outputPropertyToCompare.GetValue(actualTreeOutputs).ToString();

                var expectedValue = expectedOutput.ExpectedValue.ToString();
                if (expectedValue.ToString().StartsWith("Enumerations::"))
                {
                    expectedValue = Utilities.GetEnumerationValueForRubyEnumString(expectedValue.ToString(), enumerations);
                }
                if (outputPropertyToCompare.PropertyType == typeof(Double) || outputPropertyToCompare.PropertyType == typeof(float))
                {
                    var actualOutputAsNumber = Double.Parse(actualOutputValue);
                    var expectedValueAsNumber = Double.Parse(expectedValue);
                    actualOutputValue = Math.Round(actualOutputAsNumber, 2).ToString();
                    expectedValue = Math.Round(expectedValueAsNumber, 2).ToString();
                }

                var testResult = assertionManager.AssertOutputTestPassed(expectedOutput, expectedValue, actualOutputValue, outputPropertyToCompare.PropertyType);
                testResults.Add(testResult);
            }

            return testResults;
        }

        public List<ITestResult> ProcessDecisionTreeMessageResults(ISystemMessageCollection messages, List<IOutputMessage> expectedMessages)
        {
            var testResults = new List<ITestResult>();
            foreach (var outputMessage in expectedMessages)
            {
                testResults.Add(assertionManager.AssertMessageTestPassed(messages.AllMessages.ToList(), outputMessage));
            }
            return testResults;
        }

        public List<ITestResult> ProcessDecisionTreeSubtreeResults(IDictionary<string, dynamic> calledSubtrees, List<ISubtreeExpectation> expectedSubtrees)
        {
            var results = new List<ITestResult>();
            var calledSubtreeNames = calledSubtrees.Keys.ToList();
            foreach (var expectation in expectedSubtrees)
            {
                results.Add(assertionManager.AssertSubtreeTestPassed(calledSubtreeNames, expectation));
            }

            return results;
        }
    }
}