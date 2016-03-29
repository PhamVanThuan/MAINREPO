using System;
using System.Runtime.Serialization;

using SAHL.Core.Data;
using SAHL.Core.BusinessModel.Enums;

namespace SAHL.DomainProcessManager.Models
{
    [Serializable]
    [DataContract]
    public class ApplicantMarketingOptionModel : IDataModel
    {
        public ApplicantMarketingOptionModel(MarketingOption marketingOption, GeneralStatus generalStatus)
        {
            this.MarketingOption = marketingOption;
            this.GeneralStatus   = generalStatus;
        }

        [DataMember]
        public MarketingOption MarketingOption { get; set; }

        [DataMember]
        public GeneralStatus GeneralStatus { get; set; }
    }
}
