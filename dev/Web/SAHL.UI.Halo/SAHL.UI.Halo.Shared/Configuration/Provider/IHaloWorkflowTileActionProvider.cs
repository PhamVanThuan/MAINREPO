using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using SAHL.Core.BusinessModel;

namespace SAHL.UI.Halo.Shared.Configuration
{
    public interface  IHaloWorkflowTileActionProvider
    {
        IEnumerable<IHaloWorkflowAction> GetTileActions(BusinessContext businessContext, string roleName, string[] capabilities);
        string GetUiStatementToLoadWorkflow(BusinessContext businessContext);
    }

    public interface IHaloWorkflowTileActionProvider<T> where T : IHaloWorkflowTileActionProvider
    {
    }
}
