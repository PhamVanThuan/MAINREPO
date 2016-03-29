using System;
using System.Runtime.Serialization;

using SAHL.Core.BusinessModel.Enums;

namespace SAHL.DomainProcessManager.Models
{
    [Serializable]
    [DataContract]
    public class ApplicantFixedLongTermLiabilityModel : ApplicantAssetLiabilityModel
    {
        public ApplicantFixedLongTermLiabilityModel(double liabilityValue, string companyName)
            : base(AssetLiabilityType.FixedLongTermInvestment, null, null, null, liabilityValue, companyName, null, null, null)
        {
        }
    }
}
