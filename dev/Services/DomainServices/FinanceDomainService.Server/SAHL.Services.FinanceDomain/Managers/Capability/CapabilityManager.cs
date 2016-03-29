using SAHL.Core.Data;
using SAHL.Services.FinanceDomain.Managers.Capability.Statements;
using System.Collections.Generic;

namespace SAHL.Services.FinanceDomain.Managers.Capability
{
    public class CapabilityManager : ICapabilityManager
    {
        IDbFactory dbFactory;

        public CapabilityManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public IEnumerable<ApprovalMandateRanges> GetCapabilityMandates()
        {
            using (var db = dbFactory.NewDb().InReadOnlyAppContext())
            {
                var query = new GetCapabilityMandatesStatement();
                return db.Select(query);
            }
        }
    }
}
