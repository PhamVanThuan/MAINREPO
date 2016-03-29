using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.Origination.FurtherLending;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;

namespace FurtherLendingTests.Workflow
{
    [RequiresSTA]
    public class QATests : TestBase<BasePage>
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

        protected override void OnTestTearDown()
        {
            base.OnTestTearDown();
        }

        /// <summary>
        /// A FL Processor can perform the Query on Application action on a case at the Manage Application state. Unlike the
        /// New Business case the workflow case remains assigned to the FL Processor, they can select a reason for the Application
        /// Query and move the case to the Application Query state. The test ensures the following occur when performing this action:
        /// <list type="bullet">
        /// <item>
        /// <description>The case is moved to the Application Query state</description>
        /// </item>
        /// <item>
        /// <description>The workflow assignment is not affected and the case is assigned to the same user.</description>
        /// </item>
        /// <item>
        /// <description>The reason provided for the Application Query exists against the Offer</description>
        /// </item>
        /// </list>
        /// Note that this test case searches for an Application at the Manage Application state and therefore gets the assigned user
        /// and account number for the test case from the Further Lending Application summaru view.
        /// </summary>
        [Test, Description("A FL Processor can perform the Query on Application action at Manage Application state"), Category("Further Advances")]
        public void _01_QueryOnApplication()
        {
            int offerKey = Service<IX2WorkflowService>().GetOfferKeyAtStateByType(WorkflowStates.ApplicationManagementWF.ManageApplication, Workflows.ApplicationManagement,
                OfferTypeEnum.FurtherAdvance, "FLAutomation");
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.ApplicationManagementWF.ManageApplication);
            //perform the action
            string adUserName = base.Browser.Page<ApplicationSummaryFurtherLending>().GetApplicationProcessor();
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.QueryonApplication);
            string selectedReason = base.Browser.Page<CommonReasonCommonDecline>().SelectReasonAndSubmit(ReasonType.ProcessorQuery);
            ReasonAssertions.AssertReason(selectedReason, ReasonType.ProcessorQuery, offerKey, GenericKeyTypeEnum.Offer_OfferKey);
            //assert case has moved
            X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.ApplicationQuery);
            AssignmentAssertions.AssertWorkflowAssignment(adUserName, offerKey, OfferRoleTypeEnum.FLProcessorD);
        }

        /// <summary>
        /// Once the query on the application has been resolved a FL Processor can move the case back to the Manage Application
        /// state by completing the Feedback on Query action. Once this action has been completed this test ensures that:
        /// <list type="bullet">
        /// <item>
        /// <description>The case is moved to the Manage Application state</description>
        /// </item>
        /// <item>
        /// <description>The workflow assignment is not affected and the case is assigned to the same user.</description>
        /// </item>
        /// </list>
        /// Note that this test case searches for an Application at the Manage Application state and therefore gets the assigned user
        /// and account number for the test case from the Further Lending Application summaru view.
        /// </summary>
        [Test, Description("A FL Processor can perform the Feedback on Query action"), Category("Further Advances")]
        public void _02_FeedbackOnQuery()
        {
            int offerKey = Service<IX2WorkflowService>().GetOfferKeyAtStateByType(WorkflowStates.ApplicationManagementWF.ApplicationQuery, Workflows.ApplicationManagement,
                OfferTypeEnum.FurtherAdvance, "FLAutomation");
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.ApplicationManagementWF.ApplicationQuery);
            //perform the action
            string adUserName = base.Browser.Page<ApplicationSummaryFurtherLending>().GetApplicationProcessor();
            //there are rules running on the next action so we clean up the offer data
            Service<IFurtherLendingService>().CleanUpOfferData(offerKey);
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.FeedbackonQuery);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, true);
            //assert case has moved
            X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.ManageApplication);
            AssignmentAssertions.AssertWorkflowAssignment(adUserName, offerKey, OfferRoleTypeEnum.FLProcessorD);
        }
    }
}