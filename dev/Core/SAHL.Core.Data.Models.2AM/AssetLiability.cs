using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AssetLiabilityDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public AssetLiabilityDataModel(int assetLiabilityTypeKey, int? assetLiabilitySubTypeKey, int? addressKey, double? assetValue, double? liabilityValue, string companyName, double? cost, DateTime? date, string description)
        {
            this.AssetLiabilityTypeKey = assetLiabilityTypeKey;
            this.AssetLiabilitySubTypeKey = assetLiabilitySubTypeKey;
            this.AddressKey = addressKey;
            this.AssetValue = assetValue;
            this.LiabilityValue = liabilityValue;
            this.CompanyName = companyName;
            this.Cost = cost;
            this.Date = date;
            this.Description = description;
		
        }
		[JsonConstructor]
        public AssetLiabilityDataModel(int assetLiabilityKey, int assetLiabilityTypeKey, int? assetLiabilitySubTypeKey, int? addressKey, double? assetValue, double? liabilityValue, string companyName, double? cost, DateTime? date, string description)
        {
            this.AssetLiabilityKey = assetLiabilityKey;
            this.AssetLiabilityTypeKey = assetLiabilityTypeKey;
            this.AssetLiabilitySubTypeKey = assetLiabilitySubTypeKey;
            this.AddressKey = addressKey;
            this.AssetValue = assetValue;
            this.LiabilityValue = liabilityValue;
            this.CompanyName = companyName;
            this.Cost = cost;
            this.Date = date;
            this.Description = description;
		
        }		

        public int AssetLiabilityKey { get; set; }

        public int AssetLiabilityTypeKey { get; set; }

        public int? AssetLiabilitySubTypeKey { get; set; }

        public int? AddressKey { get; set; }

        public double? AssetValue { get; set; }

        public double? LiabilityValue { get; set; }

        public string CompanyName { get; set; }

        public double? Cost { get; set; }

        public DateTime? Date { get; set; }

        public string Description { get; set; }

        public void SetKey(int key)
        {
            this.AssetLiabilityKey =  key;
        }
    }
}