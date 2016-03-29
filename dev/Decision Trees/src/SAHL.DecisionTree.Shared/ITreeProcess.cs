using Microsoft.Scripting.Hosting;
using SAHL.Core.SystemMessages;
using System.Collections.Generic;

namespace SAHL.DecisionTree.Shared
{
    public interface ITreeProcess
    {
        Dictionary<int, Node> Nodes { get; }
        List<Link> NodeLinks { get; }
        QueryGlobalsVersion GlobalsVersion { get; }

        int CurrentNodeId { get; set; }
        bool CurrentResult { get; set; }
        ScriptScope Scope { get; set; }
        bool NodeExecutionResultedInError { get; set; }
        bool ExecutionCompleted { get; set; }
        dynamic VariablesCollection { get; set; }
        ISystemMessageCollection SystemMessages { get; set; }

        Dictionary<string, ISystemMessageCollection> SubtreeMessagesDictionary { get; set; }
        List<string> SubtreeMessagesToClear { get; set; }

        void Run(ScriptScope runningScope, int startNodeId);
        Node Step(ScriptScope runningScope, int nodeToRunId);
    }
}