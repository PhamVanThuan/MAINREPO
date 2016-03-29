using System;
using System.Collections.Generic;
using WatiN.Core;
using WatiN.Core.Logging;
using NUnit.Framework;
using BuildingBlocks;
using BuildingBlocks.Constants;
using SQLQuerying;
using SQLQuerying.DataHelper;
using BuildingBlocks.Enums;

namespace ApplicationManagementTests
{
    [TestFixture, RequiresSTA]
    [Ignore]
    class Andrew_s_workfile
    {
        Browser browser;
        System.IO.StreamWriter _ConsoleWriter;
        Dictionary<string, string> OfferKeys = new Dictionary<string,string>();

        [TestFixtureSetUp]
        public void TestSuiteStartUp()
        {
            Logger.LogWriter = new ConsoleLogWriter();
            //_ConsoleWriter = new System.IO.StreamWriter(@"TestLog.txt");
            //Console.SetOut(_ConsoleWriter);
            IDNumbers.CleanIDNumbers();
        }

        [TestFixtureTearDown]
        public void TestSuiteCleanUp()
        {
            //_ConsoleWriter.Close();
        }

        [SetUp]
        public void TestStartUp()
        {
            Common.CloseAllOpenIEBrowsers();
        }

        //[TearDown]
        public void TestCleanUp()
        {
            if (browser != null)
            {
                try
                {
                    Navigation.Base.CheckForErrorMessages(browser);
                }
                finally
                {
                    browser.Dispose();
                    browser = null;
                }
            }
        }

        #region TestSetupData

        [Test, Description("Data setup test for the NTU Timer on an Application")]
        public void _001_NTUTimeout()
        {
            //Open browser and login to Halo
            string Username = @"sahl\nbpuser1";
            browser = Navigation.Base.StartBrowser(Username, "Natal1");
            Navigation.Base.CloseLoanNodes(browser);

            //Begin work
            //Navigation.Base.Task(browser);
            Navigation.WorkFlowsNode.WorkFlows(browser);
            string OfferKey = Helper.GetOfferKeyByTestIdentifier("NTUTimeout", "ApplicationManagementTestID");
            Views.WorkflowSuperSearch.SearchByOfferKey(browser, OfferKey, "Manage Application");
            Navigation.ApplicationManagementWorkFlow.NTU(browser);
            Views.CommonReasonCommonDecline.SelectReasonAndSubmit(browser, "Application NTU");
            
            //Update the Scheduled Activity Time
            SQLQuerying.DataHelper._2AM db = new SQLQuerying.DataHelper._2AM(AppMan.Default.SAHLDataBaseServer);
            db.UpdateScheduledActivityTime(OfferKey, SQLQuerying.DataHelper._2AM.WorkflowMap.Application_Management, "5", "NTU Timeout");
            
            //see _004_NTUTimeout() method in the ApplicationManagementWorkflowTests #region for the Assertion steps
        }

        [Test, Description("Data setup test for the Decline Timer on an Application")]
        public void _002_DeclineTimeoutApplication()
        {
            string OfferKey = Helper.GetOfferKeyByTestIdentifier("DeclineTimeoutApplication", "ApplicationManagementTestID");
            string UserName = @"sahl\nbpuser"; // OffersAtApplicationCaptureCSV.GetValue("Username", index);
            string Password = "Natal1"; // OffersAtApplicationCaptureCSV.GetValue("Password", index);
            browser = Navigation.Base.StartBrowser(UserName, Password);
            Navigation.Base.CloseLoanNodes(browser);
            Navigation.WorkFlowsNode.WorkFlows(browser);
            Views.WorkflowSuperSearch.SearchByOfferKey(browser, OfferKey, "Manage Application");
            //Decline the Application
            Navigation.ApplicationManagementWorkFlow.Decline(browser);
            Views.CommonReasonCommonDecline.SelectReasonAndSubmit(browser, "Administrative Decline");
            //Update the Scheduled Activity Time
            SQLQuerying.DataHelper._2AM db = new SQLQuerying.DataHelper._2AM(AppMan.Default.SAHLDataBaseServer);
            db.UpdateScheduledActivityTime(OfferKey, SQLQuerying.DataHelper._2AM.WorkflowMap.Application_Capture, "1", "Decline Timeout");
        }

        #endregion

        #region ApplicationManagementWorkflowTests

        [Test, Description("Verify that a New Business Processor can move a case from 'Manage Application' to the 'NTU' state by performing the NTU action")]
        public void _001_NTUOfferAtManageApplication()
        {
            //Open browser and login to Halo
            string Username = @"sahl\nbpuser1";
            browser = Navigation.Base.StartBrowser(Username, "Natal1");
            Navigation.Base.CloseLoanNodes(browser);
            
            //Begin work
            //Navigation.Base.Task(browser);
            Navigation.WorkFlowsNode.WorkFlows(browser);
            string OfferKey = Helper.GetOfferKeyByTestIdentifier("NTUOfferAtManageApplication", "ApplicationManagementTestID");
            Views.WorkflowSuperSearch.SearchByOfferKey(browser, OfferKey, "Manage Application");
            Navigation.ApplicationManagementWorkFlow.NTU(browser);
            Views.CommonReasonCommonDecline.SelectReasonAndSubmit(browser, "Application NTU");

            //Assertions
            Assertions.AssertOfferStatus(OfferKey, OfferStatus.NTU);
            Assertions.AssertOfferEndDate(OfferKey, DateTime.Now, 0, false);
            Assertions.CaseCreation.AssertOfferExistsOnWorkList(browser, OfferKey, WorkflowStates.ApplicationManagementWF.NTU);
        }

        [Test, Description("Verify that a New Business Processor can  move a case at 'NTU' state to 'Manage Application' state, by performing the 'Reinstate NTU' action")]
        public void _002_ReinstateNTU()
        {
            //Open browser and login to Halo
            string Username = @"sahl\nbpuser1";
            browser = Navigation.Base.StartBrowser(Username, "Natal1");
            Navigation.Base.CloseLoanNodes(browser);

            //Begin work
            //Navigation.Base.Task(browser);
            Navigation.WorkFlowsNode.WorkFlows(browser);
            string OfferKey = Helper.GetOfferKeyByTestIdentifier("ReinstateNTU", "ApplicationManagementTestID");
            Views.WorkflowSuperSearch.SearchByOfferKey(browser, OfferKey, "Manage Application");
            Navigation.ApplicationManagementWorkFlow.NTU(browser);
            Views.CommonReasonCommonDecline.SelectReasonAndSubmit(browser, "Application NTU");
            //Reactivate NTU'd Application
            Navigation.NTU.ReinstateNTU(browser);
            Views.WorkflowYesNo.Confirm(browser, true);

            //Asserions
            Assertions.AssertOfferStatus(OfferKey, OfferStatus.Open);
            Assertions.AssertOfferEndDate(OfferKey, DateTime.Now, 0, true);
            Assertions.CaseCreation.AssertOfferExistsOnWorkList(browser, OfferKey, WorkflowStates.ApplicationManagementWF.ManageApplication);
        }

