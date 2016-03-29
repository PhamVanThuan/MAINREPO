using Automation.DataAccess;
using Automation.DataModels;
using BuildingBlocks;
using BuildingBlocks.Navigation;
using BuildingBlocks.Navigation.CBO.LoanServicing;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using System.Collections.Generic;
using WatiN.Core;

namespace DisabilityClaimTests
{
    public abstract class DisabilityClaimsWorkflowTestBase<TestView> : TestBase<TestView> where TestView : ObjectMaps.Pages.BasePageControls, new()
    {
        protected override void OnTestFixtureSetup()
        {
            Service<IWatiNService>().KillAllIEProcesses();
            Service<IWatiNService>().SetWatiNTimeouts(120);
            base.Browser = new TestBrowser(TestUsers.LifeClaimsAssessor, TestUsers.Password);
        }

        protected override void OnTestStart()
        {
            Settings.WaitForCompleteTimeOut = 120;
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Task();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(base.Browser);
        }

        protected override void OnTestFixtureTearDown()
        {
            Service<IWatiNService>().KillAllIEProcesses();
            if (base.Browser != null)
            {
                base.Browser.Dispose();
            }
        }

        protected override void OnTestTearDown()
        {
            base.OnTestTearDown();            
        }

        protected QueryResults GetOpenLifeAccountWithAssuredLife()
        {
            return base.Service<IAccountService>().GetOpenLifeAccountWithAssuredLife();
        }

        protected DisabilityClaim GetDisabilityClaim(int disabilityClaimKey)
        {
            return base.Service<IDisabilityClaimService>().GetDisabilityClaim(disabilityClaimKey);
        }

        protected DisabilityClaim GetDisabilityClaimByLegalEntityAndAccountKey(int legalEntityKey, int accountKey)
        {
            return base.Service<IDisabilityClaimService>().GetDisabilityClaimByLegalEntityAndAccountKey(legalEntityKey, accountKey);
        }

        protected int GetInstanceID(int disabilityClaimKey)
        {
            return Service<IX2WorkflowService>().GetDisabilityClaimInstanceDetails(disabilityClaimKey).Rows(0).Column("InstanceID").GetValueAs<int>();
        }

        protected void CreateDisabilityClaim(int accountKey)
        {
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);
            base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstLegalEntity(accountKey);
            base.Browser.Navigate<LoanServicingCBO>().LifeAccountNode(accountKey);
            base.Browser.Navigate<LoanServicingCBO>().CreateDisabilityClaim();
        }
    }
}