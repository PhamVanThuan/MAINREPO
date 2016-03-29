using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Events;
using System;

namespace SAHL.Services.Interfaces.ApplicationDomain.Events
{
    public class DebitOrderAddedToApplicationEvent : Event
    {
        public DebitOrderAddedToApplicationEvent(DateTime date, FinancialServicePaymentType paymentType, int debitOrderDay, int clientBankAccountKey, int applicationNumber)
            : base(date)
        {
            this.PaymentType = paymentType;
            this.DebitOrderDay = debitOrderDay;
            this.ClientBankAccountKey = clientBankAccountKey;
            this.ApplicationNumber = applicationNumber;
        }

        public FinancialServicePaymentType PaymentType { get; protected set; }

        public int DebitOrderDay { get; protected set; }

        public int ClientBankAccountKey { get; protected set; }

        public int ApplicationNumber { get; protected set; }
    }
}