using SAHL.Core.Events;
using System;

namespace SAHL.Services.Interfaces.ClientDomain.Events
{
    public class FixedPropertyAssetAddedToClientEvent : Event
    {
        public FixedPropertyAssetAddedToClientEvent(DateTime date, DateTime dateAcquired, int addressKey, double assetValue, double liabilityValue)
            : base(date)
        {
            this.DateAcquired = dateAcquired;
            this.AddressKey = addressKey;
            this.AssetValue = assetValue;
            this.LiabilityValue = liabilityValue;
        }

        public DateTime DateAcquired { get; protected set; }

        public int AddressKey { get; protected set; }

        public double AssetValue { get; protected set; }

        public double LiabilityValue { get; protected set; }
    }
}