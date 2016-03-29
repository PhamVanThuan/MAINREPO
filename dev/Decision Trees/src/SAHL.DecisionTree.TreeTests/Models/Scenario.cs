using SAHL.Tools.DecisionTree.TestRunner.Interfaces;
using System.Collections.Generic;

namespace SAHL.DecisionTree.TreeTests.Models
{
    public class Scenario : IScenario
    {
        public Scenario(string name, List<ITestInput> inputs, List<ITestOutput> outputs, List<IOutputMessage> messages, List<ISubtreeExpectation> subtreeExpectations)
        {
            this.Name = name;
            this.ScenarioInputs = inputs;
            this.ExpectedOutputs = outputs;
            this.ExpectedMessages = messages;
            this.SubtreeExpectations = subtreeExpectations;
        }

        public string Name { get; set; }

        public IEnumerable<IOutputMessage> ExpectedMessages
        {
            get;
            set;
        }

        public IEnumerable<ITestOutput> ExpectedOutputs
        {
            get;
            set;
        }

        public IEnumerable<ITestInput> ScenarioInputs
        {
            get;
            set;
        }

        public IEnumerable<ISubtreeExpectation> SubtreeExpectations
        {
            get;
            set;
        }
    }
}