using SAHL.Tools.DecisionTree.TestRunner.Lib.Interfaces;

namespace SAHL.Tools.DecisionTree.Coverage.Lib
{
    public interface ICoverageMonitor
    {
        void BindToTreeExecutionManager(ITreeExecutionManager treeExecutionManager);

        void WriteCoverageResult(string outputPath, ICoverageResultWriter coverageWriter);
    }
}