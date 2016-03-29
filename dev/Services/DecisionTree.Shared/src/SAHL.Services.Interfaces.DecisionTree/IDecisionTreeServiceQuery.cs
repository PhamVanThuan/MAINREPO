using SAHL.Core.Services;
using SAHL.DecisionTree.Shared;
using SAHL.DecisionTree.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.DecisionTree
{
    public interface IDecisionTreeServiceQuery<T> : IServiceQuery<IServiceQueryResult<T>>
    {
        string TreeName { get; }
        int TreeVersion { get; }
        QueryGlobalsVersion GlobalsVersion { get; }
    }
}
