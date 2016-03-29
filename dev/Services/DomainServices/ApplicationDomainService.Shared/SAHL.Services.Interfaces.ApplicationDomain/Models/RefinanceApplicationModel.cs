using SAHL.Core.BusinessModel.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ApplicationDomain.Models
{
    public class RefinanceApplicationModel : ApplicationModel
    {
        public RefinanceApplicationModel(OfferStatus applicationStatus, int? applicationSourceKey, OriginationSource originationSource,
                                     decimal estimatedPropertyValue, int term, decimal cashOut, Product product, string reference, int applicantCount)
            : base(OfferType.RefinanceLoan, applicationStatus, applicationSourceKey, originationSource, term, product, reference, applicantCount)
        {
            this.CashOut = cashOut;
            this.EstimatedPropertyValue = estimatedPropertyValue;

            Validate();
        }

        
        [Range(1, double.MaxValue, ErrorMessage = "Cash Required must be greater than R 0.00")]
        [Required]
        public decimal CashOut { get; protected set; }

        [Range(1, double.MaxValue, ErrorMessage = "Estimated Market Value of the Home must be greater than R 0.00")]
        [Required]
        public decimal EstimatedPropertyValue { get; protected set; }

        public decimal LoanAmountNoFees { get { return this.CashOut; } }
    }
}
