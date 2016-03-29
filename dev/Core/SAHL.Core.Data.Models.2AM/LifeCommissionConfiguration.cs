using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class LifeCommissionConfigurationDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public LifeCommissionConfigurationDataModel(decimal maxCalls, decimal minCalls, decimal maxHits, decimal minHits, decimal targetCalls, decimal targetHits, decimal floorPrice, decimal commRate, decimal perScaleY1, decimal perScaleY2, decimal perScaleY3, DateTime? changeDate, string changeUser, int aDUserKey)
        {
            this.MaxCalls = maxCalls;
            this.MinCalls = minCalls;
            this.MaxHits = maxHits;
            this.MinHits = minHits;
            this.TargetCalls = targetCalls;
            this.TargetHits = targetHits;
            this.FloorPrice = floorPrice;
            this.CommRate = commRate;
            this.PerScaleY1 = perScaleY1;
            this.PerScaleY2 = perScaleY2;
            this.PerScaleY3 = perScaleY3;
            this.ChangeDate = changeDate;
            this.ChangeUser = changeUser;
            this.ADUserKey = aDUserKey;
		
        }
		[JsonConstructor]
        public LifeCommissionConfigurationDataModel(int commissionConfigurationKey, decimal maxCalls, decimal minCalls, decimal maxHits, decimal minHits, decimal targetCalls, decimal targetHits, decimal floorPrice, decimal commRate, decimal perScaleY1, decimal perScaleY2, decimal perScaleY3, DateTime? changeDate, string changeUser, int aDUserKey)
        {
            this.CommissionConfigurationKey = commissionConfigurationKey;
            this.MaxCalls = maxCalls;
            this.MinCalls = minCalls;
            this.MaxHits = maxHits;
            this.MinHits = minHits;
            this.TargetCalls = targetCalls;
            this.TargetHits = targetHits;
            this.FloorPrice = floorPrice;
            this.CommRate = commRate;
            this.PerScaleY1 = perScaleY1;
            this.PerScaleY2 = perScaleY2;
            this.PerScaleY3 = perScaleY3;
            this.ChangeDate = changeDate;
            this.ChangeUser = changeUser;
            this.ADUserKey = aDUserKey;
		
        }		

        public int CommissionConfigurationKey { get; set; }

        public decimal MaxCalls { get; set; }

        public decimal MinCalls { get; set; }

        public decimal MaxHits { get; set; }

        public decimal MinHits { get; set; }

        public decimal TargetCalls { get; set; }

        public decimal TargetHits { get; set; }

        public decimal FloorPrice { get; set; }

        public decimal CommRate { get; set; }

        public decimal PerScaleY1 { get; set; }

        public decimal PerScaleY2 { get; set; }

        public decimal PerScaleY3 { get; set; }

        public DateTime? ChangeDate { get; set; }

        public string ChangeUser { get; set; }

        public int ADUserKey { get; set; }

        public void SetKey(int key)
        {
            this.CommissionConfigurationKey =  key;
        }
    }
}