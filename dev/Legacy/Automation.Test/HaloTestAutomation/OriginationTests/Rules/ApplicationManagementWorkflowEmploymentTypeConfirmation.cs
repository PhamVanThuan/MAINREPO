using BuildingBlocks;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Origination.Rules
{
    [RequiresSTA]
    public class ApplicationManagementWorkflowEmploymentTypeConfirmation : OriginationTestBase<BasePage>
    {
        private int offerKey;
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();

            offerKey = Service<IX2WorkflowService>().GetOfferKeyAtStateByType
                (
                    WorkflowStates.ApplicationManagementWF.ManageApplication,
                    Workflows.ApplicationManagement,
                    OfferTypeEnum.NewPurchase,
                    ""
                );

            base.Browser = new TestBrowser(TestUsers.NewBusinessProcessor1);

        }

        protected override void OnTestStart()
        {
            base.Browser.Page<BuildingBlocks.Presenters.CommonPresenters.WorkflowSuperSearch>().SearchByUniqueIdentifierAndApplicationType(base.Browser, offerKey.ToString()
                , new string[] { WorkflowStates.ApplicationManagementWF.ManageApplication }, ApplicationType.Any);
        }

        [Test]
        public void when_navigating_to_confirm_application_employment_the_page_is_displayed_correctly()
        {
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.ConfirmApplicationEmployment);
            base.Browser.Page<BuildingBlocks.Presenters.Origination.ConfirmApplicationEmployment>().AssertPageLoaded();
        }

        [Test]
        public void when_confirming_application_employment_an_employment_type_must_be_selected()
        {
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.ConfirmApplicationEmployment);
            base.Browser.Page<BuildingBlocks.Presenters.Origination.ConfirmApplicationEmployment>().SelectEmploymentType("- Please select -");
            base.View.CheckForerrorMessages("Please select a valid employment type.");
        }

        [Test]
        public void when_confirming_application_employment()
        {
            var offerInformationCount = base.Service<IApplicationService>().GetOfferInformationRecordCount(offerKey);
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.ConfirmApplicationEmployment);
            base.Browser.Page<BuildingBlocks.Presenters.Origination.ConfirmApplicationEmployment>().SelectEmploymentType(EmploymentType.Salaried);
            base.Browser.Page<BuildingBlocks.Presenters.Origination.ConfirmApplicationEmployment>().AssertApplicationEmploymentActionCompleted(offerKey, Workflows.ApplicationManagement);
            BuildingBlocks.Assertions.OfferAssertions.AssertOfferAttributeExists(offerKey, OfferAttributeTypeEnum.ManuallySelectedEmploymentType, true);
            BuildingBlocks.Assertions.OfferAssertions.AssertOfferInformationCount(offerKey, offerInformationCount + 1);
            BuildingBlocks.Assertions.StageTransitionAssertions.AssertStageTransitionCreated(offerKey, StageDefinitionStageDefinitionGroupEnum.ApplicationManagement_ConfirmapplicationEmployment);
        }
    }
}