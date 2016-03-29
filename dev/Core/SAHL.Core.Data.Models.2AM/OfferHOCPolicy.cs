using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OfferHOCPolicyDataModel :  IDataModel
    {
        public OfferHOCPolicyDataModel(int offerKey, int? hOCInsurerKey, int? hOCPolicyNumber, int? hOCMonthlyPremium, int? hOCRoofKey, int? hOCSubsidenceKey, int? hOCConstructionKey, bool? ceded)
        {
            this.OfferKey = offerKey;
            this.HOCInsurerKey = hOCInsurerKey;
            this.HOCPolicyNumber = hOCPolicyNumber;
            this.HOCMonthlyPremium = hOCMonthlyPremium;
            this.HOCRoofKey = hOCRoofKey;
            this.HOCSubsidenceKey = hOCSubsidenceKey;
            this.HOCConstructionKey = hOCConstructionKey;
            this.Ceded = ceded;
		
        }		

        public int OfferKey { get; set; }

        public int? HOCInsurerKey { get; set; }

        public int? HOCPolicyNumber { get; set; }

        public int? HOCMonthlyPremium { get; set; }

        public int? HOCRoofKey { get; set; }

        public int? HOCSubsidenceKey { get; set; }

        public int? HOCConstructionKey { get; set; }

        public bool? Ceded { get; set; }
    }
}