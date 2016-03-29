using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.DecisionTree.TestRunner.Lib.Assertions.Models
{
    public abstract class CollectionAssertion
    {
        public abstract bool Assert<T>(IEnumerable<T> actual, T expected) where T : class;
    }

    public class CollectionNotContainsAssertion : CollectionAssertion
    {
        public override bool Assert<T>(IEnumerable<T> actual, T expected)
        {
            return !actual.Contains(expected);
        }
    }

    public class CollectionContainsAssertion : CollectionAssertion
    {
        public override bool Assert<T>(IEnumerable<T> actual, T expected)
        {
            return actual.Contains(expected);
        }
    }

    public class CollectionEmptyAssertion : CollectionAssertion
    {
        public override bool Assert<T>(IEnumerable<T> actual, T expected)
        {
            return actual.Count() == 0;
        }
    }
}
