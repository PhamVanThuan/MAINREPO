using System.Collections.Generic;

namespace SAHL.Tools.DecisionTree.TestRunner.Interfaces
{
    public interface ITestSuite
    {
        IEnumerable<ITestCase> TestCases { get; }
    }
}
