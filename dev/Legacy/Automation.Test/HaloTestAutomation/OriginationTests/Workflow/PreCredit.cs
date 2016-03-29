using Automation.DataAccess;
using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation;
using BuildingBlocks.Navigation.FLOBO.Common;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.LegalEntity;
using BuildingBlocks.Presenters.Origination;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using Common.Extensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using WatiN.Core;
using WatiN.Core.Logging;
using Description = NUnit.Framework.DescriptionAttribute;

using Navigation = BuildingBlocks.Navigation;

namespace Origination.Workflow
{
    [RequiresSTA]
    public class _02PreCredit : Origination.OriginationTestBase<BasePage>
    {
        private const string TestFixtureString = "";

        protected override void OnTestTearDown()
        {
            base.OnTestTearDown();
            if (base.Browser != null)
            {
                try
                {
                    base.Browser.Page<BasePage>().CheckForErrorMessages();
                }
                finally
                {
                    base.Browser.Dispose();
                    base.Browser = null;
                }
            }
        }

        public IEnumerable<Automation.DataModels.OriginationTestCase> GetTestCasesForApplicationInOrder()
        {
            var testCases = Service<ICommonService>().GetOriginationTestCases();
            return (from t in testCases where t.ApplicationManagementTestGroup == "ApplicationInOrder" select t);
        }

        #region TestSetupData

