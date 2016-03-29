using System.Collections.Generic;
using SAHL.Tools.DecisionTree.TestRunner.Interfaces;

namespace SAHL.Tools.DecisionTree.TestRunner.Lib.Interfaces
{
    public interface ITestEngine
    {
        ScenarioResult ExecuteScenario(IScenario scenario, List<ITestInput> testCaseInputs, string treeAssemblyPath);
        void SetupTestSuite(string treeName, int messagesVersion);
    }
}