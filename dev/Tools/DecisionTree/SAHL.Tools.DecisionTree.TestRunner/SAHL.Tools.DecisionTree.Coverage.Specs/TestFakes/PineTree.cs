using SAHL.Tools.DecisionTree.TestRunner.Lib;
using System.Collections.Generic;

namespace SAHL.Tools.DecisionTree.Coverage.Specs.TestFakes
{
    public class PineTree : ITestFakeTree
    {
        public string TreeName { get; set; }

        public Dictionary<int, Node> TreeNodes { get; set; }

        public List<Link> TreeLinks { get; set; }

        public Dictionary<int, List<int>> Paths { get; set; }

        public PineTree()
        {
            this.TreeName = "Pine";
            this.TreeNodes = new Dictionary<int, Node>
            {
                {0, new Node(0, "Start", "Start" ) },
                {1, new Node(1, "CalculateHeight", "Process") },
                {2, new Node(2, "CalculateWidth", "Process") },
                {3, new Node(3, "Is Tree Tall Enough", "Decision") },
                {4, new Node(4, "Is Tree Wide Enough", "Decision") },
                {5, new Node(5, "Calculate Optimal Spikeyness", "Process") },
                {6, new Node(6, "Is Spikeyness Satisfactory", "Decision") },
                {7, new Node(7, "All Done - Tree Is Perfect", "End") },
                {8, new Node(8, "Log Tree", "Process") },
                {9, new Node(9, "Tree is logged", "End") },
                {10, new Node(10, "Tree is not wide enough", "End") },
                {11, new Node(11, "Tree needs to grow more", "End") }
            };
            this.TreeLinks = new List<Link>
            {
                new Link(0, 0, 1, "Standard"),
                new Link(-1, 1, 2, "Standard"),
                new Link(-2, 2, 3, "Standard"),
                new Link(-3, 3, 4, "DecisionYes"),
                new Link(-4, 4, 5, "DecisionYes"),
                new Link(-5, 5, 6, "Standard"),
                new Link(-6, 6, 7, "DecisionYes"),
                new Link(-7, 6, 8, "DecisionNo"),
                new Link(-8, 8, 9, "Standard"),
                new Link(-9, 9, 10, "DecisionNo"),
                new Link(-10, 3, 11, "DecisionNo")
            };
            this.Paths = new Dictionary<int, List<int>>
            {
                { 1, new List<int> { 0, -1, -2, -10 } },
                { 2, new List<int> { 0, -1, -2, -3, -9 } },
                { 3, new List<int> { 0, -1, -2, -3, -4, -5, -6 } },
                { 4, new List<int> { 0, -1, -2, -3, -4, -5, -7, -8 } }
            };
        }
    }
}