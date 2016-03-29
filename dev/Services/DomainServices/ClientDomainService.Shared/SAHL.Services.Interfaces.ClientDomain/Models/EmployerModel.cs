using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SAHL.Services.Interfaces.ClientDomain.Models
{
    public class EmployerModel : ValidatableModel
    {
        public EmployerModel(int? employerKey, string employerName, string telephoneCode, string telephoneNumber, string contactPerson, string contactEmail, 
            EmployerBusinessType employerBusinessType, EmploymentSector employmentSector)
        {
            this.EmployerName = employerName;
            this.TelephoneCode = telephoneCode;
            this.TelephoneNumber = telephoneNumber;
            this.ContactPerson = contactPerson;
            this.ContactEmail = contactEmail;
            this.EmployerBusinessType = employerBusinessType;
            this.EmploymentSector = employmentSector;
            this.EmployerKey = employerKey;
            Validate();
        }

        public int? EmployerKey { get; protected set;}

        [Required]
        public string EmployerName { get; protected set; }

        public string TelephoneNumber { get; protected set; }

        public string TelephoneCode { get; protected set; }

        public string ContactPerson { get; protected set; }

        public string ContactEmail { get; protected set; }

        [Required]
        public EmployerBusinessType EmployerBusinessType { get; protected set; }

        [Required]
        public EmploymentSector EmploymentSector { get; protected set; }
    }
}