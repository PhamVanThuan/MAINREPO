using SAHL.Core.BusinessModel.Enums;
using SAHL.UI.Halo.Shared.Configuration;
using System;

namespace SAHL.UI.Halo.Models.Account
{
    public class PersonalLoanRootModel : IHaloTileModel
    {
        public string AccountNumber { get; set; }
        public OriginationSource OriginationSourceKey { get; set; }
        public bool IsInArrears { get; set; }
        public bool IsInAdvance { get; set; }
        public bool IsNonPerforming { get; set; }
        public string AccountStatus { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime CloseDate { get; set; }
        public double LoanAmount { get; set; }
        public double MonthToDateInterest { get; set; }
        public string DebitOrderDay { get; set; }
        public int RemainingTerm { get; set; }
        public double InterestRate { get; set; }
        public double MonthlyInstalment { get; set; }
        public double CurrentBalance { get; set; }
        public double ArrearBalance { get; set; }
        public double MonthlyServiceFee { get; set; }
        public double CreditLifePremium { get; set; }
        public double TotalInstalment { get; set; }
        public double TotalConfirmedIncome { get; set; }
        public SAHL.Core.BusinessModel.Enums.OriginationSource OriginationSource { get; set; }

    };
}
