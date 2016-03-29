using Automation.DataAccess;
using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.LegalEntity;
using BuildingBlocks.Presenters.Origination;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using WatiN.Core;
using WatiN.Core.Logging;
using Description = NUnit.Framework.DescriptionAttribute;

namespace Origination.Workflow
{
    /// <summary>
    /// Contains the tests for the Post Credit section of Origination. i.e. After Credit have approved the application.
    /// </summary>
    [TestFixture, RequiresSTA]
    public class _06PostCredit : Origination.OriginationTestBase<BasePage>
    {
        private TestBrowser browser;
        private string _identifier;
        private int offerKey;
        private int _pipelineOfferKey;
        private int _pipelineAccountKey;
        private int _instructOfferKey;
        private int _instructAccountKey;

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
        }

        protected override void OnTestTearDown()
        {
            base.OnTestTearDown();
            if (browser != null)
            {
                try
                {
                    browser.Page<BasePage>().CheckForErrorMessages();
                }
                finally
                {
                    Service<IWatiNService>().KillAllIEProcesses();
                    browser = null;
                }
            }
        }

        public IEnumerable<Automation.DataModels.OriginationTestCase> GetTestCasesForApplicationInOrder()
        {
            var testCases = Service<ICommonService>().GetOriginationTestCases();
            return (from t in testCases where t.ApplicationManagementTestGroup == "ApplicationInOrder" select t);
        }

        #region Tests

        /// <summary>
        /// This test will login as a Branch Consultant and perform the Send LOA action for a case at the LOA state in the Application Management workflow.
        /// </summary>
        [Test, Description(@"This test will login as a Branch Consultant and perform the Send LOA action for a case at the LOA state in the Application Management workflow.")]
        public void _001_SendLOA()
        {
            Console.WriteLine(string.Format(@"--********{0}********--", MethodBase.GetCurrentMethod()));
            _identifier = "ApplicationInOrderSwitchNewVariableApplication";
            offerKey = Service<ICommonService>().GetOfferKeyByTestIdentifier(_identifier, "ApplicationManagementTestID");

            CreateCaseAtStageIfOfferNotAtState(ref offerKey, WorkflowStates.ApplicationManagementWF.LOA, OfferTypeEnum.SwitchLoan, LegalEntityType.NaturalPerson, Products.NewVariableLoan, 1125365, 605000, 95000, 0, 50000, EmploymentType.Salaried, false, false);
            Service<ICommonService>().CommitOfferKeyForTestIdentifier("ApplicationManagementTestID", _identifier, offerKey);
            
            Helper.SendLOA(browser, offerKey);
        }

