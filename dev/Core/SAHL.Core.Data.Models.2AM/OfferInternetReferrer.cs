using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OfferInternetReferrerDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public OfferInternetReferrerDataModel(int offerKey, string userURL, string referringServerURL, string parameters)
        {
            this.OfferKey = offerKey;
            this.UserURL = userURL;
            this.ReferringServerURL = referringServerURL;
            this.Parameters = parameters;
		
        }
		[JsonConstructor]
        public OfferInternetReferrerDataModel(int offerInternetReferrerKey, int offerKey, string userURL, string referringServerURL, string parameters)
        {
            this.OfferInternetReferrerKey = offerInternetReferrerKey;
            this.OfferKey = offerKey;
            this.UserURL = userURL;
            this.ReferringServerURL = referringServerURL;
            this.Parameters = parameters;
		
        }		

        public int OfferInternetReferrerKey { get; set; }

        public int OfferKey { get; set; }

        public string UserURL { get; set; }

        public string ReferringServerURL { get; set; }

        public string Parameters { get; set; }

        public void SetKey(int key)
        {
            this.OfferInternetReferrerKey =  key;
        }
    }
}