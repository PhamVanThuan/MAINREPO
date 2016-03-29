using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class FutureDatedChangeDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public FutureDatedChangeDataModel(int futureDatedChangeTypeKey, int identifierReferenceKey, DateTime effectiveDate, bool notificationRequired, string userID, DateTime insertDate, DateTime? changeDate)
        {
            this.FutureDatedChangeTypeKey = futureDatedChangeTypeKey;
            this.IdentifierReferenceKey = identifierReferenceKey;
            this.EffectiveDate = effectiveDate;
            this.NotificationRequired = notificationRequired;
            this.UserID = userID;
            this.InsertDate = insertDate;
            this.ChangeDate = changeDate;
		
        }
		[JsonConstructor]
        public FutureDatedChangeDataModel(int futureDatedChangeKey, int futureDatedChangeTypeKey, int identifierReferenceKey, DateTime effectiveDate, bool notificationRequired, string userID, DateTime insertDate, DateTime? changeDate)
        {
            this.FutureDatedChangeKey = futureDatedChangeKey;
            this.FutureDatedChangeTypeKey = futureDatedChangeTypeKey;
            this.IdentifierReferenceKey = identifierReferenceKey;
            this.EffectiveDate = effectiveDate;
            this.NotificationRequired = notificationRequired;
            this.UserID = userID;
            this.InsertDate = insertDate;
            this.ChangeDate = changeDate;
		
        }		

        public int FutureDatedChangeKey { get; set; }

        public int FutureDatedChangeTypeKey { get; set; }

        public int IdentifierReferenceKey { get; set; }

        public DateTime EffectiveDate { get; set; }

        public bool NotificationRequired { get; set; }

        public string UserID { get; set; }

        public DateTime InsertDate { get; set; }

        public DateTime? ChangeDate { get; set; }

        public void SetKey(int key)
        {
            this.FutureDatedChangeKey =  key;
        }
    }
}