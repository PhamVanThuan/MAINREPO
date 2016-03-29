using SAHL.Core.Events;
using System;

namespace SAHL.Services.Interfaces.FinanceDomain.Events
{
    public class TransactionProcessedForThirdPartyInvoicePaymentReversalEvent : Event
    {
        public int AccountKey { get; protected set; }

        public int ThirdPartyInvoiceKey { get; protected set; }

        public int FinancialTransactionKey { get; protected set; }

        public decimal Amount { get; protected set; }

        public DateTime EffectiveDate { get; protected set; }

        public string Reference { get; protected set; }

        public string UserName { get; protected set; }

        public TransactionProcessedForThirdPartyInvoicePaymentReversalEvent(int financialTransactionKey, int accountKey, int thirdPartyInvoiceKey, decimal amount, DateTime effectiveDate,
            string reference, string userName)
            : base(effectiveDate)
        {
            FinancialTransactionKey = financialTransactionKey;
            AccountKey = accountKey;
            ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
            Amount = amount;
            EffectiveDate = effectiveDate;
            Reference = reference;
            UserName = UserName;
        }
    }
}