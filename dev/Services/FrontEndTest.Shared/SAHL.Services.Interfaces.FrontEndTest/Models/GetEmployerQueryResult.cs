using SAHL.Core.BusinessModel.Enums;

namespace SAHL.Services.Interfaces.FrontEndTest.Models
{
    public class GetEmployerQueryResult
    {
        public GetEmployerQueryResult(int employerKey, string name, string telephoneCode, string telephoneNumber, string contactPerson,
            string contactEmail, EmployerBusinessType employerBusinessType, EmploymentSector employmentSector)
        {
            this.EmployerKey = employerKey;
            this.Name = name;
            this.TelephoneCode = telephoneCode;
            this.TelephoneNumber = telephoneNumber;
            this.ContactPerson = contactPerson;
            this.ContactEmail = contactEmail;
            this.EmployerBusinessType = employerBusinessType;
            this.EmploymentSector = employmentSector;
        }

        public string ContactEmail { get; protected set; }

        public string ContactPerson { get; protected set; }

        public EmployerBusinessType EmployerBusinessType { get; protected set; }

        public int EmployerKey { get; protected set; }

        public string Name { get; protected set; }

        public EmploymentSector EmploymentSector { get; protected set; }

        public string TelephoneCode { get; protected set; }

        public string TelephoneNumber { get; protected set; }
    }
}