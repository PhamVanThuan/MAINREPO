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
    public class when_routing_from_a_process_node : WithFakes
    {
        private static RoutingEngine routingEngine;
        private static List<Link> treeLinks;
        private static Dictionary<int, Node> treeNodes;
        private static Node nextNode;
        private static Node processNode;

        private Establish context = () =>
        {
            treeNodes = new Dictionary<int, Node>();
            treeLinks = new List<Link>();

            processNode = new Node(1, "processNode", NodeType.Process, "");
            treeNodes.Add(1, processNode);
            Node processYesNode = new Node(2, "processYesNode", NodeType.Process, "");
            treeNodes.Add(2, processYesNode);

            Link link = new Link(1, 1, 2, LinkType.Standard);
            treeLinks.Add(link);            

            routingEngine = new RoutingEngine(treeLinks, treeNodes);

            routingEngine.CurrentNode = processNode;
        };

        private Because of = () =>
        {
            processNode.ExecutionResult = true;
            nextNode = routingEngine.MoveNext();
        };

        private It should_move_to_the_next_node_after_execution = () =>
        {
            nextNode.id.ShouldEqual(2);
        };
    }
}
