using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ReportStatementDataModel :  IDataModel
    {
        public ReportStatementDataModel(int reportStatementKey, int? originationSourceProductKey, string reportName, string description, string statementName, string groupBy, string orderBy, int? reportGroupKey, int? featureKey, int? reportTypeKey, string reportOutputPath)
        {
            this.ReportStatementKey = reportStatementKey;
            this.OriginationSourceProductKey = originationSourceProductKey;
            this.ReportName = reportName;
            this.Description = description;
            this.StatementName = statementName;
            this.GroupBy = groupBy;
            this.OrderBy = orderBy;
            this.ReportGroupKey = reportGroupKey;
            this.FeatureKey = featureKey;
            this.ReportTypeKey = reportTypeKey;
            this.ReportOutputPath = reportOutputPath;
		
        }		

        public int ReportStatementKey { get; set; }

        public int? OriginationSourceProductKey { get; set; }

        public string ReportName { get; set; }

        public string Description { get; set; }

        public string StatementName { get; set; }

        public string GroupBy { get; set; }

        public string OrderBy { get; set; }

        public int? ReportGroupKey { get; set; }

        public int? FeatureKey { get; set; }

        public int? ReportTypeKey { get; set; }

        public string ReportOutputPath { get; set; }
    }
}