        [Test, Description("Verify that a New Business Processor can  move a case at 'NTU' state to 'Archive NTU' state, by performing the 'NTU Finalised' action")]
        public void _003_NTUFinalised()
        {
            //Open browser and login to Halo
            string Username = @"sahl\nbpuser1";
            browser = Navigation.Base.StartBrowser(Username, "Natal1");
            Navigation.Base.CloseLoanNodes(browser);

            //Begin work
            //Navigation.Base.Task(browser);
            Navigation.WorkFlowsNode.WorkFlows(browser);
            string OfferKey = Helper.GetOfferKeyByTestIdentifier("NTUFinalised", "ApplicationManagementTestID");
            Views.WorkflowSuperSearch.SearchByOfferKey(browser, OfferKey, "Manage Application");
            Navigation.ApplicationManagementWorkFlow.NTU(browser);
            Views.CommonReasonCommonDecline.SelectReasonAndSubmit(browser, "Application NTU");
            //NTU Finalise Application
            Navigation.NTU.NTUFinalised(browser);
            Views.WorkflowYesNo.Confirm(browser, true);

            //Assertions
            Assertions.AssertOfferStatus(OfferKey, OfferStatus.NTU);
            Assertions.AssertOfferEndDate(OfferKey, DateTime.Now, 0, false);
            Assertions.AssertCurrentAppManX2State(OfferKey, "Archive NTU");
        }

        [Test, Description("Verify that the NTU Timer correctly archives a case from the NTU state")]
        public void _004_NTUTimeout()
        {
            //see _001_NTUTimeout() method in the TestSetupData #region for the preliminary test steps
            string OfferKey = Helper.GetOfferKeyByTestIdentifier("NTUTimeout", "ApplicationManagementTestID");

            Assertions.AssertCurrentAppManX2State(OfferKey, "Archive NTU");
            //Assert the end date has been populated
            Assertions.AssertOfferEndDate(OfferKey, DateTime.Now, 0, false);
            //Assert the offer status is correct
            Assertions.AssertOfferStatus(OfferKey, OfferStatus.NTU);
        }

        [Test, Description("Verify that a New Business Processor can Decline an Application at 'Manage Application' state")]
        public void _007_DeclineApplicationConsultant()
        {
            string TestUser = @"sahl\nbpuser"; // OffersAtApplicationCaptureCSV.GetValue("Username", index);
            string Password = "Natal1"; // OffersAtApplicationCaptureCSV.GetValue("Password", index);

            browser = Navigation.Base.StartBrowser(TestUser, Password);
            Navigation.Base.CloseLoanNodes(browser);

            string OfferKey = Helper.GetOfferKeyByTestIdentifier("DeclineApplicationConsultant", "ApplicationManagementTestID");

            //Step 1: Select offer from worklist
            Navigation.WorkFlowsNode.WorkFlows(browser);
            Views.WorkflowSuperSearch.SearchByOfferKey(browser, OfferKey, "Manage Application");
            //Step 2: Perform the action
            Navigation.ApplicationManagementWorkFlow.Decline(browser);
            //Step 3: Decline the case
            Views.CommonReasonCommonDecline.SelectReasonAndSubmit(browser, "Administrative Decline");
            browser.WaitForComplete();
            //Step 4: Assert that the workflow case is the Decline State, the OfferStatus = Decline and the OfferEndDate is set
            Assertions.AssertOfferStatus(OfferKey, OfferStatus.Declined);
            //Assert that the OfferEndDate has been populated
            DateTime ActionDate = DateTime.Now;
            Assertions.AssertOfferEndDate(OfferKey, ActionDate, 0, false);
            //Assert that the Case is in the Decline state
            Assertions.AssertCurrentAppManX2State(OfferKey, "Decline");
        }

        [Test, Description("Verify that a New Business Processor can finalise a decline on an Application")]
        public void _008_FinaliseDeclineConsultant()
        {
            string TestUser = @"sahl\nbpuser"; // OffersAtApplicationCaptureCSV.GetValue("Username", index);
            string Password = "Natal1"; // OffersAtApplicationCaptureCSV.GetValue("Password", index);

            browser = Navigation.Base.StartBrowser(TestUser, Password);
            Navigation.Base.CloseLoanNodes(browser);

            string OfferKey = Helper.GetOfferKeyByTestIdentifier("FinaliseDeclineConsultant", "ApplicationManagementTestID");

            //Step 1: Select offer from worklist
            Navigation.WorkFlowsNode.WorkFlows(browser);
            Views.WorkflowSuperSearch.SearchByOfferKey(browser, OfferKey, "Manage Application");
            //Step 2: Perform the action
            Navigation.ApplicationManagementWorkFlow.Decline(browser);
            //Step 3: Decline the case
            Views.CommonReasonCommonDecline.SelectReasonAndSubmit(browser, "Administrative Decline");
            //Step 4: Finalise the Decline
            browser.WaitForComplete();
            Navigation.ApplicationCapture.DeclineFinalised(browser);
            Views.WorkflowYesNo.Confirm(browser, true);
            //Step 5: Assert that the case is archived
            Assertions.AssertCurrentAppManX2State(OfferKey, "Archive Decline");
            //Assert the offer status is correct
            Assertions.AssertOfferStatus(OfferKey, OfferStatus.Declined);
            //Assert the end date has been populated
            Assertions.AssertOfferEndDate(OfferKey, DateTime.Now, 0, false);
        }

        [Test, Description("Verify that a New Business Processor can reinstate a decline on an Application")]
        public void _009_ReinstateDeclineConsultant()
        {
            string TestUser = @"sahl\nbpuser"; // OffersAtApplicationCaptureCSV.GetValue("Username", index);
            string Password = "Natal1"; // OffersAtApplicationCaptureCSV.GetValue("Password", index);

            browser = Navigation.Base.StartBrowser(TestUser, Password);
            Navigation.Base.CloseLoanNodes(browser);
            //we need to fetch the PREVIOUS STATE so that we know where it will be sent after reactivating
            string OfferKey = Helper.GetOfferKeyByTestIdentifier("ReinstateDeclineConsultant", "ApplicationManagementTestID");

            //Step 1: Select offer from worklist
            Navigation.WorkFlowsNode.WorkFlows(browser);
            Views.WorkflowSuperSearch.SearchByOfferKey(browser, OfferKey, "Manage Application");
            //Step 2: Perform the action
            Navigation.ApplicationManagementWorkFlow.Decline(browser);
            //Step 3: Decline the case
            Views.CommonReasonCommonDecline.SelectReasonAndSubmit(browser, "Administrative Decline");
            //Step 4: Reinstate the Decline
            browser.WaitForComplete();
            //fetch the previous state
            SQLQuerying.DataHelper._2AM db = new SQLQuerying.DataHelper._2AM(AppMan.Default.SAHLDataBaseServer);
            QueryResults Results = db.GetAppCapInstanceDetails(OfferKey);
            string PrevState = Results.Rows(0).Column("Last_State").Value;
            Results.Dispose();
            Navigation.ApplicationCapture.ReactivateDecline(browser);
            Views.WorkflowYesNo.Confirm(browser, true);
            //Step 5: Assert that the case is at the previous state, offer status is open and the OfferEndDate is NULL
            //we need to allow the workflow some time to reinstate the case and move it in the workflow
            System.Threading.Thread.Sleep(10000);
            Assertions.AssertCurrentAppManX2State(OfferKey, PrevState);
            //Assert Offer Status = Open
            Assertions.AssertOfferStatus(OfferKey, OfferStatus.Open);
            //Assert OfferEndDate is back to NULL
            DateTime date = DateTime.Now;
            Assertions.AssertOfferEndDate(OfferKey, date, 0, true);

        }

