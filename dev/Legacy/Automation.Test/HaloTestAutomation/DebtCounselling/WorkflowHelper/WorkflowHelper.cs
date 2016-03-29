using BuildingBlocks;
using BuildingBlocks.Admin;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation.CBO.LoanServicing;
using BuildingBlocks.Navigation.FLOBO.DebtCounselling;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.DebtCounselling;
using BuildingBlocks.Presenters.LoanServicing.LoanDetail;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using Common.Extensions;
using Common.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using WorkflowAutomation.Harness;

using Navigation = BuildingBlocks.Navigation;

namespace DebtCounsellingTests
{
    /// <summary>
    /// Contains helper methods for the debt counselling workflow tests
    /// </summary>
    internal static class WorkflowHelper
    {
        private static IDebtCounsellingService debtCounsellingService;
        private static IDetailTypeService detailTypeService;
        private static IExternalRoleService externalRoleService;
        private static ILegalEntityService legalEntityService;
        private static IX2WorkflowService x2Service;
        private static IAssignmentService assignmentService;
        private static ILegalEntityOrgStructureService legalEntityOrgStructureService;
        private static IAccountService accountService;

        static WorkflowHelper()
        {
            debtCounsellingService = ServiceLocator.Instance.GetService<IDebtCounsellingService>();
            externalRoleService = ServiceLocator.Instance.GetService<IExternalRoleService>();
            legalEntityService = ServiceLocator.Instance.GetService<ILegalEntityService>();
            x2Service = ServiceLocator.Instance.GetService<IX2WorkflowService>();
            assignmentService = ServiceLocator.Instance.GetService<IAssignmentService>();
            detailTypeService = ServiceLocator.Instance.GetService<IDetailTypeService>();
            legalEntityOrgStructureService = ServiceLocator.Instance.GetService<ILegalEntityOrgStructureService>();
            accountService = ServiceLocator.Instance.GetService<IAccountService>();
        }

        /// <summary>
        /// Create a case using a random legalentity
        /// </summary>
        /// <param name="hasEWorkLossControlCase">FALSE=no related loss control case, TRUE=related loss control cases</param>
        /// <param name="countOfAccounts">number of related cases for the LE</param>
        /// <param name="browser">TestBrowser</param>
        /// <param name="hasArrearBalance"></param>
        /// <param name="product"></param>
        /// <param name="idNumber"></param>
        /// <param name="_17pt1Date"></param>
        /// <param name="isInterestOnly"></param>
        /// <param name="onlyOneLegalEntityChecked"></param>
        /// <returns>idNumber</returns>
        internal static string CreateCase(bool hasEWorkLossControlCase, int countOfAccounts, TestBrowser browser, bool hasArrearBalance, DateTime _17pt1Date,
                string idNumber = "", ProductEnum product = ProductEnum.NewVariableLoan, bool onlyOneLegalEntityChecked = false, bool isInterestOnly = false, string reference = "")
        {
            var random = new Random();
            //we need a DC
            string dcNumber = debtCounsellingService.GetNCRNumber();
            //we need an ID Number
            if (string.IsNullOrEmpty(idNumber))
                idNumber = debtCounsellingService.GetLegalEntityIDNumberForDCCreate(hasEWorkLossControlCase, countOfAccounts, hasArrearBalance, product, isInterestOnly);
            //then we need to get the accounts for this legal entity
            //navigate
            browser.Navigate<Navigation.NavigationHelper>().Menu(browser);
            browser.Navigate<Navigation.MenuNode>().LossControlNode();
            browser.Navigate<Navigation.MenuNode>().CreateCase();
            browser.Page<DebtCounsellorSelect>().SearchByNCRNumber(dcNumber);
            browser.Page<DebtCounsellorSelect>().ClickButton(ButtonTypeEnum.Select);

            //step two - add 17.1 date
            browser.Page<DebtCounsellingCreateCase>().PopulateView(_17pt1Date);
            if (string.IsNullOrEmpty(reference))
                reference = string.Format("DCRef-{0}-{1}", random.Next(0, 100000).ToString(), random.Next(0, 1000).ToString());
            //step three - add test reference
            browser.Page<DebtCounsellingCreateCase>().AddReference(reference);

            //step four
            browser.Page<DebtCounsellingCreateCase>().SearchAndAddPeopleofImportance(idNumber);

            //By default all legalentity checked.
            if (!onlyOneLegalEntityChecked)
                browser.Page<DebtCounsellingCreateCase>().SelectAllCheckBoxes();
            browser.Page<DebtCounsellingCreateCase>().ClickCreateCase();
            browser.WaitForComplete();
            return idNumber;
        }

