using SAHL.Core.Data;
using SAHL.DomainServiceChecks.Managers.AccountDataManager.Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.DomainServiceChecks.Managers.AccountDataManager
{
    public class AccountDataManager : IAccountDataManager
    {
        private IDbFactory dbFactory;

        public AccountDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public bool DoesAccountExist(int accountKey)
        {
            var accountExistsQuery = new AccountExistsStatement(accountKey);
            using (var dbContext = dbFactory.NewDb().InReadOnlyAppContext())
            {
                var accountExists = dbContext.SelectOne(accountExistsQuery);
                return accountExists == 1;
            }
        }

    }
}
