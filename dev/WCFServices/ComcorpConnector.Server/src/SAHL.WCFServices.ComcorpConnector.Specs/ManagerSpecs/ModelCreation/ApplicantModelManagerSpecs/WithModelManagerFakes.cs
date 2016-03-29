using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.Testing;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using SAHL.WCFServices.ComcorpConnector.Managers.ModelCreation;
using SAHL.WCFServices.ComcorpConnector.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.WCFServices.ComcorpConnector.Specs.ManagerSpecs.ModelCreation.ApplicantModelManagerSpecs
{
    public abstract class WithModelManagerFakes : WithCoreFakes
    {
        protected static IAssetsLiabilitiesModelManager assetsManager;
        protected static IEmploymentModelManager employmentManager;
        protected static IBankAccountModelManager bankAccountManager;
        protected static IAffordabilityModelManager affordabilityManager;
        protected static IAddressModelManager addressManager;
        protected static IDeclarationsModelManager declarationsManager;
        protected static IMarketingOptionsModelManager marketingOptionsManager;
        protected static ValidationUtils _validationUtils;
        protected static List<ApplicantAssetLiabilityModel> assets;
        protected static List<ApplicantAssetLiabilityModel> liabilities;
        protected static List<ApplicantAffordabilityModel> affordabilities;
        protected static List<ApplicantDeclarationsModel> applicantDeclarations;
        protected static List<ApplicantAffordabilityModel> incomes;
        protected static List<ApplicantAffordabilityModel> expenses;
        protected static List<EmploymentModel> employments;
        protected static List<BankAccountModel> bankAccounts;
        protected static List<AddressModel> addresses;
        protected static List<ApplicantMarketingOptionModel> marketingOptions;

        private Establish context = () =>
        {
            _validationUtils = new ValidationUtils();
            assetsManager = An<IAssetsLiabilitiesModelManager>();
            employmentManager = An<IEmploymentModelManager>();
            bankAccountManager = An<IBankAccountModelManager>();
            affordabilityManager = An<IAffordabilityModelManager>();
            addressManager = An<IAddressModelManager>();
            declarationsManager = An<IDeclarationsModelManager>();
            marketingOptionsManager = An<IMarketingOptionsModelManager>();
            //setup models

            //populate domain process manager models for other managers
            employments = new List<EmploymentModel>();
            bankAccounts = new List<BankAccountModel>();
            applicantDeclarations = ApplicationCreationTestHelper.PopulateApplicantDeclarations();
            affordabilities = ApplicationCreationTestHelper.PopulateIncomeAndExpenses();
            assets = ApplicationCreationTestHelper.PopulateApplicantAssets();
            liabilities = ApplicationCreationTestHelper.PopulateApplicantLiabilities();
            incomes = ApplicationCreationTestHelper.PopulateIncomes();
            expenses = ApplicationCreationTestHelper.PopulateExpenses();
            employments = ApplicationCreationTestHelper.PopulateEmployments();
            bankAccounts = ApplicationCreationTestHelper.PopulateBankAccounts();
            addresses = ApplicationCreationTestHelper.PopulateAddresses();
            marketingOptions = ApplicationCreationTestHelper.PopulateMarketingOptions();
            //set mocks
            assetsManager.WhenToldTo(x => x.PopulateAssets(Param.IsAny<List<AssetItem>>())).Return(assets);
            assetsManager.WhenToldTo(x => x.PopulateLiabilities(Param.IsAny<List<LiabilityItem>>())).Return(liabilities);
            affordabilityManager.WhenToldTo(x => x.PopulateExpenses(Param.IsAny<List<ExpenditureItem>>())).Return(expenses);
            affordabilityManager.WhenToldTo(x => x.PopulateIncomes(Param.IsAny<List<IncomeItem>>())).Return(incomes);
            declarationsManager.WhenToldTo(x => x.PopulateDeclarations(Param.IsAny<Applicant>())).Return(applicantDeclarations);
            employmentManager.WhenToldTo(x => x.PopulateEmployment(Param.IsAny<Applicant>())).Return(employments);
            bankAccountManager.WhenToldTo(x => x.PopulateBankAccounts(Param.IsAny<List<BankAccount>>())).Return(bankAccounts);
            addressManager.WhenToldTo(x => x.PopulateAddresses(Param.IsAny<Applicant>(), Param.IsAny<ResidentialAddressModel>(), Param.IsAny<List<AssetItem>>())).Return(addresses);
            marketingOptionsManager.WhenToldTo(x => x.PopulateMarketingOptions(Param.IsAny<Applicant>())).Return(marketingOptions);
        };
    }
}
