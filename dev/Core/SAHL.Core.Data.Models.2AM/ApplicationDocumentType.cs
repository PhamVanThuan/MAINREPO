using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ApplicationDocumentTypeDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ApplicationDocumentTypeDataModel(string description)
        {
            this.Description = description;
		
        }
		[JsonConstructor]
        public ApplicationDocumentTypeDataModel(int applicationDocumentTypeKey, string description)
        {
            this.ApplicationDocumentTypeKey = applicationDocumentTypeKey;
            this.Description = description;
		
        }		

        public int ApplicationDocumentTypeKey { get; set; }

        public string Description { get; set; }

        public void SetKey(int key)
        {
            this.ApplicationDocumentTypeKey =  key;
        }
    }
}