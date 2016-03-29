using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class InterestRecalc1AugDataModel :  IDataModel
    {
        public InterestRecalc1AugDataModel(int financialServiceKey, double mayInterest, double juneInterest, double lBMay, double lBJune, double? junediff)
        {
            this.FinancialServiceKey = financialServiceKey;
            this.MayInterest = mayInterest;
            this.JuneInterest = juneInterest;
            this.LBMay = lBMay;
            this.LBJune = lBJune;
            this.Junediff = junediff;
		
        }		

        public int FinancialServiceKey { get; set; }

        public double MayInterest { get; set; }

        public double JuneInterest { get; set; }

        public double LBMay { get; set; }

        public double LBJune { get; set; }

        public double? Junediff { get; set; }
    }
}