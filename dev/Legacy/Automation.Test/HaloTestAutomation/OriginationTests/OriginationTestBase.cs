using Automation.DataModels;
using BuildingBlocks;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using WatiN.Core;
using WatiN.Core.Logging;
using WorkflowAutomation.Harness;

namespace Origination
{
    [TestFixture]
    public abstract class OriginationTestBase<TestView> : TestBase<TestView> where TestView : ObjectMaps.Pages.BasePageControls, new()
    {

        [TestFixtureSetUp]
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            Service<IWatiNService>().KillAllIEProcesses();
            ConsoleLogWriter clw = new ConsoleLogWriter { HandlesLogAction = true, HandlesLogDebug = false };
            Logger.LogWriter = clw;
        }

        [SetUp]
        protected override void OnTestStart()
        {
            Settings.WaitForCompleteTimeOut = 120;
            Service<IWatiNService>().KillAllIEProcesses();
        }

        [TearDown]
        protected override void OnTestTearDown()
        {
            this.TestCase = null;
            base.OnTestTearDown();
        }

        protected OriginationTestCase TestCase { get; set; }

        protected void GetOfferKeyAtStateAndSearchForCase(string workflowState, OfferTypeEnum offerType)
        {
            int offerKey = Service<IX2WorkflowService>().GetOfferKeyAtStateByType(workflowState, Workflows.ApplicationManagement,
                offerType, Exclusions.OrginationAutomation);
            this.TestCase = new OriginationTestCase() { OfferKey = offerKey };
            this.TestCase.InstanceID = Service<IX2WorkflowService>().GetAppManInstanceIDByState(workflowState, this.TestCase.OfferKey, false);
            SearchForCase(workflowState);
        }

        protected void GetOfferWithoutValuationsCloneAtState(string valuationsWorkflowState, int requireValuationFlag, int includeExclude)
        {
            int offerKey = Service<IX2WorkflowService>().GetAppManOfferKey_FilterByClone(WorkflowStates.ApplicationManagementWF.ManageApplication, null, includeExclude,
                valuationsWorkflowState, requireValuationFlag, (int)OfferTypeEnum.NewPurchase);
            this.TestCase = new Automation.DataModels.OriginationTestCase() { OfferKey = offerKey };
            this.SearchForCase(WorkflowStates.ApplicationManagementWF.ManageApplication);
        }

        protected void SearchForCase(string workflowState)
        {
            base.Browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().WorkFlows(this.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(this.Browser);
            base.Browser.Page<WorkflowSuperSearch>().Search(this.Browser, this.TestCase.OfferKey, workflowState);
        }
    }
}