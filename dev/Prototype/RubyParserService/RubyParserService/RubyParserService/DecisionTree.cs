using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.DecisionTree.Shared;
using Microsoft.Scripting.Hosting;
using SAHL.Core.SystemMessages;

namespace RubyParserService
{
    public class DecisionTree
    {
        List<Link> NodeLinks;

        Dictionary<int, Node> nodes;

        private int currentNodeId;
        private bool currentResult;
        private ScriptScope scope;
        //private SystemMessageCollection systemMessages;

        public DecisionTree(Dictionary<int, Node> nodes, List<Link> links)
        {
            this.nodes = nodes;
            this.NodeLinks = links;
            //this.currentNodeId = currentNodeId;
            //this.scope = scope;
            this.currentResult = true;
            //this.systemMessages = systemMessages;
        }

        private Node GetNextNode()
        {
            Node nextNode = null;
            var switchOnNodeType = nodes[currentNodeId].nodeType;
            switch (switchOnNodeType)
            {
                case NodeType.Start:
                case NodeType.Process:
                    {
                        var link = NodeLinks.SingleOrDefault(nl => nl.FromNodeID == currentNodeId);
                        nextNode = nodes[link.ToNodeID];
                        break;
                    }
                case NodeType.Subtree:
                    {
                        // ToDo
                        var link = NodeLinks.SingleOrDefault(nl => nl.FromNodeID == currentNodeId);
                        nextNode = (SubTree)nodes[link.ToNodeID];
                        var subtree = GetDecisionTree(nextNode.Name, nextNode.Version);
                        break;
                    }
                case NodeType.Decision:
                    {
                        if (currentResult)
                        {
                            var link = NodeLinks.SingleOrDefault(nl => nl.FromNodeID == currentNodeId && nl.Type == LinkType.DecisionYes);
                            nextNode = nodes[link.ToNodeID];
                        }
                        else
                        {
                            var link = NodeLinks.SingleOrDefault(nl => nl.FromNodeID == currentNodeId && nl.Type == LinkType.DecisionNo);
                            nextNode = nodes[link.ToNodeID];
                            currentResult = true;
                        }
                        break;
                    }
                case NodeType.Stop:
                    {
                        currentResult = false;
                        break;
                    }
                default:
                    {
                        // ToDo
                        // Add error message to message collection
                        var systemMessages = this.scope.Engine.Runtime.Globals.GetVariable("Messages");
                        systemMessages.AddMessage(new SystemMessage("Node type is unknown on this tree", SystemMessageSeverityEnum.Error));
                        currentResult = false;
                        break;
                    }
            }
            if (nextNode != null)
            {
                nextNode.scope = scope;
            }
            currentNodeId = nextNode.id;
            return nextNode;
        }

        public void Step(ScriptScope runningScope, int startNodeId)
        {
            this.scope = runningScope;
            this.currentNodeId = startNodeId;
            Node node = GetNextNode();
            while (currentResult && !node.nodeType.Equals(NodeType.Stop))
            {
                currentResult = node.Process();
                node = GetNextNode();
            } 
        }
    }
}
