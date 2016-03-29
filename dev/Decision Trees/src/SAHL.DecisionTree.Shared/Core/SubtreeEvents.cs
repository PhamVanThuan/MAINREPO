using System;

namespace SAHL.DecisionTree.Shared.Core
{
    public class SubtreeExecutionStatusArgs : EventArgs
    {
        public SubtreeExecutionStatus ExecutionStatus { get; protected set; }

        public SubtreeExecutionStatusArgs(SubtreeExecutionStatus executionStatus)
        {
            this.ExecutionStatus = executionStatus;
        }
    }

    public class SubtreeExecutionStartedArgs : SubtreeExecutionStatusArgs
    {
        private readonly int subtreeId;
        private readonly object[] inputs_array;
        private readonly string subtreeName;

        public SubtreeExecutionStartedArgs(int subtreeId, string subtreeName, object[] inputs_array)
            : base(SubtreeExecutionStatus.Started)
        {
            this.subtreeId = subtreeId;
            this.inputs_array = inputs_array;
            this.subtreeName = subtreeName;
        }

        public int SubtreeId
        {
            get { return subtreeId; }
        }

        public string SubtreeName
        {
            get { return subtreeName; }
        }

        public object[] Inputs_array
        {
            get { return inputs_array; }
        }
    }

    public class SubtreeExecutionCompletedArgs : SubtreeExecutionStatusArgs
    {
        private readonly int subtreeId;
        private readonly object[] outputs_array;
        private readonly object[] messages_array;
        private readonly string subtreeName;

        public SubtreeExecutionCompletedArgs(int subtreeId, string subtreeName, object[] outputs_array, object[] messages)
            : base(SubtreeExecutionStatus.Completed)
        {
            this.subtreeId = subtreeId;
            this.outputs_array = outputs_array;
            this.messages_array = messages;
            this.subtreeName = subtreeName;
        }

        public int SubtreeId
        {
            get { return subtreeId; }
        }

        public string SubtreeName
        {
            get { return subtreeName; }
        }

        public object[] Outputs_array
        {
            get { return outputs_array; }
        }

        public object[] Messages
        {
            get { return messages_array; }
        }
    }

    public delegate void SubtreeExecutionStartedHandler(object sender, SubtreeExecutionStartedArgs e);

    public delegate void SubtreeExecutionCompletedHandler(object sender, SubtreeExecutionCompletedArgs e);
}