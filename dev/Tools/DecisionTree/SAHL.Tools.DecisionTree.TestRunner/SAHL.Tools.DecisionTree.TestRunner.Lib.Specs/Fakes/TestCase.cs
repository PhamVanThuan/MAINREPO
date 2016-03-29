using SAHL.Tools.DecisionTree.TestRunner.Interfaces;
using System.Collections.Generic;

namespace SAHL.Tools.DecisionTree.TestRunner.Lib.Specs.Fakes
{
    public class TestCase : ITestCase
    {
        public TestCase(string name, List<ITestInput> inputs, List<IScenario> scenarios)
        {
            this.Name = name;
            this.TestCaseInputs = inputs;
            this.TestCaseScenarios = scenarios;
        }

        public string Name { get; set; }

        public IEnumerable<ITestInput> TestCaseInputs
        {
            get;
            set;
        }

        public IEnumerable<IScenario> TestCaseScenarios
        {
            get;
            set;
        }
    }
}