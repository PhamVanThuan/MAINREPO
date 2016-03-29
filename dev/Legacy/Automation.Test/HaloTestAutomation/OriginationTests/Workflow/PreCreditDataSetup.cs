using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.Origination;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WorkflowAutomation.Harness;
using Navigation = BuildingBlocks.Navigation;

namespace Origination.Workflow
{
    [TestFixture, RequiresSTA]
    public class _01PreCreditDataSetup : OriginationTestBase<BasePage>
    {
        private TestBrowser browser;

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
                    browser.Dispose();
                    browser = null;
                }
            }
        }

        public IEnumerable<Automation.DataModels.OriginationTestCase> GetApplicationsForSubmission()
        {
            List<string> testGroups = new List<string> { "SubmitApplication", "ManagerSubmitApplication" };
            var testCases = Service<ICommonService>().GetOriginationTestCases();
            return (from t in testCases where testGroups.Contains(t.TestGroup) select t);
        }

        [Test, TestCaseSource(typeof(_01PreCreditDataSetup), "GetApplicationsForSubmission"), Description(@"Verify that a Branch Consultant can perform the 'Submit Application' action at 'Application Capture' state,
                                which moves the case to the next state depending on the LoanType")]
        public void _01_SubmitApplication(Automation.DataModels.OriginationTestCase testCase)
        {
            IX2ScriptEngine scriptEngine = new X2ScriptEngine();
            var offerKey = Service<ICommonService>().GetOfferKeyByTestIdentifier(testCase.TestIdentifier);
            Assert.AreNotEqual(0, offerKey, "Run application capture tests, no offer created for TestIdentifier {0}", testCase.TestIdentifier);
            if (Service<IX2WorkflowService>().OfferExistsAtState(offerKey, WorkflowStates.ApplicationCaptureWF.ApplicationCapture))
            {
                var results = scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.ApplicationCapture, "SubmitApplication", offerKey);
                foreach (var returnData in results)
                {
                    Assert.True(returnData.Value.ActivityCompleted, "Failed to Submit Application {0}", offerKey);
                }	 
            }
        }

        [Test, TestCaseSource(typeof(_01PreCreditDataSetup), "GetApplicationsForSubmission"), Description("")]
        public void _02_InManageApplicationCheck(Automation.DataModels.OriginationTestCase testCase)
        {
            int offerKey = Service<ICommonService>().GetOfferKeyByTestIdentifier(testCase.ApplicationManagementTestID, "ApplicationManagementTestID");

            if (testCase.LoanType == "New purchase")
            {
                X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.QA);

                ////Assert QA Administrator D offerrole exists
                AssignmentAssertions.AssertOfferRoleRecordExists(offerKey, OfferRoleTypeEnum.QAAdministratorD);
                AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(offerKey, OfferRoleTypeEnum.QAAdministratorD);
                AssignmentAssertions.AssertThatAWorkFlowAssignmentRecordExists(offerKey, OfferRoleTypeEnum.QAAdministratorD);
                AssignmentAssertions.AssertThatTheWorkFlowAssignmentRecordIsActive(offerKey, OfferRoleTypeEnum.QAAdministratorD);
            }
            else
            {
                X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.ManageApplication);
                //a valuations clone should not exist for a switch or refinance application
                X2Assertions.AssertX2CloneDoesNotExist(offerKey, WorkflowStates.ApplicationManagementWF.ValuationHold, Workflows.ApplicationManagement);
                ////Assert New Business Processor D offerrole exists
                AssignmentAssertions.AssertOfferRoleRecordExists(offerKey, OfferRoleTypeEnum.NewBusinessProcessorD);
                AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(offerKey, OfferRoleTypeEnum.NewBusinessProcessorD);
                AssignmentAssertions.AssertThatAWorkFlowAssignmentRecordExists(offerKey, OfferRoleTypeEnum.NewBusinessProcessorD);
                AssignmentAssertions.AssertThatTheWorkFlowAssignmentRecordIsActive(offerKey, OfferRoleTypeEnum.NewBusinessProcessorD);
            }
        }

        /// <summary>
        /// Batch reassign offers from a New Business Processors to our SAHL\NBPUser test user. This test will only run if the new business processor
        /// does not have the number of cases required for the test
        /// </summary>
        [Test, Description("Batch reassign offers from a New Business Processor to sahl\nbpuser")]
        public void _03_BatchReassign()
        {
            Console.WriteLine(string.Format(@"--********{0}********--", MethodBase.GetCurrentMethod()));
            int noOfCases = 20;
            int currentCases = Service<IX2WorkflowService>().GetCountofCasesForUser(WorkflowStates.ApplicationManagementWF.ManageApplication, Workflows.ApplicationManagement,
                TestUsers.NewBusinessProcessor);
            if (currentCases < noOfCases)
            {
                int requiredCases = noOfCases - currentCases;
                browser = new TestBrowser(TestUsers.NewBusinessManager, TestUsers.Password);
                browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().WorkFlows(browser);
                browser.Navigate<Navigation.WorkFlowsNode>().BatchReassign(browser);
                browser.Page<WorkflowBatchReassign>().BatchReassign("New Business Processor", TestUsers.NewBusinessProcessor, requiredCases, Workflows.ApplicationManagement);
            }
        }
    }
}