        /// <summary>
        /// Create a debtcounselling case from the provided accountkey
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="idNumber"></param>
        /// <param name="isUnderDebtCounselling"></param>
        /// <param name="browser"></param>
        /// <param name="comment"></param>
        internal static void CreateCase(int accountKey, string idNumber, bool isUnderDebtCounselling, TestBrowser browser, string comment = "")
        {
            //we need a DC
            string dcNumber = debtCounsellingService.GetNCRNumber();
            //navigate
            browser.Navigate<Navigation.NavigationHelper>().Menu(browser);
            browser.Navigate<Navigation.MenuNode>().CloseLoanNodesCBO();
            browser.Navigate<Navigation.MenuNode>().LossControlNode();
            browser.Navigate<Navigation.MenuNode>().CreateCase();
            browser.Page<DebtCounsellorSelect>().SearchByNCRNumber(dcNumber);
            browser.Page<DebtCounsellorSelect>().ClickButton(ButtonTypeEnum.Select);
            //step two
            browser.Page<DebtCounsellingCreateCase>().PopulateView(DateTime.Now);
            //step three
            int legalentityKey = legalEntityService.GetLegalEntityKeyByIdNumber(idNumber);
            browser.Page<DebtCounsellingCreateCase>().SearchAndAddPeopleofImportance(idNumber);
            browser.Page<DebtCounsellingCreateCase>().DeselectAllCheckBoxes();
            browser.Page<DebtCounsellingCreateCase>().SelectLegalEntitiesFromTree(legalentityKey, accountKey, isUnderDebtCounselling);
            browser.Page<DebtCounsellingCreateCase>().ClickCreateCase();
            DebtCounsellingAssertions.AssertDebtCounselingCase(accountKey);
            browser.WaitForComplete();
        }

        /// <summary>
        /// This will perform the NegotiateProposal action on the case and click Yes on the WorkflowYesNo view.
        /// </summary>
        /// <param name="browser">ie browser</param>
        /// <param name="accountkey">debtcounselling account</param>
        /// <param name="instanceID"></param>
        internal static void NegotiateProposal(TestBrowser browser, int accountkey, Int64 instanceID)
        {
            browser.ClickAction(WorkflowActivities.DebtCounselling.NegotiateProposal);
            browser.Page<WorkflowYesNo>().Confirm(true, false);
            x2Service.WaitForX2WorkflowHistoryActivity(WorkflowActivities.DebtCounselling.NegotiateProposal, instanceID, 1);
        }

        /// <summary>
        /// This will do a workflow search for the debtcounselling account and load the first case at Manage Proposal.
        /// Once debt counselling case is loaded perform the SendproposalforApproval action on the case
        /// </summary>
        /// <param name="browser">ie browser</param>
        /// <param name="accountkey">debtcounselling account</param>
        /// <param name="userToAssignTo"></param>
        internal static void SendProposalForApprovalAtManageProposal(TestBrowser browser, int accountkey, string userToAssignTo, Int64 instanceID, int debtCounsellingKey, WorkflowRoleTypeEnum workflowRoleType = WorkflowRoleTypeEnum.DebtCounsellingSupervisorD)
        {
            browser.ClickAction(WorkflowActivities.DebtCounselling.SendProposalforApproval);
            browser.Page<DebtCounsellingAssignSupervisor>().AssignToUser(userToAssignTo, ButtonTypeEnum.Submit);

            X2Assertions.AssertCurrentX2State(instanceID, WorkflowStates.DebtCounsellingWF.DecisiononProposal);
            DebtCounsellingAssertions.AssertLatestDebtCounsellingWorkflowRoleAssignment(debtCounsellingKey, userToAssignTo, workflowRoleType, true, true);
            WorkflowRoleAssignmentAssertions.AssertActiveWorkflowRoleExists(debtCounsellingKey, workflowRoleType, userToAssignTo);
        }

