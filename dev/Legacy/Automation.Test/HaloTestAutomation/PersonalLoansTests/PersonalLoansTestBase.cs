using BuildingBlocks;
using BuildingBlocks.Navigation.CBO.LoanServicing;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Linq;
using WatiN.Core;
using WorkflowAutomation.Harness;
using SAHL.Core.BusinessModel.Enums;

namespace PersonalLoansTests
{
    public abstract class PersonalLoansWorkflowTestBase<TestView> : TestBase<TestView> where TestView : ObjectMaps.Pages.BasePageControls, new()
    {
        protected override void OnTestFixtureSetup()
        {
            Service<IWatiNService>().KillAllIEProcesses();
            Service<IWatiNService>().SetWatiNTimeouts(120);
            RandomGenerator = new Random();
            scriptEngine = new X2ScriptEngine();
        }

        protected override void OnTestFixtureTearDown()
        {
            Service<IWatiNService>().KillAllIEProcesses();
            if (base.Browser != null)
                base.Browser.Dispose();
            scriptEngine = null;
        }

        protected override void OnTestStart()
        {
            Settings.WaitForCompleteTimeOut = 120;
        }

        protected override void OnTestTearDown()
        {
            base.OnTestTearDown();
        }

        protected IX2ScriptEngine scriptEngine { get; set; }

        public int GenericKey { get; set; }

        public int FinancialServiceKey { get; set; }

        public Int64 InstanceID { get; set; }

        public string CaseOwner { get; set; }

        public Random RandomGenerator { get; set; }

        public Automation.DataModels.PersonalLoanApplication personalLoanApplication { get; set; }

        public Automation.DataModels.Account PersonalLoanAccount { get; set; }

        public new Automation.DataModels.ExternalLifePolicy ExternalLife { get; set; }

        protected void FindCaseAtState(string state, WorkflowRoleTypeEnum roleToLoginAs, bool isCaseOwner = true)
        {
            if (this.Browser != null)
                this.Browser.Dispose();

            if (isCaseOwner)
            {
                this.GenericKey = Service<IX2WorkflowService>().GetOfferKeyAtStateByType(state, Workflows.PersonalLoans, Common.Enums.OfferTypeEnum.UnsecuredLending, string.Empty, (int)Common.Enums.GeneralStatusEnum.Active);
            }
            else
            {
                this.GenericKey = Service<IX2WorkflowService>().GetOfferKeyAtStateByType(state, Workflows.PersonalLoans, Common.Enums.OfferTypeEnum.UnsecuredLending, string.Empty);
            }
            Assert.True(this.GenericKey > 0, "Failed to find workflow case at workflow state:{0}", state);
            this.InstanceID = Service<IX2WorkflowService>().GetPersonalLoanInstanceId(this.GenericKey);
            RefreshPersonalApplication();
            //get owner
            if (isCaseOwner)
            {
                var caseOwner = Service<IAssignmentService>().GetActiveWorkflowRoleByTypeAndGenericKey(roleToLoginAs, GenericKey);
                //get the worklist owner
                this.CaseOwner = (from c in caseOwner select c.Value).FirstOrDefault();
                this.Browser = new TestBrowser(this.CaseOwner, TestUsers.Password);
            }
            else
            {
                var adusers = from r in Service<IAssignmentService>().GetAdUsersForWorkflowRoleType(roleToLoginAs)
                              where r.Column("generalstatuskey").Value.Equals("1")
                              select r.Column("adusername").GetValueAs<string>();
                this.Browser = new TestBrowser(adusers.FirstOrDefault(), TestUsers.Password);
            }
            this.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Task();
            this.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(this.Browser);
            this.Browser.Page<WorkflowSuperSearch>().SearchByUniqueIdentifierAndApplicationType(this.Browser, this.GenericKey.ToString(), new string[] { state }, ApplicationType.Any);
        }

        /// <summary>
        /// Finds a personal loan client that either has or has not got a credit life policy
        /// </summary>
        /// <param name="hasCreditLifePolicy"></param>
        protected void FindPersonalLoanAccountAndLoadIntoLoanServicing(bool hasCreditLifePolicy, string testUser)
        {
            this.GenericKey = Helper.FindPersonalLoanAccount(hasCreditLifePolicy);
            this.PersonalLoanAccount = Service<IAccountService>().GetAccountByKey(this.GenericKey);
            this.Browser = new TestBrowser(testUser, TestUsers.Password);
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);
            base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstLegalEntity(PersonalLoanAccount.AccountKey);
            base.Browser.Navigate<LoanServicingCBO>().ParentAccountNode(PersonalLoanAccount.AccountKey);
        }

