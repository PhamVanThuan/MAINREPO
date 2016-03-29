using BuildingBlocks;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using NUnit.Framework;
using WatiN.Core;
using WatiN.Core.Logging;
using WorkflowAutomation.Harness;

namespace CAP2Tests
{
    [TestFixture]
    public abstract class TestBase<TestView> where TestView : ObjectMaps.Pages.BasePageControls, new()
    {
        protected IX2ScriptEngine scriptEngine { get; private set; }

        [TestFixtureSetUp]
        protected virtual void OnTestFixtureSetup()
        {
            Service<IWatiNService>().KillAllIEProcesses();
            Logger.LogWriter = new ConsoleLogWriter();
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
            if (this.Browser != null)
            {
                if (Browser.Page<BasePage>().CheckForErrorMessages())
                    this.Browser.Refresh();
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

        public T Service<T>()
        {
            return ServiceLocator.Instance.GetService<T>();
        }
    }
}