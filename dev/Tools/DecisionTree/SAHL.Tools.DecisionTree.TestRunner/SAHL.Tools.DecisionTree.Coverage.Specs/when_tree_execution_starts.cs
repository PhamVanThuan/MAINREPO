using Machine.Fakes;
using Machine.Specifications;
using SAHL.Tools.DecisionTree.Coverage.Lib;
using SAHL.Tools.DecisionTree.Coverage.Specs.TestFakes;
using System.Linq;

namespace SAHL.Tools.DecisionTree.Coverage.Specs
{
    public class when_tree_execution_starts : WithFakes
    {
        private static CoverageMonitor monitor;
        private static FakeTreeExecutionManager fakeTreeExecutionManager;
        private static MapleTree testTree;

        private Establish context = () =>
        {
            monitor = new CoverageMonitor();
            fakeTreeExecutionManager = new FakeTreeExecutionManager();

            testTree = new MapleTree();

            monitor.BindToTreeExecutionManager(fakeTreeExecutionManager);
        };

        private Because of = () =>
        {
            fakeTreeExecutionManager.OnDecisionTreeStarted(testTree.TreeName, testTree.TreeNodes, testTree.TreeLinks);
        };

        private It should_create_a_coverage_tree_for_the_tree_that_has_started = () =>
        {
            monitor.CoverageTrees.Count.ShouldEqual(1);
        };

        private It should_set_the_properties_on_the_new_coverage_tree = () =>
        {
            var newTree = monitor.CoverageTrees.First();
            newTree.Key.ShouldEqual(testTree.TreeName);
            newTree.Value.Nodes.ShouldNotBeEmpty();
            newTree.Value.Links.ShouldNotBeEmpty();
        };
    }
}