namespace SAHL.Tools.DecisionTree.TestRunner.Interfaces
{
    public interface ISubtreeExpectation
    {
        string Assertion { get; set; }

        string SubtreeName { get; set; }
    }
}