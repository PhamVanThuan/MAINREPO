using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ReportParameterDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ReportParameterDataModel(int? reportStatementKey, string parameterName, string parameterType, int? parameterLength, string displayName, bool? required, int? parameterTypeKey, string domainFieldKey, string statementName)
        {
            this.ReportStatementKey = reportStatementKey;
            this.ParameterName = parameterName;
            this.ParameterType = parameterType;
            this.ParameterLength = parameterLength;
            this.DisplayName = displayName;
            this.Required = required;
            this.ParameterTypeKey = parameterTypeKey;
            this.DomainFieldKey = domainFieldKey;
            this.StatementName = statementName;
		
        }
		[JsonConstructor]
        public ReportParameterDataModel(int reportParameterKey, int? reportStatementKey, string parameterName, string parameterType, int? parameterLength, string displayName, bool? required, int? parameterTypeKey, string domainFieldKey, string statementName)
        {
            this.ReportParameterKey = reportParameterKey;
            this.ReportStatementKey = reportStatementKey;
            this.ParameterName = parameterName;
            this.ParameterType = parameterType;
            this.ParameterLength = parameterLength;
            this.DisplayName = displayName;
            this.Required = required;
            this.ParameterTypeKey = parameterTypeKey;
            this.DomainFieldKey = domainFieldKey;
            this.StatementName = statementName;
		
        }		

        public int ReportParameterKey { get; set; }

        public int? ReportStatementKey { get; set; }

        public string ParameterName { get; set; }

        public string ParameterType { get; set; }

        public int? ParameterLength { get; set; }

        public string DisplayName { get; set; }

        public bool? Required { get; set; }

        public int? ParameterTypeKey { get; set; }

        public string DomainFieldKey { get; set; }

        public string StatementName { get; set; }

        public void SetKey(int key)
        {
            this.ReportParameterKey =  key;
        }
    }
}