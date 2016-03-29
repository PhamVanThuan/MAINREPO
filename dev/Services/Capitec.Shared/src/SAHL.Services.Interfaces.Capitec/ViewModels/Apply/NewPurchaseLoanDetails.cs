using SAHL.Core.Data.Models.Capitec;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.Capitec.ViewModels.Apply
{
    public class NewPurchaseLoanDetails
	{
        [Range(1, int.MaxValue, ErrorMessage = "Purchase Price must be greater than R 0")]
        [Required]
        public decimal PurchasePrice { get; protected set; }

        [Range(0, int.MaxValue, ErrorMessage = "Cash Deposit must be a number")]
        [Required(ErrorMessage = "Cash Deposit is required")]
        public decimal Deposit { get; protected set; }

        [Required]
		public Guid OccupancyType { get; protected set; }

        [Required(ErrorMessage="Majority Income Type is required")]
		public Guid IncomeType { get; protected set; }

        [Range(1, double.MaxValue, ErrorMessage = "Total Gross Income of all Applicants must be greater than R 0")]
        [Required(ErrorMessage="Total Gross Income of all Applicants is required")]
        public decimal HouseholdIncome { get; protected set; }

        public int MortgageLoanPurposeKey { get; protected set; }

        public decimal Fees { get; protected set; }

        public int TermInMonths { get; protected set; }

        public bool CapitaliseFees { get; protected set; }

        public NewPurchaseLoanDetails(Guid occupancyType, Guid incomeType, decimal householdIncome, decimal purchasePrice, decimal deposit, decimal fees, int termInMonths, bool capitaliseFees)
		{
			this.OccupancyType = occupancyType;
			this.IncomeType = incomeType;
			this.HouseholdIncome = householdIncome;
			this.PurchasePrice = purchasePrice;
			this.Deposit = deposit;
            this.MortgageLoanPurposeKey = 3;
            this.Fees = fees;
            this.TermInMonths = termInMonths;
            this.CapitaliseFees = capitaliseFees;
		}
	}
}
