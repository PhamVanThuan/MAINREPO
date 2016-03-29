using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ComcorpImagingRequestDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ComcorpImagingRequestDataModel(int offerKey, Guid imagingReference, int expectedDocuments, int receivedDocuments)
        {
            this.OfferKey = offerKey;
            this.ImagingReference = imagingReference;
            this.ExpectedDocuments = expectedDocuments;
            this.ReceivedDocuments = receivedDocuments;
		
        }
		[JsonConstructor]
        public ComcorpImagingRequestDataModel(int comcorpImagingRequestKey, int offerKey, Guid imagingReference, int expectedDocuments, int receivedDocuments)
        {
            this.ComcorpImagingRequestKey = comcorpImagingRequestKey;
            this.OfferKey = offerKey;
            this.ImagingReference = imagingReference;
            this.ExpectedDocuments = expectedDocuments;
            this.ReceivedDocuments = receivedDocuments;
		
        }		

        public int ComcorpImagingRequestKey { get; set; }

        public int OfferKey { get; set; }

        public Guid ImagingReference { get; set; }

        public int ExpectedDocuments { get; set; }

        public int ReceivedDocuments { get; set; }

        public void SetKey(int key)
        {
            this.ComcorpImagingRequestKey =  key;
        }
    }
}