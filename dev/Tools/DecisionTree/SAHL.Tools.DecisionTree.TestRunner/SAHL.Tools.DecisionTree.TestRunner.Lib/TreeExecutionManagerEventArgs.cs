using System.Collections.Generic;

namespace SAHL.Tools.DecisionTree.TestRunner.Lib
{
    public class DebugLocationChangedArgs
    {
        private readonly int? justExecutedNodeId;
        private readonly int? previousNodeId;
        private readonly bool currentNodeResult;
        private readonly bool? previousNodeResult;

        public DebugLocationChangedArgs(int? justExecutedNodeId, int? previousNodeId, bool currentNodeResult, bool? previousNodeResult)
        {
            this.justExecutedNodeId = justExecutedNodeId;
            this.previousNodeId = previousNodeId;
            this.currentNodeResult = currentNodeResult;
            this.previousNodeResult = previousNodeResult;
        }

        public bool NodeResult
        {
            get { return currentNodeResult; }
        }

        public int? JustExecutedNodeId
        {
            get { return justExecutedNodeId; }
        }

        public int? PreviousNodeId
        {
            get { return previousNodeId; }
        }

        public bool? PreviousNodeResult
        {
            get { return previousNodeResult; }
        }
    }

    public class DecisionTreeExecutionStartedArgs
    {
        public Dictionary<int, Node> Nodes { get; private set; }

        public List<Link> Links { get; private set; }

        public string TreeName { get; private set; }

        public DecisionTreeExecutionStartedArgs(string treeName, Dictionary<int, Node> nodes, List<Link> links)
        {
            this.Nodes = nodes;
            this.Links = links;
            this.TreeName = treeName;
        }
    }

    public class DecisionTreeExecutionEndedArgs
    {
        public string TreeName { get; set; }

        public DecisionTreeExecutionEndedArgs(string treeName)
        {
            this.TreeName = treeName;
        }
    }
}