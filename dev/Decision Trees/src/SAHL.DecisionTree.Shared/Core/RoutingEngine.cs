using SAHL.DecisionTree.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.DecisionTree.Shared.Core
{
    public class RoutingEngine : IRoutingEngine
    {
        public Node CurrentNode {get; set;}
        public List<Link> TreeLinks {get; set;}
        public Dictionary<int, Node> TreeNodes {get; set;}
      
        public RoutingEngine(List<Link> treeLinks, Dictionary<int, Node> treeNodes)
        {            
            this.TreeLinks = treeLinks;
            this.TreeNodes = treeNodes;
        }

        public Node MoveNext()
        {
            Node nextNode = null;

            switch (CurrentNode.nodeType)
            {
                case NodeType.Start:
                case NodeType.Process:
                case NodeType.SubTree:
                case NodeType.ClearMessages:
                    {
                        var link = TreeLinks.SingleOrDefault(nl => nl.FromNodeID == CurrentNode.id);
                        nextNode = TreeNodes[link.ToNodeID];
                        break;
                    }
                case NodeType.Decision:
                    {
                        if (CurrentNode.ExecutionResult == true)
                        {
                            var link = TreeLinks.SingleOrDefault(nl => nl.FromNodeID == CurrentNode.id && nl.Type == LinkType.DecisionYes);
                            nextNode = TreeNodes[link.ToNodeID];
                        }
                        else
                        {
                            var link = TreeLinks.SingleOrDefault(nl => nl.FromNodeID == CurrentNode.id && nl.Type == LinkType.DecisionNo);
                            nextNode = TreeNodes[link.ToNodeID];
                        }
                        break;
                    }                           
            }
            return nextNode;
        }
    }
}
