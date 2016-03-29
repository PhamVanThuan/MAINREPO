using SAHL.Core.Events;
using System;

namespace SAHL.Services.Interfaces.ClientDomain.Events
{
    public class OtherAssetAddedToClientEvent : Event
    {
        public OtherAssetAddedToClientEvent(DateTime date, int ClientAssetLiabilityKey, string description, double assetValue, double liabilityValue)
            : base(date)
        {
            this.ClientAssetLiabilityKey = ClientAssetLiabilityKey;
            this.Description = description;
            this.AssetValue = assetValue;
            this.LiabilityValue = liabilityValue;
        }

        public int ClientAssetLiabilityKey { get; protected set; }

        public string Description { get; protected set; }

        public double AssetValue { get; protected set; }

        public double LiabilityValue { get; protected set; }
    }
}