using SAHL.Core.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.FinancialDomain.Models
{
    public class PricedMortgageLoanApplicationInformationModel : ValidatableModel
    {
        public PricedMortgageLoanApplicationInformationModel(Decimal LoanAgreementAmount, Decimal LoanAmountNoFees, Decimal LTV, RateConfigurationValuesModel RateConfigurationValues, 
            Decimal MonthlyInstalment, Decimal PTI, PricedCreditCriteriaModel ApplicationCreditCriteria)
        {
            this.LoanAgreementAmount = LoanAgreementAmount;
            this.LoanAmountNoFees = LoanAmountNoFees;
            this.LTV = LTV;
            this.RateConfigurationValues = RateConfigurationValues;
            this.MonthlyInstalment = MonthlyInstalment;
            this.PTI = PTI;
            this.ApplicationCreditCriteria = ApplicationCreditCriteria;
            Validate();
        }

        [Required]
        [Range(1, Double.MaxValue, ErrorMessage = "LoanAmountNoFees must be greater than 0.")]
        public Decimal LoanAmountNoFees { get; protected set; }

        [Required]
        [Range(1, Double.MaxValue, ErrorMessage = "LoanAgreementAmount must be greater than 0.")]
        public Decimal LoanAgreementAmount { get; protected set; }

        [Required]
        [Range(0, Double.MaxValue, ErrorMessage = "LTV must be greater than 0.")]
        public Decimal LTV { get; protected set; }

        [Required(ErrorMessage = "RateConfigurationValuesModel must be provided.")]
        public RateConfigurationValuesModel RateConfigurationValues { get; protected set; }

        [Required]
        [Range(0, Double.MaxValue, ErrorMessage = "MonthlyInstalment must be greater than 0.")]
        public Decimal MonthlyInstalment { get; protected set; }

        [Required]
        [Range(0, Double.MaxValue, ErrorMessage = "PTI must be greater than 0.")]
        public Decimal PTI { get; protected set; }

        [Required(ErrorMessage = "ApplicationCreditCriteria must be provided.")]
        public PricedCreditCriteriaModel ApplicationCreditCriteria { get; protected set; }
    }
}