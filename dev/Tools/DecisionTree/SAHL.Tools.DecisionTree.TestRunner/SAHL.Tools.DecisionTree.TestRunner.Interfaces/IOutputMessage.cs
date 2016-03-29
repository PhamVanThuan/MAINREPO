namespace SAHL.Tools.DecisionTree.TestRunner.Interfaces
{
    public interface IOutputMessage
    {
        string Assertion { get; set; }

        string ExpectedMessage { get; set; }

        string ExpectedMessageSeverity { get; set; }
    }
}