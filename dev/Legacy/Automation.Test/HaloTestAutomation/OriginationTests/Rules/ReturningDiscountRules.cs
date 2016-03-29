using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.LegalEntity;
using BuildingBlocks.Presenters.Origination;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using WorkflowAutomation.Harness;

namespace Origination.Rules
{
    [RequiresSTA]
    public class ReturningDiscountRules : OriginationTestBase<BasePage>
    {
        private int applicationKey;
        private IEnumerable<Automation.DataModels.LegalEntityReturningDiscountQualifyingData> legalEntityReturningDiscountQualifyingData;
        private Automation.DataModels.OriginationTestCase testCase;
        private float returningMainApplicantInitiationFeeDiscount;

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            legalEntityReturningDiscountQualifyingData = Service<ILegalEntityService>().GetLegalEntityReturningDiscountQualifyingDataWithValidIDNumber();

            testCase = new Automation.DataModels.OriginationTestCase()
            {
                Product = Products.NewVariableLoan,
                MarketValue = "1000000",
                CashDeposit = "200000",
                EmploymentType = EmploymentType.Salaried,
                Term = "240",
                HouseHoldIncome = "50000"
            };

            returningMainApplicantInitiationFeeDiscount = ControlNumeric.InitiationFee * ControlNumeric.ReturningMainApplicantInitiationFeeDiscount;

            IX2ScriptEngine scriptEngine = new X2ScriptEngine();
            base.Browser = new TestBrowser(TestUsers.BranchConsultant);
        }

        protected override void OnTestTearDown()
        {
            base.OnTestTearDown();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(base.Browser);
        }

        protected override void OnTestStart()
        {
        }

        [Test]
        public void when_applicant_is_a_main_applicant_on_an_open_loan_and_three_applications_are_created_discounted_initiation_fee_is_applied_to_all_three_applications()
        {
            var legalEntity = (from le in legalEntityReturningDiscountQualifyingData
                               where le.MainApplicantOpenAccountsCount == 1 && le.ReturningClientDiscountOpenAccountsCount == 0 && le.Unavailable == false
                               select le).FirstOrDefault();
            legalEntityReturningDiscountQualifyingData.First(le => le.LegalEntityKey == legalEntity.LegalEntityKey).Unavailable = true;

            CreateAndCleanUpApplication(legalEntity, offerRoleAttributeExists: true);
            SubmitApplication(expectedInitiationFee: returningMainApplicantInitiationFeeDiscount, offerAttributeExists: true);
            CreateAndCleanUpApplication(legalEntity, offerRoleAttributeExists: true);
            SubmitApplication(expectedInitiationFee: returningMainApplicantInitiationFeeDiscount, offerAttributeExists: true);
            CreateAndCleanUpApplication(legalEntity, offerRoleAttributeExists: true);
            SubmitApplication(expectedInitiationFee: returningMainApplicantInitiationFeeDiscount, offerAttributeExists: true);
        }

        [Test]
        public void when_applicant_is_a_suretor_an_account_discounted_initiation_fee_is_not_applied()
        {
            var legalEntity = (from le in legalEntityReturningDiscountQualifyingData
                               where (le.MainApplicantOpenAccountsCount + le.MainApplicantClosedAccountsCount) == 0 && le.SuretorOpenAccountsCount > 0 && le.Unavailable == false
                               select le).FirstOrDefault();
            legalEntityReturningDiscountQualifyingData.First(le => le.LegalEntityKey == legalEntity.LegalEntityKey).Unavailable = true;

            CreateAndCleanUpApplication(legalEntity, offerRoleAttributeExists: false);
            SubmitApplication(expectedInitiationFee: ControlNumeric.InitiationFee, offerAttributeExists: false);
        }

