using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.DomainServiceChecks.Managers.WorkflowAssignmentDataManagerSpecs
{
    public class WorkflowAssignmentDataManager: IWorkflowAssignmentDataManager
    {
        private IDbFactory dbFactory;
        public WorkflowAssignmentDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }
        public bool DoesCapabilityExist(int capability)
        {
            using (var context = dbFactory.NewDb().InReadOnlyAppContext())
            {
                return context.SelectWhere<CapabilityDataModel>(string.Format("[CapabilityKey] = {0}", capability), null).Any();
            }
        }
    }
}