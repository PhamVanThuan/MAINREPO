using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using WatiN.Core.Logging;

namespace DebtCounsellingTests.Workflow
{
    /// <summary>
    /// This test fixture contains tests required to test the debt counselling assignment
    /// </summary>
    [TestFixture, RequiresSTA]
    public sealed class DebtCounsellingAssignment : DebtCounsellingTests.TestBase<BasePage>
    {
        #region SetupTearDown

        protected override void OnTestFixtureSetup()
        {
            Logger.LogWriter = new ConsoleLogWriter();
            base.OnTestFixtureSetup();
            SetTestUsersToActive();
        }

        protected override void OnTestTearDown()
        {
            base.OnTestTearDown();
            SetTestUsersToActive();
        }

        private void SetTestUsersToActive()
        {
            //ensure that we set any users that we have updated back to normal
            base.Service<IADUserService>().UpdateStatusOfAllUsersMappedToWorkflowRoleType(WorkflowRoleTypeEnum.DebtCounsellingConsultantD, GeneralStatusEnum.Active, true, true, true);
            //revert the business users back to inactive
            Service<IDebtCounsellingService>().MakeDebtCounsellingBusinessUsersInactive();
            //reload the cache
            base.scriptEngine.ClearCacheFor("Debt Counselling", "Debt Counselling", CacheTypes.DomainService);
        }

        #endregion SetupTearDown

        /// <summary>
        /// This test case will send a group of cases through the Respond to Debt Counsellor action, ensuring that the initial Debt Counselling Consultant
        /// user gets assigned to the subsequent case.
        /// </summary>
        [Test, Description(@"This test case will send a group of cases through the Respond to Debt Counsellor action, ensuring that the initial Debt Counselling Consultant
        user gets assigned to the subsequent case.")]
        public void _001_GroupAssignmentShouldAssignTheSameConsultantToAGroupOfCases()
        {
            var accounts = WorkflowHelper.CreateCaseUsingWorkflowAutomation(false, 2, false);
            string initialConsultant = string.Empty;
            foreach (var acc in accounts)
            {
                //add to worklist
                int debtCounsellingKey = Service<IDebtCounsellingService>().GetDebtCounsellingKey(acc.AccountKey);
                string adUserName = Service<IAssignmentService>().GetUserForDebtCounsellingAssignment(WorkflowRoleTypeEnum.DebtCounsellingConsultantD, Workflows.DebtCounselling,
                    DebtCounsellingLoadBalanceStates.consultantAssignmentExclusionStates, false, debtCounsellingKey);
                base.scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.RespondToDebtCounsellor, debtCounsellingKey);
                Service<IX2WorkflowService>().WaitForX2State(debtCounsellingKey, Workflows.DebtCounselling, WorkflowStates.DebtCounsellingWF.PendProposal);
                //Assertions
                DebtCounsellingAssertions.AssertX2StateByAccountKey(acc.AccountKey, WorkflowStates.DebtCounsellingWF.PendProposal);
                WorkflowRoleAssignmentAssertions.AssertActiveWorkflowRoleExists(debtCounsellingKey, WorkflowRoleTypeEnum.DebtCounsellingConsultantD, adUserName);
                int instanceID = Service<IX2WorkflowService>().GetInstanceIDByDebtCounsellingKey(debtCounsellingKey);
                WorkflowRoleAssignmentAssertions.AssertLatestDebtCounsellingAssignment(instanceID, adUserName, WorkflowRoleTypeEnum.DebtCounsellingConsultantD, true, true);
                if (acc.AccountKey == accounts[0].AccountKey)
                    initialConsultant = adUserName;
                if (acc.AccountKey != accounts[0].AccountKey)
                    Assert.That(adUserName == initialConsultant, string.Format("A new consultant: {0} was assigned to the second case in the group, we expected the same consultant: {1}", adUserName, initialConsultant));
            }
        }

        /// <summary>
        /// This test will ensure that a user cannot proceed in the workflow if there are no user with an active ADUser.GeneralStatus to assign the case to.
        /// </summary>
        [Test, Description(@"This test will ensure that a user cannot proceed in the workflow if there are no user with an active ADUser.GeneralStatus to assign the case to.")]
        public void _002_WhenNoActiveADUsersExistsForWorkflowRoleTypeNoAssignmentCanTakePlace()
        {
            //we need to make ALL of our users inactive
            base.Service<IADUserService>().UpdateStatusOfAllUsersMappedToWorkflowRoleType(WorkflowRoleTypeEnum.DebtCounsellingConsultantD, GeneralStatusEnum.Inactive, false, true, false);
            //reload the cache
            base.scriptEngine.ClearCacheFor("Debt Counselling", "Debt Counselling", CacheTypes.DomainService);
            var accounts = WorkflowHelper.CreateCaseUsingWorkflowAutomation(true, 1, true);
            foreach (var acc in accounts)
            {
                int debtCounsellingKey = Service<IDebtCounsellingService>().GetDebtCounsellingKey(acc.AccountKey);
                int instanceID = Service<IX2WorkflowService>().GetInstanceIDByDebtCounsellingKey(debtCounsellingKey);
                try
                {
                    var results = base.scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.RespondToDebtCounsellor, debtCounsellingKey);
                }
                catch (System.Exception ex)
                {
                    Assert.That(ex.ToString().Contains(string.Format(@"Could not find a user to assign to the case: InstanceID : {0}", instanceID)));
                }
            }
        }

