using Automation.DataAccess;
using BuildingBlocks;
using BuildingBlocks.Navigation.FLOBO.Common;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.LegalEntity;
using BuildingBlocks.Presenters.Origination;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;

namespace FurtherLendingTests.Rules
{
    [TestFixture, RequiresSTA]
    public class ApplicantTests : TestBase<BasePage>
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

        #region Tests

        /// <summary>
        /// A Main Applicant can be added to a Further Loan Application
        /// </summary>
        [Test, Description("A Main Applicant can be added to a Further Loan Application")]
        public void AddMainApplicantToFurtherLoan()
        {
            int offerKey = Service<IX2WorkflowService>().GetOfferKeyAtStateByType(WorkflowStates.ApplicationManagementWF.ManageApplication, Workflows.ApplicationManagement,
                OfferTypeEnum.FurtherLoan, "FLAutomation");
            //add the case
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.ApplicationManagementWF.ManageApplication);
            //we need to add an applicant
            base.Browser.Navigate<ApplicantsNode>().Applicants(NodeTypeEnum.Add);
            //add as a suretor
            base.Browser.Page<LegalEntityDetailsLeadApplicantAdd>().AddLegalEntity(OfferRoleTypes.MainApplicant, false, IDNumbers.GetNextIDNumber(), "Mr",
                "auto", "MainApplicantAdd", "MainApplicantAdd", "auto", Gender.Male, MaritalStatus.Single, "Unknown", "Unknown", CitizenType.SACitizen, "auto",
                null, null, "Unknown", Language.English, null, "031", "1234567", null, null, null, null, null, null, true, false, false, false, false,
                ButtonTypeEnum.Next);
            //after saving there should be a message displayed on the screen
            base.Browser.ClickWorkflowLoanNode(Workflows.ApplicationManagement);
            int count = Service<IApplicationService>().NewRolesCount(offerKey, OfferTypeEnum.FurtherLoan);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(string.Format("CAUTION: {0} NEW APPLICANT/S IN PLACE", count));
        }

        /// <summary>
        /// A Main Applicant cannot be added to a Further Advance Application or a Readvance Application. This test ensures that a rule is run when trying
        /// to add a Main Applicant to these application types.
        /// </summary>
        [Test, Sequential, Description(@"A Main Applicant cannot be added to a Further Advance Application or a Readvance Application. This test ensures that a rule is
		run when trying to add a Main Applicant to these application types.")]
        public void AddMainApplicantToNonFurtherLoanApplications([Values(OfferTypeEnum.FurtherAdvance, OfferTypeEnum.Readvance)] OfferTypeEnum offerType)
        {
            base.Browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().WorkFlows(base.Browser);
            int offerKey = Service<IX2WorkflowService>().GetOfferKeyAtStateByType(WorkflowStates.ApplicationManagementWF.ManageApplication, Workflows.ApplicationManagement,
                offerType, "FLAutomation");
            //add the case
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.ApplicationManagementWF.ManageApplication);
            //we need to add an applicant
            base.Browser.Navigate<ApplicantsNode>().Applicants(NodeTypeEnum.Add);
            //add as a suretor
            base.Browser.Page<LegalEntityDetailsLeadApplicantAdd>().AddLegalEntity(OfferRoleTypes.MainApplicant, false, IDNumbers.GetNextIDNumber(), "Mr",
                "auto", "MainApplicantAdd", "MainApplicantAdd", "auto", Gender.Male, MaritalStatus.Single, "Unknown", "Unknown", CitizenType.SACitizen, "auto",
                null, null, "Unknown", Language.English, null, "031", "1234567", null, null, null, null, null, null, true, false, false, false, false,
                ButtonTypeEnum.Next);
            //after saving there should be a message displayed on the screen
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Main applicants can not be added to Readvance & Further Advance applications.");
        }

        /// <summary>
        /// A FL Processor cannot remove an applicant from an application that plays a role on the associated Mortgage Loan application.
        /// </summary>
        [Test, Description("A FL Processor cannot remove an applicant from an application that plays a role on the associated Mortgage Loan application"), Category("Further Advances")]
        public void CannotRemoveMainApplicantFromFurtherAdvanceApplication()
        {
            var offerKey = Service<IX2WorkflowService>().GetOfferKeyAtStateByType(WorkflowStates.ApplicationManagementWF.ManageApplication, Workflows.ApplicationManagement,
                OfferTypeEnum.FurtherAdvance, "FLAutomation");
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.ApplicationManagementWF.ManageApplication);
            base.Browser.Navigate<ApplicantsNode>().Applicants(NodeTypeEnum.Delete);
            var results = Service<ILegalEntityService>().GetLegalEntityLegalNamesForOffer(offerKey);
            string legalEntityName = String.Empty;
            foreach (var row in results.RowList)
            {
                if (row.Column("OfferRoleTypeKey").Value == ((int)OfferRoleTypeEnum.MainApplicant).ToString())
                {
                    legalEntityName = row.Column("LegalEntityLegalName").Value;
                    break;
                }
            }
            base.Browser.Page<ApplicantsRemove>().RemoveApplicant(legalEntityName);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Cannot delete a legal entity from the application that exists on the account.");
        }

        /// <summary>
        /// Ensures that a FL Processor can remove the additional surety added to the application.
        /// </summary>
        [Test, Description("A FL Processor can remove the additional surety added to a Further Lending Application"), Category("Further Advances")]
        public void CanRemoveAdditionalSuretyFromFurtherAdvanceApplication()
        {
            int offerKey = Service<IX2WorkflowService>().GetOfferKeyAtStateByType(WorkflowStates.ApplicationManagementWF.ManageApplication,
                Workflows.ApplicationManagement, OfferTypeEnum.FurtherAdvance, "FLAutomation");
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.ApplicationManagementWF.ManageApplication);
            //we need to add an applicant
            base.Browser.Navigate<ApplicantsNode>().Applicants(NodeTypeEnum.Add);
            //add as a suretor
            base.Browser.Page<LegalEntityDetailsLeadApplicantAdd>().AddLegalEntity(OfferRoleTypes.Suretor, false, IDNumbers.GetNextIDNumber(), "Mr", "auto", "SuretorFirstname",
                "SuretorSurname", "auto", Gender.Male, MaritalStatus.Single, "Unknown", "Unknown", CitizenType.SACitizen, "auto", null, null, "Unknown",
                Language.English, null, "031", "1234567", null, null, null, null, null, null, true, false, false, false, false,
                ButtonTypeEnum.Next);
            //after saving there should be a message displayed on the screen
            base.Browser.ClickWorkflowLoanNode(Workflows.ApplicationManagement);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("CAUTION: 1 NEW APPLICANT/S IN PLACE");
            QueryResults le = base.Service<IApplicationService>().GetClientOfferRolesNotOnAccount(offerKey);
            string legalEntityName = le.SQLScalarValue;
            //navigate to the remove
            base.Browser.Navigate<ApplicantsNode>().Applicants(NodeTypeEnum.Delete);
            //remove the applicant
            base.Browser.Page<ApplicantsRemove>().RemoveApplicant(legalEntityName);
            base.Browser.ClickWorkflowLoanNode(Workflows.ApplicationManagement);
        }

        /// <summary>
        /// This test ensures that the FL Processor cannot add a surety to a loan at a point prior to Manage Application. It will check that the
        /// CBO activity does not appear in the menu when the FL Processor loads an application at QA state into the FLOBO.
        /// </summary>
        [Test, Description("A Further Lending Processor cannot add an applicant to a FL application until the case has reached Manage Application"), Category("Further Advances")]
        public void CannotAddSuretyAtQAState()
        {
            base.Browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().WorkFlows(base.Browser);
            var offerKey = Service<IX2WorkflowService>().GetOfferKeyAtStateByType(WorkflowStates.ApplicationManagementWF.QA, Workflows.ApplicationManagement,
                OfferTypeEnum.FurtherAdvance, "FLAutomation");
            base.Browser.Page<WorkflowSuperSearch>().Search(offerKey);
            //we need to add an applicant
            base.Browser.Navigate<ApplicantsNode>().Applicants(NodeTypeEnum.View);
            bool actionExists = base.Browser.ActionExists("Add Legal Entity");
            Assert.False(actionExists, "The Add Legal Entity action is being incorrectly applied at the QA state");
        }

        #endregion Tests
    }
}