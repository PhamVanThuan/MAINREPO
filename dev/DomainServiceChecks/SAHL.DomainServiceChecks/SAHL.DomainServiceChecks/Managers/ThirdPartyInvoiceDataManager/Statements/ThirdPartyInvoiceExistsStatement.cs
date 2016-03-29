using SAHL.Core.Data;
using System;
using System.Linq;

namespace SAHL.DomainServiceChecks.Managers.ThirdPartyInvoiceDataManager.Statements
{
    public class ThirdPartyInvoiceExistsStatement : ISqlStatement<int>
    {
        public int ThirdPartyInvoiceKey { get; protected set; }

        public ThirdPartyInvoiceExistsStatement(int thirdPartyInvoiceKey)
        {
            this.ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
        }

        public string GetStatement()
        {
            var query = "SELECT count(1) FROM [2AM].[dbo].[ThirdPartyInvoice] WHERE [ThirdPartyInvoiceKey] = @ThirdPartyInvoiceKey";
            return query;
        }
    }
}