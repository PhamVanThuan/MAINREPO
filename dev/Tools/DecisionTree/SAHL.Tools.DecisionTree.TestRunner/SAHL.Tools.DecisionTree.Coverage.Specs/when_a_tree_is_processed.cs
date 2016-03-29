using Machine.Fakes;
using Machine.Specifications;
using SAHL.Tools.DecisionTree.Coverage.Lib;
using SAHL.Tools.DecisionTree.Coverage.Specs.TestFakes;
using SAHL.Tools.DecisionTree.TestRunner.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Tools.DecisionTree.Coverage.Specs
{
    public class when_a_tree_is_processed : WithFakes
    {
        private static CoverageMonitor monitor;
        private static FakeTreeExecutionManager fakeTreeExecutionManager;
        private static string treeName;
        private static int treeVersion;

        private static List<int> expectedLinkIDs;
        private static List<int> expectedNodeIDs;

        private Establish context = () =>
        {
            monitor = new CoverageMonitor();

            fakeTreeExecutionManager = new FakeTreeExecutionManager();
            fakeTreeExecutionManager.SetRunPath(1);

            treeName = "Maple";
            treeVersion = 1;

            expectedLinkIDs = new MapleTree().Paths[1];
            expectedNodeIDs = new List<int>
            {
                1, 2, 3, 5
            };

            monitor.BindToTreeExecutionManager(fakeTreeExecutionManager);
        };

        private Because of = () =>
        {
            fakeTreeExecutionManager.Process(treeName, treeVersion, new List<ITestInput>(), new List<ITestInput>());
        };

        private It should_create_the_coverage_tree = () =>
        {
            monitor.CoverageTrees.First().Key.ShouldEqual(treeName);
        };

        private It should_contain_a_list_of_all_the_links_passed_through = () =>
        {
            monitor.CoverageTrees.First().Value.LinkIDs.ShouldContain(expectedLinkIDs.ToArray());
        };

        private It should_contain_a_list_of_all_the_nodes_passed_through = () =>
        {
            monitor.CoverageTrees.First().Value.NodeIDs.ShouldContain(expectedNodeIDs.ToArray());
        };
    }
}