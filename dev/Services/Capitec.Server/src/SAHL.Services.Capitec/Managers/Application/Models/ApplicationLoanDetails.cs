using SAHL.Core.Data.Models.Capitec;
using SAHL.Services.Interfaces.Capitec.ViewModels.Apply;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Capitec.Managers.Application.Models
{
    public class ApplicationLoanDetails
    {
        public decimal EstimatedMarketValueOfTheHome { get; set; }

        public decimal CashRequired { get; set; }

        public decimal CurrentBalance { get; set; }

		public Guid OccupancyType { get; set; }

		public Guid IncomeType { get; set; }

        public decimal HouseholdIncome { get; set; }

        public int MortgageLoanPurposeKey { get; set; }

        public decimal Fees { get; set; }

        public decimal InterimInterest { get; set; }

        public decimal PurchasePrice { get; set; }

        public decimal Deposit { get; set; }

        public Guid ApplicationType { get; set; }

        public ApplicationLoanDetails()
        {

        }

        public ApplicationLoanDetails(SwitchLoanDetails details)
		{
			this.OccupancyType = details.OccupancyType;
			this.IncomeType = details.IncomeType;
			this.HouseholdIncome = details.HouseholdIncome;
			this.EstimatedMarketValueOfTheHome = details.EstimatedMarketValueOfTheHome;
			this.CashRequired = details.CashRequired;
			this.CurrentBalance = details.CurrentBalance;
            this.MortgageLoanPurposeKey = 3;
            this.InterimInterest = details.InterimInterest;
            this.Fees = details.Fees;
            this.ApplicationType = Guid.Parse(ApplicationPurposeEnumDataModel.SWITCH);
            this.CapitaliseFees = details.CapitaliseFees;
		}

        public ApplicationLoanDetails(NewPurchaseLoanDetails details)
        {
            this.OccupancyType = details.OccupancyType;
            this.IncomeType = details.IncomeType;
            this.HouseholdIncome = details.HouseholdIncome;
            this.PurchasePrice = details.PurchasePrice;
            this.Deposit = details.Deposit;
            this.MortgageLoanPurposeKey = 3;
            this.Fees = details.Fees;
            this.ApplicationType = Guid.Parse(ApplicationPurposeEnumDataModel.NEW_PURCHASE);
            this.CapitaliseFees = details.CapitaliseFees;
        }

        public bool CapitaliseFees { get; set; }
    }
}
