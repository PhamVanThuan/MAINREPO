using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainService2.Workflow.PersonalLoan
{
    public class SendSMSToApplicantUponDisbursementCommand : StandardDomainServiceCommand
    {
        public int ApplicationKey { get; protected set; }

        public SendSMSToApplicantUponDisbursementCommand(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }
    }
}
