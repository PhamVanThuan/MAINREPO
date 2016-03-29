using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Validation;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.AddressDomain.Model
{
    public class FreeTextAddressModel : ValidatableModel
    {
        public FreeTextAddressModel(AddressFormat addressFormat, string freeText1, string freeText2, string freeText3, string freeText4, string freeText5, string country)
        {
            this.AddressFormat = addressFormat;
            this.FreeText1 = freeText1;
            this.FreeText2 = freeText2;
            this.FreeText3 = freeText3;
            this.FreeText4 = freeText4;
            this.FreeText5 = freeText5;
            this.Country = country;
            this.Validate();
        }

        [Required]
        public AddressFormat AddressFormat { get; protected set; }

        [Required]
        public string Country { get; protected set; }

        [Required]
        public string FreeText1 { get; protected set; }

        public string FreeText2 { get; protected set; }

        public string FreeText3 { get; protected set; }

        public string FreeText4 { get; protected set; }

        public string FreeText5 { get; protected set; }
    }
}