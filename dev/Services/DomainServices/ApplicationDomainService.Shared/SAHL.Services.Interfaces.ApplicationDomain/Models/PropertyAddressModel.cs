using SAHL.Core.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.ApplicationDomain.Models
{
    public class PropertyAddressModel : ValidatableModel
    {
        public PropertyAddressModel(string unitNumber, string buildingName, string buildingNumber, string streetNumber, string streetName, string suburb, string city, string province, 
            string postalCode, string erfNumber, string erfPortionNumber)
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
            this.ErfNumber = erfNumber;
            this.ErfPortionNumber = erfPortionNumber;
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

        [Required]
        public string ErfNumber { get; protected set; }

        [Required]
        public string ErfPortionNumber { get; protected set; }
    }
}
