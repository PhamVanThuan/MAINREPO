using Machine.Fakes;
using Machine.Specifications;
using SAHL.Tools.DecisionTree.Coverage.Lib;
using SAHL.Tools.DecisionTree.Coverage.Specs.TestFakes;
using SAHL.Tools.DecisionTree.TestRunner.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Tools.DecisionTree.Coverage.Specs
{
    public class when_the_debug_location_of_a_tree_changes : WithFakes
    {
        private static CoverageMonitor monitor;
        private static FakeTreeExecutionManager fakeTreeExecutionManager;
        private static MapleTree testTree;

        private Establish context = () =>
        {
            monitor = new CoverageMonitor();

            fakeTreeExecutionManager = new FakeTreeExecutionManager();
            fakeTreeExecutionManager.SetRunPath(1);

            testTree = new MapleTree();

            monitor.BindToTreeExecutionManager(fakeTreeExecutionManager);
            fakeTreeExecutionManager.OnDecisionTreeStarted(testTree.TreeName, testTree.TreeNodes, testTree.TreeLinks);
        };

        private Because of = () =>
        {
            fakeTreeExecutionManager.OnDebugLocationChanged(1, 0, true, true);
        };

        private It should_add_the_node_just_executed_to_the_coverage_tree = () =>
        {
            monitor.CoverageTrees.First().Value.NodeIDs[0].ShouldEqual(1);
        };

        private It should_add_the_link_ID_to_the_coverage_tree = () =>
        {
            monitor.CoverageTrees.First().Value.LinkIDs.Count.ShouldEqual(1);
        };
    }
}