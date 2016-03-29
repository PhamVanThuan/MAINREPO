using Automation.DataModels;
using BuildingBlocks;
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
using WatiN.Core.Logging;
using Navigation = BuildingBlocks.Navigation;

namespace Origination.Workflow
{
    [TestFixture, RequiresSTA]
    public class _03CreditDataSetup : OriginationTestBase<BasePage>
    {
        private TestBrowser _browser;

        protected override void OnTestTearDown()
        {
            base.OnTestTearDown();
            if (_browser != null)
            {   
                _browser.Page<BasePage>().CheckForErrorMessages();
                _browser.Dispose();
            }
        }

        public IEnumerable<OriginationTestCase> GetCasesInCredit()
        {
            var testCases = Service<ICommonService>().GetOriginationTestCases();
            return (from t in testCases where !string.IsNullOrEmpty(t.CreditTestID) select t);
        }

        /// <summary>
        /// Process Applications at ValuationApprovalRequired and Application Management state so that they can be moved to the Credit state in the Credit workflow map.
        /// Complete the Application in Order action to move the application to the Credit state in the Credit Workflow
        /// </summary>
        [Test, TestCaseSource(typeof(_03CreditDataSetup), "GetCasesInCredit"), Description(@"Process Applications at ValuationApprovalRequired and Application Management state so that they can be moved to the Credit state
        in the Credit workflow map. Complete the Application in Order action to move the application to the Credit state in the Credit Workflow")]
        public void _001_CreditSubmissionCheck(Automation.DataModels.OriginationTestCase testCase)
        {
            Console.WriteLine(string.Format(@"--********{0}********--", MethodBase.GetCurrentMethod()));
            int offerKey = Service<ICommonService>().GetOfferKeyFromTestSchemaTable("OffersAtApplicationCapture", "TestIdentifier", testCase.TestIdentifier);
            Logger.LogAction("Getting the Instance details for Offer {0} in the Application Management workflow", offerKey);
            
            var results = Service<IX2WorkflowService>().GetAppManInstanceDetails(offerKey);
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
            //Process the application if it is at the ManageApplication state
            if (appManState == WorkflowStates.ApplicationManagementWF.QA)
            {
                Logger.LogAction("Processing Offer at {0} state of the {1} workflow", WorkflowStates.ApplicationManagementWF.QA, Workflows.ApplicationManagement);
                Helper.QAComplete(_browser, offerKey); 
                Helper.AcceptAIP(_browser, TestUsers.BranchConsultant1,offerKey);
            }
            if (appManState == WorkflowStates.ApplicationManagementWF.ManageApplication || appManState == WorkflowStates.ApplicationManagementWF.QA)
            {
                Logger.LogAction("Processing Offer at {0} state of the {1} workflow", WorkflowStates.ApplicationManagementWF.ManageApplication, Workflows.ApplicationManagement);
                OfferTypeEnum offerType;
                switch (testCase.LoanType)
                {
                    case "Switch Loan":
                    case "Switch loan":
                        {
                            offerType = OfferTypeEnum.SwitchLoan;
                            break;
                        }
                    case "New purchase":
                        {
                            offerType = OfferTypeEnum.NewPurchase;
                            break;
                        }
                    case "Refinance":
                        {
                            offerType = OfferTypeEnum.Refinance;
                            break;
                        }
                    default:
                        offerType = new OfferTypeEnum();
                        break;
                }
                Helper.ProcessFromManageApplication(base.scriptEngine, offerKey, offerType, testCase.MarketValue);
            }

            //Get the Credit state
            results = Service<IX2WorkflowService>().GetCreditInstanceDetails(offerKey);
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

                Helper.ProcessFromValuationApprovalRequired(base.scriptEngine, offerKey, testCase.MarketValue);
            }
            results.Dispose();
        }

        /// <summary>
        /// This test will attempt to assign the specified number of cases from existing business credit users to our test credit users.
        /// </summary>
        [Test, Sequential, Description("This test will attempt to assign the specified number of cases from existing business credit users to our test credit users.")]
        public void _002_BatchReassignExistingCases(
            [Values("Credit Underwriter", "Credit Supervisor")] string role,
            [Values(TestUsers.CreditUnderwriter, TestUsers.CreditSupervisor)] string userName,
            [Values(TestUsers.CreditManager, TestUsers.CreditManager)] string loginUser)
        {
            Console.WriteLine(string.Format(@"--********{0}********--", MethodBase.GetCurrentMethod()));
            int noOfCases = 15;
            int currentCases = Service<IX2WorkflowService>().GetCountofCasesForUser(WorkflowStates.CreditWF.Credit, Workflows.Credit,
                userName);
            if (currentCases < noOfCases)
            {
                int requiredCases = noOfCases - currentCases;
                //login as a user who can access the batch reassign
                _browser = new TestBrowser(loginUser, TestUsers.Password);
                _browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().WorkFlows(_browser);
                _browser.Navigate<Navigation.WorkFlowsNode>().BatchReassign(_browser);
                _browser.Page<WorkflowBatchReassign>().BatchReassign(role, userName, requiredCases, Workflows.ApplicationManagement);
            }
        }
    }
}