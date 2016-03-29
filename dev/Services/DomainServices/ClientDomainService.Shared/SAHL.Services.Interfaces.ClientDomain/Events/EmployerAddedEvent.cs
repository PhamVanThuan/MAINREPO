using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Events;
using System;

namespace SAHL.Services.Interfaces.ClientDomain.Events
{
    public class EmployerAddedEvent : Event
    {
        public EmployerAddedEvent(DateTime date, int employerKey, string employerName, string telephoneCode, string telephoneNumber,
            string contactPerson, string contactEmail, EmployerBusinessType employerBusinessType, EmploymentSector employmentSector)
            : base(date)
        {
            this.EmployerKey = employerKey;
            this.EmployerName = employerName;
            this.TelephoneCode = telephoneCode;
            this.TelephoneNumber = telephoneNumber;
            this.ContactPerson = contactPerson;
            this.ContactEmail = contactEmail;
            this.EmployerBusinessType = employerBusinessType;
            this.EmploymentSector = employmentSector;
        }

        public int EmployerKey { get; protected set; }

        public string EmployerName { get; protected set; }

        public string TelephoneNumber { get; protected set; }

        public string TelephoneCode { get; protected set; }

        public string ContactPerson { get; protected set; }

        public string ContactEmail { get; protected set; }

        public EmployerBusinessType EmployerBusinessType { get; protected set; }

        public EmploymentSector EmploymentSector { get; protected set; }
    }
}