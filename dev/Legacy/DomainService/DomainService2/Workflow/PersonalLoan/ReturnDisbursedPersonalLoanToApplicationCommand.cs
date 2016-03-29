using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainService2.Workflow.PersonalLoan
{
   public class ReturnDisbursedPersonalLoanToApplicationCommand : StandardDomainServiceCommand
    {
       public ReturnDisbursedPersonalLoanToApplicationCommand(int applicationKey)
       {
           this.ApplicationKey = applicationKey;
       }

        public int ApplicationKey { get; set; }
    }
}
