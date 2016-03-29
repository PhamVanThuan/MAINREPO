using System;
using System.Collections.Generic;
using System.Linq;
using SAHL.Core.SystemMessages;
using SAHL.Tools.DecisionTree.TestRunner.Interfaces;
using SAHL.Tools.DecisionTree.TestRunner.Lib.TestResult;
using SAHL.Tools.DecisionTree.TestRunner.Lib.Interfaces;

namespace SAHL.Tools.DecisionTree.TestRunner.Lib
{
    public class TestEngine : ITestEngine
    {
        private string treeName;
        private int treeVersion;

        private ITestProcessor testProcessor;
        private ITreeExecutionManager treeExecutionManager;

        public TestEngine(ITestProcessor testProcessor, ITreeExecutionManager treeExecutionManager)
        {
            ISystemMessageCollection systemMessageCollection = SystemMessageCollection.Empty();
            this.treeExecutionManager = treeExecutionManager;
            this.testProcessor = testProcessor;
        }

        public ScenarioResult ExecuteScenario(IScenario scenario, List<ITestInput> testCaseInputs, string treeAssemblyPath)
        {
            var result = new ScenarioResult(scenario.Name, string.Format("{0} v{1}", treeName, treeVersion));
            try
            {
                VerifyTestCaseInputs(testCaseInputs);
                ISystemMessageCollection systemMessageCollection = SystemMessageCollection.Empty();

                treeExecutionManager.SetupTreeExecutionManager(treeAssemblyPath, systemMessageCollection);
                treeExecutionManager.Process(treeName, treeVersion, testCaseInputs, scenario.ScenarioInputs.ToList());

                var scenarioVariables = treeExecutionManager.TreeVariablesObject;
                var enumerations = treeExecutionManager.Enumerations;

                result.Results = testProcessor.ProcessDecisionTreeOutputResults(scenarioVariables, scenario.ExpectedOutputs.ToList(), enumerations);
                result.Results.AddRange(testProcessor.ProcessDecisionTreeMessageResults(systemMessageCollection, scenario.ExpectedMessages.ToList()));

                if (((IDictionary<string, dynamic>)scenarioVariables).ContainsKey("subtrees"))
                {
                    var calledSubtrees = (IDictionary<string, dynamic>)scenarioVariables.subtrees;
                    result.Results.AddRange(testProcessor.ProcessDecisionTreeSubtreeResults(calledSubtrees, scenario.SubtreeExpectations.ToList()));
                }
            }
            catch (Exception ex)
            {
                foreach (var expectedOutput in scenario.ExpectedOutputs)
                {
                    var outputResult = new TestOutputResult(expectedOutput, expectedOutput.ExpectedValue, "", false);
                    outputResult.TestException = ex;

                    result.Results.Add(outputResult);
                }
                foreach (var expectedSubtree in scenario.SubtreeExpectations)
                {
                    var subtreeResult = new SubtreeExpectationResult(expectedSubtree.SubtreeName, expectedSubtree.Assertion, new List<string>(), false);
                    subtreeResult.TestException = ex;

                    result.Results.Add(subtreeResult);
                }
                foreach (var expectedMessage in scenario.ExpectedMessages)
                {
                    var messageResult = new TestMessageResult(expectedMessage, new List<ISystemMessage>(), false);
                    messageResult.TestException = ex;

                    result.Results.Add(messageResult);
                }
            }

            return result;
        }

        private void VerifyTestCaseInputs(List<ITestInput> testCaseInputs)
        {
            foreach (var input in testCaseInputs)
            {
                if (String.IsNullOrEmpty(input.Value))
                {
                    throw new ArgumentException(String.Format("Input value \"{0}\" is not set.", input.Name));
                }
            }
        }
        public void SetupTestSuite(string treeName, int treeVersion)
        {
            this.treeName = treeName;
            this.treeVersion = treeVersion;
        }
    }
}