        /// <summary>
        /// This will navigate, search and load a debtcounselling case using workflow super search.
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="accountKey"></param>
        /// <param name="workflowStateName"></param>
        internal static void NavigateSearchAndLoadDebtCounsellingCase(TestBrowser browser, int accountKey, params string[] workflowStateName)
        {
            browser.Navigate<Navigation.NavigationHelper>().Task();
            browser.Navigate<Navigation.NavigationHelper>().CloseLoanNodesFLOBO(browser);
            browser.Navigate<Navigation.WorkFlowsNode>().WorkFlows(browser);
            browser.Page<WorkflowSuperSearch>().SearchByUniqueIdentifierAndApplicationType(browser, accountKey.ToString(), workflowStateName, Workflows.DebtCounselling);
        }

        /// <summary>
        /// Performs the Consultant Decline action. Selects a proposal from the grid and adds the reason for declining to the debt counselling case.
        /// </summary>
        /// <param name="browser">IE Browser</param>
        /// <param name="accountKey">AccountKey</param>
        /// <param name="proposalStatus">Proposal Status</param>
        /// <param name="reasonDescription">Consultant Decline Reason</param>
        /// <param name="reasonComment">Comment</param>
        /// <param name="debtCounsellingKey"></param>
        /// <param name="instanceID"></param>
        /// <param name="proposalKey"></param>
        internal static void ConsultantDecline(TestBrowser browser, int accountKey, string proposalStatus, string reasonDescription, int debtCounsellingKey,
            Int64 instanceID, int proposalKey, string reasonComment = "Consultant Decline")
        {
            string date = DateTime.Now.ToString(Formats.DateTimeFormatSQL);
            browser.ClickAction(WorkflowActivities.DebtCounselling.ConsultantDecline);
            browser.Page<ConsultantDecline>().SelectProposalToDecline(DebtCounsellingProposalType.Proposal, proposalStatus);
            browser.Page<CommonReasonCommonDecline>().SelectReasonAddCommentAndSubmit(ReasonType.ConsultantDeclined, reasonDescription, reasonComment,
                ButtonTypeEnum.Submit);
            x2Service.WaitForX2State(debtCounsellingKey, Workflows.DebtCounselling, WorkflowStates.DebtCounsellingWF.ManageProposal);
            //assertions
            StageTransitionAssertions.AssertStageTransitionCreated(debtCounsellingKey, StageDefinitionStageDefinitionGroupEnum.DebtCounselling_ConsultantDeclineReasons);
            ReasonAssertions.AssertReason(reasonDescription, ReasonType.ConsultantDeclined, proposalKey, GenericKeyTypeEnum.Proposal_ProposalKey, true);
            DebtCounsellingAssertions.AssertX2State(debtCounsellingKey, WorkflowStates.DebtCounsellingWF.ManageProposal);
            WorkflowRoleAssignmentAssertions.AssertLatestDebtCounsellingAssignment(instanceID, TestUsers.DebtCounsellingConsultant,
                WorkflowRoleTypeEnum.DebtCounsellingConsultantD, true, true);
        }

        /// <summary>
        /// Navigates to the Attorney node and selects an attorney
        /// </summary>
        /// <param name="browser">TestBrowser</param>
        /// <returns></returns>
        internal static int SelectLitigationAttorney(TestBrowser browser, int debtCounsellingKey)
        {
            int legalentitykey = 0;
            browser.Navigate<ExternalRolesNode>().ManageAttorney();
            legalentitykey = browser.Page<AttorneyUpdate>().SelectLitigationAttorney();
            ExternalRoleAssertions.AssertActiveExternalRoleExistsForLegalEntity(debtCounsellingKey, GenericKeyTypeEnum.debtCounselling_debtCounsellingKey, ExternalRoleTypeEnum.LitigationAttorney, legalentitykey);
            return legalentitykey;
        }

