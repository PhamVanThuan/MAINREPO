using SAHL.Core.Events;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Core.BusinessModel.Enums;

namespace SAHL.Services.Interfaces.ApplicationDomain.Events
{
    public class ExternalOriginatorAttributeSetEvent : Event
    {
        public ExternalOriginatorAttributeSetEvent(DateTime date, int originationSourceKey, int applicationNumber, OfferAttributeType applicationAttributeType)
            : base(date)
        {
            this.ApplicationNumber = applicationNumber;
            this.ApplicationAttributeType = applicationAttributeType;
            this.OriginationSourceKey = originationSourceKey;
        }

        public int ApplicationNumber { get; protected set; }

        public OfferAttributeType ApplicationAttributeType { get; protected set; }

        public int OriginationSourceKey { get; protected set; }
    }
}
