using System.Collections.Generic;

namespace SAHL.Tools.DecisionTree.TestRunner.Interfaces
{
    public interface ITestCase
    {
        string Name { get; set; }
        IEnumerable<ITestInput> TestCaseInputs { get; set; }
        IEnumerable<IScenario> TestCaseScenarios { get; set; }
    }
}