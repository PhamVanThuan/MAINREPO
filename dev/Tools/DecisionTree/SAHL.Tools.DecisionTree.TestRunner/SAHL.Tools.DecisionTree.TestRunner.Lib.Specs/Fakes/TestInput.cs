using SAHL.Tools.DecisionTree.TestRunner.Interfaces;

namespace SAHL.Tools.DecisionTree.TestRunner.Lib.Specs.Fakes
{
    public class TestInput : ITestInput
    {
        public string Name { get; set; }
        public string Type { get;set; }
        public string Value { get;set; }

        public TestInput(string name, string type, string value)
        {
            this.Name = name;
            this.Type = type;
            this.Value = value;
        }
    }
}
