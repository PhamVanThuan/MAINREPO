using SAHL.Core.Events;
using System;

namespace SAHL.Services.Interfaces.FinanceDomain.Events
{
    public class TransactionsProcessedForThirdPartyInvoicePaymentEvent : Event
    {
        public int AccountKey { get; protected set; }

        public int ThirdPartyInvoiceKey { get; protected set; }

        public int FinancialServiceKey { get; protected set; }

        public decimal Amount { get; protected set; }

        public DateTime EffectiveDate { get; protected set; }

        public string Reference { get; protected set; }

        public string UserName { get; protected set; }

        public TransactionsProcessedForThirdPartyInvoicePaymentEvent(int financialServiceKey, int accountKey, int thirdPartyInvoiceKey, decimal amount, DateTime effectiveDate,
            string reference, string userName)
            : base(effectiveDate)
        {
            FinancialServiceKey = financialServiceKey;
            AccountKey = accountKey;
            ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
            Amount = amount;
            EffectiveDate = effectiveDate;
            Reference = reference;
            UserName = UserName;
        }
    }
}