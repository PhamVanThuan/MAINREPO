using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ReportParameterTypeDataModel :  IDataModel
    {
        public ReportParameterTypeDataModel(int reportParameterTypeKey, string description)
        {
            this.ReportParameterTypeKey = reportParameterTypeKey;
            this.Description = description;
		
        }		

        public int ReportParameterTypeKey { get; set; }

        public string Description { get; set; }
    }
}