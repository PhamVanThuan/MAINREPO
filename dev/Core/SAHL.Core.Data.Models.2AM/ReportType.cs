using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ReportTypeDataModel :  IDataModel
    {
        public ReportTypeDataModel(int reportTypeKey, string description)
        {
            this.ReportTypeKey = reportTypeKey;
            this.Description = description;
		
        }		

        public int ReportTypeKey { get; set; }

        public string Description { get; set; }
    }
}