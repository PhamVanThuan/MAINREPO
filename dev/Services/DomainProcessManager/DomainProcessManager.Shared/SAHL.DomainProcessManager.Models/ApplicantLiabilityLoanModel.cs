using SAHL.Core.BusinessModel.Enums;
using System;
using System.Runtime.Serialization;

namespace SAHL.DomainProcessManager.Models
{
    [Serializable]
    [DataContract]
    public class ApplicantLiabilityLoanModel : ApplicantAssetLiabilityModel
    {
        public ApplicantLiabilityLoanModel(AssetLiabilitySubType loanType, string financialInstitution, double instalmentValue, double liabilityValue, DateTime dateRepayable)
            : base(AssetLiabilityType.LiabilityLoan, loanType, null, null, liabilityValue, financialInstitution, instalmentValue, dateRepayable, null)
        {
        }
    }
}