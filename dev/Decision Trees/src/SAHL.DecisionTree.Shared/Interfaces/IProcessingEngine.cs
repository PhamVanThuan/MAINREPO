using SAHL.DecisionTree.Shared.Core;
using System;
namespace SAHL.DecisionTree.Shared.Interfaces
{
    public interface IProcessingEngine
    {
        dynamic GetVariable(string name);
        void InitializeEngine();
        void SetVariable(string name, dynamic obj);
        bool Execute(string code);
        event EventHandler<NodeExceptionEventsArgs> CodeExecutionExceptionRaised;
        bool Process(string code);
    }
}
