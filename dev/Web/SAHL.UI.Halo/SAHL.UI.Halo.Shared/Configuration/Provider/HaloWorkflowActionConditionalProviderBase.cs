using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using SAHL.Core.Data;
using SAHL.Core.BusinessModel;

namespace SAHL.UI.Halo.Shared.Configuration
{
    public abstract class HaloWorkflowActionConditionalProviderBase : HaloTileBaseSqlDataProvider, IHaloWorkflowActionConditionalProvider
    {
        protected HaloWorkflowActionConditionalProviderBase(IDbFactory dbFactory, 
                                                            string processName, string workflowName, string activityName, 
                                                            HaloAlternativeWorkflowActionModel alternativeWorkflowActionModel = null) 
            : base(dbFactory)
        {
            if (string.IsNullOrWhiteSpace(processName)) { throw new ArgumentNullException("processName"); }
            if (string.IsNullOrWhiteSpace(workflowName)) { throw new ArgumentNullException("workflowName"); }
            if (string.IsNullOrWhiteSpace(activityName)) { throw new ArgumentNullException("activityName"); }

            this.ProcessName                    = processName;
            this.WorkflowName                   = workflowName;
            this.ActivityName                   = activityName;
            this.AlternativeWorkflowActionModel = alternativeWorkflowActionModel;
        }

        public string ProcessName { get; private set; }
        public string WorkflowName { get; private set; }
        public string ActivityName { get; private set; }
        public HaloAlternativeWorkflowActionModel AlternativeWorkflowActionModel { get; private set; }

        public abstract bool Execute(BusinessContext businessContext, params string[] additionalParameters);

        protected dynamic ExecuteSqlStatement(BusinessContext businessContext, params string[] additionalParameters)
        {
            if (businessContext.BusinessKey.Key == 0) { return null; }

            var result = this.RetrieveSqlDataRecord<dynamic>(businessContext, additionalParameters);
            if (result == null) { return null; }

            result = ((IEnumerable<dynamic>)result).SingleOrDefault();
            return result;
        }
    }
}
