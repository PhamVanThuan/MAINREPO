using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SAHL.Services.Capitec.Models.Shared
{
    [DataContract]
    public class SwitchLoanApplication : CapitecApplication
    {
        public SwitchLoanApplication(int applicationKey, int applicationStatusKey, DateTime applicationDate, SwitchLoanDetails switchLoanDetails, IEnumerable<Applicant> applicants, int employmentTypeKey, ConsultantDetails consultantDetails, IList<string> messages)
            : base(applicationKey, applicationStatusKey, applicationDate, applicants, consultantDetails, employmentTypeKey, messages)
        {
            this.SwitchLoanDetails = switchLoanDetails;
        }

        [DataMember]
        public SwitchLoanDetails SwitchLoanDetails { get; protected set; }
    }
}