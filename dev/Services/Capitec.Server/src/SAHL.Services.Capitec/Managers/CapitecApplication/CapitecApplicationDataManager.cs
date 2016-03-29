using SAHL.Core.Data;
using SAHL.Services.Capitec.Managers.CapitecApplication.Models;
using SAHL.Services.Capitec.Managers.CapitecApplication.Statements;
using System;

namespace SAHL.Services.Capitec.Managers.CapitecApplication
{
    public class CapitecApplicationDataManager : ICapitecApplicationDataManager
    {
        private IDbFactory dbFactory;

        public CapitecApplicationDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public CapitecUserBranchMappingModel GetCapitecUserBranchMappingForApplication(Guid applicationId)
        {
            using (var dbContext = dbFactory.NewDb().InReadOnlyAppContext())
            {
                var getConsultantDetailsQuery = new GetCapitecUserBranchForApplicationQuery(applicationId);
                return dbContext.SelectOne(getConsultantDetailsQuery);
            }
        }
    }
}