        [Test, Description("Verify that the Decline Timer correctly archives a case from the Decline state")]
        public void _025_DeclineTimeoutApplication()
        {
            //see _002_DeclineTimeoutApplication() in the TestSetupData #region for the preliminary test steps
            string OfferKey = Helper.GetOfferKeyByTestIdentifier("DeclineTimeoutApplication", "ApplicationManagementTestID");
            Assertions.AssertCurrentAppManX2State(OfferKey, "Archive Decline");
            //Assert the end date has been populated
            Assertions.AssertOfferEndDate(OfferKey, DateTime.Now, 0, false);
            //Assert the offer status is correct
            Assertions.AssertOfferStatus(OfferKey, OfferStatus.Declined);
        }

        [Test, Description("Verify that a New Business Processor can perform the 'Query On Application' action which will move the application to the 'Application Query' state")]
        public void _01_QueryOnApplication()
        {
            QueryResults results = Helper.OffersAtApplicationCaptureRow("QueryOnApplication");

            string BranchConsultant = results.Rows(0).Column("Username").Value;
            string OfferKey = results.Rows(0).Column("OfferKey").Value;

            browser = Navigation.Base.StartBrowser(@"sahl\nbpuser", "Natal1");
            Navigation.Base.CloseLoanNodes(browser);
            //Step 1: Select offer from worklist
            Navigation.WorkFlowsNode.WorkFlows(browser);
            Views.WorkflowSuperSearch.SearchByOfferKey(browser, OfferKey, "Manage Application");
            //Step 2: Complete Query On Application action
            Navigation.ApplicationManagementWorkFlow.QueryonApplication(browser);
            Views.CommonReasonCommonDecline.SelectReasonAndSubmit(browser, "Processor Query");
            //Step 3: Assert application has moved to Application Query state and been assigned to the correct user
            Assertions.AssertCurrentAppManX2State(OfferKey, "Application Query");

            Assertions.CaseCreation.AssertWhoTheWorkFlowAssignmentRecordIsAssignedTo(OfferKey, OfferRoleType.BranchConsultantD, BranchConsultant);
            Assertions.CaseCreation.AssertThatTheWorkFlowAssignmentRecordIsActive(OfferKey, OfferRoleType.BranchConsultantD);
            Assertions.CaseCreation.AssertThatTheWorkFlowAssignmentRecordIsInactive(OfferKey, OfferRoleType.NewBusinessProcessorD);

            Assertions.CaseCreation.AssertWhoTheOfferRoleRecordIsAssignedTo(OfferKey, OfferRoleType.BranchConsultantD, BranchConsultant);
            Assertions.CaseCreation.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(OfferKey, OfferRoleType.BranchConsultantD);
            Assertions.CaseCreation.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(OfferKey, OfferRoleType.NewBusinessProcessorD);

            results.Dispose();
        }

        [Test, Description("Verify that a Branch Consultant can perform the 'Feedback On Application' action which will move the application to the 'Manage Application' state")]
        public void _02_FeedbackOnApplication()
        {
            QueryResults results = Helper.OffersAtApplicationCaptureRow("QueryOnApplication");

            string BranchConsultant = results.Rows(0).Column("Username").Value;
            string OfferKey = results.Rows(0).Column("OfferKey").Value;

            browser = Navigation.Base.StartBrowser(BranchConsultant, "Natal1");
            Navigation.Base.CloseLoanNodes(browser);
            //Step 1: Select offer from worklist
            Navigation.WorkFlowsNode.WorkFlows(browser);
            Views.WorkflowSuperSearch.SearchByOfferKey(browser, OfferKey, WorkflowStates.ApplicationManagementWF.ApplicationQuery);
            //Step 2: Complete Query On Application action
            Navigation.ApplicationManagementWorkFlow.FeedbackonQuery(browser);
            Views.WorkflowYesNo.Confirm(browser, true);
            //Step 3: Assert application has moved to the 'Manage Application' state and been assigned to the correct user
            Assertions.AssertCurrentAppManX2State(OfferKey, WorkflowStates.ApplicationManagementWF.ManageApplication);

            //Assertions.CaseCreation.AssertWhoTheWorkFlowAssignmentRecordIsAssignedTo(OfferKey, "101", BranchConsultant);
            Assertions.CaseCreation.AssertThatTheWorkFlowAssignmentRecordIsActive(OfferKey, OfferRoleType.NewBusinessProcessorD);
            Assertions.CaseCreation.AssertThatTheWorkFlowAssignmentRecordIsInactive(OfferKey, OfferRoleType.BranchConsultantD);

            //Assertions.CaseCreation.AssertWhoTheOfferRoleRecordIsAssignedTo(OfferKey, "101", BranchConsultant);
            Assertions.CaseCreation.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(OfferKey, OfferRoleType.NewBusinessProcessorD);
            Assertions.CaseCreation.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(OfferKey, OfferRoleType.BranchConsultantD);

        }

