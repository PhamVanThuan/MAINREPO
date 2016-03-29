using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ApplicationDocumentDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ApplicationDocumentDataModel(int applicationDocumentTypeKey, bool received, bool required, int? aDUserKey, DateTime? receivedDate)
        {
            this.ApplicationDocumentTypeKey = applicationDocumentTypeKey;
            this.Received = received;
            this.Required = required;
            this.ADUserKey = aDUserKey;
            this.ReceivedDate = receivedDate;
		
        }
		[JsonConstructor]
        public ApplicationDocumentDataModel(int applicationDocumentKey, int applicationDocumentTypeKey, bool received, bool required, int? aDUserKey, DateTime? receivedDate)
        {
            this.ApplicationDocumentKey = applicationDocumentKey;
            this.ApplicationDocumentTypeKey = applicationDocumentTypeKey;
            this.Received = received;
            this.Required = required;
            this.ADUserKey = aDUserKey;
            this.ReceivedDate = receivedDate;
		
        }		

        public int ApplicationDocumentKey { get; set; }

        public int ApplicationDocumentTypeKey { get; set; }

        public bool Received { get; set; }

        public bool Required { get; set; }

        public int? ADUserKey { get; set; }

        public DateTime? ReceivedDate { get; set; }

        public void SetKey(int key)
        {
            this.ApplicationDocumentKey =  key;
        }
    }
}