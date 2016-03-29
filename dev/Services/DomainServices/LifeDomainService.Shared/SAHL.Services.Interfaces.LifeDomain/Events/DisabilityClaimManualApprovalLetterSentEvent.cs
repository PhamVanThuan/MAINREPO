using SAHL.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.LifeDomain.Events
{
    public class DisabilityClaimManualApprovalLetterSentEvent : Event
    {
        public DisabilityClaimManualApprovalLetterSentEvent(DateTime date, int disabilityClaimKey, IEnumerable<string> furtherLendingExclusions, string emailAddress) 
            : base(date)
        {
            this.DisabilityClaimKey = disabilityClaimKey;
            this.FurtherLendingExclusions = furtherLendingExclusions;
            this.EmailAddress = emailAddress;
        }

        public int DisabilityClaimKey { get; protected set; }
        
        public IEnumerable<string> FurtherLendingExclusions { get; protected set; }

        public string EmailAddress { get; protected set; }
    }
}


