using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class HOCRatesDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public HOCRatesDataModel(int hOCInsurerKey, int? hOCSubsidenceKey, double thatchPremium, double conventionalPremium, double shinglePremium, decimal? sASRIAFactor, decimal? minSASRIAAmount, decimal? adminFee, decimal? annualEscalation, bool? isActive)
        {
            this.HOCInsurerKey = hOCInsurerKey;
            this.HOCSubsidenceKey = hOCSubsidenceKey;
            this.ThatchPremium = thatchPremium;
            this.ConventionalPremium = conventionalPremium;
            this.ShinglePremium = shinglePremium;
            this.SASRIAFactor = sASRIAFactor;
            this.MinSASRIAAmount = minSASRIAAmount;
            this.AdminFee = adminFee;
            this.AnnualEscalation = annualEscalation;
            this.IsActive = isActive;
		
        }
		[JsonConstructor]
        public HOCRatesDataModel(int hOCRatesKey, int hOCInsurerKey, int? hOCSubsidenceKey, double thatchPremium, double conventionalPremium, double shinglePremium, decimal? sASRIAFactor, decimal? minSASRIAAmount, decimal? adminFee, decimal? annualEscalation, bool? isActive)
        {
            this.HOCRatesKey = hOCRatesKey;
            this.HOCInsurerKey = hOCInsurerKey;
            this.HOCSubsidenceKey = hOCSubsidenceKey;
            this.ThatchPremium = thatchPremium;
            this.ConventionalPremium = conventionalPremium;
            this.ShinglePremium = shinglePremium;
            this.SASRIAFactor = sASRIAFactor;
            this.MinSASRIAAmount = minSASRIAAmount;
            this.AdminFee = adminFee;
            this.AnnualEscalation = annualEscalation;
            this.IsActive = isActive;
		
        }		

        public int HOCRatesKey { get; set; }

        public int HOCInsurerKey { get; set; }

        public int? HOCSubsidenceKey { get; set; }

        public double ThatchPremium { get; set; }

        public double ConventionalPremium { get; set; }

        public double ShinglePremium { get; set; }

        public decimal? SASRIAFactor { get; set; }

        public decimal? MinSASRIAAmount { get; set; }

        public decimal? AdminFee { get; set; }

        public decimal? AnnualEscalation { get; set; }

        public bool? IsActive { get; set; }

        public void SetKey(int key)
        {
            this.HOCRatesKey =  key;
        }
    }
}