using System;
using System.Linq;

namespace SAHL.Services.Interfaces.DomainQuery.Model
{
    public class GetClientDetailsQueryResult
    {
        public int LegalEntityKey { get; set; }

        public string FirstNames { get; set; }

        public string Surname { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string IDNumber { get; set; }

        public string HomePhoneCode { get; set; }

        public string HomePhone { get; set; }

        public string WorkPhoneCode { get; set; }

        public string WorkPhone { get; set; }

        public string FaxCode { get; set; }

        public string FaxNumber { get; set; }

        public string Cellphone { get; set; }

        public string EmailAddress { get; set; }

        public int LegalEntityType { get; set; }
    }
}