        /// <summary>
        /// This is a sequential test that will take our set of cases that were approved from Credit and perform the LOA Received action on each of them.
        /// The test ensures that the case is moved to the following state and that it is correctly assigned to the static workflow roles for the Registrations
        /// Users.
        /// </summary>
        [Test, TestCaseSource(typeof(_06PostCredit), "GetTestCasesForApplicationInOrder"), Description(@"This is a sequential test that will take our set of cases that were approved from Credit and perform the LOA Received action on each of them.
        The test ensures that the case is moved to the following state and that it is correctly assigned to the static workflow roles for the Registrations
        Users.")]
        public void _002_LOAReceived(Automation.DataModels.OriginationTestCase testCase)
        {
            Console.WriteLine(string.Format(@"--********{0}********--", MethodBase.GetCurrentMethod()));
            offerKey = Service<ICommonService>().GetOfferKeyByTestIdentifier(testCase.TestIdentifier);
            Helper.LOAReceived(browser, testCase.Username, offerKey);
        }

        /// <summary>
        /// This test will perform the Query on LOA action on an application at the LOA state.
        /// </summary>
        [Test, Description("This test will perform the Query on LOA action on an application at the LOA state.")]
        public void _003_QueryOnLOA()
        {
            Console.WriteLine(string.Format(@"--********{0}********--", MethodBase.GetCurrentMethod()));
            _identifier = "ApplicationInOrderSwitchNewVariableApplication";
            offerKey = Service<ICommonService>().GetOfferKeyByTestIdentifier(_identifier, "ApplicationManagementTestID");

            CreateCaseAtStageIfOfferNotAtState(ref offerKey, WorkflowStates.ApplicationManagementWF.SignedLOAReview, OfferTypeEnum.SwitchLoan, LegalEntityType.NaturalPerson, Products.NewVariableLoan, 1125365, 605000, 95000, 0, 50000, EmploymentType.Salaried, false, false);
            Service<ICommonService>().CommitOfferKeyForTestIdentifier("ApplicationManagementTestID", _identifier, offerKey);
            
            var r = Service<ICommonService>().OffersAtApplicationCaptureRow(_identifier);
            string username = r.Rows(0).Column("Username").Value;
            Helper.QueryOnLOA(browser, offerKey, username);
        }

        /// <summary>
        /// This test will Query Complete action on a case at the LOA Query state.
        /// </summary>
        [Test, Description("This test will Query Complete action on a case at the LOA Query state.")]
        public void _004_LOAQueryComplete()
        {
            Console.WriteLine(string.Format(@"--********{0}********--", MethodBase.GetCurrentMethod()));
            _identifier = "ApplicationInOrderSwitchNewVariableApplication";
            var r = Service<ICommonService>().OffersAtApplicationCaptureRow(_identifier);
            offerKey = Service<ICommonService>().GetOfferKeyByTestIdentifier(_identifier, "ApplicationManagementTestID");

            CreateCaseAtStageIfOfferNotAtState(ref offerKey, WorkflowStates.ApplicationManagementWF.LOAQuery, OfferTypeEnum.SwitchLoan, LegalEntityType.NaturalPerson, Products.NewVariableLoan, 1125365, 605000, 95000, 0, 50000, EmploymentType.Salaried, false, false);
            Service<ICommonService>().CommitOfferKeyForTestIdentifier("ApplicationManagementTestID", _identifier, offerKey);
            
            string username = r.Rows(0).Column("Username").Value;
            Helper.LOAQueryComplete(browser, username, offerKey);
        }

        /// <summary>
        /// This test will perform the Resend LOA action on an application that is currently at the Signed LOA Review state. This will move the application
        /// back to the LOA state assigned to our Branch Consultant.
        /// </summary>
        [Test, Description(@"This test will perform the Resend LOA action on an application that is currently at the Signed LOA Review state. This will move the application
        back to the LOA state assigned to our Branch Consultant.")]
        public void _005_ResendLOA()
        {
            Console.WriteLine(string.Format(@"--********{0}********--", MethodBase.GetCurrentMethod()));
            _identifier = "ApplicationInOrderSwitchNewVariableApplication";
            offerKey = Service<ICommonService>().GetOfferKeyByTestIdentifier(_identifier, "ApplicationManagementTestID");

            CreateCaseAtStageIfOfferNotAtState(ref offerKey, WorkflowStates.ApplicationManagementWF.SignedLOAReview, OfferTypeEnum.SwitchLoan, LegalEntityType.NaturalPerson, Products.NewVariableLoan, 1125365, 605000, 95000, 0, 50000, EmploymentType.Salaried, false, false);
            Service<ICommonService>().CommitOfferKeyForTestIdentifier("ApplicationManagementTestID", _identifier, offerKey);
            
            var results = Service<ICommonService>().OffersAtApplicationCaptureRow(_identifier);
            string expectedBranchConsultant = results.Rows(0).Column("Username").Value;
            Helper.ResendLOA(browser, expectedBranchConsultant, offerKey);
        }

        /// <summary>
        /// This test returns the test case used for the Resend LOA test back to Signed LOA Review in order for the case to be able to pushed
        /// back through the workflow.
        /// </summary>
        [Test, Description(@"This test returns the test case used for the Resend LOA test back to Signed LOA Review in order for the case to be able to pushed
        back through the workflow.")]
        public void _005b_LOAReceived()
        {
            Console.WriteLine(string.Format(@"--********{0}********--", MethodBase.GetCurrentMethod()));
            _identifier = "ApplicationInOrderSwitchNewVariableApplication";
            offerKey = Service<ICommonService>().GetOfferKeyByTestIdentifier(_identifier, "ApplicationManagementTestID");

            CreateCaseAtStageIfOfferNotAtState(ref offerKey, WorkflowStates.ApplicationManagementWF.LOA, OfferTypeEnum.SwitchLoan, LegalEntityType.NaturalPerson, Products.NewVariableLoan, 1125365, 605000, 95000, 0, 50000, EmploymentType.Salaried, false, false);
            Service<ICommonService>().CommitOfferKeyForTestIdentifier("ApplicationManagementTestID", _identifier, offerKey);
            
            Helper.LOAReceived(browser, offerKey);
        }

        /// <summary>
        /// This test will perform the LOA Accepted action on our set of cases that we want to use to instruct the attorney. These cases should be sent
        /// through to the Application Check state, with the workflow assignment not being affected.
        /// </summary>
        [Test, TestCaseSource(typeof(_06PostCredit), "GetTestCasesForApplicationInOrder"), Description(@"This test will perform the LOA Accepted action on our set of cases that we want to use to instruct the attorney. These cases should
        be sent through to the Application Check state, with the workflow assignment not being affected.")]
        public void _006_LOAAccepted(Automation.DataModels.OriginationTestCase testCase)
        {
                offerKey = Service<ICommonService>().GetOfferKeyByTestIdentifier(testCase.TestIdentifier);
                CreateCaseAtStageIfOfferNotAtState(ref offerKey, WorkflowStates.ApplicationManagementWF.SignedLOAReview, OfferTypeEnum.SwitchLoan, LegalEntityType.NaturalPerson, Products.NewVariableLoan, 1125365, 605000, 95000, 0,50000, EmploymentType.Salaried, false, false);
                Service<ICommonService>().CommitOfferKeyForTestIdentifier("ApplicationManagementTestID", _identifier, offerKey);

                Helper.LOAAccepted(browser, offerKey);
        }

        /// <summary>
        /// This test will instruct the attorney for the set of cases pushed through LOA.
        /// </summary>
        [Test, TestCaseSource(typeof(_06PostCredit), "GetTestCasesForApplicationInOrder"), Description(@"This test will instruct the attorney for the set of cases pushed through LOA.")]
        public void _007_InstructAttorney(Automation.DataModels.OriginationTestCase testCase)
        {
            if (testCase.TestIdentifier == "SubmitApplicationRefinanceVarifixApplication")
            {
                offerKey = Service<ICommonService>().GetOfferKeyByTestIdentifier(testCase.TestIdentifier);
                Helper.InstructAttorney(browser, offerKey);
                //a correspondence record should have been added
                CorrespondenceAssertions.AssertCorrespondenceRecordAdded(offerKey, CorrespondenceReports.AttorneyInstruction, CorrespondenceMedium.Email);
            }
        }

        /// <summary>
        /// This test will ensure that attorney has been correctly instructed once the Instruct Attorney actions has been performed. It ensures that there is a correspondence
        /// record added and that an eWork case has been correctly created in the Assign state in the Pipeline eWork map.
        /// </summary>
        [Test, TestCaseSource(typeof(_06PostCredit), "GetTestCasesForApplicationInOrder"), Description(@"This test will ensure that attorney has been correctly instructed once the Instruct Attorney actions has been performed. It ensures
        that there is a correspondence record added and that an eWork case has been correctly created in the Assign state in the Pipeline eWork map.")]
        public void _008_InstructAttorneyAssertions(Automation.DataModels.OriginationTestCase testCase)
        {
            offerKey = Service<ICommonService>().GetOfferKeyByTestIdentifier(testCase.TestIdentifier);
            int accountKey = base.Service<IApplicationService>().GetOfferAccountKey(offerKey);

            Service<IX2WorkflowService>().WaitForX2State(offerKey, Workflows.ApplicationManagement, WorkflowStates.ApplicationManagementWF.RegistrationPipeline);

            //we need to check for the case being created in the registration pipeline state in X2
            X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.RegistrationPipeline);
            //we need to check that the ework case has been created
            eWorkAssertions.AssertEworkCaseExists(accountKey.ToString(), EworkStages.Assign, EworkMaps.Pipeline);
        }

        /// <summary>
        /// This test will ensure that when we perform the NTU action in the Origination workflow the correct
        /// Ework flag is raised and the case is moved in Ework correctly.
        /// </summary>
        [Test, Description(@"This test will ensure that when we perform the NTU action in the Origination workflow the correct
            Ework flag is raised and the case is moved in Ework correctly.")]
        public void _009_NTUPipeline()
        {
            //fetch a test case
            int offerKey; int accountKey; string reinstateStage;
            Helper.NTUApplicationFromPipeline(browser, out offerKey, out accountKey, out reinstateStage);
        }

        /// <summary>
        /// This test will ensure that if an Ework registration pipeline case is not at an Ework stage where the X2 NTU Advise action
        /// can be performed, ie Cannot NTU in Ework, then the case cannot be NTU'd in X2 via the NTU Pipeline action.
        /// </summary>
        [Test, Description(@"This test will ensure that if an Ework registration pipeline case is not at an Ework stage where the X2 NTU Advise action
        can be performed, ie Cannot NTU in Ework, then the case cannot be NTU'd in X2 via the NTU Pipeline action.")]
        public void _010_NTUPipelineNotAllowed()
        {
            //fetch a test case
            int offerKey = Service<IEWorkService>().GetPipelineCaseWhereActionNotApplied(EworkActions.X2NTUAdvise, WorkflowStates.ApplicationManagementWF.RegistrationPipeline, 0);
            //start the browser
            browser = new TestBrowser(TestUsers.RegistrationsManager);
            browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().WorkFlows(browser);
            browser.Page<WorkflowSuperSearch>().Search(offerKey);
            //navigate to the action
            browser.ClickAction(WorkflowActivities.ApplicationManagement.NTUPipeLine);
            browser.Page<CommonReasonCommonDecline>().SelectReasonAndSubmit(ReasonType.PipelineNTU);
            //a validation message should be displayed
            browser.Page<BasePageAssertions>().AssertValidationMessageExists("Unable to perform EWork Action: X2 NTU ADVISE");
        }

        /// <summary>
        /// This test will ensure that if the related Ework case has not been sent to the Resubmitted stage in the Ework workflow
        /// then the registrations user is not allowed to perform the Resubmit action in the X2 workflow.
        /// </summary>
        [Test, Description(@"This test will ensure that if the related Ework case has not been sent to the Resubmitted stage in the Ework workflow
        then the registrations user is not allowed to perform the Resubmit action in the X2 workflow.")]
        public void _011_ResubmitNotAllowed()
        {
            //fetch a test case
            int offerKey = Service<IEWorkService>().GetPipelineCaseInEworkStage(WorkflowStates.ApplicationManagementWF.RegistrationPipeline, EworkStages.LodgeDocuments, 0);
            //start the browser
            browser = new TestBrowser(TestUsers.RegistrationsManager);
            browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().WorkFlows(browser);
            browser.Page<WorkflowSuperSearch>().Search(offerKey);
            //navigate to the action
            browser.ClickAction(WorkflowActivities.ApplicationManagement.Resubmit);
            browser.Page<CommonReasonCommonDecline>().SelectReasonAndSubmit(ReasonType.Resubmission);
            //a validation message should be displayed
            browser.Page<BasePageAssertions>().AssertValidationMessageExists("Case must be at the 'Resubmitted' stage in Pipeline. It is at Lodge Documents");
        }

        /// <summary>
        /// This test will reinstate by test case back to the Registration Pipeline state, ensuring that the corresponding Ework flag is correctly
        /// raised to move the case in the Ework Pipeline Map.
        /// </summary>
        [Test, Description(@"This test will reinstate the test case back to the Registration Pipeline state, ensuring that the corresponding Ework flag is correctly
        raised to move the case in the Ework Pipeline Map.")]
        public void _012_ReinstatePipelineNTU()
        {
            //fetch a test case
            int offerKey; int accountKey; string reinstateStage;
            Helper.NTUApplicationFromPipeline(browser, out offerKey, out accountKey, out reinstateStage);
            //login as NBPUser
            browser = new TestBrowser(TestUsers.NewBusinessProcessor);
            browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().WorkFlows(browser);
            browser.Page<WorkflowSuperSearch>().Search(offerKey);
            //navigate
            browser.ClickAction(WorkflowActivities.ApplicationManagement.ReinstateNTU);
            browser.Page<WorkflowYesNo>().Confirm(true, false);
            //case has been reinstated in X2
            X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.RegistrationPipeline);
            //case has been reinstated in Ework
            Thread.Sleep(5000);
            eWorkAssertions.AssertEworkCaseExists(accountKey.ToString(), reinstateStage, EworkMaps.Pipeline);
            //offer status has been updated to OPEN
            OfferAssertions.AssertOfferStatus(offerKey, OfferStatusEnum.Open);
            //offer end date has been updated to NULL
            OfferAssertions.AssertOfferEndDate(offerKey, DateTime.Now, 0, true);
        }

        /// <summary>
        /// This test will raise the external activity for an NTU from the Ework Pipeline map that will simulate an NTU occurring
        /// from the Ework Pipeline map.
        /// </summary>
        [Test, Description(@"This test will raise the external activity for an NTU from the Ework Pipeline map that will simulate an NTU occurring
        from the Ework map.")]
        public void _013_PipelineNTUFromEwork()
        {
            int offerKey; int accountKey; string reinstateStage;
            Helper.eWorkNTUApplicationFromPipeline(out offerKey, out accountKey, out reinstateStage);
        }

        /// <summary>
        /// This test will raise the external activity for a reinstated NTU from the Ework Pipeline map that will simulate an NTU occurring
        /// from the Ework Pipeline map.
        /// </summary>
        [Test, Description(@"This test will raise the external activity for a reinstated NTU from the Ework Pipeline map that will simulate an NTU occurring
        from the Ework Pipeline map.")]
        public void _014_ReinstatePipelineNTUFromEwork()
        {
            int offerKey; int accountKey; string reinstateStage;
            Helper.eWorkNTUApplicationFromPipeline(out offerKey, out accountKey, out reinstateStage);
            //raise the flag
            Service<IX2WorkflowService>().PipeLineReinstateNTU(offerKey);
            //update the e-work database
            Service<IEWorkService>().UpdateEworkStage(EworkStages.LodgeDocuments, EworkStages.NTUReview, EworkMaps.Pipeline, accountKey.ToString());
            Thread.Sleep(5000);
            //check that the X2 case has moved
            X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.RegistrationPipeline);
            //check the offer status
            OfferAssertions.AssertOfferStatus(offerKey, OfferStatusEnum.Open);
            //offer end date has been updated to NULL
            OfferAssertions.AssertOfferEndDate(offerKey, DateTime.Now, 0, true);
        }

        /// <summary>
        /// This test will pickup our further lending application and instruct the attorney. By instructing the attorney we are creating a
        /// case in the Registration Pipeline e-work workflow map.
        /// </summary>
        [Test, Description(@"This test will pickup our further lending application and instruct the attorney. By instructing the attorney we are creating a
        case in the Registration Pipeline e-work workflow map.")]
        public void _015_InstructAttorney()
        {
            var results = Service<IX2WorkflowService>().GetOfferKeysAtStateByAdUser(WorkflowStates.ApplicationManagementWF.ApplicationCheck,
                "Registrations Administrator");
            if (results.HasResults)
            {
                var row = (from r in results
                           where r.Column(1).GetValueAs<int>() != (int)OfferTypeEnum.FurtherLoan
                           select r).FirstOrDefault();
                if (row != null)
                {
                    _instructOfferKey = row.Column(0).GetValueAs<int>();
                    _instructAccountKey = row.Column("reservedAccountKey").GetValueAs<int>();
                }
                else
                {
                    _instructOfferKey = 0;
                }
            }

            if (!results.HasResults || _instructOfferKey == 0)
            {
                results.Dispose();
                results = Service<IX2WorkflowService>().GetOfferKeysAtStateByAdUser(WorkflowStates.ApplicationManagementWF.SignedLOAReview, "Registrations LOA Admin");

                var row = (from r in results
                           where r.Column(1).GetValueAs<int>() != (int)OfferTypeEnum.FurtherLoan
                           select r).FirstOrDefault();
                if (row != null)
                {
                    _instructOfferKey = row.Column(0).GetValueAs<int>();
                    _instructAccountKey = row.Column("reservedAccountKey").GetValueAs<int>();
                    Helper.CompleteSignedLOAReview(_instructOfferKey, browser);
                }
                else
                {
                    _instructOfferKey = 0;
                }
            }
            if (_instructOfferKey != 0)
            {
                //we need to login as a registrations admin user
                //start the browser as the rv admin user
                browser = new TestBrowser(TestUsers.RegAdminUser);
                browser.Page<WorkflowSuperSearch>().Search(browser, _instructOfferKey, WorkflowStates.ApplicationManagementWF.ApplicationCheck);
                //we need to select an attorney
                browser.ClickAction(WorkflowActivities.ApplicationManagement.SelectAttorney);
                browser.Page<Orig_SelectAttorney>().SelectAttorneyByDeedsOffice(DeedsOffice.Pietermaritzburg);
                //now we can instruct
                browser.ClickAction(WorkflowActivities.ApplicationManagement.InstructAttorney);
                browser.Page<CorrespondenceProcessing>().SendCorrespondence(CorrespondenceSendMethodEnum.Email);
            }
            else
            {
                Assert.Fail(string.Format(@"No new business case found for test {0}", MethodBase.GetCurrentMethod()));
            }
        }

        /// <summary>
        /// This test asserts that the case from the previous test has been correctly instructed into the Pipeline eWork map.
        /// It ensures that a correspondence record has been added for the Further Loan Attorney Instruction, an ework workflow case has been
        /// created in the Assign state and that the X2 case has been moved to the Registration Pipeline state.
        /// </summary>
        [Test, Description(@"This test asserts that the case from the previous test has been correctly instructed into the Pipeline eWork map.
            It ensures that a correspondence record has been added for the Further Loan Attorney Instruction, an ework workflow case has been
            created in the Assign state and that the X2 case has been moved to the Registration Pipeline state.")]
        public void _016_InstructAttorneyAssertions()
        {
            if (_instructOfferKey == 0)
            {
                Assert.Fail("No offer was found for the Instruct Attorney test.");
            }
            else
            {
                //we need to wait for the ework engine to create the case
                Service<IEWorkService>().WaitForeWorkCaseToCreate(_instructAccountKey, EworkStages.Assign, EworkMaps.Pipeline);
                //we need to check for the case being created in the registration pipeline state in X2
                X2Assertions.AssertCurrentAppManX2State(_instructOfferKey, WorkflowStates.ApplicationManagementWF.RegistrationPipeline);
                //we need to check that the ework case has been created
                eWorkAssertions.AssertEworkCaseExists(_instructAccountKey.ToString(), EworkStages.Assign, EworkMaps.Pipeline);
                //a correspondence record should have been added
                CorrespondenceAssertions.AssertCorrespondenceRecordAdded(_instructOfferKey, CorrespondenceReports.AttorneyInstruction, CorrespondenceMedium.Email);
            }
        }

        /// <summary>
        /// This test ensures that the firing of the Up For Fees flag that Ework calls correctly moves our case out of the Registration Pipeline
        /// state and into the Ready for Disbursement state. This test will update the case directly in the eworks database to be at the stage
        /// directly prior to the Up For Fees action being performed.
        /// </summary>
        [Test, Description(@"This test ensures that the firing of the Up For Fees flag that Ework calls correctly moves our case out of the Registration Pipeline
        state and into the Disbursement state. This test will update the case directly in the ework database to be at the Up For Fees stage.")]
        public void _017_ExtUpForFees()
        {
            _pipelineOfferKey = Service<IEWorkService>().GetPipelineCaseInEworkStage(WorkflowStates.ApplicationManagementWF.RegistrationPipeline,
                 EworkStages.UpForFees, 0);
            //update the e-work database
            _pipelineAccountKey = base.Service<IApplicationService>().GetOfferAccountKey(_pipelineOfferKey);
            //call the x2 procedure to insert the flag
            Service<IX2WorkflowService>().PipelineUpForFees(_pipelineOfferKey);
            //update again to move the case on
            Service<IEWorkService>().UpdateEworkStage(EworkStages.Allocation, EworkStages.UpForFees, EworkMaps.Pipeline, _pipelineAccountKey.ToString());
            //check that the case has been moved to the correct state in X2
            Thread.Sleep(5000);
            X2Assertions.AssertCurrentAppManX2State(_pipelineOfferKey, WorkflowStates.ApplicationManagementWF.Disbursement);
        }

        /// <summary>
        /// This test ensures that the firing of the Complete flag that Ework calls correctly moves our case out of the Disbursement
        /// state and into the Disbursement Review state. This test will update the case directly in the ework database to be at the Prep File stage.
        /// </summary>
        [Test, Description(@"This test ensures that the firing of the Complete flag that Ework calls correctly moves our case out of the Disbursement
        state and into the Disbursement Review state. This test will update the case directly in the ework database to be at the Prep File stage.")]
        public void _018_ExtComplete()
        {
            //update the e-work database
            Service<IEWorkService>().UpdateEworkStage(EworkStages.PrepFile, EworkStages.Allocation, EworkMaps.Pipeline, _pipelineAccountKey.ToString());
            //call the x2 procedure to insert the flag
            Service<IX2WorkflowService>().PipelineComplete(_pipelineOfferKey);
            //update again to move the case on
            Service<IEWorkService>().UpdateEworkStage(EworkStages.PermissionToRegister, EworkStages.PrepFile, EworkMaps.Pipeline, _pipelineAccountKey.ToString());
            //check that the external activity fired
            Thread.Sleep(5000);
            X2Assertions.AssertCurrentAppManX2State(_pipelineOfferKey, WorkflowStates.ApplicationManagementWF.DisbursementReview);
        }

        /// <summary>
        /// This test ensures that the firing of the Held Over flag that Ework calls correctly moves our case out of the Disbursement
        /// Review state and into the Disbursement state.
        /// </summary>
        [Test, Description(@"This test ensures that the firing of the Held Over flag that Ework calls correctly moves our case out of the Disbursement
        Review state and into the Disbursement state.")]
        public void _019_ExtHeldOver()
        {
            //insert the flag
            Service<IX2WorkflowService>().PipeLineHeldOver(_pipelineOfferKey);
            //check that the external activity fired
            Thread.Sleep(5000);
            X2Assertions.AssertCurrentAppManX2State(_pipelineOfferKey, WorkflowStates.ApplicationManagementWF.Disbursement);
        }

        /// <summary>
        /// A registrations user can move a case to the Disbursement Review state by performing the Review Disbursement Setup action
        /// that is applied at the Disbursement state
        /// </summary>
        [Test, Description(@"A registrations user can move a case to the Disbursement Review state by performing the Review Disbursement Setup action
        that is applied at the Disbursement state")]
        public void _020_ReviewDisbursementSetup()
        {
            //load the case
            //we need to login as a registrations admin user
            browser = new TestBrowser(TestUsers.RegAdminUser);
            browser.Page<WorkflowSuperSearch>().Search(browser, _pipelineOfferKey, WorkflowStates.ApplicationManagementWF.Disbursement);
            //perform the action
            browser.ClickAction(WorkflowActivities.ApplicationManagement.ReviewDisbursementSetup);
            browser.Page<WorkflowYesNo>().Confirm(true, false);
            //check if it has moved states
            X2Assertions.AssertCurrentAppManX2State(_pipelineOfferKey, WorkflowStates.ApplicationManagementWF.DisbursementReview);
        }

        /// <summary>
        /// Performing the Disbursment Incorrect action will push the case back to the Disbursement state by performing the
        /// Disbursement Incorrect action.
        /// </summary>
        [Test, Description(@"Performing the Disbursement Incorrect action will push the case back to the Disbursement state by performing the
        Disbursement Incorrect action.")]
        public void _021_DisbursementIncorrect()
        {
            //we need to login as a registrations admin user
            browser = new TestBrowser(TestUsers.RegistrationsSupervisor);
            browser.Page<WorkflowSuperSearch>().Search(browser, _pipelineOfferKey, WorkflowStates.ApplicationManagementWF.DisbursementReview);
            //perform the action
            browser.ClickAction(WorkflowActivities.ApplicationManagement.DisbursementIncorrect);
            browser.Page<WorkflowYesNo>().Confirm(true, false);
            //check if it has moved states
            X2Assertions.AssertCurrentAppManX2State(_pipelineOfferKey, WorkflowStates.ApplicationManagementWF.Disbursement);
        }

        /// <summary>
        ///  This test will perform the held over action from the Disbursement Review state ensuring that the case moves back to the Disbursement state.
        /// </summary>
        [Test, Description(@"This test will perform the held over action from the Disbursement Review state ensuring that the case moves back to the Disbursement state.")]
        public void _022_HeldOver()
        {
            var offerKeys = Service<IX2WorkflowService>().GetOffersAtState(WorkflowStates.ApplicationManagementWF.DisbursementReview, Workflows.ApplicationManagement, "");
            if (offerKeys.Count > 0)
            {
                var offerKey = offerKeys[0];
                //login as consultant
                browser = new TestBrowser(TestUsers.RegistrationsSupervisor);
                browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.ApplicationManagementWF.DisbursementReview);
                browser.ClickAction(WorkflowActivities.ApplicationManagement.HeldOver);
                browser.Page<WorkflowYesNo>().Confirm(true, false);
                //case has moved back to Disbursement
                X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.Disbursement);
            }
            else
            {
                Assert.Fail("No test cases at Disbursement Review for this test.");
            }
        }

        /// <summary>
        /// This test loads up a case at the Registration Pipeline state and will resend the attorney instruction.
        /// </summary>
        [Test, Description("This test loads up a case at the Registration Pipeline state and will resend the attorney instruction.")]
        public void _023_ResendInstruction()
        {
            var offerKeys = Service<IX2WorkflowService>().GetOffersAtState(WorkflowStates.ApplicationManagementWF.RegistrationPipeline, Workflows.ApplicationManagement, "");
            if (offerKeys.Count > 0)
            {
                var offerKey = offerKeys[0];
                //login as consultant
                browser = new TestBrowser(TestUsers.RegistrationsSupervisor);
                browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.ApplicationManagementWF.RegistrationPipeline);
                browser.ClickAction(WorkflowActivities.ApplicationManagement.ResendInstruction);
                browser.Page<ResendInstruction>().SendInstruction();
                Thread.Sleep(5000);

                browser.Page<CorrespondenceProcessing>().SendCorrespondence(CorrespondenceSendMethodEnum.Email);
                var offerTypeKey = Service<IApplicationService>().GetOfferData(offerKey).Rows(0).Column("OfferTypeKey").GetValueAs<int>();
                string correspondenceReport = string.Empty;
                switch (offerTypeKey)
                {
                    case (int)OfferTypeEnum.FurtherLoan:
                        correspondenceReport = CorrespondenceReports.AttorneyFurtherLoanInstruction;
                        break;
                    case (int)OfferTypeEnum.NewPurchase:
                    case (int)OfferTypeEnum.Refinance:
                    case (int)OfferTypeEnum.SwitchLoan:
                        correspondenceReport = CorrespondenceReports.AttorneyInstruction;
                        break;
                    default:
                        break;
                }
                CorrespondenceAssertions.AssertCorrespondenceRecordAdded(offerKey, correspondenceReport, CorrespondenceMedium.Email);
            }
        }

        /// <summary>
        /// This test will change the attorney on the case and then send the instruction to the new attorney.
        /// </summary>
        [Test, Description("This test will change the attorney on the case and then send the instruction to the new attorney.")]
        public void _024_ResendInstructionToNewAttorney()
        {
            var offerKeys = Service<IX2WorkflowService>().GetOffersAtState(WorkflowStates.ApplicationManagementWF.RegistrationPipeline, Workflows.ApplicationManagement, "");
            if (offerKeys.Count > 0)
            {
                var offerKey = offerKeys[0];
                //login as consultant
                browser = new TestBrowser(TestUsers.RegistrationsSupervisor);
                browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.ApplicationManagementWF.RegistrationPipeline);
                browser.ClickAction(WorkflowActivities.ApplicationManagement.ResendInstruction);
                var newAttorney = browser.Page<ResendInstruction>().ChangeAttorney();
                var attorneyName = (from att in newAttorney select att.Value).FirstOrDefault();
                var attorneyKey = (from att in newAttorney select att.Key).FirstOrDefault();
                browser.Page<ResendInstruction>().SendInstruction();
                //we need to check the attorney is displayed on the Correspondence screen.
                browser.Page<BasePageAssertions>().AssertFrameContainsText(attorneyName);
                var attorneyDetails = base.Service<IAttorneyService>().GetAttorneyByKey(attorneyKey);
                //check the offer role has changed
                OfferAssertions.AssertOfferRoleExists(attorneyDetails.Column("LegalEntityKey").GetValueAs<int>(), offerKey, GeneralStatusEnum.Active, true);
                browser.Page<CorrespondenceProcessing>().SendCorrespondence(CorrespondenceSendMethodEnum.Email);
                var offerTypeKey = Service<IApplicationService>().GetOfferData(offerKey).Rows(0).Column("OfferTypeKey").GetValueAs<int>();
                string correspondenceReport = string.Empty;
                switch (offerTypeKey)
                {
                    case (int)OfferTypeEnum.FurtherLoan:
                        correspondenceReport = CorrespondenceReports.AttorneyFurtherLoanInstruction;
                        break;
                    case (int)OfferTypeEnum.NewPurchase:
                    case (int)OfferTypeEnum.Refinance:
                    case (int)OfferTypeEnum.SwitchLoan:
                        correspondenceReport = CorrespondenceReports.AttorneyInstruction;
                        break;
                    default:
                        break;
                }
                CorrespondenceAssertions.AssertCorrespondenceRecordAdded(offerKey, correspondenceReport, CorrespondenceMedium.Email);
            }
        }

        /// <summary>
        /// This test will ensure that when we perform the NTU action in the Origination workflow the correct
        /// Ework flag is raised and the case is moved in Ework correctly.
        /// </summary>
        [Test, Description(@"This test will ensure that when we perform the NTU action in the Origination workflow the correct
            Ework flag is raised and the case is moved in Ework correctly.")]
        public void _025_NTUPipelineFollowedByNTUTimeout()
        {
            //fetch a test case
            int offerKey; int accountKey; string reinstateStage;
            Helper.NTUApplicationFromPipeline(browser, out offerKey, out accountKey, out reinstateStage);
            Helper.NTUTimeoutFromPipeline(base.scriptEngine, offerKey, accountKey);
        }

        #endregion Tests

        #region Helpers

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

        private void CreateCaseAtStageIfOfferkeyEmpty(ref int offerKey, string stateName, OfferTypeEnum offerType, string legalEntityType, string product,
            int marketValue, int existingLoan, int cashOut, int cashDeposit, int houseHoldIncome, string employmentType,
            bool interestOnly, bool capitaliseFees)
        {
            if (offerKey == 0 || offerKey == null)
            {
                browser = new TestBrowser(TestUsers.BranchConsultant, TestUsers.Password);
                browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(browser);
                browser.Navigate<BuildingBlocks.Navigation.CalculatorsNode>().Calculators(browser, CalculatorNodeTypeEnum.ApplicationCalculator);

                //Create Offer via Calculator
                switch (offerType)
                {
                    case OfferTypeEnum.NewPurchase:
                        browser.Page<Views.LoanCalculator>().LoanCalculatorLead_NewPurchase(product.ToString(), marketValue.ToString(), cashDeposit.ToString(),
                            employmentType.ToString(), "240", houseHoldIncome.ToString(),
                            ButtonTypeEnum.CreateApplication);
                        break;

                    case OfferTypeEnum.SwitchLoan:
                        browser.Page<Views.LoanCalculator>().LoanCalculatorLead_Switch(product.ToString(), marketValue.ToString(), existingLoan.ToString(),
                            cashOut.ToString(), employmentType.ToString(), "240",  Convert.ToBoolean(capitaliseFees),
                            houseHoldIncome.ToString(), ButtonTypeEnum.CreateApplication);
                        break;

                    case OfferTypeEnum.Refinance:
                        browser.Page<Views.LoanCalculator>().LoanCalculatorLead_Refinance(product.ToString(), marketValue.ToString(), cashOut.ToString(),
                            employmentType.ToString(), "240", Convert.ToBoolean(capitaliseFees), houseHoldIncome.ToString(), ButtonTypeEnum.CreateApplication);
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

                offerKey = browser.Page<ApplicationSummaryBase>().GetOfferKey();
                browser.Dispose();
                browser = null;

                //Push offer to correct State

                scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.ApplicationCapture, "SubmitApplication", offerKey);

                Service<IX2WorkflowService>().WaitForAppManCaseCreate(offerKey);

                if (offerType == OfferTypeEnum.NewPurchase)
                {
                    MoveCaseFromQAtoManageApplication(offerKey);
                }

                Helper.ProcessFromManageApplication(base.scriptEngine, offerKey, offerType, marketValue.ToString());

                var results = Service<IX2WorkflowService>().GetCreditInstanceDetails(offerKey);
                string creditState = string.Empty;
                if (results.HasResults)
                {
                    creditState = results.Rows(0).Column("StateName").Value;
                    Logger.LogAction("Offer is at the {0} state of the {1} workflow", creditState, Workflows.Credit);
                }
                else
                {
                    Logger.LogAction("Offer is not in the {0} workflow", Workflows.Credit);
                }
                //Process the application if it is at the ValuatonApprovalRequired state
                if (creditState == WorkflowStates.CreditWF.ValuationApprovalRequired)
                {
                    Logger.LogAction("Processing Offer at {0} state of the {1} workflow", WorkflowStates.CreditWF.ValuationApprovalRequired, Workflows.Credit);

                    Helper.ProcessFromValuationApprovalRequired(base.scriptEngine, offerKey, marketValue.ToString());
                }
                results.Dispose();

                results = Service<IX2WorkflowService>().GetCreditInstanceDetails(offerKey);
                if (results.HasResults && (stateName == WorkflowStates.ApplicationManagementWF.LOA || stateName == WorkflowAutomationScripts.ApplicationManagement.LOAReceived || 
                    stateName == WorkflowStates.ApplicationManagementWF.LOAQuery || stateName == WorkflowStates.ApplicationManagementWF.SignedLOAReview))
                {
                    creditState = string.Empty;
                    creditState = results.Rows(0).Column("StateName").Value;

                    if (creditState == WorkflowStates.CreditWF.Credit)
                    {
                        scriptEngine.ExecuteScript(WorkflowEnum.Credit, WorkflowAutomationScripts.Credit.ConfirmApplicationEmployment, offerKey);
                        scriptEngine.ExecuteScript(WorkflowEnum.Credit, WorkflowAutomationScripts.Credit.ApproveApplication, offerKey);
                        Service<IX2WorkflowService>().WaitForX2State(offerKey, Workflows.ApplicationManagement, WorkflowStates.ApplicationManagementWF.LOA);
                    }
                }
                results.Dispose();

                results = Service<IX2WorkflowService>().GetAppManInstanceDetails(offerKey);
                if (results.HasResults && (stateName == WorkflowStates.ApplicationManagementWF.SignedLOAReview || stateName == WorkflowStates.ApplicationManagementWF.LOAQuery))
                {
                    var appManState = results.Rows(0).Column("StateName").Value;

                    if (appManState == WorkflowStates.ApplicationManagementWF.LOA)
                    {
                        scriptEngine.ExecuteScript(WorkflowEnum.ApplicationManagement, WorkflowAutomationScripts.ApplicationManagement.LOAReceived, offerKey);
                        Service<IX2WorkflowService>().WaitForX2State(offerKey, Workflows.ApplicationManagement, WorkflowStates.ApplicationManagementWF.SignedLOAReview);
                    }
                }
                results.Dispose();

                results = Service<IX2WorkflowService>().GetAppManInstanceDetails(offerKey);
                if (results.HasResults && stateName == WorkflowStates.ApplicationManagementWF.LOAQuery)
                {
                    var appManState = results.Rows(0).Column("StateName").Value;

                    if (appManState == WorkflowStates.ApplicationManagementWF.SignedLOAReview)
                    {
                        scriptEngine.ExecuteScript(WorkflowEnum.ApplicationManagement, WorkflowAutomationScripts.ApplicationManagement.QueryOnLOA, offerKey);
                        Service<IX2WorkflowService>().WaitForX2State(offerKey, Workflows.ApplicationManagement, WorkflowStates.ApplicationManagementWF.LOAQuery);
                    }
                }
                results.Dispose();
            }
        }

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

                scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.ApplicationManagement, "RequestResolved", offerKey);
            }

            //Process the application if it is at the QA state
            if (appManState == WorkflowStates.ApplicationManagementWF.QA || appManState == WorkflowStates.ApplicationManagementWF.RequestatQA)
            {
                Logger.LogAction("Processing Offer at {0} state of the {1} workflow", WorkflowStates.ApplicationManagementWF.QA, Workflows.ApplicationManagement);

                scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.ApplicationManagement, "QAComplete", offerKey);

                var instanceID = Service<IX2WorkflowService>().GetAppManInstanceIDByOfferKey(offerKey);
                Service<IX2WorkflowService>().WaitForX2WorkflowHistoryActivity(instanceID, 1, "", WorkflowActivities.ApplicationManagement.NewPurchase, WorkflowActivities.ApplicationManagement.OtherTypes);
            }
            results = Service<IX2WorkflowService>().GetAppManInstanceDetails(offerKey);
            appManState = results.Rows(0).Column("StateName").Value;

            //Process the application if it is at the Issue AIP state
            if (appManState == WorkflowStates.ApplicationManagementWF.IssueAIP)
            {
                Logger.LogAction("Processing Offer at {0} state of the {1} workflow", WorkflowStates.ApplicationManagementWF.IssueAIP, Workflows.ApplicationManagement);

                scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.ApplicationManagement, "ClientAccepts", offerKey);
            }
        }
        #endregion Helpers
    }
}