using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OfferInformationDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public OfferInformationDataModel(DateTime offerInsertDate, int offerKey, int? offerInformationTypeKey, string userName, DateTime? changeDate, int? productKey)
        {
            this.OfferInsertDate = offerInsertDate;
            this.OfferKey = offerKey;
            this.OfferInformationTypeKey = offerInformationTypeKey;
            this.UserName = userName;
            this.ChangeDate = changeDate;
            this.ProductKey = productKey;
		
        }
		[JsonConstructor]
        public OfferInformationDataModel(int offerInformationKey, DateTime offerInsertDate, int offerKey, int? offerInformationTypeKey, string userName, DateTime? changeDate, int? productKey)
        {
            this.OfferInformationKey = offerInformationKey;
            this.OfferInsertDate = offerInsertDate;
            this.OfferKey = offerKey;
            this.OfferInformationTypeKey = offerInformationTypeKey;
            this.UserName = userName;
            this.ChangeDate = changeDate;
            this.ProductKey = productKey;
		
        }		

        public int OfferInformationKey { get; set; }

        public DateTime OfferInsertDate { get; set; }

        public int OfferKey { get; set; }

        public int? OfferInformationTypeKey { get; set; }

        public string UserName { get; set; }

        public DateTime? ChangeDate { get; set; }

        public int? ProductKey { get; set; }

        public void SetKey(int key)
        {
            this.OfferInformationKey =  key;
        }
    }
}