using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Events;
using System;

namespace SAHL.Services.Interfaces.ApplicationDomain.Events
{
    public class ApplicationEmploymentTypeSetEvent : Event
    {
        public ApplicationEmploymentTypeSetEvent(DateTime date, int applicationNumber, int employmentTypeKey)
            : base(date)
        {
            this.ApplicationNumber = applicationNumber;
            this.EmploymentTypeKey = employmentTypeKey;
        }

        public int ApplicationNumber { get; protected set; }

        public int EmploymentTypeKey { get; protected set; }
    }
}