        protected void SearchAndLoadAccount()
        {
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);
            base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstLegalEntity(this.GenericKey);
            base.Browser.Navigate<LoanServicingCBO>().ParentAccountNode(this.GenericKey);
        }

        protected void ReloadCase(string state, WorkflowRoleTypeEnum roleToLoginAs)
        {
            if (this.Browser != null)
                this.Browser.Dispose();
            RefreshPersonalApplication();
            //get owner
            var caseOwner = Service<IAssignmentService>().GetActiveWorkflowRoleByTypeAndGenericKey(roleToLoginAs, this.GenericKey);
            this.CaseOwner = (from c in caseOwner select c.Value).FirstOrDefault();
            this.Browser = new TestBrowser(CaseOwner);
            this.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Task();
            this.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(this.Browser);
            this.Browser.Page<WorkflowSuperSearch>().SearchByUniqueIdentifierAndApplicationType(this.Browser, this.GenericKey.ToString(),
                    new string[] { state }, ApplicationType.Any);
        }

        protected void RefreshPersonalApplication()
        {
            this.personalLoanApplication = Service<IApplicationService>().GetPersonalLoanApplication(this.GenericKey);
        }

        protected void CreateMandatoryDataForSubmissionToCredit()
        {
            //Create mandatory data
            UpdateDeclarations();
            base.Service<IApplicationService>().InsertEmploymentRecords(this.GenericKey);
            base.Service<IApplicationService>().InsertOfferMailingAddress(this.GenericKey);
            base.Service<IApplicationService>().CleanUpOfferDebitOrder(this.GenericKey);
            base.Service<ILegalEntityService>().InsertITC(this.GenericKey, 4, 2);
            base.Service<ILegalEntityService>().CreateApplicationAffordabilities(this.GenericKey, (int)AffordabilityAssessmentStatus.Confirmed);
            base.Service<IApplicationService>().InsertExternalRoleDomicilium(this.GenericKey);
            base.Service<IApplicationService>().CleanUpLegalEntityRequiredFields(this.GenericKey);
        }

        protected void UpdateDeclarations()
        {
            base.Service<IApplicationService>().DeleteExternalRoleDeclarations(this.GenericKey);
            base.Service<IApplicationService>().InsertDeclarations(this.GenericKey, GenericKeyTypeEnum.ExternalRoleType_ExternalRoleTypeKey, OriginationSourceProductEnum.SAHomeLoans_PersonalLoan);
        }

        protected void FindAndLoadPersonalLoanApplication(string state, Common.Enums.WorkflowRoleTypeEnum roleToLoginAs, bool hasSAHLLife, bool hasExternalLife)
        {
            FindPersonalLoanApplication(state, roleToLoginAs, hasSAHLLife, hasExternalLife);
            if (this.GenericKey != 0)
                ReloadCase(state, roleToLoginAs);
        }

        protected void FindPersonalLoanApplication(string state, Common.Enums.WorkflowRoleTypeEnum roleToLoginAs, bool hasSAHLLife, bool hasExternalLife)
        {
            this.GenericKey = 0;
            this.CaseOwner = string.Empty;
            var offer = Service<IX2WorkflowService>().GetPersonalLoanOfferAtState(state, hasSAHLLife, hasExternalLife);
            if (offer != null)
            {
                this.GenericKey = offer.OfferKey;
                var caseOwner = Service<IAssignmentService>().GetActiveWorkflowRoleByTypeAndGenericKey(roleToLoginAs, this.GenericKey);
                this.CaseOwner = (from c in caseOwner select c.Value).FirstOrDefault();
                RefreshPersonalApplication();
            }
        }

        protected Automation.DataModels.ExternalLifePolicy CreateDefaultExternalCreditLife()
        {
            var random = new Random();
            RefreshPersonalApplication();
            return this.ExternalLife = new Automation.DataModels.ExternalLifePolicy()
            {
                InsurerKey = (int)Common.Enums.Insurer.ABSALife,
                Insurer = new Automation.DataModels.Insurer() { InsurerKey = (int)Common.Enums.Insurer.ABSALife, Descripton = Common.Constants.Insurer.ABSALife },
                PolicyNumber = random.Next(1000, 10000).ToString(),
                CommencementDate = DateTime.Now,
                LifePolicyStatusKey = (int)Common.Enums.LifePolicyStatusEnum.Inforce,
                LifePolicyStatus = new Automation.DataModels.LifePolicyStatus() { PolicyStatusKey = (int)Common.Enums.LifePolicyStatusEnum.Inforce, Description = Common.Constants.lifePolicyStatus.Inforce },
                SumInsured = personalLoanApplication.LoanAmount,
                PolicyCeded = true
            };
        }

        protected void FindPersonalLoanAccountAndLoadIntoLoanServicing(string testUser)
        {
            if (this.Browser != null)
                this.Browser.Dispose();
            this.PersonalLoanAccount = Service<IAccountService>().GetAccountByKey(personalLoanApplication.ReservedAccountKey);
            this.Browser = new TestBrowser(testUser, TestUsers.Password);
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);
            base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstLegalEntity(PersonalLoanAccount.AccountKey);
            base.Browser.Navigate<LoanServicingCBO>().ParentAccountNode(PersonalLoanAccount.AccountKey);
        }
    }
}