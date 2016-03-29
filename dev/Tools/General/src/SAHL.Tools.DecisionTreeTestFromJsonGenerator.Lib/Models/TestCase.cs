using System.Collections.Generic;

namespace SAHL.Tools.DecisionTreeTestFromJsonGenerator.Lib.Models
{
    public class TestCase
    {
        public string Name { get; set; }

        public List<Scenario> Scenarios { get; set; }

        public List<InputVariable> Inputs { get; set; }

        public TestCase(string name, List<Scenario> scenarios, List<InputVariable> inputs)
        {
            this.Name = name;
            this.Scenarios = scenarios;
            this.Inputs = inputs;
        }
    }
}