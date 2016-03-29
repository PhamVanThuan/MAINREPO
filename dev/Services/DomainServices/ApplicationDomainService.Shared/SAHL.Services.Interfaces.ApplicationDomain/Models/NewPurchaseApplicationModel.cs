using SAHL.Core.BusinessModel.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ApplicationDomain.Models
{
    public class NewPurchaseApplicationModel : ApplicationModel
    {
        public NewPurchaseApplicationModel(OfferStatus applicationStatus, int? applicationSourceKey, OriginationSource originationSource, 
            decimal deposit, decimal purchasePrice, decimal estimatedPropertyValue, int term, Product product, string reference, int applicantCount, string transferAttorney)
            : base(OfferType.NewPurchaseLoan, applicationStatus, applicationSourceKey, originationSource, term, product, reference, applicantCount)
        {
            this.PurchasePrice = purchasePrice;
            this.Deposit = deposit;
            this.TransferAttorney = transferAttorney;

            Validate();
        }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Purchase price must be greater than R 1.00")]
        public decimal PurchasePrice { get; protected set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Deposit must be R 0.00 or greater")]
        public decimal Deposit { get; protected set; }

        public decimal LoanAmountNoFees { get { return this.PurchasePrice - this.Deposit; } }

        public string TransferAttorney { get; protected set; }
    }
}