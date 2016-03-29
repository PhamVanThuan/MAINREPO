using SAHL.Tools.DecisionTree.TestRunner.Interfaces;

namespace SAHL.Tools.DecisionTree.TestRunner.Lib.Interfaces
{
    public interface ITestRunner
    {
        event ScenarioFinishedEventHandler ScenarioFinished;
        event ScenarioStartedEventHandler ScenarioStarted;
        event TestSuiteFinishedEventHandler TestSuiteFinished;
        event TestSuiteStartedEventHandler TestSuiteStarted;

        void RunAllTestSuitesInAssembly(string treeAssemblyPath, string testAssemblyPath);
        void RunTestCase(ITestCase testCase);
        void RunTestsForDecisionTree(string treeName, int treeVersion, string treeAssemblyPath, string testAssemblyPath);
        void RunTestSuite(ITestSuite testSuite);
    }
}