using System;

namespace SAHL.Services.Interfaces.LegacyEventGenerator.Events.Domains.Application
{
    public class ApplicationLoanAgreementAmountUpdatedLegacyEvent : LegacyEvent
    {
        public ApplicationLoanAgreementAmountUpdatedLegacyEvent(Guid id, DateTime date, int adUserKey, string aduserName, 
            int applicationKey, int applicationInformationKey, decimal oldLoanAgreementAmountValue, decimal newLoanAgreementAmountValue)
            : base(id, date, adUserKey, aduserName)
        {
            this.ApplicationKey = applicationKey;
            this.ApplicationInformationKey = applicationInformationKey;
            this.OldLoanAgreementAmountValue = oldLoanAgreementAmountValue;
            this.NewLoanAgreementAmountValue = newLoanAgreementAmountValue;
        }

        public int ApplicationKey { get; protected set; }

        public int ApplicationInformationKey { get; protected set; }

        public decimal OldLoanAgreementAmountValue { get; protected set; }

        public decimal NewLoanAgreementAmountValue { get; protected set; }
    }
}