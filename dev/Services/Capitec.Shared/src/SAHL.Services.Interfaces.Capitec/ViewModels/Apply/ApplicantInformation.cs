using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.Capitec.ViewModels.Apply
{
    public class ApplicantInformation
    {
        public ApplicantInformation(string identityNumber, string firstName, string surname, Guid salutationEnumId, string homePhoneNumber, string workPhoneNumber, string cellPhoneNumber, string emailAddress, DateTime? dateOfBirth, string title, bool mainContact)
        {
            this.IdentityNumber = identityNumber;
            this.FirstName = firstName;
            this.Surname = surname;
            this.SalutationEnumId = salutationEnumId;
            this.HomePhoneNumber = homePhoneNumber;
            this.WorkPhoneNumber = workPhoneNumber;
            this.CellPhoneNumber = cellPhoneNumber;
            this.EmailAddress = emailAddress;
            this.DateOfBirth = dateOfBirth;
            this.Title = title;
            this.MainContact = mainContact;
        }

        [RegularExpression("\\d{13}", ErrorMessage = "Please provide a valid South African ID Number.")]
        public string IdentityNumber { get; protected set; }

        [Required]
        public string FirstName { get; protected set; }

        [Required]
        public string Surname { get; protected set; }

        [Required(ErrorMessage = "A Title is required")]
        public Guid SalutationEnumId { get; protected set; }

        public string Title { get; protected set; }

        [RegularExpression("^$|0[0-8]\\d{8}", ErrorMessage = "Please provide a valid home phone number.")]
        public string HomePhoneNumber { get; protected set; }

        [RegularExpression("^$|0[0-8]\\d{8}", ErrorMessage = "Please provide a valid work phone number.")]
        public string WorkPhoneNumber { get; protected set; }

        [RegularExpression("0[0-8]\\d{8}", ErrorMessage = "Please provide a valid cellphone number.")]
        public string CellPhoneNumber { get; protected set; }

        [RegularExpression("(^$)|([A-Za-z0-9_\\-\\.])+\\@([A-Za-z0-9_\\-\\.])+\\.([A-Za-z]{2,4})", ErrorMessage = "Please provide a valid email address.")]
        public string EmailAddress { get; protected set; }

        [RegularExpression("^(?:(?:(\\d{4}[^\\d](?:(1[012])|(0?[13456789]))[^\\d]\\d{1,2})|(\\d{1,2}[^\\d](?:(1[012])|(0?[13456789]))[^\\d]\\d{4}))|(?:(?:\\d{4}[^\\d](?:0?2)[^\\d]((0[1-9])|([12][0-9])))|(?:((0[1-9])|([12][0-9]))[^\\d](?:0?2)[^\\d]\\d{4}))).*$", ErrorMessage = "Please provide a valid date of Birth")]
        public DateTime? DateOfBirth { get; protected set; }

        public bool MainContact { get; protected set; }
    }
}
