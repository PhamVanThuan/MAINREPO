using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ReportStatementDocumentTemplateDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ReportStatementDocumentTemplateDataModel(int reportStatementKey, int documentTemplateKey, int templateGenerationOrder)
        {
            this.ReportStatementKey = reportStatementKey;
            this.DocumentTemplateKey = documentTemplateKey;
            this.TemplateGenerationOrder = templateGenerationOrder;
		
        }
		[JsonConstructor]
        public ReportStatementDocumentTemplateDataModel(int reportStatementDocumentTemplateKey, int reportStatementKey, int documentTemplateKey, int templateGenerationOrder)
        {
            this.ReportStatementDocumentTemplateKey = reportStatementDocumentTemplateKey;
            this.ReportStatementKey = reportStatementKey;
            this.DocumentTemplateKey = documentTemplateKey;
            this.TemplateGenerationOrder = templateGenerationOrder;
		
        }		

        public int ReportStatementDocumentTemplateKey { get; set; }

        public int ReportStatementKey { get; set; }

        public int DocumentTemplateKey { get; set; }

        public int TemplateGenerationOrder { get; set; }

        public void SetKey(int key)
        {
            this.ReportStatementDocumentTemplateKey =  key;
        }
    }
}