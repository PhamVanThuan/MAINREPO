using Machine.Fakes;
using Machine.Specifications;
using SAHL.Tools.DecisionTree.Coverage.Lib;
using SAHL.Tools.DecisionTree.Coverage.Specs.TestFakes;
using SAHL.Tools.DecisionTree.TestRunner.Interfaces;
using System.Collections.Generic;

namespace SAHL.Tools.DecisionTree.Coverage.Specs
{
    public class when_two_trees_are_processed : WithFakes
    {
        private static CoverageMonitor monitor;
        private static FakeTreeExecutionManager fakeTreeExecutionManager;
        private static string firstTreeName;
        private static string secondTreeName;
        private static int treeVersion;

        private static List<int> expectedLinkIDs;
        private static List<int> expectedNodeIDs;

        private Establish context = () =>
        {
            monitor = new CoverageMonitor();

            fakeTreeExecutionManager = new FakeTreeExecutionManager();
            fakeTreeExecutionManager.SetRunPath(1);

            firstTreeName = "Maple";
            secondTreeName = "Pine";
            treeVersion = 1;

            expectedLinkIDs = new MapleTree().Paths[1];
            expectedNodeIDs = new List<int>
            {
                1, 2, 3, 5
            };

            monitor.BindToTreeExecutionManager(fakeTreeExecutionManager);
            fakeTreeExecutionManager.Process(firstTreeName, treeVersion, new List<ITestInput>(), new List<ITestInput>());
        };

        private Because of = () =>
        {
            fakeTreeExecutionManager.Process(secondTreeName, treeVersion, new List<ITestInput>(), new List<ITestInput>());
        };

        private It should_create_a_coverage_tree_for_each_tree = () =>
        {
            monitor.CoverageTrees.Count.ShouldEqual(2);
            monitor.CoverageTrees[firstTreeName].ShouldNotBeNull();
            monitor.CoverageTrees[secondTreeName].ShouldNotBeNull();
        };
    }
}