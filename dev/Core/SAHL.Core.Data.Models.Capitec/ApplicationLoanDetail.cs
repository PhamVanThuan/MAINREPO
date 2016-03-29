using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Capitec
{
    [Serializable]
    public partial class ApplicationLoanDetailDataModel :  IDataModel
    {
        public ApplicationLoanDetailDataModel(Guid applicationId, Guid employmentTypeID, Guid occupancyTypeEnumID, decimal householdIncome, decimal instalment, decimal interestRate, decimal loanAmount, decimal lTV, decimal pTI, decimal fees, int termInMonths, bool capitaliseFees)
        {
            this.ApplicationId = applicationId;
            this.EmploymentTypeID = employmentTypeID;
            this.OccupancyTypeEnumID = occupancyTypeEnumID;
            this.HouseholdIncome = householdIncome;
            this.Instalment = instalment;
            this.InterestRate = interestRate;
            this.LoanAmount = loanAmount;
            this.LTV = lTV;
            this.PTI = pTI;
            this.Fees = fees;
            this.TermInMonths = termInMonths;
            this.CapitaliseFees = capitaliseFees;
		
        }

        public ApplicationLoanDetailDataModel(Guid id, Guid applicationId, Guid employmentTypeID, Guid occupancyTypeEnumID, decimal householdIncome, decimal instalment, decimal interestRate, decimal loanAmount, decimal lTV, decimal pTI, decimal fees, int termInMonths, bool capitaliseFees)
        {
            this.Id = id;
            this.ApplicationId = applicationId;
            this.EmploymentTypeID = employmentTypeID;
            this.OccupancyTypeEnumID = occupancyTypeEnumID;
            this.HouseholdIncome = householdIncome;
            this.Instalment = instalment;
            this.InterestRate = interestRate;
            this.LoanAmount = loanAmount;
            this.LTV = lTV;
            this.PTI = pTI;
            this.Fees = fees;
            this.TermInMonths = termInMonths;
            this.CapitaliseFees = capitaliseFees;
		
        }		

        public Guid Id { get; set; }

        public Guid ApplicationId { get; set; }

        public Guid EmploymentTypeID { get; set; }

        public Guid OccupancyTypeEnumID { get; set; }

        public decimal HouseholdIncome { get; set; }

        public decimal Instalment { get; set; }

        public decimal InterestRate { get; set; }

        public decimal LoanAmount { get; set; }

        public decimal LTV { get; set; }

        public decimal PTI { get; set; }

        public decimal Fees { get; set; }

        public int TermInMonths { get; set; }

        public bool CapitaliseFees { get; set; }
    }
}