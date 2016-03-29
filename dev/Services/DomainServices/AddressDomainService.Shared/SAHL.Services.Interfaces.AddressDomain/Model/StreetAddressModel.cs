using SAHL.Core.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SAHL.Services.Interfaces.AddressDomain.Model
{
    public class StreetAddressModel : ValidatableModel
    {
        public StreetAddressModel(string unitNumber, string buildingName, string buildingNumber, string streetNumber, string streetName, string suburb, string city, string province, string postalCode)
        {
            this.BuildingName = buildingName;
            this.BuildingNumber = buildingNumber;
            this.City = city;
            this.PostalCode = postalCode;
            this.Province = province;
            this.StreetName = streetName;
            this.StreetNumber = streetNumber;
            this.Suburb = suburb;
            this.UnitNumber = unitNumber;
            Validate();
        }

        public string UnitNumber { get; protected set; }

        public string BuildingNumber { get; protected set; }

        public string BuildingName { get; protected set; }

        [Required]
        public string StreetNumber { get; protected set; }

        [Required]
        public string StreetName { get; protected set; }

        [Required]
        public string Suburb { get; protected set; }

        [Required]
        public string City { get; protected set; }

        [Required]
        public string Province { get; protected set; }

        [Required]
        public string PostalCode { get; protected set; }
    }
}