        [Test, Description("Verify that a New Business Processor can perform the Create Followup action")]
        public void _01_CreateFollowUp(
            [Values("CreateFollowUp", "ReinstateFollowUp", "ContinueWithFollowupReadyToFollowup", "ArchiveCompletedFollowup")] string TestIdentifier)
        {
            Console.WriteLine(@"--********CreateFollowup(" + TestIdentifier + ")********--");

            //QueryResults results = Helper.OffersAtApplicationCaptureRow(TestIdentifier);
            string Username = @"sahl\nbpuser";
            string OfferKey = Helper.GetAppManOfferKey_FilterByClone(BuildingBlocks.Constants.WorkflowStates.ApplicationManagementWF.ManageApplication, @"sahl\nbpuser", 1, "%Followup%");
            OfferKeys.Add(TestIdentifier, OfferKey);
            string Password = @"Natal1";
            //results.Dispose();

            //login as New Business Processor
            browser = Navigation.Base.StartBrowser(Username, Password);
            Navigation.Base.CloseLoanNodes(browser);
            //search for the case and add it to the FloBo
            Navigation.WorkFlowsNode.WorkFlows(browser);
            Views.WorkflowSuperSearch.SearchByOfferKey(browser, OfferKey, WorkflowStates.ApplicationManagementWF.ManageApplication);
            //select the action
            Navigation.ApplicationManagementWorkFlow.CreateFollowup(browser);
            int MinsToAdd;
            //create the FollowUp
            switch (TestIdentifier)
            {
                case "CreateFollowUp":
                    MinsToAdd = 45;
                    break;
                default:
                    MinsToAdd = 5;
                    break;
            }
            string HourValueToAssert;
            string MinValueToAssert;
            Views.MemoFollowUpAdd.CreateFollowup(browser, MinsToAdd, out HourValueToAssert, out MinValueToAssert);
            //assert that the case was sent to the correct state
            Assertions.AssertAppManX2CloneExists(OfferKey, WorkflowStates.ApplicationManagementWF.FollowupHold);
            //assert that the scheduled activty was setup with the correct hour and minute values
            Assertions.AssertScheduleActivitySetup(OfferKey, "OnFollowup", 0, Convert.ToInt32(HourValueToAssert), Convert.ToInt32(MinValueToAssert), 1, true);
            //assert only one 694 and 101 OfferRole exists
            Assertions.CaseCreation.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(OfferKey, OfferRoleType.NewBusinessProcessorD);
            Assertions.CaseCreation.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(OfferKey, OfferRoleType.BranchConsultantD);
            //assert the WFA records for Manage Application is still active
            Assertions.AssertWorkFlowAssignmentRecordAssignment(OfferKey, WorkflowStates.ApplicationManagementWF.ManageApplication, null, 1, "694");
            //assert 2 new WFA records have been created and assigned to a Branch Consultant 101 and New Business Processor 694
            if (Helper.OfferRoleTypesAssignedInX2WorkFlowAssignment(OfferKey, "Application Submitted", "102"))
            {
                Assertions.AssertWorkFlowAssignmentRecordAssignment(OfferKey, WorkflowStates.ApplicationManagementWF.FollowupHold, null, 1, "694", "101", "102");
            }
            else Assertions.AssertWorkFlowAssignmentRecordAssignment(OfferKey, WorkflowStates.ApplicationManagementWF.FollowupHold, null, 1, "694", "101");
        }

        [Test, Description("Verify that a Branch Consultant can perform the Update Followup action")]
        public void _02_UpdateFollowUp()
        {
            Console.WriteLine(@"--********UpdateFollowUp********--");

            //QueryResults results = Helper.OffersAtApplicationCaptureRow("CreateFollowUp");
            string Username = @"sahl\nbpuser";
            string OfferKey = OfferKeys["CreateFollowUp"]; // results.Rows(0).Column("OfferKey").Value;
            string Password = @"Natal1";
            //results.Dispose();

            //login as New Business Processor
            browser = Navigation.Base.StartBrowser(Username, Password);
            Navigation.Base.CloseLoanNodes(browser);
            //search for the case and add it to the FloBo
            Navigation.WorkFlowsNode.WorkFlows(browser);
            Views.WorkflowSuperSearch.SearchByOfferKey(browser, OfferKey, "Followup Hold");
            //select the action
            Navigation.ApplicationManagementWorkFlow.UpdateFollowup(browser);
            //update the FollowUp
            int NewMins = 45;
            string AssertHours;
            string AssertMins;
            Views.MemoFollowUpAdd.CreateFollowup(browser, NewMins, out AssertHours, out AssertMins);
            //assert that the case was sent to the correct state
            Assertions.AssertAppManX2CloneExists(OfferKey, WorkflowStates.ApplicationManagementWF.FollowupHold);
            //assert that the scheduled activty was setup with the correct hour and minute values
            Assertions.AssertScheduleActivitySetup(OfferKey, "OnFollowup", 0, Convert.ToInt32(AssertHours), Convert.ToInt32(AssertMins), 1, true);
            //assert only one 694 and 101 OfferRole exists
            Assertions.CaseCreation.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(OfferKey, OfferRoleType.NewBusinessProcessorD);
            Assertions.CaseCreation.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(OfferKey, OfferRoleType.BranchConsultantD);
            //assert the WFA records for Manage Application is still active
            Assertions.AssertWorkFlowAssignmentRecordAssignment(OfferKey, WorkflowStates.ApplicationManagementWF.ManageApplication, null, 1, "694");
            //assert 2 new WFA records have been created and assigned to a Branch Consultant 101 and New Business Processor 694
            if (Helper.OfferRoleTypesAssignedInX2WorkFlowAssignment(OfferKey, "Application Submitted", "102"))
            {
                Assertions.AssertWorkFlowAssignmentRecordAssignment(OfferKey, WorkflowStates.ApplicationManagementWF.FollowupHold, null, 1, "694", "101", "102");
            }
            else Assertions.AssertWorkFlowAssignmentRecordAssignment(OfferKey, WorkflowStates.ApplicationManagementWF.FollowupHold, null, 1, "694", "101");
        }
        
        [Test, Description("Verify that a Branch Consultant can create a follow up and continue with the application")]
        public void _03_ContinueApplicationFromFollowUpHold()
        {
            Console.WriteLine(@"--********ContinueApplicationFromFollowUpHold********--");

            QueryResults results = Helper.OffersAtApplicationCaptureRow("CreateFollowUp");
            string Username = @"sahl\nbpuser";
            string OfferKey = results.Rows(0).Column("OfferKey").Value;
            string Password = @"Natal1";
            results.Dispose();

            //login as New Business Processor
            browser = Navigation.Base.StartBrowser(Username, Password);
            Navigation.Base.CloseLoanNodes(browser);
            //search for the case and add it to the FloBo
            Navigation.WorkFlowsNode.WorkFlows(browser);
            Views.WorkflowSuperSearch.SearchByOfferKey(browser, OfferKey, "Followup Hold");

            //we now need to get previous state of the application
            SQLQuerying.DataHelper._2AM db = new SQLQuerying.DataHelper._2AM(AppMan.Default.SAHLDataBaseServer);
            QueryResults InstanceDetails = db.GetAppManInstanceDetails(OfferKey);
            string PrevState = InstanceDetails.Rows(0).Column("PreviousState").Value;
            InstanceDetails.Dispose();

            //Continue Application
            Navigation.ApplicationManagementWorkFlow.CompleteFollowup(browser);
            //Views.WorkflowYesNo.Confirm(browser, true);
            //Assert the case is now at the previous state
            Assertions.AssertCurrentAppManX2State(OfferKey, PrevState);
            //assert only one 694 and 101 OfferRole exists
            Assertions.CaseCreation.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(OfferKey, OfferRoleType.NewBusinessProcessorD);
            Assertions.CaseCreation.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(OfferKey, OfferRoleType.BranchConsultantD);
            //assert the WFA records for Manage Application is still active
            Assertions.AssertWorkFlowAssignmentRecordAssignment(OfferKey, WorkflowStates.ApplicationManagementWF.ManageApplication, null, 1, "694");
            //assert 2 new WFA records have been created and assigned to a Branch Consultant 101 and New Business Processor 694
            if (Helper.OfferRoleTypesAssignedInX2WorkFlowAssignment(OfferKey, "Application Submitted", "102"))
            {
                Assertions.AssertWorkFlowAssignmentRecordAssignment(OfferKey, WorkflowStates.ApplicationManagementWF.FollowupComplete, null, 2, "694", "101", "102");
            }
            else Assertions.AssertWorkFlowAssignmentRecordAssignment(OfferKey, WorkflowStates.ApplicationManagementWF.FollowupComplete, null, 2, "694", "101");
        }

