using SAHL.DecisionTree.Shared.Core;
using System;
namespace SAHL.DecisionTree.Shared.Interfaces
{
    public interface ISubTreeContextManager
    {        
        TreeProcessingContext GetSubTreeDebugContext(string treeName, int treeVersion);
        TreeProcessingContext GetSubtreeExecutionContext(string subtreeName, int subtreeVersion, string dbConnection);
    }
}
