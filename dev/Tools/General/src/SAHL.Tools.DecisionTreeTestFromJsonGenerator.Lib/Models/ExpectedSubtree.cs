namespace SAHL.Tools.DecisionTreeTestFromJsonGenerator.Lib.Models
{
    public class ExpectedSubtree
    {
        public ExpectedSubtree(string name, string assertion)
        {
            this.Name = name;
            this.Assertion = assertion;
        }

        public string Assertion { get; private set; }
        public string Name { get; private set; }
    }
}
