using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class DocumentTypeDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public DocumentTypeDataModel(string description, bool? legalEntity, int? genericKeyTypeKey)
        {
            this.Description = description;
            this.LegalEntity = legalEntity;
            this.GenericKeyTypeKey = genericKeyTypeKey;
		
        }
		[JsonConstructor]
        public DocumentTypeDataModel(int documentTypeKey, string description, bool? legalEntity, int? genericKeyTypeKey)
        {
            this.DocumentTypeKey = documentTypeKey;
            this.Description = description;
            this.LegalEntity = legalEntity;
            this.GenericKeyTypeKey = genericKeyTypeKey;
		
        }		

        public int DocumentTypeKey { get; set; }

        public string Description { get; set; }

        public bool? LegalEntity { get; set; }

        public int? GenericKeyTypeKey { get; set; }

        public void SetKey(int key)
        {
            this.DocumentTypeKey =  key;
        }
    }
}