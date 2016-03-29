using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using SAHL.Core.BusinessModel;

namespace SAHL.UI.Halo.Shared.Configuration
{
    public interface IHaloWorkflowActionConditionalProvider
    {
        string ProcessName { get; }
        string WorkflowName { get; }
        string ActivityName { get; }
        HaloAlternativeWorkflowActionModel AlternativeWorkflowActionModel { get; }

        string GetSqlStatement(BusinessContext businessContext, params object[] additionalParameters);

        bool Execute(BusinessContext businessContext, params string[] additionalParameters);
    }

    public interface IHaloWorkflowActionConditionalProvider<T> : IHaloWorkflowActionConditionalProvider 
    {
    }
}
