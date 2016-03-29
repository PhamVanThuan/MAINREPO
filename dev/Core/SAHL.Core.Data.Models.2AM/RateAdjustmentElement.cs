using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class RateAdjustmentElementDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public RateAdjustmentElementDataModel(double? elementMinValue, double? elementMaxValue, string elementText, double rateAdjustmentValue, DateTime effectiveDate, string description, int rateAdjustmentGroupKey, int rateAdjustmentElementTypeKey, int genericKeyTypeKey, int generalStatusKey, int ruleItemKey, int financialAdjustmentTypeSourceKey)
        {
            this.ElementMinValue = elementMinValue;
            this.ElementMaxValue = elementMaxValue;
            this.ElementText = elementText;
            this.RateAdjustmentValue = rateAdjustmentValue;
            this.EffectiveDate = effectiveDate;
            this.Description = description;
            this.RateAdjustmentGroupKey = rateAdjustmentGroupKey;
            this.RateAdjustmentElementTypeKey = rateAdjustmentElementTypeKey;
            this.GenericKeyTypeKey = genericKeyTypeKey;
            this.GeneralStatusKey = generalStatusKey;
            this.RuleItemKey = ruleItemKey;
            this.FinancialAdjustmentTypeSourceKey = financialAdjustmentTypeSourceKey;
		
        }
		[JsonConstructor]
        public RateAdjustmentElementDataModel(int rateAdjustmentElementKey, double? elementMinValue, double? elementMaxValue, string elementText, double rateAdjustmentValue, DateTime effectiveDate, string description, int rateAdjustmentGroupKey, int rateAdjustmentElementTypeKey, int genericKeyTypeKey, int generalStatusKey, int ruleItemKey, int financialAdjustmentTypeSourceKey)
        {
            this.RateAdjustmentElementKey = rateAdjustmentElementKey;
            this.ElementMinValue = elementMinValue;
            this.ElementMaxValue = elementMaxValue;
            this.ElementText = elementText;
            this.RateAdjustmentValue = rateAdjustmentValue;
            this.EffectiveDate = effectiveDate;
            this.Description = description;
            this.RateAdjustmentGroupKey = rateAdjustmentGroupKey;
            this.RateAdjustmentElementTypeKey = rateAdjustmentElementTypeKey;
            this.GenericKeyTypeKey = genericKeyTypeKey;
            this.GeneralStatusKey = generalStatusKey;
            this.RuleItemKey = ruleItemKey;
            this.FinancialAdjustmentTypeSourceKey = financialAdjustmentTypeSourceKey;
		
        }		

        public int RateAdjustmentElementKey { get; set; }

        public double? ElementMinValue { get; set; }

        public double? ElementMaxValue { get; set; }

        public string ElementText { get; set; }

        public double RateAdjustmentValue { get; set; }

        public DateTime EffectiveDate { get; set; }

        public string Description { get; set; }

        public int RateAdjustmentGroupKey { get; set; }

        public int RateAdjustmentElementTypeKey { get; set; }

        public int GenericKeyTypeKey { get; set; }

        public int GeneralStatusKey { get; set; }

        public int RuleItemKey { get; set; }

        public int FinancialAdjustmentTypeSourceKey { get; set; }

        public void SetKey(int key)
        {
            this.RateAdjustmentElementKey =  key;
        }
    }
}