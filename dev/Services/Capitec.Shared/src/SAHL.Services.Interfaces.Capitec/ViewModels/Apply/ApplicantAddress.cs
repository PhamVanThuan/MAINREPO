using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.Capitec.ViewModels.Apply
{
    public class ApplicantResidentialAddress
    {
        public ApplicantResidentialAddress(string unitNumber, string buildingNumber, string buildingName, string streetNumber, string streetName, string suburb, string province, string city, string postalCode, Guid suburbId)
        {
            this.UnitNumber = unitNumber;
            this.BuildingNumber = buildingNumber;
            this.BuildingName = buildingName;
            this.StreetNumber = streetNumber;
            this.StreetName = streetName;
            this.Suburb = suburb;
            this.Province = province;
            this.City = city;
            this.PostalCode = postalCode;
            this.SuburbId = suburbId;
        }

        [MaxLength(15, ErrorMessage="Unit Number cannot be longer than 15 characters.")]
        public string UnitNumber { get; protected set; }

        [MaxLength(10, ErrorMessage = "Building Number cannot be longer than 10 characters.")]
        public string BuildingNumber { get; protected set; }

        [MaxLength(50, ErrorMessage = "Building Name cannot be longer than 50 characters.")]
        public string BuildingName { get; protected set; }

        [Required]
        [MaxLength(10, ErrorMessage = "Street Number cannot be longer than 10 characters.")]
        public string StreetNumber { get; protected set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Street Name cannot be longer than 50 characters.")]
        public string StreetName { get; protected set; }

        [Required]
        public string Suburb { get; protected set; }

        public Guid SuburbId { get; protected set; }

        [Required]
        public string Province { get; protected set; }

        public string City { get; protected set; }

        public string PostalCode { get; protected set; }
    }
}
