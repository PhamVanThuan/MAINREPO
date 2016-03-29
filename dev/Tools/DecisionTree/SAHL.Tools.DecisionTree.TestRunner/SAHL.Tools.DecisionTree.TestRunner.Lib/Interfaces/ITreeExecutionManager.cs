using SAHL.Core.SystemMessages;
using SAHL.Tools.DecisionTree.TestRunner.Interfaces;

namespace SAHL.Tools.DecisionTree.TestRunner.Lib.Interfaces
{
    public interface ITreeExecutionManager
    {
        dynamic Enumerations { get; }

        void Process(string treeName, int treeVersion, System.Collections.Generic.List<ITestInput> testCaseInputs, System.Collections.Generic.List<ITestInput> scenarioInputs);

        void SetupTreeExecutionManager(string assemblyPath, ISystemMessageCollection systemMessages);

        ISystemMessageCollection SystemMessages { get; }

        dynamic TreeVariablesObject { get; }

        event DebugLocationChanged DebugLocationChanged;
        event DecisionTreeExecutionStarted DecisionTreeExecutionStarted;
        event DecisionTreeExecutionEnded DecisionTreeExecutionEnded;
    }
}