using SAHL.Services.Capitec.Managers.Application.Models;
using SAHL.Services.Interfaces.Capitec.ViewModels.Apply;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Capitec.Specs.Stubs
{
    public class SwitchLoan
    {
        public List<Applicant> Applicants;
        public Guid OccupancyTypeID;
        public Guid IncomeTypeID;
        public decimal CashRequired;
        public decimal CurrentBalance;
        public decimal EstimatedMarketValueOfTheHome;
        public decimal HouseholdIncome;
        public decimal Fees;
        public decimal InterimInterest;

        public SwitchLoan(decimal cashRequired, decimal currentBalance, decimal estimatedMarketValueOfTheHome, decimal householdIncome, Guid occupancyTypeID, Guid incomeTypeID, List<Applicant> applicants)
        {
            this.CashRequired = cashRequired;
            this.CurrentBalance = currentBalance;
            this.EstimatedMarketValueOfTheHome = estimatedMarketValueOfTheHome;
            this.EstimatedMarketValueOfTheHome = householdIncome;
            this.OccupancyTypeID = occupancyTypeID;
            this.IncomeTypeID = incomeTypeID;
            this.Applicants = applicants;

            this.Fees = 0;
            this.InterimInterest = 0;
        }


        private SwitchLoanApplication switchLoanApplication;
        public SwitchLoanApplication GetSwitchLoanApplication
        {
            get
            {
                if (switchLoanApplication == null)
                {
                    switchLoanApplication = new SwitchLoanApplication( GetSwitchLoanDetails, this.Applicants, Guid.NewGuid(), "1184050800000-0700");
                }
                return switchLoanApplication;
            }
        }
        private SwitchLoanDetails switchLoanDetails;
        public SwitchLoanDetails GetSwitchLoanDetails
        {
            get
            {
                if (switchLoanDetails == null)
                {
                    switchLoanDetails = new SwitchLoanDetails(OccupancyTypeID, IncomeTypeID, HouseholdIncome, EstimatedMarketValueOfTheHome, CashRequired, CurrentBalance, Fees, InterimInterest, 240, true);
                }
                return switchLoanDetails;
            }
        }
    }

    public class NewPurchaseLoan
    {
        public List<Applicant> Applicants;
        public Guid OccupancyTypeID;
        public Guid IncomeTypeID;
        public decimal deposit;
        public decimal loanAmount;
        public decimal householdIncome;
        public decimal Fees;
        public decimal InterimInterest;


        public NewPurchaseLoan(decimal deposit, decimal loanAmount, decimal householdIncome, Guid occupancyTypeID, Guid incomeTypeID, List<Applicant> applicants)
        {
            this.deposit = deposit;
            this.loanAmount = loanAmount;
            this.householdIncome = householdIncome;
            this.OccupancyTypeID = occupancyTypeID;
            this.IncomeTypeID = incomeTypeID;
            this.Applicants = applicants;

            this.Fees = 0;
            this.InterimInterest = 0;
        }


        private NewPurchaseApplication newPurchaseLoanApplication;
        public NewPurchaseApplication GetNewPurchaseLoanApplication
        {
            get
            {
                if (newPurchaseLoanApplication == null)
                {
                    newPurchaseLoanApplication = new NewPurchaseApplication( GetNewPurchaseLoanDetails, this.Applicants, Guid.NewGuid(), "1184050800000-0700");
                }
                return newPurchaseLoanApplication;
            }
        }


        private NewPurchaseLoanDetails newPurchaseLoanDetails;
        public NewPurchaseLoanDetails GetNewPurchaseLoanDetails
        {
            get
            {
                if (newPurchaseLoanDetails == null)
                {
                    newPurchaseLoanDetails = new NewPurchaseLoanDetails(OccupancyTypeID, IncomeTypeID, householdIncome, loanAmount, deposit, Fees, 240, true);
                }
                return newPurchaseLoanDetails;
            }
        }
    }
}
