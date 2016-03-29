using SAHL.Core.Data;
using SAHL.DomainServiceChecks.Managers.CatsDataManager;
using SAHL.DomainServiceChecks.Managers.CatsDataManager.Statements;
using SAHL.DomainServiceChecks.Managers.ThirdPartyInvoiceDataManager.Statements;
using System;
using System.Linq;

namespace SAHL.DomainServiceChecks.Managers.CatsDataManager
{
    public class CatsDataManager : ICatsDataManager
    {
        private IDbFactory dbFactory;

        public CatsDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public bool DoesCATSPaymentBatchExist(int thirdPaymentBatchKey)
        {
            var statement = new DoesCATSPaymentBatchExistStatement(thirdPaymentBatchKey);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var thirdParty = db.Select(statement);
                return thirdParty.FirstOrDefault() > 0;
            }
        }
    }
}