using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class DocumentTypeGroupConfigurationDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public DocumentTypeGroupConfigurationDataModel(int documentTypeKey, int documentGroupKey, int originationSourceProductKey)
        {
            this.DocumentTypeKey = documentTypeKey;
            this.DocumentGroupKey = documentGroupKey;
            this.OriginationSourceProductKey = originationSourceProductKey;
		
        }
		[JsonConstructor]
        public DocumentTypeGroupConfigurationDataModel(int documentTypeGroupConfigurationKey, int documentTypeKey, int documentGroupKey, int originationSourceProductKey)
        {
            this.DocumentTypeGroupConfigurationKey = documentTypeGroupConfigurationKey;
            this.DocumentTypeKey = documentTypeKey;
            this.DocumentGroupKey = documentGroupKey;
            this.OriginationSourceProductKey = originationSourceProductKey;
		
        }		

        public int DocumentTypeGroupConfigurationKey { get; set; }

        public int DocumentTypeKey { get; set; }

        public int DocumentGroupKey { get; set; }

        public int OriginationSourceProductKey { get; set; }

        public void SetKey(int key)
        {
            this.DocumentTypeGroupConfigurationKey =  key;
        }
    }
}