using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Validation;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ApplicationDomain.Models
{
    public class VendorModel : ValidatableModel
    {
        public VendorModel(int vendorKey, string vendorCode, int organisationStructureKey, int legalEntityKey, int generalStatusKey)
        {
            this.VendorKey = vendorKey;
            this.VendorCode = vendorCode;
            this.OrganisationStructureKey = organisationStructureKey;
            this.LegalEntityKey = legalEntityKey;
            this.GeneralStatusKey = generalStatusKey;
        }

        [Required]
        public int VendorKey { get; protected set; }

        [Required]
        public string VendorCode { get; protected set; }

        [Required]
        public int OrganisationStructureKey { get; protected set; }

        [Required]
        public int LegalEntityKey { get; protected set; }

        [Required]
        public int GeneralStatusKey { get; protected set; }
    }
}