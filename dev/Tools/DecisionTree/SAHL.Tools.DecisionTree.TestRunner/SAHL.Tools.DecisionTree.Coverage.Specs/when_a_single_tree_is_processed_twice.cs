using Machine.Fakes;
using Machine.Specifications;
using SAHL.Tools.DecisionTree.Coverage.Lib;
using SAHL.Tools.DecisionTree.Coverage.Specs.TestFakes;
using SAHL.Tools.DecisionTree.TestRunner.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Tools.DecisionTree.Coverage.Specs
{
    public class when_a_single_tree_is_processed_twice : WithFakes
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

            var maple = new MapleTree();
            expectedLinkIDs = maple.Paths[1];
            expectedLinkIDs.AddRange(maple.Paths[2]);
            expectedNodeIDs = new List<int>
            {
                1, 2, 3, 5, 1, 2, 4
            };

            monitor.BindToTreeExecutionManager(fakeTreeExecutionManager);
            fakeTreeExecutionManager.Process(treeName, treeVersion, new List<ITestInput>(), new List<ITestInput>());
            fakeTreeExecutionManager.SetRunPath(2);
        };

        private Because of = () =>
        {
            fakeTreeExecutionManager.Process(treeName, treeVersion, new List<ITestInput>(), new List<ITestInput>());
        };

        private It should_only_create_one_coverage_tree = () =>
        {
            monitor.CoverageTrees.Count.ShouldEqual(1);
        };

        It should_contain_link_ids_for_both_paths = () =>
        {
            monitor.CoverageTrees.First().Value.LinkIDs.ShouldContainOnly(expectedLinkIDs.ToArray());
        };
        It should_contain_node_ids_for_both_paths = () =>
        {
            monitor.CoverageTrees.First().Value.NodeIDs.ShouldContainOnly(expectedNodeIDs.ToArray());
        };
    }
}