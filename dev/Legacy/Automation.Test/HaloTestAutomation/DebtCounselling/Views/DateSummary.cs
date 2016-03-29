using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation;
using BuildingBlocks.Presenters.DebtCounselling;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using WatiN.Core;
using Description = NUnit.Framework.DescriptionAttribute;

namespace DebtCounsellingTests.Views
{
    [RequiresSTA]
    public class DateSummaryTests : DebtCounsellingTests.TestBase<DebtCounsellingDocumentChecklist>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.DebtCounsellingConsultant);
        }

        /// <summary>
        /// This test ensures that a 17.1 date can be captured using the date summary screen. It captures multiple dates in order to ensure that the screen
        /// correctly displays the latest stage transition record of that type.
        /// </summary>
        [Test, Description(@"This test ensures that a 17.1 date can be captured using the date summary screen. It captures multiple dates in order to ensure that the screen
        correctly displays the latest stage transition record of that type.")]
        public void Capture17pt1Date()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.PendProposal, TestUsers.DebtCounsellingConsultant);
            base.Browser.Navigate<DebtCounsellingNode>().DebtCounsellingSummaryNode();
            base.Browser.Navigate<DebtCounsellingNode>().DateSummaryNode();
            string comment = "17.1 Comment";
            DateTime _17pt1Date = DateTime.Now;
            base.Browser.Page<DebtCounsellingDocumentChecklist>().CaptureDate(_17pt1Date, comment, "17.1");
            StageTransitionAssertions.AssertStageTransitionCreated(base.TestCase.DebtCounsellingKey, Common.Enums.StageDefinitionStageDefinitionGroupEnum.DebtCounselling_17_1Received
                , comment: comment);
            base.Browser.Page<DebtCounsellingDocumentChecklist>().AssertRowExists(_17pt1Date.ToString(Formats.DateFormat));
            comment = "17.1 Comment: New";
            _17pt1Date = DateTime.Now.AddDays(-2);
            base.Browser.Page<DebtCounsellingDocumentChecklist>().CaptureDate(_17pt1Date, comment, "17.1");
            StageTransitionAssertions.AssertStageTransitionCreated(base.TestCase.DebtCounsellingKey, Common.Enums.StageDefinitionStageDefinitionGroupEnum.DebtCounselling_17_1Received
                , comment: comment);
            base.Browser.Page<DebtCounsellingDocumentChecklist>().AssertRowExists(_17pt1Date.ToString(Formats.DateFormat));
        }

        /// <summary>
        /// This test ensures that a 17.2 date can be captured using the date summary screen. It captures multiple dates in order to ensure that the screen
        /// correctly displays the latest stage transition record of that type.
        /// </summary>
        [Test, Description(@"This test ensures that a 17.1 date can be captured using the date summary screen. It captures multiple dates in order to ensure that the screen
        correctly displays the latest stage transition record of that type.")]
        public void Capture17pt2Date()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.PendProposal, TestUsers.DebtCounsellingConsultant);
            base.Browser.Navigate<DebtCounsellingNode>().DebtCounsellingSummaryNode();
            base.Browser.Navigate<DebtCounsellingNode>().DateSummaryNode();
            string comment = "17.2 Comment";
            DateTime _17pt2Date = DateTime.Now;
            base.Browser.Page<DebtCounsellingDocumentChecklist>().CaptureDate(_17pt2Date, comment, "17.2");
            StageTransitionAssertions.AssertStageTransitionCreated(base.TestCase.DebtCounsellingKey, Common.Enums.StageDefinitionStageDefinitionGroupEnum.DebtCounselling_17_2Received
                , comment: comment);
            base.Browser.Page<DebtCounsellingDocumentChecklist>().AssertRowExists(_17pt2Date.ToString(Formats.DateFormat));
            comment = "17.2 Comment: New";
            _17pt2Date = DateTime.Now.AddDays(-2);
            base.Browser.Page<DebtCounsellingDocumentChecklist>().CaptureDate(_17pt2Date, comment, "17.2");
            StageTransitionAssertions.AssertStageTransitionCreated(base.TestCase.DebtCounsellingKey, Common.Enums.StageDefinitionStageDefinitionGroupEnum.DebtCounselling_17_2Received
                , comment: comment);
            base.Browser.Page<DebtCounsellingDocumentChecklist>().AssertRowExists(_17pt2Date.ToString(Formats.DateFormat));
        }

        /// <summary>
        /// The test inserts multiple court details against the debt counselling case, ensuring that the latest date is displayed.
        /// </summary>
        [Test, Description(@"The test inserts multiple court details against the debt counselling case, ensuring that the latest date is displayed.")]
        public void CheckLatestHearingDate()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.PendProposal, TestUsers.DebtCounsellingConsultant);
            //insert court details
            DateTime hearingDate = DateTime.Now.AddDays(5);
            base.Service<ICourtDetailsService>().InsertCourtDetails(base.TestCase.DebtCounsellingKey, HearingTypeEnum.Court, HearingAppearanceTypeEnum.CourtAppeal, hearingDate);
            base.Browser.Navigate<DebtCounsellingNode>().DebtCounsellingSummaryNode();
            base.Browser.Navigate<DebtCounsellingNode>().DateSummaryNode();
            base.Browser.Page<DebtCounsellingDocumentChecklist>().AssertRowExists(hearingDate.ToString(Formats.DateFormat));
            hearingDate = DateTime.Now.AddDays(12);
            base.Service<ICourtDetailsService>().InsertCourtDetails(base.TestCase.DebtCounsellingKey, HearingTypeEnum.Court, HearingAppearanceTypeEnum.CourtAppeal, hearingDate);
            base.Browser.Navigate<DebtCounsellingNode>().DebtCounsellingSummaryNode();
            base.Browser.Navigate<DebtCounsellingNode>().DateSummaryNode();
            base.Browser.Page<DebtCounsellingDocumentChecklist>().AssertRowExists(hearingDate.ToString(Formats.DateFormat));
        }

        /// <summary>
        /// The test will perform the Change in Circumstance action against a debt counsellling case and then navigate to the Date Summary screen in
        /// order to ensure that it is correctly being displayed.
        /// </summary>
        [Test, Description(@"The test will perform the Change in Circumstance action against a debt counsellling case and then navigate to the Date Summary screen in
        order to ensure that it is correctly being displayed.")]
        public void Check17pt3Date()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.PendPayment, TestUsers.DebtCounsellingConsultant);
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.ChangeinCircumstance);
            DateTime date = DateTime.Now.AddDays(-2);
            base.Browser.Page<ChangeInCircumstance>().Enter17pt3Date(date);
            base.Browser.Page<ChangeInCircumstance>().ClickSave();
            //case moves states
            DebtCounsellingAssertions.AssertX2State(base.TestCase.DebtCounsellingKey, WorkflowStates.DebtCounsellingWF.ManageProposal);
            base.Browser.Navigate<DebtCounsellingNode>().DebtCounsellingSummaryNode();
            base.Browser.Navigate<DebtCounsellingNode>().DateSummaryNode();
            base.Browser.Page<DebtCounsellingDocumentChecklist>().AssertRowExists(date.ToString(Formats.DateFormat));
        }

        /// <summary>
        /// The test retrieves the value of the 60 Days scheduled activity timer against the instance and then ensures that the Date Summary screen correctly
        /// displays the timer date.
        /// </summary>
        [Test, Description(@"The test retrieves the value of the 60 Days scheduled activity timer against the instance and then ensures that the Date Summary screen correctly
        displays the timer date.")]
        public void Check60DayTimerDate()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.PendProposal, TestUsers.DebtCounsellingConsultant);
            var results = Service<IX2WorkflowService>().GetScheduledActivityTime(base.TestCase.AccountKey.ToString(), ScheduledActivities.DebtCounselling._60days);
            DateTime activityDate = results.Rows(0).Column("SCHEDULE").GetValueAs<DateTime>();
            base.Browser.Navigate<DebtCounsellingNode>().DebtCounsellingSummaryNode();
            base.Browser.Navigate<DebtCounsellingNode>().DateSummaryNode();
            base.Browser.Page<DebtCounsellingDocumentChecklist>().AssertRowExists(activityDate.ToString(Formats.DateFormat));
        }

        /// <summary>
        /// The test will try and find a case at the Payment Review state. Cases at this state should have a review date, so the review date is retrieved and we
        /// navigate to the Date Summary screen to ensure that the date is being displayed correctly.
        /// </summary>
        //        [Test, Description(@"The test will try and find a case at the Payment Review state. Cases at this state should have a review date, so the review date is retrieved and we
        //        navigate to the Date Summary screen to ensure that the date is being displayed correctly.")]
        //        public void CheckReviewDate()
        //        {
        //TODO: REQUIRES REFACTOR
        //            SearchForCase(WorkflowStates.DebtCounsellingWF.PaymentReview, TestUsers.DebtCounsellingSupervisor);
        //            string reviewDate = Service<IProposalService>().GetReviewDateOfAcceptedProposal(debtCounsellingKey, ProposalType.Proposal);
        //            base.Browser.Navigate<DebtCounsellingNode>().DebtCounsellingSummaryNode();
        //            base.Browser.Navigate<DebtCounsellingNode>().DateSummaryNode();
        //            base.Browser.Page<DebtCounsellingDocumentChecklist>().AssertRowExists(Convert.ToDateTime(reviewDate).ToString(Formats.DateFormat));
        //        }
    }
}