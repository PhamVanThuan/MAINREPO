using System;
using System.ComponentModel.DataAnnotations;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Models.ThirdParty.Invoices
{
    public class AggregatedThirdPartyInvoiceRootModel : IHaloTileModel
    {
        [Required]
        public string LegalName
        { get; set; }

        [Required]
        [RegularExpression("\\d{13}", ErrorMessage = "ID Number must be 13 digits")]
        public string IDNumber
        { get; set; }

        [Range(12, 24)]
        public string RegistrationNumber
        { get; set; }

        [Required]
        public string LegalEntityStatus
        { get; set; }

        [StringLength(13)]
        public string HomephoneNumber
        { get; set; }

        public string WorkphoneNumber
        { get; set; }

        [Phone]
        public string CellPhoneNumber
        { get; set; }

        [Required]
        public DateTime DateOfBirth
        { get; set; }

        [Required]
        [MaxLength(255)]
        public string PostalAddress
        { get; set; }

        [Required]
        [MaxLength(255)]
        public string ResidentialAddress
        { get; set; }

        [EmailAddress]
        public string EmailAddress
        { get; set; }

        public string BankingInstitute
        { get; set; }
    }
}