        [Test, Description("")]
        public void _04_OnFollowUpTimer(
            [Values("ReinstateFollowUp", "ContinueWithFollowupReadyToFollowup", "ArchiveCompletedFollowup")] string TestIdentifier)
        {
            Console.WriteLine(@"--********OnFollowUpTimer(" + TestIdentifier + ")********--");

            QueryResults results = Helper.OffersAtApplicationCaptureRow(TestIdentifier);
            string OfferKey = results.Rows(0).Column("OfferKey").Value;
            results.Dispose();

            //Assert the case is now at the Ready To Followup state
            Assertions.AssertAppManX2CloneExists(OfferKey, WorkflowStates.ApplicationManagementWF.ReadyToFollowup);
            //Assert the Archive Complete Followup timer has been set up correctly
            Assertions.AssertScheduleActivitySetup(OfferKey, "Archive Completed Followup", 10, 0, 0, 2, true);
            //assert only one 694 and 101 OfferRole exists
            Assertions.CaseCreation.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(OfferKey, OfferRoleType.NewBusinessProcessorD);
            Assertions.CaseCreation.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(OfferKey, OfferRoleType.BranchConsultantD);
            //assert the WFA records for Manage Application is still active
            Assertions.AssertWorkFlowAssignmentRecordAssignment(OfferKey, WorkflowStates.ApplicationManagementWF.ManageApplication, null, 1, "694");
            //assert 2 new WFA records have been created and assigned to a Branch Consultant 101 and New Business Processor 694
            if (Helper.OfferRoleTypesAssignedInX2WorkFlowAssignment(OfferKey, "Application Submitted", "102"))
            {
                Assertions.AssertWorkFlowAssignmentRecordAssignment(OfferKey, WorkflowStates.ApplicationManagementWF.ReadyToFollowup, null, 1, "694", "101", "102");
            }
            else Assertions.AssertWorkFlowAssignmentRecordAssignment(OfferKey, WorkflowStates.ApplicationManagementWF.ReadyToFollowup, null, 1, "694", "101");
        }

        [Test, Description("")]
        public void _05_ArchiveCompletedFollowupTimer_Part1()
        {
            Console.WriteLine(@"--********ArchiveCompletedFollowupTimer_Part1********--");

            QueryResults results = Helper.OffersAtApplicationCaptureRow("ArchiveCompletedFollowup");
            string OfferKey = results.Rows(0).Column("OfferKey").Value;
            results.Dispose();

            Common.UpdateScheduledActivity(OfferKey, _2AM.WorkflowMap.Application_Management, "1", "Archive Completed Followup");
        }

        [Test, Description("Verify that a branch consultant user can Reinstate a Follow Up from the Ready to Follow Up state")]
        public void _06_ReinstateFollowUp()
        {
            Console.WriteLine(@"--********ReinstateFollowUp********--");

            QueryResults results = Helper.OffersAtApplicationCaptureRow("ReinstateFollowUp");
            string Username = @"sahl\nbpuser";
            string OfferKey = results.Rows(0).Column("OfferKey").Value;
            string Password = @"Natal1";
            results.Dispose();

            //login as New Business Processor
            browser = Navigation.Base.StartBrowser(Username, Password);
            Navigation.Base.CloseLoanNodes(browser);
            //search for the case and add it to the FloBo
            Navigation.WorkFlowsNode.WorkFlows(browser);
            Views.WorkflowSuperSearch.SearchByOfferKey(browser, OfferKey, WorkflowStates.ApplicationManagementWF.ReadyToFollowup);
            //reinstate
            Navigation.ApplicationManagementWorkFlow.ReinstateFollowup(browser);
            //create a new follow up time
            int NewMins = 45;
            string HourToAssert;
            string MinuteToAssert;
            Views.MemoFollowUpAdd.CreateFollowup(browser, NewMins, out HourToAssert, out MinuteToAssert);
            //assert that the case was sent to the correct state
            Assertions.AssertAppManX2CloneExists(OfferKey, WorkflowStates.ApplicationManagementWF.FollowupHold);
            //assert that the scheduled activty was setup with the correct hour and minute values
            Assertions.AssertScheduleActivitySetup(OfferKey, "OnFollowup", 0, Convert.ToInt32(HourToAssert), Convert.ToInt32(MinuteToAssert), 1, true);
            //assert only one 694 and 101 OfferRole exists
            Assertions.CaseCreation.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(OfferKey, OfferRoleType.NewBusinessProcessorD);
            Assertions.CaseCreation.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(OfferKey, OfferRoleType.BranchConsultantD);
            //assert the WFA records for Manage Application is still active
            Assertions.AssertWorkFlowAssignmentRecordAssignment(OfferKey, WorkflowStates.ApplicationManagementWF.ManageApplication, null, 1, "694");
            //assert 2 new WFA records have been created and assigned to a Branch Consultant 101 and New Business Processor 694
            if (Helper.OfferRoleTypesAssignedInX2WorkFlowAssignment(OfferKey, "Application Submitted", "102"))
            {
                Assertions.AssertWorkFlowAssignmentRecordAssignment(OfferKey, WorkflowStates.ApplicationManagementWF.FollowupHold, null, 1, "694", "101", "102");
            }
            else Assertions.AssertWorkFlowAssignmentRecordAssignment(OfferKey, WorkflowStates.ApplicationManagementWF.FollowupHold, null, 1, "694", "101");
        }

        [Test, Description("Verify that a branch consultant user can continue with the Application from the Ready to Follow Up state")]
        public void _07_ContinueWithFollowupReadyToFollowup()
        {
            Console.WriteLine(@"--********ContinueWithFollowupReadyToFollowup********--");

            QueryResults results = Helper.OffersAtApplicationCaptureRow("ContinueWithFollowupReadyToFollowup");
            string Username = @"sahl\nbpuser";
            string OfferKey = results.Rows(0).Column("OfferKey").Value;
            string Password = @"Natal1";
            results.Dispose();

            //login as New Business Processor
            browser = Navigation.Base.StartBrowser(Username, Password);
            Navigation.Base.CloseLoanNodes(browser);
            //search for the case and add it to the FloBo
            Navigation.WorkFlowsNode.WorkFlows(browser);

            //fetch the previous state
            SQLQuerying.DataHelper._2AM db = new SQLQuerying.DataHelper._2AM(AppMan.Default.SAHLDataBaseServer);
            QueryResults Results = db.GetAppManInstanceDetails(OfferKey);
            string PrevState = Results.Rows(0).Column("PreviousState").Value;
            Results.Dispose();

            Views.WorkflowSuperSearch.SearchByOfferKey(browser, OfferKey, WorkflowStates.ApplicationManagementWF.ReadyToFollowup);
            //reinstate
            Navigation.ApplicationManagementWorkFlow.ContinuewithFollowup(browser);
            //confirm
            Views.WorkflowYesNo.Confirm(browser, true);
            //assert the case has moved back to its prev state
            Assertions.AssertCurrentAppManX2State(OfferKey, PrevState);
            //assert only one 694 and 101 OfferRole exists
            Assertions.CaseCreation.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(OfferKey, OfferRoleType.NewBusinessProcessorD);
            Assertions.CaseCreation.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(OfferKey, OfferRoleType.BranchConsultantD);
            //assert the WFA records for Manage Application is still active
            Assertions.AssertWorkFlowAssignmentRecordAssignment(OfferKey, WorkflowStates.ApplicationManagementWF.ManageApplication, null, 1, "694");
            //assert 2 new WFA records have been created and assigned to a Branch Consultant 101 and New Business Processor 694
            if (Helper.OfferRoleTypesAssignedInX2WorkFlowAssignment(OfferKey, "Application Submitted", "102"))
            {
                Assertions.AssertWorkFlowAssignmentRecordAssignment(OfferKey, WorkflowStates.ApplicationManagementWF.FollowupComplete, null, 2, "694", "101", "102");
            }
            else Assertions.AssertWorkFlowAssignmentRecordAssignment(OfferKey, WorkflowStates.ApplicationManagementWF.FollowupComplete, null, 2, "694", "101");

        }

