using System;
using System.Linq;
using BuildingBlocks;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using WatiN.Core;
using WatiN.Core.Logging;
using WorkflowAutomation.Harness;

namespace DebtCounsellingTests
{
    [TestFixture]
    public abstract class TestBase<TestView> where TestView : ObjectMaps.Pages.BasePageControls, new()
    {
        [TestFixtureSetUp]
        protected virtual void OnTestFixtureSetup()
        {
            ConsoleLogWriter clw = new ConsoleLogWriter { HandlesLogAction = true, HandlesLogDebug = false };
            Logger.LogWriter = clw;
            scriptEngine = new X2ScriptEngine();
            Service<ICommonService>().DeleteTestMethodDataForFixture("DebtCounselling");
            Service<ICommonService>().DeleteTestMethodDataForFixture("DebtCounselling.WorkFlow.WorkflowTests");
            Service<IWatiNService>().KillAllIEProcesses();
            this.Service<IADUserService>().UpdateStatusOfAllUsersMappedToWorkflowRoleType(WorkflowRoleTypeEnum.DebtCounsellingConsultantD, GeneralStatusEnum.Inactive, true, true, true);
            this.Service<IADUserService>().UpdateStatusOfAllUsersMappedToWorkflowRoleType(WorkflowRoleTypeEnum.DebtCounsellingCourtConsultantD, GeneralStatusEnum.Inactive, true, true, true);
            this.Service<IADUserService>().UpdateADUserStatus(TestUsers.DebtCounsellingConsultant, GeneralStatusEnum.Active, GeneralStatusEnum.Active, GeneralStatusEnum.Active);
            this.Service<IADUserService>().UpdateADUserStatus(TestUsers.DebtCounsellingCourtConsultant, GeneralStatusEnum.Active, GeneralStatusEnum.Active, GeneralStatusEnum.Active);
            //reload the cache
            this.scriptEngine.ClearCacheFor(Processes.DebtCounselling, Workflows.DebtCounselling, CacheTypes.DomainService);
        }

        [TestFixtureTearDown]
        protected virtual void OnTestFixtureTearDown()
        {
            this.Service<IADUserService>().UpdateStatusOfAllUsersMappedToWorkflowRoleType(WorkflowRoleTypeEnum.DebtCounsellingConsultantD, GeneralStatusEnum.Active, true, true, true);
            this.Service<IADUserService>().UpdateStatusOfAllUsersMappedToWorkflowRoleType(WorkflowRoleTypeEnum.DebtCounsellingCourtConsultantD, GeneralStatusEnum.Active,
                true, true, true);
            this.Service<IADUserService>().DeactivateDebtCounsellingBusinessUsers();
            //reload the cache
            this.scriptEngine.ClearCacheFor(Processes.DebtCounselling, Workflows.DebtCounselling, CacheTypes.DomainService);
            Service<IWatiNService>().KillAllIEProcesses();
            if (this.Browser != null)
                this.Browser.Dispose();
            this.scriptEngine = null;
        }

        [SetUp]
        protected virtual void OnTestStart()
        {
            Settings.WaitForCompleteTimeOut = 120;
            if (this.Browser != null)
            {
                this.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(Browser);
            }
        }

        [TearDown]
        protected virtual void OnTestTearDown()
        {
            if (this.Browser != null)
            {
                if (this.Browser.Page<BasePage>().CheckForErrorMessages())
                    this.Browser.Refresh();
            }
            this.TestCase = new Automation.DataModels.DebtCounselling { DebtCounsellingKey = 0 };
        }

        protected TestView View
        {
            get
            {
                return (TestView)this.Browser.Page<TestView>();
            }
        }

        public T Service<T>()
        {
            return ServiceLocator.Instance.GetService<T>();
        }

        protected TestBrowser Browser { get; set; }

        protected Automation.DataModels.DebtCounselling TestCase { get; set; }

        protected IX2ScriptEngine scriptEngine { get; set; }

        public void LoadCase(params string[] state)
        {
            if (this.Browser != null)
            {
                this.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Task();
                this.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(Browser);
                this.Browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().WorkFlows(Browser);
                this.Browser.Page<WorkflowSuperSearch>().SearchByUniqueIdentifierAndApplicationType(Browser, this.TestCase.AccountKey.ToString(), state, Workflows.DebtCounselling);
            }
        }

        public void StartTest(string state, string assignedUser, bool eWorkCase = true, int countOfAccounts = 1, bool hasArrearTransactions = true, ProductEnum product = ProductEnum.NewVariableLoan,
                                string idNumber = "", DateTime? date = null, bool isInterestOnly = false, bool searchForCase = true)
        {
            if (searchForCase)
                this.TestCase = WorkflowHelper.SearchForCase(state, assignedUser, productKey: (int)product, isInterestOnly: isInterestOnly);
            if (this.TestCase == null || this.TestCase.DebtCounsellingKey == 0)
            {
                this.TestCase = WorkflowHelper.CreateCaseAndSendToState(state, eWorkCase, countOfAccounts, hasArrearTransactions, product, idNumber, date, isInterestOnly)
                                                .FirstOrDefault(x => x.Account.ProductKey == product);
                if (this.TestCase == null)
                    throw new Exception(String.Format(
                    @"WorkflowHelper.CreateCaseAndSendToState failed with state: {0}, eWorkCase: {1}, countOfAccounts: {2}, hasArrearTransactions: {3}, product: {4}, idNumber: {5}, date: {6}, isInterestOnly: {7}",
                    state, eWorkCase, countOfAccounts, hasArrearTransactions, product, idNumber, date, isInterestOnly));
            }
            LoadCase(state);
        }

        public void ReloadTestCase()
        {
            if (this.TestCase != null && this.TestCase.DebtCounsellingKey > 0)
                this.TestCase = Service<IDebtCounsellingService>().GetDebtCounsellingCases(debtcounsellingkey: this.TestCase.DebtCounsellingKey).FirstOrDefault();
        }

        public void StartTestWithMultipleClientsUnderDebtCounselling(string[] users, params string[] workflowState)
        {
            //we have to search for a case as we need one with multiple LE's on the case.
            int accountKey = Service<IDebtCounsellingService>().GetDebtCounsellingCaseWithMoreThanOneLegalEntity(users, workflowState);
            //remove any existing reasons for any legal entity on the case
            this.TestCase = Service<IDebtCounsellingService>().GetDebtCounsellingCases(accountkey: accountKey).FirstOrDefault();
            this.LoadCase(this.TestCase.StageName);
        }
    }
}