using SAHL.Core.Data;
using System;

namespace SAHL.Services.EventProjection.Managers.AccountsPaidForAttorneyInvoicesMonthly.Statements
{
    public class InsertAccountsPaidForAttorneyInvoicesMonthlyStatement : ISqlStatement<object>
    {
        public int ThirdPartyInvoiceKey { get; protected set; }
        public int AccountKey { get; protected set; }

        public Guid ThirdPartyId { get; protected set; }

        public InsertAccountsPaidForAttorneyInvoicesMonthlyStatement(int thirdPartyInvoiceKey, int accountKey, Guid thirdPartyId)
        {
            this.ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
            this.AccountKey = accountKey;
            this.ThirdPartyId = thirdPartyId;
        }

        public string GetStatement()
        {
            return @"INSERT INTO [EventProjection].[projection].[AccountsPaidForAttorneyInvoicesMonthly]
           ([AttorneyId]
           ,[ThirdPartyInvoiceKey]
           ,[AccountKey])
             VALUES (
                   @ThirdPartyId,
		           @ThirdPartyInvoiceKey,
                   @AccountKey
		           )";
        }
    }
}