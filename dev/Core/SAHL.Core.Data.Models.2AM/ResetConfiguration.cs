using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ResetConfigurationDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ResetConfigurationDataModel(string intervalType, int intervalDuration, DateTime resetDate, DateTime actionDate, string businessDayIndicator, string description)
        {
            this.IntervalType = intervalType;
            this.IntervalDuration = intervalDuration;
            this.ResetDate = resetDate;
            this.ActionDate = actionDate;
            this.BusinessDayIndicator = businessDayIndicator;
            this.Description = description;
		
        }
		[JsonConstructor]
        public ResetConfigurationDataModel(int resetConfigurationKey, string intervalType, int intervalDuration, DateTime resetDate, DateTime actionDate, string businessDayIndicator, string description)
        {
            this.ResetConfigurationKey = resetConfigurationKey;
            this.IntervalType = intervalType;
            this.IntervalDuration = intervalDuration;
            this.ResetDate = resetDate;
            this.ActionDate = actionDate;
            this.BusinessDayIndicator = businessDayIndicator;
            this.Description = description;
		
        }		

        public int ResetConfigurationKey { get; set; }

        public string IntervalType { get; set; }

        public int IntervalDuration { get; set; }

        public DateTime ResetDate { get; set; }

        public DateTime ActionDate { get; set; }

        public string BusinessDayIndicator { get; set; }

        public string Description { get; set; }

        public void SetKey(int key)
        {
            this.ResetConfigurationKey =  key;
        }
    }
}