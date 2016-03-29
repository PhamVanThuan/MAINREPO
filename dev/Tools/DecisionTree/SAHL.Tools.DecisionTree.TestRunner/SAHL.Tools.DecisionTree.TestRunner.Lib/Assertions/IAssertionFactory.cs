using SAHL.Tools.DecisionTree.TestRunner.Lib.Assertions.Models;

namespace SAHL.Tools.DecisionTree.TestRunner.Lib.Assertions
{
    public interface IAssertionFactory
    {
        Assertion GetAssertion(string assertion);

        CollectionAssertion GetCollectionAssertion(string assertion);
    }
}