        /// <summary>
        /// This test will ensure that a user cannot proceed in the workflow if there are no user with an active UOS.GeneralStatus to assign the case to.
        /// </summary>
        [Test, Description(@"This test will ensure that a user cannot proceed in the workflow if there are no user with an active UOS.GeneralStatus to assign the case to.")]
        public void _002_WhenNoActiveRoundRobinUsersExistsForWorkflowRoleTypeNoAssignmentCanTakePlace()
        {
            //we need to make ALL of our users inactive
            base.Service<IADUserService>().UpdateStatusOfAllUsersMappedToWorkflowRoleType(WorkflowRoleTypeEnum.DebtCounsellingConsultantD, GeneralStatusEnum.Inactive, true, false, false);
            //reload the cache
            base.scriptEngine.ClearCacheFor(Processes.DebtCounselling, Workflows.DebtCounselling, CacheTypes.DomainService);
            var accounts = WorkflowHelper.CreateCaseUsingWorkflowAutomation(true, 1, true);
            foreach (var acc in accounts)
            {
                int debtCounsellingKey = Service<IDebtCounsellingService>().GetDebtCounsellingKey(acc.AccountKey);
                int instanceID = Service<IX2WorkflowService>().GetInstanceIDByDebtCounsellingKey(debtCounsellingKey);
                try
                {
                    var results = base.scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.RespondToDebtCounsellor, debtCounsellingKey);
                }
                catch (System.Exception ex)
                {
                    Assert.That(ex.ToString().Contains(string.Format(@"Could not find a user to assign to the case: InstanceID : {0}", instanceID)));
                }
            }
        }

