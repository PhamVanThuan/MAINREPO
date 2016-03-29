using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;

namespace Origination.Workflow.PreCredit.QA
{
    [RequiresSTA]
    public class QATests : Origination.OriginationTestBase<BasePage>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new BuildingBlocks.TestBrowser(TestUsers.CreditSupervisor);
        }
        protected override void OnTestStart()
        {
        }
        /// <summary>
        /// This test will perform the QA Query action on a New Purchase Application at the QA state.
        /// </summary>
        [Test, Description("This ensures that a user can perfrom a QA query, when unhappy about something on the application")]
        public void QAQuery()
        {
            base.GetOfferKeyAtStateAndSearchForCase(WorkflowStates.ApplicationManagementWF.QA, OfferTypeEnum.NewPurchase);
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.QAQuery);
            base.Browser.Page<CommonReasonCommonDecline>().SelectReasonAndSubmit(ReasonType.QAQuery);
            Service<IX2WorkflowService>().WaitForX2State(base.TestCase.OfferKey, Workflows.ApplicationManagement, WorkflowStates.ApplicationManagementWF.RequestatQA);
            //we need find the active Branch Consultant
            string adUserName = base.Service<IApplicationService>().GetADUserByActiveOfferRoles(base.TestCase.OfferKey, OfferRoleTypeEnum.BranchConsultantD);
            X2Assertions.AssertCurrentAppManX2State(base.TestCase.OfferKey, WorkflowStates.ApplicationManagementWF.RequestatQA);
            AssignmentAssertions.AssertWorkflowAssignment(adUserName, base.TestCase.OfferKey, OfferRoleTypeEnum.BranchConsultantD);
        }

        /// <summary>
        /// Once the QA Admin has performed the QA Complete action on a New Purchase application it is sent to the Issue AIP state and assigned to the current Branch Consultant role on the
        /// application.
        /// </summary>
        [Test, Description("This ensures that the user can complete the quality check")]
        public void QAComplete()
        {
            base.GetOfferKeyAtStateAndSearchForCase(WorkflowStates.ApplicationManagementWF.QA, OfferTypeEnum.NewPurchase);
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.QAComplete);
            //get the next user
            string adUserName = base.Service<IApplicationService>().GetADUserByActiveOfferRoles(base.TestCase.OfferKey, OfferRoleTypeEnum.BranchConsultantD);
            base.Browser.Page<CommonReasonCommonDecline>().SelectReasonAndSubmit(ReasonType.QAComplete);
            //wait for case to be assigned into AIP
            Service<IX2WorkflowService>().WaitForX2State(base.TestCase.OfferKey, Workflows.ApplicationManagement, WorkflowStates.ApplicationManagementWF.IssueAIP);
            //case is now at Issue AIP
            X2Assertions.AssertCurrentAppManX2State(base.TestCase.OfferKey, WorkflowStates.ApplicationManagementWF.IssueAIP);
            //belongs to BCUser
            AssignmentAssertions.AssertWorkflowAssignment(adUserName, base.TestCase.OfferKey, OfferRoleTypeEnum.BranchConsultantD);
            //case is now at Issue AIP
            X2Assertions.AssertCurrentAppManX2State(base.TestCase.OfferKey, WorkflowStates.ApplicationManagementWF.IssueAIP);
            //belongs to BCUser
            AssignmentAssertions.AssertWorkflowAssignment(adUserName, base.TestCase.OfferKey, OfferRoleTypeEnum.BranchConsultantD);
        }
    }
}