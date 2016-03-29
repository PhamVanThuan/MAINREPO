using SAHL.Core.BusinessModel.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ClientDomain.Models
{
    public interface INaturalPersonClientModel : IClientContactDetails
    {
        string IDNumber { get; }

        string PassportNumber { get; }

        SalutationType? Salutation { get; }

        [Required]
        string FirstName { get; }

        [Required]
        string Surname { get; }

        string Initials { get; }

        string PreferredName { get; }

        Gender? Gender { get; }

        MaritalStatus? MaritalStatus { get; }

        PopulationGroup? PopulationGroup { get; }

        CitizenType? CitizenshipType { get; }

        DateTime? DateOfBirth { get; }

        Language? HomeLanguage { get; }

        CorrespondenceLanguage? CorrespondenceLanguage { get; }

        Education? Education { get; }
    }
}