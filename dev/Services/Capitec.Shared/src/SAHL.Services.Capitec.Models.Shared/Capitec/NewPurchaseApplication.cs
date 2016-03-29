using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SAHL.Services.Capitec.Models.Shared
{
    [DataContract]   
    public class NewPurchaseApplication : CapitecApplication
    {
        public NewPurchaseApplication(int applicationKey, int applicationStatusKey, DateTime applicationDate, NewPurchaseLoanDetails newPurchaseLoanDetails, IEnumerable<Applicant> applicants, int employmentTypeKey, ConsultantDetails consultantDetails, IList<string> messages)
            : base(applicationKey, applicationStatusKey, applicationDate, applicants, consultantDetails, employmentTypeKey, messages)
        {
            this.NewPurchaseLoanDetails = newPurchaseLoanDetails;
        }

        [DataMember]
        public NewPurchaseLoanDetails NewPurchaseLoanDetails { get; protected set; }
    }
}