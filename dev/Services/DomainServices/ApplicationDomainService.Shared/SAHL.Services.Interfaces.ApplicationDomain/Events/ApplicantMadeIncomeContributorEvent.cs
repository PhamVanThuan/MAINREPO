using SAHL.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.ApplicationDomain.Events
{
    public class ApplicantMadeIncomeContributorEvent : Event
    {
        public ApplicantMadeIncomeContributorEvent(DateTime date, int applicationRoleKey)
            : base(date)
        {
            this.ApplicationRoleKey = applicationRoleKey;
        }
        public int ApplicationRoleKey
        {
            get;
            protected set;
        }
    }
}
