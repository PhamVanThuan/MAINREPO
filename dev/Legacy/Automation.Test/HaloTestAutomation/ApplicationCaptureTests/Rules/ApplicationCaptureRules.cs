using Automation.DataAccess;
using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation;
using BuildingBlocks.Navigation.FLOBO.Common;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.LegalEntity;
using BuildingBlocks.Presenters.Origination;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System.Collections.Generic;

using Navigation = BuildingBlocks.Navigation;

namespace ApplicationCaptureTests.Rules
{
    /// <summary>
    /// Contains rule tests for the Application Capture workflow.
    /// </summary>
    [TestFixture, RequiresSTA]
    public class ApplicationCaptureRules : ApplicationCaptureTests.TestBase<BasePage>
    {
        /// <summary>
        /// IE TestBrowser Object for the tests to use.
        /// </summary>
        private TestBrowser browser;

        /// <summary>
        /// Holds the offerKey for the ITC tests
        /// </summary>
        private int _itcOfferKey;

        private int _foreignerItCofferKey;

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            browser = new TestBrowser(TestUsers.BranchConsultant);
        }

        protected override void OnTestStart()
        {
            base.OnTestStart();
            if (browser != null)
            {
                //clear flowbo
                browser.Navigate<Navigation.NavigationHelper>().Task();
                browser.Navigate<Navigation.NavigationHelper>().CloseLoanNodesFLOBO(browser);
            }
        }

        [Test, Description(@"An error message warning is displayed on the screen when the user captures	the same person and the same property which already exists as an application within the new origination process.")]
        public void when_another_application_is_captured_for_the_same_applicant_on_the_same_property_a_warning_should_be_displayed()
        {
            //Navigate to the application calculator.
            browser.Navigate<Navigation.NavigationHelper>().gotoApplicationCalculator(browser);
            //Capture a switch loan.
            browser.Page<BuildingBlocks.Views.LoanCalculator>().LoanCalculatorLead_Switch
                ("New Variable Loan", "741000", "210000", "102000", EmploymentType.Salaried, "", false, "38000",
                ButtonTypeEnum.CreateApplication);
            //Get predefined LegalEntity data from the database.
            QueryResults r = Service<IApplicationService>().GetOpenApplicationCaptureOffer();
            if (r.HasResults)
            {
                browser.Page<LegalEntityDetailsLeadApplicantAdd>().AddExistingLegalEntity(r.Rows(0).Column("idNumber").Value);
            }
            browser.WaitForComplete(60);
            //Get offerkey for created application.
            int offerkey = browser.Page<ApplicationSummaryBase>().GetOfferKey();
            //Close all open workflow cases in the flobo
            browser.Navigate<Navigation.NavigationHelper>().CloseLoanNodesFLOBO(browser);
            //Look for the case on the users worklist
            browser.Page<X2Worklist>().SelectCaseFromWorklist(browser, WorkflowStates.ApplicationCaptureWF.ApplicationCapture, offerkey);
            //Navigate to the property node.
            browser.Navigate<Navigation.PropertiesNode>().Properties();
            //Look for the case on the users worklist.
            browser.Navigate<Navigation.PropertiesNode>().PropertySummary(NodeTypeEnum.Add);
            //fetch the details of the property for our existing offer.
            int propertyKey = r.Rows(0).Column("propertyKey").GetValueAs<int>();
            Automation.DataModels.Property property = Service<IPropertyService>().GetPropertyByPropertyKey(propertyKey);

            if (property != null)
            {
                //Capture existing property
                browser.Page<App_PropertyCaptureStep1>().PerformEmptySearch();
                //Capture existing property
                browser.Page<App_ProperyCaptureStep2>().CaptureProperty(property);
                //Capture existing property
                browser.Page<App_PropertyCaptureStep3>().CapturePropertyDetails(property);
            }
            //Assert that a warning is displayed on the screen.
            browser.Page<BasePageAssertions>().AssertFrameContainsText("containing the clients ID number and the respective property");
        }

