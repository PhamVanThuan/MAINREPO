using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class CorrespondenceMediumReportStatementDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public CorrespondenceMediumReportStatementDataModel(int reportStatementKey, int correspondenceMediumKey)
        {
            this.ReportStatementKey = reportStatementKey;
            this.CorrespondenceMediumKey = correspondenceMediumKey;
		
        }
		[JsonConstructor]
        public CorrespondenceMediumReportStatementDataModel(int correspondenceMediumReportStatementKey, int reportStatementKey, int correspondenceMediumKey)
        {
            this.CorrespondenceMediumReportStatementKey = correspondenceMediumReportStatementKey;
            this.ReportStatementKey = reportStatementKey;
            this.CorrespondenceMediumKey = correspondenceMediumKey;
		
        }		

        public int CorrespondenceMediumReportStatementKey { get; set; }

        public int ReportStatementKey { get; set; }

        public int CorrespondenceMediumKey { get; set; }

        public void SetKey(int key)
        {
            this.CorrespondenceMediumReportStatementKey =  key;
        }
    }
}