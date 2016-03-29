#region namespaces

using Automation.DataModels;
using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using Common.Extensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using WorkflowAutomation.Harness;

#endregion namespaces

namespace Origination.Workflow
{
    [RequiresSTA]
    public class _07ValuationTests : OriginationTestBase<BasePage>
    {
        protected override void OnTestStart()
        {
        }

        protected override void OnTestTearDown()
        {
        }

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            WatiN.Core.Settings.WaitUntilExistsTimeOut = 10;
            base.Browser = new TestBrowser(Common.Constants.TestUsers.ValuationsProcessor2);
            base.Browser.Navigate<NavigationHelper>().Task();
            var offersAtManagerReview = Service<IX2WorkflowService>().GetOffersAtState(WorkflowStates.ValuationsWF.ManagerReview, Workflows.Valuations, String.Empty).Take(10);
            if (offersAtManagerReview.Count() < 10)
            {
                var offersAtPendingVal = Service<IX2WorkflowService>().GetOffersAtState(WorkflowStates.ValuationsWF.ValuationAssessmentPending, Workflows.Valuations, String.Empty).Take(10);
                foreach (var valOffer in offersAtPendingVal)
                {
                    Service<IValuationService>().SubmitRejectedEzVal(valOffer);
                    Service<IX2WorkflowService>().WaitForX2State(valOffer, Workflows.Valuations, WorkflowStates.ValuationsWF.ScheduleValuationAssessment);
                    //Escalate to Manager at Schedule Valuation Assessment
                    base.scriptEngine.ExecuteScript(WorkflowEnum.Valuations, WorkflowAutomationScripts.Valuations.EscalateToManager, valOffer);
                }
            }
        }

        [Test]
        public void when_further_valuation_required_should_move_to_further_valuation_request_stage()
        {
            var offerKey = MoveCaseTo(WorkflowStates.ValuationsWF.ManagerReview, false);

            base.Browser.Close();
            base.Browser.Dispose();
            base.Browser = new TestBrowser(TestUsers.ValuationsManager);
            base.Browser.Navigate<NavigationHelper>().Task();

            base.Browser.Navigate<WorkFlowsNode>().WorkFlows(base.Browser);
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.ValuationsWF.ManagerReview);
            base.Browser.ClickAction(WorkflowActivities.Valuations.FurtherValuationRequired);
            base.Browser.Page<WorkflowYesNo>().ClickYes();

            X2Assertions.AssertCurrentValuationsX2State(offerKey, WorkflowStates.ValuationsWF.FurtherValuationRequest);
            StageTransitionAssertions.AssertStageTransitionCreated(offerKey, StageDefinitionStageDefinitionGroupEnum.FurtherValuationRequired);

            base.Browser.Close();
            base.Browser.Dispose();

