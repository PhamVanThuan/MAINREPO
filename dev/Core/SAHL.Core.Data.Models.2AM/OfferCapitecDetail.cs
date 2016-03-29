using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OfferCapitecDetailDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public OfferCapitecDetailDataModel(int offerKey, string branch, string consultant)
        {
            this.OfferKey = offerKey;
            this.Branch = branch;
            this.Consultant = consultant;
		
        }
		[JsonConstructor]
        public OfferCapitecDetailDataModel(int offerCapitecDetailKey, int offerKey, string branch, string consultant)
        {
            this.OfferCapitecDetailKey = offerCapitecDetailKey;
            this.OfferKey = offerKey;
            this.Branch = branch;
            this.Consultant = consultant;
		
        }		

        public int OfferCapitecDetailKey { get; set; }

        public int OfferKey { get; set; }

        public string Branch { get; set; }

        public string Consultant { get; set; }

        public void SetKey(int key)
        {
            this.OfferCapitecDetailKey =  key;
        }
    }
}