using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.DomainProcessManager.Models;
using SAHL.WCFServices.ComcorpConnector.Managers.ModelCreation;
using SAHL.WCFServices.ComcorpConnector.Objects;
using SAHL.WCFServices.ComcorpConnector.Specs.HandlerSpecs.Mocks;
using System;
using System.Linq;

namespace SAHL.WCFServices.ComcorpConnector.Specs.ManagerSpecs.ModelCreation.ApplicantModelManagerSpecs
{
    public class when_populating_an_applicants_related_models : WithModelManagerFakes
    {
        private static ApplicantModelManager applicantModelManager;
        private static Applicant comcorpApplicant;
        private static ApplicantModel applicantModel;
        private static ResidentialAddressModel propertyAddress;

        private Establish context = () =>
        {
            comcorpApplicant = IntegrationServiceTestHelper.PopulateComcorpApplicant(IntegrationServiceTestHelper.ComcorpApplicantType.MainApplicant
                , MaritalStatus.Married_CommunityofProperty
                , false
                , false
                , false
                , false
                , true);
            propertyAddress = new ResidentialAddressModel("45", "Chelsea Drive", string.Empty, string.Empty, string.Empty, "Durban North", "Durban", "Kwazulu-Natal", "4051",
                "South Africa", false);
            applicantModelManager = new ApplicantModelManager(_validationUtils, assetsManager, affordabilityManager, declarationsManager, addressManager, bankAccountManager,
                employmentManager, marketingOptionsManager);
        };

        private Because of = () =>
        {
            applicantModel = applicantModelManager.PopulateApplicantDetails(comcorpApplicant, propertyAddress);
        };

        private It should_populate_applicant_addresses_using_the_comcorp_addresses_and_add_them_to_the_applicant = () =>
        {
            addressManager.Received().PopulateAddresses(comcorpApplicant, propertyAddress, null);
            applicantModel.Addresses.ShouldEqual(addresses);
        };

        private It should_populate_the_applicants_declarations_and_add_them_to_the_applicant = () =>
        {
            declarationsManager.Received().PopulateDeclarations(comcorpApplicant);
            applicantModel.ApplicantDeclarations.ShouldEqual(applicantDeclarations);
        };

        private It should_populate_the_applicants_marketing_options_and_add_them_to_the_applicant = () =>
        {
            marketingOptionsManager.Received().PopulateMarketingOptions(comcorpApplicant);
            applicantModel.ApplicantMarketingOptions.ShouldEqual(marketingOptions);
        };

        private It should_populate_the_applicants_employment_records_and_add_them_to_the_applicant = () =>
        {
            employmentManager.Received().PopulateEmployment(comcorpApplicant);
            applicantModel.Employments.ShouldEqual(employments);
        };

        private It should_populate_the_applicants_assets_based_on_the_comcorp_applicant_asset_items_and_add_them_to_the_applicant = () =>
        {
            assetsManager.Received().PopulateAssets(comcorpApplicant.AssetItems);
            applicantModel.ApplicantAssetLiabilities.ShouldContain(assets);
        };

        private It should_populate_the_applicants_liabilities_based_on_the_comcorp_applicant_liabilities_items_and_add_them_to_the_applicant = () =>
        {
            assetsManager.Received().PopulateLiabilities(comcorpApplicant.LiabilityItems);
            applicantModel.ApplicantAssetLiabilities.ShouldContain(liabilities);
        };

        private It should_populate_the_applicants_incomes_based_on_the_comcorp_income_items_and_add_them_to_the_applicant = () =>
        {
            affordabilityManager.Received().PopulateIncomes(comcorpApplicant.IncomeItems);
            applicantModel.Affordabilities.ShouldContain(incomes);
        };

        private It should_populate_the_applicants_expenses_based_on_the_comcorp_expenditure_items_and_add_them_to_the_applicant = () =>
        {
            affordabilityManager.Received().PopulateExpenses(comcorpApplicant.ExpenditureItems);
            applicantModel.Affordabilities.ShouldContain(expenses);
        };
    }
}