using System;
using System.Runtime.Serialization;

using SAHL.Core.BusinessModel.Enums;

namespace SAHL.DomainProcessManager.Models
{
    [Serializable]
    [DataContract]
    public class ApplicantLifeAssuranceAssetModel : ApplicantAssetLiabilityModel
    {
        public ApplicantLifeAssuranceAssetModel(double surrenderValue, string companyName)
            : base(AssetLiabilityType.LifeAssurance, null, null, surrenderValue, null, companyName, null, null, null)
        {
        }
    }
}