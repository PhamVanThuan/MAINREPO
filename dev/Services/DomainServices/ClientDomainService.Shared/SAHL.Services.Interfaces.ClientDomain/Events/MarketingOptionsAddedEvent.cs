using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Events;
using System;
using System.Collections.Generic;
using SAHL.Services.Interfaces.ClientDomain.Models;

namespace SAHL.Services.Interfaces.ClientDomain.Events
{
    public class MarketingOptionsAddedEvent : Event
    {
        public IEnumerable<MarketingOptionModel> MarketingOptionCollection { get; protected set; }

        public MarketingOptionsAddedEvent(DateTime date, IEnumerable<MarketingOptionModel> marketingOptionCollection)
                : base(date)
        {
            this.MarketingOptionCollection = marketingOptionCollection;
        }
    }
}