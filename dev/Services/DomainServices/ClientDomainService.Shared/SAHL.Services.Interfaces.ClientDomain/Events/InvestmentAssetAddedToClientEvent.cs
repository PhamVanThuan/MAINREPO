using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Events;
using System;

namespace SAHL.Services.Interfaces.ClientDomain.Events
{
    public class InvestmentAssetAddedToClientEvent : Event
    {
        public InvestmentAssetAddedToClientEvent(DateTime date, AssetInvestmentType investmentType, string companyName, double assetValue)
            : base(date)
        {
            this.InvestmentType = investmentType;
            this.CompanyName = companyName;
            this.AssetValue = assetValue;
        }

        public AssetInvestmentType InvestmentType { get; protected set; }

        public string CompanyName { get; protected set; }

        public double AssetValue { get; protected set; }
    }
}
