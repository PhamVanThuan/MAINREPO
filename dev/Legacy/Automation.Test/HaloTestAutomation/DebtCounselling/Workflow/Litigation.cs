using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.DebtCounselling;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using Common.Extensions;
using NUnit.Framework;
using System;
using WatiN.Core;
using Description = NUnit.Framework.DescriptionAttribute;

namespace DebtCounsellingTests.Workflow
{
    [RequiresSTA]
    public sealed class LitigationTests : DebtCounsellingTests.TestBase<BasePage>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.DebtCounsellingAdmin);
        }

        /// <summary>
        /// This test will try and send a debt counselling case that does not have a corresponding EWork case to the Litigation state. This is not allowed and the user
        /// should be notified and the case does not move.
        /// </summary>
        [Test, Description(@"This test will try and send a debt counselling case that does not have a corresponding EWork case to the Litigation state. This is not allowed
        and the user should be notified and the case does not move.")]
        public void SendToLitigationNoEworkCaseExists()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.ReviewNotification, TestUsers.DebtCounsellingAdmin, false, 1, false, searchForCase: false);
            Assert.False(base.Browser.Page<DebtCounsellingSummaryReview>().eWorkLabelExists(), "Error message should not be displayed.");
            //try and send the case to ligitation
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.SendtoLitigation);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, false);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("There is no active case in the E-Work loss control map.");
            X2Assertions.AssertCurrentX2State(base.TestCase.InstanceID, WorkflowStates.DebtCounsellingWF.ReviewNotification);
        }

        /// <summary>
        /// This test will ensure that if a loss control case exists for a debt counselling case then the Ework user and the Ework stage are correctly
        /// displayed on the debt counselling summary.
        /// </summary>
        [Test, Description(@"This test will ensure that if a loss control case exists for a debt counselling case then the Ework user and the Ework stage are correctly
        displayed on the debt counselling summary.")]
        public void SendToLitigationEworkCaseExists()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.ReviewNotification, TestUsers.DebtCounsellingAdmin, true, 1, true, searchForCase: false);
            string errorMsg = base.Browser.Page<DebtCounsellingSummaryReview>().GetEworkLossControlLabel();
            StringAssert.AreEqualIgnoringCase("A loss control case exists for this account in e-work - please see details below.", errorMsg);
            //check the user
            base.Browser.Page<DebtCounsellingSummaryReview>().ClickEworkCaseDetailsLink();
            string eWorkUser = Service<IEWorkService>().GetEworkUser(base.TestCase.AccountKey);
            StringAssert.AreEqualIgnoringCase(String.Format(@"SAHL\{0}", eWorkUser),
                base.Browser.Page<DebtCounsellingSummaryReview>().GetEworkAssignedUserLabel());
            //check the ework state
            string eWorkState = Service<IEWorkService>().GetEworkStage(base.TestCase.AccountKey);
            StringAssert.AreEqualIgnoringCase(eWorkState, base.Browser.Page<DebtCounsellingSummaryReview>().GetEworkStageLabel());
        }

        /// <summary>
        /// This test will update the Ework user linked to the debt counselling case to be the Foreclosure Consultant and send the case to the Litigation state.
        /// </summary>
        [Test, Description(@"This test will update the Ework user linked to the debt counselling case to be the Foreclosure Consultant and send the case to the Litigation state.")]
        public void SendToLitigationTestUser()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.ReviewNotification, TestUsers.DebtCounsellingAdmin, true, 1, true, searchForCase: false);
            string eStageName = Service<IEWorkService>().GetEworkStage(base.TestCase.AccountKey);
            string eworkTestUser = TestUsers.ForeclosureConsultant;
            //we need to update this to belong to our test user
            Service<IEWorkService>().UpdateEworkAssignedUser(base.TestCase.AccountKey, eworkTestUser, eStageName);
            //complete the action
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.SendtoLitigation);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, false);
            //check that it has moved states
            DebtCounsellingAssertions.AssertX2State(base.TestCase.DebtCounsellingKey, WorkflowStates.DebtCounsellingWF.Litigation);
            DebtCounsellingAssertions.AssertLatestDebtCounsellingWorkflowRoleAssignment(base.TestCase.DebtCounsellingKey, TestUsers.ForeclosureConsultant,
                    WorkflowRoleTypeEnum.ForeclosureConsultantD, true, true);
            //check that the workflow role exists
            WorkflowRoleAssignmentAssertions.AssertActiveWorkflowRoleExists(base.TestCase.DebtCounsellingKey, WorkflowRoleTypeEnum.ForeclosureConsultantD,
                TestUsers.ForeclosureConsultant);
        }

        /// <summary>
        /// If the user currently assigned to the Ework case is not setup as a foreclosure consultant then we cannot assign the case to them in X2. In this case the
        /// user will be displayed a message and the case will not move states.
        /// </summary>
        [Test, Description(@"If the user currently assigned to the Ework case is not setup as a foreclosure consultant then we cannot assign the case to them in X2.
        In this case the user will be displayed a message and the case will not move states.")]
        public void SendToLitigationAssignedUserNotForeclosureConsultant()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.ReviewNotification, TestUsers.DebtCounsellingAdmin, true, 1, true, searchForCase: false);
            string eWorkTestUser = TestUsers.DebtCounsellingAdmin;
            string eStageName = Service<IEWorkService>().GetEworkStage(base.TestCase.AccountKey);
            //we need to update this to belong to our test user
            Service<IEWorkService>().UpdateEworkAssignedUser(base.TestCase.AccountKey, eWorkTestUser, eStageName);
            //complete the action
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.SendtoLitigation);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, false);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(
                string.Format(@"The E-Work User ({0}) assigned to this case is not a Foreclosure Consultant", TestUsers.DebtCounsellingAdmin));
        }

        /// <summary>
        ///
        /// </summary>
        [Test, Description("When sending the case to litigation and there is no ework user assigned, the user should be prevented from sending the case.")]
        public void SendToLitigationNoAssignedUser()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.ReviewNotification, TestUsers.DebtCounsellingAdmin, true, 1, true, searchForCase: false);
            string eWorkTestUser = String.Empty;
            string eStageName = Service<IEWorkService>().GetEworkStage(base.TestCase.AccountKey);
            //we need to update this to belong to our test user
            Service<IEWorkService>().UpdateEworkAssignedUser(base.TestCase.AccountKey, eWorkTestUser, eStageName);
            //complete the action
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.SendtoLitigation);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, false);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("There is no E-Work User assigned to this case.");
        }

        /// <summary>
        ///
        /// </summary>
        [Test, Description("")]
        public void SendToLitigitationInactiveUser()
        {
            try
            {
                base.StartTest(WorkflowStates.DebtCounsellingWF.ReviewNotification, TestUsers.DebtCounsellingAdmin, true, 1, true, searchForCase: false);
                this.Service<IADUserService>().UpdateADUserStatus(TestUsers.ForeclosureConsultant2, GeneralStatusEnum.Inactive, GeneralStatusEnum.Active,
                    GeneralStatusEnum.Inactive);
                //reload the cache
                base.scriptEngine.ClearCacheFor(Processes.DebtCounselling, Workflows.DebtCounselling, CacheTypes.DomainService);                
                string eWorkTestUser = TestUsers.ForeclosureConsultant2.RemoveDomainPrefix();
                string eStageName = Service<IEWorkService>().GetEworkStage(base.TestCase.AccountKey);
                //we need to update this to belong to our test user
                Service<IEWorkService>().UpdateEworkAssignedUser(base.TestCase.AccountKey, eWorkTestUser, eStageName);
                //complete the action
                base.Browser.ClickAction(WorkflowActivities.DebtCounselling.SendtoLitigation);
                base.Browser.Page<WorkflowYesNo>().Confirm(true, false);
                base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(@"The E-Work User assigned to this case is inactive.");
            }
            finally
            {
                this.Service<IADUserService>().UpdateADUserStatus(TestUsers.ForeclosureConsultant2, GeneralStatusEnum.Active, GeneralStatusEnum.Active, GeneralStatusEnum.Active);
                //reload the cache
                base.scriptEngine.ClearCacheFor(Processes.DebtCounselling, Workflows.DebtCounselling, CacheTypes.DomainService); 
            }
        }
    }
}