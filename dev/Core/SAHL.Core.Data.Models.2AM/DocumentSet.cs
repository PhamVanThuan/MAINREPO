using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class DocumentSetDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public DocumentSetDataModel(string description, int originationSourceProductKey, int offerTypeKey)
        {
            this.Description = description;
            this.OriginationSourceProductKey = originationSourceProductKey;
            this.OfferTypeKey = offerTypeKey;
		
        }
		[JsonConstructor]
        public DocumentSetDataModel(int documentSetKey, string description, int originationSourceProductKey, int offerTypeKey)
        {
            this.DocumentSetKey = documentSetKey;
            this.Description = description;
            this.OriginationSourceProductKey = originationSourceProductKey;
            this.OfferTypeKey = offerTypeKey;
		
        }		

        public int DocumentSetKey { get; set; }

        public string Description { get; set; }

        public int OriginationSourceProductKey { get; set; }

        public int OfferTypeKey { get; set; }

        public void SetKey(int key)
        {
            this.DocumentSetKey =  key;
        }
    }
}