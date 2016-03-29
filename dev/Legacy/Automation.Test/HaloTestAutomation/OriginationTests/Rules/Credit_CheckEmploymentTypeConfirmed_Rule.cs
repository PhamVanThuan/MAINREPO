using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Origination.Rules
{
    [RequiresSTA]
    public class Credit_CheckEmploymentTypeConfirmed_Rule : OriginationTestBase<BasePage>
    {
        private Automation.DataModels.Offer Offer;

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            Offer = Service<IX2WorkflowService>().GetOfferAtCreditWithUserConfirmedApplicationEmployment(true);
        }

        [Test, TestCaseSource(typeof(Credit_CheckEmploymentTypeConfirmed_Rule), "GetActivities")]
        public void when_exiting_the_credit_workflow_the_application_employment_type_needs_to_be_confirmed(WorkflowActivitiesToTest testCase)
        {
            try
            {
                var scriptResults = base.scriptEngine.ExecuteScript(WorkflowEnum.Credit, testCase.WorkflowScript, Offer.OfferKey, TestUsers.CreditExceptions);
            }
            catch (Exception ex)
            {
                Assert.That(ex.ToString().Contains("Please perform the 'Confirm Application Employment' action before proceeding"),
                    string.Format(@"The 'Confirm Application Employment' message was not displayed when trying to perform the {0} activity", testCase.WorkflowActivity));
            }
        }

        public IEnumerable<WorkflowActivitiesToTest> GetActivities()
        {
            return new[] {
                new WorkflowActivitiesToTest { WorkflowActivity = WorkflowActivities.Credit.ApproveApplication, WorkflowScript = WorkflowAutomationScripts.Credit.ApproveApplication },
                new WorkflowActivitiesToTest { WorkflowActivity = WorkflowActivities.Credit.DeclineApplication, WorkflowScript = WorkflowAutomationScripts.Credit.DeclineApplication },
                new WorkflowActivitiesToTest { WorkflowActivity = WorkflowActivities.Credit.DeclinewithOffer, WorkflowScript = WorkflowAutomationScripts.Credit.DeclineWithOffer },
                new WorkflowActivitiesToTest { WorkflowActivity = WorkflowActivities.Credit.ExceptionsDeclinewithOffer, WorkflowScript = WorkflowAutomationScripts.Credit.ExceptionsDeclinewithOffer },
                new WorkflowActivitiesToTest { WorkflowActivity = WorkflowActivities.Credit.ApprovewithPricingChanges, WorkflowScript = WorkflowAutomationScripts.Credit.ApproveWithPricingChanges },
                new WorkflowActivitiesToTest { WorkflowActivity = WorkflowActivities.Credit.ExceptionsRateAdjustment, WorkflowScript = WorkflowAutomationScripts.Credit.ExceptionsRateAdjustment }
            };
        }

        public class WorkflowActivitiesToTest
        {
            public string WorkflowActivity { get; set; }

            public string WorkflowScript { get; set; }

            public override string ToString()
            {
                return WorkflowActivity;
            }
        }
    }
}