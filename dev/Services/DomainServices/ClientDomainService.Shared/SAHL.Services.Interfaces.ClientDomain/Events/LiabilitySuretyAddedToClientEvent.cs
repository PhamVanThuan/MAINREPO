using SAHL.Core.Events;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;

namespace SAHL.Services.Interfaces.ClientDomain.Events
{
    public class LiabilitySuretyAddedToClientEvent : Event
    {
        public LiabilitySuretyAddedToClientEvent(DateTime date, int suretyLiabilityKey, int clientSuretyLiabilityKey, double assetValue, double liabilityValue, string description)
            : base(date)
        {
            this.AssetValue = assetValue;
            this.LiabilityValue = liabilityValue;
            this.Description = description;
            this.SuretyLiabilityKey = suretyLiabilityKey;
            this.ClientSuretyLiabilityKey = clientSuretyLiabilityKey;
        }

        public double AssetValue { get; protected set; }

        public double LiabilityValue { get; protected set; }

        public string Description { get; protected set; }

        public int ClientSuretyLiabilityKey { get; protected set; }

        public int SuretyLiabilityKey { get; protected set; }
    }
}