        [Test, Description("")]
        public void _08_ArchiveCompletedFollowupTimer_Part2()
        {
            Console.WriteLine(@"--********ArchiveCompletedFollowupTimer_Part2********--");

            QueryResults results = Helper.OffersAtApplicationCaptureRow("ArchiveCompletedFollowup");
            string OfferKey = results.Rows(0).Column("OfferKey").Value;
            results.Dispose();

            //fetch the previous state
            SQLQuerying.DataHelper._2AM db = new SQLQuerying.DataHelper._2AM(AppMan.Default.SAHLDataBaseServer);
            QueryResults Results = db.GetAppManInstanceDetails(OfferKey);
            string PrevState = Results.Rows(0).Column("PreviousState").Value;
            Results.Dispose();

            //assert the case has moved back to its prev state
            Assertions.AssertCurrentAppManX2State(OfferKey, PrevState);
            //assert only one 694 and 101 OfferRole exists
            Assertions.CaseCreation.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(OfferKey, OfferRoleType.NewBusinessProcessorD);
            Assertions.CaseCreation.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(OfferKey, OfferRoleType.BranchConsultantD);
            //assert the WFA records for Manage Application is still active
            Assertions.AssertWorkFlowAssignmentRecordAssignment(OfferKey, WorkflowStates.ApplicationManagementWF.ManageApplication, null, 1, "694");
            //assert 2 new WFA records have been created and assigned to a Branch Consultant 101 and New Business Processor 694
            if (Helper.OfferRoleTypesAssignedInX2WorkFlowAssignment(OfferKey, "Application Submitted", "102"))
            {
                Assertions.AssertWorkFlowAssignmentRecordAssignment(OfferKey, WorkflowStates.ApplicationManagementWF.FollowupComplete, null, 2, "694", "101", "102");
            }
            else Assertions.AssertWorkFlowAssignmentRecordAssignment(OfferKey, WorkflowStates.ApplicationManagementWF.FollowupComplete, null, 2, "694", "101");
        }

        [Test, Description("")]
        public void BatchReassign()
        {
            browser = Navigation.Base.StartBrowser(@"sahl\nbmuser", @"Natal1");

            Navigation.WorkFlowsNode.WorkFlows(browser);
            Navigation.WorkFlowsNode.BatchReassign(browser);

            Views.WorkflowBatchReassign.BatchReassign(browser, "New Business Processor", @"sahl\nbpuser", 25, "781691", "782440", "787476", "788523");
        }

        [Test, Description("")]
        public void Test()
        {
            string OfferKey = Helper.GetOffersByLatestLightstoneValuationsDate(BuildingBlocks.Constants.WorkflowName.ApplicationManagement, BuildingBlocks.Constants.WorkflowStates.ApplicationManagementWF.ManageApplication, '>');
            Console.WriteLine(OfferKey);
        }

        [Test, Description(@"")]
        public void OverrideCheck()
        {
            //search for the case
            browser = Navigation.Base.StartBrowser(@"sahl\nbmuser", "Natal1");
            Navigation.WorkFlowsNode.WorkFlows(browser);
            string OfferKey = Helper.GetAppManOfferKey_FilterByClone(BuildingBlocks.Constants.WorkflowStates.ApplicationManagementWF.ManageApplication, @"sahl\nbpuser", 1, "%Followup%");
            Views.WorkflowSuperSearch.SearchByOfferKey(browser, OfferKey, WorkflowStates.ApplicationManagementWF.ManageApplication);
            //perform the action
            Navigation.ApplicationManagementWorkFlow.OverrideCheck(browser);
            Views.WorkflowYesNo.Confirm(browser, true);

            Assertions.AssertCurrentCreditX2State(OfferKey, WorkflowStates.CreditWF.Credit);
            //assert only one 694 and 808 OfferRole exists
            Assertions.CaseCreation.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(OfferKey, OfferRoleType.NewBusinessProcessorD);
            Assertions.CaseCreation.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(OfferKey, OfferRoleType.CreditUnderwriterD);
            //assert a new WFA records have been created and assigned to a Credit Underwriter 808
            Assertions.AssertWorkFlowAssignmentRecordAssignment(OfferKey, WorkflowStates.CreditWF.Credit, null, 1, "808");
        }

        [Test, Description("")]
        public void RequestLightstoneValuation_LessThan2MonthsOld()
        {
            Console.WriteLine(@"--********RequestLightstoneValuation_LessThan2MonthsOld********--");

            string OfferKey = Helper.GetOffersByLatestLightstoneValuationsDate(BuildingBlocks.Constants.WorkflowName.ApplicationManagement, BuildingBlocks.Constants.WorkflowStates.ApplicationManagementWF.ManageApplication, '>');
            
            browser = Navigation.Base.StartBrowser(@"sahl\nbpuser", "Natal1");
            Navigation.Base.CloseLoanNodes(browser);
            Navigation.WorkFlowsNode.WorkFlows(browser);Views.WorkflowSuperSearch.SearchByOfferKey(browser, OfferKey, "Manage Application");

            Navigation.ApplicationManagementWorkFlow.RequestLightstoneValuation(browser);
            Views.WorkflowYesNo.Confirm(browser, true);
            Assertions.AssertValidationMessageExists(browser, @"A LightStone valuation for this property exists less than 2 months old.");
        }

        [Test, Description("")]
        public void RequestLightstoneValuation_GreaterThan2MonthsOld()
        {
            Console.WriteLine(@"--********RequestLightstoneValuation_GreaterThan2MonthsOld********--");

            string OfferKey = Helper.GetOffersByLatestLightstoneValuationsDate(BuildingBlocks.Constants.WorkflowName.ApplicationManagement, BuildingBlocks.Constants.WorkflowStates.ApplicationManagementWF.ManageApplication, '<');
            
            browser = Navigation.Base.StartBrowser(@"sahl\nbpuser", "Natal1");
            Navigation.Base.CloseLoanNodes(browser);
            Navigation.WorkFlowsNode.WorkFlows(browser);Views.WorkflowSuperSearch.SearchByOfferKey(browser, OfferKey, "Manage Application");

            Navigation.ApplicationManagementWorkFlow.RequestLightstoneValuation(browser);
            Views.WorkflowYesNo.Confirm(browser, true);
            Assertions.AssertLatestLightstoneValuationRecord(OfferKey);
        }

