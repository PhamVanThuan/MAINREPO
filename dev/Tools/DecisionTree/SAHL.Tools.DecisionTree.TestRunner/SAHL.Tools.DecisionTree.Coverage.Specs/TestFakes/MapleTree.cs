using SAHL.Tools.DecisionTree.TestRunner.Lib;
using System.Collections.Generic;

namespace SAHL.Tools.DecisionTree.Coverage.Specs.TestFakes
{
    public class MapleTree : ITestFakeTree
    {
        public string TreeName { get; set; }

        public Dictionary<int, Node> TreeNodes { get; set; }

        public List<Link> TreeLinks { get; set; }

        public Dictionary<int, List<int>> Paths { get; set; }

        public MapleTree()
        {
            this.TreeName = "Maple";
            this.TreeNodes = new Dictionary<int, Node>
            {
                {0, new Node(0,"Start", "Start" ) },
                {1, new Node(1, "Grow maple tree", "Process") },
                {2, new Node(2, "Is tree ready?", "Decision") },
                {3, new Node(3, "Make maple syrup", "Process") },
                {4, new Node(4, "Tree is not ready", "End") },
                {5, new Node(5, "Maple syrup made", "End") }
            };
            this.TreeLinks = new List<Link>
            {
                new Link(0, 0, 1, "Standard"),
                new Link(-1, 1, 2, "Standard"),
                new Link(-2, 2, 3, "DecisionYes"),
                new Link(-3, 2,4,"DecisionNo"),
                new Link(-4, 3, 5, "Standard")
            };
            this.Paths = new Dictionary<int, List<int>>
            {
                { 1, new List<int> { 0,-1,-2,-4 } },
                { 2, new List<int> {0,-1,-3 } }
            };
        }
    }
}