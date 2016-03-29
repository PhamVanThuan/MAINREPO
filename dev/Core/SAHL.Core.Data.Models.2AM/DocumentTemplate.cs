using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class DocumentTemplateDataModel :  IDataModel
    {
        public DocumentTemplateDataModel(int documentTemplateKey, string description, string path, string applicationName, string statementName)
        {
            this.DocumentTemplateKey = documentTemplateKey;
            this.Description = description;
            this.Path = path;
            this.ApplicationName = applicationName;
            this.StatementName = statementName;
		
        }		

        public int DocumentTemplateKey { get; set; }

        public string Description { get; set; }

        public string Path { get; set; }

        public string ApplicationName { get; set; }

        public string StatementName { get; set; }
    }
}