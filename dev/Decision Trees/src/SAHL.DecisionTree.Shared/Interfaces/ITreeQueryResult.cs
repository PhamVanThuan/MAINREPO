using SAHL.DecisionTree.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.DecisionTree.Shared.Interfaces
{
    public interface ITreeQueryResult
    {
        QueryGlobalsVersion GlobalsVersionResultIsBasedOn { get; set; }
    }
}
