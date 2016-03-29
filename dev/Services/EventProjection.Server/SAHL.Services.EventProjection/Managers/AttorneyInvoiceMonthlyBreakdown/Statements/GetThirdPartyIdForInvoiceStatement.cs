using SAHL.Core.Data;
using System;

namespace SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown.Statements
{
    public class GetThirdPartyIdForInvoiceStatement : ISqlStatement<Guid>
    {
        public int ThirdPartyInvoiceKey { get; protected set; }

        public GetThirdPartyIdForInvoiceStatement(int thirdPartyInvoiceKey)
        {
            this.ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
        }

        public string GetStatement()
        {
            return @"select thirdPartyId from [2am].dbo.ThirdPartyInvoice where thirdPartyInvoiceKey = @thirdPartyInvoiceKey";
        }
    }
}