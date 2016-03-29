using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ClientDomain.Models
{
    public class LiabilityLoanModel : ValidatableModel
    {
        public LiabilityLoanModel(AssetLiabilitySubType loanType, string financialInstitution, DateTime dateRepayable, double instalmentValue, double liabilityValue)
        {
            this.LoanType = loanType;
            this.FinancialInstitution = financialInstitution;
            this.DateRepayable = dateRepayable;
            this.InstalmentValue = instalmentValue;
            this.LiabilityValue = liabilityValue;
            Validate();
        }

        [Required]
        public AssetLiabilitySubType LoanType { get; protected set; }

        [Required]
        public string FinancialInstitution { get; protected set; }

        [Required]
        public DateTime DateRepayable { get; protected set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Instalment Value must be greater than or equal to zero.")]
        public double InstalmentValue { get; protected set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Liability Value must be greater than zero.")]
        public double LiabilityValue { get; protected set; }
    }
}