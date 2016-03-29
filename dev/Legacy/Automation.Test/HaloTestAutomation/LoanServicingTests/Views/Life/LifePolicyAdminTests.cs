using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation.CBO.LoanServicing;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.LegalEntity;
using BuildingBlocks.Presenters.Life;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Linq;

namespace LoanServicingTests.Views.Life
{
    [TestFixture, RequiresSTA]
    public sealed class LifePolicyAdminTests : TestBase<LifePolicyAdmin>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.HaloUser);
        }

        [Test]
        public void WhenAddExistingLegalEntity_ShouldAddLegalEntityToLifeAccount()
        {
            //---------------Set up test -------------------
            int lifeaccountkey = base.Service<IAccountService>().GetLifeAccountKey(2, LifePolicyStatusEnum.Inforce);

            //get legalentity that does not play any life assured roles
            var legalentityId = Service<ILegalEntityService>().GetLegalEntityIDNumberNotPlayRole(RoleTypeEnum.AssuredLife);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);
            base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstLegalEntity(lifeaccountkey);
            base.Browser.Navigate<LoanServicingCBO>().LifeAccountNode(lifeaccountkey);
            base.View.ClickAddButton();
            //Had to move the Pop up handler into the above click method
            //base.View.ConfirmAddLifeDialog();
            base.Browser.Page<ClientSuperSearch>().PopulateSearch("", "", legalentityId.ToString(), "", "");
            base.Browser.Page<ClientSuperSearch>().PerformSearch();
            base.Browser.Page<ClientSuperSearch>().SelectByIDNumber(legalentityId.ToString());
            base.Browser.Page<LegalEntityDetails>().PopulateLegalEntity(insurableInterest: "Mortgagor");
            base.Browser.Page<LegalEntityDetails>().ClickUpdate();

            //halo backen API's are slow the updated takes a while
            BuildingBlocks.Timers.GeneralTimer.Wait(8000);

            //---------------Test Result -----------------------
            LegalEntityAssertions.AssertLegalEntityRoles(legalentityId, lifeaccountkey);
        }

        [Test]
        public void WhenAddLegalEntity_WithNewLegalEntity_ShouldAddLegalEntityToLifeAccount()
        {
            //---------------Set up test -------------------
            int lifeaccountkey = base.Service<IAccountService>().GetLifeAccountKey(2, LifePolicyStatusEnum.Inforce);

            //get a new dob
            var date = DateTime.Now.AddYears(-35);
            var month = date.Month.ToString();
            month = month.Length == 1 ? string.Format(@"0{0}", month) : month;
            var day = date.Day.ToString();
            day = day.Length == 1 ? string.Format(@"0{0}", day) : day;
            string dateString = string.Format(@"{0}{1}{2}", date.Year.ToString().Substring(2, 2), month, day);
            var idNumber = IDNumbers.GetNextIDNumber(dateString);

            var newLegalEntity = new Automation.DataModels.LegalEntity()
            {
                LegalEntityTypeKey = Common.Enums.LegalEntityTypeEnum.NaturalPerson,
                FirstNames = "John",
                Surname = "TestCase",
                IdNumber = idNumber,
                WorkPhoneCode = "013",
                WorkPhoneNumber = "1234567",
                FaxCode = "013",
                FaxNumber = "1234567",
                HomePhoneCode = "031",
                HomePhoneNumber = "1234567",
                CellPhoneNumber = "0791234567",
                EmailAddress = "clintons@sahomeloans.com",
                PreferredName = "John",
                SalutationDescription = "Mrs",
                Initials = "J",
                GenderDescription = "Male",
                MaritalStatusDescription = "Single",
                PopulationGroupDescription = "Unknown",
                EducationDescription = "Matric",
                CitizenTypeDescription = "SA Citizen",
                DateOfBirth = date,
                TaxNumber = "TaxNo1234",
                HomeLanguageDescription = "English",
                DocumentLanguageDescription = "English",
            };

            //---------------Assert Precondition----------------
            LegalEntityAssertions.AssertLegalEntityNotExist(newLegalEntity.IdNumber);

            //---------------Execute Test ----------------------
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);
            base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstLegalEntity(lifeaccountkey);
            base.Browser.Navigate<LoanServicingCBO>().LifeAccountNode(lifeaccountkey);
            base.View.ClickAddButton();
            //Had to move the Pop up handler into the above click method
            //base.View.ConfirmAddLifeDialog();
            base.Browser.Page<ClientSuperSearch>().ClickNewAssuredLife();
            base.Browser.Page<LegalEntityDetails>().PopulateLegalEntity(newLegalEntity, "Mortgagor");
            base.Browser.Page<LegalEntityDetails>().ClickUpdate();

            //halo backen API's are slow the updated takes a while
            BuildingBlocks.Timers.GeneralTimer.Wait(8000);

            //---------------Test Result -----------------------
            LegalEntityAssertions.AssertLegalEntityRoles(newLegalEntity.IdNumber, lifeaccountkey);
        }

        [Test]
        public void WhenRemoveLegalEntity_WithOnlyOneAssuredLifePlayingRoleOnLifeAccount_ShouldDisplayValidationMessage()
        {
            //---------------Set up test -------------------
            var lifeaccountkey = base.Service<IAccountService>().GetLifeAccountKey(1, LifePolicyStatusEnum.Inforce);

            //---------------Assert Precondition----------------
            LegalEntityAssertions.AssertOneLegalEntityRoleLifeAccount(lifeaccountkey);

            //---------------Execute Test ----------------------
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);
            base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstLegalEntity(lifeaccountkey);
            base.Browser.Navigate<LoanServicingCBO>().LifeAccountNode(lifeaccountkey);
            var selectedlegalentityid = base.View.SelectFirstLegalEntity();
            base.View.ClickRemoveButton();
            //Had to move the Pop up handler into the above click method
            //base.View.ConfirmRemoveLifeDialog();

            //---------------Test Result -----------------------
            base.View.AssertOneAssuredLifeValidationMessage();
        }

        [Test]
        public void WhenRemoveLegalEntity_WithMoreThanOneLegalEntityPlayRoleOnAccount_ShouldRemoveLegalEntityFromLifeAccount()
        {
            //---------------Set up test -------------------
            var lifeaccountkey = base.Service<IAccountService>().GetLifeAccountKey(2, LifePolicyStatusEnum.Inforce);

            //---------------Assert Precondition----------------
            LegalEntityAssertions.AssertManyLegalEntityRolesLifeAccount(lifeaccountkey);

            //---------------Execute Test ----------------------
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);
            base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstLegalEntity(lifeaccountkey);
            base.Browser.Navigate<LoanServicingCBO>().LifeAccountNode(lifeaccountkey);
            var selectedlegalentityid = base.View.SelectFirstLegalEntity();
            base.View.ClickRemoveButton();
            //Had to move the Pop up handler into the above click method
            //base.View.ConfirmRemoveLifeDialog();

            //---------------Test Result -----------------------
            LegalEntityAssertions.AssertNoLegalEntityRoles(selectedlegalentityid, lifeaccountkey);
        }

        [Test]
        public void WhenLifePolicyAdminViewLoaded_ShouldHaveAddLifeButton()
        {
            //---------------Set up test -------------------
            var lifeaccountkey = base.Service<IAccountService>().GetLifeAccountKey(2, LifePolicyStatusEnum.Inforce);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);
            base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstLegalEntity(lifeaccountkey);
            base.Browser.Navigate<LoanServicingCBO>().LifeAccountNode(lifeaccountkey);

            //---------------Test Result -----------------------
            base.View.AssertAddLifeButton();
        }

        [Test]
        public void WhenLifePolicyAdminViewLoaded_ShouldHaveRemoveLifeButton()
        {
            //---------------Set up test -------------------
            var lifeaccountkey = base.Service<IAccountService>().GetLifeAccountKey(2, LifePolicyStatusEnum.Inforce);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);
            base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstLegalEntity(lifeaccountkey);
            base.Browser.Navigate<LoanServicingCBO>().LifeAccountNode(lifeaccountkey);

            //---------------Test Result -----------------------
            base.View.AssertRemoveLifeButton();
        }

        [Test]
        public void WhenLifePolicyAdminViewLoaded_ShouldHaveRecalculatePremiumsButton()
        {
            //---------------Set up test -------------------
            var lifeaccountkey = base.Service<IAccountService>().GetLifeAccountKey(2, LifePolicyStatusEnum.Inforce);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);
            base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstLegalEntity(lifeaccountkey);
            base.Browser.Navigate<LoanServicingCBO>().LifeAccountNode(lifeaccountkey);

            //---------------Test Result -----------------------
            base.View.AssertRecalculatePremiumsButton();
        }

        [Test]
        public void WhenLifePolicyAdminViewLoaded_ShouldHavePremiumCalculatorButton()
        {
            //---------------Set up test -------------------
            var lifeaccountkey = base.Service<IAccountService>().GetLifeAccountKey(2, LifePolicyStatusEnum.Inforce);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);
            base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstLegalEntity(lifeaccountkey);
            base.Browser.Navigate<LoanServicingCBO>().LifeAccountNode(lifeaccountkey);

            //---------------Test Result -----------------------
            base.View.AssertPremiumCalculatorButton();
        }

        [Test]
        public void WhenLifeAddLegalEntity_WithInforcedLifePolicy_ShouldRequestUserConfirmation()
        {
            //---------------Set up test -------------------
            var lifeaccountkey = base.Service<IAccountService>().GetLifeAccountKey(2, LifePolicyStatusEnum.Inforce);

            //---------------Assert Precondition----------------
            LifeAssertions.AssertLifePolicyStatus(lifeaccountkey, LifePolicyStatusEnum.Inforce);

            //---------------Execute Test ----------------------
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);
            base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstLegalEntity(lifeaccountkey);
            base.Browser.Navigate<LoanServicingCBO>().LifeAccountNode(lifeaccountkey);
            var selectedlegalentityid = base.View.SelectFirstLegalEntity();
            //---------------Test Result -----------------------
            base.View.AssertInforcedConfirmationDialogMessage();
        }

        [Test]
        public void WhenLifeAddLegalEntityAndClickingCancel_WithInforcedLifePolicy_ShouldNotProcessAnything()
        {
            //---------------Set up test -----------------------
            int lifeaccountkey = base.Service<IAccountService>().GetLifeAccountKey(2, LifePolicyStatusEnum.Inforce);
            var legalentityId = Service<ILegalEntityService>().GetLegalEntityIDNumberNotPlayRole(RoleTypeEnum.AssuredLife);

            //---------------Assert Precondition----------------
            LifeAssertions.AssertLifePolicyStatus(lifeaccountkey, LifePolicyStatusEnum.Inforce);

            //---------------Execute Test ----------------------
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);
            base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstLegalEntity(lifeaccountkey);
            base.Browser.Navigate<LoanServicingCBO>().LifeAccountNode(lifeaccountkey);
            base.View.CancelAddLifeDialog();

            //---------------Test Result -----------------------
            base.Browser.Page<ClientSuperSearch>().AssertViewDisplayed("Life_PolicyAdmin");
        }

        [Test]
        public void WhenLifeAddLegalEntityAndClickingOK_WithInforcedLifePolicy_ShouldContinueProcessing()
        {
            //---------------Set up test -----------------------
            int lifeaccountkey = base.Service<IAccountService>().GetLifeAccountKey(2, LifePolicyStatusEnum.Inforce);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);
            base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstLegalEntity(lifeaccountkey);
            base.Browser.Navigate<LoanServicingCBO>().LifeAccountNode(lifeaccountkey);
            base.View.ClickAddButton();
            //Had to move the Pop up handler into the above click method
            //base.View.ConfirmAddLifeDialog();

            //---------------Test Result -----------------------
            base.Browser.Page<ClientSuperSearch>().AssertViewDisplayed("Life_ClientSuperSearchAdd_Admin");
        }

        [Test]
        public void WhenLifePremiumRecalc_WithProRataPremiumDecrease_ShouldPostBondProtectionPlanPremiumCorrection()
        {
            //---------------Set up test -----------------------
            int lifeaccountkey = base.Service<IAccountService>().GetLifeAccountKey(2, LifePolicyStatusEnum.Inforce);
            var accountBalances = base.Service<IAccountService>().GetAccountBalances(lifeaccountkey);
            decimal lifeBalance = (from a in accountBalances where a.BalanceTypeKey == (int)BalanceTypeEnum.Loan select a.Amount).FirstOrDefault();
            base.Service<IAccountService>().UpdateFinancialServiceBalance(lifeaccountkey, FinancialServiceTypeEnum.LifePolicy, lifeBalance * 0.5M);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);
            base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstLegalEntity(lifeaccountkey);
            base.Browser.Navigate<LoanServicingCBO>().LifeAccountNode(lifeaccountkey);
            base.View.ClickRecalculatePremium();

            BuildingBlocks.Timers.GeneralTimer.Wait(2000);

            //---------------Test Result -----------------------
            TransactionAssertions.AssertLoanTransactionExists(lifeaccountkey, TransactionTypeEnum.BondProtectionPlanPremiumCorrection);
        }

        [Test]
        public void WhenLifeAddLegalEntity_ShouldDisplayPolicyAdminView()
        {
            //---------------Set up test -----------------------
            int lifeaccountkey = base.Service<IAccountService>().GetLifeAccountKey(2, LifePolicyStatusEnum.Inforce);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);
            base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstLegalEntity(lifeaccountkey);
            base.Browser.Navigate<LoanServicingCBO>().LifeAccountNode(lifeaccountkey);

            //---------------Test Result -----------------------
            base.View.AssertViewDisplayed("Life_PolicyAdmin");
        }

        [Test]
        public void WhenLifeAddLegalEntity_ShouldDisplayLifeClientSuperSearchAddAdminView()
        {
            //---------------Set up test -----------------------
            int lifeaccountkey = base.Service<IAccountService>().GetLifeAccountKey(2, LifePolicyStatusEnum.Inforce);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);
            base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstLegalEntity(lifeaccountkey);
            base.Browser.Navigate<LoanServicingCBO>().LifeAccountNode(lifeaccountkey);
            base.View.ClickAddButton();
            //Had to move the Pop up handler into the above click method
            //base.View.ConfirmAddLifeDialog();

            //---------------Test Result -----------------------
            base.View.AssertViewDisplayed("Life_ClientSuperSearchAdd_Admin");
        }

        [Test]
        public void WhenLifeAddExistingLegalEntity_ShouldDisplayLifeLegalEntityAddExistingAdminView()
        {
            //---------------Set up test -----------------------
            int lifeaccountkey = base.Service<IAccountService>().GetLifeAccountKey(2, LifePolicyStatusEnum.Inforce);
            var legalentityId = Service<ILegalEntityService>().GetLegalEntityIDNumberNotPlayRole(RoleTypeEnum.AssuredLife);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);
            base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstLegalEntity(lifeaccountkey);
            base.Browser.Navigate<LoanServicingCBO>().LifeAccountNode(lifeaccountkey);
            base.View.ClickAddButton();
            //Had to move the Pop up handler into the above click method
            //base.View.ConfirmAddLifeDialog();
            base.Browser.Page<ClientSuperSearch>().PopulateSearch("", "", legalentityId.ToString(), "", "");
            base.Browser.Page<ClientSuperSearch>().PerformSearch();
            base.Browser.Page<ClientSuperSearch>().SelectByIDNumber(legalentityId.ToString());

            //---------------Test Result -----------------------
            base.View.AssertViewDisplayed("Life_LegalEntityAddExisting_Admin");
        }

        [Test]
        public void WhenLifeAddExistingLegalEntity_ShouldCreateAddAssuredLifeStageTransition()
        {
            //---------------Set up test -------------------
            var lifeaccountkey = base.Service<IAccountService>().GetLifeAccountKey(2, LifePolicyStatusEnum.Inforce);
            var legalentityId = Service<ILegalEntityService>().GetLegalEntityIDNumberNotPlayRole(RoleTypeEnum.AssuredLife);

            //---------------Execute Test ----------------------
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);
            base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstLegalEntity(lifeaccountkey);
            base.Browser.Navigate<LoanServicingCBO>().LifeAccountNode(lifeaccountkey);
            base.View.ClickAddButton();
            //Had to move the Pop up handler into the above click method
            //base.View.ConfirmAddLifeDialog();
            base.Browser.Page<ClientSuperSearch>().PopulateSearch("", "", legalentityId.ToString(), "", "");
            base.Browser.Page<ClientSuperSearch>().PerformSearch();
            base.Browser.Page<ClientSuperSearch>().SelectByIDNumber(legalentityId.ToString());
            base.Browser.Page<LegalEntityDetails>().PopulateLegalEntity(insurableInterest: "Mortgagor");
            base.Browser.Page<LegalEntityDetails>().ClickUpdate();

            //---------------Test Result -----------------------
            StageTransitionAssertions.AssertStageTransitionCreated(lifeaccountkey, StageDefinitionStageDefinitionGroupEnum.AddAssuredLife);
        }

        [Test]
        public void WhenLifeRemoveExistingLegalEntity_ShouldCreateRemoveAssuredLifeStageTransition()
        {
            //---------------Set up test -------------------
            var lifeaccountkey = base.Service<IAccountService>().GetLifeAccountKey(2, LifePolicyStatusEnum.Inforce);

            //---------------Execute Test ----------------------
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);
            base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstLegalEntity(lifeaccountkey);
            base.Browser.Navigate<LoanServicingCBO>().LifeAccountNode(lifeaccountkey);
            var selectedlegalentityid = base.View.SelectFirstLegalEntity();
            base.View.ClickRemoveButton();
            //Had to move the Pop up handler into the above click method
            //base.View.ConfirmRemoveLifeDialog();

            //---------------Test Result -----------------------
            StageTransitionAssertions.AssertStageTransitionCreated(lifeaccountkey, StageDefinitionStageDefinitionGroupEnum.RemoveAssuredLife);
        }

        [Test]
        public void WhenLifeAddExistingLegalEntity_WhenLegalEntityPlayRoleOnTwoOtherLifeAccounts_ShouldRaiseErrorMessage()
        {
            //---------------Set up test -----------------------
            int lifeaccountkey = base.Service<IAccountService>().GetLifeAccountKey(2, LifePolicyStatusEnum.Inforce);
            var legalentityId = Service<ILegalEntityService>().GetLegalEntityIDNumberPlaySameRoleTwiceDifferentAccounts(RoleTypeEnum.AssuredLife);

            //---------------Assert Precondition----------------
            LifeAssertions.AssertMoreThanOneAssuredLifeRoles(legalentityId);

            //---------------Execute Test ----------------------
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);
            base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstLegalEntity(lifeaccountkey);
            base.Browser.Navigate<LoanServicingCBO>().LifeAccountNode(lifeaccountkey);
            base.View.ClickAddButton();
            //Had to move the Pop up handler into the above click method
            //base.View.ConfirmAddLifeDialog();
            base.Browser.Page<ClientSuperSearch>().PopulateSearch("", "", legalentityId.ToString(), "", "");
            base.Browser.Page<ClientSuperSearch>().PerformSearch();
            base.Browser.Page<ClientSuperSearch>().SelectByIDNumber(legalentityId.ToString());
            base.Browser.Page<LegalEntityDetails>().PopulateLegalEntity(insurableInterest: "Mortgagor");
            base.Browser.Page<LegalEntityDetails>().ClickUpdate();

            //---------------Test Result -----------------------
            base.Browser.Page<BasePage>().CheckForerrorMessages("The selected Legal Entity is already covered on 2 Life Policies.");
        }

        [Test]
        public void WhenLifePremiumRecalcCancelled_ShouldNotPostBondProtectionPlanPremiumCorrection()
        {
            //---------------Set up test -----------------------
            int lifeaccountkey = base.Service<IAccountService>().GetLifeAccountKey(2, LifePolicyStatusEnum.Inforce);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);
            base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstLegalEntity(lifeaccountkey);
            base.Browser.Navigate<LoanServicingCBO>().LifeAccountNode(lifeaccountkey);
            base.View.ClickRecalculatePremium();

            BuildingBlocks.Timers.GeneralTimer.Wait(2000);

            //---------------Test Result -----------------------
            TransactionAssertions.AssertFinancialTransactionNotExists(lifeaccountkey, DateTime.Now, TransactionTypeEnum.BondProtectionPlanPremiumCorrection);
        }

        [Test]
        public void WhenLifePremiumRecalc_ShouldCreateRecalculatePremiumsStageTransition()
        {
            //---------------Set up test -----------------------
            int lifeaccountkey = base.Service<IAccountService>().GetLifeAccountKey(2, LifePolicyStatusEnum.Inforce);
            var accountBalances = base.Service<IAccountService>().GetAccountBalances(lifeaccountkey);
            decimal lifeBalance = (from a in accountBalances where a.BalanceTypeKey == (int)BalanceTypeEnum.Loan select a.Amount).FirstOrDefault();
            base.Service<IAccountService>().UpdateFinancialServiceBalance(lifeaccountkey, FinancialServiceTypeEnum.LifePolicy, lifeBalance * 0.5M);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);
            base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstLegalEntity(lifeaccountkey);
            base.Browser.Navigate<LoanServicingCBO>().LifeAccountNode(lifeaccountkey);
            base.View.ClickRecalculatePremium();

            BuildingBlocks.Timers.GeneralTimer.Wait(2000);

            //---------------Test Result -----------------------
            StageTransitionAssertions.AssertStageTransitionCreated(lifeaccountkey, StageDefinitionStageDefinitionGroupEnum.RecalculatePremiums);
        }
    }
}