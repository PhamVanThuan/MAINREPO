using Automation.DataModels;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using NUnit.Framework;
using System;
using System.DirectoryServices;
using WatiN.Core;
using WatiN.Core.Logging;
using WorkflowAutomation.Harness;

namespace BuildingBlocks
{
    /// <summary>
    /// It the base class of all testsuites, something that every test will share and to enfore wanted behaviour.
    /// </summary>
    /// <typeparam name="TestView"></typeparam>
    [TestFixture]
    public abstract class TestBase<TestView> where TestView : ObjectMaps.Pages.BasePageControls, new()
    {
        public IX2ScriptEngine scriptEngine { get; protected set; }

        [TestFixtureSetUp]
        protected virtual void OnTestFixtureSetup()
        {
            Settings.WaitForCompleteTimeOut = 120;
            Service<IWatiNService>().KillAllIEProcesses();
            ConsoleLogWriter clw = new ConsoleLogWriter { HandlesLogAction = true, HandlesLogDebug = false };
            Logger.LogWriter = clw;
            scriptEngine = new X2ScriptEngine();
        }

        [TestFixtureTearDown]
        protected virtual void OnTestFixtureTearDown()
        {
            if (Browser != null)
            {
                this.Browser.Dispose();
                this.Browser = null;
                Service<IWatiNService>().KillAllIEProcesses();
            }
            scriptEngine = null;
        }

        [SetUp]
        protected virtual void OnTestStart()
        {
            //Need to add something in here to notify us when the browser is in
            //a state that we can't work with it.
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

        public void FailTest(string reason)
        {
            throw new NUnit.Framework.SuccessException(String.Format("Test was failed intentionally, Reason: {0}", reason));
        }
    }
}