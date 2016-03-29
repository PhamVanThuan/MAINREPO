using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation;
using BuildingBlocks.Navigation.FLOBO.Common;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.LegalEntity;
using BuildingBlocks.Presenters.Origination;
using BuildingBlocks.Presenters.Origination.Valuations;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using WatiN.Core.Logging;
using WorkflowAutomation.Harness;
using Navigation = BuildingBlocks.Navigation;

namespace Origination
{
    /// <summary>
    /// Helper Class for Application Management tests containing common methods used among the tests in the test suite
    /// </summary>
    public class Helper
    {
        private static IApplicationService _Application;
        private static IEWorkService eworkService;
        private static IX2WorkflowService x2Service;
        private static IBankingDetailsService bankingDetailsService;
        private static IPropertyService propertyService;
        private static IX2WorkflowService workflowService;
        private static IValuationService valuationService;

        static Helper()
        {
            _Application = ServiceLocator.Instance.GetService<IApplicationService>();
            eworkService = ServiceLocator.Instance.GetService<IEWorkService>();
            x2Service = ServiceLocator.Instance.GetService<IX2WorkflowService>();
            bankingDetailsService = ServiceLocator.Instance.GetService<IBankingDetailsService>();
            propertyService = ServiceLocator.Instance.GetService<IPropertyService>();
            workflowService = ServiceLocator.Instance.GetService<IX2WorkflowService>();
            valuationService = ServiceLocator.Instance.GetService<IValuationService>();
        }

        /// <summary>
        /// Common assertions for the tests, testing Decline with Offer and approve with Pricing Changes actions.  Because we cannot predict
        /// if the application will be assigned to a credit underwriter or credit supervisor user and that the application is processed differently for these users,
        /// different assertions will be run according to the assigned user.  If the assigned user was a Credit Underwriter, the offer is expected to move to the Review Decision state.
        /// If the assigned user is a Credit Supervisor, Credit Manager or Credit Exceptions Consultant, the offer is expecte to move directly to the LOA state.
        ///
        /// This assertion only applies to happy days examples
        /// </summary>
        /// <param name="username"></param>
        /// <param name="offerKey"></param>
        public static void AssertionDeclineWithOfferApproveWithPricingChanges(string username, int offerKey)
        {
            WatiN.Core.Logging.Logger.LogAction(string.Format("Original Assigned User: {0}", username));
            if (username.Contains(@"cuuser"))
            {
                System.Threading.Thread.Sleep(5000);
                X2Assertions.AssertCurrentCreditX2State(offerKey, WorkflowStates.CreditWF.ReviewDecision);

                AssignmentAssertions.AssertThatAWorkFlowAssignmentRecordExists(offerKey, OfferRoleTypeEnum.CreditSupervisorD);
                AssignmentAssertions.AssertThatTheWorkFlowAssignmentRecordIsActive(offerKey, OfferRoleTypeEnum.CreditSupervisorD);
                AssignmentAssertions.AssertOfferRoleRecordExists(offerKey, OfferRoleTypeEnum.CreditSupervisorD);
                AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(offerKey, OfferRoleTypeEnum.CreditSupervisorD);

                AssignmentAssertions.AssertThatTheWorkFlowAssignmentRecordIsInactive(offerKey, OfferRoleTypeEnum.CreditUnderwriterD);
                AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(offerKey, OfferRoleTypeEnum.CreditUnderwriterD);
            }
            else
            {
                System.Threading.Thread.Sleep(5000);
                X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.LOA);

                AssignmentAssertions.AssertThatAWorkFlowAssignmentRecordExists(offerKey, OfferRoleTypeEnum.BranchConsultantD);
                AssignmentAssertions.AssertThatTheWorkFlowAssignmentRecordIsActive(offerKey, OfferRoleTypeEnum.BranchConsultantD);
                AssignmentAssertions.AssertOfferRoleRecordExists(offerKey, OfferRoleTypeEnum.BranchConsultantD);
                AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(offerKey, OfferRoleTypeEnum.BranchConsultantD);
            }
        }

        /// <summary>
        /// Completes the LOA Accepted Action for a case.
        /// </summary>
        /// <param name="_instructOfferKey"></param>
        /// <param name="browser"></param>
        internal static void CompleteSignedLOAReview(int _instructOfferKey, TestBrowser browser)
        {
            browser = new TestBrowser(TestUsers.RegistrationsLOAAdmin, TestUsers.Password);
            browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().WorkFlows(browser);
            browser.Page<WorkflowSuperSearch>().Search(browser, _instructOfferKey, WorkflowStates.ApplicationManagementWF.SignedLOAReview);
            browser.ClickAction(WorkflowActivities.ApplicationManagement.LOAAccepted);
            browser.Page<WorkflowYesNo>().Confirm(true, false);
            browser.Dispose();
        }

