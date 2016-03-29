using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.FinancialDomain.Models
{
    public class MortgageLoanApplicationInformationModel
    {
        public MortgageLoanApplicationInformationModel(decimal loanAmountNoFees, decimal? bondToRegister,
            OfferType offerTypeKey, decimal? requestedCashAmount, decimal householdIncome, EmploymentType? employmentTypeKey,
            decimal propertyValuation, MortgageLoanPurpose mortgageLoanPurposeKey, OriginationSource originationSourceKey, int term)
        {
            this.LoanAmountNoFees = loanAmountNoFees;
            this.BondToRegister = bondToRegister;
            this.OfferType = offerTypeKey;
            this.RequestedCashAmount = requestedCashAmount;
            this.HouseholdIncome = householdIncome;
            this.EmploymentType = employmentTypeKey;
            this.PropertyValuation = propertyValuation;
            this.MortgageLoanPurpose = mortgageLoanPurposeKey;
            this.OriginationSource = originationSourceKey;
            this.Term = term;
        }

        public decimal LoanAmountNoFees { get; protected set; }
        
        public decimal? BondToRegister { get; protected set; }
        
        public OfferType OfferType { get; protected set; }
        
        public decimal? RequestedCashAmount { get; protected set; }

        public decimal HouseholdIncome { get; protected set; }

        public EmploymentType? EmploymentType { get; protected set; }

        public decimal PropertyValuation { get; protected set; }

        public MortgageLoanPurpose MortgageLoanPurpose { get; protected set; }

        public OriginationSource OriginationSource { get; protected set; }

        public int Term { get; protected set; }
    }
}
