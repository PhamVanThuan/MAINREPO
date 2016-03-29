using SAHL.Tools.DecisionTree.Coverage.Lib.CoverageResults;

namespace SAHL.Tools.DecisionTree.Coverage.Lib
{
    public interface ICoverageResultWriter
    {
        void WriteCoverageReport(CoverageResultSummary resultSummary, string outputPath);
    }
}