        [Test]
        public void when_applicant_is_a_main_applicant_on_two_open_accounts_both_with_discounted_initiation_fee_discounted_initiation_fee_is_not_applied_to_new_application()
        {
            var legalEntity = (from le in legalEntityReturningDiscountQualifyingData
                               where le.ReturningClientDiscountOpenAccountsCount == 2 && le.Unavailable == false
                               select le).FirstOrDefault();
            if (legalEntity == null)
            {
                legalEntity = (from le in legalEntityReturningDiscountQualifyingData
                               where le.MainApplicantOpenAccountsCount == 2 && le.MainApplicantClosedAccountsCount == 0 && le.ReturningClientDiscountOpenAccountsCount == 0
                               select le).FirstOrDefault();

                var accounts = Service<ILegalEntityService>().GetLegalEntityLoanAccounts(legalEntity.LegalEntityKey, AccountStatusEnum.Open);
                foreach (var account in accounts)
                {
                    var financialService = Service<IAccountService>().GetFinancialServiceRecordByType(account.AccountKey, FinancialServiceTypeEnum.VariableLoan);
                    Service<ILoanTransactionService>().pProcessTran(financialService.Rows(0).Column("FinancialServiceKey").GetValueAs<int>(), TransactionTypeEnum.InitiationFeeDiscount_ReturningClient, (decimal)returningMainApplicantInitiationFeeDiscount, "Test", TestUsers.HaloUser);
                }
            }
            legalEntityReturningDiscountQualifyingData.First(le => le.LegalEntityKey == legalEntity.LegalEntityKey).Unavailable = true;

            CreateAndCleanUpApplication(legalEntity, offerRoleAttributeExists: true);
            SubmitApplication(expectedInitiationFee: ControlNumeric.InitiationFee, offerAttributeExists: false);
        }

        [Test]
        [Ignore] //cant Post tran on Closed account.  Need to find another way to set up data or wait for real data to be created
        public void when_applicant_is_a_main_applicant_on_two_closed_accounts_both_with_discounted_initiation_fee_discounted_initiation_fee_is_applied_to_new_application()
        {
            var legalEntity = (from le in legalEntityReturningDiscountQualifyingData
                               where le.ReturningClientDiscountOpenAccountsCount == 0 && le.ReturningClientDiscountClosedAccountsCount == 2
                               select le).FirstOrDefault();
            if (legalEntity == null)
            {
                legalEntity = (from le in legalEntityReturningDiscountQualifyingData
                               where le.MainApplicantOpenAccountsCount == 0 && le.MainApplicantClosedAccountsCount == 2 && le.ReturningClientDiscountOpenAccountsCount == 0
                               select le).FirstOrDefault();

                var accounts = Service<ILegalEntityService>().GetLegalEntityLoanAccounts(legalEntity.LegalEntityKey, AccountStatusEnum.Closed);
                foreach (var account in accounts)
                {
                    var financialService = Service<IAccountService>().GetFinancialServiceRecordByType(account.AccountKey, FinancialServiceTypeEnum.VariableLoan);
                    Service<ILoanTransactionService>().pProcessTran(financialService.Rows(0).Column("FinancialServiceKey").GetValueAs<int>(), TransactionTypeEnum.InitiationFeeDiscount_ReturningClient, (decimal)returningMainApplicantInitiationFeeDiscount, "Test", TestUsers.HaloUser);
                }
            }
            legalEntityReturningDiscountQualifyingData.First(le => le.LegalEntityKey == legalEntity.LegalEntityKey).Unavailable = true;

            CreateAndCleanUpApplication(legalEntity, offerRoleAttributeExists: true);
            SubmitApplication(expectedInitiationFee: returningMainApplicantInitiationFeeDiscount, offerAttributeExists: true);
        }

        [Test]
        public void when_applicant_is_a_main_applicant_on_a_loan_related_to_the_same_property_discounted_initiation_fee_is_not_applied()
        {
            var legalEntity = (from le in legalEntityReturningDiscountQualifyingData
                               where le.MainApplicantOpenAccountsCount > 0 && le.ReturningClientDiscountOpenAccountsCount == 0 && le.Unavailable == false
                               select le).FirstOrDefault();
            legalEntityReturningDiscountQualifyingData.First(le => le.LegalEntityKey == legalEntity.LegalEntityKey).Unavailable = true;

            CreateAndCleanUpApplication(legalEntity, offerRoleAttributeExists: true);
            UpdateOMLPropertyToAnExistingMLPropertyAssociatedWithLegalEntity(legalEntity.LegalEntityKey);
            SubmitApplication(expectedInitiationFee: ControlNumeric.InitiationFee, offerAttributeExists: false);
        }

