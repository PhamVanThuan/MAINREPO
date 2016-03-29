using Machine.Fakes;
using Machine.Specifications;
using SAHL.Tools.DecisionTree.Coverage.Lib;
using SAHL.Tools.DecisionTree.Coverage.Specs.TestFakes;
using SAHL.Tools.DecisionTree.TestRunner.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.DecisionTree.Coverage.Specs
{
    class when_two_links_lead_to_the_same_node : WithFakes
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

            treeName = "Birch";
            treeVersion = 1;

            monitor.BindToTreeExecutionManager(fakeTreeExecutionManager);
        };

        private Because of = () =>
        {
            // Run the first path
            fakeTreeExecutionManager.SetRunPath(1);
            fakeTreeExecutionManager.Process(treeName, treeVersion, new List<ITestInput>(), new List<ITestInput>());

            // Run the second path
            fakeTreeExecutionManager.SetRunPath(2);
            fakeTreeExecutionManager.Process(treeName, treeVersion, new List<ITestInput>(), new List<ITestInput>());
        };

        private It should_create_the_coverage_tree = () =>
        {
            monitor.CoverageTrees.First().Key.ShouldEqual(treeName);
        };
        private It should_cover_the_decision_yes_link = () =>
        {
            monitor.CoverageTrees.First().Value.LinkIDs.ShouldContain(-1);
        };
        private It should_cover_the_decision_no_link = () =>
        {
            monitor.CoverageTrees.First().Value.LinkIDs.ShouldContain(-2);
        };

        //private It should_contain_a_list_of_all_the_links_passed_through = () =>
        //{
        //    monitor.CoverageTrees.First().Value.LinkIDs.ShouldContain(expectedLinkIDs.ToArray());
        //};

        //private It should_contain_a_list_of_all_the_nodes_passed_through = () =>
        //{
        //    monitor.CoverageTrees.First().Value.NodeIDs.ShouldContain(expectedNodeIDs.ToArray());
        //};
    }
}