        /// <summary>
        /// This test will NTU an application and ensure that the NTU timer is correctly setup to expire the case in 11 BUSINESS days from the date it enters the NTU state.
        /// The scheduled activity is then updated to expire almost immediately. The scheduled activities are then refreshed via an engine interface and then we make sure
        /// that the case has moved correctly.
        /// </summary>
        [Test, Description(@"This test will NTU an application and ensure that the NTU timer is correctly setup to expire the case in 11 BUSINESS days from the date it enters
		the NTU state. The scheduled activity is then updated to expire almost immediately. The scheduled activities are then refreshed via an engine interface and then we
		make sure that the case has moved correctly")]
        public void _001_NTUTimeout()
        {
            //Open base.Browser and login to Halo
            int offerKey = Service<IX2WorkflowService>().GetOfferKeyAtStateByType(WorkflowStates.ApplicationManagementWF.ManageApplication,
                Workflows.ApplicationManagement, OfferTypeEnum.SwitchLoan, Exclusions.OrginationAutomation);
            CreateCaseAtStageIfOfferkeyEmpty(ref offerKey, WorkflowStates.ApplicationManagementWF.ManageApplication, OfferTypeEnum.SwitchLoan,
                LegalEntityType.NaturalPerson, Products.NewVariableLoan, 1000000, 0, 0, 500000,  50000, EmploymentType.Salaried, false, false);
            base.Browser = new TestBrowser(TestUsers.NewBusinessProcessor, TestUsers.Password);
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.ApplicationManagementWF.ManageApplication);
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.NTU);
            base.Browser.Page<CommonReasonCommonDecline>().SelectReasonAndSubmit(ReasonType.ApplicationNTU);
            X2Assertions.AssertScheduledActivityTimer(offerKey.ToString(), ScheduledActivities.ApplicationManagement.NTUTimeout, 11, true);
            int instanceID = X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.NTU);
            //Update the Scheduled Activity Time
            base.scriptEngine.ExecuteScript(WorkflowEnum.ApplicationManagement, WorkflowAutomationScripts.ApplicationManagement.FireNTUTimeoutTimer, offerKey);
            Service<IX2WorkflowService>().WaitForX2WorkflowHistoryActivity(ScheduledActivities.ApplicationManagement.NTUTimeout, instanceID, 1);
            //check that the case has been archived
            X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.ArchiveNTU);
            //Assert the end date has been populated
            OfferAssertions.AssertOfferEndDate(offerKey, DateTime.Now, 0, false);
            //Assert the offer status is correct
            OfferAssertions.AssertOfferStatus(offerKey, OfferStatusEnum.NTU);
        }

        /// <summary>
        /// This test will decline an application and then update the scheduled activity to expire almost immediately. The scheduled activities are then refreshed via
        /// an engine interface and then we make sure that the case has moved correctly.
        /// </summary>
        [Test, Description(@"This test will decline an application and then update the scheduled activity to expire almost immediately. The scheduled activities are then refreshed
		via an engine interface and then we make sure that the case has moved correctly.")]
        public void _002_DeclineTimeoutApplication()
        {
            int offerKey = Service<IX2WorkflowService>().GetOfferKeyAtStateByType(WorkflowStates.ApplicationManagementWF.ManageApplication, Workflows.ApplicationManagement,
                OfferTypeEnum.SwitchLoan, Exclusions.OrginationAutomation);
            CreateCaseAtStageIfOfferkeyEmpty(ref offerKey, WorkflowStates.ApplicationManagementWF.ManageApplication, OfferTypeEnum.SwitchLoan,
                 LegalEntityType.NaturalPerson, Products.NewVariableLoan, 1000000, 0, 0, 500000,  50000, EmploymentType.Salaried, false, false);
            base.Browser = new TestBrowser(TestUsers.NewBusinessProcessor);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().WorkFlows(base.Browser);
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.ApplicationManagementWF.ManageApplication);
            //Decline the Application
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.Decline);
            base.Browser.Page<CommonReasonCommonDecline>().SelectReasonAndSubmit(ReasonType.AdministrativeDecline);
            int instanceID = X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.Decline);
            base.scriptEngine.ExecuteScript(WorkflowEnum.ApplicationManagement, WorkflowAutomationScripts.ApplicationManagement.FireDeclineTimeoutTimer, offerKey, TestUsers.NewBusinessProcessor);
            Service<IX2WorkflowService>().WaitForX2WorkflowHistoryActivity(ScheduledActivities.ApplicationManagement.DeclineTimeout, instanceID, 1);
            X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.ArchiveDecline);
            //Assert the end date has been populated
            OfferAssertions.AssertOfferEndDate(offerKey, DateTime.Now, 0, false);
            //Assert the offer status is correct
            OfferAssertions.AssertOfferStatus(offerKey, OfferStatusEnum.Declined);
        }

        #endregion TestSetupData

        #region ApplicationManagementWorkflowTests

        /// <summary>
        /// This test ensures that a branch consultant user can perform the Send AIP action on a case at the Issue AIP state. Once the action has been completed we then ensure
        /// that the Correspondence record has been added to the database correctly
        /// </summary>
        [Test, Description("Test ensures that a user can send the AIP document to the client")]
        public void _009_SendAIP()
        {
            //Andrewk: If you are looking at this test because it failed please check the TextOutput for a warning message.
            //This test will fail until we fix the reports for Revamp

            Console.WriteLine(@"--********SendAIP********--");
            int offerKey = Service<IX2WorkflowService>().GetOfferKeyAtStateByType(WorkflowStates.ApplicationManagementWF.IssueAIP, Workflows.ApplicationManagement,
                OfferTypeEnum.NewPurchase, Exclusions.OrginationAutomation);
            CreateCaseAtStageIfOfferkeyEmpty(ref offerKey, WorkflowStates.ApplicationManagementWF.IssueAIP, OfferTypeEnum.NewPurchase,
                 LegalEntityType.NaturalPerson, Products.NewVariableLoan, 1000000, 0, 0, 500000,  50000, EmploymentType.Salaried, false, false);
            base.Browser = new TestBrowser(TestUsers.BranchConsultant, TestUsers.Password);
            base.Browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().WorkFlows(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(base.Browser);
            base.Browser.Page<WorkflowSuperSearch>().Search(offerKey);
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.SendAIP);
            Settings.WaitForCompleteTimeOut = 90;

            base.Browser.Page<CorrespondenceProcessing>().SendCorrespondence(CorrespondenceSendMethodEnum.Post);
            //assert that the correspondence record has been added
            CorrespondenceAssertions.AssertCorrespondenceRecordAdded(offerKey, CorrespondenceReports.ApprovalInPrinciple, CorrespondenceMedium.Post);
        }

        /// <summary>
        /// The Branch Consultant user records the clients refusal of our AIP by performing the Client Refuse action at the Issue AIP state. This test will ensure the following:
        /// <list type="bullet">
        /// <item>
        /// <description>The case that is picked up at QA is in the same branch as our test consultant</description>
        /// </item>
        /// <item>
        /// <description>The case is moved to the Archive AIP state</description>
        /// </item>
        /// <item>
        /// <description>The application is marked as NTU</description>
        /// </item>
        /// <item>
        /// <description>The Offer End Date is set</description>
        /// </item>
        /// </list>
        /// </summary>
        [Test, Description("Test ensures that the Client Refuses action can be completed and the offer is updated correctly")]
        public void _010_ClientRefusesAIP()
        {
            Console.WriteLine(string.Format(@"--********{0}********--", MethodBase.GetCurrentMethod()));
            var results = Service<IX2WorkflowService>().GetOfferKeysAtStateByType(WorkflowStates.ApplicationManagementWF.IssueAIP, Workflows.ApplicationManagement,
                OfferTypeEnum.NewPurchase, Exclusions.OrginationAutomation);

            int offerKey = 0;

            offerKey = (from r in results
                        where Service<IADUserService>().IsUserInSameBranchAsApp(TestUsers.BranchConsultant, r.Column(0).Value)
                        select r.Column(0).GetValueAs<int>()).FirstOrDefault();

            CreateCaseAtStageIfOfferkeyEmpty(ref offerKey, WorkflowStates.ApplicationManagementWF.IssueAIP, OfferTypeEnum.NewPurchase,
                LegalEntityType.NaturalPerson, Products.NewVariableLoan, 1000000, 0, 0, 500000, 50000, EmploymentType.Salaried, false, false);

            Helper.ClientRefusesAIP(base.Browser, offerKey);
        }

        /// <summary>
        /// A branch user cannot perform Client Refuse on an application in another branch. This test will ensure that the warning message is displayed correctly when the
        /// user tries to perform the action.
        /// </summary>
        [Test, Description("A branch user cannot perform Client Refuse on an application in another branch.")]
        public void _010a_ClientRefusesAIPNotInSameBranch()
        {
            Console.WriteLine(@"--********_010a_ClientRefusesAIPNotInSameBranch********--");
            var results = Service<IX2WorkflowService>().GetOfferKeysAtStateByType(WorkflowStates.ApplicationManagementWF.IssueAIP, Workflows.ApplicationManagement,
                OfferTypeEnum.NewPurchase, Exclusions.OrginationAutomation);
            int offerKey = 0;
            offerKey = (from r in results
                        where
                            Service<IADUserService>().IsUserInSameBranchAsApp(TestUsers.BranchConsultant1, r.Column(0).Value) == false
                        select r.Column(0).GetValueAs<int>()).FirstOrDefault();

            CreateCaseAtStageIfOfferkeyEmpty(ref offerKey, WorkflowStates.ApplicationManagementWF.IssueAIP, OfferTypeEnum.NewPurchase,
                LegalEntityType.NaturalPerson, Products.NewVariableLoan, 1000000, 0, 0, 500000, 50000, EmploymentType.Salaried, false, false);

            base.Browser = new TestBrowser(TestUsers.BranchConsultant10, TestUsers.Password);
            base.Browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().WorkFlows(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(base.Browser);
            base.Browser.Page<WorkflowSuperSearch>().Search(offerKey);
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.ClientRefuse);
            base.Browser.Page<BasePageAssertions>().AssertFrameContainsText("To perform this function you must be within the same branch as the application.");
        }

        /// <summary>
        /// Once the client has accepted the AIP then the case is sent to the Manage Application state. This test will ensure that the following occurs:
        /// <list type="bullet">
        /// <item>
        /// <description>The case has moved to the Manage Application state in the Application Management workflow</description>
        /// </item>
        /// <item>
        /// <description>A cloned case has been created in the Valuations workflow at the Schedule Valuations Assessment state</description>
        /// </item>
        /// <item>
        /// <description>The next NBPUser due to be round robin assigned the application is correctly assigned to the application</description>
        /// </item>
        /// <item>
        /// <description>The next VPUser due to be round robin assigned the application is correctly assigned to the cloned application in Valuations</description>
        /// </item>
        /// </list>
        /// </summary>
        [Test, Description("Test ensures that the Client Accepts action can be completed and the case moves to Manage Application")]
        public void _011_ClientAcceptsAIP()
        {
            Console.WriteLine(@"--********ClientAcceptAIP********--");
            int offerKey = Service<IX2WorkflowService>().GetOfferKeyAtStateByType(WorkflowStates.ApplicationManagementWF.IssueAIP, Workflows.ApplicationManagement,
                OfferTypeEnum.NewPurchase, Exclusions.OrginationAutomation);
            CreateCaseAtStageIfOfferkeyEmpty(ref offerKey, WorkflowStates.ApplicationManagementWF.IssueAIP, OfferTypeEnum.NewPurchase,
                LegalEntityType.NaturalPerson, Products.NewVariableLoan, 1000000, 0, 0, 500000, 50000, EmploymentType.Salaried, false, false);
            base.Browser = new TestBrowser(TestUsers.BranchConsultant, TestUsers.Password);
            base.Browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().WorkFlows(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(base.Browser);
            base.Browser.Page<WorkflowSuperSearch>().Search(offerKey);
            //we need to fetch the next NBPUser we expect to get the case
            string expectedUser = Service<IAssignmentService>().GetNextRoundRobinAssignmentByOfferRoleTypeKey(OfferRoleTypeEnum.NewBusinessProcessorD,
                RoundRobinPointerEnum.NewBusinessProcessor);
            string expValuationsUser = Service<IAssignmentService>().GetNextRoundRobinAssignmentByOfferRoleTypeKey(OfferRoleTypeEnum.ValuationsAdministratorD,
                RoundRobinPointerEnum.ValuationsAdministrator);
            Int64 instanceid = Service<IX2WorkflowService>().GetAppManInstanceIDByState(WorkflowStates.ApplicationManagementWF.IssueAIP, offerKey);
            //perform the action
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.ClientAccepts);
            //confirm
            base.Browser.Page<WorkflowYesNo>().Confirm(true, false);
            //we need to wait
            Service<IX2WorkflowService>().WaitForX2WorkflowHistoryActivity(ConditionalActivities.ApplicationManagement.AssignProcessor, instanceid, 1);
            //assert that the case has moved states
            X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.ManageApplication);
            //we need to wait
            int valInstanceID = Service<IX2WorkflowService>().GetValuationsInstanceDetails(offerKey).Rows(0).Column("InstanceID").GetValueAs<int>();
            Service<IX2WorkflowService>().WaitForX2WorkflowHistoryActivity(ConditionalActivities.Valuations.ValuationRequested, valInstanceID, 1);
            //is the val clone created
            X2Assertions.AssertCurrentValuationsX2State(offerKey, WorkflowStates.ValuationsWF.ScheduleValuationAssessment);
            //is the case assigned to the correct NBPUser
            AssignmentAssertions.AssertThatAWorkFlowAssignmentRecordExists(offerKey, OfferRoleTypeEnum.NewBusinessProcessorD);
            AssignmentAssertions.AssertOfferRoleRecordExists(offerKey, OfferRoleTypeEnum.NewBusinessProcessorD);
            AssignmentAssertions.AssertThatTheWorkFlowAssignmentRecordIsActive(offerKey, OfferRoleTypeEnum.NewBusinessProcessorD);
            AssignmentAssertions.AssertWhoTheOfferRoleRecordIsAssignedTo(offerKey, OfferRoleTypeEnum.NewBusinessProcessorD, expectedUser);
            AssignmentAssertions.AssertWhoTheWorkFlowAssignmentRecordIsAssignedTo(offerKey, OfferRoleTypeEnum.NewBusinessProcessorD, expectedUser);
            //is the valuations case assigned correctly
            AssignmentAssertions.AssertThatAWorkFlowAssignmentRecordExists(offerKey, OfferRoleTypeEnum.ValuationsAdministratorD);
            AssignmentAssertions.AssertOfferRoleRecordExists(offerKey, OfferRoleTypeEnum.ValuationsAdministratorD);
            AssignmentAssertions.AssertThatTheWorkFlowAssignmentRecordIsActive(offerKey, OfferRoleTypeEnum.ValuationsAdministratorD);
            AssignmentAssertions.AssertWhoTheOfferRoleRecordIsAssignedTo(offerKey, OfferRoleTypeEnum.ValuationsAdministratorD, expValuationsUser);
            AssignmentAssertions.AssertWhoTheWorkFlowAssignmentRecordIsAssignedTo(offerKey, OfferRoleTypeEnum.ValuationsAdministratorD, expValuationsUser);
        }

        /// <summary>
        /// Verify that a New Business Processor can perform the 'Query On Application' action which will move the application
        /// to the 'Application Query' state
        /// <list type="table">
        /// <listheader>
        /// <workflow>Workflow</workflow>
        /// </listheader>
        /// <item>
        /// <workflow>Application Management</workflow>
        /// </item>
        /// </list>
        /// </summary>
        [Test, Description("Verify that a New Business Processor can perform the 'Query On Application' action which will move the application to the 'Application Query' state")]
        public void _025_QueryOnApplication()
        {
            Console.WriteLine(@"--********QueryOnApplication********--");

            QueryResults results = Service<ICommonService>().OffersAtApplicationCaptureRow("QueryOnApplication");

            string branchConsultant = results.Rows(0).Column("Username").Value;
            int offerKey = results.Rows(0).Column("OfferKey").GetValueAs<int>();
            string loanType = results.Rows(0).Column("LoanType").Value;
            string legalEntityType = results.Rows(0).Column("LegalEntityType").Value;
            string product = results.Rows(0).Column("Product").Value;
            int marketValue = results.Rows(0).Column("MarketValue").GetValueAs<int>();
            int existingLoan = results.Rows(0).Column("ExistingLoan").GetValueAs<int>();
            int cashOut = results.Rows(0).Column("CashOut").GetValueAs<int>();
            int cashDeposit = results.Rows(0).Column("CashDeposit").GetValueAs<int>();
            int confirmedIncome = results.Rows(0).Column("HouseHoldIncome").GetValueAs<int>();
            string employmentType = results.Rows(0).Column("EmploymentType").Value;

            OfferTypeEnum offerType = new OfferTypeEnum();
            switch (loanType)
            {
                case "Switch loan":
                    offerType = OfferTypeEnum.SwitchLoan;
                    break;

                case "New purchase":
                    offerType = OfferTypeEnum.NewPurchase;
                    break;

                case "Refinance":
                    offerType = OfferTypeEnum.Refinance;
                    break;

                default:
                    break;
            }

            CreateCaseAtStageIfOfferNotAtState(ref offerKey, WorkflowStates.ApplicationManagementWF.QA, offerType,
                legalEntityType, product, marketValue, existingLoan, cashOut,
                cashDeposit, confirmedIncome, employmentType, false, false);

            Service<ICommonService>().CommitOfferKeyForTestIdentifier("ApplicationManagementTestID", "QueryOnApplication", offerKey);

            base.Browser = new TestBrowser(TestUsers.NewBusinessProcessor, TestUsers.Password);
            //Step 1: Select offer from worklist
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.ApplicationManagementWF.ManageApplication);
            //Step 2: Complete Query On Application action
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.QueryonApplication);
            base.Browser.Page<CommonReasonCommonDecline>().SelectReasonAndSubmit(ReasonType.ProcessorQuery);
            //Step 3: Assert application has moved to Application Query state and been assigned to the correct user
            X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.ApplicationQuery);

            AssignmentAssertions.AssertWhoTheWorkFlowAssignmentRecordIsAssignedTo(offerKey, OfferRoleTypeEnum.BranchConsultantD, branchConsultant);
            AssignmentAssertions.AssertThatTheWorkFlowAssignmentRecordIsActive(offerKey, OfferRoleTypeEnum.BranchConsultantD);
            AssignmentAssertions.AssertThatTheWorkFlowAssignmentRecordIsInactive(offerKey, OfferRoleTypeEnum.NewBusinessProcessorD);

            AssignmentAssertions.AssertWhoTheOfferRoleRecordIsAssignedTo(offerKey, OfferRoleTypeEnum.BranchConsultantD, branchConsultant);
            AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(offerKey, OfferRoleTypeEnum.BranchConsultantD);
            AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(offerKey, OfferRoleTypeEnum.NewBusinessProcessorD);

            results.Dispose();
        }

        /// <summary>
        /// Verify that a Branch Consultant can perform the 'Feedback On Application' action which will move the application
        /// to the 'Manage Application' state
        /// <list type="table">
        /// <listheader>
        /// <workflow>Workflow</workflow>
        /// </listheader>
        /// <item>
        /// <workflow>Application Management</workflow>
        /// </item>
        /// </list>
        /// </summary>
        [Test, Description("Verify that a Branch Consultant can perform the 'Feedback On Application' action which will move the application to the 'Manage Application' state")]
        public void _026_FeedbackOnQuery()
        {
            Console.WriteLine(@"--********FeedbackOnQuery********--");
            QueryResults results = Service<ICommonService>().OffersAtApplicationCaptureRow("QueryOnApplication");
            string branchConsultant = results.Rows(0).Column("Username").Value;
            int offerKey = results.Rows(0).Column("OfferKey").GetValueAs<int>();
            string loanType = results.Rows(0).Column("LoanType").Value;
            string legalEntityType = results.Rows(0).Column("LegalEntityType").Value;
            string product = results.Rows(0).Column("Product").Value;
            int marketValue = results.Rows(0).Column("MarketValue").GetValueAs<int>();
            int existingLoan = results.Rows(0).Column("ExistingLoan").GetValueAs<int>();
            int cashOut = results.Rows(0).Column("CashOut").GetValueAs<int>();
            int cashDeposit = results.Rows(0).Column("CashDeposit").GetValueAs<int>();
            int confirmedIncome = results.Rows(0).Column("HouseHoldIncome").GetValueAs<int>();
            string employmentType = results.Rows(0).Column("EmploymentType").Value;

            OfferTypeEnum offerType = new OfferTypeEnum();
            switch (loanType)
            {
                case "Switch loan":
                    offerType = OfferTypeEnum.SwitchLoan;
                    break;

                case "New purchase":
                    offerType = OfferTypeEnum.NewPurchase;
                    break;

                case "Refinance":
                    offerType = OfferTypeEnum.Refinance;
                    break;

                default:
                    break;
            }

            CreateCaseAtStageIfOfferNotAtState(ref offerKey, WorkflowStates.ApplicationManagementWF.ApplicationQuery, offerType,
                legalEntityType, product, marketValue, existingLoan, cashOut,
                cashDeposit, confirmedIncome, employmentType, false, false);

            Service<ICommonService>().CommitOfferKeyForTestIdentifier("ApplicationManagementTestID", "QueryOnApplication", offerKey);

            base.Browser = new TestBrowser(branchConsultant, TestUsers.Password);
            //Step 1: Select offer from worklist
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.ApplicationManagementWF.ApplicationQuery);
            //Step 2: Complete Query On Application action
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.FeedbackonQuery);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, false);
            //Step 3: Assert application has moved to the 'Manage Application' state and been assigned to the correct user
            X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.ManageApplication);

            AssignmentAssertions.AssertThatTheWorkFlowAssignmentRecordIsActive(offerKey, OfferRoleTypeEnum.NewBusinessProcessorD);
            AssignmentAssertions.AssertThatTheWorkFlowAssignmentRecordIsInactive(offerKey, OfferRoleTypeEnum.BranchConsultantD);

            AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(offerKey, OfferRoleTypeEnum.NewBusinessProcessorD);
            AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(offerKey, OfferRoleTypeEnum.BranchConsultantD);
        }

       

        /// <summary>
        /// Verify that a New Business Processor can perform the 'Request Lightstone Valuation' action at 'Manage Application' state.  Ensure
        /// no valuation is carried out where no Lightstone PropertyID exists for the Offer Property
        /// <list type="table">
        /// <listheader>
        /// <workflow>Workflow</workflow>
        /// </listheader>
        /// <item>
        /// <workflow>Application Management</workflow>
        /// </item>
        /// </list>
        /// </summary>
        [Test, Description(@"Verify that a New Business Processor can perform the 'Request Lightstone Valuation' action at 'Manage Application' state.  Ensure
			no valuation is carried out where no Lightstone PropertyID exists for the Offer Property")]
        public void _030_RequestLightstoneValuation_NoPropertyID()
        {
            Console.WriteLine(@"--********RequestLightstoneValuation_NoPropertyID********--");
            int offerKey = Service<IX2WorkflowService>().GetOffersWithoutLightstonePropertyID("Application Management",
                WorkflowStates.ApplicationManagementWF.ManageApplication);
            base.Browser = new TestBrowser(TestUsers.NewBusinessProcessor, TestUsers.Password);
            Helper.RequestLightstoneValuation(base.Browser, offerKey);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(@"There is no LightStone property ID to do a valuation.");
        }
    
        /// <summary>
        /// Verify that a New Business Processor can complete the Application in Order action at the Manage Application state.
        /// Ensure that the application moves to the Cridit state in the Credit workflow and is assigned to the correct user.
        /// </summary>
        [Test, TestCaseSource(typeof(_02PreCredit), "GetTestCasesForApplicationInOrder"), Description(@"Verify that a New Business Processor can complete the Application in Order action at the Manage Application state.
            Ensure that the application moves to the Cridit state in the Credit workflow and is assigned to the correct user.")]
        public void _038_ApplicationInOrder(Automation.DataModels.OriginationTestCase testCase)
        {
            ApplicationInOrder(testCase.ApplicationManagementTestID);
        }

        /// <summary>
        /// Verify that a New Business Processor can complete the Application in Order action at the Manage Application state.  Ensure that the application moves
        /// to the Credit state in the Credit workflow and is assigned to the correct user.
        /// <list type="table">
        /// <listheader>
        /// <workflow>Workflow</workflow>
        /// </listheader>
        /// <item>
        /// <workflow>Application Management</workflow>
        /// </item>
        /// <item>
        /// <workflow>Credit</workflow>
        /// </item>
        /// </list>
        /// <seealso cref="_038_ApplicationInOrder"/>
        /// </summary>
        [Test, TestCaseSource(typeof(_02PreCredit), "GetTestCasesForApplicationInOrder"), Description(@"Verify that a New Business Processor can complete the Application in Order action at the Manage Application state.
            Ensure that the application moves to the Cridit state in the Credit workflow and is assigned to the correct user.")]
        public void _038a_ApplicationInOrderAssertions(Automation.DataModels.OriginationTestCase testCase)
        {
            int offerKey = Service<ICommonService>().GetOfferKeyByTestIdentifier(testCase.ApplicationManagementTestID, "ApplicationManagementTestID");
            string creditAdUser;
            OfferRoleTypeEnum creditOfferRoleType;

            //Assert that case is at the ValuationApprovalRequired or Credit state.
            X2Assertions.AssertCurrentCreditX2State(offerKey,
                    WorkflowStates.CreditWF.ValuationApprovalRequired,
                    WorkflowStates.CreditWF.Credit);

            //Assert Credit Underwriter D offerrole exists
            Service<IFurtherLendingService>().UpdateAssignedCreditUser(offerKey, null, out creditAdUser, out creditOfferRoleType);
            AssignmentAssertions.AssertOfferRoleRecordExists(offerKey, creditOfferRoleType);
            AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(offerKey, creditOfferRoleType);
            AssignmentAssertions.AssertThatAWorkFlowAssignmentRecordExists(offerKey, creditOfferRoleType);
            AssignmentAssertions.AssertThatTheWorkFlowAssignmentRecordIsActive(offerKey, creditOfferRoleType);

            //Check New Business Processor D offerrole
            AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(offerKey, OfferRoleTypeEnum.NewBusinessProcessorD);
            AssignmentAssertions.AssertThatTheWorkFlowAssignmentRecordIsInactive(offerKey, OfferRoleTypeEnum.NewBusinessProcessorD);
        }

        /// <summary>
        /// Verify that a New Business Processor can rework an application in the application management map, and that the revision history is correct.
        /// <list type="table">
        /// <listheader>
        /// <workflow>Workflow</workflow>
        /// </listheader>
        /// <item>
        /// <workflow>Application Management</workflow>
        /// </item>
        /// </list>
        /// <seealso cref="_038a_ApplicationInOrderAssertions"/>
        /// </summary>
        [Test, Description("Verify that a New Business Processor can rework an application in the application management map, and that the revision history is correct.")]
        public void _039_ReworkApplication()
        {
            string productToSelect = string.Empty;
            ProductEnum expectedProduct = ProductEnum.None;
            QueryResultsRow testOfferRow = Service<ICommonService>().GetRandomOfferRow(OfferTypeEnum.NewPurchase, Workflows.ApplicationManagement,
                WorkflowStates.ApplicationManagementWF.ManageApplication);
            int offerKey = testOfferRow.Column("OfferKey").GetValueAs<int>();

            CreateCaseAtStageIfOfferkeyEmpty(ref offerKey, WorkflowStates.ApplicationManagementWF.ManageApplication, OfferTypeEnum.NewPurchase,
                LegalEntityType.NaturalPerson, Products.NewVariableLoan, 1000000, 0, 0, 500000,  50000, EmploymentType.Salaried, false, false);

            int ExpectedOfferInfoCount = Service<IApplicationService>().GetOfferInformationRecordCount(offerKey);
            ExpectedOfferInfoCount = ExpectedOfferInfoCount + 1;

            ProductEnum currentProduct = new ProductEnum();
            string str;
            foreach (ProductEnum p in Enum.GetValues(typeof(ProductEnum)))
            {
                str = testOfferRow.Column("Product").Value;
                str = str.RemoveWhiteSpace();
                if (p.ToString() == str)
                    currentProduct = p;
            }

            if (currentProduct != ProductEnum.NewVariableLoan)
            {
                productToSelect = Products.NewVariableLoan;
                expectedProduct = ProductEnum.NewVariableLoan;
            }
            else if (currentProduct != ProductEnum.Edge)
            {
                productToSelect = Products.Edge;
                expectedProduct = ProductEnum.Edge;
            }
            else if (currentProduct != ProductEnum.VariFixLoan)
            {
                productToSelect = Products.VariFixLoan;
                expectedProduct = ProductEnum.VariFixLoan;
            }

            base.Browser = new TestBrowser(TestUsers.NewBusinessProcessor);
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.ApplicationManagementWF.ManageApplication);
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.ReworkApplication);
            base.Browser.Page<ApplicationLoanDetailsUpdate>().ChangeApplicationProduct(productToSelect);
            //assert that the latest OI has been updated
            OfferAssertions.AssertLatestOfferInformationProduct(offerKey, productToSelect);
            //assert the revision has been created
            OfferAssertions.OfferInformationUpdated(offerKey, OfferInformationTypeEnum.RevisedOffer, expectedProduct);
        }

        /// <summary>
        /// Verify that whilst the ValuationRequired flag is set to FALSE the NBPUser cannot perform the Review Valuation Required action.
        /// <list type="table">
        /// <listheader>
        /// <workflow>Workflow</workflow>
        /// </listheader>
        /// <item>
        /// <workflow>Application Management</workflow>
        /// </item>
        /// </list>
        /// </summary>
        [Test, Description("Verify that whilst the ValuationRequired flag is set to FALSE the NBPUser cannot perform the Review Valuation Required action")]
        public void _040_ReviewValuationRequired_ValuationRequiredFlagFalse()
        {
            int offerKey = Service<IX2WorkflowService>().GetWorkflowCaseWithoutBusinessEvent(WorkflowStates.ApplicationManagementWF.ManageApplication,
                Workflows.ApplicationManagement, OfferTypeEnum.SwitchLoan, StageDefinitionStageDefinitionGroupEnum.Credit_ValuationApproved);
            base.Browser = new TestBrowser(TestUsers.NewBusinessProcessor, TestUsers.Password);
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.ApplicationManagementWF.ManageApplication);
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.ReviewValuationRequired);
            base.Browser.Page<BasePageAssertions>().AssertFrameContainsText("Valuation is not required");
        }

        #region Valuation 12 Months Old

        /// <summary>
        /// Check that when the valuation age for a property valuation is less than 12 months old, that the valuation amount from the valuation record found
        /// is used in the LTV calculations and is displayed on all screens that display the valuation amount.  Ensure that a Valuation Clone is NOT created on
        /// entering  Application Management map
        /// </summary>
        [Test, Description(@"Check that when the valuation age for a property valuation is less than 12 months old, that the valuation amount from the valuation record found
		is used in the LTV calculations and is displayed on all screens that display the valuation amount.  Ensure that a Valuation Clone is NOT created on
		entering  Application Management map")]
        public void _044_ValuationLessThan12MonthsOld_CloneNOTCreated_LTVReCalc()
        {
            var randomOffer
                  = Service<ICommonService>().GetRandomOfferRow(OfferTypeEnum.NewPurchase, Workflows.ApplicationManagement, WorkflowStates.ApplicationManagementWF.IssueAIP);
            var offerKey = 0;
            if (randomOffer != null)
            {
                offerKey = randomOffer.Column(0).GetValueAs<int>();
            }
            else
            {
                CreateCaseAtStageIfOfferkeyEmpty(ref offerKey, WorkflowStates.ApplicationManagementWF.IssueAIP, OfferTypeEnum.NewPurchase,
                    LegalEntityType.NaturalPerson, Products.NewVariableLoan, 1000000, 0, 0, 500000,  50000, EmploymentType.Salaried, false, false);
            }
            var propertyRow = Service<IPropertyService>().GetPropertyByOfferKey(offerKey);

            var valuation = new Automation.DataModels.Valuation();
            valuation.ValuationDate = DateTime.Now.Subtract(TimeSpan.FromDays(270));
            valuation.ValuatorKey = 18;
            valuation.ValuationAmount = 1110000;
            valuation.ValuationHOCValue = 1861200;
            valuation.ValuationClassificationKey = null;
            valuation.ValuationMunicipal = 0;
            valuation.ChangeDate = DateTime.Now;
            valuation.ValuationUserID = TestUsers.HaloUser;
            valuation.PropertyKey = propertyRow.Rows(0).Column("propertykey").GetValueAs<int>();
            valuation.ValuationStatusKey = Common.Enums.ValuationStatusEnum.Complete;
            valuation.ValuationDataProviderDataServiceKey = 7;
            valuation.ValuationEscalationPercentage = 20;
            valuation.IsActive = true;
            valuation.HOCRoofKey = HOCRoofEnum.Conventional;

            Service<IValuationService>().InsertValuation(valuation);
            Helper.AcceptAIP(base.Browser, TestUsers.BranchConsultant, offerKey);
            Service<IX2WorkflowService>().WaitForX2State(offerKey, Workflows.ApplicationManagement, WorkflowStates.ApplicationManagementWF.ManageApplication);
            X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.ManageApplication);
            X2Assertions.AssertX2CloneDoesNotExist(offerKey, WorkflowStates.ApplicationManagementWF.ValuationHold, Workflows.ApplicationManagement);
            PropertyValuationAssertions.AssertRequireValuationIndicatorValue(offerKey, true);

            Service<IValuationService>().DeleteValuationRecord(valuation);
        }

        /// <summary>
        /// Check that if an active valuation record is found for a property where the valuation age for that valuation is exactly 12 months old,
        /// that the client’s estimate/purchase price, is used in the LTV calculations and is displayed on all screens that display the valuation amount.
        /// Ensure that a Valuation Clone IS created on entering  Application Management map
        /// </summary>
        [Test, Description(@"Check that if an active valuation record is found for a property where the valuation age for that valuation is exactly 12 months
		old, that the client’s estimate/purchase price, is used in the LTV calculations and is displayed on all screens that display the valuation amount.
		Ensure that a Valuation Clone IS created on entering  Application Management map")]
        public void _045_Valuation12MonthsOld_CloneCreated_LTVReCalc()
        {
            var randomOffer
                   = Service<ICommonService>().GetRandomOfferRow(OfferTypeEnum.NewPurchase, Workflows.ApplicationManagement, WorkflowStates.ApplicationManagementWF.IssueAIP);
            var offerKey = 0;
            if (randomOffer != null)
            {
                offerKey = randomOffer.Column(0).GetValueAs<int>();
            }
            else
            {
                CreateCaseAtStageIfOfferkeyEmpty(ref offerKey, WorkflowStates.ApplicationManagementWF.IssueAIP, OfferTypeEnum.NewPurchase,
                    LegalEntityType.NaturalPerson, Products.NewVariableLoan, 1000000, 0, 0, 500000, 50000, EmploymentType.Salaried, false, false);
            }
            var propertyRow = Service<IPropertyService>().GetPropertyByOfferKey(offerKey);

            var valuation = new Automation.DataModels.Valuation();
            valuation.ValuationDate = DateTime.Now.Subtract(TimeSpan.FromDays(365.242));
            valuation.ValuatorKey = 18;
            valuation.ValuationAmount = 1110000;
            valuation.ValuationHOCValue = 1861200;
            valuation.ValuationClassificationKey = null;
            valuation.ValuationMunicipal = 0;
            valuation.ChangeDate = DateTime.Now;
            valuation.ValuationUserID = @"SAHL\HaloUser";
            valuation.PropertyKey = propertyRow.Rows(0).Column("propertykey").GetValueAs<int>();
            valuation.ValuationStatusKey = Common.Enums.ValuationStatusEnum.Complete;
            valuation.ValuationDataProviderDataServiceKey = 7;
            valuation.ValuationEscalationPercentage = 20;
            valuation.IsActive = true;
            valuation.HOCRoofKey = HOCRoofEnum.Conventional;

            Service<IValuationService>().InsertValuation(valuation);

            //Set the valuation indicator to required so that a valuation clone gets created and not interfere with testing the 12 month rule.
            var instanceId = Service<IX2WorkflowService>().GetAppManInstanceDetails(offerKey)
                                               .Select(x => x.Column("instanceId").GetValueAs<int>()).FirstOrDefault();
            Service<IX2WorkflowService>().SetIsValuationRequiredIndicator(true, instanceId);

            Helper.AcceptAIP(base.Browser, TestUsers.BranchConsultant, offerKey);

            Service<IX2WorkflowService>().WaitForX2State(offerKey, Workflows.ApplicationManagement, WorkflowStates.ApplicationManagementWF.ManageApplication);

            X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.ManageApplication);
            X2Assertions.AssertX2CloneExists(offerKey.ToString(), WorkflowStates.ApplicationManagementWF.ValuationHold, Workflows.ApplicationManagement);
            X2Assertions.AssertCurrentValuationsX2State(offerKey, WorkflowStates.ValuationsWF.ScheduleValuationAssessment);

            Service<IValuationService>().DeleteValuationRecord(valuation);
        }

        /// <summary>
        /// Check that if an active valuation record is found for a property where the valuation age for that valuation is greater than 12 months old,
        /// that the client’s estimate/purchase price, is used in the LTV calculations and is displayed on all screens that display the valuation amount.
        /// Ensure that a Valuation Clone IS created on entering  Application Management map
        /// </summary>
        [Test, Description(@"Check that if an active valuation record is found for a property where the valuation age for that valuation is greater than 12
		months old, that the client’s estimate/purchase price, is used in the LTV calculations and is displayed on all screens that display the valuation amount.
		Ensure that a Valuation Clone IS created on entering  Application Management map")]
        public void _046_ValuationGreaterThan12MonthsOld_CloneCreated_LTVReCalc()
        {
            const string testIdentifier = "ValuationGreaterThan12MonthsOld";

            QueryResults results = Service<ICommonService>().OffersAtApplicationCaptureRow(testIdentifier, "ApplicationManagementTestID");
            string branchConsultant = results.Rows(0).Column("Username").Value;
            int offerKey = results.Rows(0).Column("OfferKey").GetValueAs<int>();
            string loanType = results.Rows(0).Column("LoanType").Value;
            string legalEntityType = results.Rows(0).Column("LegalEntityType").Value;
            string product = results.Rows(0).Column("Product").Value;
            int marketValue = results.Rows(0).Column("MarketValue").GetValueAs<int>();
            int existingLoan = results.Rows(0).Column("ExistingLoan").GetValueAs<int>();
            int cashOut = results.Rows(0).Column("CashOut").GetValueAs<int>();
            int cashDeposit = results.Rows(0).Column("CashDeposit").GetValueAs<int>();
            int confirmedIncome = results.Rows(0).Column("HouseHoldIncome").GetValueAs<int>();
            string employmentType = results.Rows(0).Column("EmploymentType").Value;

            OfferTypeEnum offerType = new OfferTypeEnum();
            switch (loanType)
            {
                case "Switch loan":
                    offerType = OfferTypeEnum.SwitchLoan;
                    break;

                case "New purchase":
                    offerType = OfferTypeEnum.NewPurchase;
                    break;

                case "Refinance":
                    offerType = OfferTypeEnum.Refinance;
                    break;

                default:
                    break;
            }

            CreateCaseAtStageIfOfferNotAtState(ref offerKey, WorkflowStates.ApplicationManagementWF.QA, offerType,
                legalEntityType, product, marketValue, existingLoan, cashOut,
                cashDeposit, confirmedIncome, employmentType, false, false);

            Service<ICommonService>().CommitOfferKeyForTestIdentifier("ApplicationManagementTestID", testIdentifier, offerKey);

            string propertyValuationAmount = Service<ICommonService>().OffersAtApplicationCaptureRow(testIdentifier).Rows(0).Column("MarketValue").Value;
            string propertyKey = Service<IPropertyService>().GetAddressDetailsForPropertyValuationGreaterThan12MonthsOld(
                Convert.ToInt32(propertyValuationAmount)).Rows(0).Column("PropertyKey").Value;
            Service<IPropertyService>().UpdateOfferMortgageLoanPropertyKey(Convert.ToInt32(propertyKey), Convert.ToInt32(offerKey));
            MoveCaseFromQAtoManageApplication(offerKey);
            Service<IX2WorkflowService>().WaitForX2State(offerKey, Workflows.ApplicationManagement, WorkflowStates.ApplicationManagementWF.ManageApplication);
            X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.ManageApplication);
            X2Assertions.AssertX2CloneExists(offerKey.ToString(), WorkflowStates.ApplicationManagementWF.ValuationHold, Workflows.ApplicationManagement);
            X2Assertions.AssertCurrentValuationsX2State(offerKey, WorkflowStates.ValuationsWF.ScheduleValuationAssessment);
        }

        /// <summary>
        /// Verify that a New Business Processor can perform the 'Request Lightstone Valuation' action at 'Manage Application' state.  Ensure
        /// no valuation is carried out where the latest valuation for the Offer Property is less than 2 months old
        /// <list type="table">
        /// <listheader>
        /// <workflow>Workflow</workflow>
        /// </listheader>
        /// <item>
        /// <workflow>Application Management</workflow>
        /// </item>
        /// </list>
        /// </summary>
        [Test, Description(@"Verify that a New Business Processor can perform the 'Request Lightstone Valuation' action at 'Manage Application' state.  Ensure
			no valuation is carried out where the latest valuation for the Offer Property is less than 2 months old")]
        public void _047_RequestLightstoneValuation_LessThan2MonthsOld()
        {
            Console.WriteLine(@"--********RequestLightstoneValuation_LessThan2MonthsOld********--");

            var randomOffer
                = Service<ICommonService>().GetRandomOfferRow(OfferTypeEnum.NewPurchase, Workflows.ApplicationManagement, WorkflowStates.ApplicationManagementWF.ManageApplication);
            var offerkey = 0;
            if (randomOffer != null)
            {
                offerkey = randomOffer.Column(0).GetValueAs<int>();
            }
            else
            {
                CreateCaseAtStageIfOfferkeyEmpty(ref offerkey, WorkflowStates.ApplicationManagementWF.ManageApplication, OfferTypeEnum.NewPurchase,
                    LegalEntityType.NaturalPerson, Products.NewVariableLoan, 1000000, 0, 0, 500000, 50000, EmploymentType.Salaried, false, false);
            }
            var property = Service<IPropertyService>().GetPropertyByOfferKey(offerkey);

            var valuation = new Automation.DataModels.Valuation();
            valuation.ValuationKey = 9999998;
            valuation.ValuationDate = DateTime.Now;
            valuation.ValuatorKey = 50;
            valuation.ValuationAmount = 1070000;
            valuation.ValuationHOCValue = 1070000;
            valuation.ValuationClassificationKey = null;
            valuation.ChangeDate = DateTime.Now;
            valuation.ValuationMunicipal = 0;
            valuation.ValuationUserID = @"SAHL\HaloUser";
            valuation.PropertyKey = property.Rows(0).Column("propertykey").GetValueAs<int>();
            valuation.ValuationStatusKey = Common.Enums.ValuationStatusEnum.Complete;
            valuation.ValuationDataProviderDataServiceKey = 3;
            valuation.IsActive = false;
            valuation.HOCRoofKey = HOCRoofEnum.Conventional;

            Service<IValuationService>().InsertValuation(valuation);
            base.Browser = new TestBrowser(TestUsers.NewBusinessProcessor, TestUsers.Password);
            Helper.RequestLightstoneValuation(base.Browser, offerkey);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(@"A LightStone valuation for this property exists less than 2 months old.");
            Service<IValuationService>().DeleteValuationRecord(valuation);
        }

        /// <summary>
        /// Verify that a New Business Processor can perform the 'Request Lightstone Valuation' action at 'Manage Application' state.  Ensure
        /// the valuation resquest is processed where the latest valuation for the Offer Property is greater than 2 months old
        /// <list type="table">
        /// <listheader>
        /// <workflow>Workflow</workflow>
        /// </listheader>
        /// <item>
        /// <workflow>Application Management</workflow>
        /// </item>
        /// </list>
        /// </summary>
        [Test, Description(@"Verify that a New Business Processor can perform the 'Request Lightstone Valuation' action at 'Manage Application' state.  Ensure
			the valuation resquest is processed where the latest valuation for the Offer Property is greater than 2 months old")]
        public void _048_RequestLightstoneValuation_GreaterThan2MonthsOld()
        {
            var randomOffer
                = Service<ICommonService>().GetRandomOfferRow(OfferTypeEnum.NewPurchase, Workflows.ApplicationManagement, WorkflowStates.ApplicationManagementWF.ManageApplication);
            var offerkey = 0;
            if (randomOffer != null)
            {
                offerkey = randomOffer.Column(0).GetValueAs<int>();
            }
            else
            {
                CreateCaseAtStageIfOfferkeyEmpty(ref offerkey, WorkflowStates.ApplicationManagementWF.ManageApplication, OfferTypeEnum.NewPurchase,
                    LegalEntityType.NaturalPerson, Products.NewVariableLoan, 1000000, 0, 0, 500000,  50000, EmploymentType.Salaried, false, false);
            }
            var property = Service<IPropertyService>().GetPropertyByOfferKey(offerkey);

            var valuation = new Automation.DataModels.Valuation();
            valuation.ValuationKey = 9999999;
            valuation.ValuationDate = DateTime.Now.Subtract(TimeSpan.FromDays(90));
            valuation.ValuatorKey = 50;
            valuation.ValuationAmount = 1070000;
            valuation.ValuationHOCValue = 1070000;
            valuation.ValuationClassificationKey = null;
            valuation.ValuationMunicipal = 0;
            valuation.ChangeDate = DateTime.Now;
            valuation.ValuationUserID = @"SAHL\HaloUser";
            valuation.PropertyKey = property.Rows(0).Column("propertykey").GetValueAs<int>();
            valuation.ValuationStatusKey = Common.Enums.ValuationStatusEnum.Complete;
            valuation.ValuationDataProviderDataServiceKey = 3;
            valuation.IsActive = false;
            valuation.HOCRoofKey = HOCRoofEnum.Conventional;

            Service<IValuationService>().InsertValuation(valuation);
            base.Browser = new TestBrowser(TestUsers.NewBusinessProcessor, TestUsers.Password);
            Helper.RequestLightstoneValuation(base.Browser, offerkey);

            base.Browser.Page<BasePageAssertions>().AssertValidationMessageDoesNotExist(@"The LightStone web service call failed. Unable to retrieve Valuation data.");

            PropertyValuationAssertions.AssertLatestLightstoneValuationRecord(offerkey);
            Service<IValuationService>().DeleteValuationRecord(valuation);
        }

        /// <summary>
        /// Check that when the valuation age for a property valuation is less than 12 months old, that the valuation amount from the valuation record found
        /// is used in the LTV calculations and is displayed on all screens that display the valuation amount.  Ensure that a Valuation Clone is NOT created on
        /// entering  Application Management map
        /// </summary>
        [Test, Description(@"Check that when the valuation age for a property valuation is less than 12 months old, that the valuation amount from the valuation record found
		is used in the LTV calculations and is displayed on all screens that display the valuation amount.  Ensure that a Valuation Clone is NOT created on
		entering  Application Management map")]
        public void _049_ValuationLessThan12MonthsOld_SwitchAndRefinance()
        {
            var testIdentifier = "ValuationLessThan12MonthsOld_SwitchAndRefinance";
            int offerKey = Service<ICommonService>().GetOfferKeyByTestIdentifier(testIdentifier);
            var notAtValuationApprovalStage = false;
            QueryResults results = Service<IX2WorkflowService>().GetCreditInstanceDetails(offerKey);
            if (!results.HasResults)
            {
                notAtValuationApprovalStage = true;
            }
            else
            {
                var appManState = results.FirstOrDefault().Columns.SingleOrDefault(x => x.Name == "StateName").Value;
                if (appManState != WorkflowStates.CreditWF.ValuationApprovalRequired)
                    notAtValuationApprovalStage = true;
            }

            if (notAtValuationApprovalStage)
                offerKey = ApplicationInOrder(testIdentifier);

            string propertyValuationAmount = Service<ICommonService>().OffersAtApplicationCaptureRow(testIdentifier,
                "TestIdentifier").Rows(0).Column("MarketValue").Value;
            int propertyKey = Service<IPropertyService>().GetAddressDetailsForPropertyValuationLessThan12MonthsOld(
                Convert.ToInt32(propertyValuationAmount)).Rows(0).Column("PropertyKey").GetValueAs<int>();
            Service<IPropertyService>().UpdateOfferMortgageLoanPropertyKey(propertyKey, offerKey);
            base.Browser = new TestBrowser(TestUsers.CreditUnderwriter, TestUsers.Password);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().WorkFlows(base.Browser);
            base.Browser.Page<WorkflowSuperSearch>().Search(offerKey);
            base.Browser.ClickAction(WorkflowActivities.Credit.ValuationApproved);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, true);
            base.Browser.Dispose();
            base.Browser = null;
        }

        /// <summary>
        /// Check that if an active valuation record is found for a property where the valuation age for that valuation is exactly 12 months old,
        /// that the client’s estimate/purchase price, is used in the LTV calculations and is displayed on all screens that display the valuation amount.
        /// Ensure that a Valuation Clone IS created on entering  Application Management map
        /// </summary>
        [Test, Description(@"Check that if an active valuation record is found for a property where the valuation age for that valuation is exactly 12 months old,
		that the client’s estimate/purchase price, is used in the LTV calculations and is displayed on all screens that display the valuation amount.
		Ensure that a Valuation Clone IS created on entering  Application Management map")]
        public void _051_Valuation12MonthsOld_SwitchAndRefinance()
        {
            const string testIdentifier = "Valuation12MonthsOld_SwitchAndRefinance";
            int offerKey = Service<ICommonService>().GetOfferKeyByTestIdentifier(testIdentifier);

            QueryResults results = Service<IX2WorkflowService>().GetCreditInstanceDetails(offerKey);
            string appManState = results.FirstOrDefault().Columns.SingleOrDefault(x => x.Name == "StateName").Value;
            if (appManState != WorkflowStates.CreditWF.ValuationApprovalRequired)
            {
                offerKey = ApplicationInOrder(testIdentifier);
            }

            string propertyValuationAmount = Service<ICommonService>().OffersAtApplicationCaptureRow(testIdentifier,
                "TestIdentifier").Rows(0).Column("MarketValue").Value;
            string propertyKey = Service<IPropertyService>().GetAddressDetailsForPropertyValuation12MonthsOld(
                Convert.ToInt32(propertyValuationAmount)).Rows(0).Column("PropertyKey").Value;
            Service<IPropertyService>().UpdateOfferMortgageLoanPropertyKey(Convert.ToInt32(propertyKey), Convert.ToInt32(offerKey));
            base.Browser = new TestBrowser(TestUsers.CreditUnderwriter, TestUsers.Password);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().WorkFlows(base.Browser);
            base.Browser.Page<WorkflowSuperSearch>().Search(offerKey);
            base.Browser.ClickAction(WorkflowActivities.Credit.ValuationApproved);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, true);
            base.Browser.Dispose();
            base.Browser = null;
        }

        /// <summary>
        /// Check that if an active valuation record is found for a property where the valuation age for that valuation is greater than 12 months old,
        /// that the client’s estimate/purchase price, is used in the LTV calculations and is displayed on all screens that display the valuation amount.
        /// Ensure that a Valuation Clone IS created on entering  Application Management map
        /// </summary>
        [Test, Description(@"Check that if an active valuation record is found for a property where the valuation age for that valuation is greater than 12 months old,
		that the client’s estimate/purchase price, is used in the LTV calculations and is displayed on all screens that display the valuation amount.
		Ensure that a Valuation Clone IS created on entering Application Management map")]
        public void _053_ValuationGreaterThan12MonthsOld_SwitchAndRefinance()
        {
            const string testIdentifier = "ValuationGreaterThan12MonthsOld_SwitchAndRefinance";
            int offerKey = Service<ICommonService>().GetOfferKeyByTestIdentifier(testIdentifier);

            QueryResults results = Service<IX2WorkflowService>().GetCreditInstanceDetails(offerKey);
            string appManState = results.FirstOrDefault().Columns.SingleOrDefault(x => x.Name == "StateName").Value;
            if (appManState != WorkflowStates.CreditWF.ValuationApprovalRequired)
            {
                offerKey = ApplicationInOrder(testIdentifier);
            }

            string propertyValuationAmount = Service<ICommonService>().OffersAtApplicationCaptureRow(testIdentifier,
                "TestIdentifier").Rows(0).Column("MarketValue").Value;
            string propertyKey = Service<IPropertyService>().GetAddressDetailsForPropertyValuationGreaterThan12MonthsOld(
                Convert.ToInt32(propertyValuationAmount)).Rows(0).Column("PropertyKey").Value;

            Service<IPropertyService>().UpdateOfferMortgageLoanPropertyKey(Convert.ToInt32(propertyKey), Convert.ToInt32(offerKey));

            base.Browser = new TestBrowser(TestUsers.CreditUnderwriter, TestUsers.Password);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(base.Browser);

            base.Browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().WorkFlows(base.Browser);
            base.Browser.Page<WorkflowSuperSearch>().Search(offerKey);

            base.Browser.ClickAction(WorkflowActivities.Credit.ValuationApproved);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, true);

            base.Browser.Dispose();
            base.Browser = null;
        }

        #endregion Valuation 12 Months Old

        /// <summary>
        /// Process Applications at QA, Issue AIP and Application Management state so that they can be moved to the Credit state or Valuation Approval Required state in the Credit workflow map.
        /// Complete the Application in Order action to move the application to the Credit or Valuation Approval Required state in the Credit Workflow
        /// </summary>
        [Test, TestCaseSource(typeof(_02PreCredit), "GetTestCasesForApplicationInOrder"), Description(@"Process Applications at QA, Issue AIP and Application Management state so that they can be moved to the Credit state
		or Valuation Approval Required state in the Credit workflow map. Complete the Application in Order action to move the application to the Credit
		or Valuation Approval Required state in the Credit Workflow")]
        public void _050_ApplicationInOrderCheck(Automation.DataModels.OriginationTestCase testCase)
        {
            int offerKey = Service<ICommonService>().GetOfferKeyFromTestSchemaTable("OffersAtApplicationCapture", "TestIdentifier", testCase.TestIdentifier);
            Logger.LogAction("Getting the Instance details for Offer {0} in the Application Management workflow", offerKey);
            QueryResults results = Service<IX2WorkflowService>().GetAppManInstanceDetails(offerKey);
            //Get the Application Management state
            string appManState = string.Empty;
            if (results.HasResults)
            {
                appManState = results.Rows(0).Column("StateName").Value;
                Logger.LogAction("Offer is at the {0} state of the {1} workflow", appManState, Workflows.ApplicationManagement);
            }
            else
            {
                Logger.LogAction("Offer is not in the {0} workflow", Workflows.ApplicationManagement);
            }
            results.Dispose();

            //Process the application if it is at the QA state
            if (appManState == WorkflowStates.ApplicationManagementWF.QA)
            {
                Logger.LogAction("Processing Offer at {0} state of the {1} workflow", WorkflowStates.ApplicationManagementWF.QA, Workflows.ApplicationManagement);
                Helper.QAComplete(base.Browser, offerKey);
            }

            //Process the application if it is at the Issue AIP state or was processed at the QA state
            if (appManState == WorkflowStates.ApplicationManagementWF.IssueAIP || appManState == WorkflowStates.ApplicationManagementWF.QA)
            {
                Logger.LogAction("Processing Offer at {0} state of the {1} workflow", WorkflowStates.ApplicationManagementWF.IssueAIP,
                    Workflows.ApplicationManagement);
                Helper.AcceptAIP(base.Browser, testCase.Username, offerKey);
            }

            //Get the Valuation state
            results = Service<IX2WorkflowService>().GetValuationsInstanceDetails(offerKey);
            string valuationState = string.Empty;
            if (results.HasResults)
            {
                valuationState = results.Rows(0).Column("StateName").Value;
                Logger.LogAction("Offer is at the {0} state of the {1} workflow", valuationState, Workflows.Valuations);
            }
            else
            {
                Logger.LogAction("Offer is not in the {0} workflow", Workflows.Valuations);
            }

            //Process the application if it is at the Schedule Valuation Assessment state
            if (valuationState == WorkflowStates.ValuationsWF.ScheduleValuationAssessment)
            {
                Helper.PerformManualValuation(base.Browser, offerKey, testCase.MarketValue);
            }

            //Process the application if it is at the Manage Application state or was processed at the QA or Issue AIP state
            if (appManState == WorkflowStates.ApplicationManagementWF.ManageApplication || appManState == WorkflowStates.ApplicationManagementWF.IssueAIP ||
                appManState == WorkflowStates.ApplicationManagementWF.QA)
            {
                Logger.LogAction("Processing Offer at {0} state of the {1} workflow", WorkflowStates.ApplicationManagementWF.ManageApplication, Workflows.ApplicationManagement);

                base.Browser = new TestBrowser(TestUsers.NewBusinessProcessor, TestUsers.Password);
                base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, Common.Constants.WorkflowStates.ApplicationManagementWF.ManageApplication);
                //Check that the warning message "This property is connected to an open loan and cannot be updated, please use the capture property functionality or alternately contact client services to update." does not exist before trying to update Deeds Office details
                base.Browser.Navigate<Navigation.PropertyAddressNode>().PropertyAddress(offerKey);
                base.Browser.Navigate<Navigation.PropertyAddressNode>().UpdateDeedsOfficeDetails(NodeTypeEnum.Update);
                if (!base.Browser.Page<BasePage>().CheckForerrorMessages(
                    "This property is connected to an open loan and cannot be updated, please use the capture property functionality or alternately contact client services to update."))
                {
                    Helper.UpdatePropertyAddress(base.Browser, offerKey);
                }

                //if (loanType == "New purchase")
                //{
                Helper.UpdateHOCDetails(base.Browser);
                //}

                base.Browser.Navigate<LegalEntityNode>().LegalEntity(offerKey);
                Helper.AddConfirmedIncome(base.Browser, testCase.HouseHoldIncome);
                Helper.AddAssetsAndLiabilities(base.Browser);

                base.Browser.Navigate<LoanDetailsNode>().ClickLoanDetailsNode();
                Helper.UpdateApplicationAttributes(base.Browser);
                Helper.SaveConditions(base.Browser);
                Helper.CompleteDocumentChecklist(base.Browser);

                base.Browser.ClickWorkflowLoanNode(Workflows.ApplicationManagement);
                base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.ApplicationinOrder);
                base.Browser.Page<WorkflowYesNo>().Confirm(true, true);
            }
        }

        /// <summary>
        /// This test ensures that the Branch Consultant User can pickup a case at the Request at QA state and perform the Branch Rework Application action.
        /// </summary>
        [Test, Description("This test ensures that the Branch Consultant User can pickup a case at the Request at QA state and perform the Branch Rework Application action.")]
        public void _057_BranchReworkAtQAQuery()
        {
            int offerKey = Service<IX2WorkflowService>().GetOfferKeyAtStateByType(WorkflowStates.ApplicationManagementWF.RequestatQA, Workflows.ApplicationManagement, OfferTypeEnum.NewPurchase, "");
            CreateCaseAtStageIfOfferkeyEmpty(ref offerKey, WorkflowStates.ApplicationManagementWF.RequestatQA, OfferTypeEnum.NewPurchase,
                LegalEntityType.NaturalPerson, Products.NewVariableLoan, 1000000, 0, 0, 500000,   50000, EmploymentType.Salaried, false, false);
            //login as branch consultant
            base.Browser = new TestBrowser(TestUsers.BranchConsultant, TestUsers.Password);
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.ApplicationManagementWF.RequestatQA);
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.BranchReworkApplication);
            base.Browser.Page<ApplicationLoanDetailsUpdate>().ChangeTerm("220");
            base.Browser.Page<ApplicationLoanDetailsUpdate>().RecalcAndSave(false);
            StageTransitionAssertions.AssertStageTransitionCreated(offerKey, StageDefinitionStageDefinitionGroupEnum.ApplicationManagement_ReworkApplication);
        }

        /// <summary>
        /// This test ensures that a Branch Manager can search for a case at the QA Query stage and then reassign the commission earning consultant as well as
        /// the branch consultant on the deal.
        /// </summary>
        [Test, Description(@"This test ensures that a Branch Manager can search for a case at the QA Query stage and then reassign the commission earning consultant as well as
		the branch consultant on the deal.")]
        public void _058_ReassignCommissionEarningConsultantAtQAQuery()
        {
            //we need a new application
            int offerKey = 0;
            CreateCaseAtStageIfOfferkeyEmpty(ref offerKey, WorkflowStates.ApplicationManagementWF.RequestatQA, OfferTypeEnum.NewPurchase,
                LegalEntityType.NaturalPerson, Products.NewVariableLoan, 1000000, 0, 0, 500000, 50000, EmploymentType.Salaried, false, false);
            //login as Branch Manager
            base.Browser = new TestBrowser(TestUsers.BranchManager, TestUsers.Password);
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.ApplicationManagementWF.RequestatQA);
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.ReAssignCommissionConsultant);
            base.Browser.Page<ReassignOriginatingBranchConsultant>().ReassignCommissionConsultant(TestUsers.BranchConsultant1, true);
            AssignmentAssertions.AssertOfferRoleRecordExists(offerKey, OfferRoleTypeEnum.CommissionableConsultant);
            AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(offerKey, OfferRoleTypeEnum.CommissionableConsultant);
            AssignmentAssertions.AssertWhoTheOfferRoleRecordIsAssignedTo(offerKey, OfferRoleTypeEnum.CommissionableConsultant, TestUsers.BranchConsultant1);
            //we need to check the 101 OfferRole
            AssignmentAssertions.AssertOfferRoleRecordExists(offerKey, OfferRoleTypeEnum.BranchConsultantD);
            AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(offerKey, OfferRoleTypeEnum.BranchConsultantD);
            AssignmentAssertions.AssertWhoTheOfferRoleRecordIsAssignedTo(offerKey, OfferRoleTypeEnum.BranchConsultantD, TestUsers.BranchConsultant1);
            //we need to check the 101 WFA record
            AssignmentAssertions.AssertThatAWorkFlowAssignmentRecordExists(offerKey, OfferRoleTypeEnum.BranchConsultantD);
            AssignmentAssertions.AssertThatTheWorkFlowAssignmentRecordIsActive(offerKey, OfferRoleTypeEnum.BranchConsultantD);
            AssignmentAssertions.AssertWhoTheWorkFlowAssignmentRecordIsAssignedTo(offerKey, OfferRoleTypeEnum.BranchConsultantD, TestUsers.BranchConsultant1);
        }

        /// <summary>
        /// A branch consultant user should be able to Motivate an application back to Credit by performing the Motivate action at the Declined by Credit
        /// state in the Application Management workflow. This will send the case back to the Credit state.
        /// </summary>
        [Test, Description(@"A branch consultant user should be able to Motivate an application back to Credit by performing the Motivate action at the Declined by Credit
		state in the Application Management workflow. This will send the case back to the Credit state.")]
        public void _059_MotivateFromDeclinedByCredit()
        {
            int offerKey = Service<IX2WorkflowService>().GetOfferKeyAtStateByType(WorkflowStates.ApplicationManagementWF.DeclinedbyCredit, Workflows.ApplicationManagement,
                OfferTypeEnum.NewPurchase, "");
            if (offerKey == 0)
            {
                offerKey = Service<IX2WorkflowService>().GetOfferKeyAtStateByType(WorkflowStates.CreditWF.Credit, Workflows.Credit, OfferTypeEnum.NewPurchase, "");

                if (offerKey == 0)
                    throw new WatiN.Core.Exceptions.WatiNException(string.Format(@"No offers could be found at the {0} or {1} states.  I'm giving up!!", WorkflowStates.ApplicationManagementWF.DeclinedbyCredit, WorkflowStates.CreditWF.Credit));

                base.scriptEngine.ExecuteScript(WorkflowEnum.Credit, WorkflowAutomationScripts.Credit.ConfirmApplicationEmployment, offerKey);
                Int64 creditInstanceID = Service<IX2WorkflowService>().GetCreditInstanceIDByState(WorkflowStates.CreditWF.Credit, offerKey, false);
                Int64 appmanInstanceID = Service<IX2WorkflowService>().GetAppManInstanceIDByState(WorkflowStates.ApplicationManagementWF.CreditHold, offerKey, false);
                base.Browser = new TestBrowser(TestUsers.CreditSupervisor, TestUsers.Password);
                base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.CreditWF.Credit);
                base.Browser.ClickAction(WorkflowActivities.Credit.DeclineApplication);
                base.Browser.Page<CommonReasonCommonDecline>().SelectReasonAndSubmit(ReasonType.CreditDecline);
                Service<IX2WorkflowService>().WaitForX2WorkflowHistoryActivity(ConditionalActivities.Credit.DeclinedbyCredit, creditInstanceID, 1);
                //case should be at Declined by Credit
                Service<IX2WorkflowService>().WaitForX2WorkflowHistoryActivity(ConditionalActivities.ApplicationManagement.CreditDeclineApplication, appmanInstanceID, 1);
                X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.DeclinedbyCredit);
                base.Browser.Dispose();
            }
            //login as consultant
            Int64 instanceID = Service<IX2WorkflowService>().GetAppManInstanceIDByState(WorkflowStates.ApplicationManagementWF.DeclinedbyCredit, offerKey, false);
            base.Browser = new TestBrowser(TestUsers.BranchConsultant, TestUsers.Password);
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.ApplicationManagementWF.DeclinedbyCredit);
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.Motivate);
            base.Browser.Page<GenericMemoAdd>().AddMemoRecord(MemoStatus.UnResolved, "Motivated");
            base.Browser.Page<BasePage>().DomainWarningClickYes();
            //case should be sent back to credit
            Service<IX2WorkflowService>().WaitForX2WorkflowHistoryActivity(ConditionalActivities.ApplicationManagement.SystemBackToCredit, instanceID, 1);
            //we need the instanceid
            instanceID = X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.BackToCreditHold);
            Service<IX2WorkflowService>().WaitForCreditCaseCreate(instanceID, offerKey, WorkflowStates.CreditWF.Credit);
            X2Assertions.AssertCurrentCreditX2State(offerKey, WorkflowStates.CreditWF.Credit);
        }

        /// <summary>
        /// This test will perform the Decline Final Action on a case at the Declined by Credit state in the Application Management workflow. This test
        /// requires that the user performing the Decline Final action is in the same branch as the application itself.
        /// </summary>
        [Test, Description(@"This test will perform the Decline Final Action on a case at the Declined by Credit state in the Application Management workflow. This test
		requires that the user performing the Decline Final action is in the same branch as the application itself.")]
        public void _060_DeclineFinal()
        {
            int offerKey = 0;
            var results = Service<IX2WorkflowService>().GetX2DataByWorkflowAndState(WorkflowStates.CreditWF.Credit, Workflows.Credit);
            foreach (QueryResultsRow row in results.RowList)
            {
                bool isUserInSameBranch = base.Service<IADUserService>().IsUserInSameBranchAsApp(TestUsers.BranchConsultant,
                    row.Column("OfferKey").GetValueAs<string>());
                bool isAutomationTestCase = Service<ICommonService>().IsAutomationTestCase(offerKey);
                if (isUserInSameBranch && !isAutomationTestCase)
                {
                    offerKey = row.Column("OfferKey").GetValueAs<int>();
                    break;
                }
            }
            if (offerKey != 0)
            {
                //get the instanceid
                base.scriptEngine.ExecuteScript(WorkflowEnum.Credit, WorkflowAutomationScripts.Credit.ConfirmApplicationEmployment, offerKey);
                Int64 instanceid = Service<IX2WorkflowService>().GetAppManInstanceIDByState(WorkflowStates.ApplicationManagementWF.CreditHold, offerKey);
                if (instanceid == -1)
                {
                    //look in back to credit hold
                    instanceid = Service<IX2WorkflowService>().GetAppManInstanceIDByState(WorkflowStates.ApplicationManagementWF.BackToCreditHold, offerKey);
                }
                base.Browser = new TestBrowser(TestUsers.CreditSupervisor, TestUsers.Password);
                base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.CreditWF.Credit);
                base.Browser.ClickAction(WorkflowActivities.Credit.DeclineApplication);
                base.Browser.Page<CommonReasonCommonDecline>().SelectReasonAndSubmit(ReasonType.CreditDecline);
                //wait for the return
                Service<IX2WorkflowService>().WaitForX2State(offerKey, Workflows.ApplicationManagement, WorkflowStates.ApplicationManagementWF.DeclinedbyCredit);
                //case should be at Declined by Credit
                X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.DeclinedbyCredit);
                string branchConsultant = base.Service<IApplicationService>().GetADUserByActiveOfferRoles(offerKey, OfferRoleTypeEnum.BranchConsultantD);
                AssignmentAssertions.AssertWorkflowAssignment(branchConsultant, offerKey, OfferRoleTypeEnum.BranchConsultantD);
                base.Browser.Dispose();
                //login as consultant
                base.Browser = new TestBrowser(TestUsers.BranchConsultant, TestUsers.Password);
                base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.ApplicationManagementWF.DeclinedbyCredit);
                base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.DeclineFinal);
                base.Browser.Page<WorkflowYesNo>().Confirm(true, false);
                X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.DeclineBin);
                OfferAssertions.AssertOfferEndDate(offerKey, DateTime.Now, 0, false);
                OfferAssertions.AssertOfferStatus(offerKey, OfferStatusEnum.Declined);
            }
            else
            {
                Assert.Fail("No test case found in Credit originated by a Test User. Decline Final requires the application to be in the same branch.");
            }
        }

        /// <summary>
        /// This test will assign an admin to a case at the Application Query state. It searches for any application at that state and then picks an admin user
        /// from the same branch as the application to assign to the case.
        /// </summary>
        [Test, Description(@"This test will assign an admin to a case at the Application Query state. It searches for any application at that state and then picks an admin user
		from the same branch as the application to assign to the case.")]
        public void _061_AssignAdminAtApplicationQuery()
        {
            var offerKeys = Service<IX2WorkflowService>().GetOffersAtState(WorkflowStates.ApplicationManagementWF.ApplicationQuery, Workflows.ApplicationManagement, "");
            int offerKey = 0;
            if (offerKeys.Count > 0)
            {
                offerKey = offerKeys[0];
            }
            else
            {
                CreateCaseAtStageIfOfferkeyEmpty(ref offerKey, WorkflowStates.ApplicationManagementWF.ApplicationQuery, OfferTypeEnum.NewPurchase,
                    LegalEntityType.NaturalPerson, Products.NewVariableLoan, 1000000, 0,  500000, 0, 50000, EmploymentType.Salaried, false, false);
            }
            //login as consultant
            base.Browser = new TestBrowser(TestUsers.BranchConsultant, TestUsers.Password);
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.ApplicationManagementWF.ApplicationQuery);
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.AssignAdmin);
            var users = base.Service<IADUserService>().GetBranchUsersForApplication(offerKey, OfferRoleTypeEnum.BranchAdminD);
            var adUserName = users.Where(x => x.Column("ADUserStatus").GetValueAs<int>() == 1).Select(x=>x.Column("aduserName").GetValueAs<string>()).FirstOrDefault();
            base.Browser.Page<App_AssignAdmin>().SelectAdminFromDropdownAndCommit(adUserName);
            AssignmentAssertions.AssertThatAWorkFlowAssignmentRecordExists(offerKey, OfferRoleTypeEnum.BranchAdminD);
            AssignmentAssertions.AssertThatTheWorkFlowAssignmentRecordIsActive(offerKey, OfferRoleTypeEnum.BranchAdminD);
            AssignmentAssertions.AssertWhoTheWorkFlowAssignmentRecordIsAssignedTo(offerKey, OfferRoleTypeEnum.BranchAdminD, adUserName);
        }

        /// <summary>
        /// #16957 - Add a property with an existing valuation, older than 12 months to a New Purchase application.
        /// Valuation request is Archived by Valuations Manager.
        /// Attempt to perform Application in Order action
        /// Check that a warning message "Active valuation not conducted within the last 12 months." is displayed
        /// </summary>
        [Test, Description(@"#16957")]
        public void _062_when_performing_application_in_order_action_and_valuation_older_than_12_months()
        {
            const string testIdentifier = "ValuationGreaterThan12MonthsOld_NewPurchase";
            QueryResults results = Service<ICommonService>().OffersAtApplicationCaptureRow(testIdentifier, "TestIdentifier");

            string branchConsultant = results.Rows(0).Column("Username").Value;
            int offerKey = results.Rows(0).Column("OfferKey").GetValueAs<int>();
            string loanType = results.Rows(0).Column("LoanType").Value;
            string legalEntityType = results.Rows(0).Column("LegalEntityType").Value;
            string product = results.Rows(0).Column("Product").Value;
            int marketValue = results.Rows(0).Column("MarketValue").GetValueAs<int>();
            int existingLoan = results.Rows(0).Column("ExistingLoan").GetValueAs<int>();
            int cashOut = results.Rows(0).Column("CashOut").GetValueAs<int>();
            int cashDeposit = results.Rows(0).Column("CashDeposit").GetValueAs<int>();
            int confirmedIncome = results.Rows(0).Column("HouseHoldIncome").GetValueAs<int>();
            string employmentType = results.Rows(0).Column("EmploymentType").Value;

            OfferTypeEnum offerType = new OfferTypeEnum();
            switch (loanType)
            {
                case "Switch loan":
                    offerType = OfferTypeEnum.SwitchLoan;
                    break;

                case "New purchase":
                    offerType = OfferTypeEnum.NewPurchase;
                    break;

                case "Refinance":
                    offerType = OfferTypeEnum.Refinance;
                    break;

                default:
                    break;
            }

            CreateCaseAtStageIfOfferNotAtState(ref offerKey, WorkflowStates.ApplicationManagementWF.QA, offerType, legalEntityType, product, marketValue, existingLoan, cashOut, cashDeposit,
                confirmedIncome, employmentType, false, false);

            Service<ICommonService>().CommitOfferKeyForTestIdentifier("ApplicationManagementTestID", testIdentifier, offerKey);

            var propertyValuationAmount = Service<ICommonService>().OffersAtApplicationCaptureRow(testIdentifier, "TestIdentifier").Rows(0).Column("MarketValue").GetValueAs<int>();
            string propertyKey = Service<IPropertyService>().GetAddressDetailsForPropertyValuationGreaterThan12MonthsOld(propertyValuationAmount).Rows(0).Column("PropertyKey").Value;

            Service<IPropertyService>().UpdateOfferMortgageLoanPropertyKey(Convert.ToInt32(propertyKey), Convert.ToInt32(offerKey));

            Helper.QAComplete(base.Browser, offerKey);
            Helper.AcceptAIP(base.Browser, TestUsers.BranchConsultant, offerKey);

            Helper.EscalateToManager(base.Browser, offerKey);
            Helper.ManagerArchive(base.Browser, offerKey);

            base.Browser = new TestBrowser(TestUsers.NewBusinessProcessor, TestUsers.Password);
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.ApplicationManagementWF.ManageApplication);
            base.Browser.Navigate<Navigation.PropertyAddressNode>().PropertyAddress(offerKey);
            Helper.UpdateHOCDetails(base.Browser);
            base.Browser.Navigate<LoanDetailsNode>().ClickLoanDetailsNode();
            Helper.UpdateApplicationAttributes(base.Browser);
            Helper.SaveConditions(base.Browser);
            Helper.CompleteDocumentChecklist(base.Browser);
            base.Browser.ClickWorkflowLoanNode(Workflows.ApplicationManagement);
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.ApplicationinOrder);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, false);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Active valuation not conducted within the last 12 months.");
            base.Browser.Page<BasePageAssertions>().AssertValidationIsWarning();
            base.Browser.Page<BasePage>().DomainWarningClickYes();
            Service<IX2WorkflowService>().WaitForX2State(offerKey, Workflows.Credit, WorkflowStates.CreditWF.Credit);
            X2Assertions.AssertCurrentCreditX2State(offerKey, WorkflowStates.CreditWF.Credit);
        }

        /// <summary>
        /// #16957 - Add a property with an existing valuation, older than 12 months to a New Purchase application.
        /// Valuation request is Archived by Valuations Manager.
        /// Attempt to perform Override Check action
        /// Check that there are no validations and that the application goes through to credit automatically
        /// </summary>
        [Test, Description(@"#16957")]
        public void _063_when_performing_override_check_action_and_valuation_older_than_12_months()
        {
            const string testIdentifier = "ValuationGreaterThan12MonthsOld_NewPurchase_Jumbo";
            QueryResults results = Service<ICommonService>().OffersAtApplicationCaptureRow(testIdentifier, "TestIdentifier");

            string branchConsultant = results.Rows(0).Column("Username").Value;
            int offerKey = results.Rows(0).Column("OfferKey").GetValueAs<int>();
            string loanType = results.Rows(0).Column("LoanType").Value;
            string legalEntityType = results.Rows(0).Column("LegalEntityType").Value;
            string product = results.Rows(0).Column("Product").Value;
            int marketValue = results.Rows(0).Column("MarketValue").GetValueAs<int>();
            int existingLoan = results.Rows(0).Column("ExistingLoan").GetValueAs<int>();
            int cashOut = results.Rows(0).Column("CashOut").GetValueAs<int>();
            int cashDeposit = results.Rows(0).Column("CashDeposit").GetValueAs<int>();
            int confirmedIncome = results.Rows(0).Column("HouseHoldIncome").GetValueAs<int>();
            string employmentType = results.Rows(0).Column("EmploymentType").Value;

            OfferTypeEnum offerType = new OfferTypeEnum();
            switch (loanType)
            {
                case "Switch loan":
                    offerType = OfferTypeEnum.SwitchLoan;
                    break;

                case "New purchase":
                    offerType = OfferTypeEnum.NewPurchase;
                    break;

                case "Refinance":
                    offerType = OfferTypeEnum.Refinance;
                    break;

                default:
                    break;
            }

            CreateCaseAtStageIfOfferNotAtState(ref offerKey, WorkflowStates.ApplicationManagementWF.QA, offerType,
                legalEntityType, product, marketValue, existingLoan, cashOut,
                cashDeposit, confirmedIncome, employmentType, false, false);

            Service<ICommonService>().CommitOfferKeyForTestIdentifier("TestIdentifier", testIdentifier, offerKey);

            string propertyValuationAmount = Service<ICommonService>().OffersAtApplicationCaptureRow(testIdentifier,
                "TestIdentifier").Rows(0).Column("MarketValue").Value;
            string propertyKey = Service<IPropertyService>().GetAddressDetailsForPropertyValuationGreaterThan12MonthsOld(
                Convert.ToInt32(propertyValuationAmount)).Rows(0).Column("PropertyKey").Value;

            Service<IPropertyService>().UpdateOfferMortgageLoanPropertyKey(Convert.ToInt32(propertyKey), Convert.ToInt32(offerKey));

            Helper.QAComplete(base.Browser, offerKey);
            Helper.AcceptAIP(base.Browser, TestUsers.BranchConsultant, offerKey);

            Helper.EscalateToManager(base.Browser, offerKey);
            Helper.ManagerArchive(base.Browser, offerKey);

            base.Browser = new TestBrowser(TestUsers.NewBusinessProcessor);
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.ApplicationManagementWF.ManageApplication);
            base.Browser.Navigate<Navigation.PropertyAddressNode>().PropertyAddress(offerKey);
            Helper.UpdateHOCDetails(base.Browser);
            base.Browser.Navigate<LoanDetailsNode>().ClickLoanDetailsNode();
            Helper.UpdateApplicationAttributes(base.Browser);
            Helper.SaveConditions(base.Browser);
            Helper.CompleteDocumentChecklist(base.Browser);

            var instanceID = X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.ManageApplication);
            Service<IX2WorkflowService>().SetIsValuationRequiredIndicator(true, instanceID);

            base.Browser.Dispose();
            base.Browser = null;
            base.Browser = new TestBrowser(TestUsers.NewBusinessManager);
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.ApplicationManagementWF.ManageApplication);
            base.Browser.ClickWorkflowLoanNode(Workflows.ApplicationManagement);
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.OverrideCheck);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, false);
            Service<IX2WorkflowService>().WaitForX2State(offerKey, Workflows.Credit, WorkflowStates.CreditWF.Credit);
            X2Assertions.AssertCurrentCreditX2State(offerKey, WorkflowStates.CreditWF.Credit);
        }

        public void _063_when_capitalising_initiation_fees_on_an_alpha_housing_application_should_add_loan_condition_245() 
        {
            int offerKey = base.Service<IApplicationService>().GetAlphaOffersAtAppMan().FirstOrDefault().OfferKey;
            Service<IApplicationService>().InsertOfferAttribute(offerKey, OfferAttributeTypeEnum.CapitaliseInitiationFee);
            string[] expectedConditions = new string[] { "245" };
            base.Browser = new TestBrowser(TestUsers.NewBusinessProcessor, TestUsers.Password);
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.ApplicationManagementWF.ManageApplication);
            Helper.SaveConditions(base.Browser);

            OfferAssertions.AssertOfferConditionsExist(offerKey, expectedConditions);
        }

        #endregion ApplicationManagementWorkflowTests

        #region HelperMethods

        private void MoveCaseFromQAtoManageApplication(int offerKey)
        {
            Logger.LogAction("Getting the Instance details for Offer {0} in the Application Management workflow", offerKey);
            QueryResults results = Service<IX2WorkflowService>().GetAppManInstanceDetails(offerKey);

            //Get the Application Management state
            string appManState = string.Empty;
            if (results.HasResults)
            {
                appManState = results.Rows(0).Column("StateName").Value;
                Logger.LogAction("Offer is at the {0} state of the {1} workflow", appManState, Workflows.ApplicationManagement);
            }
            else
            {
                Logger.LogAction("Offer is not in the {0} workflow", Workflows.ApplicationManagement);
            }
            results.Dispose();

            //Process the application if it is at the Issue AIP state or was processed at the RequestatQA state
            if (appManState == WorkflowStates.ApplicationManagementWF.RequestatQA)
            {
                Logger.LogAction("Processing Offer at {0} state of the {1} workflow", WorkflowStates.ApplicationManagementWF.RequestatQA, Workflows.ApplicationManagement);

                base.scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.ApplicationManagement, "RequestResolved", offerKey);
            }

            //Process the application if it is at the QA state
            if (appManState == WorkflowStates.ApplicationManagementWF.QA || appManState == WorkflowStates.ApplicationManagementWF.RequestatQA)
            {
                Logger.LogAction("Processing Offer at {0} state of the {1} workflow", WorkflowStates.ApplicationManagementWF.QA, Workflows.ApplicationManagement);

                base.scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.ApplicationManagement, "QAComplete", offerKey);

                var instanceID = Service<IX2WorkflowService>().GetAppManInstanceIDByOfferKey(offerKey);
                Service<IX2WorkflowService>().WaitForX2WorkflowHistoryActivity(instanceID, 1, "", WorkflowActivities.ApplicationManagement.NewPurchase, WorkflowActivities.ApplicationManagement.OtherTypes);
            }
            results = Service<IX2WorkflowService>().GetAppManInstanceDetails(offerKey);
            appManState = results.Rows(0).Column("StateName").Value;

            //Process the application if it is at the Issue AIP state
            if (appManState == WorkflowStates.ApplicationManagementWF.IssueAIP)
            {
                Logger.LogAction("Processing Offer at {0} state of the {1} workflow", WorkflowStates.ApplicationManagementWF.IssueAIP, Workflows.ApplicationManagement);

                base.scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.ApplicationManagement, "ClientAccepts", offerKey);
            }
        }

        private void CreateCaseAtStageIfOfferkeyEmpty(ref int offerKey, string stateName, OfferTypeEnum offerType, string legalEntityType, string product,
            int marketValue, int existingLoan, int cashOut, int cashDeposit, int houseHoldIncome, string employmentType,
            bool interestOnly, bool capitaliseFees)
        {
            if (offerKey == 0 || offerKey == null)
            {
                base.Browser = new TestBrowser(TestUsers.BranchConsultant, TestUsers.Password);
                base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(base.Browser);
                base.Browser.Navigate<BuildingBlocks.Navigation.CalculatorsNode>().Calculators(base.Browser, CalculatorNodeTypeEnum.ApplicationCalculator);

                //Create Offer via Calculator
                switch (offerType)
                {
                    case OfferTypeEnum.NewPurchase:
                        base.Browser.Page<Views.LoanCalculator>().LoanCalculatorLead_NewPurchase(product.ToString(), marketValue.ToString(), cashDeposit.ToString(),
                            employmentType.ToString(), "240", houseHoldIncome.ToString(),
                            ButtonTypeEnum.CreateApplication);
                        break;

                    case OfferTypeEnum.SwitchLoan:
                        base.Browser.Page<Views.LoanCalculator>().LoanCalculatorLead_Switch(product.ToString(), marketValue.ToString(), existingLoan.ToString(),
                            cashOut.ToString(), employmentType.ToString(), "240", Convert.ToBoolean(capitaliseFees),
                            houseHoldIncome.ToString(), ButtonTypeEnum.CreateApplication);
                        break;

                    case OfferTypeEnum.Refinance:
                        base.Browser.Page<Views.LoanCalculator>().LoanCalculatorLead_Refinance(product.ToString(), marketValue.ToString(), cashOut.ToString(),
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
                    base.Browser.Page<LegalEntityDetailsLeadApplicantAdd>().AddLegalEntity(OfferRoleTypes.LeadMainApplicant, true, idNumber, "Mr", "auto", firstname, surname, "auto", Gender.Male,
                        MaritalStatus.Single, "Unknown", "Unknown", CitizenType.SACitizen, "auto", null, null, "Unknown", Language.English, null, "031", "1234567",
                        null, null, null, null, null, null, true, false, false, false, false, ButtonTypeEnum.Next);
                }
                else
                {
                    var companyName = "CompanyName" + rnum.ToString();
                    base.Browser.Page<LegalEntityDetails>().AddLegalEntityCompany(legalEntityType.ToString(), companyName, "031", "1234567");
                }

                offerKey = base.Browser.Page<ApplicationSummaryBase>().GetOfferKey();
                base.Browser.Dispose();
                base.Browser = null;

                //Push offer to correct State

                base.scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.ApplicationCapture, "SubmitApplication", offerKey);

                Service<IX2WorkflowService>().WaitForAppManCaseCreate(offerKey);

                BuildingBlocks.Timers.GeneralTimer.Wait(2000);

                switch (stateName.ToString())
                {
                    case WorkflowStates.ApplicationManagementWF.QA:
                        {
                            break;
                        }
                    case WorkflowStates.ApplicationManagementWF.RequestatQA:
                        {
                            base.scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.ApplicationManagement, "CreateAtRequestAtQA", offerKey);
                            break;
                        }
                    case WorkflowStates.ApplicationManagementWF.IssueAIP:
                        {
                            base.scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.ApplicationManagement, "QAComplete", offerKey);
                            break;
                        }
                    case WorkflowStates.ApplicationManagementWF.ManageApplication:
                        {
                            if (offerType == OfferTypeEnum.NewPurchase)
                            {
                                MoveCaseFromQAtoManageApplication(offerKey);
                            }
                            break;
                        }
                    case WorkflowStates.ApplicationManagementWF.ApplicationQuery:
                        {
                            if (offerType == OfferTypeEnum.NewPurchase)
                            {
                                MoveCaseFromQAtoManageApplication(offerKey);
                            }
                            base.scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.ApplicationManagement, "QueryOnApplication", offerKey);
                            break;
                        }
                    case WorkflowStates.ApplicationManagementWF.Decline:
                        {
                            base.scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.ApplicationManagement, "Decline", offerKey);
                            break;
                        }
                    case WorkflowStates.ApplicationManagementWF.NTU:
                        {
                            base.scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.ApplicationManagement, "NTU", offerKey);
                            break;
                        }
                    default:
                        break;
                }
            }
        }

        private void CreateCaseAtStageIfOfferNotAtState(ref int offerKey, string stateName, OfferTypeEnum offerType, string legalEntityType, string product,
            int marketValue, int existingLoan, int cashOut, int cashDeposit, int houseHoldIncome, string employmentType,
            bool interestOnly, bool capitaliseFees)
        {
            string appManState = string.Empty;
            if (offerKey > 0)
            {
                QueryResults results = Service<IX2WorkflowService>().GetAppManInstanceDetails(offerKey);

                if (results.HasResults)
                    appManState = results.Rows(0).Column("StateName").Value;

                results.Dispose();
            }
            if (appManState != stateName)
            {
                offerKey = 0;
            }
            CreateCaseAtStageIfOfferkeyEmpty(ref offerKey, stateName, offerType, legalEntityType, product,
                marketValue, existingLoan, cashOut, cashDeposit, houseHoldIncome, employmentType,
                interestOnly, capitaliseFees);
        }

        private int ApplicationInOrder(string testIdentifier)
        {
            //SubmitApplicationNewPurchaseEdgeApplication
            QueryResults results = Service<ICommonService>().OffersAtApplicationCaptureRow(testIdentifier, "ApplicationManagementTestID");
            string branchConsultant = results.Rows(0).Column("Username").Value;
            int offerKey = results.Rows(0).Column("OfferKey").GetValueAs<int>();
            string loanType = results.Rows(0).Column("LoanType").Value;
            string legalEntityType = results.Rows(0).Column("LegalEntityType").Value;
            string product = results.Rows(0).Column("Product").Value;
            int marketValue = results.Rows(0).Column("MarketValue").GetValueAs<int>();
            int existingLoan = results.Rows(0).Column("ExistingLoan").GetValueAs<int>();
            int cashOut = results.Rows(0).Column("CashOut").GetValueAs<int>();
            int cashDeposit = results.Rows(0).Column("CashDeposit").GetValueAs<int>();
            int confirmedIncome = results.Rows(0).Column("HouseHoldIncome").GetValueAs<int>();
            string employmentType = results.Rows(0).Column("EmploymentType").Value;

            var expectedState = WorkflowStates.ApplicationManagementWF.ManageApplication;
            OfferTypeEnum offerType = new OfferTypeEnum();
            switch (loanType)
            {
                case "Switch loan":
                    offerType = OfferTypeEnum.SwitchLoan;
                    break;

                case "New purchase":
                    offerType = OfferTypeEnum.NewPurchase;
                    break;

                case "Refinance":
                    offerType = OfferTypeEnum.Refinance;
                    break;

                default:
                    break;
            }

            CreateCaseAtStageIfOfferNotAtState(ref offerKey, expectedState, offerType,
                legalEntityType, product, marketValue, existingLoan, cashOut,
                cashDeposit,   confirmedIncome, employmentType, false, false);

            Service<ICommonService>().CommitOfferKeyForTestIdentifier("ApplicationManagementTestID", testIdentifier, offerKey);

            if (loanType == "New purchase")
            {
                Helper.PerformManualValuation(base.Browser, offerKey, marketValue.ToString());
            }

            base.Browser = new TestBrowser(TestUsers.NewBusinessProcessor);
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.ApplicationManagementWF.ManageApplication);

            base.Service<IPropertyService>().DBUpdateDeedsOfficeDetails(offerKey);

            //HOC Exist rules applies to SwitchLoan,NewPurchaseLoan, RefinanceLoan need to make sure that it gets updated.
            base.Browser.Navigate<Navigation.PropertyAddressNode>().PropertyAddress(offerKey);
            Helper.UpdateHOCDetails(base.Browser);

            base.Browser.Navigate<LoanDetailsNode>().ClickLoanDetailsNode();
            Service<IApplicationService>().InsertSettlementBanking(offerKey);
            Helper.UpdateApplicationAttributes(base.Browser);
            Helper.SaveConditions(base.Browser,expectedNoValidationMessages:true);
            Helper.CompleteDocumentChecklist(base.Browser);
            base.Browser.ClickWorkflowLoanNode(Workflows.ApplicationManagement);
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.ApplicationinOrder);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, false);
            base.Browser.Page<WorkflowYesNo>().ContinueWithWarnings(false);
            base.Browser.Dispose();
            base.Browser = null;
            return offerKey;
        }

        #endregion HelperMethods
    }
}