namespace SAHL.Tools.DecisionTreeTestFromJsonGenerator.Lib.Models
{
    public class OutputVariable
    {
        public string Name { get; set; }

        public string ExpectedValue { get; set; }

        public string Assertion { get; set; }

        public string Type { get; set; }

        public OutputVariable(string name, string assertion, string type, string expectedValue)
        {
            this.Name = name;
            this.Assertion = assertion;
            this.Type = type;
            this.ExpectedValue = expectedValue;
        }
    }
}