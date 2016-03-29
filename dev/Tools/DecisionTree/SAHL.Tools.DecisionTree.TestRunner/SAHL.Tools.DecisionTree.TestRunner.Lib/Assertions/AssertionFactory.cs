using System;
using SAHL.Tools.DecisionTree.TestRunner.Lib.Assertions.Models;

namespace SAHL.Tools.DecisionTree.TestRunner.Lib.Assertions
{
    public class AssertionFactory : IAssertionFactory
    {
        public Assertion GetAssertion(string assertion)
        {
            if (assertion.Equals("to equal", StringComparison.OrdinalIgnoreCase))
            {
                return new EqualsAssertion();
            }
            throw new UnknownAssertionException(assertion);
        }

        public CollectionAssertion GetCollectionAssertion(string assertion)
        {
            if (assertion.Equals("should contain", StringComparison.OrdinalIgnoreCase) || 
                assertion.Equals("should have been called", StringComparison.OrdinalIgnoreCase))
            {
                return new CollectionContainsAssertion();
            }
            else if (assertion.Equals("should not contain" , StringComparison.OrdinalIgnoreCase)|| 
                assertion.Equals("should not have been called", StringComparison.OrdinalIgnoreCase))
            {
                return new CollectionNotContainsAssertion();
            }
            else if (assertion.Equals("should be empty", StringComparison.OrdinalIgnoreCase))
            {
                return new CollectionEmptyAssertion();
            }
            throw new UnknownAssertionException(assertion);
        }
    }
}
