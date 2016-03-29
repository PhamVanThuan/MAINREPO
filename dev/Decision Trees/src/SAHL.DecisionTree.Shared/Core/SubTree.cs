using Newtonsoft.Json;
using System;

namespace SAHL.DecisionTree.Shared.Core
{
    public class SubTree : Node
    {
        private SubtreeExecutionStatus executionStatus;

        public string Name { get; protected set; }

        public string Version { get; protected set; }

        public dynamic subtreeParentVariablesMap { get; protected set; }

        public SubtreeExecutionStatus ExecutionStatus { get { return executionStatus; } }

        public SubTree(string subTreeName, string version, string subtreeParentVariablesMap, int id)
            : base(id, subTreeName, NodeType.SubTree, "")
        {
            this.Name = subTreeName;
            this.Version = version;
            this.subtreeParentVariablesMap = JsonConvert.DeserializeObject<dynamic>(subtreeParentVariablesMap);
            this.nodeType = NodeType.SubTree;
        }

        public event EventHandler<SubtreeExecutionStartedArgs> SubtreeExecutionStarted;

        protected virtual void OnSubtreeExecutionStarted(SubtreeExecutionStartedArgs e)
        {
            this.executionStatus = SubtreeExecutionStatus.Started;
            EventHandler<SubtreeExecutionStartedArgs> handler = SubtreeExecutionStarted;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event EventHandler<SubtreeExecutionCompletedArgs> SubtreeExecutionCompleted;

        protected virtual void OnSubtreeExecutionCompleted(SubtreeExecutionCompletedArgs e)
        {
            this.executionStatus = SubtreeExecutionStatus.Completed;
            EventHandler<SubtreeExecutionCompletedArgs> handler = SubtreeExecutionCompleted;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public void RaiseExecutionStatusEvent(SubtreeExecutionStatus executionStatus, dynamic variablesArgs, dynamic messagesArgs)
        {
            switch (executionStatus)
            {
                case SubtreeExecutionStatus.Started:
                    var inputs_array = variablesArgs as object[];
                    OnSubtreeExecutionStarted(new SubtreeExecutionStartedArgs(this.id, this.Name, inputs_array));
                    break;

                case SubtreeExecutionStatus.Completed:
                    var outputs_array = variablesArgs as object[];
                    var messages_array = messagesArgs as object[];
                    OnSubtreeExecutionCompleted(new SubtreeExecutionCompletedArgs(this.id, this.Name, outputs_array, messages_array));
                    break;

                case SubtreeExecutionStatus.Error:
                    break;

                default:
                    break;
            }
        }
    }

    public enum SubtreeExecutionStatus
    {
        Started,
        Completed,
        Error
    }

    public enum SessionDebugStatus
    {
        Debug,
        TestSuite,
        Indeterminate
    }
}