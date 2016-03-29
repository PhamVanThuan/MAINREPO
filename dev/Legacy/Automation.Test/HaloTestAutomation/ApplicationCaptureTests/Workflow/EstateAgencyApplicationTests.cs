using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.LegalEntity;
using BuildingBlocks.Presenters.Origination;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;

namespace ApplicationCaptureTests.Workflow
{
    [RequiresSTA]
    public class EstateAgencyApplicationTests : TestBase<BasePage>
    {
        /// <summary>
        /// This test case will create a lead as a Branch Consultant and then create the application as an Estate Agent Application. This will result in the case being
        /// moved to the Estate Agent Application after the Create Application action has been performed.
        /// </summary>
        [Test, Description("An Estate Agency created lead will also require an estate agency to be selected")]
        public void _01_CreateEstateAgencyApplicationFromLead()
        {
            base.Browser = new TestBrowser(TestUsers.BranchConsultant);
            base.Browser.Navigate<NavigationHelper>().CloseLoanNodesFLOBO(base.Browser);
            base.Browser.Navigate<NavigationHelper>().Menu(base.Browser);
            //Navigate to Calculator
            base.Browser.Navigate<NavigationHelper>().gotoLeadCaptureCalculator(base.Browser);
            base.Browser.WaitForComplete();
            //capture legalentity
            base.Browser.Page<LegalEntityDetails>().AddLegalEntity(
                    false, IDNumbers.GetNextIDNumber(), OfferRoleTypes.LeadMainApplicant, true, "Mr", "T", "Test", "EstateAgentLead", "Test", Gender.Male,
                    MaritalStatus.Single, PopulationGroup.Coloured, Education.Matric, CitizenType.SACitizen, "", "", Language.English, Language.English, "Alive", "031",
                    "3003001", "", "", "", "", "", "", true, true, true, true, true, "", Service<ICommonService>().GetDateOfBirthFromIDNumber(IDNumbers.GetNextIDNumber())
                );
            //Set the OfferKey
            int offerKey = base.Browser.Page<ApplicationSummaryBase>().GetOfferKey();
            base.Browser.WaitForComplete(30);
            //Create the Application
            base.Browser.ClickAction(WorkflowActivities.ApplicationCapture.CreateApplication);
            //Create the application
            base.Browser.Page<BuildingBlocks.Views.LoanCalculator>().LoanCalculatorLead_Refinance(Products.NewVariableLoan, "850000", "250000", EmploymentType.Salaried, "240", true, "40000",
                        ButtonTypeEnum.CreateApplication, true);
            //assert that the case is now in Application Capture state
            var instanceID = Service<IX2WorkflowService>().GetAppCapInstanceDetails(offerKey).Rows(0).Column("InstanceID").GetValueAs<long>();
            Service<IX2WorkflowService>().WaitForX2WorkflowHistoryActivity(WorkflowActivities.ApplicationCapture.IsEAApplication, instanceID, 1);
            X2Assertions.AssertCurrentAppCapX2State(offerKey, WorkflowStates.ApplicationCaptureWF.EstateAgentApplication);
            //assert the workflow assignment
            AssignmentAssertions.AssertWorkflowAssignment(TestUsers.BranchConsultant, offerKey, OfferRoleTypeEnum.BranchConsultantD);
        }

        /// <summary>
        /// An Admin Lead can also be marked as an Esate Agent case when the the application is created. Once the case is marked as an EA Application then the application
        /// should move to the Estate Agent Assignment state after the Assign Consultant action has been performed and the branch consultant role has been assigned
        /// to the application.
        /// </summary>
        [Test, Description("A lead created by an admin can also be turned into an Estate Agent Application")]
        public void _02_AdminLeadToEstateAgencyApplication()
        {
            base.Browser = new TestBrowser(TestUsers.BranchAdmin);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(base.Browser);
            //Navigate to Calculator
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().gotoLeadCaptureCalculator(base.Browser);
            base.Browser.WaitForComplete();
            //capture legalentity
            base.Browser.Page<LegalEntityDetails>().AddLegalEntity
                (
                    false, IDNumbers.GetNextIDNumber(), OfferRoleTypes.LeadMainApplicant, true, "Mr", "T", "Test", "EstateAgentLead", "Test", Gender.Male,
                    MaritalStatus.Single, PopulationGroup.Coloured, Education.Matric, CitizenType.SACitizen, "", "", Language.English, Language.English, "Alive", "031",
                    "3003001", "", "", "", "", "", "", true, true, true, true, true, "",
                    Service<ICommonService>().GetDateOfBirthFromIDNumber(IDNumbers.GetNextIDNumber())
                );
            //Set the OfferKey
            int offerKey = base.Browser.Page<ApplicationSummaryBase>().GetOfferKey();
            base.Browser.WaitForComplete(30);
            //create the application
            base.Browser.ClickAction(WorkflowActivities.ApplicationCapture.CreateApplication);
            //Create the application
            base.Browser.Page<BuildingBlocks.Views.LoanCalculator>().LoanCalculatorLead_Refinance(Products.NewVariableLoan, "850000", "250000", EmploymentType.Salaried, "240",
               true, "40000", ButtonTypeEnum.CreateApplication, true);
            Service<IX2WorkflowService>().WaitForX2State(offerKey, Workflows.ApplicationCapture, WorkflowStates.ApplicationCaptureWF.ConsultantAssignment);
            X2Assertions.AssertCurrentAppCapX2State(offerKey, WorkflowStates.ApplicationCaptureWF.ConsultantAssignment);
            //assert the workflow assignment
            AssignmentAssertions.AssertWorkflowAssignment(TestUsers.BranchAdmin, offerKey, OfferRoleTypeEnum.BranchAdminD);
            //assign the Branch Consultant
            base.Browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().WorkFlows(base.Browser);
            base.Browser.Page<WorkflowSuperSearch>().Search(offerKey);
            base.Browser.ClickAction(WorkflowActivities.ApplicationCapture.AssignConsultant);
            base.Browser.Page<AssignInitialConsultant>().AssignConsultant(TestUsers.BranchConsultant, ButtonTypeEnum.Submit);
            //wait for the case
            Service<IX2WorkflowService>().WaitForX2State(offerKey, Workflows.ApplicationCapture, WorkflowStates.ApplicationCaptureWF.EstateAgentApplication);
            X2Assertions.AssertCurrentAppCapX2State(offerKey, WorkflowStates.ApplicationCaptureWF.EstateAgentApplication);
            AssignmentAssertions.AssertWorkflowAssignment(TestUsers.BranchAdmin, offerKey, OfferRoleTypeEnum.BranchAdminD);
            AssignmentAssertions.AssertWorkflowAssignment(TestUsers.BranchConsultant, offerKey, OfferRoleTypeEnum.BranchConsultantD);
        }
    }
}