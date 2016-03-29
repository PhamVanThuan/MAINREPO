using BuildingBlocks;
using BuildingBlocks.Navigation.CBO.LoanServicing;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.LoanServicing;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using NUnit.Framework;
using System.Linq;

namespace LoanServicingTests.Views.Account
{
    [RequiresSTA]
    public class LoanSummaryTests : TestBase<LoanSummary>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.HaloUser);
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
        }

        [Test, Description("This test ensures that the SPV description displayed on the loan summary matches its parent SPV's description")]
        public void CheckSPVDescriptionMatchesParentDescription()
        {
            var spvList = Service<ISPVTestService>().GetActiveChildSPVs().Take(3);
            foreach (var spv in spvList)
            {
                //we need an open account
                var account = Service<IAccountService>().GetOpenMortgageLoanAccountInSPV(spv.SPVKey);
                if (account != null)
                {
                    //go to the loan summary
                    int legalEntityKey = base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(account.AccountKey);
                    //we need to navigate to loan adjustments --> change rate
                    base.Browser.Navigate<LoanServicingCBO>().LoanAccountNode(account.AccountKey);
                    base.View.AssertSPVDescription(spv.ParentDescription);
                    base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
                }
            }
        }

        [Test, Description("If the parent SPV description does not exist then display the report description of the accounts SPV")]
        public void SPVDescriptionShowsChildWhenParentDescriptionIsEmpty()
        {
            var spvList = Service<ISPVTestService>().GetActiveChildSPVs();
            var spv = (from s in spvList where s.SPVKey == 148 select s).FirstOrDefault();
            string parentDescription = spv.ParentDescription;
            string newDescription = string.Empty;
            Service<ISPVTestService>().UpdateSPVDescription(spv.ParentSPVKey, newDescription);
            try
            {
                var account = Service<IAccountService>().GetOpenMortgageLoanAccountInSPV(spv.SPVKey);
                if (account != null)
                {
                    //go to the loan summary
                    int legalEntityKey = base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(account.AccountKey);
                    base.Browser.Navigate<LoanServicingCBO>().LoanAccountNode(account.AccountKey);
                    //the parent is empty so we should be showing the child's report description
                    base.View.AssertSPVDescription(spv.ReportDescription);
                    base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
                }
            }
            finally
            {
                //set it back
                Service<ISPVTestService>().UpdateSPVDescription(spv.ParentSPVKey, parentDescription);
            }
        }
    }
}