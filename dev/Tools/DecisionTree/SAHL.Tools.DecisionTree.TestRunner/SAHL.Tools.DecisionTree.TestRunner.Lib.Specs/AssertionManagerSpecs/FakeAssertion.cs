using System;
using SAHL.Tools.DecisionTree.TestRunner.Lib.Assertions.Models;

namespace SAHL.Tools.DecisionTree.TestRunner.Lib.Specs.AssertionManagerSpecs
{
    public class FakeAssertion : Assertion
    {
        public override bool Assert(object expected, object actual, Type outputType)
        {
            return true;
        }
    }
}