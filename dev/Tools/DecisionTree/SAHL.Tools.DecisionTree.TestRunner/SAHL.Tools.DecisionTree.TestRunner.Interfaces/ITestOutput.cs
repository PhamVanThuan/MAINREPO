namespace SAHL.Tools.DecisionTree.TestRunner.Interfaces
{
    public interface ITestOutput
    {
        string Name { get; set; }

        string ExpectedValue { get; set; }

        string Type { get; set; }

        string Assertion { get; set; }
    }
}