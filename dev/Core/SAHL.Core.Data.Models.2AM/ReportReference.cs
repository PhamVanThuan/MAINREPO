using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ReportReferenceDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ReportReferenceDataModel(int? reportStatementKey, string description)
        {
            this.ReportStatementKey = reportStatementKey;
            this.Description = description;
		
        }
		[JsonConstructor]
        public ReportReferenceDataModel(int reportReferenceKey, int? reportStatementKey, string description)
        {
            this.ReportReferenceKey = reportReferenceKey;
            this.ReportStatementKey = reportStatementKey;
            this.Description = description;
		
        }		

        public int ReportReferenceKey { get; set; }

        public int? ReportStatementKey { get; set; }

        public string Description { get; set; }

        public void SetKey(int key)
        {
            this.ReportReferenceKey =  key;
        }
    }
}