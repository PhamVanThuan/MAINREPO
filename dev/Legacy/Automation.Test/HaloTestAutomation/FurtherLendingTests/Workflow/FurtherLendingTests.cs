using Automation.DataAccess;
using System.Linq;
using System.Reflection;
using System.Threading;
using Automation.DataAccess;
using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation.FLOBO.Common;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.LegalEntity;
using BuildingBlocks.Presenters.Origination;
using BuildingBlocks.Presenters.Origination.FurtherLending;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using BuildingBlocks.CBO;

namespace FurtherLendingTests
{
    /// <summary>
    /// Contains the suite of tests for Further Lending in the Application Management workflow.
    /// </summary>
    [RequiresSTA]
    public class FurtherLendingTests : TestBase<BasePage>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.FLProcessor3);
        }

        protected override void OnTestStart()
        {
            base.OnTestStart();
            if (base.Browser != null)
            {
                base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(base.Browser);
            }
        }

        /// <summary>
        /// This test will create a number of Further Lending Applications using the FurtherLendingSequentialData data class
        /// populated from the test.AutomationFLTestCases table.
        /// </summary>
        /// <param name="identifier">The TestIdentifier of the Test Case</param>
        /// <param name="accountKey">Mortgage Loan Account Key</param>
        /// <param name="testGroup">The Test Group i.e. Readvances, Further Advances or Further Loans</param>
        [Test, Sequential, Description("Ensures that a FL Processor User can create a readvance application."), Category("All")]
        public void _001_CreateFurtherLendingApplications(
                                        [ValueSource(typeof(FurtherLendingSequentialData), "Identifier")] string identifier,
                                        [ValueSource(typeof(FurtherLendingSequentialData), "AccountKey")] int accountKey,
                                        [ValueSource(typeof(FurtherLendingSequentialData), "TestGroup")] string testGroup
                                                        )
        {
            //get the row
            Helper.CreateFurtherLendingApplications(identifier, accountKey, base.Browser);
        }

        /// <summary>
        /// A Further Lending Processor can search for an Application and perform the Application Received action on a further lending case
        /// in order to move it to the QA state. At this point a FL Processor is round robin assigned the case and an Account Memo record
        /// is inserted indicating that the FL application has been picked up and is assigned to X.
        /// </summary>
        /// <param name="identifier">Test Identifier(s) to perform the test for</param>
        [Test, Sequential, Description("Ensures that a FL Processor user can pick up a further lending case and perform Application Received"), Category("All")]
        public void _002_ApplicationReceived([Values(
                                                 FurtherLendingTestCases.ReadvanceCreate1,
                                                 FurtherLendingTestCases.ReadvanceCreate2,
                                                 FurtherLendingTestCases.ReadvanceCreate3,
                                                 FurtherLendingTestCases.ReadvanceCreate4,
                                                 FurtherLendingTestCases.ReadvanceCreate5,
                                                 FurtherLendingTestCases.ReadvanceCreate6,
                                                 FurtherLendingTestCases.ReadvanceCreate7,
                                                 FurtherLendingTestCases.ReadvanceCreate8,
                                                 FurtherLendingTestCases.ReadvanceCreate9,
                                                 FurtherLendingTestCases.ReadvanceCreate10,
                                                 FurtherLendingTestCases.FurtherAdvanceCreate1,
                                                 FurtherLendingTestCases.FurtherAdvanceCreate2,
                                                 FurtherLendingTestCases.FurtherAdvanceCreate3,
                                                 FurtherLendingTestCases.FurtherAdvanceCreate4,
                                                 FurtherLendingTestCases.FurtherLoanCreate1,
                                                 FurtherLendingTestCases.FurtherLoanCreate2,
                                                 FurtherLendingTestCases.FurtherLoanCreate3,
                                                 FurtherLendingTestCases.ReadvanceOver80Percent,
                                                 FurtherLendingTestCases.ReadvanceLessThanLAA
                                                 )] string identifier)
        {
            Helper.ApplicationReceived(identifier, base.Browser);
        }

        /// <summary>
        /// Once the case is at the QA state then the FL Processor can perform the QA Complete action on the Application which moves the
        /// case to Manage Application. This test also ensures that a user can provide a QA complete reason and that it is stored against the application correctly.
        /// The workflow case should not change ownership and should be assigned to the same user.
        /// </summary>
        /// <param name="identifier">Test Identifier(s) to perform the test for</param>
        [Test, Sequential, Description("Ensures that a FL Processor can QA Complete an application, moving it into Manage Application"), Category("All")]
        public void _003_QAComplete([Values(
                                                 FurtherLendingTestCases.ReadvanceCreate2,
                                                 FurtherLendingTestCases.ReadvanceCreate3,
                                                 FurtherLendingTestCases.ReadvanceCreate4,
                                                 FurtherLendingTestCases.ReadvanceCreate6,
                                                 FurtherLendingTestCases.ReadvanceCreate7,
                                                 FurtherLendingTestCases.ReadvanceCreate8,
                                                 FurtherLendingTestCases.ReadvanceCreate9,
                                                 FurtherLendingTestCases.ReadvanceCreate10,
                                                 FurtherLendingTestCases.FurtherAdvanceCreate2,
                                                 FurtherLendingTestCases.FurtherAdvanceCreate3,
                                                 FurtherLendingTestCases.FurtherAdvanceCreate4,
                                                 FurtherLendingTestCases.FurtherLoanCreate2,
                                                 FurtherLendingTestCases.FurtherLoanCreate3,
                                                 FurtherLendingTestCases.ReadvanceCreate5,
                                                 FurtherLendingTestCases.ReadvanceOver80Percent,
                                                 FurtherLendingTestCases.ReadvanceLessThanLAA
                                        )] string identifier)
        {
            Helper.QAComplete(identifier, base.Browser);
        }

        /// <summary>
        /// Performing the QA Query action on a Further Lending case will result in the case being moved to the QA Query state but unlike
        /// a new business application the case remains with the user who currently owns the case, in this case the FL Processor.
        /// </summary>
        /// <param name="identifier">Test Identifier</param>
        [Test, Description("A Further Lending Processor can perform the QA Query action at QA"), Category("All")]
        public void _004_QAQuery([Values(
                                     FurtherLendingTestCases.ReadvanceCreate1,
                                     FurtherLendingTestCases.FurtherAdvanceCreate1
                                     )] string identifier)
        {
            var testCase = Helper.SearchForFurtherLendingApp(identifier, base.Browser);
            //perform the action
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.QAQuery);
            string selectedReason = base.Browser.Page<CommonReasonCommonDecline>().SelectReasonAndSubmit(ReasonType.QAQuery);
            //REASON ASSERTION
            ReasonAssertions.AssertReason(selectedReason, ReasonType.QAQuery, testCase.OfferKey, GenericKeyTypeEnum.Offer_OfferKey);
            X2Assertions.AssertCurrentAppManX2State(testCase.OfferKey, WorkflowStates.ApplicationManagementWF.RequestatQA);
            //TODO: add the scheduled activity assertion, needs to include months
            //the case still belongs to the same user
            AssignmentAssertions.AssertWorkflowAssignment(testCase.Processor, testCase.OfferKey, OfferRoleTypeEnum.FLProcessorD);
        }

        /// <summary>
        /// Once the QA request has been resolved on the FL application the processor performs the Request Resolved action in order to
        /// move the case back to the Manage Application state. Again the case does not change worklists and remains in the same FL Processor
        /// worklist
        /// </summary>
        [Test, Description("Performing the Request Resolved action on a case will return the case back to the QA state"), Category("All")]
        public void _005_QARequestResolved()
        {
            const string identifier = FurtherLendingTestCases.ReadvanceCreate1;
            var testCase = Helper.SearchForFurtherLendingApp(identifier, base.Browser);
            //we need to clean up the further lending offer data
            Service<IFurtherLendingService>().CleanUpOfferData(testCase.OfferKey);
            //save the employment record
            LegalEntityCBONode.CompleteLegalEntityNode(base.Browser, testCase.OfferKey, false, true, false, false, false, false, false, false);
            //perform the action
            //base.Browser.ClickWorkflowLoanNode(WorkflowStates.ApplicationManagementWF.RequestatQA);
            base.Browser.ClickWorkflowLoanNode(Workflows.ApplicationManagement);
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.RequestResolved);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, true);
            //assert case has moved
            X2Assertions.AssertCurrentAppManX2State(testCase.OfferKey, WorkflowStates.ApplicationManagementWF.QA);
            //case belongs to the same user
            AssignmentAssertions.AssertWorkflowAssignment(testCase.Processor, testCase.OfferKey, OfferRoleTypeEnum.FLProcessorD);
        }

        /// <summary>
        /// A readvance application that is under 80%, under R1.5 million in value or does not have an additional surety added to the application
        /// does not have to go to Credit for a credit decision. This case will be assigned to a FL Supervisor at the Rapid Decision state
        /// in the Readvance Payments workflow.
        /// </summary>
        [Test, Description("After performing Application in Order for a readvance that does not need to go to credit it is assigned to an FLSUser"), Category("Readvances")]
        public void _006_ApplicationInOrderReadvancesNoCreditApproval([Values(
                                                                     FurtherLendingTestCases.ReadvanceCreate3,
                                                                     FurtherLendingTestCases.ReadvanceCreate5,
                                                                     FurtherLendingTestCases.ReadvanceCreate6,
                                                                     FurtherLendingTestCases.ReadvanceCreate7,
                                                                     FurtherLendingTestCases.ReadvanceCreate8,
                                                                     FurtherLendingTestCases.ReadvanceCreate9,
                                                                     FurtherLendingTestCases.ReadvanceCreate10,
                                                                    FurtherLendingTestCases.ReadvanceLessThanLAA
                                                                     )] string identifier)
        {
            var testCase = Helper.SearchForFurtherLendingApp(identifier, base.Browser);
            //we need to clean up the further lending offer data
            Service<IFurtherLendingService>().CleanUpOfferData(testCase.OfferKey);
            //save the employment record
            LegalEntityCBONode.CompleteLegalEntityNode(base.Browser, testCase.OfferKey, false, true, false, false, false, false, false, false);
            //perform the action
            base.Browser.ClickWorkflowLoanNode(Workflows.ApplicationManagement);
            string flSupervisor = Service<IAssignmentService>().GetNextRoundRobinAssignmentByOfferRoleTypeKey(OfferRoleTypeEnum.FLSupervisorD,
                RoundRobinPointerEnum.FLSupervisor);
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.ApplicationinOrder);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, true);
            Service<IX2WorkflowService>().WaitForX2State(testCase.OfferKey, Workflows.ReadvancePayments, WorkflowStates.ReadvancePaymentsWF.RapidDecision);
            X2Assertions.AssertCurrentReadvPaymentsX2State(testCase.OfferKey, WorkflowStates.ReadvancePaymentsWF.RapidDecision);
            AssignmentAssertions.AssertWorkflowAssignment(flSupervisor, testCase.OfferKey, OfferRoleTypeEnum.FLSupervisorD);
            //save the supervisor
            Service<IFurtherLendingService>().UpdateFLAutomation("AssignedFLSupervisor", flSupervisor, identifier);
            //update the indication that this is in the readvance payments workflow
            Service<IFurtherLendingService>().UpdateFLAutomation("ReadvancePayments", "1", identifier);
        }

        /// <summary>
        /// All Further Advance applications must be sent to Credit for a credit decision to be made. This test case ensures that the
        /// case ends up in the Credit state and also that the Application Management case is sent to the Credit Hold state.
        /// </summary>
        [Test, Description("A further advance case requires a credit decision to be made."), Category("Further Advances")]
        public void _007_ApplicationInOrderFurtherAdvance([Values(FurtherLendingTestCases.FurtherAdvanceCreate3)] string identifier)
        {
            Helper.ApplicationInOrderFurtherAdvance(identifier, "_007_ApplicationInOrderFurtherAdvance", base.Browser);
        }

        /// <summary>
        /// All Further Loan applications must be sent to Credit for a credit decision to be made. This test case ensures that the
        /// case ends up in the Credit state and also that the Application Management case is sent to the Credit Hold state.
        /// </summary>
        [Test, Description("A Further Loan case requires a credit decision to be made."), Category("Further Loans")]
        public void _007_ApplicationInOrderFurtherLoan()
        {
            const string identifier = FurtherLendingTestCases.FurtherLoanCreate3;
            var testCase = Helper.SearchForFurtherLendingApp(identifier, base.Browser);
            //navigate to the doc checklist
            Service<IFurtherLendingService>().CleanUpOfferData(testCase.OfferKey);
            base.Browser.Navigate<BuildingBlocks.Navigation.DocumentCheckListNode>().DocumentChecklist();
            base.Browser.Navigate<BuildingBlocks.Navigation.DocumentCheckListNode>().ViewDocumentChecklist(NodeTypeEnum.Update);
            base.Browser.Page<DocumentCheckListUpdate>().UpdateDocumentChecklist();
            //update the loan conditions
            Helper.SaveLoanConditions(base.Browser, testCase.OfferKey);
            //complete the LE CBO
            LegalEntityCBONode.CompleteLegalEntityNode(base.Browser, testCase.OfferKey, true, true, false, false, false, false, false, false);
            //perform the action
            base.Browser.ClickWorkflowLoanNode(Workflows.ApplicationManagement);
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.ApplicationinOrder);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, true);
            //Assert the case has moved
            X2Assertions.AssertCurrentAppManX2State(testCase.OfferKey, WorkflowStates.ApplicationManagementWF.CreditHold);
            Service<ICommonService>().InsertTestMethod("_008_ApplicationInOrderFurtherLoan", identifier, "FurtherLendingTests");
            //then store our params
            Service<ICommonService>().SaveTestMethodParameters("_008_ApplicationInOrderFurtherLoan", identifier, ParameterTypeEnum.OfferKey, testCase.OfferKey.ToString());
        }

        /// <summary>
        /// A Readvance application with an effective LTV of over 80% requires a Credit Decision, so after performing the Application in Order
        /// action a workflow case is created in the Credit workflow at the Credit state.
        /// </summary>
        [Test, Description("A Readvance application with an LTV > 80% requires a credit decision"), Category("Readvances")]
        public void _008_ApplicationInOrderReadvOver80()
        {
            const string identifier = FurtherLendingTestCases.ReadvanceOver80Percent;
            var testCase = Helper.SearchForFurtherLendingApp(identifier, base.Browser);
            //we need to clean up the further lending offer data
            Service<IFurtherLendingService>().CleanUpOfferData(testCase.OfferKey);
            //save the employment record
            LegalEntityCBONode.CompleteLegalEntityNode(base.Browser, testCase.OfferKey, false, true, false, false, false, false, false, false);
            //capture an ITC
            Service<ILegalEntityService>().InsertITC(testCase.OfferKey, 999, 711);
            base.Browser.ClickWorkflowLoanNode(Workflows.ApplicationManagement);
            //perform the action
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.ApplicationinOrder);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, true);
            Service<ICommonService>().InsertTestMethod("_009_ApplicationInOrderReadvOver80", identifier, "FurtherLendingTests");
            //then store our params
            Service<ICommonService>().SaveTestMethodParameters("_009_ApplicationInOrderReadvOver80", identifier, ParameterTypeEnum.OfferKey, testCase.OfferKey.ToString());
        }

        /// <summary>
        /// A new surety can be added to a Readvance Application at the Manage Application state. Once added the case should be sent
        /// to Credit for a credit decision to be made. This rule ensures that a FL Processor cannot add a Main Applicant to the application,
        /// that a warning appears in the FL Summary after an additional applicant has been added as well as that the a Credit workflow
        /// case is created at the Credit state.
        /// </summary>
        [Test, Description(@"A Readvance with an additional surety added requires a credit decision. You cannot add a main applicant
							and a warning should be displayed to the user after adding a surety indicating the additional applicant"), Category("Surety Add")]
        public void _009_ApplicationInOrderReadvSurety()
        {
            const string identifier = FurtherLendingTestCases.ReadvanceCreate2;
            var testCase = Helper.SearchForFurtherLendingApp(identifier, base.Browser);
            //we need to add an applicant
            base.Browser.Navigate<ApplicantsNode>().Applicants(NodeTypeEnum.Add);
            //add as a suretor
            base.Browser.Page<LegalEntityDetailsLeadApplicantAdd>().AddLegalEntity(OfferRoleTypes.Suretor, false, IDNumbers.GetNextIDNumber(), "Mr",
                "auto", "SuretorFirstname", "SuretorSurname", "auto", Gender.Male, MaritalStatus.Single, "Unknown", "Unknown", CitizenType.SACitizen, "auto",
                null, null, "Unknown", Language.English, null, "031", "1234567", null, null, null, null, null, null, true, false, false, false, false,
                ButtonTypeEnum.Next);
            //after saving there should be a message displayed on the screen
            base.Browser.ClickWorkflowLoanNode(Workflows.ApplicationManagement);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("CAUTION: 1 NEW APPLICANT/S IN PLACE");
            //we need to update applicants to provide declarations etc.
            LegalEntityCBONode.CompleteLegalEntityNode(base.Browser, testCase.OfferKey, true, false, false, false, false, false, false, false);
            Service<IFurtherLendingService>().CleanUpOfferData(testCase.OfferKey);
            //save the employment record
            LegalEntityCBONode.CompleteLegalEntityNode(base.Browser, testCase.OfferKey, false, true, false, false, false, false, false, false);
            //capture an ITC
            Service<ILegalEntityService>().InsertITC(testCase.OfferKey, 999, 711);
            //submit the application
            base.Browser.ClickWorkflowLoanNode(Workflows.ApplicationManagement);
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.ApplicationinOrder);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, true);
            Service<ICommonService>().InsertTestMethod("_010_ApplicationInOrderReadvSurety", identifier, "FurtherLendingTests");
            //then store our params
            Service<ICommonService>().SaveTestMethodParameters("_010_ApplicationInOrderReadvSurety", identifier, ParameterTypeEnum.OfferKey, testCase.OfferKey.ToString());
        }

        /// <summary>
        /// A Further Advance application with an additional surety added will require to be sent to Credit for a credit decision.
        /// This test ensures that the surety is added to the application and submits the case into credit.
        /// </summary>
        [Test, Description(@"A Further Advance application with an additional surety added requires a credit decision
							This test also ensures that the surety specific conditions have been added to the further advance
							offer."), Category("Surety Add")]
        public void _010_ApplicationInOrderFAdvSurety()
        {
            const string identifier = FurtherLendingTestCases.FurtherAdvanceCreate2;
            var testCase = Helper.SearchForFurtherLendingApp(identifier, base.Browser);
            //we need to add an applicant
            base.Browser.Navigate<ApplicantsNode>().Applicants(NodeTypeEnum.Add);
            //add an additional Surety
            base.Browser.Page<LegalEntityDetailsLeadApplicantAdd>().AddLegalEntity(OfferRoleTypes.Suretor, false, IDNumbers.GetNextIDNumber(), "Mr", "auto", "SuretorFirstname",
                "SuretorSurname", "auto", Gender.Male, MaritalStatus.Single, "Unknown", "Unknown", CitizenType.SACitizen, "auto", null, null, "Unknown",
                Language.English, null, "031", "1234567", null, null, null, null, null, null, true, false, false, false, false,
                ButtonTypeEnum.Next);
            //after saving there should be a message displayed on the screen
            base.Browser.ClickWorkflowLoanNode(Workflows.ApplicationManagement);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("CAUTION: 1 NEW APPLICANT/S IN PLACE");
            //we need to update applicants to provide declarations etc.
            LegalEntityCBONode.CompleteLegalEntityNode(base.Browser, testCase.OfferKey, true, false, false, false, false, false, false, false);
            //need to add loan conditions
            Helper.SaveLoanConditions(base.Browser, testCase.OfferKey);
            //Assert the further advance condition for surety additions are saved against the Offer
            string[] expectedConditions = new string[] { "224", "225", "226" };
            OfferAssertions.AssertOfferConditionsExist(testCase.OfferKey, expectedConditions);
            //document checklist
            Service<IFurtherLendingService>().CleanUpOfferData(testCase.OfferKey);
            //save the employment record
            LegalEntityCBONode.CompleteLegalEntityNode(base.Browser, testCase.OfferKey, false, true, false, false, false, false, false, false);
            //capture an ITC
            Service<ILegalEntityService>().InsertITC(testCase.OfferKey, 999, 711);
            base.Browser.Navigate<BuildingBlocks.Navigation.DocumentCheckListNode>().DocumentChecklist();
            base.Browser.Navigate<BuildingBlocks.Navigation.DocumentCheckListNode>().ViewDocumentChecklist(NodeTypeEnum.Update);
            base.Browser.Page<DocumentCheckListUpdate>().UpdateDocumentChecklist();
            //submit the application
            base.Browser.ClickWorkflowLoanNode(Workflows.ApplicationManagement);
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.ApplicationinOrder);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, true);
            Service<ICommonService>().InsertTestMethod("_011_ApplicationInOrderFAdvSurety", identifier, "FurtherLendingTests");
            //then store our params
            Service<ICommonService>().SaveTestMethodParameters("_011_ApplicationInOrderFAdvSurety", identifier, ParameterTypeEnum.OfferKey, testCase.OfferKey.ToString());
        }

        /// <summary>
        /// A Further Loan application with an additional surety added will require to be sent to Credit for a credit decision.
        /// This test ensures that the surety is added to the application and submits the case into credit.
        /// </summary>
        [Test, Description(@"A Further Loan application with an additional surety added requires a credit decision. This test also
							ensures that the correct loan conditions have been added to the Further Loan offer"), Category("Surety Add")]
        public void _011_ApplicationInOrderFLoanSurety()
        {
            const string identifier = FurtherLendingTestCases.FurtherLoanCreate2;
            var testCase = Helper.SearchForFurtherLendingApp(identifier, base.Browser);
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, testCase.OfferKey, WorkflowStates.ApplicationManagementWF.ManageApplication);
            //we need to add an applicant
            base.Browser.Navigate<ApplicantsNode>().Applicants(NodeTypeEnum.Add);
            //add an additional Surety
            base.Browser.Page<LegalEntityDetailsLeadApplicantAdd>().AddLegalEntity(OfferRoleTypes.Suretor, false, IDNumbers.GetNextIDNumber(), "Mr", "auto", "SuretorFirstname",
                "SuretorSurname", "auto", Gender.Male, MaritalStatus.Single, "Unknown", "Unknown", CitizenType.SACitizen, "auto", null, null, "Unknown", Language.English,
                null, "031", "1234567", null, null, null, null, null, null, true, false, false, false, false, ButtonTypeEnum.Next);
            //after saving there should be a message displayed on the screen
            base.Browser.ClickWorkflowLoanNode(Workflows.ApplicationManagement);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("CAUTION: 1 NEW APPLICANT/S IN PLACE");
            //Clean up the offer data
            Service<IFurtherLendingService>().CleanUpOfferData(testCase.OfferKey);
            //we need to update applicants to provide declarations etc.
            LegalEntityCBONode.CompleteLegalEntityNode(base.Browser, testCase.OfferKey, true, true, false, false, false, false, false, false);
            //need to add loan conditions
            Helper.SaveLoanConditions(base.Browser, testCase.OfferKey);
            //Assert the further advance condition for surety additions are saved against the Offer
            string[] expectedConditions = new string[] { "219", "225" };
            OfferAssertions.AssertOfferConditionsExist(testCase.OfferKey, expectedConditions);
            //capture an ITC
            Service<ILegalEntityService>().InsertITC(testCase.OfferKey, 999, 711);
            //document checklist
            base.Browser.Navigate<BuildingBlocks.Navigation.DocumentCheckListNode>().DocumentChecklist();
            base.Browser.Navigate<BuildingBlocks.Navigation.DocumentCheckListNode>().ViewDocumentChecklist(NodeTypeEnum.Update);
            base.Browser.Page<DocumentCheckListUpdate>().UpdateDocumentChecklist();
            //submit the application
            base.Browser.ClickWorkflowLoanNode(Workflows.ApplicationManagement);
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.ApplicationinOrder);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, true);
            Service<ICommonService>().InsertTestMethod("_012_ApplicationInOrderFLoanSurety", identifier, "FurtherLendingTests");
            //then store our params
            Service<ICommonService>().SaveTestMethodParameters("_012_ApplicationInOrderFLoanSurety", identifier, ParameterTypeEnum.OfferKey, testCase.OfferKey.ToString());
        }

        /// <summary>
        /// Runs the test assertions and data table updates for the Further Lending Test Cases that have been sent to credit
        /// </summary>
        /// <param name="testMethod"></param>
        /// <param name="identifier"></param>
        [Test, Sequential, Description("Ensures that the further lending cases sent to credit have had their cases created correctly in Credit")]
        public void _012a_AssertFurtherLendingCreditCases([Values(
                                                              "_007_ApplicationInOrderFurtherAdvance",
                                                              "_008_ApplicationInOrderFurtherLoan",
                                                              "_009_ApplicationInOrderReadvOver80",
                                                              "_010_ApplicationInOrderReadvSurety",
                                                              "_011_ApplicationInOrderFAdvSurety",
                                                              "_012_ApplicationInOrderFLoanSurety"
                                                              )] string testMethod,
                                                          [Values(
                                                              FurtherLendingTestCases.FurtherAdvanceCreate3,
                                                              FurtherLendingTestCases.FurtherLoanCreate3,
                                                              FurtherLendingTestCases.ReadvanceOver80Percent,
                                                              FurtherLendingTestCases.ReadvanceCreate2,
                                                              FurtherLendingTestCases.FurtherAdvanceCreate2,
                                                              FurtherLendingTestCases.FurtherLoanCreate2
                                                              )] string identifier)
        {
            Helper.AssertFurtherLendingCreditCases(testMethod, identifier);
        }

        /// <summary>
        /// This test will setup the QA Query timer for a case that enters the QA Query state, and then update the scheduled activity to an
        /// earlier time so that the scheduled activity fires as expected.
        /// </summary>
        [Test, Description(@"When a further lending case is moved into the QA Query state a scheduled activity is setup to
							archive the case after a certain period of time."), Category("Readvances")]
        public void _013_QAQueryTimerArchivesCase()
        {
            int offerKey = Service<IX2WorkflowService>().GetOfferKeyAtStateByType(WorkflowStates.ApplicationManagementWF.QA, Workflows.ApplicationManagement, OfferTypeEnum.Readvance,
                "FLAutomation");
            var instanceID = Service<IX2WorkflowService>().GetAppManInstanceIDByState(WorkflowStates.ApplicationManagementWF.QA, offerKey);
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.ApplicationManagementWF.QA);
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.QAQuery);
            string selectedReason = base.Browser.Page<CommonReasonCommonDecline>().SelectReasonAndSubmit(ReasonType.QAQuery);
            //REASON ASSERTION
            ReasonAssertions.AssertReason(selectedReason, ReasonType.QAQuery, offerKey, GenericKeyTypeEnum.Offer_OfferKey);
            X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.RequestatQA);
            //scheduled activity has been setup
            X2Assertions.AssertScheduleActivitySetup(offerKey.ToString(), ScheduledActivities.ApplicationManagement._2Months, 0, 0, 2);
            //fire the timer
            scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.ApplicationManagement, WorkflowAutomationScripts.FurtherLending.Fire2MonthsTimer, offerKey);
            //assert
            X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.ArchiveQAQuery);
            OfferAssertions.AssertOfferEndDate(offerKey, DateTime.Now, 0, false);
            OfferAssertions.AssertOfferStatus(offerKey, OfferStatusEnum.NTU);
        }

        /// <summary>
        /// This test ensures that the FL Processor can rework a Further Lending Application. Once complete the
        /// test will assert that the application amount for each application that was reworked has been updated
        /// correctly. The case should also not move states in the X2 workflow and remained assigned to the
        /// currently assigned FL Processor.
        /// </summary>
        [Test, Description("This test ensures that a FL Processor can rework a FL application"), Category("Readvances")]
        public void _014_FLReworkApplication()
        {
            int offerKey = Service<IX2WorkflowService>().GetOfferKeyAtStateByType(WorkflowStates.ApplicationManagementWF.ManageApplication, Workflows.ApplicationManagement,
                OfferTypeEnum.FurtherAdvance, "FLAutomation");
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.ApplicationManagementWF.ManageApplication);
            //perform the Rework Action
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.FLReworkApplication);
            //rework the application
            int appAmt = base.Browser.Page<FurtherLendingCalculator>().ReworkFLApplication(offerKey, OfferTypeEnum.FurtherAdvance);
            //assert that the LoanAmountNoFees has changed
            OfferAssertions.AssertApplicationInformation(appAmt, offerKey, "LoanAmountNoFees");
        }

        /// <summary>
        /// A FL Processor should be allowed to request for a valuation to be performed on the property linked to the application, as
        /// long as there is not an existing open valuation case for the application. This should result in a valuations instance being
        /// created for the application. Test ensures the following: Valuations clone is created, Valuations clone is in the correct state and is assigned correctly
        /// </summary>
        [Test, Description("A FL Processor can request a valuation to be performed on the property linked to an application"), Category("Further Advances")]
        public void _015_PerformValuation()
        {
            var offerKeys = Service<IX2WorkflowService>().GetOfferKeysAtStateByType(WorkflowStates.ApplicationManagementWF.ManageApplication,
                                                         Workflows.ApplicationManagement,
                                                         OfferTypeEnum.FurtherAdvance, "FLAutomation");
            var states = new[] {
                                    WorkflowStates.ApplicationManagementWF.ValuationHold,
                                    WorkflowStates.ApplicationManagementWF.ValuationReviewRequired
                                };
            int offerKey = (from o in offerKeys
                            where Service<IX2WorkflowService>().CloneExists(o.Column("OfferKey").GetValueAs<int>(), states, Workflows.ApplicationManagement) == false
                            select o.Column("OfferKey").GetValueAs<int>())
                               .FirstOrDefault();
            if (offerKey > 0)
            {
                //we now have an offer without a val clone => no open valuation case
                base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, Workflows.ApplicationManagement);
                //get the next VP User
                string vpUser = Service<IAssignmentService>().GetNextRoundRobinAssignmentByOfferRoleTypeKey(OfferRoleTypeEnum.ValuationsAdministratorD,
                    RoundRobinPointerEnum.ValuationsAdministrator);
                //perform the action
                base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.PerformValuation);
                base.Browser.Page<WorkflowYesNo>().Confirm(true, true);
                Thread.Sleep(5000);
                //assert the val clone has been created
                Int64 clonedInstanceID = X2Assertions.AssertX2CloneExists(offerKey.ToString(), WorkflowStates.ApplicationManagementWF.ValuationHold, Workflows.ApplicationManagement);
                //wait for the valuations case
                Service<IX2WorkflowService>().WaitForValuationsCaseCreate(clonedInstanceID, offerKey, WorkflowStates.ValuationsWF.ScheduleValuationAssessment);
                //assert the valuations case has been created
                X2Assertions.AssertCurrentValuationsX2State(offerKey, WorkflowStates.ValuationsWF.ScheduleValuationAssessment);
                //assert WFA
                var roleTypes = new[] { ((int)OfferRoleTypeEnum.ValuationsAdministratorD).ToString() };
                WorkflowRoleAssignmentAssertions.AssertWorkFlowAssignmentRecordOfferRoleAssignment(offerKey, WorkflowStates.ValuationsWF.ScheduleValuationAssessment,
                    vpUser, 1, roleTypes);
            }
            else
            {
                Assert.Fail("No further advance application could be found without an existing valuations clone.");
            }
        }

        /// <summary>
        /// This test case will attempt to move a Further Advance from the Awaiting Application state, while the Mortgage Loan Account currently
        /// has a Readvance application in progress, that the Further Advance case is sent to the Application Hold state.
        /// </summary>
        [Test, Description("An application for an account that has a higher priority application in progress should not be able to commence"), Category("All")]
        public void _016_AppsHigherPriorityInProgress()
        {
            const string identifier = FurtherLendingTestCases.ReadvFAdvAndFLCreate1;
            QueryResults results = Service<IFurtherLendingService>().GetFLAutomationRowByTestIdentifier(identifier);
            //login
            int readvOfferKey = results.Rows(0).Column("ReadvOfferKey").GetValueAs<int>();
            int offerKey = results.Rows(0).Column("FAdvOfferKey").GetValueAs<int>();
            base.Browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().WorkFlows(base.Browser);
            base.Browser.Page<WorkflowSuperSearch>().Search(readvOfferKey);
            //perform application received on the readvance
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.ApplicationReceived);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, false);
            Service<IX2WorkflowService>().WaitForX2State(readvOfferKey, Workflows.ApplicationManagement, WorkflowStates.ApplicationManagementWF.QA);
            //perform app received now on the further advance
            base.Browser.Page<WorkflowSuperSearch>().Search(offerKey);
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.ApplicationReceived);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, false);
            Service<IX2WorkflowService>().WaitForX2State(offerKey, Workflows.ApplicationManagement, WorkflowStates.ApplicationManagementWF.ApplicationHold);
            //should be in app hold state
            X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.ApplicationHold);
            results.Dispose();
        }

        /// <summary>
        /// If a Further Advance application is currently in progress and a FL Processor performs the Application Received action on a Readvance Application
        /// then the Readvance case will take priority. This results in the Further Advance case being moved from its current state in the
        /// workflow to the Application Hold state, whilst the Readvance application should move through to the QA state. This test case ensures
        /// that:
        /// <list type="bullet">
        /// <item>
        /// <description>The Further Advance case is moved to Application Hold</description>
        /// </item>
        /// <item>
        /// <description>The Readvance case is sent through to QA</description>
        /// </item>
        /// <item>
        /// <description>The Readvance case is assigned to the same FL Processor as the F.Advance</description>
        /// </item>
        /// <item>
        /// <description>A memo record is saved against the Account once the Application Received action has completed for the Readvance</description>
        /// </item>
        /// </list>
        /// </summary>
        [Test, Description(@"An application of a higher priority should replace a lower priority application in progress when performing the
			Application Received action"), Category("All")]
        public void _017_AppHighestPriority()
        {
            const string identifier = FurtherLendingTestCases.ReadvFAdvAndFLCreate2;
            QueryResults results = Service<IFurtherLendingService>().GetFLAutomationRowByTestIdentifier(identifier);
            //login
            int offerKey = results.Rows(0).Column("FAdvOfferKey").GetValueAs<int>();
            //perform application received on the further advance
            base.Browser.Page<WorkflowSuperSearch>().Search(offerKey);
            string flAppProcUser = Service<IAssignmentService>().GetNextRoundRobinAssignmentByOfferRoleTypeKey(OfferRoleTypeEnum.FLProcessorD,
                RoundRobinPointerEnum.FLProcessor);
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.ApplicationReceived);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, false);
            Service<IX2WorkflowService>().WaitForX2State(offerKey, Workflows.ApplicationManagement, WorkflowStates.ApplicationManagementWF.QA);
            //assert the memo record exists
            MemoAssertions.AssertApplicationReceivedMemo(results.Rows(0).Column("AccountKey").GetValueAs<int>(), offerKey);
            //assert workflow assignment to expected user
            AssignmentAssertions.AssertWorkflowAssignment(flAppProcUser, offerKey, OfferRoleTypeEnum.FLProcessorD);
            //if all is okay then we can save the FLAppProcUser
            Service<IFurtherLendingService>().UpdateFLAutomation("AssignedFLAppProcUser", flAppProcUser, identifier);
            //we now need to get the readvance application
            int readvanceOfferKey = results.Rows(0).Column("ReadvOfferKey").GetValueAs<int>();
            base.Browser.Page<WorkflowSuperSearch>().Search(readvanceOfferKey);
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.ApplicationReceived);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, false);
            //the readvance case should now be at QA, the further advance case at Application Hold.
            Service<IX2WorkflowService>().WaitForX2State(readvanceOfferKey, Workflows.ApplicationManagement, WorkflowStates.ApplicationManagementWF.QA);
            X2Assertions.AssertCurrentAppManX2State(readvanceOfferKey, WorkflowStates.ApplicationManagementWF.QA);
            Service<IX2WorkflowService>().WaitForX2State(offerKey, Workflows.ApplicationManagement, WorkflowStates.ApplicationManagementWF.ApplicationHold);
            X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.ApplicationHold);
            //wfa assertion for the readvance
            AssignmentAssertions.AssertWorkflowAssignment(flAppProcUser, readvanceOfferKey, OfferRoleTypeEnum.FLProcessorD);
            MemoAssertions.AssertApplicationReceivedMemo(results.Rows(0).Column("AccountKey").GetValueAs<int>(), readvanceOfferKey);
            results.Dispose();
        }

        /// <summary>
        /// This test case sets up the 2 Months timer in order to expire a Readvance application at the QA state. The archiving of the
        /// Readvance application which has a Further Advance application in the Application Hold state should result in the Further
        /// Advance case being moved out to its Previous State prior to it being moved to Application Hold.
        /// </summary>
        [Test, Description(@"Archiving a FL application should move related applications out of the Application Hold state. This test will
							update the scheduled activity so that the readvance application will archive correctly."), Category("All")]
        public void _018_ArchiveAppReactivateAppTimerSetup()
        {
            const string identifier = FurtherLendingTestCases.ReadvFAdvAndFLCreate2;
            QueryResults results = Service<IFurtherLendingService>().GetFLAutomationRowByTestIdentifier(identifier);
            int offerKey = results.Rows(0).Column("ReadvOfferKey").GetValueAs<int>();
            int fAdvOfferKey = results.Rows(0).Column("FAdvOfferKey").GetValueAs<int>();
            //get the instanceid
            var reAdvanceInstanceID = Service<IX2WorkflowService>().GetAppManInstanceIDByState(WorkflowStates.ApplicationManagementWF.QA, offerKey);
            var furtherAdvanceinstanceID = Service<IX2WorkflowService>().GetAppManInstanceIDByState(WorkflowStates.ApplicationManagementWF.ApplicationHold, fAdvOfferKey);
            //load it into the workflow
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.ApplicationManagementWF.QA);
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.QAQuery);
            string selectedReason = base.Browser.Page<CommonReasonCommonDecline>().SelectReasonAndSubmit(ReasonType.QAQuery);
            //REASON ASSERTION
            ReasonAssertions.AssertReason(selectedReason, ReasonType.QAQuery, offerKey, GenericKeyTypeEnum.Offer_OfferKey);
            Thread.Sleep(2500);
            X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.RequestatQA);
            //update the scheduled activity time
            scriptEngine.ExecuteScript(WorkflowEnum.ApplicationManagement, WorkflowAutomationScripts.ApplicationManagement.TwoMonthQAQueryTimer, offerKey);
            //wait to fire
            Service<IX2WorkflowService>().WaitForX2WorkflowHistoryActivity(ScheduledActivities.ApplicationManagement._2Months, reAdvanceInstanceID, 1);
            //wait for reactive
            Service<IX2WorkflowService>().WaitForX2WorkflowHistoryActivity(ExternalActivities.ApplicationManagement.EXTReactivateApp, furtherAdvanceinstanceID, 1);
            results.Dispose();
        }

        /// <summary>
        /// This test will perform the Return to Manage Application as a FL Supervisor on an application that is in the
        /// Rapid Decision state in the Readvance Payments workflow. This test ensures;
        /// <list type="bullet">
        /// <item>
        /// <description>The case is assigned to the current FL Processor unless they are inactive</description>
        /// </item>
        /// <item>
        /// <description>If they are inactive we then fetch the next FL Processor and check the case is assigned to them</description>
        /// </item>
        /// <item>
        /// <description>The workflow case ends up at the Manage Application state</description>
        /// </item>
        /// </list>
        /// </summary>
        [Test, Description(@"A FL Supervisor can return a case to the FL Processor's Manage Application state"), Category("Readvances")]
        public void _019_ReturnToManageApplication()
        {
            base.FLSupervisorBrowser = new TestBrowser(TestUsers.FLSupervisor);
            const string identifier = FurtherLendingTestCases.ReadvanceCreate10;
            var testCase = Helper.SearchForFurtherLendingApp(identifier, base.FLSupervisorBrowser);
            string flAppProcUser = base.FLSupervisorBrowser.Page<ApplicationSummaryFurtherLending>().GetApplicationProcessor();
            bool isActive = base.Service<IADUserService>().IsADUserActive(flAppProcUser);
            //if the user is inactive they will not be assigned the case when returning to Manage Application
            if (!isActive)
            {
                //so we find the next FL Processor
                flAppProcUser = Service<IAssignmentService>().GetNextRoundRobinAssignmentByOfferRoleTypeKey(OfferRoleTypeEnum.FLProcessorD,
                    RoundRobinPointerEnum.FLProcessor);
            }
            //perform the action
            base.FLSupervisorBrowser.ClickAction(WorkflowActivities.ApplicationManagement.ReturntoManageApplication);
            base.FLSupervisorBrowser.Page<WorkflowYesNo>().Confirm(true, true);
            //case should have moved to Manage Application
            X2Assertions.AssertCurrentAppManX2State(testCase.OfferKey, WorkflowStates.ApplicationManagementWF.ManageApplication);
            AssignmentAssertions.AssertWorkflowAssignment(flAppProcUser, testCase.OfferKey, OfferRoleTypeEnum.FLProcessorD);
        }

        /// <summary>
        /// When a Further Lending application is archived and there are related applications against the account we need to reactivate the
        /// applications that are in Application Hold. This test uses an account that has previously had a Further Advance application moved
        /// from QA to Application Hold. The readvance application will be archived using the 2 Months QA timer, which should result in the
        /// Further Advance moving back to its previous state, in this case QA.
        /// <seealso cref="_033_ArchiveAppReactivateAppTimerSetup()"/>
        /// </summary>
        [Test, Description("Archiving a further lending application will reactivate any related applications in Application Hold"), Category("All")]
        public void _020_ReactivateAppFromApplicationHold()
        {
            Console.WriteLine(String.Format(@"--********{0}********", MethodBase.GetCurrentMethod()));
            const string identifier = FurtherLendingTestCases.ReadvFAdvAndFLCreate2;
            QueryResults results = Service<IFurtherLendingService>().GetFLAutomationRowByTestIdentifier(identifier);
            var offerKey = results.Rows(0).Column("FAdvOfferKey").GetValueAs<int>();
            string flAppProcUser = results.Rows(0).Column("AssignedFLAppProcUser").Value;
            //this case should be at its prev state = QA
            X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.QA);
            //assigned to the original user
            AssignmentAssertions.AssertWorkflowAssignment(flAppProcUser, offerKey, OfferRoleTypeEnum.FLProcessorD);
            results.Dispose();
        }

        /// <summary>
        /// If the property linked to the Further Advance application has not had a valuation performed in the last 3 years then
        /// the application requires a valuation to be completed and a valuations clone should be created.
        /// </summary>
        [Test, Description(@"If the property linked to the Further Advance application has not had a valuation performed in the last 3 years then
			the application requires a valuation to be completed and a valuations clone should be created.")]
        public void _021_FurtherAdvanceValuationOlderThan36Months()
        {
            const string identifier = FurtherLendingTestCases.FurtherAdvanceValRequired;
            QueryResults r = Service<IFurtherLendingService>().GetFLAutomationRowByTestIdentifier(identifier);
            //create the application
            Helper.CreateFurtherLendingApplications(identifier, r.Rows(0).Column("AccountKey").GetValueAs<int>(), base.Browser);
            //perform application received
            Helper.ApplicationReceived(identifier, base.Browser);
            //perform QA Complete
            Helper.QAComplete(identifier, base.Browser);
        }

        /// <summary>
        /// If the property linked to the Further Loan application has not had a valuation performed in the last 2 years then
        /// the application requires a valuation to be completed and a valuations clone should be created.
        /// </summary>
        [Test, Description(@"If the property linked to the Further Loan application has not had a valuation performed in the last 2 years then
			the application requires a valuation to be completed and a valuations clone should be created.")]
        public void _022_FurtherLoanValuationOlderThan24Months()
        {
            const string identifier = FurtherLendingTestCases.FurtherLoanValRequired;
            QueryResults r = Service<IFurtherLendingService>().GetFLAutomationRowByTestIdentifier(identifier);
            //create the application
            Helper.CreateFurtherLendingApplications(identifier, r.Rows(0).Column("AccountKey").GetValueAs<int>(), base.Browser);
            //perform application received
            Helper.ApplicationReceived(identifier, base.Browser);
            //perform QA Complete
            Helper.QAComplete(identifier, base.Browser);
        }

        /// <summary>
        /// This test will run the assertions in order to ensure that the valuation clone has been created.
        /// </summary>
        /// <param name="identifier"></param>
        [Test, Description("This test will run the assertions in order to ensure that the valuation clone has been created.")]
        public void _023_FurtherLendingValCloneAssertions([Values(FurtherLendingTestCases.FurtherAdvanceValRequired, FurtherLendingTestCases.FurtherLoanValRequired)] string identifier)
        {
            Thread.Sleep(10000);
            var testCase = Helper.GetTestCase(identifier);
            switch (identifier)
            {
                case FurtherLendingTestCases.FurtherLoanValRequired:
                    X2Assertions.AssertCurrentValuationsX2State(testCase.FurtherLoanOfferKey, WorkflowStates.ValuationsWF.ScheduleValuationAssessment);
                    break;

                case FurtherLendingTestCases.FurtherAdvanceValRequired:
                    X2Assertions.AssertCurrentValuationsX2State(testCase.FurtherAdvanceOfferKey, WorkflowStates.ValuationsWF.ScheduleValuationAssessment);
                    break;
            }
        }

        /// <summary>
        /// This test will ensure that the discounted initiation fee is not applied to further lending applications.
        /// </summary>
        [Test, Description("This test will ensure that the discounted initiation fee is not applied to further lending applications.")]
        public void _024_discounted_initiation_fee_is_not_applied_on_further_lending_applications()
        {
            string identifier = FurtherLendingTestCases.ReturningCustomerDiscountNotApplied;
            QueryResults r = Service<IFurtherLendingService>().GetFLAutomationRowByTestIdentifier(identifier);
            int offerKey = r.Rows(0).Column("FLOfferKey").GetValueAs<int>();
            OfferAssertions.AssertOfferAttributeExists(offerKey, Common.Enums.OfferAttributeTypeEnum.DiscountedInitiationFee_ReturningClient, false);
            OfferAssertions.AssertOfferExpense(offerKey, 5529.0f, false, ExpenseTypeEnum.InitiationFeeBondPreparationFee);
        }
    }
}