        [Test]
        public void when_applicant_is_a_main_applicant_on_a_closed_loan_related_to_the_same_property_discounted_initiation_fee_is_not_applied()
        {
            var legalEntity = (from le in legalEntityReturningDiscountQualifyingData
                               where le.MainApplicantOpenAccountsCount == 0 && le.MainApplicantClosedAccountsCount > 0 && le.ReturningClientDiscountOpenAccountsCount == 0
                               && Service<IPropertyService>().GetMortgageLoanPropertiesForLegalEntity(le.LegalEntityKey).FirstOrDefault() != null && le.Unavailable == false
                               select le).FirstOrDefault();
            legalEntityReturningDiscountQualifyingData.First(le => le.LegalEntityKey == legalEntity.LegalEntityKey).Unavailable = true;

            CreateAndCleanUpApplication(legalEntity, offerRoleAttributeExists: true);
            UpdateOMLPropertyToAnExistingMLPropertyAssociatedWithLegalEntity(legalEntity.LegalEntityKey);
            SubmitApplication(expectedInitiationFee: ControlNumeric.InitiationFee, offerAttributeExists: false);
        }

        [Test]
        public void when_adding_an_additional_applicant_who_is_a_main_applicant_on_an_existing_loan_discounted_initiation_fee_is_applied()
        {
            LoginAsNewUser(TestUsers.NewBusinessProcessor);
            var offer = Service<IX2WorkflowService>().GetOfferWithOfferAttributeAtState(WorkflowEnum.ApplicationManagement, WorkflowStates.ApplicationManagementWF.ManageApplication, OfferAttributeTypeEnum.DiscountedInitiationFee_ReturningClient, offerAttributeExists: false);
            base.Browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().WorkFlows(base.Browser);
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offer.OfferKey, WorkflowStates.ApplicationManagementWF.ManageApplication);

            var legalEntity = (from le in legalEntityReturningDiscountQualifyingData
                               where le.MainApplicantOpenAccountsCount == 1 && le.ReturningClientDiscountOpenAccountsCount == 0 && le.Unavailable == false
                               select le).FirstOrDefault();
            legalEntityReturningDiscountQualifyingData.First(le => le.LegalEntityKey == legalEntity.LegalEntityKey).Unavailable = true;

            applicationKey = offer.OfferKey;
            AddLegalEntity(legalEntity, true);

