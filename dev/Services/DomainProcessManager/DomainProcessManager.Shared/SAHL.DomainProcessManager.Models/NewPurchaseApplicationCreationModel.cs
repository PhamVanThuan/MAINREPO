using SAHL.Core.BusinessModel.Enums;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SAHL.DomainProcessManager.Models
{
    [Serializable]
    [DataContract]
    public class NewPurchaseApplicationCreationModel : ApplicationCreationModel
    {
        public NewPurchaseApplicationCreationModel(OfferStatus applicationStatus, string reference, int? applicationSourceKey, OriginationSource originationSource,
                                                   string consultantFirstName, string consultantSurname, List<ApplicantModel> applicants, decimal deposit, decimal purchasePrice,
                                                   decimal estimatedPropertyValue, ComcorpApplicationPropertyDetailsModel comcorpApplicationPropertyDetail, int term, Product product,
                                                   ApplicationDebitOrderModel debitOrder, ApplicationMailingAddressModel applicationMailingAddress, string vendorCode,
                                                   decimal loanAmountRequired, string sellerIDNumber, string transferAttorney, string namePropertyRegistered,
                                                   PropertyAddressModel propertyAddress)
            : base(OfferType.NewPurchaseLoan, applicationStatus, reference, applicationSourceKey, originationSource, consultantFirstName, consultantSurname, applicants,
                   term, estimatedPropertyValue, comcorpApplicationPropertyDetail, product, debitOrder, applicationMailingAddress, vendorCode, transferAttorney, namePropertyRegistered, 
            propertyAddress)
        {
            this.PurchasePrice = purchasePrice;
            this.Deposit = deposit;
            this.EstimatedPropertyValue = estimatedPropertyValue;
            this.LoanAmountRequired = loanAmountRequired;
            this.SellerIDNumber = sellerIDNumber;
        }

        [DataMember]
        public decimal PurchasePrice { get; set; }

        [DataMember]
        public decimal Deposit { get; set; }

        [DataMember]
        public decimal LoanAmountRequired { get; set; }

        [DataMember]
        public string SellerIDNumber { get; set; }
    }
}