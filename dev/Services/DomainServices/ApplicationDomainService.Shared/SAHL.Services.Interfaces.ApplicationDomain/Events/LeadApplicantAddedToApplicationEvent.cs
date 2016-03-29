using System;
using SAHL.Core.Events;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Core.BusinessModel.Enums;

namespace SAHL.Services.Interfaces.ApplicationDomain.Events
{
    public class LeadApplicantAddedToApplicationEvent : Event
    {
        public LeadApplicantAddedToApplicationEvent(DateTime date, int applicationNumber, int clientKey, int applicationRoleKey)
            : base(date)
        {
            this.ApplicationNumber = applicationNumber;
            this.ClientKey = clientKey;      
            this.ApplicationRoleKey = applicationRoleKey;
        }

        public int ApplicationNumber { get; protected set; }

        public int ClientKey { get; protected set; }

        public int ApplicationRoleKey { get; protected set; }

    }
}