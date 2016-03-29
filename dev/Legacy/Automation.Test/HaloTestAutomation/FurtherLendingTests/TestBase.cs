using BuildingBlocks;
using BuildingBlocks.Services.Contracts;
using NUnit.Framework;
using WatiN.Core;
using WorkflowAutomation.Harness;

namespace FurtherLendingTests
{
    [TestFixture]
    public abstract class TestBase<TestView>
        where TestView : ObjectMaps.Pages.BasePageControls, new()
    {
        [TestFixtureSetUp]
        protected virtual void OnTestFixtureSetup()
        {
            Service<ICommonService>().DeleteTestMethodDataForFixture("FurtherLendingTests");
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
            if (this.Browser != null)
            {
                this.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(this.Browser);
            }
        }

        [TearDown]
        protected virtual void OnTestTearDown()
        {
            if (this.FLSupervisorBrowser != null)
            {
                this.FLSupervisorBrowser.Dispose();
                this.FLSupervisorBrowser = null;
            }

            if (this.FLProcessorBrowser != null)
            {
                this.FLProcessorBrowser.Dispose();
                this.FLProcessorBrowser = null;
            }
        }

        protected TestView View
        {
            get
            {
                return (TestView)this.Browser.Page<TestView>();
            }
        }

        protected TestBrowser Browser { get; set; }

        protected TestBrowser FLSupervisorBrowser { get; set; }

        protected TestBrowser FLProcessorBrowser { get; set; }

        protected IX2ScriptEngine scriptEngine { get; set; }

        public T Service<T>()
        {
            return ServiceLocator.Instance.GetService<T>();
        }
    }
}