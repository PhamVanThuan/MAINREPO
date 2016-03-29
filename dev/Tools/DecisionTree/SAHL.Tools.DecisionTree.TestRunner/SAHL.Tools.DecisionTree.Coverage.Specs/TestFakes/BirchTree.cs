using SAHL.Tools.DecisionTree.TestRunner.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.DecisionTree.Coverage.Specs.TestFakes
{
    public class BirchTree : ITestFakeTree
    {
        public string TreeName { get; set; }

        public Dictionary<int, Node> TreeNodes { get; set; }

        public List<Link> TreeLinks { get; set; }

        public Dictionary<int, List<int>> Paths { get; set; }

        public BirchTree()
        {
            this.TreeName = "Birch";
            this.TreeNodes = new Dictionary<int, Node>
            {
                {0, new Node(0, "Start","Start")},
                {1, new Node(1, "Tree age > 30", "Decision")},
                {2, new Node(2, "End","End")}
            };
            this.TreeLinks = new List<Link>
            {
                new Link(0, 0, 1, "Standard"),
                new Link(-1, 1, 2, "DecisionYes"),
                new Link(-2, 1, 2, "DecisionNo"),
            };

            this.Paths = new Dictionary<int, List<int>>
            {
                {1, new List<int>{0, -1}},
                {2, new List<int>{0, -2}}
            };
        }
    }
}
