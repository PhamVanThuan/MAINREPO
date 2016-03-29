using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OfferDocumentReferenceDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public OfferDocumentReferenceDataModel(int offerDocumentKey, int documentTypeReferenceObjectKey, int genericKey)
        {
            this.OfferDocumentKey = offerDocumentKey;
            this.DocumentTypeReferenceObjectKey = documentTypeReferenceObjectKey;
            this.GenericKey = genericKey;
		
        }
		[JsonConstructor]
        public OfferDocumentReferenceDataModel(int offerDocumentReferenceKey, int offerDocumentKey, int documentTypeReferenceObjectKey, int genericKey)
        {
            this.OfferDocumentReferenceKey = offerDocumentReferenceKey;
            this.OfferDocumentKey = offerDocumentKey;
            this.DocumentTypeReferenceObjectKey = documentTypeReferenceObjectKey;
            this.GenericKey = genericKey;
		
        }		

        public int OfferDocumentReferenceKey { get; set; }

        public int OfferDocumentKey { get; set; }

        public int DocumentTypeReferenceObjectKey { get; set; }

        public int GenericKey { get; set; }

        public void SetKey(int key)
        {
            this.OfferDocumentReferenceKey =  key;
        }
    }
}