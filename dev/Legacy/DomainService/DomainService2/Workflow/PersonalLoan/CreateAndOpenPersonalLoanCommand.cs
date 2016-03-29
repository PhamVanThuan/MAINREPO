using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainService2.Workflow.PersonalLoan
{
    public class CreateAndOpenPersonalLoanCommand : StandardDomainServiceCommand
    {

        public CreateAndOpenPersonalLoanCommand(int applicationKey, string userID)
        {
            this.ApplicationKey = applicationKey;
            this.UserID = userID;
        }

        public int ApplicationKey { get; set; }

        public string UserID { get; set; }
    }
}