        [Test, Description(@"This test will create an application and then try to request an ITC without first supplying the required details. This particular
							test ensures that an ID Number has been provided prior to the user being able to request an ITC.")]
        public void _1_when_performing_an_ITC_an_ID_Number_is_required()
        {
            _itcOfferKey = 0;
            //application calculator
            browser.Navigate<Navigation.NavigationHelper>().Menu(browser);
            browser.Navigate<Navigation.CalculatorsNode>().Calculators(browser, CalculatorNodeTypeEnum.ApplicationCalculator);
            //complete the calculator
            browser.Page<BuildingBlocks.Views.LoanCalculator>().LoanCalculatorLead_Refinance(Products.Edge, "3500000", "2500000",
                EmploymentType.Salaried, null, false, "105000", ButtonTypeEnum.CreateApplication);
            //add the LE
            browser.Page<LegalEntityDetailsLeadApplicantAdd>().AddLegalEntity("Lead - Main Applicant", true, null, "Mr", "auto", "Clark", "Duncan",
                null, Gender.Male, MaritalStatus.Single, "Unknown", "Unknown", null, null, null, null, "Unknown", Language.English,
                null, null, null, null, null, null, null, "0821234567", null, false, false, false, true, false,
                ButtonTypeEnum.Next);
            _itcOfferKey = browser.Page<ApplicationSummaryBase>().GetOfferKey();
            //assert offer created
            Assert.That(_itcOfferKey > 0, "Offer not created");
            //we need to update applicants to provide declarations etc.
            BuildingBlocks.CBO.LegalEntityCBONode.CompleteLegalEntityNode(browser, _itcOfferKey, true, false, false, false, false, false, false, false);
            //navigate to the applicants node
            browser.Navigate<ApplicantsNode>().Applicants(NodeTypeEnum.View);
            browser.Navigate<ApplicantsNode>().ITCDetails(NodeTypeEnum.View);
            //select all checkboxes
            Service<IWatiNService>().GenericCheckAllCheckBoxes(browser.DomContainer);
            //try and perform the ITC
            browser.Page<ITCApplication>().DoEnquiry();
            //we should have a warning indicating that the LE requires an ID Number
            QueryResults r = Service<ILegalEntityService>().GetLegalEntityLegalNamesForOffer(_itcOfferKey);
            foreach (QueryResultsRow row in r.RowList)
            {
                browser.Page<BasePageAssertions>().AssertValidationMessageExists(string.Format(@"Legal Entity: {0} requires a valid ID Number before requesting an ITC",
                    row.Column("LegalEntityLegalName").Value));
            }
        }

        /// <summary>
        /// This test will ensure that if the applicant on an application does not have a legal entity address then an ITC
        /// cannot be performed
        /// </summary>
        [Test, Description(@"This test will ensure that if the applicant on an application does not have a legal entity address then an ITC cannot be performed")]
        public void _2_when_performing_an_ITC_a_legal_entity_address_is_required()
        {
            //add the case
            browser.Page<WorkflowSuperSearch>().Search(_itcOfferKey);
            //give the applicant an ID Number
            browser.Navigate<LegalEntityNode>().LegalEntity(_itcOfferKey);
            browser.Navigate<LegalEntityNode>().LegalEntityDetails(NodeTypeEnum.Update);
            browser.Page<LegalEntityDetailsUpdateApplicant>().UpdateLegalEntityDetails_NaturalPerson(IDNumbers.GetNextIDNumber());
            //navigate to the applicants node
            browser.Navigate<ApplicantsNode>().Applicants(NodeTypeEnum.View);
            browser.Navigate<ApplicantsNode>().ITCDetails(NodeTypeEnum.View);
            //select all checkboxes
            Service<IWatiNService>().GenericCheckAllCheckBoxes(browser.DomContainer);
            //try and perform the ITC
            browser.Page<ITCApplication>().DoEnquiry();
            //we should have a warning indicating that the LE requires an Address
            QueryResults r = Service<ILegalEntityService>().GetLegalEntityLegalNamesForOffer(_itcOfferKey);
            foreach (QueryResultsRow row in r.RowList)
            {
                browser.Page<BasePageAssertions>().AssertValidationMessageExists(string.Format(@"Legal Entity: {0} requires a valid street address before requesting an ITC.",
                    row.Column("LegalEntityLegalName").Value));
            }
        }

        /// <summary>
        /// This test will perform an ITC on a legal entity that has a NULL citizen type. this test has been added in order to regression test a production reported
        /// bug.
        /// </summary>
        [Test, Description(@"This test will perform an ITC on a legal entity that has a NULL citizen type. this test has been added in order to regression test a production reported
		bug.")]
        public void _3_when_performing_an_ITC_and_the_citizenship_type_is_null_the_ITC_should_be_requested()
        {
            //add the case
            browser.Page<WorkflowSuperSearch>().Search(_itcOfferKey);
            //update the citizen type
            Service<ILegalEntityService>().UpdateCitizenTypeForApplicants(0, _itcOfferKey);
            //we need to add an address
            BuildingBlocks.CBO.LegalEntityCBONode.CompleteLegalEntityNode(browser, _itcOfferKey, false, false, true, false, false, false, false, false);
            //navigate to the applicants node
            browser.Navigate<ApplicantsNode>().Applicants(NodeTypeEnum.View);
            browser.Navigate<ApplicantsNode>().ITCDetails(NodeTypeEnum.View);
            //select all checkboxes
            Service<IWatiNService>().GenericCheckAllCheckBoxes(browser.DomContainer);
            //try and perform the ITC
            browser.Page<ITCApplication>().DoEnquiry();
            //assert that an ITC record exists
            OfferAssertions.AssertITCRecordExists(_itcOfferKey);
        }

        /// <summary>
        /// This test ensures that the correct warning message is displayed to the user when an ITC is requested on a legal entity who has a
        /// citizen type of foreigner without any ID number.
        /// </summary>
        [Test, Description(@"This test ensures that the correct warning message is displayed to the user when an ITC is requested on a legal entity who has a
		citizen type of foreigner without any ID number.")]
        public void _4_when_performing_an_ITC_on_a_foreigner_with_no_valid_ID_number_a_warning_should_be_displayed()
        {
            var foreignerCitizenshipTypes = new List<CitizenTypeEnum> {
                    CitizenTypeEnum.Foreigner,
                    CitizenTypeEnum.NonResidentConsulate,
                    CitizenTypeEnum.NonResidentDiplomat,
                    CitizenTypeEnum.NonResidentHighCommissioner,
                    CitizenTypeEnum.NonResidentRefugee,
                    CitizenTypeEnum.NonResident
            };
            _foreignerItCofferKey = 0;
            //application calculator
            browser.Navigate<Navigation.NavigationHelper>().Menu(browser);
            browser.Navigate<Navigation.CalculatorsNode>().Calculators(browser, CalculatorNodeTypeEnum.ApplicationCalculator);
            //complete the calculator
            browser.Page<BuildingBlocks.Views.LoanCalculator>().LoanCalculatorLead_Refinance("Edge", "3500000", "2500000", EmploymentType.Salaried, null, false, 
                "105000", ButtonTypeEnum.CreateApplication);
            //add the LE
            browser.Page<LegalEntityDetailsLeadApplicantAdd>().AddLegalEntity("Lead - Main Applicant", true, null, "Mr", "auto", "Shane", "Warne",
                null, Gender.Male, MaritalStatus.Single, "Unknown", "Unknown", null, null, null, null, "Unknown", Language.English,
                null, null, null, null, null, null, null, "0821234567", null, false, false, false, true, false,
                ButtonTypeEnum.Next);
            //assert offer created
            _foreignerItCofferKey = browser.Page<ApplicationSummaryBase>().GetOfferKey();
            Assert.That(_foreignerItCofferKey > 0, "Offer not created");
            //we need to update applicants to provide declarations etc.
            BuildingBlocks.CBO.LegalEntityCBONode.CompleteLegalEntityNode(browser, _foreignerItCofferKey, true, false, true, false, false, false, false, false);
            //navigate to the applicants node
            browser.Navigate<ApplicantsNode>().Applicants(NodeTypeEnum.View);
            //try and perform the ITC
            foreach (var foreignerType in foreignerCitizenshipTypes)
            {
                //update the citizen type
                Service<ILegalEntityService>().UpdateCitizenTypeForApplicants((int)foreignerType, _foreignerItCofferKey);
                browser.Navigate<ApplicantsNode>().ITCDetails(NodeTypeEnum.View);
                //select all checkboxes
                Service<IWatiNService>().GenericCheckAllCheckBoxes(browser.DomContainer);
                browser.Page<ITCApplication>().DoEnquiry();
                //we should have a warning indicating that the LE is a Foreigner without a valid ID number
                QueryResults r = Service<ILegalEntityService>().GetLegalEntityLegalNamesForOffer(_foreignerItCofferKey);
                foreach (QueryResultsRow row in r.RowList)
                {
                    browser.Page<BasePageAssertions>().AssertValidationMessageExists(
                        string.Format(@"Legal Entity: {0} is a foreign citizen without a valid ID Number, no ITC can be requested without valid ID number.",
                        row.Column("LegalEntityLegalName").Value));
                }
            }
        }
    }
}