        [Test, Description("")]
        public void RequestLightstoneValuation_NoPropertyID()
        {
            Console.WriteLine(@"--********RequestLightstoneValuation_NoPropertyID********--");

            string OfferKey = Helper.GetOffersWithoutLightstonePropertyID("Application Management", "Manage Application");
            
            browser = Navigation.Base.StartBrowser(@"sahl\nbpuser", "Natal1");
            Navigation.Base.CloseLoanNodes(browser);
            Navigation.WorkFlowsNode.WorkFlows(browser);Views.WorkflowSuperSearch.SearchByOfferKey(browser, OfferKey, "Manage Application");
            Navigation.ApplicationManagementWorkFlow.RequestLightstoneValuation(browser);
            Views.WorkflowYesNo.Confirm(browser, true);
            Assertions.AssertValidationMessageExists(browser, @"There is no LightStone property ID to do a valuation.");
        }

        [Test]
        public void _001_PerformFurtherValuation()
        {
            Console.WriteLine(@"--********PerformFurtherValuation********--");

            // Get an offer at "Manage Application" state that does not have an Instance clone at "Valuation Hold" State
            string OfferKey = Helper.GetAppManOffers_FilterByValuationsAndWorkflowHistory(WorkflowStates.ValuationsWF.FurtherValuationRequest, 1, 1);
            OfferKeys.Add("PerformFurtherValuation", OfferKey);
           
            browser = Navigation.Base.StartBrowser(@"sahl\nbpuser", "Natal1");
            Navigation.Base.CloseLoanNodes(browser);
            Navigation.WorkFlowsNode.WorkFlows(browser);
            Views.WorkflowSuperSearch.SearchByOfferKey(browser, OfferKey, WorkflowStates.ApplicationManagementWF.ManageApplication);
            Navigation.ApplicationManagementWorkFlow.PerformFurtherValuation(browser);
            Views.WorkflowYesNo.Confirm(browser, true);

            Assertions.AssertAppManX2CloneExists(OfferKey, WorkflowStates.ApplicationManagementWF.FurtherValuationRequired);
            Assertions.AssertCurrentValuationsX2State(OfferKey, WorkflowStates.ValuationsWF.FurtherValuationRequest);
        }

        [Test]
        public void PerformFurtherValuation_FurtherValuationRequiredCloneExists()
        {
            Console.WriteLine(@"--********PerformFurtherValuation_FurtherValuationRequiredCloneExists********--");

            // Get an offer at "Manage Application" state that does not have an Instance clone at "Valuation Hold" State
            //string OfferKey = Helper.GetAppManOfferKey_FilterByClone(WorkflowStates.ApplicationManagement.ManageApplication, @"sahl\nbpuser", 0, WorkflowStates.ApplicationManagement.FurtherValuationRequired);
            string OfferKey = Helper.GetAppManOffers_FilterByValuationsAndWorkflowHistory(WorkflowStates.ValuationsWF.FurtherValuationRequest, 0, 1);

            browser = Navigation.Base.StartBrowser(@"sahl\nbpuser", "Natal1");
            Navigation.Base.CloseLoanNodes(browser);
            Navigation.WorkFlowsNode.WorkFlows(browser); Views.WorkflowSuperSearch.SearchByOfferKey(browser, OfferKey);
            Navigation.ApplicationManagementWorkFlow.PerformFurtherValuation(browser);

            Assertions.AssertNotification(browser, "This case has already had a Further Valuation Performed");
        }

