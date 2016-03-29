using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;

namespace SAHL.DomainProcessManager.Models
{
    [Serializable]
    [DataContract]
    public class ApplicationCreationModel : IDataModel
    {
        private IEnumerable<ApplicantModel> applicants = new List<ApplicantModel>();

        public ApplicationCreationModel(OfferType applicationType, OfferStatus applicationStatus, string reference, int? applicationSourceKey,
            OriginationSource originationSource,
            string consultantFirstName, string consultantSurname, List<ApplicantModel> applicants, int term, decimal estimatedPropertyValue,
            ComcorpApplicationPropertyDetailsModel comcorpApplicationPropertyDetail, Product product, ApplicationDebitOrderModel applicationDebitOrder,
            ApplicationMailingAddressModel applicationMailingAddress, string vendorCode, string transferAttorney, string namePropertyRegistered,
            PropertyAddressModel propertyAddress)
        {
            this.ApplicationType = applicationType;
            this.ApplicationStatus = applicationStatus;
            this.ApplicationSourceKey = applicationSourceKey;
            this.OriginationSource = originationSource;
            this.Reference = reference;
            this.ConsultantFirstName = consultantFirstName;
            this.ConsultantSurname = consultantSurname;
            this.Applicants = applicants;
            this.Term = term;
            this.EstimatedPropertyValue = estimatedPropertyValue;
            this.Product = product;
            this.PropertyAddress = propertyAddress;
            this.ApplicationDebitOrder = applicationDebitOrder;
            this.ApplicationMailingAddress = applicationMailingAddress;
            this.VendorCode = vendorCode;
            this.TransferAttorney = transferAttorney;
            this.NamePropertyRegistered = namePropertyRegistered;
            this.ComcorpApplicationPropertyDetail = comcorpApplicationPropertyDetail;
        }

        [DataMember]
        public Product Product { get; set; }

        [DataMember]
        public OfferType ApplicationType { get; set; }

        [DataMember]
        public OfferStatus ApplicationStatus { get; set; }

        [DataMember]
        public string Reference { get; set; }

        [DataMember]
        public int? ApplicationSourceKey { get; set; }

        [DataMember]
        public OriginationSource OriginationSource { get; set; }

        [DataMember]
        public string ConsultantFirstName { get; set; }

        [DataMember]
        public string ConsultantSurname { get; set; }

        [DataMember]
        public IEnumerable<ApplicantModel> Applicants
        {
            get { return applicants; }

            set
            {
                if (value != null)
                {
                    applicants = new List<ApplicantModel>(value);
                }
            }
        }

        [DataMember]
        public int Term { get; set; }

        [DataMember]
        public decimal EstimatedPropertyValue { get; set; }

        [DataMember]
        public ComcorpApplicationPropertyDetailsModel ComcorpApplicationPropertyDetail { get; set; }

        [DataMember]
        public ApplicationDebitOrderModel ApplicationDebitOrder { get; set; }

        [DataMember]
        public ApplicationMailingAddressModel ApplicationMailingAddress { get; set; }

        [DataMember]
        public string VendorCode { get; set; }

        [DataMember]
        public string TransferAttorney { get; set; }

        [DataMember]
        public string NamePropertyRegistered { get; set; }

        [DataMember]
        public PropertyAddressModel PropertyAddress { get; set; }

        public int ApplicantCount
        {
            get { return Applicants.Count(); }
        }
    }
}
