using System.Collections.Generic;

namespace SAHL.Tools.DecisionTreeTestFromJsonGenerator.Lib.Models
{
    public class Scenario
    {
        public string Name { get; set; }

        public List<InputVariable> Inputs { get; set; }

        public List<OutputVariable> ExpectedOutputs { get; set; }

        public List<OutputMessage> ExpectedMessages { get; set; }

        public List<ExpectedSubtree> SubtreeExpectations { get; private set; }

        public Scenario(string name, List<InputVariable> inputs, List<OutputVariable> expectedOutputs, List<OutputMessage> expectedMessages, List<ExpectedSubtree> subtreeExpectations)
        {
            this.Name = name;
            this.Inputs = inputs;
            this.ExpectedOutputs = expectedOutputs;
            this.ExpectedMessages = expectedMessages;
            this.SubtreeExpectations = subtreeExpectations;
        }
    }
}