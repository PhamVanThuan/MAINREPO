using BuildingBlocks;
using BuildingBlocks.Navigation.FLOBO.AffordabilityAssessments;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.Origination;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WatiN.Core;

namespace Origination.Rules
{
    [RequiresSTA]
    public class AffordabilityAssessmentRuleTests : OriginationTestBase<BasePage>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();            
        }

        [Test]
        public void when_updating_a_confirmed_affordability_assessment()
        {
            int offerKey = base.Service<IApplicationService>().GetOfferByWorkflowState(WorkflowStates.CreditWF.Credit);
            base.Service<IApplicationService>().InsertAffordabilityAssessment((int)AffordabilityAssessmentStatusKey.Confirmed, offerKey);

            base.Browser = new TestBrowser(TestUsers.CreditSupervisor3);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Task();
            base.Browser.Page<WorkflowSuperSearch>().Search(offerKey);
            base.Browser.Navigate<AffordabilityAssessmentNode>().ClickAffordabilityAssessments();

            var affordabilityAssessmentKey = base.Service<IAffordabilityAssessmentService>().GetAffordabilityAssessmentByGenericKeyAndAssessmentStatus(offerKey, (int)AffordabilityAssessmentStatusKey.Confirmed).FirstOrDefault().AffordabilityAssessmentKey;
            string affordabilityAssessmentContributors = base.Service<IAffordabilityAssessmentService>().GetAffordabilityAssessmentContributorsByAffordabilityAssessmentStatus(offerKey, (int)AffordabilityAssessmentStatusKey.Confirmed).ToList().FirstOrDefault().Column("AffordabilityAssessmentContributors").Value;
            base.Browser.Navigate<AffordabilityAssessmentNode>().ClickAffordabilityAssessment(affordabilityAssessmentContributors);
            base.Browser.Navigate<AffordabilityAssessmentNode>().ClickUpdateAffordabilityAssessment();
            base.Browser.Page<UpdateAffordabilityAssessment>().PopulateSingleCreditField(555);
            base.Browser.Page<UpdateAffordabilityAssessment>().SetCommentFields("txtOtherIncome1_Comments");
            base.Browser.Page<UpdateAffordabilityAssessment>().ClickSave();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Confirmed Assessments cannot be updated. If you continue, a new Unconfirmed copy will be made and this Assessment will be Archived.");
        }
    }
}


