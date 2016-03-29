using System;
using System.Runtime.Serialization;

namespace SAHL.Services.Capitec.Models.Shared
{
    [DataContract]
    public class ApplicantInformation
    {
        public ApplicantInformation(string identityNumber, string firstName, string surname, int salutationTypeKey, string homePhoneNumber, string workPhoneNumber, string cellPhoneNumber, string emailAddress, DateTime? dateOfBirth, string title, bool mainContact)
        {
            this.IdentityNumber = identityNumber;
            this.FirstName = firstName;
            this.Surname = surname;
            this.SalutationTypeKey = salutationTypeKey;
            this.HomePhoneNumber = homePhoneNumber;
            this.WorkPhoneNumber = workPhoneNumber;
            this.CellPhoneNumber = cellPhoneNumber;
            this.EmailAddress = emailAddress;
            this.DateOfBirth = dateOfBirth;
            this.Title = title;
            this.MainContact = mainContact;
        }

        [DataMember]
        public string IdentityNumber { get; protected set; }

        [DataMember]
        public string FirstName { get; protected set; }

        [DataMember]
        public string Surname { get; protected set; }

        [DataMember]
        public int SalutationTypeKey { get; protected set; }

        [DataMember]
        public string Title { get; protected set; }

        [DataMember]
        public string HomePhoneNumber { get; protected set; }

        [DataMember]
        public string WorkPhoneNumber { get; protected set; }

        [DataMember]
        public string CellPhoneNumber { get; protected set; }

        [DataMember]
        public string EmailAddress { get; protected set; }

        [DataMember]
        public DateTime? DateOfBirth { get; protected set; }

        [DataMember]
        public bool MainContact { get; protected set; }
    }
}