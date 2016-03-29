using System.Collections.Generic;

namespace SAHL.Tools.DecisionTree.TestRunner.Interfaces
{
    public interface IScenario
    {
        string Name { get; set; }

        IEnumerable<ITestInput> ScenarioInputs { get; set; }

        IEnumerable<IOutputMessage> ExpectedMessages { get; set; }

        IEnumerable<ITestOutput> ExpectedOutputs { get; set; }

        IEnumerable<ISubtreeExpectation> SubtreeExpectations { get; set; }
    }
}