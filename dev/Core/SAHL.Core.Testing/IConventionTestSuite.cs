using System;
namespace SAHL.Core.Testing
{
    public interface IConventionTestSuite
    {
        void Run(string testSuiteName, string testItemName, ITestParams testParams, Action testToRun);
    }
}
