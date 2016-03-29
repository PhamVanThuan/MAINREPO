using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.FETest
{
    [Serializable]
    public partial class ActiveNewBusinessApplicantsDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ActiveNewBusinessApplicantsDataModel(int offerKey, int offerRoleKey, int legalEntityKey, int offerRoleTypeKey, bool isIncomeContributor, bool? hasDeclarations, bool? hasAffordabilityAssessment, bool? hasAssetsLiabilities, bool? hasBankAccount, bool? hasEmployment, bool? hasResidentialAddress, bool? hasPostalAddress, bool? hasDomicilium)
        {
            this.OfferKey = offerKey;
            this.OfferRoleKey = offerRoleKey;
            this.LegalEntityKey = legalEntityKey;
            this.OfferRoleTypeKey = offerRoleTypeKey;
            this.IsIncomeContributor = isIncomeContributor;
            this.HasDeclarations = hasDeclarations;
            this.HasAffordabilityAssessment = hasAffordabilityAssessment;
            this.HasAssetsLiabilities = hasAssetsLiabilities;
            this.HasBankAccount = hasBankAccount;
            this.HasEmployment = hasEmployment;
            this.HasResidentialAddress = hasResidentialAddress;
            this.HasPostalAddress = hasPostalAddress;
            this.HasDomicilium = hasDomicilium;
		
        }
		[JsonConstructor]
        public ActiveNewBusinessApplicantsDataModel(int id, int offerKey, int offerRoleKey, int legalEntityKey, int offerRoleTypeKey, bool isIncomeContributor, bool? hasDeclarations, bool? hasAffordabilityAssessment, bool? hasAssetsLiabilities, bool? hasBankAccount, bool? hasEmployment, bool? hasResidentialAddress, bool? hasPostalAddress, bool? hasDomicilium)
        {
            this.Id = id;
            this.OfferKey = offerKey;
            this.OfferRoleKey = offerRoleKey;
            this.LegalEntityKey = legalEntityKey;
            this.OfferRoleTypeKey = offerRoleTypeKey;
            this.IsIncomeContributor = isIncomeContributor;
            this.HasDeclarations = hasDeclarations;
            this.HasAffordabilityAssessment = hasAffordabilityAssessment;
            this.HasAssetsLiabilities = hasAssetsLiabilities;
            this.HasBankAccount = hasBankAccount;
            this.HasEmployment = hasEmployment;
            this.HasResidentialAddress = hasResidentialAddress;
            this.HasPostalAddress = hasPostalAddress;
            this.HasDomicilium = hasDomicilium;
		
        }		

        public int Id { get; set; }

        public int OfferKey { get; set; }

        public int OfferRoleKey { get; set; }

        public int LegalEntityKey { get; set; }

        public int OfferRoleTypeKey { get; set; }

        public bool IsIncomeContributor { get; set; }

        public bool? HasDeclarations { get; set; }

        public bool? HasAffordabilityAssessment { get; set; }

        public bool? HasAssetsLiabilities { get; set; }

        public bool? HasBankAccount { get; set; }

        public bool? HasEmployment { get; set; }

        public bool? HasResidentialAddress { get; set; }

        public bool? HasPostalAddress { get; set; }

        public bool? HasDomicilium { get; set; }

        public void SetKey(int key)
        {
            this.Id =  key;
        }
    }
}