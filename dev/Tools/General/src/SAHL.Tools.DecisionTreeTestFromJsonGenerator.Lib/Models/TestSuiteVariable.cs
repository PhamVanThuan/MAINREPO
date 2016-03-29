namespace SAHL.Tools.DecisionTreeTestFromJsonGenerator.Lib.Models
{
    public class TestSuiteVariable
    {
        public int ID { get; set; }
        public string Value { get; set; }

        public TestSuiteVariable(int id, string value)
        {
            this.ID = id;
            this.Value = value;
        }
    }
}