            OfferAssertions.AssertOfferAttributeExists(offer.OfferKey, Common.Enums.OfferAttributeTypeEnum.DiscountedInitiationFee_ReturningClient, true);
            OfferAssertions.AssertOfferExpense(offer.OfferKey, returningMainApplicantInitiationFeeDiscount, false, ExpenseTypeEnum.InitiationFeeBondPreparationFee);
        }

        [Test]
        public void when_removing_and_remaining_applicant_is_not_a_main_applicant_on_an_existing_loan_discounted_initiation_fee_is_removed()
        {
            var legalEntity = (from le in legalEntityReturningDiscountQualifyingData
                               where le.MainApplicantOpenAccountsCount == 0 && le.MainApplicantClosedAccountsCount == 0 && le.Unavailable == false
                               select le).FirstOrDefault();
            legalEntityReturningDiscountQualifyingData.First(le => le.LegalEntityKey == legalEntity.LegalEntityKey).Unavailable = true;

            CreateAndCleanUpApplication(legalEntity, offerRoleAttributeExists: false);

            var legalEntity2 = (from le in legalEntityReturningDiscountQualifyingData
                                where le.MainApplicantOpenAccountsCount == 1 && le.ReturningClientDiscountOpenAccountsCount == 0 && le.Unavailable == false
                                select le).FirstOrDefault();
            legalEntityReturningDiscountQualifyingData.First(le => le.LegalEntityKey == legalEntity2.LegalEntityKey).Unavailable = true;

            AddLegalEntity(legalEntity2, true);

            scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.ApplicationCapture, "SubmitApplication", applicationKey);
            Service<IX2WorkflowService>().WaitForAppManCaseCreate(applicationKey);
            scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.ApplicationManagement, "QAToManageApplication", applicationKey);

            OfferAssertions.AssertOfferRoleAttributeExists(applicationKey, legalEntity2.LegalEntityKey, Common.Enums.OfferRoleAttributeTypeEnum.ReturningClient, true);
            OfferAssertions.AssertOfferAttributeExists(applicationKey, Common.Enums.OfferAttributeTypeEnum.DiscountedInitiationFee_ReturningClient, true);
            OfferAssertions.AssertOfferExpense(applicationKey, returningMainApplicantInitiationFeeDiscount, false, ExpenseTypeEnum.InitiationFeeBondPreparationFee);

            LoginAsNewUser(TestUsers.NewBusinessProcessor);

            base.Browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().WorkFlows(base.Browser);
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, applicationKey, WorkflowStates.ApplicationManagementWF.ManageApplication);

            base.Browser.Navigate<BuildingBlocks.Navigation.FLOBO.Common.ApplicantsNode>().Applicants(NodeTypeEnum.Delete);
            base.Browser.Page<ApplicantsRemove>().RemoveApplicant(legalEntity2.LegalEntityLegalName);

            OfferAssertions.AssertOfferRoleAttributeExists(applicationKey, legalEntity2.LegalEntityKey, Common.Enums.OfferRoleAttributeTypeEnum.ReturningClient, false);
            OfferAssertions.AssertOfferAttributeExists(applicationKey, Common.Enums.OfferAttributeTypeEnum.DiscountedInitiationFee_ReturningClient, false);
            OfferAssertions.AssertOfferExpense(applicationKey, ControlNumeric.InitiationFee, false, ExpenseTypeEnum.InitiationFeeBondPreparationFee);
        }

        [Test]
        public void when_2_applicants_on_application_1_with_2_discounted_accounts_and_a_second_with_1_discounted_account_the_discount_is_applied()
        {
            //find legal entity with 2 open loans both with a discounted initiation fee
            var legalEntity1 = (from le in legalEntityReturningDiscountQualifyingData
                                where le.ReturningClientDiscountOpenAccountsCount == 2 && le.Unavailable == false
                                select le).FirstOrDefault();
            if (legalEntity1 == null)
            {
                legalEntity1 = (from le in legalEntityReturningDiscountQualifyingData
                                where le.MainApplicantOpenAccountsCount == 2 && le.MainApplicantClosedAccountsCount == 0 && le.ReturningClientDiscountOpenAccountsCount == 0
                                select le).FirstOrDefault();

                var accounts = Service<ILegalEntityService>().GetLegalEntityLoanAccounts(legalEntity1.LegalEntityKey, AccountStatusEnum.Open);
                foreach (var account in accounts)
                {
                    var financialService = Service<IAccountService>().GetFinancialServiceRecordByType(account.AccountKey, FinancialServiceTypeEnum.VariableLoan);
                    Service<ILoanTransactionService>().pProcessTran(financialService.Rows(0).Column("FinancialServiceKey").GetValueAs<int>(), TransactionTypeEnum.InitiationFeeDiscount_ReturningClient, (decimal)returningMainApplicantInitiationFeeDiscount, "Test", TestUsers.HaloUser);
                }
            }
            legalEntityReturningDiscountQualifyingData.First(le => le.LegalEntityKey == legalEntity1.LegalEntityKey).Unavailable = true;

            //find legal entity with 1 open loan with a discounted initiation fee
            var legalEntity2 = (from le in legalEntityReturningDiscountQualifyingData
                                where le.ReturningClientDiscountOpenAccountsCount == 1 && le.Unavailable == false
                                select le).FirstOrDefault();
            if (legalEntity2 == null)
            {
                legalEntity2 = (from le in legalEntityReturningDiscountQualifyingData
                                where le.MainApplicantOpenAccountsCount == 1 && le.MainApplicantClosedAccountsCount == 0 && le.ReturningClientDiscountOpenAccountsCount == 0 && le.Unavailable == false
                                select le).FirstOrDefault();

                var accounts = Service<ILegalEntityService>().GetLegalEntityLoanAccounts(legalEntity2.LegalEntityKey, AccountStatusEnum.Open);
                foreach (var account in accounts)
                {
                    var financialService = Service<IAccountService>().GetFinancialServiceRecordByType(account.AccountKey, FinancialServiceTypeEnum.VariableLoan);
                    Service<ILoanTransactionService>().pProcessTran(financialService.Rows(0).Column("FinancialServiceKey").GetValueAs<int>(), TransactionTypeEnum.InitiationFeeDiscount_ReturningClient, (decimal)returningMainApplicantInitiationFeeDiscount, "Test", TestUsers.HaloUser);
                }
            }
            legalEntityReturningDiscountQualifyingData.First(le => le.LegalEntityKey == legalEntity2.LegalEntityKey).Unavailable = true;

            CreateApplication(legalEntity1, true);
            AddLegalEntity(legalEntity2, true);
            Service<IApplicationService>().CleanupNewBusinessOffer(applicationKey);
            SubmitApplication(returningMainApplicantInitiationFeeDiscount, true);
        }

        private void CreateApplication(Automation.DataModels.LegalEntityReturningDiscountQualifyingData legalEntity, bool offerRoleAttributeExists)
        {
            LoginAsNewUser(TestUsers.BranchConsultant);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().gotoApplicationCalculator(base.Browser);
            base.Browser.Page<Views.LoanCalculator>().LoanCalculatorLead_NewPurchase(testCase.Product, testCase.MarketValue, testCase.CashDeposit, testCase.EmploymentType, testCase.Term
                             , testCase.HouseHoldIncome, ButtonTypeEnum.CreateApplication);
            base.Browser.Page<LegalEntityDetailsLeadApplicantAdd>().AddExistingLegalEntity(legalEntity.IDNumber);
            applicationKey = base.Browser.Page<ApplicationSummaryBase>().GetOfferKey();
            OfferAssertions.AssertOfferRoleAttributeExists(applicationKey, legalEntity.LegalEntityKey, Common.Enums.OfferRoleAttributeTypeEnum.ReturningClient, offerRoleAttributeExists);
        }

        private void CreateAndCleanUpApplication(Automation.DataModels.LegalEntityReturningDiscountQualifyingData legalEntity, bool offerRoleAttributeExists)
        {
            CreateApplication(legalEntity, offerRoleAttributeExists);
            Service<IApplicationService>().CleanupNewBusinessOffer(applicationKey);
        }

        private void SubmitApplication(float expectedInitiationFee, bool offerAttributeExists)
        {
            base.Browser.ClickWorkflowLoanNode(Workflows.ApplicationCapture);
            base.Browser.ClickAction(WorkflowActivities.ApplicationCapture.SubmitApplication);
            base.Browser.Page<WorkflowYesNo>().ClickYes();
            base.Browser.Page<WorkflowYesNo>().ContinueWithWarnings(true);
            OfferAssertions.AssertOfferAttributeExists(applicationKey, Common.Enums.OfferAttributeTypeEnum.DiscountedInitiationFee_ReturningClient, offerAttributeExists);
            OfferAssertions.AssertOfferExpense(applicationKey, expectedInitiationFee, false, ExpenseTypeEnum.InitiationFeeBondPreparationFee);
        }

        private void UpdateOMLPropertyToAnExistingMLPropertyAssociatedWithLegalEntity(int legalEntityKey)
        {
            var property = Service<IPropertyService>().GetMortgageLoanPropertiesForLegalEntity(legalEntityKey).FirstOrDefault();
            Service<IPropertyService>().UpdateOfferMortgageLoanPropertyKey(property.PropertyKey, applicationKey);
            Service<IPropertyService>().DBUpdateDeedsOfficeDetails(applicationKey);
        }

        private void LoginAsNewUser(string userName)
        {
            if (base.Browser != null)
            {
                if (!base.Browser.Page<BasePage>().LoggedInAs(userName))
                {
                    base.Browser.Dispose();
                    base.Browser = new TestBrowser(userName);
                }
            }
            else
                base.Browser = new TestBrowser(userName);
        }

        private void AddLegalEntity(Automation.DataModels.LegalEntityReturningDiscountQualifyingData legalEntity, bool offerRoleAttributeExists)
        {
            base.Browser.Navigate<BuildingBlocks.Navigation.FLOBO.Common.ApplicantsNode>().Applicants(NodeTypeEnum.Add);
            base.Browser.Page<LegalEntityDetails>().AddExisting(legalEntity.IDNumber);
            OfferAssertions.AssertOfferRoleAttributeExists(applicationKey, legalEntity.LegalEntityKey, Common.Enums.OfferRoleAttributeTypeEnum.ReturningClient, offerRoleAttributeExists);
        }
    }
}