        /// <summary>
        /// Completes the Manage PDA action from the FloBO. This will also insert a new PDA in the database to ensure that the test can change the PDA
        /// currently on the case.
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="debtCounsellingKey"></param>
        internal static int ManagePDA(TestBrowser browser, int debtCounsellingKey)
        {
            //we need the current PDA
            var externalRole = externalRoleService.GetFirstActiveExternalRole(debtCounsellingKey, GenericKeyTypeEnum.debtCounselling_debtCounsellingKey,
                ExternalRoleTypeEnum.PaymentDistributionAgent);
            int legalEntityKey = externalRole == null ? 0 : externalRole.LegalEntityKey;
            //we need a new PDA that is not our current one
            var newPDA = legalEntityOrgStructureService.GetLegalEntityFromLEOS("Payment Distribution Agencies", legalEntityKeyToExclude: legalEntityKey);
            if (!newPDA.HasResults)
            {
                legalEntityOrgStructureService.InsertNewPDA();
                newPDA = legalEntityOrgStructureService.GetLegalEntityFromLEOS("Payment Distribution Agencies", legalEntityKeyToExclude: legalEntityKey);
            }
            string newPDAName = newPDA.Rows(0).Column("Name").GetValueAs<string>();
            int newPDALegalEntityKey = newPDA.Rows(0).Column("legalentitykey").GetValueAs<int>();
            browser.Page<PaymentDistributionAgentUpdate>().ClickSelect();
            //we need to select a new PDA
            browser.Page<LegalEntityOrganisationStructureMaintenanceView>().SelectTier("Payment Distribution Agencies", newPDAName);
            browser.Page<LegalEntityOrganisationStructureMaintenanceView>().Select();
            browser.Page<PaymentDistributionAgentUpdate>().SelectAllAccounts(browser);
            browser.Page<PaymentDistributionAgentUpdate>().ClickUpdate();
            return newPDALegalEntityKey;
        }

