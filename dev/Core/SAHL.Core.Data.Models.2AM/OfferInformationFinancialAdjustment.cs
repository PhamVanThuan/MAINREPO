using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OfferInformationFinancialAdjustmentDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public OfferInformationFinancialAdjustmentDataModel(int offerInformationKey, int financialAdjustmentTypeSourceKey, int? term, double? capRate, double? cAPBalance, double? floorRate, double? fixedRate, double? discount, DateTime? fromDate)
        {
            this.OfferInformationKey = offerInformationKey;
            this.FinancialAdjustmentTypeSourceKey = financialAdjustmentTypeSourceKey;
            this.Term = term;
            this.CapRate = capRate;
            this.CAPBalance = cAPBalance;
            this.FloorRate = floorRate;
            this.FixedRate = fixedRate;
            this.Discount = discount;
            this.FromDate = fromDate;
		
        }
		[JsonConstructor]
        public OfferInformationFinancialAdjustmentDataModel(int offerInformationFinancialAdjustmentKey, int offerInformationKey, int financialAdjustmentTypeSourceKey, int? term, double? capRate, double? cAPBalance, double? floorRate, double? fixedRate, double? discount, DateTime? fromDate)
        {
            this.OfferInformationFinancialAdjustmentKey = offerInformationFinancialAdjustmentKey;
            this.OfferInformationKey = offerInformationKey;
            this.FinancialAdjustmentTypeSourceKey = financialAdjustmentTypeSourceKey;
            this.Term = term;
            this.CapRate = capRate;
            this.CAPBalance = cAPBalance;
            this.FloorRate = floorRate;
            this.FixedRate = fixedRate;
            this.Discount = discount;
            this.FromDate = fromDate;
		
        }		

        public int OfferInformationFinancialAdjustmentKey { get; set; }

        public int OfferInformationKey { get; set; }

        public int FinancialAdjustmentTypeSourceKey { get; set; }

        public int? Term { get; set; }

        public double? CapRate { get; set; }

        public double? CAPBalance { get; set; }

        public double? FloorRate { get; set; }

        public double? FixedRate { get; set; }

        public double? Discount { get; set; }

        public DateTime? FromDate { get; set; }

        public void SetKey(int key)
        {
            this.OfferInformationFinancialAdjustmentKey =  key;
        }
    }
}