            base.Browser = new TestBrowser(TestUsers.ValuationsProcessor2);
            base.Browser.Navigate<NavigationHelper>().Task();
        }

        [Test]
        public void when_review_valuation_required_should_move_to_valuation_review_request_stage()
        {
            var offerKey = MoveCaseTo(WorkflowStates.ValuationsWF.ManagerReview, false);
            //added this in as the test was loggin in with ValuationsProcessor2 and actually needed to login with VMuser.
            base.Browser.Close();
            base.Browser.Dispose();
            base.Browser = new TestBrowser(TestUsers.ValuationsManager);

            base.Browser.Navigate<WorkFlowsNode>().WorkFlows(base.Browser);
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.ValuationsWF.ManagerReview);
            base.Browser.ClickAction(WorkflowActivities.Valuations.ReviewValuationRequired);
            base.Browser.Page<WorkflowYesNo>().ClickYes();

            X2Assertions.AssertCurrentValuationsX2State(offerKey, WorkflowStates.ValuationsWF.ValuationReviewRequest);
            StageTransitionAssertions.AssertStageTransitionCreated(offerKey, StageDefinitionStageDefinitionGroupEnum.ReviewValuationRequired);
        }

        [Test]
        public void when_reinstruct_valuer_should_move_to_schedule_valuation_assessment_stage()
        {
            var offerKey = MoveCaseTo(WorkflowStates.ValuationsWF.FurtherValuationRequest, false);

            base.Browser.Navigate<WorkFlowsNode>().WorkFlows(base.Browser);
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.ValuationsWF.FurtherValuationRequest);
            base.Browser.ClickAction(WorkflowActivities.Valuations.ReinstructValuer);
            base.Browser.Page<WorkflowYesNo>().ClickYes();

            X2Assertions.AssertCurrentValuationsX2State(offerKey, WorkflowStates.ValuationsWF.ScheduleValuationAssessment);
            StageTransitionAssertions.AssertStageTransitionCreated(offerKey, StageDefinitionStageDefinitionGroupEnum.ReinstructValuer);
        }

        [Test]
        public void when_request_valuation_review_should_move_to_schedule_valuation_assessment_stage()
        {
            var offerKey = MoveCaseTo(WorkflowStates.ValuationsWF.ValuationReviewRequest, false);

            base.Browser.Navigate<WorkFlowsNode>().WorkFlows(base.Browser);
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.ValuationsWF.ValuationReviewRequest);
            base.Browser.ClickAction(WorkflowActivities.Valuations.RequestValuationReview);
            base.Browser.Page<WorkflowYesNo>().ClickYes();

            X2Assertions.AssertCurrentValuationsX2State(offerKey, WorkflowStates.ValuationsWF.ScheduleValuationAssessment);
            StageTransitionAssertions.AssertStageTransitionCreated(offerKey, StageDefinitionStageDefinitionGroupEnum.RequestValuationReview);
        }

        [Test]
        public void when_instruct_ezVal_valuer_should_move_valuation_case_to_valuation_assessment_pending_stage()
        {
            var offerKey = MoveCaseTo(WorkflowStates.ValuationsWF.ScheduleValuationAssessment, false);

            base.Browser.Navigate<WorkFlowsNode>().WorkFlows(base.Browser);
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.ValuationsWF.ScheduleValuationAssessment);

            //Instruct a case and move to pending valuation assessment
            scriptEngine.ExecuteScript(WorkflowEnum.Valuations, WorkflowAutomationScripts.Valuations.InstructEzValValuer, offerKey);

            //Assert
            var propertyKey = Service<IPropertyService>().GetPropertyByOfferKey(offerKey).FirstOrDefault().Column("propertykey").GetValueAs<int>();
            var valuations = base.Service<IValuationService>().GetValuations(propertyKey);
            var instance = Service<IX2WorkflowService>().GetValuationsInstanceDetails(offerKey);
            var instanceId = instance.FirstOrDefault().Column("instanceid").GetValueAs<int>();
            var instanceValKey = instance.FirstOrDefault().Column("valuationkey").GetValueAs<int>();
            var activeVal = valuations.Where(x => x.IsActive).FirstOrDefault();
            var newVal = valuations.Where(x => x.ValuationKey == instanceValKey).FirstOrDefault();
            Assert.That(newVal.ValuationDate > DateTime.Now.Subtract(TimeSpan.FromMinutes(30)), "expected a new valuation record.");
            if (activeVal != null)
                Assert.AreNotEqual(newVal.ValuationKey, activeVal.ValuationKey
                    , "The valuation that was set active is not the same valuation that is referenced by valuations in workflow");
            Assert.AreEqual(ValuationStatusEnum.Pending, newVal.ValuationStatusKey, "Expected the valuation to be set to pending after valuation instruction");
            StageTransitionAssertions.AssertStageTransitionCreated(offerKey, StageDefinitionStageDefinitionGroupEnum.InstructEzValValuer);
        }

        [Test]
        public void when_ezval_completed_should_submit_back_a_completed_valuation_and_move_to_valuation_completed()
        {
            var offerKey = MoveCaseTo(WorkflowStates.ValuationsWF.ValuationAssessmentPending, false);

            //Complete the valuation by posting completed ezval xml back to halo
            Service<IValuationService>().SubmitCompletedEzVal(offerKey, HOCRoofEnum.Thatch, thatchAmount: 400000f, valuationAmount: 1800000f);
            Service<IX2WorkflowService>().WaitForX2State(offerKey, Workflows.Valuations, WorkflowStates.ValuationsWF.ValuationComplete);

            //Assert
            var instance = Service<IX2WorkflowService>().GetValuationsInstanceDetails(offerKey);
            var instanceId = instance.FirstOrDefault().Column("instanceid").GetValueAs<int>();
            X2Assertions.AssertWorkflowHistoryActivityCount(instanceId, "EXT_Complete_LS", 1, DateTime.Now.Subtract(TimeSpan.FromMinutes(1)));
            var propertyKey = Service<IPropertyService>().GetPropertyByOfferKey(offerKey).FirstOrDefault().Column("propertykey").GetValueAs<int>();
            var valuations = base.Service<IValuationService>().GetValuations(propertyKey);
            instanceId = instance.FirstOrDefault().Column("instanceid").GetValueAs<int>();
            var instanceValKey = instance.FirstOrDefault().Column("valuationkey").GetValueAs<int>();
            var newVal = valuations.Where(x => x.ValuationKey == instanceValKey).FirstOrDefault();
            Assert.AreEqual(ValuationStatusEnum.Complete, newVal.ValuationStatusKey, "Expected the valuation to be set to complete when coming back from ezval");
            StageTransitionAssertions.AssertStageTransitionCreated(offerKey, StageDefinitionStageDefinitionGroupEnum.LightstoneValuationCompleted);
        }

        [Test]
        public void when_ezval_completed_should_submit_back_an_amended_valuation_and_move_to_valuation_completed()
        {
            var offerKey = MoveCaseTo(WorkflowStates.ValuationsWF.ValuationAssessmentPending, true);

            //Complete the valuation by posting amended ezval xml back to halo
            Service<IValuationService>().SubmitAmendedEzVal(offerKey, HOCRoofEnum.Thatch, thatchAmount: 400000f, valuationAmount: 1800000f);
            Service<IX2WorkflowService>().WaitForX2State(offerKey, Workflows.Valuations, WorkflowStates.ValuationsWF.ValuationComplete);

            //Assert
            var instance = Service<IX2WorkflowService>().GetValuationsInstanceDetails(offerKey);
            var instanceId = instance.FirstOrDefault().Column("instanceid").GetValueAs<int>();
            X2Assertions.AssertWorkflowHistoryActivityCount(instanceId, "EXT_Amended_LS", 1, DateTime.Now.Subtract(TimeSpan.FromMinutes(1)));
            var propertyKey = Service<IPropertyService>().GetPropertyByOfferKey(offerKey).FirstOrDefault().Column("propertykey").GetValueAs<int>();
            var valuations = base.Service<IValuationService>().GetValuations(propertyKey);
            instance = Service<IX2WorkflowService>().GetValuationsInstanceDetails(offerKey);
            instanceId = instance.FirstOrDefault().Column("instanceid").GetValueAs<int>();
            var instanceValKey = instance.FirstOrDefault().Column("valuationkey").GetValueAs<int>();
            var newVal = valuations.Where(x => x.ValuationKey == instanceValKey).FirstOrDefault();
            Assert.AreEqual(ValuationStatusEnum.Complete, newVal.ValuationStatusKey, "Expected the valuation to be set to complete when coming back from ezval");
            StageTransitionAssertions.AssertStageTransitionCreated(offerKey, StageDefinitionStageDefinitionGroupEnum.LightstoneValuationAmended);
        }

        [Test]
        public void when_ezval_rejected_should_move_the_case_to_scheduled_valuation_assessment_stage()
        {
            var offerKey = MoveCaseTo(WorkflowStates.ValuationsWF.ValuationAssessmentPending, false);

            //Reject the valuation by posting rejected xml
            Service<IValuationService>().SubmitRejectedEzVal(offerKey);
            Service<IX2WorkflowService>().WaitForX2State(offerKey, Workflows.Valuations, WorkflowStates.ValuationsWF.ScheduleValuationAssessment);

            //Assert
            var instance = Service<IX2WorkflowService>().GetValuationsInstanceDetails(offerKey);
            var instanceId = instance.FirstOrDefault().Column("instanceid").GetValueAs<int>();
            X2Assertions.AssertWorkflowHistoryActivityCount(instanceId, "EXT_Rejected_LS", 1, DateTime.Now.Subtract(TimeSpan.FromMinutes(1)));
            var propertyKey = Service<IPropertyService>().GetPropertyByOfferKey(offerKey).FirstOrDefault().Column("propertykey").GetValueAs<int>();
            var valuations = base.Service<IValuationService>().GetValuations(propertyKey);
            instance = Service<IX2WorkflowService>().GetValuationsInstanceDetails(offerKey);
            instanceId = instance.FirstOrDefault().Column("instanceid").GetValueAs<int>();
            var instanceValKey = instance.FirstOrDefault().Column("valuationkey").GetValueAs<int>();
            var newVal = valuations.Where(x => x.ValuationKey == instanceValKey).FirstOrDefault();
            Assert.AreEqual(ValuationStatusEnum.Returned, newVal.ValuationStatusKey, "Expected the valuation to be set to returned when coming back from ezval");
            StageTransitionAssertions.AssertStageTransitionCreated(offerKey, StageDefinitionStageDefinitionGroupEnum.LightstoneValuationRejected);
        }

        [Test]
        public void when_valuation_in_order_should_archive_case()
        {
            var offerKey = MoveCaseTo(WorkflowStates.ValuationsWF.ValuationComplete, false);

            var propertyKey = Service<IPropertyService>().GetPropertyByOfferKey(offerKey).FirstOrDefault().Column("propertykey").GetValueAs<int>();
        
            var hoc = Service<IHOCService>().GetHOCAccountByPropertyKey(propertyKey);
            if (hoc == null)
                Service<IHOCService>().CreateSAHLHOCAccount(offerKey);

            //Archive Valuation
            scriptEngine.ExecuteScript(WorkflowEnum.Valuations, WorkflowAutomationScripts.Valuations.ValuationinOrder, offerKey);

            var valuations = base.Service<IValuationService>().GetValuations(propertyKey);
            var activeVal = valuations.Where(x => x.IsActive).FirstOrDefault();

            HOCAssertions.HOCUpdatedToValuationHOC(activeVal);
            StageTransitionAssertions.AssertStageTransitionCreated(offerKey, StageDefinitionStageDefinitionGroupEnum.ValuationInOrder);
        }

        [Test]
        public void when_perform_manual_valuation_should_archive_valuation_case()
        {
            var offerKey = MoveCaseTo(WorkflowStates.ValuationsWF.ScheduleValuationAssessment, false);

            //Archive Valuation
            scriptEngine.ExecuteScript(WorkflowEnum.Valuations, WorkflowAutomationScripts.Valuations.PerformManualValuation, offerKey);

            X2Assertions.AssertCurrentValuationsX2State(offerKey, WorkflowStates.ValuationsWF.ManualArchive);
            StageTransitionAssertions.AssertStageTransitionCreated(offerKey, StageDefinitionStageDefinitionGroupEnum.PerformManualValuation);
        }

        [Test]
        public void when_escalate_to_manager_should_assign_to_manager_and_move_to_manager_review_stage()
        {
            var offerKey = MoveCaseTo(WorkflowStates.ValuationsWF.ScheduleValuationAssessment, false);

            base.Browser.Navigate<WorkFlowsNode>().WorkFlows(base.Browser);
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.ValuationsWF.ScheduleValuationAssessment);
            base.Browser.ClickAction(WorkflowActivities.Valuations.EscalatetoManager);
            base.Browser.Page<WorkflowYesNo>().ClickYes();

            //Assert
            StageTransitionAssertions.AssertStageTransitionCreated(offerKey, StageDefinitionStageDefinitionGroupEnum.EscalateToManager);
            X2Assertions.AssertCurrentValuationsX2State(offerKey, WorkflowStates.ValuationsWF.ManagerReview);
        }

        private int MoveCaseTo(string destinationState, bool needAmendedValuation)
        {
            var offer = default(Offer);
            var createAmendedValuation = false;
            if (needAmendedValuation)
                offer = Service<IX2WorkflowService>().GetOffersAtStateWithAmendedValuation(WorkflowStates.ValuationsWF.ManagerReview, Workflows.Valuations).FirstOrDefault();
            if (offer == null && needAmendedValuation)
            {
                createAmendedValuation = true;
                offer = Service<IX2WorkflowService>().GetOffersAtStateWithoutAmendedValuation(WorkflowStates.ValuationsWF.ManagerReview, Workflows.Valuations).FirstOrDefault();
            }
            else
            {
                offer = Service<IX2WorkflowService>().GetOffersAtStateWithoutAmendedValuation(WorkflowStates.ValuationsWF.ManagerReview, Workflows.Valuations).FirstOrDefault();
            }
            Assert.Greater(offer.OfferKey, 1, "OfferKey is zero no cases was found at Manager Review state.");
            switch (destinationState)
            {
                case WorkflowStates.ValuationsWF.FurtherValuationRequest:
                    base.scriptEngine.ExecuteScript(WorkflowEnum.Valuations, WorkflowAutomationScripts.Valuations.FurtherValuationRequired, offer.OfferKey);
                    break;

                case WorkflowStates.ValuationsWF.ValuationReviewRequest:
                    base.scriptEngine.ExecuteScript(WorkflowEnum.Valuations, WorkflowAutomationScripts.Valuations.ReviewValuationRequired, offer.OfferKey);
                    break;

                case WorkflowStates.ValuationsWF.ScheduleValuationAssessment:
                    base.scriptEngine.ExecuteScript(WorkflowEnum.Valuations, WorkflowAutomationScripts.Valuations.ReviewValuationRequired, offer.OfferKey);
                    base.scriptEngine.ExecuteScript(WorkflowEnum.Valuations, WorkflowAutomationScripts.Valuations.RequestValuationReview, offer.OfferKey);
                    break;

                case WorkflowStates.ValuationsWF.ValuationAssessmentPending:
                    base.scriptEngine.ExecuteScript(WorkflowEnum.Valuations, WorkflowAutomationScripts.Valuations.ReviewValuationRequired, offer.OfferKey);
                    base.scriptEngine.ExecuteScript(WorkflowEnum.Valuations, WorkflowAutomationScripts.Valuations.RequestValuationReview, offer.OfferKey);
                    base.scriptEngine.ExecuteScript(WorkflowEnum.Valuations, WorkflowAutomationScripts.Valuations.InstructEzValValuer, offer.OfferKey);
                    if (createAmendedValuation)
                    {
                        Service<IValuationService>().SubmitCompletedEzVal(offer.OfferKey, HOCRoofEnum.Thatch, thatchAmount: 400000f, valuationAmount: 1800000f);
                        base.scriptEngine.ExecuteScript(WorkflowEnum.Valuations, WorkflowAutomationScripts.Valuations.EscalateToManager, offer.OfferKey);
                        base.scriptEngine.ExecuteScript(WorkflowEnum.Valuations, WorkflowAutomationScripts.Valuations.ReviewValuationRequired, offer.OfferKey);
                        base.scriptEngine.ExecuteScript(WorkflowEnum.Valuations, WorkflowAutomationScripts.Valuations.RequestValuationReview, offer.OfferKey);
                        base.scriptEngine.ExecuteScript(WorkflowEnum.Valuations, WorkflowAutomationScripts.Valuations.InstructEzValValuer, offer.OfferKey);
                    }
                    break;

                case WorkflowStates.ValuationsWF.ValuationComplete:
                    base.scriptEngine.ExecuteScript(WorkflowEnum.Valuations, WorkflowAutomationScripts.Valuations.ReviewValuationRequired, offer.OfferKey);
                    base.scriptEngine.ExecuteScript(WorkflowEnum.Valuations, WorkflowAutomationScripts.Valuations.RequestValuationReview, offer.OfferKey);
                    base.scriptEngine.ExecuteScript(WorkflowEnum.Valuations, WorkflowAutomationScripts.Valuations.InstructEzValValuer, offer.OfferKey);
                    Service<IValuationService>().SubmitCompletedEzVal(offer.OfferKey, HOCRoofEnum.Thatch, thatchAmount: 400000f, valuationAmount: 1800000f);
                    if (createAmendedValuation)
                    {
                        base.scriptEngine.ExecuteScript(WorkflowEnum.Valuations, WorkflowAutomationScripts.Valuations.EscalateToManager, offer.OfferKey);
                        base.scriptEngine.ExecuteScript(WorkflowEnum.Valuations, WorkflowAutomationScripts.Valuations.ReviewValuationRequired, offer.OfferKey);
                        base.scriptEngine.ExecuteScript(WorkflowEnum.Valuations, WorkflowAutomationScripts.Valuations.RequestValuationReview, offer.OfferKey);
                        base.scriptEngine.ExecuteScript(WorkflowEnum.Valuations, WorkflowAutomationScripts.Valuations.InstructEzValValuer, offer.OfferKey);
                        Service<IValuationService>().SubmitAmendedEzVal(offer.OfferKey, HOCRoofEnum.Thatch, thatchAmount: 400000f, valuationAmount: 1800000f);
                    }
                    break;
            }
            return offer.OfferKey;
        }

        private void ExecuteScript(Common.Enums.WorkflowEnum _workflow, string scriptToRun, int offerkey, string identity = "")
        {
            Dictionary<int, Common.Models.WorkflowReturnData> response = scriptEngine.ExecuteScript(_workflow, scriptToRun, offerkey);
            System.Diagnostics.Debug.WriteLine("Script {0} was executed for OfferKey: {1}", scriptToRun, offerkey);
            Thread.Sleep(1000);
            Assert.True(response.LastActivitySucceeded(), "Failed to perform activity: {0}", offerkey);
        }
    }
}