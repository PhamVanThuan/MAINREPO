using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class DocumentSourceTypeDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public DocumentSourceTypeDataModel(string description, int genericKeyTypeKey)
        {
            this.Description = description;
            this.GenericKeyTypeKey = genericKeyTypeKey;
		
        }
		[JsonConstructor]
        public DocumentSourceTypeDataModel(int documentSourceTypeKey, string description, int genericKeyTypeKey)
        {
            this.DocumentSourceTypeKey = documentSourceTypeKey;
            this.Description = description;
            this.GenericKeyTypeKey = genericKeyTypeKey;
		
        }		

        public int DocumentSourceTypeKey { get; set; }

        public string Description { get; set; }

        public int GenericKeyTypeKey { get; set; }

        public void SetKey(int key)
        {
            this.DocumentSourceTypeKey =  key;
        }
    }
}