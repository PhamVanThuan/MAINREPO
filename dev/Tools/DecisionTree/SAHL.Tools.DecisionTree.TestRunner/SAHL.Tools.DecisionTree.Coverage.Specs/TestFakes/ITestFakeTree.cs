using System.Collections.Generic;
using SAHL.Tools.DecisionTree.TestRunner.Lib;

namespace SAHL.Tools.DecisionTree.Coverage.Specs.TestFakes
{
    interface ITestFakeTree
    {
        Dictionary<int, List<int>> Paths { get; set; }
        List<Link> TreeLinks { get; set; }
        string TreeName { get; set; }
        Dictionary<int, Node> TreeNodes { get; set; }
    }
}