using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.BusinessModel.Validation;
using SAHL.DomainProcessManager.Models;
using SAHL.WCFServices.ComcorpConnector.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SAHL.WCFServices.ComcorpConnector.Managers.ModelCreation
{
    public class ApplicantModelManager : IApplicantModelManager
    {
        private IValidationUtils validationUtils;
        private IAssetsLiabilitiesModelManager assetsLiabilitiesModelManager;
        private IAffordabilityModelManager affordabilityModelManager;
        private IDeclarationsModelManager declarationsModelManager;
        private IAddressModelManager addressModelManager;
        private IBankAccountModelManager bankAccountModelManager;
        private IEmploymentModelManager employmentModelManager;
        private IMarketingOptionsModelManager marketingOptionsModelManager;

        public ApplicantModelManager(IValidationUtils validationUtils, IAssetsLiabilitiesModelManager assetsLiabilitiesModelManager, IAffordabilityModelManager affordabilityModelManager,
            IDeclarationsModelManager declarationsModelManager, IAddressModelManager addressModelManager, IBankAccountModelManager bankAccountModelManager, IEmploymentModelManager
            employmentModelManager, IMarketingOptionsModelManager marketingOptionsModelManager)
        {
            this.validationUtils = validationUtils;
            this.assetsLiabilitiesModelManager = assetsLiabilitiesModelManager;
            this.affordabilityModelManager = affordabilityModelManager;
            this.declarationsModelManager = declarationsModelManager;
            this.addressModelManager = addressModelManager;
            this.bankAccountModelManager = bankAccountModelManager;
            this.employmentModelManager = employmentModelManager;
            this.marketingOptionsModelManager = marketingOptionsModelManager;
        }

        public ApplicantModel PopulateApplicantDetails(Applicant comcorpApplicant, ResidentialAddressModel propertyAddress)
        {
            SalutationType saluationType = validationUtils.ParseEnum<SalutationType>(comcorpApplicant.Title);
            Gender gender = validationUtils.ParseEnum<Gender>(comcorpApplicant.Gender);
            MaritalStatus maritalStatus = validationUtils.ParseEnum<MaritalStatus>(comcorpApplicant.MaritalStatus);
            PopulationGroup populationGroup = validationUtils.ParseEnum<PopulationGroup>(comcorpApplicant.EthnicGroup);
            CitizenType citizenType = validationUtils.ParseEnum<CitizenType>(comcorpApplicant.SAHLSACitizenType);
            Language homeLanguage = validationUtils.ParseEnum<Language>(comcorpApplicant.HomeLanguage);

            CorrespondenceLanguage correspondenceLanguage = String.Compare(comcorpApplicant.CorrespondenceLanguage, "Afrikaans", true) == 0 ? 
                CorrespondenceLanguage.Afrikaans : CorrespondenceLanguage.English;

            LeadApplicantOfferRoleTypeEnum applicationRoleType = comcorpApplicant.SAHLIsSurety ? LeadApplicantOfferRoleTypeEnum.Lead_Suretor : LeadApplicantOfferRoleTypeEnum.Lead_MainApplicant;
            Education education = !String.IsNullOrWhiteSpace(comcorpApplicant.SAHLHighestQualification) ? validationUtils.ParseEnum<Education>(comcorpApplicant.SAHLHighestQualification) 
                : Education.Unknown;
            //assets & liabilities
            List<ApplicantAssetLiabilityModel> assetsAndLiabilities = new List<ApplicantAssetLiabilityModel>();
            assetsAndLiabilities.AddRange(assetsLiabilitiesModelManager.PopulateAssets(comcorpApplicant.AssetItems));
            assetsAndLiabilities.AddRange(assetsLiabilitiesModelManager.PopulateLiabilities(comcorpApplicant.LiabilityItems));
            //affordability assessment
            List<ApplicantAffordabilityModel> affordabilities = new List<ApplicantAffordabilityModel>();
            affordabilities.AddRange(affordabilityModelManager.PopulateIncomes(comcorpApplicant.IncomeItems));
            affordabilities.AddRange(affordabilityModelManager.PopulateExpenses(comcorpApplicant.ExpenditureItems));
            //declarations
            List<ApplicantDeclarationsModel> applicantDeclarations = declarationsModelManager.PopulateDeclarations(comcorpApplicant);
            //addresses
            List<AddressModel> addresses = addressModelManager.PopulateAddresses(comcorpApplicant, propertyAddress, comcorpApplicant.AssetItems);
            //bank accounts
            List<BankAccountModel> bankAccounts = bankAccountModelManager.PopulateBankAccounts(comcorpApplicant.BankAccounts);
            //employment
            List<EmploymentModel> employments = new List<EmploymentModel>();
            if (!String.IsNullOrWhiteSpace(comcorpApplicant.CurrentEmploymentType))
            {
                employments = employmentModelManager.PopulateEmployment(comcorpApplicant);
            }
            //marketing option
            List<ApplicantMarketingOptionModel> applicantMarketingOptions = marketingOptionsModelManager.PopulateMarketingOptions(comcorpApplicant);

            //set the applicants initials
            Regex initialsRegex = new Regex(@"(\b[a-zA-Z])[a-zA-Z]* ?");
            string initials = initialsRegex.Replace(comcorpApplicant.FirstName, "$1");
          
            ApplicantModel applicant = new ApplicantModel(comcorpApplicant.IdentificationNo, null, saluationType, comcorpApplicant.FirstName, comcorpApplicant.Surname,initials, 
                comcorpApplicant.PreferredName, gender, maritalStatus, populationGroup, citizenType, comcorpApplicant.DateOfBirth, homeLanguage, correspondenceLanguage,
                comcorpApplicant.HomePhoneCode, comcorpApplicant.HomePhone, comcorpApplicant.WorkPhoneCode, comcorpApplicant.WorkPhone, comcorpApplicant.Cellphone, comcorpApplicant.EmailAddress,
                comcorpApplicant.FaxCode, comcorpApplicant.FaxNo, applicationRoleType, bankAccounts, employments, addresses, affordabilities, applicantDeclarations, assetsAndLiabilities,
                applicantMarketingOptions, education, comcorpApplicant.IncomeContributor);
            return applicant;
        }
    }
}