using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation;
using BuildingBlocks.Navigation.CBO.LoanServicing;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.PersonalLoans;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;

namespace PersonalLoansTests.LoanServicing
{
    [RequiresSTA]
    public class LifePolicyClaimTests : PersonalLoansWorkflowTestBase<BuildingBlocks.Presenters.PersonalLoans.LifePolicyClaim>
    {
        private Automation.DataModels.FinancialService creditLifePolicyFS;
        private Automation.DataModels.Account plAccount;

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new BuildingBlocks.TestBrowser(TestUsers.LifeAdmin);
            int accountKey = Helper.FindPersonalLoanAccount(true);
            this.plAccount = Service<IAccountService>().GetAccountByKey(accountKey);
            this.creditLifePolicyFS = base.Service<ILifeService>().GetCreditLifePolicyFinancialService(this.plAccount.AccountKey);
            base.Service<NavigationHelper>().Menu(base.Browser);
            base.Service<NavigationHelper>().LegalEntityMenu(base.Browser);
            base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstLegalEntity(this.plAccount.AccountKey);
            base.Browser.Navigate<LoanServicingCBO>().LifeAccountNode(creditLifePolicyFS.AccountKey);
        }

        protected override void OnTestStart()
        {
            //Cleanup life policy claims
            base.Service<ILifeService>().ClearCreditLifePolicyClaims(creditLifePolicyFS.FinancialServiceKey);
            base.OnTestStart();
        }

        protected override void OnTestTearDown()
        {
            base.Service<ILifeService>().ClearCreditLifePolicyClaims(creditLifePolicyFS.FinancialServiceKey);
            base.OnTestTearDown();
        }

        [Test]
        public void when_updating_claim_should_not_be_able_to_update_claim_type()
        {
            base.Service<ILifeService>().InsertCreditLifePolicyClaim(new Automation.DataModels.LifePolicyClaim()
                 {
                     FinancialServiceKey = creditLifePolicyFS.FinancialServiceKey,
                     ClaimStatusKey = ClaimStatusEnum.Pending,
                     ClaimTypeKey = ClaimTypeEnum.DeathClaim,
                     ClaimDate = DateTime.Now
                 });
            base.Browser.Navigate<LoanServicingCBO>().LifePolicyClaim(NodeTypeEnum.Update);
            base.View.SelectClaim(ClaimType.DeathClaim, ClaimStatus.Pending);
            base.View.AssertClaimTypeReadonly();
        }

        [Test]
        public void when_adding_claims_should_not_be_able_to_capture_future_date()
        {
            var date = DateTime.Now.AddDays(1);
            base.Browser.Navigate<LoanServicingCBO>().LifePolicyClaim(NodeTypeEnum.Add);
            base.View.PopulateView(ClaimType.DeathClaim, ClaimStatus.Pending, date);
            base.View.ClickAdd();
            base.View.AssertThatFutureDateCannotBeCaptured();
            LifeAssertions.AssertNoCreditLifePolicyClaim(creditLifePolicyFS, ClaimTypeEnum.DeathClaim, ClaimStatusEnum.Pending, date);
        }

        [Test]
        public void when_adding_claim_should_create_new_life_policy_claim()
        {
            base.Browser.Navigate<LoanServicingCBO>().LifePolicyClaim(NodeTypeEnum.Add);
            var date = DateTime.Now;
            base.View.PopulateView(ClaimType.DeathClaim, ClaimStatus.Pending, date);
            base.View.ClickAdd();
            LifeAssertions.AssertCreditLifePolicyClaim(creditLifePolicyFS, ClaimTypeEnum.DeathClaim, ClaimStatusEnum.Pending, date);
        }

        [Test]
        public void when_updating_claim_should_update_selected_claim()
        {
            base.Service<ILifeService>().InsertCreditLifePolicyClaim(new Automation.DataModels.LifePolicyClaim()
            {
                FinancialServiceKey = creditLifePolicyFS.FinancialServiceKey,
                ClaimStatusKey = ClaimStatusEnum.Pending,
                ClaimTypeKey = ClaimTypeEnum.DeathClaim,
                ClaimDate = DateTime.Now
            });
            base.Browser.Navigate<LoanServicingCBO>().LifePolicyClaim(NodeTypeEnum.Update);
            base.View.SelectClaim(ClaimType.DeathClaim, ClaimStatus.Pending);
        }

        [Test]
        public void when_adding_claim_should_have_correct_claim_status_in_dropdown()
        {
            base.Browser.Navigate<LoanServicingCBO>().LifePolicyClaim(NodeTypeEnum.Add);
            base.View.AssertClaimStatusses();
        }

        [Test]
        public void when_updating_claim_should_have_correct_claim_status_in_dropdown()
        {
            base.Service<ILifeService>().InsertCreditLifePolicyClaim(new Automation.DataModels.LifePolicyClaim()
            {
                FinancialServiceKey = creditLifePolicyFS.FinancialServiceKey,
                ClaimStatusKey = ClaimStatusEnum.Pending,
                ClaimTypeKey = ClaimTypeEnum.DeathClaim,
                ClaimDate = DateTime.Now
            });
            base.Browser.Navigate<LoanServicingCBO>().LifePolicyClaim(NodeTypeEnum.Update);
            base.View.AssertClaimStatusses();
        }

        [Test]
        public void when_adding_claim_should_have_correct_claim_type_in_dropdown()
        {
            base.Browser.Navigate<LoanServicingCBO>().LifePolicyClaim(NodeTypeEnum.Add);
            base.View.AssertClaimTypes();
        }

        [Test]
        public void when_viewing_loan_summary_should_display_warning_if_claims_are_captured()
        {
            var date = DateTime.Now;
            base.Service<ILifeService>().InsertCreditLifePolicyClaim(new Automation.DataModels.LifePolicyClaim()
            {
                FinancialServiceKey = creditLifePolicyFS.FinancialServiceKey,
                ClaimStatusKey = ClaimStatusEnum.Pending,
                ClaimTypeKey = ClaimTypeEnum.DeathClaim,
                ClaimDate = date
            });
            base.Browser.Navigate<LoanServicingCBO>().LifeAccountNode(this.plAccount.AccountKey);
            base.Browser.Page<UnsecuredLoanSummary>().AssertPendingLifePolicyClaimDisplayed(date);
        }
    }
}