        /// <summary>
        /// Processes an application from the Manage Application State
        /// </summary>
        /// <param name="offerKey"></param>
        internal static void ProcessFromManageApplication(int offerKey, TestBrowser browser)
        {
            browser = new TestBrowser(TestUsers.NewBusinessProcessor, TestUsers.Password);
            browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(browser);
            browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().WorkFlows(browser);
            browser.Page<WorkflowSuperSearch>().Search(offerKey);
            browser.Navigate<Navigation.PropertyAddressNode>().PropertyAddress(offerKey);
            browser.Navigate<Navigation.PropertyAddressNode>().UpdateDeedsOfficeDetails(NodeTypeEnum.Update);
            //Check that the warning message "This property is connected to an open loan and cannot be updated,
            //please use the capture property functionality or alternately contact client services to update." does not exist before trying to update Deeds Office details
            if (!browser.Page<BasePage>().CheckForerrorMessages(
                "This property is connected to an open loan and cannot be updated, please use the capture property functionality or alternately contact client services to update."))
            {
                browser.Page<PropertyDetailsUpdateDeedsNoExclusions>().
                    UpdateDeedsOfficeDetails("Test", "Test", "Test", "Test", "Test", "Test", "Test", ButtonTypeEnum.Update);
            }
            //Update HOC
            browser.Navigate<Navigation.PropertyAddressNode>().HomeOwnersCover(NodeTypeEnum.Update);
            browser.Page<HOCFSSummaryAddApplication>().UpdateHOCDetails(2, ButtonTypeEnum.Add);
            if (browser.Page<BasePage>().CheckForerrorMessages(
                "The property is listed as 'Sectional Title', HOC only allowed on title type of 'Sectional Title with HOC'."))
            {
                browser.Page<HOCFSSummaryAddApplication>().UpdateHOCDetails(20, 100001, ButtonTypeEnum.Add);
            }
            //Update Document Checklist
            browser.Navigate<Navigation.DocumentCheckListNode>().DocumentChecklist();
            browser.Navigate<Navigation.DocumentCheckListNode>().ViewDocumentChecklist(NodeTypeEnum.Update);
            browser.Page<DocumentCheckListUpdate>().UpdateDocumentChecklist();
            //Complete the 'Application in Order' action
            browser.ClickWorkflowLoanNode(Workflows.ApplicationManagement);
            browser.ClickAction(WorkflowActivities.ApplicationManagement.ApplicationinOrder);
            browser.Page<WorkflowYesNo>().Confirm(true, true);
            browser.Dispose();
            browser = null;
        }

