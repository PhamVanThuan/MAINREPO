using SAHL.Core.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.DocumentManager.Models
{
    public class ClientFileDocumentModel : ValidatableModel
    {
        [Required(ErrorMessage ="Document is required.")]
        public string Document { get; protected set; }

        [Required(ErrorMessage ="A category must be specified.")]
        public string Category { get; protected set; }

        [Required(ErrorMessage ="ID Number is required.")]
        public string IdentityNumber { get; protected set; }

        public string FirstName { get; protected set; }

        public string Surname { get; protected set; }

        public string Username { get; protected set; }

        public DateTime DocumentSubmitDate { get; protected set; }

        [Required(ErrorMessage ="A file extension is required.")]
        public FileExtension OriginalExtension { get; protected set; }

        public ClientFileDocumentModel(string document, string category, string identityNumber, string firstName, string surname, string username, 
            DateTime documentSubmitDate, FileExtension originalExtension)
        {
            this.Document = document;
            this.Category = category;
            this.IdentityNumber = identityNumber;
            this.FirstName = firstName;
            this.Surname = surname;
            this.Username = username;
            this.DocumentSubmitDate = documentSubmitDate;
            this.OriginalExtension = originalExtension;

            this.Validate();
        }
    }
}