        /// <summary>
        /// Completes the Manage Debt Counsellor action from the FloBO. This will also insert a new Debt Counsellor in the database to ensure that the test can change the
        /// Debt Counsellor currently on the case.
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="debtCounsellingKey"></param>
        internal static int ManageDebtCounsellor(TestBrowser browser, int debtCounsellingKey)
        {
            browser.Page<PaymentDistributionAgentUpdate>().ClickSelect();
            //we need the current DC
            var externalRole = externalRoleService.GetFirstActiveExternalRole(debtCounsellingKey, GenericKeyTypeEnum.debtCounselling_debtCounsellingKey,
                ExternalRoleTypeEnum.DebtCounsellor);
            int legalEntityKey = externalRole == null ? 0 : externalRole.LegalEntityKey;
            //we need a new Debt Counsellor that is not our current one
            var newDC = legalEntityOrgStructureService.GetLegalEntityFromLEOS("Debt Counsellors", legalEntityKeyToExclude: legalEntityKey);
            int newDCLegalEntityKey = newDC.Rows(0).Column("legalentitykey").GetValueAs<int>();
            string NCRNumber = newDC.Rows(0).Column("RegNumber").GetValueAs<string>();
            //we need to select a new PDA
            browser.Page<DebtCounsellorSelect>().SearchByNCRNumber(NCRNumber);
            browser.Page<DebtCounsellorSelect>().ClickButton(ButtonTypeEnum.Select);
            browser.Page<PaymentDistributionAgentUpdate>().SelectAllAccounts(browser);
            browser.Page<PaymentDistributionAgentUpdate>().ClickUpdate();
            return newDCLegalEntityKey;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountkey"></param>
        /// <param name="detailType"></param>
        /// <param name="debtcounsellingkey"></param>
        internal static void CaptureDetailType(int accountkey, int debtcounsellingkey, string detailType)
        {
            var browser = new TestBrowser(TestUsers.HaloUser);
            browser.Navigate<Navigation.MenuNode>().CloseLoanNodesCBO();
            browser.Navigate<Navigation.MenuNode>().Menu();
            browser.Navigate<Navigation.MenuNode>().LegalEntityMenu();
            browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(accountkey);
            browser.Navigate<LoanServicingCBO>().LoanAccountNode(accountkey);
            browser.Navigate<LoanServicingCBO>().LoanDetail(NodeTypeEnum.Add);
            var loanDetail = detailTypeService.GetEmptyDetailRecord();
            loanDetail.AccountKey = accountkey;
            loanDetail.LoanDetailType.Description = detailType;
            loanDetail.LoanDetailType.LoanDetailClass.Description = DetailClass.LoanManagement;
            loanDetail.Description = "Testing Description";
            loanDetail.DetailDate = DateTime.Now;

            browser.Page<LoanDetailAdd>().PopulateView(loanDetail, CancellationType.Sale);
            browser.Page<LoanDetailAdd>().ClickAdd();

            browser.Dispose();
            browser = null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountkey"></param>
        /// <param name="detailType"></param>
        /// <param name="debtcounsellingkey"></param>
        internal static void RemoveDetailType(int accountkey, int debtcounsellingkey, string detailType)
        {
            var browser = new TestBrowser(TestUsers.HaloUser);
            browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            browser.Navigate<Navigation.MenuNode>().Menu();
            browser.Navigate<Navigation.MenuNode>().LegalEntityMenu();
            browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(accountkey);

            browser.Navigate<LoanServicingCBO>().LoanAccountNode(accountkey);
            browser.Navigate<LoanServicingCBO>().LoanDetail(NodeTypeEnum.Delete);
            browser.Page<LoanDetailBase>().SelectDetailType(detailType);
            browser.Page<LoanDetailDelete>().ClickDelete();
            browser.Dispose();
            browser = null;
        }

        /// <summary>
        /// Will add a PDA to a case if one does not already exist as an external role
        /// </summary>
        /// <param name="debtCounsellingKey"></param>
        /// <param name="accountKey"></param>
        /// <param name="browser"></param>
        internal static void AddPDAIfOneDoesNotExist(int debtCounsellingKey, int accountKey, TestBrowser browser)
        {
            var externalRole = externalRoleService.GetFirstActiveExternalRole(debtCounsellingKey, GenericKeyTypeEnum.debtCounselling_debtCounsellingKey, ExternalRoleTypeEnum.PaymentDistributionAgent);
            if (externalRole == null)
            {
                browser.Navigate<ExternalRolesNode>().ManagePDA();
                ManagePDA(browser, debtCounsellingKey);
                browser.ClickWorkflowLoanNode(Common.Constants.Workflows.DebtCounselling, accountKey);
            }
        }

        /// <summary>
        /// Selects multiple legal entities from the grid and ensures that the reason exists agains each legal entity.
        /// </summary>
        /// <param name="leKeys">legal entities on the account</param>
        /// <param name="instanceID">instanceID</param>
        /// <param name="debtCounsellingKey">debtCounsellingKey</param>
        /// <param name="assignedDCCUser">assignedDCCUser</param>
        /// <param name="type">Death or Sequestration</param>
        /// <param name="accountKey">AccountKey</param>
        /// <param name="template">Expected Correspondence Template</param>
        /// <param name="reasonDescription">ExpectedReason</param>
        internal static void NotificationOfDeathOrSequestrationMultipleLegalEntities(TestBrowser browser, Dictionary<int, LegalEntityTypeEnum> leKeys, Int64 instanceID,
            int debtCounsellingKey, string assignedDCCUser, NotificationTypeEnum type, string reasonDescription, Common.Enums.CorrespondenceTemplateEnum template, int accountKey)
        {
            DateTime dt = DateTime.Now;
            var keys = (from l in leKeys
                        where l.Value == LegalEntityTypeEnum.NaturalPerson
                        select l.Key).ToList<int>();
            foreach (var leKey in keys)
            {
                browser.Page<LegalEntitySequestrationDeathNotify>().SelectLegalEntityCheckBox(leKey, type);
            }
            browser.Page<LegalEntitySequestrationDeathNotify>().ClickUpdateButton();
            foreach (var leKey in keys)
            {
                ReasonAssertions.AssertReason(reasonDescription, ReasonType.DebtCounsellingNotification, leKey,
                    GenericKeyTypeEnum.LegalEntity_LegalEntityKey);
            }
            //we need the email address of the debt counsellor
            string emailAddress = externalRoleService.GetActiveExternalRoleEmailAddress(debtCounsellingKey, ExternalRoleTypeEnum.DebtCounsellor);
            X2Assertions.AssertCurrentX2State(instanceID, WorkflowStates.DebtCounsellingWF.EstatesandSequestration);
            //check the assignment
            DebtCounsellingAssertions.AssertLatestDebtCounsellingWorkflowRoleAssignment(debtCounsellingKey, assignedDCCUser, WorkflowRoleTypeEnum.DebtCounsellingConsultantD,
                false, true);
            //check that the client email is there as per the template
            CorrespondenceAssertions.AssertClientEmailByCorrespondenceTemplate(debtCounsellingKey, template, dt,
                emailAddress, accountKey);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hasEWorkLossControlCase"></param>
        /// <param name="countOfAccounts"></param>
        /// <param name="hasArrearBalance"></param>
        /// <param name="product"></param>
        /// <param name="isInterestOnly"></param>
        /// <returns></returns>
        internal static List<Automation.DataModels.Account> CreateCaseUsingWorkflowAutomation(bool hasEWorkLossControlCase, int countOfAccounts, bool hasArrearBalance,
                ProductEnum product = ProductEnum.NewVariableLoan, bool isInterestOnly = false, string idNumber = null)
        {
            //we need an ID Number
            if (string.IsNullOrEmpty(idNumber))
                idNumber = debtCounsellingService.GetLegalEntityIDNumberForDCCreate(hasEWorkLossControlCase, countOfAccounts, hasArrearBalance, product, isInterestOnly);
            //then we need to get the accounts for this legal entity
            var accountList = legalEntityService.GetLegalEntityLoanAccountsByIDNumber(idNumber);
            foreach (var acc in accountList)
            {
                var testCase = new Automation.DataModels.TestCase
                {
                    AccountKey = acc.AccountKey,
                    DataTable = "x2.x2data.debt_counselling",
                    ExpectedEndState = WorkflowStates.DebtCounsellingWF.ReviewNotification,
                    KeyType = "DebtCounsellingKey",
                    ScriptFile = "DebtCounselling.xaml",
                    ScriptToRun = "CaseCreate",
                    TestCaseResults = new Dictionary<int, WorkflowReturnData>()
                };
                //we need to do the case creation
                IDataCreationHarness dataCreation = new DataCreationHarness();
                dataCreation.CreateSingleTestCase(Common.Enums.WorkflowEnum.DebtCounselling, testCase);
            }
            return accountList;
        }

        internal static List<Automation.DataModels.DebtCounselling> CreateCaseAndSendToState(string endState, bool eWorkCase = true, int countOfAccounts = 1,
            bool hasArrearTran = true, ProductEnum product = ProductEnum.NewVariableLoan, string idNumber = "", DateTime? date = null, bool isInterestOnly = false)
        {
            IX2ScriptEngine scriptEngine = new X2ScriptEngine();
            var testCases = new List<Automation.DataModels.DebtCounselling>();
            var debtCounsellingKeys = new List<int>();
            var accounts = new List<Automation.DataModels.Account>();
            int debtCounsellingKey = 0;
            //if we have do not pass an ID number we need to use the one created from the create case helper.
            if (string.IsNullOrEmpty(idNumber))
                accounts = WorkflowHelper.CreateCaseUsingWorkflowAutomation(eWorkCase, countOfAccounts, hasArrearTran, product: product, isInterestOnly: isInterestOnly);
            else
                accounts = WorkflowHelper.CreateCaseUsingWorkflowAutomation(eWorkCase, countOfAccounts, hasArrearTran, product: product, isInterestOnly: isInterestOnly, idNumber: idNumber);
            //products requiring opt out
            var optOutProducts = new List<ProductEnum>() {
					ProductEnum.DefendingDiscountRate,
					ProductEnum.Edge,
					ProductEnum.VariFixLoan,
					ProductEnum.SuperLo
			};
            //variable declaration
            foreach (var acc in accounts)
            {
                int accKey = acc.AccountKey;
                debtCounsellingKey = debtCounsellingService.GetDebtCounsellingKey(accKey);
                debtCounsellingKeys.Add(debtCounsellingKey);

                #region ENDSTATE: REVIEW NOTIFICATION

                if (endState == WorkflowStates.DebtCounsellingWF.ReviewNotification)
                    continue;

                #endregion ENDSTATE: REVIEW NOTIFICATION

                #region ENDSTATE: PEND CANCELLATION / RECOVERIES PROPOSAL & RECOVERIES PROPOSAL DECISION

                if (endState == WorkflowStates.DebtCounsellingWF.PendCancellation || endState == WorkflowStates.DebtCounsellingWF.RecoveriesProposal || endState == WorkflowStates.DebtCounsellingWF.RecoveriesProposalDecision)
                {
                    //need to fire the flag to get to pend cancellation
                    scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.EXTUnderCancellation, debtCounsellingKey);
                    if (endState == WorkflowStates.DebtCounsellingWF.RecoveriesProposal || endState == WorkflowStates.DebtCounsellingWF.RecoveriesProposalDecision)
                    {
                        scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.CaptureRecoveriesProposal, debtCounsellingKey);
                        if (endState == WorkflowStates.DebtCounsellingWF.RecoveriesProposalDecision)
                            scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.EscalateRecoveriesProposal, debtCounsellingKey);
                    }
                    continue;
                }

                #endregion ENDSTATE: PEND CANCELLATION / RECOVERIES PROPOSAL & RECOVERIES PROPOSAL DECISION

                #region ENDSTATE: LITIGATION STATE

                if (endState == WorkflowStates.DebtCounsellingWF.Litigation)
                {
                    scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.SendToLitigation, debtCounsellingKey);
                    continue;
                }

                #endregion ENDSTATE: LITIGATION STATE

                scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.RespondToDebtCounsellor, debtCounsellingKey);
                if (endState == WorkflowStates.DebtCounsellingWF.PendProposal)
                    continue;

