using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.DecisionTree.TestRunner.Lib.Assertions.Models
{
    public abstract class Assertion
    {
        public abstract bool Assert(object expected, object actual, Type outputType);
    }


    public class EqualsAssertion : Assertion
    {
        public override bool Assert(object expected, object actual, Type outputType)
        {
            var expectedValue = Convert.ChangeType(expected, outputType);
            return actual.Equals(expectedValue);
        }
    }
}
