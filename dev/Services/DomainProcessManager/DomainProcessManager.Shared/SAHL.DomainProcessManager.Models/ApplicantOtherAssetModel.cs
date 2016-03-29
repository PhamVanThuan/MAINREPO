using System;
using System.Runtime.Serialization;

using SAHL.Core.BusinessModel.Enums;

namespace SAHL.DomainProcessManager.Models
{
    [Serializable]
    [DataContract]
    public class ApplicantOtherAssetModel : ApplicantAssetLiabilityModel
    {
        public ApplicantOtherAssetModel(double assetValue, double liabilityValue, string description)
            : base (AssetLiabilityType.OtherAsset, null, null, assetValue, liabilityValue, null, null, null, description)
        {
        }
    }
}
