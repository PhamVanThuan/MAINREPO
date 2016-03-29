using System;

namespace SAHL.Services.Interfaces.ITC.Models
{
    public class ApplicantITCRequestDetailsModel
    {
        public ApplicantITCRequestDetailsModel(string firstName, string surname, DateTime? dateOfBirth, string identityNumber, string title, string homePhoneNumber,
            string workPhoneNumber, string cellPhoneNumber, string emailAddress, string addressLine1, string addressLine2, string suburb, string city, string postalCode)
        {
            this.FirstName = firstName;
            this.Surname = surname;
            this.DateOfBirth = dateOfBirth;
            this.IdentityNumber = identityNumber;
            this.Title = title;
            this.HomePhoneNumber = homePhoneNumber;
            this.WorkPhoneNumber = workPhoneNumber;
            this.CellPhoneNumber = cellPhoneNumber;
            this.EmailAddress = emailAddress;
            this.AddressLine1 = addressLine1;
            this.AddressLine2 = addressLine2;
            this.Suburb = suburb;
            this.City = city;
            this.PostalCode = postalCode;
        }

        public string FirstName { get; protected set; }

        public string Surname { get; protected set; }

        public DateTime? DateOfBirth { get; protected set; }

        public string IdentityNumber { get; protected set; }

        public string Title { get; protected set; }

        public string HomePhoneNumber { get; protected set; }

        public string WorkPhoneNumber { get; protected set; }

        public string CellPhoneNumber { get; protected set; }

        public string EmailAddress { get; protected set; }

        public string AddressLine1 { get; protected set; }

        public string AddressLine2 { get; protected set; }

        public string Suburb { get; protected set; }

        public string City { get; protected set; }

        public string PostalCode { get; protected set; }
    }
}