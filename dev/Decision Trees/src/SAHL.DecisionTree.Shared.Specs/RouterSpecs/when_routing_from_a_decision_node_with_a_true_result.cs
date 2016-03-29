using Machine.Fakes;
using Machine.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Tools.ObjectFromJsonGenerator.Lib;
using SAHL.DecisionTree.Shared.Interfaces;
using SAHL.DecisionTree.Shared.Helpers;
using SAHL.DecisionTree.Shared.Core;

namespace SAHL.DecisionTree.Shared.Specs
{
    public class when_routing_from_a_decision_node_with_a_false_result : WithFakes
    {
        private static RoutingEngine routingEngine;
        private static List<Link> treeLinks;
        private static Dictionary<int, Node> treeNodes;
        private static Node nextNode;
        private static Node decisionNode;

        private Establish context = () =>
        {
            treeNodes = new Dictionary<int, Node>();
            treeLinks = new List<Link>();

            decisionNode =  new Node(1,"decisionNode", NodeType.Decision, "");            
            treeNodes.Add(1, decisionNode);
            Node processYesNode = new Node(2,"processYesNode", NodeType.Process, "");
            treeNodes.Add(2, processYesNode);
            Node processNoNode = new Node(3,"processNoNode", NodeType.Process, "");
            treeNodes.Add(3, processNoNode);

            Link yesLink = new Link(1, 1, 2, LinkType.DecisionYes);
            Link noLink = new Link(1, 1, 3, LinkType.DecisionNo);
            treeLinks.Add(yesLink);
            treeLinks.Add(noLink);

            routingEngine = new RoutingEngine(treeLinks, treeNodes);

            routingEngine.CurrentNode = decisionNode;
        };

        private Because of = () =>
        {
            decisionNode.ExecutionResult = false; 
            nextNode = routingEngine.MoveNext();
        };

        private It should_move_to_the_false_node_after_a_false_decision = () =>
        {
            nextNode.id.ShouldEqual(3);                    
        };
    }
}
