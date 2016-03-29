using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System.Collections.Generic;

namespace DebtCounsellingTests.Workflow
{
    [RequiresSTA]
    public sealed class ContinueDebtReviewTests : DebtCounsellingTests.TestBase<BasePage>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.ForeclosureConsultant);
        }

        /// <summary>
        /// This test will send the case back to the Review Notification state from the Litigation state. The DC Admin User on the case is still active
        /// so they should be assigned the case. When selecting the 'Manage DC' reason and continuing with the debt review then there should be no detail types added to
        /// the debt counselling case and the case is assigned a record in the [e-work]..eAlert table linked to an e-work user. There should be no detail type added to
        /// the case, even if a Foreclosure Underway detail type exists on the account.
        /// </summary>
        [Test, Description(@"This test will send the case back to the Review Notification state from the Litigation state. The DC Admin User on the case is still active
		so they should be assigned the case. When selecting the 'Manage DC' reason and continuing with the debt review then there should be no detail types added to the debt
        counselling casev and the case is assigned a record in the [e-work]..eAlert table linked to an e-work user. There should be no detail type added to the case, even if
		if a Foreclosure Underway detail type exists on the account.")]
        public void ContinueDebtReviewManageDebtCounselling()
        {
            //search for account with ework case at debt counselling stage in loss control
            var eworkLossControlCase = Service<IEWorkService>().GetEWorkCase(EworkMaps.LossControl, EworkStages.PendLitigation, product: ProductEnum.NewVariableLoan);
            Service<IEWorkService>().UpdateEworkAssignedUser(eworkLossControlCase.AccountKey, TestUsers.ForeclosureConsultant, EworkStages.PendLitigation);
            base.StartTest(WorkflowStates.DebtCounsellingWF.Litigation, TestUsers.ForeclosureConsultant, false, 1, false, idNumber: eworkLossControlCase.IDNumber, searchForCase: false);
            //we need to remove all detail types
            Service<IDetailTypeService>().RemoveDetailTypes(base.TestCase.AccountKey);
            //add the foreclosure detail type --> this is a negative test to ensure we do not add the LegalActionStopped detail type when using this reason
            Service<IDetailTypeService>().InsertDetailType(DetailTypeEnum.ForeclosureUnderway, base.TestCase.AccountKey);
            base.LoadCase(WorkflowStates.DebtCounsellingWF.Litigation);
            string adminUser = Service<IAssignmentService>().GetUserForDebtCounsellingAssignment(WorkflowRoleTypeEnum.DebtCounsellingAdminD, Workflows.DebtCounselling,
                new List<string> { WorkflowStates.DebtCounsellingWF.ReviewNotification }, true, base.TestCase.DebtCounsellingKey);
            //select the action
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.ContinueDebtReview);
            //confirm
            base.Browser.Page<CommonReasonCommonDecline>().SelectReasonAndSubmit(ReasonType.LitigationReallocation, ReasonDescription.ManageDC);
            //wait
            Service<IX2WorkflowService>().WaitForX2WorkflowHistoryActivity(WorkflowActivities.DebtCounselling.ContinueDebtReview, base.TestCase.InstanceID, 1);
            DebtCounsellingAssertions.AssertLatestDebtCounsellingWorkflowRoleAssignment(base.TestCase.DebtCounsellingKey, adminUser, WorkflowRoleTypeEnum.DebtCounsellingAdminD,
                true, true);
            WorkflowRoleAssignmentAssertions.AssertActiveWorkflowRoleExists(base.TestCase.DebtCounsellingKey, WorkflowRoleTypeEnum.DebtCounsellingAdminD, adminUser);
            //check that it has moved states
            DebtCounsellingAssertions.AssertX2State(base.TestCase.DebtCounsellingKey, WorkflowStates.DebtCounsellingWF.ReviewNotification);
            //case should now be at a debt counselling state in ework
            eWorkAssertions.AssertEworkCaseExists(base.TestCase.AccountKey.ToString(), @"%Debt Counselling%", EworkMaps.LossControl);
            //no detail type should have been added.
            AccountAssertions.AssertDetailType(base.TestCase.AccountKey, DetailTypeEnum.LegalActionStopped, DetailClassEnum.LoanManagement, false);
        }

        //        /// <summary>
        //        /// This test will set the DC Admin user on the workflow case to inactive and then perform the Continue Debt Review action. This will send the case back to
        //        /// the Review Notification state, but round robin assign the case to a DC Admin user as the original one linked to the case is now inactive.
        //        /// </summary>
        //        [Test, Description(@"This test will set the DC Admin user on the workflow case to inactive and then perform the Continue Debt Review action. This will send the
        //		case back to the Review Notification state, but round robin assign the case to a DC Admin user as the original one linked to the case is now inactive.")]
        //        public void ContinueDebtReviewInactiveDCAUser()
        //        {
        //            string adUserName = string.Empty;
        //            try
        //            {
        //                Console.WriteLine(String.Format(@"--********{0}********--", MethodBase.GetCurrentMethod()));
        //                //search for a case at Pend Proposal
        //                int debtCounsellingKey = 0;
        //                Int64 instanceID;
        //                int accountKey = 0;
        //                string assignedDCCUser;
        //                WorkflowHelper.SearchForCaseAtState(WorkflowStates.DebtCounsellingWF.Litigation, TestUsers.ForeclosureConsultant, out instanceID,
        //                    out debtCounsellingKey, out accountKey, out assignedDCCUser, out browser);
        //                if (debtCounsellingKey == 0)
        //                {
        //                    WorkflowHelper.CreateAndProcessDebtCounsellingCase(out browser, WorkflowStates.DebtCounsellingWF.Litigation, out debtCounsellingKey, out instanceID,
        //                        out accountKey, out assignedDCCUser, eWorkCase: true);
        //                }
        //                //we need the admin user
        //                var results = Service<IAssignmentService>().GetActiveWorkflowRoleByTypeAndGenericKey(WorkflowRoleTypeEnum.DebtCounsellingAdminD, debtCounsellingKey);
        //                string adusername = (from r in results
        //                                     select r.Value).FirstOrDefault();
        //                //we need to make our user inactive
        //                base.Service<IADUserService>().UpdateADUserStatus(adUserName, GeneralStatus.Inactive, GeneralStatus.Active, GeneralStatus.Inactive);
        //                //reload the cache
        //                Service<IX2Service>().InsertActiveExternalActivity(Workflows.IT, ExternalActivities.IT_EXTRefreshDSCache, 0);
        //                //start the activity
        //                browser.Navigate<Navigation.DebtCounsellingWorkFlow>().ContinueDebtReview();
        //                //we need to find the next person due the assignment from the load balancing
        //                List<string> states = new List<string>();
        //                states.Add(WorkflowStates.DebtCounsellingWF.ReviewNotification);
        //                string userToAssign = Service<IAssignmentService>().GetUserForDebtCounsellingAssignment(WorkflowRoleTypeEnum.DebtCounsellingAdminD, Workflows.DebtCounselling, states, true,
        //                    debtCounsellingKey);
        //                //confirm
        //                browser.Page<CommonReasonCommonDecline>().SelectReasonAndSubmit(ReasonType.LitigationReallocation, ReasonDescription.ReallocatetoDC);
        //                Thread.Sleep(5000);
        //                //should now be assigned to this users worklist
        //                DebtCounsellingAssertions.AssertLatestDebtCounsellingWorkflowRoleAssignment(debtCounsellingKey, userToAssign,
        //                    WorkflowRoleTypeEnum.DebtCounsellingAdminD, true, true);
        //                //check that the workflow role exists
        //                WorkflowRoleAssignmentAssertions.AssertActiveWorkflowRoleExists(debtCounsellingKey, WorkflowRoleTypeEnum.DebtCounsellingAdminD,
        //                    userToAssign);
        //                //check that it has moved states
        //                DebtCounsellingAssertions.AssertX2State(debtCounsellingKey, WorkflowStates.DebtCounsellingWF.ReviewNotification);
        //                //update the adUser on the record
        //                //check the ework case has moved.
        //                eWorkAssertions.AssertEworkCaseExists(accountKey.ToString(), @"%Debt Counselling%", EworkMaps.LossControl);
        //                //The ework usertodo chould not be asssigned when 'Reallocate To DC' reason selected
        //                eWorkAssertions.AssertLossControlUserToDo(accountKey.ToString(), EworkMaps.LossControl, @"Debt Counselling%", string.Empty);
        //            }
        //            finally
        //            {
        //                //we need to set our user back to active
        //                base.Service<IADUserService>().UpdateADUserStatus(adUserName, GeneralStatus.Active, GeneralStatus.Active, GeneralStatus.Active);
        //                //reload the cache
        //                Service<IX2Service>().InsertActiveExternalActivity(Workflows.IT, ExternalActivities.IT_EXTRefreshDSCache, 0);
        //            }

        //        }
        //        /// <summary>
        //        /// Performing the Continue Litigation action will send the case to an archive state. This will set the status of the
        //        ///	debt counselling case to inactive.
        //        /// </summary>
        //        [Test, Description(@"Performing the Continue Litigation action will send the case to an archive state. This will set the status of the
        //			debt counselling case to inactive.")]
        //        public void ContinueLitigation()
        //        {
        //            Console.WriteLine(String.Format(@"--********{0}********--", MethodBase.GetCurrentMethod()));
        //            //search for a case at Pend Proposal
        //            int debtCounsellingKey = 0;
        //            Int64 instanceID;
        //            int accountKey = 0;
        //            string assignedDCCUser;
        //            WorkflowHelper.SearchForCaseAtState(WorkflowStates.DebtCounsellingWF.Litigation, TestUsers.ForeclosureConsultant, out instanceID,
        //                out debtCounsellingKey, out accountKey, out assignedDCCUser, out browser);
        //            if (debtCounsellingKey == 0)
        //            {
        //                WorkflowHelper.CreateAndProcessDebtCounsellingCase(out browser, WorkflowStates.DebtCounsellingWF.Litigation, out debtCounsellingKey, out instanceID,
        //                    out accountKey, out assignedDCCUser, eWorkCase: true);
        //            }
        //            browser.Navigate<Navigation.DebtCounsellingWorkFlow>().ContinueLitigation();
        //            browser.Page<WorkflowYesNo>().Confirm(true, false);
        //            //case should now be archived
        //            DebtCounsellingAssertions.AssertDebtCounsellingStatus(DebtCounsellingStatus.Closed, debtCounsellingKey);
        //            DebtCounsellingAssertions.AssertX2State(debtCounsellingKey, WorkflowStates.DebtCounsellingWF.ArchiveLitigation);
        //        }
        //        /// <summary>
        //        /// When selecting 'Reallocate to DC' as the reason for the Continue Debt Review AND a Foreclosure Underway detail type
        //        /// exists on the account, then the LEGAL ACTION STOPPED detail type must be added to the account and the case is moved back to the Review
        //        /// Notification state.
        //        /// </summary>
        //        [Test, Description(@"When selecting 'Reallocate to DC' as the reason for the Continue Debt Review AND a Foreclosure Underway detail type
        //		exists on the account, then the LEGAL ACTION STOPPED detail type must be added to the account and the case is moved back to the Review
        //		Notification state.")]
        //        public void ContinueDebtReviewForeclosureUnderwayDetailType()
        //        {
        //            Console.WriteLine(String.Format(@"--********{0}********--", MethodBase.GetCurrentMethod()));
        //            //search for a case at Pend Proposal
        //            int debtCounsellingKey = 0;
        //            Int64 instanceID;
        //            int accountKey = 0;
        //            string assignedDCCUser;
        //            WorkflowHelper.SearchForCaseAtState(WorkflowStates.DebtCounsellingWF.Litigation, TestUsers.ForeclosureConsultant, out instanceID,
        //                out debtCounsellingKey, out accountKey, out assignedDCCUser, out browser);
        //            if (debtCounsellingKey == 0)
        //            {
        //                WorkflowHelper.CreateAndProcessDebtCounsellingCase(out browser, WorkflowStates.DebtCounsellingWF.Litigation, out debtCounsellingKey, out instanceID,
        //                    out accountKey, out assignedDCCUser, eWorkCase: true);
        //            }
        //            //we need to remove the detail types
        //            Service<IDetailTypeService>().RemoveDetailTypes(accountKey);
        //            //add the foreclosure detail type
        //            Service<IDetailTypeService>().InsertDetailType(DetailTypeEnum.ForeclosureUnderway, accountKey);
        //            browser.Navigate<Navigation.DebtCounsellingWorkFlow>().ContinueDebtReview();
        //            //confirm
        //            browser.Page<CommonReasonCommonDecline>().SelectReasonAndSubmit(ReasonType.LitigationReallocation, ReasonDescription.ReallocatetoDC);
        //            //wait
        //            Thread.Sleep(5000);
        //            var results = Service<IAssignmentService>().GetActiveWorkflowRoleByTypeAndGenericKey(WorkflowRoleTypeEnum.DebtCounsellingAdminD, debtCounsellingKey);
        //            string adUserName = (from r in results
        //                                 select r.Value).FirstOrDefault();
        //            //should now be assigned to this users worklist
        //            DebtCounsellingAssertions.AssertLatestDebtCounsellingWorkflowRoleAssignment(debtCounsellingKey, adUserName,
        //                WorkflowRoleTypeEnum.DebtCounsellingAdminD, true, true);
        //            //check that the workflow role exists
        //            WorkflowRoleAssignmentAssertions.AssertActiveWorkflowRoleExists(debtCounsellingKey, WorkflowRoleTypeEnum.DebtCounsellingAdminD,
        //                adUserName);
        //            //check that it has moved states
        //            DebtCounsellingAssertions.AssertX2State(debtCounsellingKey, WorkflowStates.DebtCounsellingWF.ReviewNotification);
        //            //check the ework case has moved.
        //            eWorkAssertions.AssertEworkCaseExists(accountKey.ToString(), @"%Debt Counselling%", EworkMaps.LossControl);
        //            AccountAssertions.AssertDetailType(accountKey, DetailTypeEnum.LegalActionStopped, DetailClassEnum.LoanManagement, true);
        //            //The ework usertodo should not be assigned when 'Reallocate to DC' reason selected
        //            eWorkAssertions.AssertLossControlUserToDo(accountKey.ToString(), EworkMaps.LossControl, @"Debt Counselling%", string.Empty);
        //        }
        //        /// <summary>
        //        /// When selecting 'Reallocate to DC' as the reason for the Continue Debt Review AND a Foreclosure Underway detail type
        //        /// does not exists on the account, then the LEGAL ACTION STOPPED detail type must NOT be added to the account and the case is moved back to the Review
        //        /// Notification state.
        //        /// </summary>
        //        [Test, Description(@"When selecting 'Reallocate to DC' as the reason for the Continue Debt Review AND a Foreclosure Underway detail type
        //		does not exist on the account, then the LEGAL ACTION STOPPED detail type must NOT be added to the account and the case is moved back to the Review
        //		Notification state.")]
        //        public void NoForeclosureUnderwayDetailType()
        //        {
        //            Console.WriteLine(String.Format(@"--********{0}********--", MethodBase.GetCurrentMethod()));
        //            //search for a case at Pend Proposal
        //            int debtCounsellingKey = 0;
        //            Int64 instanceID;
        //            int accountKey = 0;
        //            string assignedDCCUser;
        //            WorkflowHelper.SearchForCaseAtState(WorkflowStates.DebtCounsellingWF.Litigation, TestUsers.ForeclosureConsultant, out instanceID,
        //                out debtCounsellingKey, out accountKey, out assignedDCCUser, out browser);
        //            if (debtCounsellingKey == 0)
        //            {
        //                WorkflowHelper.CreateAndProcessDebtCounsellingCase(out browser, WorkflowStates.DebtCounsellingWF.Litigation, out debtCounsellingKey, out instanceID,
        //                    out accountKey, out assignedDCCUser, eWorkCase: true);
        //            }
        //            //we need to remove the detail types
        //            Service<IDetailTypeService>().RemoveDetailTypes(accountKey);
        //            browser.Navigate<Navigation.DebtCounsellingWorkFlow>().ContinueDebtReview();
        //            //confirm
        //            browser.Page<CommonReasonCommonDecline>().SelectReasonAndSubmit(ReasonType.LitigationReallocation, ReasonDescription.ReallocatetoDC);
        //            //wait
        //            Thread.Sleep(5000);
        //            var results = Service<IAssignmentService>().GetActiveWorkflowRoleByTypeAndGenericKey(WorkflowRoleTypeEnum.DebtCounsellingAdminD, debtCounsellingKey);
        //            string adUserName = (from r in results select r.Value).FirstOrDefault();
        //            //should now be assigned to this users worklist
        //            DebtCounsellingAssertions.AssertLatestDebtCounsellingWorkflowRoleAssignment(debtCounsellingKey, adUserName,
        //                WorkflowRoleTypeEnum.DebtCounsellingAdminD, true, true);
        //            //check that the workflow role exists
        //            WorkflowRoleAssignmentAssertions.AssertActiveWorkflowRoleExists(debtCounsellingKey, WorkflowRoleTypeEnum.DebtCounsellingAdminD,
        //                adUserName);
        //            //check that it has moved states
        //            DebtCounsellingAssertions.AssertX2State(debtCounsellingKey, WorkflowStates.DebtCounsellingWF.ReviewNotification);
        //            //check the ework case has moved.
        //            eWorkAssertions.AssertEworkCaseExists(accountKey.ToString(), @"%Debt Counselling%", EworkMaps.LossControl);
        //            AccountAssertions.AssertDetailType(accountKey, DetailTypeEnum.LegalActionStopped, DetailClassEnum.LoanManagement, false);
        //            //The ework usertodo should not be assigned when 'Reallocate to DC' reason selected
        //            eWorkAssertions.AssertLossControlUserToDo(accountKey.ToString(), EworkMaps.LossControl, @"Debt Counselling%", string.Empty);
        //        }
        //        /// <summary>
        //        /// When selecting 'Reallocate to DC' as the reason for the Continue Debt Review AND a FORECLOSURE UNDERWAY detail type
        //        /// exists on the account, then the LEGAL ACTION STOPPED detail type must be added to the account and the case is moved back to the Review
        //        /// Notification state. This should be added even if there has previously been a LEGAL ACTION STOPPED detail type on the account, as long
        //        /// as the FORECLOSURE UNDERWAY detail type has been added after the LEGAL ACTION STOPPED detail type
        //        /// </summary>
        //        [Test, Description(@"When selecting 'Reallocate to DC' as the reason for the Continue Debt Review AND a FORECLOSURE UNDERWAY detail type
        //        exists on the account, then the LEGAL ACTION STOPPED detail type must be added to the account and the case is moved back to the Review
        //        Notification state. This should be added even if there has previously been a LEGAL ACTION STOPPED detail type on the account, as long
        //        as the FORECLOSURE UNDERWAY detail type has been added after the LEGAL ACTION STOPPED detail type")]
        //        public void ForeclosureUnderwayDetailTypeExistsWithPreviousLegalActionStopped()
        //        {
        //            Console.WriteLine(String.Format(@"--********{0}********--", MethodBase.GetCurrentMethod()));
        //            //search for a case at Pend Proposal
        //            int debtCounsellingKey = 0;
        //            Int64 instanceID;
        //            int accountKey = 0;
        //            string assignedDCCUser;
        //            WorkflowHelper.SearchForCaseAtState(WorkflowStates.DebtCounsellingWF.Litigation, TestUsers.ForeclosureConsultant, out instanceID,
        //                out debtCounsellingKey, out accountKey, out assignedDCCUser, out browser);
        //            if (debtCounsellingKey == 0)
        //            {
        //                WorkflowHelper.CreateAndProcessDebtCounsellingCase(out browser, WorkflowStates.DebtCounsellingWF.Litigation, out debtCounsellingKey, out instanceID,
        //                    out accountKey, out assignedDCCUser, eWorkCase: true);
        //            }
        //            //we need to remove the detail types
        //            Service<IDetailTypeService>().RemoveDetailTypes(accountKey);
        //            //add a legal action stopped
        //            Service<IDetailTypeService>().InsertDetailType(DetailTypeEnum.LegalActionStopped, accountKey);
        //            //add the foreclosure detail type
        //            Service<IDetailTypeService>().InsertDetailType(DetailTypeEnum.ForeclosureUnderway, accountKey);
        //            browser.Navigate<Navigation.DebtCounsellingWorkFlow>().ContinueDebtReview();
        //            //confirm
        //            browser.Page<CommonReasonCommonDecline>().SelectReasonAndSubmit(ReasonType.LitigationReallocation, ReasonDescription.ReallocatetoDC);
        //            //wait
        //            Thread.Sleep(5000);
        //            var results = Service<IAssignmentService>().GetActiveWorkflowRoleByTypeAndGenericKey(WorkflowRoleTypeEnum.DebtCounsellingAdminD, debtCounsellingKey);
        //            string adUserName = (from r in results select r.Value).FirstOrDefault();
        //            //should now be assigned to this users worklist
        //            DebtCounsellingAssertions.AssertLatestDebtCounsellingWorkflowRoleAssignment(debtCounsellingKey, adUserName,
        //                WorkflowRoleTypeEnum.DebtCounsellingAdminD, true, true);
        //            //check that the workflow role exists
        //            WorkflowRoleAssignmentAssertions.AssertActiveWorkflowRoleExists(debtCounsellingKey, WorkflowRoleTypeEnum.DebtCounsellingAdminD,
        //                adUserName);
        //            //check that it has moved states
        //            DebtCounsellingAssertions.AssertX2State(debtCounsellingKey, WorkflowStates.DebtCounsellingWF.ReviewNotification);
        //            //check the ework case has moved.
        //            eWorkAssertions.AssertEworkCaseExists(accountKey.ToString(), @"%Debt Counselling%", EworkMaps.LossControl);
        //            //there should still be a new legal action stopped detail type as the last detail type is foreclosure underway
        //            AccountAssertions.AssertDetailType(accountKey, DetailTypeEnum.LegalActionStopped, DetailClassEnum.LoanManagement, true);
        //            //The ework usertodo should not be assigned when 'Reallocate to DC' reason selected
        //            eWorkAssertions.AssertLossControlUserToDo(accountKey.ToString(), EworkMaps.LossControl, @"Debt Counselling%", string.Empty);
        //        }
    }
}