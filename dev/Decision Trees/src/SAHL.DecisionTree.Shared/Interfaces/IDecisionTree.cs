using Microsoft.Scripting.Hosting;
using SAHL.DecisionTree.Shared.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.DecisionTree.Shared.Interfaces
{
    public interface IDecisionTree
    {
        List<Link> NodeLinks { get; }
        Dictionary<int, Node> Nodes { get; }
    }
}
