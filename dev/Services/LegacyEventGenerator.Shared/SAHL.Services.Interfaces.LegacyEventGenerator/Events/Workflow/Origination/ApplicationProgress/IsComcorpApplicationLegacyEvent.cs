using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.LegacyEventGenerator.Events.Workflow.Origination.ApplicationProgress
{
    public class IsComcorpApplicationLegacyEvent : LegacyEvent
    {
        public IsComcorpApplicationLegacyEvent(Guid id, DateTime date, int adUserKey, string aduserName, int applicationKey)
            : base(id, date, adUserKey, aduserName)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; protected set; }
    }
}
