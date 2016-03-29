using SAHL.Tools.DecisionTree.TestRunner.Interfaces;

namespace SAHL.DecisionTree.TreeTests.Models
{
    public class TestOutput : ITestOutput
    {
        public TestOutput(string name, string assertion, string type, string value)
        {
            this.Name = name;
            this.Assertion = assertion;
            this.Type = type;
            this.ExpectedValue = value;
        }

        public string Assertion
        {
            get;
            set;
        }

        public string ExpectedValue
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Type
        {
            get;
            set;
        }
    }
}