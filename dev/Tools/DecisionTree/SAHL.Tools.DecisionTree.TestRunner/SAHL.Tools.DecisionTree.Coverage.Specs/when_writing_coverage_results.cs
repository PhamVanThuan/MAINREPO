using Machine.Fakes;
using Machine.Specifications;
using SAHL.Tools.DecisionTree.Coverage.Lib;
using SAHL.Tools.DecisionTree.Coverage.Lib.CoverageResults;
using SAHL.Tools.DecisionTree.Coverage.Specs.TestFakes;
using SAHL.Tools.DecisionTree.TestRunner.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Tools.DecisionTree.Coverage.Specs
{
    public class when_writing_coverage_results_with_the_default_writer : WithFakes
    {
        private static CoverageMonitor monitor;
        private static ICoverageResultWriter coverageWriter;
        private static string coverageOutputPath;

        private static FakeTreeExecutionManager fakeTreeExecutionManager;
        private static string treeName;
        private static int treeVersion;

        private Establish context = () =>
        {
            coverageWriter = An<ICoverageResultWriter>();
            monitor = new CoverageMonitor();
            fakeTreeExecutionManager = new FakeTreeExecutionManager();
            fakeTreeExecutionManager.SetRunPath(1);

            treeName = "Maple";
            treeVersion = 1;
            coverageOutputPath = "C:\\CoverageOutput.xml";
            monitor.BindToTreeExecutionManager(fakeTreeExecutionManager);
            fakeTreeExecutionManager.Process(treeName, treeVersion, new List<ITestInput>(), new List<ITestInput>());
        };

        private Because of = () =>
        {
            monitor.WriteCoverageResult(coverageOutputPath, coverageWriter);
        };

        private It should_call_the_coverage_writer = () =>
        {
            coverageWriter.WasToldTo(x => x.WriteCoverageReport(Param.IsAny<CoverageResultSummary>(), coverageOutputPath));
        };

        private It should_summarise_the_tree_results = () =>
        {
            coverageWriter.WasToldTo(x => x.WriteCoverageReport(Param<CoverageResultSummary>.Matches(m =>
                m.TreeResults.Count == 1 &&
                m.TreeResults.First().TreeName == "Maple" &&
                m.TreeResults.First().TotalCoveredLinks == 4 &&
                m.TreeResults.First().TotalNumberOfLinks == 5),
                coverageOutputPath));
        };
    }
}