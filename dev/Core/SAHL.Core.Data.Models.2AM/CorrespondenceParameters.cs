using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class CorrespondenceParametersDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public CorrespondenceParametersDataModel(int correspondenceKey, int reportParameterKey, string reportParameterValue)
        {
            this.CorrespondenceKey = correspondenceKey;
            this.ReportParameterKey = reportParameterKey;
            this.ReportParameterValue = reportParameterValue;
		
        }
		[JsonConstructor]
        public CorrespondenceParametersDataModel(int correspondenceParameterKey, int correspondenceKey, int reportParameterKey, string reportParameterValue)
        {
            this.CorrespondenceParameterKey = correspondenceParameterKey;
            this.CorrespondenceKey = correspondenceKey;
            this.ReportParameterKey = reportParameterKey;
            this.ReportParameterValue = reportParameterValue;
		
        }		

        public int CorrespondenceParameterKey { get; set; }

        public int CorrespondenceKey { get; set; }

        public int ReportParameterKey { get; set; }

        public string ReportParameterValue { get; set; }

        public void SetKey(int key)
        {
            this.CorrespondenceParameterKey =  key;
        }
    }
}