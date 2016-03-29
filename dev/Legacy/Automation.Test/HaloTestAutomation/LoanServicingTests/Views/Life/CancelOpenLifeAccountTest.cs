using BuildingBlocks;
using BuildingBlocks.Navigation.CBO.LoanServicing;
using BuildingBlocks.Presenters.Life;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System.Linq;

namespace LoanServicingTests.Views.Life
{
    /// <summary>
    /// TODO: rework this for life test.
    /// </summary>
    [TestFixture, RequiresSTA]
    public sealed class CancelOpenLifeAccountTest : TestBase<Life_CancelPolicy>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.HaloUser, TestUsers.Password);
        }

        protected override void OnTestStart()
        {
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);
            base.OnTestStart();
        }

        [Test]
        public void CancelLifePolicy_WhenAcceptedLifePolicyCancelledFromInception_ShouldEnableCancelledFromInceptionCheckbox()
        {
            var accountkey
               = (from policy in base.Service<BuildingBlocks.Services.LifeService>().GetLifePolicyAccounts(1, LifePolicyStatusEnum.Accepted, false, false)
                  select policy.AccountKey).FirstOrDefault();
            base.Browser.Page<BuildingBlocks.Presenters.CommonPresenters.ClientSuperSearch>();
            base.Browser.Page<BuildingBlocks.Presenters.CommonPresenters.ClientSuperSearch>().SearchByAccountKeyForFirstLegalEntity(accountkey);
            base.Browser.Navigate<BuildingBlocks.Navigation.CBO.LoanServicing.LoanServicingCBO>().LifeAccountNode(accountkey);
            base.Browser.Navigate<LoanServicingCBO>().CancelLifePolicy();
            base.View.AssertCancelledFromInceptionEnabled();
        }

        [Test]
        public void CancelLifePolicy_WhenInforedLifePolicyCancelledWithProRata_ShouldEnableCancelledWithProRataCheckbox()
        {
            var accountkey
             = (from policy in base.Service<BuildingBlocks.Services.LifeService>().GetLifePolicyAccounts(1, LifePolicyStatusEnum.Inforce, false, false)
                select policy.AccountKey).FirstOrDefault();
            base.Browser.Page<BuildingBlocks.Presenters.CommonPresenters.ClientSuperSearch>();
            base.Browser.Page<BuildingBlocks.Presenters.CommonPresenters.ClientSuperSearch>().SearchByAccountKeyForFirstLegalEntity(accountkey);
            base.Browser.Navigate<BuildingBlocks.Navigation.CBO.LoanServicing.LoanServicingCBO>().LifeAccountNode(accountkey);
            base.Browser.Navigate<LoanServicingCBO>().CancelLifePolicy();
            base.View.AssertCancelledWithProRataEnabled();
        }

        [Test]
        public void CancelLifePolicy_WhenAcceptedLifePolicyCancelledFromInception_ShouldSucessfullySave()
        {
            var accountkey
                = (from policy in base.Service<BuildingBlocks.Services.LifeService>().GetLifePolicyAccounts(1, LifePolicyStatusEnum.Accepted, false, false)
                   select policy.AccountKey).FirstOrDefault();
            base.Browser.Page<BuildingBlocks.Presenters.CommonPresenters.ClientSuperSearch>();
            base.Browser.Page<BuildingBlocks.Presenters.CommonPresenters.ClientSuperSearch>().SearchByAccountKeyForFirstLegalEntity(accountkey);
            base.Browser.Navigate<BuildingBlocks.Navigation.CBO.LoanServicing.LoanServicingCBO>().LifeAccountNode(accountkey);
            base.Browser.Navigate<LoanServicingCBO>().CancelLifePolicy();
            base.View.Populate("Other", LifePolicyStatusEnum.Accepted);
            base.View.Submit();
        }

        [Test]
        public void CancelLifePolicy_WhenInforedLifePolicyCancelledWithProRata_ShouldSucessfullySave()
        {
            var accountkey
               = (from policy in base.Service<BuildingBlocks.Services.LifeService>().GetLifePolicyAccounts(1, LifePolicyStatusEnum.Inforce, false, false)
                  select policy.AccountKey).FirstOrDefault();
            base.Browser.Page<BuildingBlocks.Presenters.CommonPresenters.ClientSuperSearch>();
            base.Browser.Page<BuildingBlocks.Presenters.CommonPresenters.ClientSuperSearch>().SearchByAccountKeyForFirstLegalEntity(accountkey);
            base.Browser.Navigate<BuildingBlocks.Navigation.CBO.LoanServicing.LoanServicingCBO>().LifeAccountNode(accountkey);
            base.Browser.Navigate<LoanServicingCBO>().CancelLifePolicy();
            base.View.Populate("Other", LifePolicyStatusEnum.Inforce);
            base.View.Submit();
        }
    }
}