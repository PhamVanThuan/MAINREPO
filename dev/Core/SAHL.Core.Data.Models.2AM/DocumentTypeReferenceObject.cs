using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class DocumentTypeReferenceObjectDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public DocumentTypeReferenceObjectDataModel(int documentTypeKey, int genericKeyTypeKey)
        {
            this.DocumentTypeKey = documentTypeKey;
            this.GenericKeyTypeKey = genericKeyTypeKey;
		
        }
		[JsonConstructor]
        public DocumentTypeReferenceObjectDataModel(int documentTypeReferenceObjectKey, int documentTypeKey, int genericKeyTypeKey)
        {
            this.DocumentTypeReferenceObjectKey = documentTypeReferenceObjectKey;
            this.DocumentTypeKey = documentTypeKey;
            this.GenericKeyTypeKey = genericKeyTypeKey;
		
        }		

        public int DocumentTypeReferenceObjectKey { get; set; }

        public int DocumentTypeKey { get; set; }

        public int GenericKeyTypeKey { get; set; }

        public void SetKey(int key)
        {
            this.DocumentTypeReferenceObjectKey =  key;
        }
    }
}