using SAHL.Core.Data;
using SAHL.DomainServiceChecks.Managers.ApplicationDataManager.Statements;
using System;

namespace SAHL.DomainServiceChecks.Managers.ApplicationDataManager
{
    public class ApplicationDataManager : IApplicationDataManager
    {
        private IDbFactory dbFactory;

        public ApplicationDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public bool IsApplicationOpen(int applicationNumber)
        {
            bool response;
            IsOpenApplicationStatement query = new IsOpenApplicationStatement(applicationNumber);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var results = db.SelectOne<int>(query);
                response = (results > 0);
            }
            return response;
        }

        public bool IsLatestApplicationInformationOpen(int applicationNumber)
        {
            bool response;
            var query = new IsLatestApplicationInformationOpenStatement(applicationNumber);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                response = Convert.ToBoolean(db.SelectOne<int>(query));
            }
            return response;
        }

        public bool IsActiveClientRole(int applicationRoleKey)
        {
            bool response;
            var query = new IsActiveClientRoleStatement(applicationRoleKey);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                response = Convert.ToBoolean(db.SelectOne<int>(query));
            }
            return response;
        }
    }
}