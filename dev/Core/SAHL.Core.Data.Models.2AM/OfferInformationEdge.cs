using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OfferInformationEdgeDataModel :  IDataModel
    {
        public OfferInformationEdgeDataModel(int offerInformationKey, double fullTermInstalment, double amortisationTermInstalment, double interestOnlyInstalment, int interestOnlyTerm)
        {
            this.OfferInformationKey = offerInformationKey;
            this.FullTermInstalment = fullTermInstalment;
            this.AmortisationTermInstalment = amortisationTermInstalment;
            this.InterestOnlyInstalment = interestOnlyInstalment;
            this.InterestOnlyTerm = interestOnlyTerm;
		
        }		

        public int OfferInformationKey { get; set; }

        public double FullTermInstalment { get; set; }

        public double AmortisationTermInstalment { get; set; }

        public double InterestOnlyInstalment { get; set; }

        public int InterestOnlyTerm { get; set; }
    }
}