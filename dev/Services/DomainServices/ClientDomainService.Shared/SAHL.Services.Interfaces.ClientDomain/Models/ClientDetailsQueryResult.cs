
using System;
namespace SAHL.Services.Interfaces.ClientDomain.Models
{
    public class ClientDetailsQueryResult
    {
        public int LegalEntityKey { get; set; }

        public string FirstNames { get; set; }

        public string Surname { get; set; }

        public int MaritalStatusKey { get; set; }

        public int GenderKey { get; set; }

        public int SalutationKey { get; set; }

        public int PopulationGroupKey { get; set; }

        public int CitizenTypeKey { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string IDNumber { get; set; }

        public string PassportNumber { get; set; }

        public string HomePhoneCode { get; set; }

        public string HomePhoneNumber { get; set; }

        public string WorkPhoneCode { get; set; }

        public string WorkPhoneNumber { get; set; }

        public string CellPhoneNumber { get; set; }

        public string EmailAddress { get; set; }

    }
}