                #region ENDSTATE: PEND COURT DECISION

                if (endState == WorkflowStates.DebtCounsellingWF.PendCourtDecision)
                {
                    scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.CaptureCourtDetailsToAttorneyToOppose, debtCounsellingKey);
                    continue;
                }

                #endregion ENDSTATE: PEND COURT DECISION

                #region ENDSTATE: BOND EXCLUSIONS AND BOND EXCLUSIONS ARREARS

                if (endState == WorkflowStates.DebtCounsellingWF.BondExclusions || endState == WorkflowStates.DebtCounsellingWF.BondExclusionsArrears)
                {
                    scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.ExcludeBond, debtCounsellingKey);
                    if (endState == WorkflowStates.DebtCounsellingWF.BondExclusionsArrears)
                    {
                        scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.BondExclusionsArrears, debtCounsellingKey);
                    }
                    continue;
                }

                #endregion ENDSTATE: BOND EXCLUSIONS AND BOND EXCLUSIONS ARREARS

                #region ENDSTATE: ESTATES AND SEQUESTRATION

                if (endState == WorkflowStates.DebtCounsellingWF.EstatesandSequestration)
                {
                    scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.NotificationOfSequestration, debtCounsellingKey);
                    continue;
                }

                #endregion ENDSTATE: ESTATES AND SEQUESTRATION

                #region ENDSTATE: INTENT TO TERMINATE

                if (endState == WorkflowStates.DebtCounsellingWF.IntenttoTerminate)
                {
                    scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.RaiseEXT60DateNoDateOrPayment, debtCounsellingKey);
                    continue;
                }

                #endregion ENDSTATE: INTENT TO TERMINATE

                //Perform the Negotiate Proposal action
                scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.NegotiateProposal, debtCounsellingKey);

                #region ENDSTATE: Manage Proposal

                if (endState == WorkflowStates.DebtCounsellingWF.ManageProposal)
                    continue;

                #endregion ENDSTATE: Manage Proposal

                #region ENDSTATE: TERMINATION OR TERMINATION LETTER SENT

                if (endState == WorkflowStates.DebtCounsellingWF.Termination || endState == WorkflowStates.DebtCounsellingWF.TerminationLetterSent)
                {
                    scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.TerminateApplication, debtCounsellingKey);
                    if (endState == WorkflowStates.DebtCounsellingWF.TerminationLetterSent)
                    {
                        scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.SendTerminationLetter, debtCounsellingKey);
                    }
                    continue;
                }

                #endregion ENDSTATE: TERMINATION OR TERMINATION LETTER SENT

                scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.SendProposalForApproval, debtCounsellingKey);

                #region ENDSTATE: DECISION ON PROPOSAL

                if (endState == WorkflowStates.DebtCounsellingWF.DecisiononProposal)
                    continue;

                #endregion ENDSTATE: DECISION ON PROPOSAL

                scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.AcceptProposal, debtCounsellingKey);

                #region ENDSTATE: SEND LOAN AGREEMENTS

                if (endState == WorkflowStates.DebtCounsellingWF.SendLoanAgreements)
                    continue;

                #endregion ENDSTATE: SEND LOAN AGREEMENTS

                #region PRODUCT CHECK TO PROCESS CASE AT SEND LOAN AGREEMENTS

                if (optOutProducts.Contains(product) || isInterestOnly)
                    scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.SignedDocsReceived, debtCounsellingKey);

                #endregion PRODUCT CHECK TO PROCESS CASE AT SEND LOAN AGREEMENTS

                #region ENDSTATE: ACCEPTED PROPOSAL

                if (endState == WorkflowStates.DebtCounsellingWF.AcceptedProposal)
                    continue;

                #endregion ENDSTATE: ACCEPTED PROPOSAL

                scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.NotificationOfDecision, debtCounsellingKey);

                #region ENDSTATE: PEND PAYMENT

                if (endState == WorkflowStates.DebtCounsellingWF.PendPayment)
                    continue;

                #endregion ENDSTATE: PEND PAYMENT

                scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.PaymentReceived, debtCounsellingKey);

                #region ENDSTATE: PAYMENT REVIEW

                if (endState == WorkflowStates.DebtCounsellingWF.PaymentReview)
                    continue;

                #endregion ENDSTATE: PAYMENT REVIEW

                scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.PaymentInOrder, debtCounsellingKey);

                #region ENDSTATE: DEBT REVIEW APPROVED

                if (endState == WorkflowStates.DebtCounsellingWF.DebtReviewApproved)
                    continue;

                #endregion ENDSTATE: DEBT REVIEW APPROVED

                #region ENDSTATE: DEFAULT IN PAYMENT

                if (endState == WorkflowStates.DebtCounsellingWF.DefaultinPayment)
                {
                    scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.EXTIntoArrears, debtCounsellingKey);
                }

                #endregion ENDSTATE: DEFAULT IN PAYMENT

                #region ENDSTATE: TERM REVIEW

                if (endState == WorkflowStates.DebtCounsellingWF.TermReview)
                {
                    scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.FireTermExpiredTimer, debtCounsellingKey);
                    continue;
                }

                #endregion ENDSTATE: TERM REVIEW
            }
            //add to the test cases
            foreach (var key in debtCounsellingKeys)
            {
                var testCase = new Automation.DataModels.DebtCounselling();
                testCase.InstanceID = x2Service.GetInstanceIDByDebtCounsellingKey(key);
                testCase.AssignedUser = assignmentService.GetADUserNameOfActiveWorkflowRoleAssignment(testCase.InstanceID);
                testCase.AccountKey = debtCounsellingService.GetAccountKeyByDebtCounsellingKey(key, DebtCounsellingStatusEnum.Open);
                testCase.DebtCounsellingKey = key;
                testCase.Account = accountService.GetAccountByKey(testCase.AccountKey);
                var results = x2Service.GetInstanceCloneByState(testCase.AccountKey.ToString(), WorkflowStates.DebtCounsellingWF._60DayTimerHold, Workflows.DebtCounselling);
                if (results.HasResults)
                    testCase.ClonedInstanceID = results.Rows(0).Column("ClonedInstanceID").GetValueAs<Int64>();
                testCases.Add(testCase);
                x2Service.WaitForX2State(testCase.DebtCounsellingKey, Workflows.DebtCounselling, endState);
            }
            return testCases;
        }

        public static void CreateCase(int accountKey)
        {
            var testCase = new Automation.DataModels.TestCase
            {
                AccountKey = accountKey,
                DataTable = "x2.x2data.debt_counselling",
                ExpectedEndState = WorkflowStates.DebtCounsellingWF.ReviewNotification,
                KeyType = "DebtCounsellingKey",
                ScriptFile = "DebtCounselling.xaml",
                ScriptToRun = "CaseCreate",
                TestCaseResults = new Dictionary<int, WorkflowReturnData>()
            };
            //we need to do the case creation
            IDataCreationHarness dataCreation = new DataCreationHarness();
            dataCreation.CreateSingleTestCase(Common.Enums.WorkflowEnum.DebtCounselling, testCase);
        }

        public static Automation.DataModels.DebtCounselling SearchForCase(string workflowState, string aduserName, int productKey = 0, bool isInterestOnly = false)
        {
            int interestOnly = isInterestOnly ? 1 : 0;
            var debtCounsellingCases = debtCounsellingService.GetDebtCounsellingCaseByStateAndWorkflowRoleType(workflowState, aduserName);
            if (debtCounsellingCases.Count() > 0 && productKey == 0)
            {
                var dcCase = debtCounsellingCases.Where(x => x.InterestOnly == interestOnly).SelectRandom();
                dcCase.AssignedUser = dcCase.ADUserName;
                return dcCase;
            }
            else if (debtCounsellingCases.Count() > 0 && productKey != 0)
            {
                var row = (from dc in debtCounsellingCases where dc.ProductKey == productKey && dc.InterestOnly == interestOnly select dc).FirstOrDefault();
                if (row != null)
                {
                    row.AssignedUser = row.ADUserName;
                    return row;
                }
            }
            return new Automation.DataModels.DebtCounselling { DebtCounsellingKey = 0 };
        }
    }
}