        /// <summary>
        /// Captures a manual valuation given the market value
        /// </summary>
        /// <param name="offerKey"></param>
        /// <param name="browser"></param>
        internal static void CaptureManualValuation(int offerKey, TestBrowser browser, string marketValue)
        {
            int extent = Convert.ToInt32(marketValue) / 3000;
            Logger.LogAction("Processing Offer at {0} state of the {1} workflow", WorkflowStates.ValuationsWF.ScheduleValuationAssessment, Workflows.Valuations);
            browser = new TestBrowser(TestUsers.ValuationsProcessor2, TestUsers.Password);
            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.ValuationsWF.ScheduleValuationAssessment);
            //Perform Manual Valuation
            browser.ClickAction(WorkflowActivities.Valuations.PerformManualValuation);
            BasicPerformManualValuation(browser, "Normal standard", "Conventional", extent, 3000, "Carl nel");
            browser.Dispose();
            browser = null;
        }

        /// <summary>
        /// Processes an application from the Valuation Approval Required state
        /// </summary>
        /// <param name="offerKey"></param>
        /// <param name="browser"></param>
        internal static void ProcessFromValuationApprovalRequired(int offerKey, TestBrowser browser)
        {
            browser = new TestBrowser(TestUsers.CreditUnderwriter, TestUsers.Password);
            browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(browser);
            browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().WorkFlows(browser);
            browser.Page<WorkflowSuperSearch>().Search(offerKey);
            //Complete the Valuation Approved action
            browser.ClickAction(WorkflowActivities.Credit.ValuationApproved);
            browser.Page<WorkflowYesNo>().Confirm(true, false);
            browser.Dispose();
            browser = null;
        }

        /// <summary>
        /// Finds an application that can be NTU'd in Pipeline.
        /// </summary>
        /// <param name="browser">IE TestBrowser</param>
        /// <param name="offerKey">OUT OfferKey</param>
        /// <param name="accountKey">OUT AccountKey</param>
        /// <param name="reinstateStage">OUT Stage to which the case will be reinstated to in eWork</param>
        internal static void NTUApplicationFromPipeline(TestBrowser browser, out int offerKey, out int accountKey, out string reinstateStage)
        {
            var results = eworkService.GetPipelineCaseWhereActionIsApplied(EworkActions.X2NTUAdvise, WorkflowStates.ApplicationManagementWF.RegistrationPipeline, 0);
            offerKey = results.Rows(0).Column("applicationKey").GetValueAs<int>();
            accountKey = results.Rows(0).Column("accountKey").GetValueAs<int>();
            reinstateStage = results.Rows(0).Column("eStageName").Value;
            //start the browser
            browser = new TestBrowser(TestUsers.RegistrationsManager, TestUsers.Password);
            browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().WorkFlows(browser);
            browser.Page<WorkflowSuperSearch>().Search(offerKey);
            //navigate to the action
            browser.ClickAction(WorkflowActivities.ApplicationManagement.NTUPipeLine);
            //select the reason and return the selected reason
            string selectedReason = browser.Page<CommonReasonCommonDecline>().SelectReasonAndSubmit(ReasonType.PipelineNTU);
            //case is now at the NTU state in X2
            var instanceID = x2Service.GetAppManInstanceIDByOfferKey(offerKey);
            x2Service.WaitForX2WorkflowHistoryActivity(WorkflowActivities.ApplicationManagement.NTUPipeLine, instanceID, 1);
            X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.NTU);
            //we need to check that the ework case has been created
            eWorkAssertions.AssertEworkCaseExists(accountKey.ToString(), EworkStages.NTUReview, EworkMaps.Pipeline);
            //check the reason has been added
            ReasonAssertions.AssertReason(selectedReason, ReasonType.PipelineNTU, offerKey, GenericKeyTypeEnum.Offer_OfferKey);
            //check the offer status
            OfferAssertions.AssertOfferStatus(offerKey, OfferStatusEnum.NTU);
            //check the offer end date
            OfferAssertions.AssertOfferEndDate(offerKey, DateTime.Now, 0, false);
            browser.Dispose();
        }

        /// <summary>
        /// Performs an NTU in the Pipeline eWork map that should raise a corresponding flag in the Application Management map to move the X2 case.
        /// </summary>
        /// <param name="offerKey">offerKey</param>
        /// <param name="accountKey">accountKey</param>
        /// <param name="reinstateStage">stage to which the case should be reinstated</param>
        internal static void eWorkNTUApplicationFromPipeline(out int offerKey, out int accountKey, out string reinstateStage)
        {
            var results = eworkService.GetPipelineCaseWhereActionIsApplied(EworkActions.X2NTUAdvise, WorkflowStates.ApplicationManagementWF.RegistrationPipeline, 0);
            offerKey = results.Rows(0).Column("applicationKey").GetValueAs<int>();
            accountKey = results.Rows(0).Column("accountKey").GetValueAs<int>();
            reinstateStage = results.Rows(0).Column("eStageName").Value;
            //raise the flag
            x2Service.PipeLineNTU(offerKey);
            //update the e-work database
            eworkService.UpdateEworkStage(EworkStages.NTUReview, EworkStages.LodgeDocuments, EworkMaps.Pipeline, accountKey.ToString());

            var instanceID = x2Service.GetAppManInstanceIDByOfferKey(offerKey);
            x2Service.WaitForX2WorkflowHistoryActivity(WorkflowActivities.ApplicationManagement.EXTNTU, instanceID, 1);
            //check that the X2 case has moved
            X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.NTU);
            //check the offer status
            OfferAssertions.AssertOfferStatus(offerKey, OfferStatusEnum.NTU);
            //check the offer end date
            OfferAssertions.AssertOfferEndDate(offerKey, DateTime.Now, 0, false);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="browser"></param>
        /// <returns></returns>
        internal static void QAQueryAtQA(TestBrowser browser, int offerKey)
        {
            browser = new TestBrowser(TestUsers.CreditUnderwriter, TestUsers.Password);
            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.ApplicationManagementWF.QA);
            browser.ClickAction(WorkflowActivities.ApplicationManagement.QAQuery);
            browser.Page<CommonReasonCommonDecline>().SelectReasonAndSubmit(ReasonType.QAQuery);
            //we need find the active Branch Consultant
            X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.RequestatQA);
            browser.Dispose();
        }

        internal static void AddConfirmedIncome(TestBrowser browser, string confirmedIncome)
        {
            browser.Navigate<LegalEntityNode>().EmploymentDetails(NodeTypeEnum.Update);
            string remunerationType = browser.Page<LegalEntityEmploymentUpdate>().GetEmploymentType();
            switch (remunerationType)
            {
                case EmploymentType.Salaried:
                case EmploymentType.SalariedWithDeductions:
                    browser.Page<LegalEntityEmploymentDetails>().BasicConfirmSalariedEmployment(browser, Convert.ToInt32(confirmedIncome), 20);
                    if (remunerationType == EmploymentType.SalariedWithDeductions)
                        browser.Page<LegalEntityDetailsSubsidyAddLegalEntityEmploymentDetails>().AddSubsidyDetailsForAccount(-1, null, null, null,
                        null, null, -1, Common.Enums.ButtonTypeEnum.Save);
                    break;

                case EmploymentType.SelfEmployed:
                    browser.Page<LegalEntityEmploymentDetails>().BasicConfirmSelfEmployedEmployment(browser, Convert.ToInt32(confirmedIncome), 20);
                    break;

                default:
                    Logger.LogAction("Employment details not updated as RemunerationType: {0} not yet catered for.  Please update Test or Test data accordingly", remunerationType);
                    break;
            }
        }

        internal static void AddBankingDetails(TestBrowser browser)
        {
            var bankAccount = bankingDetailsService.GetNextUnusedBankAccountDetails();
            bankAccount.AccountName = "Test";
            browser.Navigate<LegalEntityNode>().BankingDetails(NodeTypeEnum.Add);
            browser.Page<BankingDetails>().AddBankingDetails(bankAccount, ButtonTypeEnum.Add);
        }

        internal static void AddSettlementBank(TestBrowser browser)
        {
            var bankAccount = bankingDetailsService.GetNextUnusedBankAccountDetails();
            bankAccount.AccountName = "Test";
            browser.Navigate<LoanDetailsNode>().ClickAddSettlementBankingDetailsNode();
            browser.Page<BankingDetails>().AddSettlementBankingDetails(bankAccount, "Test", ButtonTypeEnum.Add);
        }

        internal static void UpdateDebitOrderDetails(TestBrowser browser)
        {
            browser.Navigate<LoanDetailsNode>().ClickUpdateDebitOrderDetailsNode();
            browser.Page<DebitOrderDetailsAppUpdate>().UpdateDebitOrderDetails("Debit Order Payment", 1, 1, ButtonTypeEnum.Update);
        }

        /// <summary>
        /// Sets the attorney loan attribute
        /// </summary>
        /// <param name="browser"></param>
        internal static void UpdateApplicationAttributes(TestBrowser browser)
        {
            browser.Navigate<LoanDetailsNode>().ClickUpdateApplicationLoanAttributesNode();
            browser.Page<ApplicationAttributesUpdate>().UpdateApplicationLoanAttributes("Test", 1, ButtonTypeEnum.Update);
        }

        /// <summary>
        /// Saves the required condition set against the case
        /// </summary>
        /// <param name="browser"></param>
        internal static void SaveConditions(TestBrowser browser,bool expectedNoValidationMessages = false)
        {
            browser.Navigate<LoanDetailsNode>().ClickManageLoanConditionsNode();
            browser.Page<ConditionAddSet>().SaveConditionSet();
            if (expectedNoValidationMessages)
                browser.Page<BasePageAssertions>().AssertNoValidationMessageExists();
        }

        /// <summary>
        /// Completes the document checklist
        /// </summary>
        /// <param name="browser"></param>
        internal static void CompleteDocumentChecklist(TestBrowser browser)
        {
            browser.Navigate<Navigation.DocumentCheckListNode>().DocumentChecklist();
            browser.Navigate<Navigation.DocumentCheckListNode>().ViewDocumentChecklist(NodeTypeEnum.Update);
            browser.Page<DocumentCheckListUpdate>().UpdateDocumentChecklist();
        }

        /// <summary>
        /// Updates the Property Address
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="offerKey"></param>
        internal static void UpdatePropertyAddress(TestBrowser browser, int offerKey)
        {
            browser.Navigate<Navigation.PropertyAddressNode>().PropertyAddress(offerKey);
            browser.Navigate<Navigation.PropertyAddressNode>().UpdateDeedsOfficeDetails(NodeTypeEnum.Update);
            browser.Page<PropertyDetailsUpdateDeedsNoExclusions>().UpdateDeedsOfficeDetails(
                "Test", "Test", "Test", "Test", "Test", "Test", "Test", ButtonTypeEnum.Update);
        }

        internal static void UpdateHOCDetails(TestBrowser browser)
        {
            browser.Navigate<Navigation.PropertyAddressNode>().HomeOwnersCover(NodeTypeEnum.Update);
            browser.Page<HOCFSSummaryAddApplication>().UpdateHOCDetails(2, ButtonTypeEnum.Add);
            if (browser.Page<BasePage>().CheckForerrorMessages(
                "The property is listed as 'Sectional Title', HOC only allowed on title type of 'Sectional Title with HOC'."))
            {
                browser.Page<HOCFSSummaryAddApplication>().UpdateHOCDetails(20, 100001, ButtonTypeEnum.Add);
            }

            if (browser.Page<BasePageAssertions>().ValidationSummaryExists())
            {
                browser.Page<BasePage>().DomainWarningClickYes();
            }
        }

        /// <summary>
        /// Carries out the steps required in order to complete the QA Complete activity at the QA state
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="offerKey"></param>
        internal static void QAComplete(TestBrowser browser, int offerKey)
        {
            browser = new TestBrowser(TestUsers.CreditUnderwriter, TestUsers.Password);
            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.ApplicationManagementWF.QA);
            browser.ClickAction(WorkflowActivities.ApplicationManagement.QAComplete);
            browser.Page<CommonReasonCommonDecline>().SelectReasonAndSubmit(ReasonType.QAComplete);
            browser.Dispose();
            browser = null;
        }

        /// <summary>
        /// Carrys out the steps required in order to complete the Client Accepts action at the Issue AIP state
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="username"></param>
        /// <param name="offerKey"></param>
        internal static void AcceptAIP(TestBrowser browser, string username, int offerKey)
        {
            browser = new TestBrowser(username);
            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.ApplicationManagementWF.IssueAIP);
            browser.ClickAction(WorkflowActivities.ApplicationManagement.ClientAccepts);
            browser.Page<WorkflowYesNo>().Confirm(true, false);
            browser.Dispose();
            browser = null;
        }

        internal static void PerformManualValuation(TestBrowser browser, int offerKey, string marketValue)
        {
            int rate = 3000;
            int extent = Convert.ToInt32(marketValue) / rate;
            var dateFilter = DateTime.Now.Subtract(TimeSpan.FromMinutes(1));
            Logger.LogAction("Processing Offer at {0} state of the {1} workflow", WorkflowStates.ValuationsWF.ScheduleValuationAssessment, Workflows.Valuations);
            browser = new TestBrowser(TestUsers.ValuationsProcessor, TestUsers.Password);
            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.ValuationsWF.ScheduleValuationAssessment);
            var instanceID = x2Service.GetValuationsInstanceDetails(offerKey).Rows(0).Column("ID").GetValueAs<int>();
            browser.ClickAction(WorkflowActivities.Valuations.PerformManualValuation);
            BasicPerformManualValuation(browser, "Normal standard", "Conventional", extent, rate, "Carl nel");
            //workflowService.WaitForX2State(offerKey, Workflows.Valuations, WorkflowStates.ValuationsWF.ManualArchive);
            workflowService.WaitForX2WorkflowHistoryActivity(instanceID, 1, dateFilter.ToString(Common.Constants.Formats.DateTimeFormatSQL), WorkflowActivities.Valuations.PerformManualValuation);
            browser.Dispose();
            browser = null;
        }

        internal static void AddAssetsAndLiabilities(TestBrowser browser)
        {
            browser.Navigate<LegalEntityNode>().AssetsAndLiabilities(NodeTypeEnum.Add);
            Automation.DataModels.LegalEntityAssetLiabilities legalentityAssetsAndLiabilities = new Automation.DataModels.LegalEntityAssetLiabilities();
            legalentityAssetsAndLiabilities.AssetLiabilityTypeKey = Common.Enums.AssetLiabilityTypeEnum.ListedInvestments;
            legalentityAssetsAndLiabilities.CompanyName = "Test";
            legalentityAssetsAndLiabilities.AssetValue = 1000f;
            browser.Page<BuildingBlocks.Presenters.LegalEntity.LegalEntityAssetLiability>().AddAssetsAndLiabilities(legalentityAssetsAndLiabilities);
        }

        internal static void ClientRefusesAIP(TestBrowser browser, int offerKey)
        {
            browser = new TestBrowser(TestUsers.BranchConsultant, TestUsers.Password);
            browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().WorkFlows(browser);
            browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(browser);
            browser.Page<WorkflowSuperSearch>().Search(offerKey);
            browser.ClickAction(WorkflowActivities.ApplicationManagement.ClientRefuse);
            //confirm
            browser.Page<WorkflowYesNo>().Confirm(true, false);
            //assert the case has been archived correctly
            X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.ArchiveAIP);
            OfferAssertions.AssertOfferEndDate(offerKey, DateTime.Now, 0, false);
            OfferAssertions.AssertOfferStatus(offerKey, OfferStatusEnum.NTU);
            browser.Dispose();
            browser = null;
        }

        internal static void RequestLightstoneValuation(TestBrowser browser, int offerKey)
        {
            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.ApplicationManagementWF.ManageApplication);
            browser.ClickAction(WorkflowActivities.ApplicationManagement.RequestLightstoneValuation);
            browser.Page<WorkflowYesNo>().Confirm(true, false);
        }

        internal static void SendLOA(TestBrowser browser, int offerKey)
        {
            browser = new TestBrowser(TestUsers.BranchConsultant, TestUsers.Password);
            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.ApplicationManagementWF.LOA);
            //perform the Send LOA action
            browser.ClickAction(WorkflowActivities.ApplicationManagement.SendLOA);
            browser.Page<CorrespondenceProcessing>().SendCorrespondence(CorrespondenceSendMethodEnum.Email);
            CorrespondenceAssertions.AssertCorrespondenceRecordAdded(offerKey, CorrespondenceReports.LetterOfAcceptance, CorrespondenceMedium.Email);
        }

        internal static void LOAReceived(TestBrowser browser, string userName, int offerKey)
        {
            browser = new TestBrowser(userName, TestUsers.Password);
            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.ApplicationManagementWF.LOA);
            //perform the action
            browser.ClickAction(WorkflowActivities.ApplicationManagement.LOAReceived);
            browser.Page<WorkflowYesNo>().Confirm(true, false);
            //case has moved
            X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.SignedLOAReview);
            //check the worklist assignment
            X2Assertions.AssertWorklistOwner(offerKey, WorkflowStates.ApplicationManagementWF.SignedLOAReview, Workflows.ApplicationManagement,
            OfferRoleTypes.RegistrationsAdmin);
            X2Assertions.AssertWorklistOwner(offerKey, WorkflowStates.ApplicationManagementWF.SignedLOAReview, Workflows.ApplicationManagement,
            OfferRoleTypes.RegistrationsLOAAdmin);
            X2Assertions.AssertWorklistOwner(offerKey, WorkflowStates.ApplicationManagementWF.SignedLOAReview, Workflows.ApplicationManagement,
            OfferRoleTypes.RegistrationsManager);
            X2Assertions.AssertWorklistOwner(offerKey, WorkflowStates.ApplicationManagementWF.SignedLOAReview, Workflows.ApplicationManagement,
            OfferRoleTypes.RegistrationsSupervisor);
        }

        internal static void QueryOnLOA(TestBrowser browser, int offerKey, string expectedBranchConsultant)
        {
            browser = new TestBrowser(TestUsers.RegAdminUser, TestUsers.Password);
            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.ApplicationManagementWF.SignedLOAReview);
            browser.ClickAction(WorkflowActivities.ApplicationManagement.QueryonLOA);
            browser.Page<GenericMemoAdd>().AddMemoRecord(MemoStatus.UnResolved, "LOA Query");
            //case should now be LOA Query
            X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.LOAQuery);
            //assigned to Branch Consultant
            AssignmentAssertions.AssertWorkflowAssignment(expectedBranchConsultant, offerKey, OfferRoleTypeEnum.BranchConsultantD);
        }

        internal static void LOAQueryComplete(TestBrowser browser, string branchConsultant, int offerKey)
        {
            browser = new TestBrowser(branchConsultant, TestUsers.Password);
            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.ApplicationManagementWF.LOAQuery);
            browser.ClickAction(WorkflowActivities.ApplicationManagement.QueryComplete);
            browser.Page<WorkflowYesNo>().Confirm(true, false);
            //case has moved
            X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.SignedLOAReview);
            //check the worklist assignment
            X2Assertions.AssertWorklistOwner(offerKey, WorkflowStates.ApplicationManagementWF.SignedLOAReview, Workflows.ApplicationManagement,
                OfferRoleTypes.RegistrationsAdmin);
            X2Assertions.AssertWorklistOwner(offerKey, WorkflowStates.ApplicationManagementWF.SignedLOAReview, Workflows.ApplicationManagement,
                OfferRoleTypes.RegistrationsLOAAdmin);
            X2Assertions.AssertWorklistOwner(offerKey, WorkflowStates.ApplicationManagementWF.SignedLOAReview, Workflows.ApplicationManagement,
                OfferRoleTypes.RegistrationsManager);
            X2Assertions.AssertWorklistOwner(offerKey, WorkflowStates.ApplicationManagementWF.SignedLOAReview, Workflows.ApplicationManagement,
                OfferRoleTypes.RegistrationsSupervisor);
        }

        internal static void ResendLOA(TestBrowser browser, string expectedBranchConsultant, int offerKey)
        {
            browser = new TestBrowser(TestUsers.RegAdminUser, TestUsers.Password);
            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.ApplicationManagementWF.SignedLOAReview);
            //performed the resend LOA action
            browser.ClickAction(WorkflowActivities.ApplicationManagement.ResendLOA);
            browser.Page<WorkflowYesNo>().Confirm(true, false);
            X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.LOA);
            //assigned to Branch Consultant
            AssignmentAssertions.AssertWorkflowAssignment(expectedBranchConsultant, offerKey, OfferRoleTypeEnum.BranchConsultantD);
        }

        internal static void LOAReceived(TestBrowser browser, int offerKey)
        {
            browser = new TestBrowser(TestUsers.BranchConsultant, TestUsers.Password);
            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.ApplicationManagementWF.LOA);
            //perform the action
            browser.ClickAction(WorkflowActivities.ApplicationManagement.LOAReceived);
            browser.Page<WorkflowYesNo>().Confirm(true, false);
            //case has moved
            X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.SignedLOAReview);
            //check the worklist assignment
            X2Assertions.AssertWorklistOwner(offerKey, WorkflowStates.ApplicationManagementWF.SignedLOAReview, Workflows.ApplicationManagement,
            OfferRoleTypes.RegistrationsAdmin);
            X2Assertions.AssertWorklistOwner(offerKey, WorkflowStates.ApplicationManagementWF.SignedLOAReview, Workflows.ApplicationManagement,
            OfferRoleTypes.RegistrationsLOAAdmin);
            X2Assertions.AssertWorklistOwner(offerKey, WorkflowStates.ApplicationManagementWF.SignedLOAReview, Workflows.ApplicationManagement,
            OfferRoleTypes.RegistrationsManager);
            X2Assertions.AssertWorklistOwner(offerKey, WorkflowStates.ApplicationManagementWF.SignedLOAReview, Workflows.ApplicationManagement,
            OfferRoleTypes.RegistrationsSupervisor);
        }

        internal static void LOAAccepted(TestBrowser browser, int offerKey)
        {
            browser = new TestBrowser(TestUsers.RegAdminUser, TestUsers.Password);
            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.ApplicationManagementWF.SignedLOAReview);
            //perform the action
            browser.ClickAction(WorkflowActivities.ApplicationManagement.LOAAccepted);
            browser.Page<WorkflowYesNo>().Confirm(true, false);
            //case should have moved states
            X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.ApplicationCheck);
            //check the worklist assignment
            X2Assertions.AssertWorklistOwner(offerKey, WorkflowStates.ApplicationManagementWF.ApplicationCheck, Workflows.ApplicationManagement,
            OfferRoleTypes.RegistrationsAdmin);
            X2Assertions.AssertWorklistOwner(offerKey, WorkflowStates.ApplicationManagementWF.ApplicationCheck, Workflows.ApplicationManagement,
            OfferRoleTypes.RegistrationsLOAAdmin);
            X2Assertions.AssertWorklistOwner(offerKey, WorkflowStates.ApplicationManagementWF.ApplicationCheck, Workflows.ApplicationManagement,
            OfferRoleTypes.RegistrationsManager);
            X2Assertions.AssertWorklistOwner(offerKey, WorkflowStates.ApplicationManagementWF.ApplicationCheck, Workflows.ApplicationManagement,
            OfferRoleTypes.RegistrationsSupervisor);
        }

        internal static void InstructAttorney(TestBrowser browser, int offerKey)
        {
            browser = new TestBrowser(TestUsers.RegAdminUser, TestUsers.Password);
            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.ApplicationManagementWF.ApplicationCheck);
            //we need to select an attorney
            browser.ClickAction(WorkflowActivities.ApplicationManagement.SelectAttorney);
            browser.Page<Orig_SelectAttorney>().SelectAttorneyByDeedsOffice(Common.Constants.DeedsOffice.CapeTown);
            //now we can instruct
            browser.ClickAction(WorkflowActivities.ApplicationManagement.InstructAttorney);
            browser.Page<CorrespondenceProcessing>().SendCorrespondence(CorrespondenceSendMethodEnum.Email);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="Classification"></param>
        /// <param name="Rooftype"></param>
        /// <param name="Extent"></param>
        /// <param name="Rate"></param>
        /// <param name="Valuer"></param>
        internal static void BasicPerformManualValuation(TestBrowser browser, string Classification, string Rooftype, int Extent, int Rate, string Valuer)
        {
            DateTime Date = DateTime.Today;
            string ValuationDate;

            if (Date.Month < 10) ValuationDate = @"01" + @"/0" + Date.Month.ToString() + @"/" + Date.Year.ToString();
            else ValuationDate = @"01" + @"/" + Date.Month.ToString() + @"/" + Date.Year.ToString();

            string RoofDescription;
            int ReplacementValue;
            int TotalConventionalValue;
            int TotalThatchValue;

            browser.Page<ManualValuationsMainDwellingDetailsAdd>().MainDwellingDetailsAdd(Classification, Rooftype, Extent, Rate, null, -1, -1, -1,
                ButtonTypeEnum.Next);
            browser.Page<ManualValuationsMainDwellingExtendedDetailsAdd>().
                MainDwellingExtendedDetailsAdd(string.Empty, string.Empty, -1, -1, ButtonTypeEnum.Next);
            browser.Page<ManualValuationsMainDwellingDetails>().MainDwellingDetails(ButtonTypeEnum.Next, out ReplacementValue, out TotalConventionalValue,
                out TotalThatchValue);

            if (TotalConventionalValue > 0 && TotalThatchValue == 0)
            {
                RoofDescription = RoofTypes.Conventional;
                TotalThatchValue = -1;
            }
            else if (TotalConventionalValue == 0 && TotalThatchValue > 0)
            {
                RoofDescription = RoofTypes.Thatch;
                TotalConventionalValue = -1;
            }
            else
            {
                RoofDescription = RoofTypes.Partial;
            }

            browser.Page<ManualValuationAdd>().ValuationDetailsAdd(Valuer, ReplacementValue, ValuationDate, -1, RoofDescription, TotalConventionalValue, TotalThatchValue,
                ButtonTypeEnum.Next);
        }

        internal static void NTUTimeoutFromPipeline(IX2ScriptEngine scriptEngine, int offerKey, int accountKey)
        {
            var instanceID = x2Service.GetAppManInstanceIDByOfferKey(offerKey);
            scriptEngine.ExecuteScript(WorkflowEnum.ApplicationManagement, WorkflowAutomationScripts.ApplicationManagement.FireNTUTimeoutTimer, offerKey, TestUsers.NewBusinessProcessor);
            //check ework case
            eWorkAssertions.AssertEworkCaseExists(accountKey.ToString(), EworkStages.RefusalNoted, EworkMaps.Pipeline);
            X2Assertions.AssertCurrentX2State(instanceID, WorkflowStates.ApplicationManagementWF.ArchiveNTU);
        }

        internal static void EscalateToManager(TestBrowser browser, int offerKey)
        {
            browser = new TestBrowser(TestUsers.ValuationsProcessor, TestUsers.Password);
            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.ValuationsWF.ScheduleValuationAssessment);
            browser.ClickAction(WorkflowActivities.Valuations.EscalatetoManager);
            browser.Page<WorkflowYesNo>().ClickYes();
            browser.Dispose();
            browser = null;
        }

        internal static void ManagerArchive(TestBrowser browser, int offerKey)
        {
            browser = new TestBrowser(TestUsers.ValuationsManager, TestUsers.Password);
            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.ValuationsWF.ManagerReview);
            browser.ClickAction(WorkflowActivities.Valuations.ManagerArchive);
            browser.Page<WorkflowYesNo>().ClickYes();
            browser.Dispose();
            browser = null;
        }

        internal static int CreateApplicationAtStage(TestBrowser browser, IX2ScriptEngine scriptEngine, string stateName, OfferTypeEnum offerType, string legalEntityType, string product,
            int marketValue, int existingLoan, int cashOut, int cashDeposit, int houseHoldIncome, string employmentType,
            bool interestOnly, bool capitaliseFees)
        {
            browser = new TestBrowser(TestUsers.BranchConsultant, TestUsers.Password);
            browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(browser);
            browser.Navigate<BuildingBlocks.Navigation.CalculatorsNode>().Calculators(browser, CalculatorNodeTypeEnum.ApplicationCalculator);

            //Create Offer via Calculator
            switch (offerType)
            {
                case OfferTypeEnum.NewPurchase:
                    browser.Page<Views.LoanCalculator>().LoanCalculatorLead_NewPurchase(product.ToString(), marketValue.ToString(), cashDeposit.ToString(),
                        employmentType.ToString(), "240",  houseHoldIncome.ToString(),
                        ButtonTypeEnum.CreateApplication);
                    break;

                case OfferTypeEnum.SwitchLoan:
                    browser.Page<Views.LoanCalculator>().LoanCalculatorLead_Switch(product.ToString(), marketValue.ToString(), existingLoan.ToString(),
                        cashOut.ToString(), employmentType.ToString(), "240", Convert.ToBoolean(capitaliseFees), 
                        houseHoldIncome.ToString(), ButtonTypeEnum.CreateApplication);
                    break;

                case OfferTypeEnum.Refinance:
                    browser.Page<Views.LoanCalculator>().LoanCalculatorLead_Refinance(product.ToString(), marketValue.ToString(), cashOut.ToString(),
                        employmentType.ToString(), "240", Convert.ToBoolean(capitaliseFees), 
                        houseHoldIncome.ToString(), ButtonTypeEnum.CreateApplication);
                    break;
            }

            var random = new Random();
            var rnum = random.Next(0, 1000);

            if (legalEntityType.ToString() == LegalEntityType.NaturalPerson)
            {
                var idNumber = IDNumbers.GetNextIDNumber();
                var firstname = "FirstName" + rnum.ToString();
                var surname = "Surname" + rnum.ToString();
                browser.Page<LegalEntityDetailsLeadApplicantAdd>().AddLegalEntity(OfferRoleTypes.LeadMainApplicant, true, idNumber, "Mr", "auto", firstname, surname, "auto", Gender.Male,
                    MaritalStatus.Single, "Unknown", "Unknown", CitizenType.SACitizen, "auto", null, null, "Unknown", Language.English, null, "031", "1234567",
                    null, null, null, null, null, null, true, false, false, false, false, ButtonTypeEnum.Next);
            }
            else
            {
                var companyName = "CompanyName" + rnum.ToString();
                browser.Page<LegalEntityDetails>().AddLegalEntityCompany(legalEntityType.ToString(), companyName, "031", "1234567");
            }

            var offerKey = browser.Page<ApplicationSummaryBase>().GetOfferKey();
            browser.Dispose();
            browser = null;

            //Push offer to correct State
            //IX2ScriptEngine scriptEngine = new X2ScriptEngine();

            scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.ApplicationCapture, "SubmitApplication", offerKey);

            x2Service.WaitForAppManCaseCreate(offerKey);

            switch (stateName.ToString())
            {
                case WorkflowStates.ApplicationManagementWF.QA:
                    {
                        break;
                    }
                case WorkflowStates.ApplicationManagementWF.RequestatQA:
                    {
                        scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.ApplicationManagement, "CreateAtRequestAtQA", offerKey);
                        break;
                    }
                case WorkflowStates.ApplicationManagementWF.IssueAIP:
                    {
                        scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.ApplicationManagement, "QAComplete", offerKey);
                        break;
                    }
                case WorkflowStates.ApplicationManagementWF.ManageApplication:
                    {
                        if (offerType == OfferTypeEnum.NewPurchase)
                        {
                            scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.ApplicationManagement, "QAToManageApplication", offerKey);
                        }
                        break;
                    }
                case WorkflowStates.ApplicationManagementWF.ApplicationQuery:
                    {
                        if (offerType == OfferTypeEnum.NewPurchase)
                        {
                            scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.ApplicationManagement, "QAToManageApplication", offerKey);
                        }
                        scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.ApplicationManagement, "QueryOnApplication", offerKey);
                        break;
                    }
                case WorkflowStates.ApplicationManagementWF.Decline:
                    {
                        scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.ApplicationManagement, "Decline", offerKey);
                        break;
                    }
                case WorkflowStates.ApplicationManagementWF.NTU:
                    {
                        scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.ApplicationManagement, "NTU", offerKey);
                        break;
                    }
                case WorkflowStates.CreditWF.ValuationApprovalRequired:
                    {
                        browser = new TestBrowser(TestUsers.NewBusinessProcessor);
                        browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.ApplicationManagementWF.ManageApplication);

                        browser.Navigate<LoanDetailsNode>().ClickLoanDetailsNode();
                        _Application.InsertSettlementBanking(offerKey);
                        UpdateApplicationAttributes(browser);
                        SaveConditions(browser);
                        CompleteDocumentChecklist(browser);
                        browser.Dispose();
                        browser = null;
                        scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.ApplicationManagement, "SubmitToCredit", offerKey);

                        break;
                    }
                case WorkflowStates.CreditWF.Credit:
                    {
                        if (offerType == OfferTypeEnum.NewPurchase)
                        {
                            scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.ApplicationManagement, "QAToManageApplication", offerKey);
                        }
                        ProcessFromManageApplication(scriptEngine, offerKey, offerType, marketValue.ToString());

                        break;
                    }
                default:
                    break;
            }
            return offerKey;
        }

        internal static void ProcessFromManageApplication(IX2ScriptEngine scriptEngine, int offerKey, OfferTypeEnum offerType, string marketValue)
        {
            TestBrowser browser = null;
            _Application.InsertSettlementBanking(offerKey);
            if (offerType == OfferTypeEnum.SwitchLoan || offerType == OfferTypeEnum.Refinance)
            {
                X2Assertions.AssertX2CloneDoesNotExist(offerKey, WorkflowStates.ApplicationManagementWF.ValuationHold, Workflows.ApplicationManagement);
                scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.ApplicationManagement, "SubmitToCredit", offerKey);
                workflowService.WaitForX2State(offerKey, Workflows.Credit, WorkflowStates.CreditWF.ValuationApprovalRequired);
                scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.Credit, "ValuationApproved", offerKey);
                workflowService.WaitForX2State(offerKey, Workflows.Valuations, WorkflowStates.ValuationsWF.ScheduleValuationAssessment);
            }
            Helper.PerformManualValuation(browser, offerKey, marketValue.ToString());
            browser = new TestBrowser(TestUsers.NewBusinessProcessor);
            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.ApplicationManagementWF.ManageApplication);
            browser.Navigate<LoanDetailsNode>().ClickLoanDetailsNode();
            propertyService.DBUpdateDeedsOfficeDetails(offerKey);
            browser.Navigate<Navigation.PropertyAddressNode>().PropertyAddress(offerKey);
            Helper.UpdateHOCDetails(browser);
            browser.Navigate<LoanDetailsNode>().ClickLoanDetailsNode();
            Helper.UpdateApplicationAttributes(browser);
            Helper.SaveConditions(browser);
            Helper.CompleteDocumentChecklist(browser);
            browser.Dispose();
            browser = null;

            scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.ApplicationManagement, "SubmitToCredit", offerKey);
            workflowService.WaitForX2State(offerKey, Workflows.Credit, WorkflowStates.CreditWF.Credit);
        }

        internal static void ProcessFromValuationApprovalRequired(IX2ScriptEngine scriptEngine, int offerKey, string marketValue)
        {
            scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.Credit, "ValuationApproved", offerKey);
            var browser = new TestBrowser(TestUsers.NewBusinessProcessor);
            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.ApplicationManagementWF.ManageApplication);
            Helper.PerformManualValuation(browser, offerKey, marketValue.ToString());
            propertyService.DBUpdateDeedsOfficeDetails(offerKey);
            browser.Navigate<Navigation.PropertyAddressNode>().PropertyAddress(offerKey);
            Helper.UpdateHOCDetails(browser);
            browser.Navigate<LoanDetailsNode>().ClickLoanDetailsNode();
            Helper.UpdateApplicationAttributes(browser);
            Helper.SaveConditions(browser);
            Helper.CompleteDocumentChecklist(browser);
            browser.Dispose();
            browser = null;
            scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.ApplicationManagement, "SubmitToCredit", offerKey);
        }

        internal static void CompleteEzValForOffer(IX2ScriptEngine scriptEngine, int offerKey, float valuationAmount)
        {
            var workflowState = String.Empty;
            while (!workflowService.IsValuationsAtArchiveState(offerKey))
            {
                workflowState = workflowService.GetWorkflowState(offerKey);
                switch (workflowState)
                {
                    case WorkflowStates.ValuationsWF.ValuationAssessmentPending:
                        {
                            valuationService.SubmitCompletedEzVal(offerKey, HOCRoofEnum.Thatch, thatchAmount: 600000.00f, valuationAmount: valuationAmount);
                            workflowService.WaitForX2State(offerKey, Workflows.Valuations, WorkflowStates.ValuationsWF.ValuationComplete);
                            break;
                        }
                    case WorkflowStates.ValuationsWF.FurtherValuationRequest:
                        {
                            scriptEngine.ExecuteScript(WorkflowEnum.Valuations, WorkflowAutomationScripts.Valuations.RenstructValuer, offerKey, TestUsers.ValuationsProcessor);
                            workflowService.WaitForX2State(offerKey, Workflows.Valuations, WorkflowStates.ValuationsWF.ScheduleValuationAssessment);
                            break;
                        }
                    case WorkflowStates.ValuationsWF.ValuationReviewRequest:
                        {
                            scriptEngine.ExecuteScript(WorkflowEnum.Valuations, WorkflowAutomationScripts.Valuations.RequestValuationReview, offerKey, TestUsers.ValuationsProcessor);
                            workflowService.WaitForX2State(offerKey, Workflows.Valuations, WorkflowStates.ValuationsWF.ScheduleValuationAssessment);
                            break;
                        }
                    case WorkflowStates.ValuationsWF.ValuationComplete:
                        {
                            var dateFilter = DateTime.Now.Subtract(TimeSpan.FromMinutes(5));
                            var instanceID = x2Service.GetValuationsInstanceDetails(offerKey);
                            scriptEngine.ExecuteScript(WorkflowEnum.Valuations, WorkflowAutomationScripts.Valuations.ValuationinOrder, offerKey, TestUsers.ValuationsProcessor);
                            workflowService.WaitForX2WorkflowHistoryActivity(instanceID.Rows(0).Column("ID").GetValueAs<int>(),1,dateFilter.ToString(Formats.DateTimeFormatSQL), WorkflowActivities.Valuations.ValuationinOrder);
                            break;
                        }
                    case WorkflowStates.ValuationsWF.ScheduleValuationAssessment:
                        {
                            var browser = new TestBrowser(TestUsers.ValuationsProcessor);
                            browser.Navigate<NavigationHelper>().Task();
                            browser.Navigate<WorkFlowsNode>().WorkFlows(browser);
                            browser.Page<WorkflowSuperSearch>().WorkflowSearch(offerKey);
                            browser.ClickAction(WorkflowActivities.Valuations.InstructEzValValuer);
                            browser.Page<ValuationScheduleLightstoneValuation>().Populate(DateTime.Now.AddDays(1), "contact1", "workPhone", "tel phone");
                            browser.Page<ValuationScheduleLightstoneValuation>().Instruct();

                            workflowService.WaitForX2State(offerKey, Workflows.Valuations, WorkflowStates.ValuationsWF.ValuationAssessmentPending);
                        }
                        break;
                }
            }
        }

    }
}