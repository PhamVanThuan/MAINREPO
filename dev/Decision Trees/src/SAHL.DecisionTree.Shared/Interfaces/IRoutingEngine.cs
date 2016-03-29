using SAHL.DecisionTree.Shared.Core;
using System;
namespace SAHL.DecisionTree.Shared.Interfaces
{
    public interface IRoutingEngine
    {
        Node CurrentNode { get; set; }
        Node MoveNext();
    }
}