        /// <summary>
        /// This test will ensure that we do not try and assign the user that already exists on the group of cases if that user has been set to inactive. A new user should
        /// be added to the case via the load balance assignment.
        /// </summary>
        [Test, Description(@"This test will ensure that we do not try and assign the user that already exists on the group of cases if that user has been set to inactive.
        A new user should be added to the case via the load balance assignment.")]
        public void _004_AssignDifferentUserToGroupIfGroupUserIsInactive()
        {
            var accounts = WorkflowHelper.CreateCaseUsingWorkflowAutomation(false, 2, false);
            string initialConsultant = string.Empty;
            foreach (var acc in accounts)
            {
                //add to worklist
                int debtCounsellingKey = Service<IDebtCounsellingService>().GetDebtCounsellingKey(acc.AccountKey);
                string adUserName = Service<IAssignmentService>().GetUserForDebtCounsellingAssignment(WorkflowRoleTypeEnum.DebtCounsellingConsultantD, Workflows.DebtCounselling,
                    DebtCounsellingLoadBalanceStates.consultantAssignmentExclusionStates, false, debtCounsellingKey);
                base.scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.RespondToDebtCounsellor, debtCounsellingKey);
                //Assertions
                Service<IX2WorkflowService>().WaitForX2State(debtCounsellingKey, Workflows.DebtCounselling, WorkflowStates.DebtCounsellingWF.PendProposal);
                DebtCounsellingAssertions.AssertX2StateByAccountKey(acc.AccountKey, WorkflowStates.DebtCounsellingWF.PendProposal);
                WorkflowRoleAssignmentAssertions.AssertActiveWorkflowRoleExists(debtCounsellingKey, WorkflowRoleTypeEnum.DebtCounsellingConsultantD, adUserName);
                int instanceID = Service<IX2WorkflowService>().GetInstanceIDByDebtCounsellingKey(debtCounsellingKey);
                WorkflowRoleAssignmentAssertions.AssertLatestDebtCounsellingAssignment(instanceID, adUserName, WorkflowRoleTypeEnum.DebtCounsellingConsultantD, true, true);
                if (acc.AccountKey == accounts[0].AccountKey)
                {
                    initialConsultant = adUserName;
                    //we need to set our user to be inactive for the next case to pick up a new user
                    this.Service<IADUserService>().UpdateADUserStatus(adUserName, GeneralStatusEnum.Inactive, GeneralStatusEnum.Inactive, GeneralStatusEnum.Inactive);
                    base.scriptEngine.ClearCacheFor(Processes.DebtCounselling, Workflows.DebtCounselling, CacheTypes.DomainService);
                }
                if (acc.AccountKey != accounts[0].AccountKey)
                    Assert.That(adUserName != initialConsultant, "A new consultant was not assigned to the second case in the group.");
            }
        }

        /// <summary>
        /// The group assignment for debt counselling will assign the case to the same user if both the ADUser.GeneralStatus and UOS.GeneralStatus are set to ACTIVE. If
        /// the ROUND ROBIN STATUS is set to Inactive we should still be assigning the case to the user as this status governs only the load balance assignment.
        /// </summary>
        [Test, Description(@"The group assignment for debt counselling will assign the case to the same user if both the ADUser.GeneralStatus and UOS.GeneralStatus are
        set to ACTIVE. If the ROUND ROBIN STATUS is set to Inactive we should still be assigning the case to the user as this status governs only the load balance
        assignment.")]
        public void _005_AssignSameUserToGroupIfGroupUserRoundRobinStatusIsInactive()
        {
            var accounts = WorkflowHelper.CreateCaseUsingWorkflowAutomation(false, 2, false);
            string initialConsultant = string.Empty;
            foreach (var acc in accounts)
            {
                //add to worklist
                int debtCounsellingKey = Service<IDebtCounsellingService>().GetDebtCounsellingKey(acc.AccountKey);
                string adUserName = Service<IAssignmentService>().GetUserForDebtCounsellingAssignment(WorkflowRoleTypeEnum.DebtCounsellingConsultantD, Workflows.DebtCounselling,
                    DebtCounsellingLoadBalanceStates.consultantAssignmentExclusionStates, false, debtCounsellingKey);
                base.scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.RespondToDebtCounsellor, debtCounsellingKey);
                //Assertions
                Service<IX2WorkflowService>().WaitForX2State(debtCounsellingKey, Workflows.DebtCounselling, WorkflowStates.DebtCounsellingWF.PendProposal);
                DebtCounsellingAssertions.AssertX2StateByAccountKey(acc.AccountKey, WorkflowStates.DebtCounsellingWF.PendProposal);
                WorkflowRoleAssignmentAssertions.AssertActiveWorkflowRoleExists(debtCounsellingKey, WorkflowRoleTypeEnum.DebtCounsellingConsultantD, adUserName);
                int instanceID = Service<IX2WorkflowService>().GetInstanceIDByDebtCounsellingKey(debtCounsellingKey);
                WorkflowRoleAssignmentAssertions.AssertLatestDebtCounsellingAssignment(instanceID, adUserName,
                    WorkflowRoleTypeEnum.DebtCounsellingConsultantD, true, true);
                if (acc.AccountKey == accounts[0].AccountKey)
                {
                    initialConsultant = adUserName;
                    //we need to set the round robin status on our user to be inactive
                    this.Service<IADUserService>().UpdateADUserStatus(adUserName, GeneralStatusEnum.Active, GeneralStatusEnum.Inactive, GeneralStatusEnum.Active);
                    base.scriptEngine.ClearCacheFor(Processes.DebtCounselling, Workflows.DebtCounselling, CacheTypes.DomainService);
                }
                if (acc.AccountKey != accounts[0].AccountKey)
                    Assert.That(initialConsultant == adUserName, "A new consultant was assigned to the case in our group when we expected it not to be.");
            }
        }

        [Test]
        [Repeat(5)]
        public void _006_CheckRoundRobinAssignmentAtPendProposal()
        {
            string nextUserForRoundRobinAssignment = Service<IAssignmentService>().GetNextLoadBalanceAssignment(WorkflowRoleTypeEnum.DebtCounsellingConsultantD, Workflows.DebtCounselling,
                Common.Constants.DebtCounsellingLoadBalanceStates.consultantAssignmentExclusionStates, false);
            base.StartTest(WorkflowStates.DebtCounsellingWF.PendProposal, TestUsers.DebtCounsellingConsultant, false, 1, false, searchForCase: false);
            WorkflowRoleAssignmentAssertions.AssertActiveWorkflowRoleExists(base.TestCase.DebtCounsellingKey, WorkflowRoleTypeEnum.DebtCounsellingConsultantD, nextUserForRoundRobinAssignment);
        }
    }
}