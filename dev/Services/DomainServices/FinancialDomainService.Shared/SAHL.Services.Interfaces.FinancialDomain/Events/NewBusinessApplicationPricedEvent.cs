using SAHL.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.FinancialDomain.Events
{
    public class NewBusinessApplicationPricedEvent : Event
    {
        public int ApplicationNumber { get; protected set; }

        public NewBusinessApplicationPricedEvent(DateTime date, int ApplicationNumber)
            : base(date)
        {
            this.ApplicationNumber = ApplicationNumber;
        }
    }
}
