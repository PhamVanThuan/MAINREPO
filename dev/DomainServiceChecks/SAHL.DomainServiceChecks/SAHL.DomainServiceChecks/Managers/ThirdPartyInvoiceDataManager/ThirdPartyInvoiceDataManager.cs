using SAHL.Core.Data;
using SAHL.DomainServiceChecks.Managers.ThirdPartyInvoiceDataManager.Statements;
using System;
using System.Linq;

namespace SAHL.DomainServiceChecks.Managers.ThirdPartyInvoiceDataManager
{
    public class ThirdPartyInvoiceDataManager : IThirdPartyInvoiceDataManager
    {
        private IDbFactory dbFactory;

        public ThirdPartyInvoiceDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public bool DoesThirdPartyInvoiceExist(int thirdPartyInvoiceKey)
        {
            ThirdPartyInvoiceExistsStatement statement = new ThirdPartyInvoiceExistsStatement(thirdPartyInvoiceKey);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var results = db.SelectOne(statement);
                return (results > 0);
            }
        }

        public bool DoesThirdPartyExist(Guid thirdPartyId)
        {
            var statement = new ThirdPartyExistsStatement(thirdPartyId);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var thirdParty = db.Select(statement);
                return thirdParty.Any();
            }
        }
    }
}