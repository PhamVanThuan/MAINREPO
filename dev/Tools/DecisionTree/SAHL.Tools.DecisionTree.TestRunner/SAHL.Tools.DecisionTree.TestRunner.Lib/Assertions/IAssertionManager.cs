using System;
using System.Collections.Generic;
using SAHL.Core.SystemMessages;
using SAHL.Tools.DecisionTree.TestRunner.Interfaces;
using SAHL.Tools.DecisionTree.TestRunner.Lib.TestResult;

namespace SAHL.Tools.DecisionTree.TestRunner.Lib.Assertions
{
    public interface IAssertionManager
    {
        TestMessageResult AssertMessageTestPassed(List<ISystemMessage> messages, IOutputMessage outputMessage);

        TestOutputResult AssertOutputTestPassed(ITestOutput testOutput, object expected, object actual, Type outputType);

        SubtreeExpectationResult AssertSubtreeTestPassed(List<string> calledSubtrees, ISubtreeExpectation expectation);
    }
}