using System;
using System.Runtime.Serialization;

using SAHL.Core.BusinessModel.Enums;

namespace SAHL.DomainProcessManager.Models
{
    [Serializable]
    [DataContract]
    public class ApplicantListedInvestmentAssetModel : ApplicantAssetLiabilityModel
    {
        public ApplicantListedInvestmentAssetModel(double assetValue, string companyName)
            : base (AssetLiabilityType.ListedInvestments, null, null, assetValue, 0, companyName, null, null,null)
        {
        }
    }
}