        [Test]
        public void PerformFurtherValuation_ValuationInstanceExists()
        {
            Console.WriteLine(@"--********PerformFurtherValuation_ValuationHoldCloneExists********--");

            // Get an offer at "Manage Application" state that does not have an Instance clone at "Valuation Hold" State
            //string OfferKey = Helper.GetAppManOfferKey_FilterByClone(WorkflowStates.ApplicationManagement.ManageApplication, @"sahl\nbpuser", 0, WorkflowStates.ApplicationManagement.ApplicationHold);
            string OfferKey = Helper.GetAppManOffers_FilterByValuationsAndWorkflowHistory(WorkflowStates.ValuationsWF.FurtherValuationRequest, 1, 0);

            browser = Navigation.Base.StartBrowser(@"sahl\nbpuser", "Natal1");
            Navigation.Base.CloseLoanNodes(browser);
            Navigation.WorkFlowsNode.WorkFlows(browser);Views.WorkflowSuperSearch.SearchByOfferKey(browser, OfferKey, WorkflowStates.ApplicationManagementWF.ManageApplication);
            Navigation.ApplicationManagementWorkFlow.PerformFurtherValuation(browser);
            Views.WorkflowYesNo.Confirm(browser, true);

            Assertions.AssertValidationMessageExists(browser, new System.Text.RegularExpressions.Regex("Application:[0-9]* ReservedAccountKey:[0-9]* in Valuations is not complete. Is at State:"));
        }
        /// <summary>
        /// Verify that a New Business Processor can complete the Application in Order action at the Manage Application state.  Ensure that the application moves to the Cridit state in the Credit workflow 
        /// and is assigned to the correct user.
        /// </summary>
        /// <param name="TestIdentifier">OffersAtApplicationCapture.ApplicationManagementTestID</param>
        /// <param name="LoanType">OffersAtApplicationCapture.LoanType</param>
        /// <param name="Username">OffersAtApplicationCapture.Username</param>
        /// <param name="Password">OffersAtApplicationCapture.Password</param>
        [Test, Sequential]
        public void ApplicationInOrder(
            [ValueSource(typeof(ApplicationInOrderSeqentialData), "ApplicationManagementTestID")] string TestIdentifier,
            [ValueSource(typeof(ApplicationInOrderSeqentialData), "LoanType")] string LoanType,
            [ValueSource(typeof(ApplicationInOrderSeqentialData), "Username")] string Username,
            [ValueSource(typeof(ApplicationInOrderSeqentialData), "Password")] string Password)
        {
            string OfferKey = Helper.GetOfferKeyByTestIdentifier(TestIdentifier, "ApplicationManagementTestID");

            if (LoanType == "New purchase")
            {
                browser = Navigation.Base.StartBrowser(@"sahl\cuuser", "Natal1");
                Navigation.Base.CloseLoanNodes(browser);

                Navigation.WorkFlowsNode.WorkFlows(browser);
                Views.WorkflowSuperSearch.SearchByOfferKey(browser, OfferKey);

                Navigation.ApplicationManagementWorkFlow.QAComplete(browser);
                Views.CommonReasonCommonDecline.SelectReasonAndSubmit(browser, "QA Complete");

                browser.Dispose();
                browser = null;

                browser = Navigation.Base.StartBrowser(Username, Password);
                Navigation.Base.CloseLoanNodes(browser);

                Navigation.WorkFlowsNode.WorkFlows(browser);
                Views.WorkflowSuperSearch.SearchByOfferKey(browser, OfferKey);

                Navigation.ApplicationManagementWorkFlow.ClientAccepts(browser);
                Views.WorkflowYesNo.Confirm(browser, true);

                browser.Dispose();
                browser = null;
            }

            browser = Navigation.Base.StartBrowser(@"sahl\vpuser2", "Natal1");
            Navigation.Base.CloseLoanNodes(browser);

            Navigation.WorkFlowsNode.WorkFlows(browser);
            Views.WorkflowSuperSearch.SearchByOfferKey(browser, OfferKey, BuildingBlocks.Constants.WorkflowStates.ValuationsWF.ScheduleValuationAssessment);

            Navigation.ValuationsWorkFlow.ManualValuation(browser);
            Views.PerformManualValuation.BasicPerformManualValuation(browser, "Normal standard", "Conventional", 300, 3000, "Carl nel");

            browser.Dispose();
            browser = null;

            browser = Navigation.Base.StartBrowser(@"sahl\nbpuser", "Natal1");
            Navigation.Base.CloseLoanNodes(browser);

            Navigation.WorkFlowsNode.WorkFlows(browser);
            Views.WorkflowSuperSearch.SearchByOfferKey(browser, OfferKey, BuildingBlocks.Constants.WorkflowStates.ApplicationManagementWF.ManageApplication);

            Navigation.PropertiesNode.PropertyAddressNode.PropertyAddress(browser, OfferKey);

            Navigation.PropertiesNode.PropertyAddressNode.UpdateDeedsOfficeDetails(browser, Navigation.NodeType.Update);
            Views.PropertyDetailsUpdateDeedsNoExclusions.UpdateDeedsOffeceDetials(browser, "Test", "Test", "Test", "Test", "Test", "Test", "Test", Views.btn.Update);

            Navigation.PropertiesNode.PropertyAddressNode.HomeOwnersCover(browser, Navigation.NodeType.Update);
            Views.HOCFSSummaryAddApplication.UpdateHOCDetials(browser, 2, Views.btn.Add);

            Navigation.SellersNode.Sellers(browser, Navigation.NodeType.Add);
            Views.LegalEntitySellerDetails.AddSeller(browser, IDNumbers.GetNextIDNumber(), "Mr", "Test", "Seller", "Male", "Single", null, null, "SA Citizen", null, null, "English", null, null, "012", "3456789", null, null, null, null, null, null);

            Navigation.LegalEntityNode.LegalEntity(browser, OfferKey);

            Navigation.LegalEntityNode.EmploymentDetails(browser, Navigation.NodeType.Add);
            Views.LegalEntityEmploymentDetails.AddEmploymentDetails(browser, "BLU RIBBON", "Self Employed", "Business Profits", "01/05/2010", "50000");

            string Bank;
            string BankCodeNumber;
            string TypeDescription;
            string AccountNumber;

            Common.GetNextUnusedBankAccountDetails(out Bank, out BankCodeNumber, out TypeDescription, out AccountNumber);

            Navigation.LegalEntityNode.BankingDetails(browser, Navigation.NodeType.Add);
            Views.BankingDetails.AddBankingDetails(browser, Bank, BankCodeNumber, TypeDescription, AccountNumber, "Test", Views.btn.Add);

            Navigation.LoanDetailsNode.LoanDetails(browser);

            if (LoanType != "New purchase")
            {

                Common.GetNextUnusedBankAccountDetails(out Bank, out BankCodeNumber, out TypeDescription, out AccountNumber);

                Navigation.LoanDetailsNode.SettlementBankingDetails(browser, Navigation.NodeType.Add);
                Views.BankingDetails.AddSettlementBankingDetails(browser, Bank, BankCodeNumber, TypeDescription, AccountNumber, "Test", "Test", Views.btn.Add);
            }

            Navigation.LoanDetailsNode.DebitOrderDetails(browser, Navigation.NodeType.Update);
            Views.DebitOrderDetailsAppUpdate.UpdateDebitOrderDetails(browser, "Debit Order Payment", 1, 1, Views.btn.Update);

            Navigation.LoanDetailsNode.LoanAttributes(browser, Navigation.NodeType.Update);
            Views.ApplicationAttributesUpdate.UpdateApplicationLoanAttributes(browser, "Test", 1, Views.btn.Update);

            Navigation.LoanDetailsNode.LoanConditions(browser, Navigation.NodeType.Add);
            Views.ConditionAddSet.SaveConditionSet(browser);

            Navigation.DocumentCheckListNode.DocumentChecklist(browser);
            Navigation.DocumentCheckListNode.ViewDocumentChecklist(browser, Navigation.NodeType.Update);
            Views.DocumentCheckListUpdate.UpdateDocumentChecklist(browser);

            Navigation.ApplicationLoanNode.FirstLoanInFloBOMenu(browser);
            Navigation.ApplicationManagementWorkFlow.ApplicationinOrder(browser);
            Views.WorkflowYesNo.Confirm(browser, true);
        }
        /// <summary>
        /// Verify that a New Business Processor can complete the Application in Order action at the Manage Application state.  Ensure that the application moves to the Cridit state in the Credit workflow 
        /// and is assigned to the correct user.
        /// </summary>
        /// <param name="TestIdentifier">OffersAtApplicationCapture.ApplicationManagementTestID</param>
        /// <param name="LoanType">OffersAtApplicationCapture.LoanType</param>
        /// <param name="Username">OffersAtApplicationCapture.Username</param>
        /// <param name="Password">OffersAtApplicationCapture.Password</param>
        [Test, Sequential]
        public void ApplicationInOrderAssertions(
            [ValueSource(typeof(ApplicationInOrderSeqentialData), "ApplicationManagementTestID")] string TestIdentifier)
        {
            string OfferKey = Helper.GetOfferKeyByTestIdentifier(TestIdentifier, "ApplicationManagementTestID");

            Assertions.AssertCurrentCreditX2State(OfferKey, WorkflowStates.CreditWF.Credit);
            //Assert Credi tUnderwriter D offerrole exists
            Assertions.CaseCreation.AssertOfferRoleRecordExists(OfferKey, OfferRoleType.CreditUnderwriterD);
            Assertions.CaseCreation.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(OfferKey, OfferRoleType.CreditUnderwriterD);
            Assertions.CaseCreation.AssertThatAWorkFlowAssignmentRecordExists(OfferKey, OfferRoleType.CreditUnderwriterD);
            Assertions.CaseCreation.AssertThatTheWorkFlowAssignmentRecordIsActive(OfferKey, OfferRoleType.CreditUnderwriterD);

            //Check New Business Processor D offerrole
            Assertions.CaseCreation.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(OfferKey, OfferRoleType.NewBusinessProcessorD);
            Assertions.CaseCreation.AssertThatTheWorkFlowAssignmentRecordIsInactive(OfferKey, OfferRoleType.NewBusinessProcessorD);
        }

        #endregion


    }
}
