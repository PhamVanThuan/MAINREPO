namespace SAHL.Tools.DecisionTreeTestFromJsonGenerator.Lib.Models
{
    public class InputVariable
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public string Value { get; set; }

        public InputVariable(string name, string type, string value)
        {
            this.Name = name;
            this.Type = type;
            this.Value = value;
        }
    }
}