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
using Common.Extensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCaptureTests.Workflow
{
    [RequiresSTA]
    public class ApplicationCaptureDataSetup : ApplicationCaptureTests.TestBase<BuildingBlocks.Views.LoanCalculator>
    {
        private TestBrowser browser;
        private string PreviousUser;

        protected override void OnTestTearDown()
        {
            base.OnTestTearDown();
        }

        public IEnumerable<Automation.DataModels.OriginationTestCase> GetApplicationsForCaseCreate()
        {
            return Service<ICommonService>().GetOriginationTestCases();
        }

        public IEnumerable<Automation.DataModels.OriginationTestCase> GetApplicationsForSubmitApplication()
        {
            var testCases = Service<ICommonService>().GetOriginationTestCases();
            return (from t in testCases where t.TestGroup == "SubmitApplication" select t);
        }

        public IEnumerable<Automation.DataModels.LeadTestCase> GetLeadsForCaseCreate()
        {
            return Service<ICommonService>().GetAutomationTestLeads();
        }

        /// <summary>
        /// Creates Branch Consultant Originated Applications , TestCaseSource(typeof(IOCRegistry), "GetViews")
        /// </summary>
        [Test, TestCaseSource(typeof(ApplicationCaptureDataSetup), "GetApplicationsForCaseCreate"), Description("Create a basic Application and Lead at the Application Capture stage.")]
        public void _01_OffersAtApplicationCapture(Automation.DataModels.OriginationTestCase testCase)
        {
            testCase.Username = TestUsers.BranchConsultant;

            int offerKey = 0;
            string idNumber = string.Empty;
            try
            {
                if (browser != null)
                {
                    if (testCase.Username.ToUpper() != this.PreviousUser.ToUpper())
                    {
                        browser.Dispose();
                        browser = new TestBrowser(testCase.Username);
                    }
                }
                else
                {
                    browser = new TestBrowser(testCase.Username);
                }
                browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(browser);

                const string phoneCode = "031";
                const string phoneNumber = "1234567";

                //Application Calculator
                browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(browser);
                browser.Navigate<BuildingBlocks.Navigation.CalculatorsNode>().Calculators(browser, CalculatorNodeTypeEnum.ApplicationCalculator);

                switch (testCase.LoanType)
                {
                    case "New purchase":
                        browser.Page<BuildingBlocks.Views.LoanCalculator>().LoanCalculatorLead_NewPurchase(testCase.Product, testCase.MarketValue, testCase.CashDeposit, testCase.EmploymentType, testCase.Term,
                            testCase.HouseHoldIncome, ButtonTypeEnum.CreateApplication);
                        break;

                    case "Switch loan":
                        browser.Page<BuildingBlocks.Views.LoanCalculator>().LoanCalculatorLead_Switch(testCase.Product, testCase.MarketValue, testCase.ExistingLoan, testCase.CashOut, testCase.EmploymentType, testCase.Term,
                             Convert.ToBoolean(testCase.CapitaliseFees), testCase.HouseHoldIncome, ButtonTypeEnum.CreateApplication);
                        break;

                    case "Refinance":
                        browser.Page<BuildingBlocks.Views.LoanCalculator>().LoanCalculatorLead_Refinance(testCase.Product, testCase.MarketValue, testCase.CashOut, testCase.EmploymentType, testCase.Term,
                            Convert.ToBoolean(testCase.CapitaliseFees), testCase.HouseHoldIncome, ButtonTypeEnum.CreateApplication);
                        break;
                }

                if (testCase.LegalEntityType == "Natural Person")
                {
                    idNumber = IDNumbers.GetNextIDNumber();
                    browser.Page<LegalEntityDetailsLeadApplicantAdd>().AddLegalEntity(testCase.LegalEntityRole, true, idNumber, "Mr", "auto", testCase.Firstname, testCase.Surname, "auto", Gender.Male,
                        MaritalStatus.Single, "Unknown", "Unknown", CitizenType.SACitizen, "auto", null, null, "Unknown", Language.English, null, phoneCode, phoneNumber,
                        null, null, null, null, null, null, true, false, false, false, false, ButtonTypeEnum.Next);
                    offerKey = browser.Page<ApplicationSummaryBase>().GetOfferKey();
                }
                else
                {
                    browser.Page<LegalEntityDetails>().AddLegalEntityCompany(testCase.LegalEntityType, testCase.CompanyName, phoneCode, phoneNumber);
                    offerKey = browser.Page<ApplicationSummaryBase>().GetOfferKey();
                }

                //Commit OfferKey
                Service<IApplicationService>().CleanupNewBusinessOffer(offerKey);
            }
            // Let NUnit catch any exceptions
            finally
            {
                Service<ICommonService>().CommitOfferKeyForTestIdentifier(testCase.TestIdentifier, offerKey);
                this.PreviousUser = testCase.Username;
            }
        }

        /// <summary>
        /// Creates Branch Admin Originated Leads
        /// </summary>
        [Test, TestCaseSource(typeof(ApplicationCaptureDataSetup), "GetLeadsForCaseCreate"), Description("Create Admin Leads")]
        public void _02_CreateAdminLeads(Automation.DataModels.LeadTestCase lead)
        {
            if (lead.TestGroup == "Admin")
                CreateLead(lead);
        }

        /// <summary>
        /// Creates Branch Consultant Leads
        /// </summary>
        [Test, TestCaseSource(typeof(ApplicationCaptureDataSetup), "GetLeadsForCaseCreate"), Description("Create Branch Leads")]
        public void _03_CreateBranchConsultantLeads(Automation.DataModels.LeadTestCase lead)
        {
            if (lead.TestGroup == "Consultant")
                CreateLead(lead);
        }

        /// <summary>
        /// Creates Estate Agent Leads
        /// </summary>
        [Test, TestCaseSource(typeof(ApplicationCaptureDataSetup), "GetLeadsForCaseCreate"), Description("Create Estate Agent Leads")]
        public void _04_CreateEstateAgentLeads(Automation.DataModels.LeadTestCase lead)
        {
            if (lead.TestGroup == "EstateAgents")
                CreateLead(lead);
        }

        /// <summary>
        /// Verify that a Branch Consultant can perform the 'Submit Application' action at 'Application Capture' state, which moves the case to the next state depending on the LoanType
        /// </summary>
        [Test, TestCaseSource(typeof(ApplicationCaptureDataSetup), "GetApplicationsForSubmitApplication"), Description("Submits the application.")]
        public void _05_SubmitApplication(Automation.DataModels.OriginationTestCase application)
        {
            SubmitApplication(application);
        }

        /// <summary>
        /// This test is a sequential test that assert that the cases submitted in _052_SubmitCreditScoring have been
        /// submitted correctly into the Application Management workflow.
        /// </summary>
        [Test, TestCaseSource(typeof(ApplicationCaptureDataSetup), "GetApplicationsForSubmitApplication"), Description(@"Runs the assertions to ensure that the cases have been submitted correctly from Application Capture")]
        public void _06_SubmitAssertions(Automation.DataModels.OriginationTestCase application)
        {
            AssertSubmitApplication(application);
        }

        private void CreateLead(Automation.DataModels.LeadTestCase lead)
        {
            if (browser != null)
                browser.Dispose();
            //Open a new browser and log in as TestUser.
            browser = new TestBrowser(lead.OrigConsultant);
            Service<IWatiNService>().SetWatiNTimeouts(120);
            browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(browser);
            browser.WaitForComplete();
            //Navigate to Calculator
            browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().gotoLeadCaptureCalculator(browser);
            browser.WaitForComplete();
            //capture legalentity
            var idNumber = IDNumbers.GetNextIDNumber();
            var dateOfBirth = Service<ICommonService>().GetDateOfBirthFromIDNumber(idNumber);
            browser.Page<LegalEntityDetails>().AddLegalEntity(false, idNumber, lead.Role, true, lead.Salutation, lead.Initials, lead.FirstNames, lead.Surname, lead.PreferredName, lead.Gender, lead.MaritalStatus,
                    lead.PopulationGroup, lead.Education, lead.CitizenshipType, lead.PassportNumber, lead.TaxNumber, lead.HomeLanguage, lead.DocumentLanguage, lead.Status, "013", "1234567", "", "", "", "", "", "", true, true, true, true, true, "",
                    dateOfBirth);
            string adusernamewithoutdomain = lead.OrigConsultant.RemoveDomainPrefix();
            //Get the OfferKey
            int offerKey = 0;
            if (lead.OrigConsultant.ToUpper().Contains(TestUsers.EstateAgentConsultant.RemoveDomainPrefix().ToUpper()))
            {
                var results = Service<IApplicationService>().GetLastOfferCreatedByADUser(lead.OrigConsultant);
                offerKey = results.Rows(0).Column("OfferKey").GetValueAs<int>();
            }
            else
            {
                offerKey = browser.Page<ApplicationSummaryBase>().GetOfferKey();
            }
            Assert.That(offerKey != 0, "Lead not created");
            //Get the OfferKey
            //save the offerkey
            Service<ICommonService>().CommitOfferKeyForAutomationLeads(lead.TestIdentifier, offerKey, "AutomationLeads");
            //Get rid of the browser.
            browser.WaitForComplete(30);
            browser.Dispose();
            browser = null;
        }

        private void SubmitApplication(Automation.DataModels.OriginationTestCase application)
        {
            try
            {
                if (browser != null)
                {
                    if (application.Username.ToUpper() != this.PreviousUser.ToUpper())
                    {
                        browser.Dispose();
                        browser = new TestBrowser(application.Username);
                    }
                }
                else
                {
                    browser = new TestBrowser(application.Username);
                }
                browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(browser);
                int offerKey = Service<ICommonService>().GetOfferKeyByTestIdentifier(application.TestIdentifier);
                int accountKey = Service<IApplicationService>().GetOfferAccountKey(offerKey, true);
                browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().WorkFlows(browser);
                browser.Page<WorkflowSuperSearch>().Search(offerKey);
                const string houseHoldIncome = "50000";
                const string phoneCode = "031";
                const string phoneNumber = "1234567";
                const string employer = "Test & Data Services";
                const string startDate = "01/01/2000";
                // If Company/Trust/CC add Suretor
                if (application.LegalEntityType != "Natural Person")
                {
                    browser.Navigate<ApplicantsNode>().Applicants(NodeTypeEnum.Add);
                    browser.Page<LegalEntityDetailsLeadApplicantAdd>().AddLegalEntity(OfferRoleTypes.LeadSuretor, true, IDNumbers.GetNextIDNumber(), "Mr", "auto", "SuretorFirstname", "SuretorSurname", "auto",
                        Gender.Male, MaritalStatus.Single, "Unknown", "Unknown", CitizenType.SACitizen, "auto", null, null, "Unknown", Language.English, null, phoneCode, phoneNumber, null, null, null, null,
                        null, null, true, false, false, false, false, ButtonTypeEnum.Next);
                }
                // Get LegalEntityLegalNames of all Main Applicant and Suretor LegalEntities on Offer
                var results = Service<ILegalEntityService>().GetLegalEntityLegalNamesForOffer(offerKey);
                // Iterate through Main Applicant and Suretor LegalEntities on Offer and update LegalEntity Information
                for (int row = 0; row < results.RowList.Count; row++)
                {
                    int legalEntityKey = results.Rows(row).Column("LegalEntityKey").GetValueAs<int>();
                    int legalEntityTypeKey = results.Rows(row).Column("LegalEntityTypeKey").GetValueAs<int>();
                    browser.Navigate<LegalEntityNode>().LegalEntity_ByLegalEntityKey(legalEntityKey);
                    if (legalEntityTypeKey == (int)LegalEntityTypeEnum.CloseCorporation || legalEntityKey == (int)LegalEntityTypeEnum.Trust || legalEntityKey == (int)LegalEntityTypeEnum.Company)
                    {
                        // Add Employment Details
                        browser.Navigate<LegalEntityNode>().EmploymentDetails(NodeTypeEnum.Add);
                        browser.Page<LegalEntityEmploymentDetails>().AddEmploymentDetails(employer, EmploymentType.SelfEmployed, RemunerationType.BusinessProfits, startDate, houseHoldIncome, true);
                    }
                }
                if (!application.ProcessViaWFAutomation)
                {
                    browser.ClickWorkflowLoanNode(Workflows.ApplicationCapture);
                    browser.ClickAction(WorkflowActivities.ApplicationCapture.SubmitApplication);
                    browser.Page<WorkflowYesNo>().Confirm(true, true);
                    //Assert Branch Consultant D offerrole exists
                    AssignmentAssertions.AssertOfferRoleRecordExists(offerKey, OfferRoleTypeEnum.BranchConsultantD);
                    AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(offerKey, OfferRoleTypeEnum.BranchConsultantD);
                }
                else
                {
                    browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(browser);
                    scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.ApplicationCapture, WorkflowAutomationScripts.ApplicationCapture.SubmitApplicationNoCleanup, offerKey);
                }
            }
            finally
            {
                this.PreviousUser = application.Username;
            }
        }

        private void AssertSubmitApplication(Automation.DataModels.OriginationTestCase application)
        {
            int offerKey = Service<ICommonService>().GetOfferKeyByTestIdentifier(application.TestIdentifier);
            OfferRoleTypeEnum existingWorkflowAssignment = OfferRoleTypeEnum.BranchConsultantD;
            if (application.LoanType == "New purchase")
            {
                X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.QA);

                //Assert QA Administrator D offerrole exists
                AssignmentAssertions.AssertOfferRoleRecordExists(offerKey, OfferRoleTypeEnum.QAAdministratorD);
                AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(offerKey, OfferRoleTypeEnum.QAAdministratorD);
                AssignmentAssertions.AssertThatAWorkFlowAssignmentRecordExists(offerKey, OfferRoleTypeEnum.QAAdministratorD);
                AssignmentAssertions.AssertThatTheWorkFlowAssignmentRecordIsActive(offerKey, OfferRoleTypeEnum.QAAdministratorD);

                PropertyValuationAssertions.AssertRequireValuationIndicatorValue(offerKey, true);
            }
            else
            {
                X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.ManageApplication);
                /*A Valuation Hold clone used to be created but moving from Application Capture state to Manage Application state until the introduction of the Valuation Approval state in the Credit workflow was introduced*/
                X2Assertions.AssertX2CloneDoesNotExist(offerKey, WorkflowStates.ApplicationManagementWF.ValuationHold, Workflows.ApplicationManagement);

                //Assert New Business Processor D offerrole exists
                AssignmentAssertions.AssertOfferRoleRecordExists(offerKey, OfferRoleTypeEnum.NewBusinessProcessorD);
                AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(offerKey, OfferRoleTypeEnum.NewBusinessProcessorD);
                AssignmentAssertions.AssertThatAWorkFlowAssignmentRecordExists(offerKey, OfferRoleTypeEnum.NewBusinessProcessorD);
                AssignmentAssertions.AssertThatTheWorkFlowAssignmentRecordIsActive(offerKey, OfferRoleTypeEnum.NewBusinessProcessorD);

                PropertyValuationAssertions.AssertRequireValuationIndicatorValue(offerKey, false);
            }
            AssignmentAssertions.AssertThatTheWorkFlowAssignmentRecordIsInactive(offerKey, existingWorkflowAssignment);
            AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(offerKey, existingWorkflowAssignment);
        }
    }
}