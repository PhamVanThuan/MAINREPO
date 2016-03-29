using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ResetDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ResetDataModel(DateTime resetDate, DateTime runDate, int resetConfigurationKey, double jIBARRate, double jIBARDiscountRate)
        {
            this.ResetDate = resetDate;
            this.RunDate = runDate;
            this.ResetConfigurationKey = resetConfigurationKey;
            this.JIBARRate = jIBARRate;
            this.JIBARDiscountRate = jIBARDiscountRate;
		
        }
		[JsonConstructor]
        public ResetDataModel(int resetKey, DateTime resetDate, DateTime runDate, int resetConfigurationKey, double jIBARRate, double jIBARDiscountRate)
        {
            this.ResetKey = resetKey;
            this.ResetDate = resetDate;
            this.RunDate = runDate;
            this.ResetConfigurationKey = resetConfigurationKey;
            this.JIBARRate = jIBARRate;
            this.JIBARDiscountRate = jIBARDiscountRate;
		
        }		

        public int ResetKey { get; set; }

        public DateTime ResetDate { get; set; }

        public DateTime RunDate { get; set; }

        public int ResetConfigurationKey { get; set; }

        public double JIBARRate { get; set; }

        public double JIBARDiscountRate { get; set; }

        public void SetKey(int key)
        {
            this.ResetKey =  key;
        }
    }
}