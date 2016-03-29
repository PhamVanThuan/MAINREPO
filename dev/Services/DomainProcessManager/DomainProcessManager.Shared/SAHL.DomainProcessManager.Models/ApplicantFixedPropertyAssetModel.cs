using System;
using System.Runtime.Serialization;

using SAHL.Core.BusinessModel.Enums;

namespace SAHL.DomainProcessManager.Models
{
    [Serializable]
    [DataContract]
    public class ApplicantFixedPropertyAssetModel : ApplicantAssetLiabilityModel
    {
        public ApplicantFixedPropertyAssetModel(AddressModel address, double assetValue, double liabilityValue, DateTime dateAcquired)
            : base (AssetLiabilityType.FixedProperty, null, address, assetValue, liabilityValue, null, null, dateAcquired,null)
        {
        }
    }
}
