using SAHL.Core.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SAHL.Core.BusinessModel.Enums;

namespace SAHL.Services.Interfaces.ApplicationDomain.Models
{
    public class ApplicationModel : ValidatableModel
    {
        public ApplicationModel(OfferType applicationType, OfferStatus applicationStatus, int? applicationSourceKey, OriginationSource originationSource, int term, Product product, 
            string reference, int applicantCount)
        {
            this.ApplicationType = applicationType;
            this.ApplicationStatus = applicationStatus;
            this.ApplicationSourceKey = applicationSourceKey;
            this.OriginationSource = originationSource;
            this.Term = term;
            this.Product = product;
            this.Reference = reference;
            this.ApplicantCount = applicantCount;
        }

        [Required]
        public Product Product { get; protected set; }

        [Required]
        public OfferType ApplicationType { get; protected set; }

        [Required]
        public OfferStatus ApplicationStatus { get; protected set; }

        public int? ApplicationSourceKey { get; protected set; }

        [Required]
        public OriginationSource OriginationSource { get; protected set; }

        [Required]
        public int Term { get; protected set; }

        public string Reference { get; protected set; }

        [Required]
        public int ApplicantCount { get; protected set; }
    }
}
