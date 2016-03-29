using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OfferDocumentDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public OfferDocumentDataModel(int offerKey, int documentTypeKey, DateTime? documentReceivedDate, string documentReceivedBy, string description, int? genericKey)
        {
            this.OfferKey = offerKey;
            this.DocumentTypeKey = documentTypeKey;
            this.DocumentReceivedDate = documentReceivedDate;
            this.DocumentReceivedBy = documentReceivedBy;
            this.Description = description;
            this.GenericKey = genericKey;
		
        }
		[JsonConstructor]
        public OfferDocumentDataModel(int offerDocumentKey, int offerKey, int documentTypeKey, DateTime? documentReceivedDate, string documentReceivedBy, string description, int? genericKey)
        {
            this.OfferDocumentKey = offerDocumentKey;
            this.OfferKey = offerKey;
            this.DocumentTypeKey = documentTypeKey;
            this.DocumentReceivedDate = documentReceivedDate;
            this.DocumentReceivedBy = documentReceivedBy;
            this.Description = description;
            this.GenericKey = genericKey;
		
        }		

        public int OfferDocumentKey { get; set; }

        public int OfferKey { get; set; }

        public int DocumentTypeKey { get; set; }

        public DateTime? DocumentReceivedDate { get; set; }

        public string DocumentReceivedBy { get; set; }

        public string Description { get; set; }

        public int? GenericKey { get; set; }

        public void SetKey(int key)
        {
            this.OfferDocumentKey =  key;
        }
    }
}