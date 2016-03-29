using System;
using System.Runtime.Serialization;

using SAHL.Core.BusinessModel.Enums;

namespace SAHL.DomainProcessManager.Models
{
    [Serializable]
    [DataContract]
    public class ApplicantUnListedInvestmentAssetModel : ApplicantAssetLiabilityModel
    {
        public ApplicantUnListedInvestmentAssetModel(double assetValue, string companyName)
            : base (AssetLiabilityType.UnlistedInvestments, null, null, assetValue, 0, companyName, null, null,null)
        {
        }
    }
}
