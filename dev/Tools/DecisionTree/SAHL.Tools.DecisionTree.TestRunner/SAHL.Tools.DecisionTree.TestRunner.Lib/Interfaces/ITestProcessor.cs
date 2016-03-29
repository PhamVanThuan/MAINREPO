using System.Collections.Generic;
using System.Reflection;
using SAHL.Core.SystemMessages;
using SAHL.Tools.DecisionTree.TestRunner.Interfaces;
using SAHL.Tools.DecisionTree.TestRunner.Lib.TestResult;

namespace SAHL.Tools.DecisionTree.TestRunner.Lib.Interfaces
{
    public interface ITestProcessor
    {
        List<ITestResult> ProcessDecisionTreeMessageResults(ISystemMessageCollection messages, List<IOutputMessage> expectedMessages);

        List<ITestResult> ProcessDecisionTreeOutputResults(dynamic treeVariablesObject, List<ITestOutput> expectedTestOutputs, dynamic enumerations);

        List<ITestResult> ProcessDecisionTreeSubtreeResults(IDictionary<string, dynamic> calledSubtrees, List<ISubtreeExpectation> expectedSubtrees);
    }
}