using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.Capitec.ViewModels.Apply
{
    public class SwitchLoanDetails
    {
        [Range(1, double.MaxValue, ErrorMessage = "Estimated Market Value of the Home must be greater than R 0")]
        [Required(ErrorMessage = "Estimated Market Value of the Home is required")]
        public decimal EstimatedMarketValueOfTheHome { get; protected set; }

        [Range(0, double.MaxValue, ErrorMessage = "Cash Required must be a number")]
        [Required]
        public decimal CashRequired { get; protected set; }

        [Range(0, double.MaxValue, ErrorMessage = "Current Outstanding Home Loan Balance must be greater than or equal to R 0")]
        [Required(ErrorMessage = "Current Outstanding Home Loan Balance is required")]
        public decimal CurrentBalance { get; protected set; }

        [Required]
        public Guid OccupancyType { get; protected set; }

        [Required(ErrorMessage = "Majority Income Type is required")]
        public Guid IncomeType { get; protected set; }

        [Range(1, double.MaxValue, ErrorMessage = "Total Gross Income of all Applicants must be greater than R 0")]
        [Required(ErrorMessage = "Total Gross Income of all Applicants is required")]
        public decimal HouseholdIncome { get; protected set; }

        public int MortgageLoanPurposeKey { get; protected set; }

        public decimal Fees { get; protected set; }

        public decimal InterimInterest { get; protected set; }

        public int TermInMonths { get; protected set; }

        public bool CapitaliseFees { get; protected set; }

        public SwitchLoanDetails(Guid occupancyType, Guid incomeType, decimal householdIncome, decimal estimatedMarketValueOfTheHome, decimal cashRequired, decimal currentBalance, decimal fees, decimal interimInterest, int termInMonths, bool capitaliseFees)
        {
            this.OccupancyType = occupancyType;
            this.IncomeType = incomeType;
            this.HouseholdIncome = householdIncome;
            this.EstimatedMarketValueOfTheHome = estimatedMarketValueOfTheHome;
            this.CashRequired = cashRequired;
            this.CurrentBalance = currentBalance;
            this.MortgageLoanPurposeKey = 3;
            this.InterimInterest = interimInterest;
            this.Fees = fees;
            this.TermInMonths = termInMonths;
            this.CapitaliseFees = capitaliseFees;
        }
    }
}
