using SAHL.Core.BusinessModel.Enums;
using System;
using System.Runtime.Serialization;

namespace SAHL.DomainProcessManager.Models
{
    [Serializable]
    [DataContract]
    public class ApplicantLiabilitySuretyModel : ApplicantAssetLiabilityModel
    {
        public ApplicantLiabilitySuretyModel(double assetValue, double liabilityValue, string description)
            : base(AssetLiabilityType.LiabilitySurety, null, null, assetValue, liabilityValue, null, null, null, description)
        {
        }
    }
}