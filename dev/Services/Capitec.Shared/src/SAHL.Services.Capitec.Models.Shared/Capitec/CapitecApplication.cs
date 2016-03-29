using SAHL.Core.Messaging.Shared;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SAHL.Services.Capitec.Models.Shared
{
    [DataContract]
    public abstract class CapitecApplication
    {
        public CapitecApplication(int reservedApplicationKey, int applicationStatusKey, DateTime applicationDate, IEnumerable<Applicant> applicants, ConsultantDetails consultantDetails, int employmentTypeKey, IList<string> messages)
        {
            this.ReservedApplicationKey = reservedApplicationKey;
            this.ApplicationStatusKey = applicationStatusKey;
            this.ApplicationDate = applicationDate;
            this.Applicants = applicants;
            this.ConsultantDetails = consultantDetails;
            this.EmploymentTypeKey = employmentTypeKey;
            this.Messages = messages;
        }

        [DataMember]
        public int ReservedApplicationKey { get; protected set; }

        [DataMember]
        public int ApplicationStatusKey { get; protected set; }

        [DataMember]
        public DateTime ApplicationDate { get; protected set; }

        [DataMember]
        public IEnumerable<Applicant> Applicants { get; protected set; }

        [DataMember]
        public ConsultantDetails ConsultantDetails { get; protected set; }

        [DataMember]
        public int EmploymentTypeKey { get; protected set; }

        [DataMember]
        public IList<string> Messages { get; protected set; }
    }
}