using SAHL.Tools.DecisionTree.TestRunner.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAHL.Tools.DecisionTree.Coverage.Lib
{
    public class CoverageTree
    {
        public Dictionary<int, Node> Nodes { get; set; }
        public List<Link> Links { get; set; }
        public string TreeName { get; set; }
        public List<int> NodeIDs { get; set; }
        public List<int> LinkIDs { get; set; }

        public CoverageTree(string treeName, Dictionary<int, Node> nodes, List<Link> links)
        {
            this.TreeName = treeName;
            this.Nodes = nodes;
            this.Links = links;

            this.NodeIDs = new List<int>();
            this.LinkIDs = new List<int>();
        }

        internal void UpdateCoverage(int? previousNodeId, int? justExecutedNodeId, bool nodeResult)
        {
            if (justExecutedNodeId.HasValue)
            {
                NodeIDs.Add(justExecutedNodeId.Value);
            }
            var possibleLinks = Links.Where(x => x.ToNodeID == justExecutedNodeId && x.FromNodeID == previousNodeId);
            if (possibleLinks.Count() == 1)
            {
                this.LinkIDs.Add(possibleLinks.First().ID);
            }
            else
            {
                var previousNode = Nodes.First(x => x.Key == previousNodeId);
                if (previousNode.Value.nodeType == "Decision")
                {
                    if (nodeResult == true)
                    {
                        var yesLink = possibleLinks.First(x => x.LinkType == "DecisionYes");
                        this.LinkIDs.Add(yesLink.ID);
                    }
                    else
                    {
                        var noLink = possibleLinks.First(x => x.LinkType == "DecisionNo");
                        this.LinkIDs.Add(noLink.ID);
                    }
                }
            }
        }

        
        public string GetCoverageResult()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(String.Format("{0} of {1} nodes executed.", NodeIDs.Distinct().ToList().Count, Nodes.Count));
            var missedNodes = Nodes.Where(x => !NodeIDs.Contains(x.Key));
            foreach (var node in missedNodes)
            {
                if (node.Key != missedNodes.First().Key) { sb.Append(", "); }
                sb.Append(node.Value.Name);
            }
            return sb.ToString();
        }
    }
}