using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.FETest
{
    [Serializable]
    public partial class AlphaHousingApplicationsDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public AlphaHousingApplicationsDataModel(int offerKey)
        {
            this.OfferKey = offerKey;
		
        }
		[JsonConstructor]
        public AlphaHousingApplicationsDataModel(int id, int offerKey)
        {
            this.Id = id;
            this.OfferKey = offerKey;
		
        }		

        public int Id { get; set; }

        public int OfferKey { get; set; }

        public void SetKey(int key)
        {
            this.Id =  key;
        }
    }
}