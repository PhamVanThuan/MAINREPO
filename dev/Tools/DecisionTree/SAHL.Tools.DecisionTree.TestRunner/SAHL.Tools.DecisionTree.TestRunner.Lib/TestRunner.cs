using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SAHL.Tools.DecisionTree.TestRunner.Interfaces;
using SAHL.Tools.DecisionTree.TestRunner.Lib.Interfaces;

namespace SAHL.Tools.DecisionTree.TestRunner.Lib
{
    public class TestRunner : ITestRunner
    {
        private ITestEngine testEngine;
        private Assembly testAssembly;
        private string treeAssemblyPath;
        private string testAssemblyPath;

        public TestRunner(ITestEngine testEngine)
        {
            this.testEngine = testEngine;
        }

        public void RunTestsForDecisionTree(string treeName, int treeVersion, string treeAssemblyPath, string testAssemblyPath)
        {
            this.treeAssemblyPath = treeAssemblyPath;
            this.testAssemblyPath = testAssemblyPath;
            this.testAssembly = Assembly.LoadFile(testAssemblyPath);

            var testSuiteName = String.Format("{0}_{1}TestSuite", treeName, treeVersion);
            var testSuiteType = testAssembly.GetTypes().First(x => x.IsClass && x.GetInterface("ITestSuite") != null && x.Name == testSuiteName);
            ITestSuite testSuite = (ITestSuite)Activator.CreateInstance(testSuiteType);
            RunTestSuite(testSuite);
        }

        public void RunAllTestSuitesInAssembly(string treeAssemblyPath, string testAssemblyPath)
        {
            this.treeAssemblyPath = treeAssemblyPath;
            this.testAssemblyPath = testAssemblyPath;
            this.testAssembly = Assembly.LoadFile(testAssemblyPath);

            var testSuiteTypes = testAssembly.GetTypes().Where(x => x.IsClass && x.GetInterface("ITestSuite") != null).ToList();

            var testSuites = new List<ITestSuite>();

            foreach (var testSuiteType in testSuiteTypes)
            {
                var testSuite = Activator.CreateInstance(testSuiteType);
                testSuites.Add((ITestSuite)testSuite);
            }

            foreach (var testSuite in testSuites)
            {
                RunTestSuite(testSuite);
            }
        }

        public void RunTestSuite(ITestSuite testSuite)
        {
            var treeNameParts = testSuite.GetType().Name.Split('_');
            var treeName = treeNameParts.First();
            int treeVersion = Int32.Parse(treeNameParts.Last().Replace("TestSuite", String.Empty));
            var formattedTreeName = String.Format("{0} v{1}", treeName, treeVersion);
            testEngine.SetupTestSuite(treeName, treeVersion);
           
            OnTestSuiteStarted(formattedTreeName);

            foreach (var testCase in testSuite.TestCases)
            {
                RunTestCase(testCase);
            }

            OnTestSuiteFinished(formattedTreeName);
        }

        public void RunTestCase(ITestCase testCase)
        {
            OnTestSuiteStarted(testCase.Name);

            foreach (var scenario in testCase.TestCaseScenarios)
            {
                OnScenarioStarted(scenario.Name);

                var scenarioResult = testEngine.ExecuteScenario(scenario, testCase.TestCaseInputs.ToList(), treeAssemblyPath);

               OnScenarioFinished(scenarioResult);
            }

            OnTestSuiteFinished(testCase.Name);
        }

        public event ScenarioStartedEventHandler ScenarioStarted;
        public event ScenarioFinishedEventHandler ScenarioFinished;
        public event TestSuiteFinishedEventHandler TestSuiteFinished;
        public event TestSuiteStartedEventHandler TestSuiteStarted;

        private void OnScenarioStarted(string scenarioName)
        {
            if (ScenarioStarted != null)
            {
                ScenarioStarted(this, scenarioName);
            }
        }
        private void OnScenarioFinished(ScenarioResult scenarioResult)
        {
            if (ScenarioFinished != null)
            {
                ScenarioFinished(this, scenarioResult);
            }
        }
        private void OnTestSuiteStarted(string testSuiteName)
        {
            if (TestSuiteStarted != null)
            {
                TestSuiteStarted(this, testSuiteName);
            }
        }
        private void OnTestSuiteFinished(string testSuiteName)
        {
            if (TestSuiteFinished != null)
            {
                TestSuiteFinished(this, testSuiteName);
            }
        }
    }
}
