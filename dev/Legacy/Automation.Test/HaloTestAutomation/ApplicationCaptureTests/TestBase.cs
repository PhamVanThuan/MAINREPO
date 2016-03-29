using BuildingBlocks;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using NUnit.Framework;
using SAHL.Core.Logging;
using System;
using WatiN.Core;
using WorkflowAutomation.Harness;

namespace ApplicationCaptureTests
{
    [TestFixture]
    public abstract class TestBase<TestView> where TestView : ObjectMaps.Pages.BasePageControls, new()
    {
        protected Randomizer randomizer { get; set; }
        [TestFixtureSetUp]
        protected virtual void OnTestFixtureSetup()
        {
            randomizer = new Randomizer();
            Service<IWatiNService>().KillAllIEProcesses();
            scriptEngine = new X2ScriptEngine();
        }

        [TestFixtureTearDown]
        protected virtual void OnTestFixtureTearDown()
        {
            Service<IWatiNService>().KillAllIEProcesses();
            scriptEngine = null;
        }

        [SetUp]
        protected virtual void OnTestStart()
        {
            Settings.WaitForCompleteTimeOut = 120;
        }

        [TearDown]
        protected virtual void OnTestTearDown()
        {
            TestCase = null;
        }

        protected TestView View
        {
            get
            {
                return (TestView)this.Browser.Page<TestView>();
            }
        }

        protected Automation.DataModels.OriginationTestCase TestCase { get; set; }

        protected TestBrowser Browser { get; set; }

        public T Service<T>()
        {
            return ServiceLocator.Instance.GetService<T>();
        }

        public void FailTest(string reason)
        {
            throw new NUnit.Framework.SuccessException(String.Format("Test was failed intentionally, Reason: {0}", reason));
        }

        public void GetTestCase(string identifier, bool isLead = false)
        {
            if (isLead)
            {
                this.TestCase = Service<ICommonService>().GetTestDataAutomationLeads(identifier);
            }
            else
            {
                this.TestCase = Service<ICommonService>().GetTestDataByTestIdentifier(identifier);
            }
        }

        public void SelectCaseFromWorklist(string workflowState)
        {
            Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(Browser);
            Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Task();
            Browser.Page<X2Worklist>().SelectCaseFromWorklist(Browser, workflowState, TestCase.OfferKey);
            Browser.ClickAction(WorkflowActivities.ApplicationCapture.AssignConsultant);
        }

        public void SearchForCase()
        {
            //Clear Nodes
            Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(Browser);
            //Step 1: Select offer from worklist.
            Browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().WorkFlows(Browser);
            Browser.Page<WorkflowSuperSearch>().Search(TestCase.OfferKey);
        }

        protected IX2ScriptEngine scriptEngine { get; set; }
    }
}