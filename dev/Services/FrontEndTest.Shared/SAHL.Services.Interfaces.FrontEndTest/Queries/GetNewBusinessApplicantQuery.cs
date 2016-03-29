using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using System;
using System.Linq;

namespace SAHL.Services.Interfaces.FrontEndTest.Queries
{
    public class GetNewBusinessApplicantQuery : ServiceQuery<GetNewBusinessApplicantQueryResult>, IFrontEndTestQuery, ISqlServiceQuery<GetNewBusinessApplicantQueryResult>
    {
        public bool IsIncomeContributor { get; protected set; }
        public bool HasDeclarations { get; protected set; }
        public bool HasAffordabilityAssessment { get; protected set; }
        public bool HasAssetsLiabilities { get; protected set; }
        public bool HasBankAccount { get; protected set; }
        public bool HasEmployment { get; protected set; }
        public bool HasDomicilium { get; protected set; }
        public bool HasResidentialAddress { get; protected set; }
        public bool HasPostalAddress { get; protected set; }

        public GetNewBusinessApplicantQuery(bool isIncomeContributor, bool hasDeclarations, bool hasAffordabilityAssessment, bool hasAssetsLiabilities, bool hasBankAccount,
            bool hasEmployment, bool hasDomicilium, bool hasResidentialAddress, bool hasPostalAdddress )
        {
            this.IsIncomeContributor = isIncomeContributor;
            this.HasDeclarations = hasDeclarations;
            this.HasAffordabilityAssessment = hasAffordabilityAssessment;
            this.HasAssetsLiabilities = hasAssetsLiabilities;
            this.HasBankAccount = hasBankAccount;
            this.HasEmployment = hasEmployment;
            this.HasResidentialAddress = hasResidentialAddress;
            this.HasPostalAddress = hasPostalAdddress;
            
        }
    }
}