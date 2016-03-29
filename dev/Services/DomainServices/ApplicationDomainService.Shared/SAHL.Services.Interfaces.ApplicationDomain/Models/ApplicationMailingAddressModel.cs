using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ApplicationDomain.Models
{
    public class ApplicationMailingAddressModel : ValidatableModel
    {
        public ApplicationMailingAddressModel(int applicationNumber, int clientAddressKey, CorrespondenceLanguage correspondenceLanguage,
            OnlineStatementFormat onlineStatementFormat, CorrespondenceMedium correspondenceMedium, int? clientToUseForEmailCorrespondence,
            bool onlineStatementRequired)
        {
            this.ApplicationNumber = applicationNumber;
            this.ClientAddressKey = clientAddressKey;
            this.CorrespondenceLanguage = correspondenceLanguage;
            this.OnlineStatementRequired = onlineStatementRequired;
            this.OnlineStatementFormat = onlineStatementFormat;
            this.CorrespondenceMedium = correspondenceMedium;
            this.ClientToUseForEmailCorrespondence = clientToUseForEmailCorrespondence;
            base.Validate();
        }

        [Range(1, Int32.MaxValue, ErrorMessage = "An Application Number must be provided.")]
        public int ApplicationNumber { get; protected set; }

        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage="A ClientAddressKey must be provided.")]
        public int ClientAddressKey { get; protected set; }

        [Required]
        public CorrespondenceLanguage CorrespondenceLanguage { get; protected set; }

        [Required]
        public bool OnlineStatementRequired { get; protected set; }

        public OnlineStatementFormat OnlineStatementFormat { get; protected set; }

        [Required]
        public CorrespondenceMedium CorrespondenceMedium { get; protected set; }

        public int? ClientToUseForEmailCorrespondence { get; protected set; }
    }
}