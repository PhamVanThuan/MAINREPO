using SAHL.Core.BusinessModel.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ApplicationDomain.Models
{
    public class SwitchApplicationModel : ApplicationModel
    {
        public SwitchApplicationModel(OfferStatus applicationStatus, int? applicationSourceKey, OriginationSource originationSource,
                                      decimal existingLoan, decimal estimatedPropertyValue, int term, decimal cashOut, Product product, string reference, int applicantCount)
            : base(OfferType.SwitchLoan, applicationStatus, applicationSourceKey, originationSource, term, product, reference, applicantCount)
        {
            this.ExistingLoan = existingLoan;
            this.CashOut = cashOut;
            this.EstimatedPropertyValue = estimatedPropertyValue;

            Validate();
        }

        
        [Range(0, double.MaxValue, ErrorMessage = "Cash Required must be a number")]
        [Required]
        public decimal CashOut { get; protected set; }

        [Range(1, double.MaxValue, ErrorMessage = "Estimated Market Value of the Home must be greater than R 0")]
        [Required]
        public decimal EstimatedPropertyValue { get; protected set; }

        [Range(1, double.MaxValue, ErrorMessage = "Current Outstanding Home Loan Balance must be greater than R 0")]
        [Required]
        public decimal ExistingLoan { get; protected set; }

        public decimal LoanAmountNoFees { get { return this.ExistingLoan + this.CashOut; } }
    }
}
