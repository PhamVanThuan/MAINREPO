using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Validation;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.AddressDomain.Model
{
    public class PostalAddressModel : ValidatableModel
    {
        public PostalAddressModel(string boxNumber, string postNetSuiteNumber, string postOffice, string province, string city, string postalCode, AddressFormat addressFormat)
        {
            this.PostalCode = postalCode;
            this.AddressFormat = addressFormat;
            this.PostOffice = postOffice;
            this.Province = province;
            this.City = city;
            this.PostNetSuiteNumber = postNetSuiteNumber;
            this.BoxNumber = boxNumber;
            Validate();
        }

        [Required]
        public string BoxNumber { get; protected set; }

        public string PostNetSuiteNumber { get; protected set; }

        [Required]
        public string City { get; protected set; }

        [Required]
        public string Province { get; protected set; }

        [Required]
        public string PostOffice { get; protected set; }

        [Required]
        public string PostalCode { get; protected set; }

        [Required]
        public AddressFormat AddressFormat { get; protected set; }
    }
}