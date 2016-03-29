using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Events;
using System;

namespace SAHL.Services.Interfaces.ClientDomain.Events
{
    public class LiabilityLoanAddedToClientEvent : Event
    {
        public LiabilityLoanAddedToClientEvent(DateTime date, AssetLiabilitySubType loanType, string financialInstitution, DateTime dateRepayable, double instalmentValue, 
            double liabilityValue)
            : base(date)
        {
            this.LoanType = loanType;
            this.FinancialInstitution = financialInstitution;
            this.DateRepayable = dateRepayable;
            this.InstalmentValue = instalmentValue;
            this.LiabilityValue = liabilityValue;
        }

        public AssetLiabilitySubType LoanType { get; protected set; }

        public string FinancialInstitution { get; protected set; }

        public DateTime DateRepayable { get; protected set; }

        public double InstalmentValue { get; protected set; }

        public double LiabilityValue { get; protected set; }
    }
}
