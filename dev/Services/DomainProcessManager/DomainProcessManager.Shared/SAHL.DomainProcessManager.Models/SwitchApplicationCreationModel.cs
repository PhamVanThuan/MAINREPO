using SAHL.Core.BusinessModel.Enums;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SAHL.DomainProcessManager.Models
{
    [Serializable]
    [DataContract]
    public class SwitchApplicationCreationModel : ApplicationCreationModel
    {
        public SwitchApplicationCreationModel(OfferStatus applicationStatus, string reference, int? applicationSourceKey, OriginationSource originationSource,
                                              string consultantFirstName, string consultantSurname, List<ApplicantModel> applicants, decimal existingLoan,
                                              decimal estimatedPropertyValue, ComcorpApplicationPropertyDetailsModel comcorpApplicationPropertyDetail, int term,
                                              decimal cashOut, string cashOutReason, Product product, decimal higherBond, decimal estimatedLoan, bool quickCash, bool capitaliseFees,
                                              ApplicationDebitOrderModel debitOrder, ApplicationMailingAddressModel applicationMailingAddress, string vendorCode,
                                              string namePropertyRegistered, PropertyAddressModel propertyAddress)
            : base(OfferType.SwitchLoan, applicationStatus, reference, applicationSourceKey, originationSource, consultantFirstName, consultantSurname, applicants, term,
                   estimatedPropertyValue, comcorpApplicationPropertyDetail, product, debitOrder, applicationMailingAddress, vendorCode, null, namePropertyRegistered, propertyAddress)
        {
            this.ExistingLoan = existingLoan;
            this.CashOut = cashOut;
            this.CashOutReason = cashOutReason;
            this.EstimatedPropertyValue = estimatedPropertyValue;
            this.HigherBond = higherBond;
            this.EstimatedLoan = estimatedLoan;
            this.QuickCash = quickCash;
            this.CapitaliseFees = capitaliseFees;
        }

        [DataMember]
        public decimal ExistingLoan { get; set; }

        [DataMember]
        public decimal CashOut { get; set; }

        [DataMember]
        public string CashOutReason { get; set; }

        [DataMember]
        public decimal HigherBond { get; set; }

        [DataMember]
        public decimal EstimatedLoan { get; set; }

        [DataMember]
        public bool QuickCash { get; set; }

        [DataMember]
        public bool CapitaliseFees { get; set; }
    }
}