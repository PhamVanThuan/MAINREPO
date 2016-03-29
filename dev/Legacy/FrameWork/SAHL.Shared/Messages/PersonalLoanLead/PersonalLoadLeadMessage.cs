using System.Collections.Generic;

namespace SAHL.Shared.Messages.PersonalLoanLead
{ 
    public class PersonalLoanLeadMessage : MessageBase, IPersonalLoanLeadMessage
    {
        protected PersonalLoanLeadMessage()
        {
        }

        public PersonalLoanLeadMessage(string idNumber, string application)
            : this(idNumber, application, "")
        {
        }

        public PersonalLoanLeadMessage(string idNumber, string application, string user = "")
            : base(application, user, (Dictionary<string, object>)null)
        {
            this.IdNumber = idNumber;
        }

        public virtual string IdNumber { get; set; }

        public virtual int BatchID { get; set; }

        public virtual int FailureCount { get; set; }
    }
}