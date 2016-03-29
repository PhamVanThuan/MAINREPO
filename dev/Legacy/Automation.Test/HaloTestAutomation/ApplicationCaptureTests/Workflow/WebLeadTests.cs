using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.InternetComponents.Calculators;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;

namespace ApplicationCaptureTests.Workflow
{
    [RequiresSTA]
    [Ignore]
    public class WebLeadTests : TestBase<BasePage>
    {
        /// <summary>
        /// This will test that a web lead can be captured and that it can be picked up by a consultant and be moved to the Manage Lead state.
        /// </summary>
        //[Ignore]
        [Test, Sequential, Description("Create a web lead using the internet calculators and process it in the application capture workflow")]
        public void _047_CreateAndProcessWebLead()
        {
            var browser = new TestBrowser(isSahomeloansWebsite: true);
            try
            {
                browser.Navigate<BuildingBlocks.Navigation.InternetComponents.WebsiteCalculators>(hasFrame: false).ClickCalculators();
                browser.Navigate<BuildingBlocks.Navigation.InternetComponents.WebsiteCalculators>(hasFrame: false).ClickAffordabilityCalculator();

                //random employment from 2am
                var clientEmp = Service<ILegalEntityService>().GetRandomLegalEntityEmploymentRecord
                                            (
                                                Common.Enums.EmploymentStatusEnum.Current,
                                                Common.Enums.RemunerationTypeEnum.Salaried,
                                                Common.Enums.EmploymentTypeEnum.Salaried
                                            );

                //Random OfferInformationVariableLoanRecord

                var offerdetail = base.Service<IApplicationService>().GetRandomLatestOfferInformationMortgageLoanRecord(MortgageLoanPurposeEnum.Newpurchase,
                    ProductEnum.NewVariableLoan, OfferStatusEnum.Open);

                //Capture employment and loan detail
                clientEmp.Income01 = (clientEmp.MonthlyIncome / 2);
                clientEmp.Income02 = (clientEmp.MonthlyIncome / 2);

                float installmentMax = base.Service<ICalculationsService>().CalculateAffordabilityInstallment(clientEmp.MonthlyIncome);
                if (offerdetail.MonthlyInstalment >= installmentMax)
                    offerdetail.MonthlyInstalment = installmentMax;
                browser.Page<AffordabilityCalculator>(hasFrame: false).PopulateCalculator(clientEmp, offerdetail);
                browser.Page<AffordabilityCalculator>(hasFrame: false).Calculate();
                browser.Page<AffordabilityCalculator>(hasFrame: false).Apply();

                //Capture client info
                var client = Service<ILegalEntityService>().GetRandomLegalEntityRecord(LegalEntityTypeEnum.NaturalPerson);

                browser.Page<CalculatorApplicationForm>(hasFrame: false).PopulateApplicantDetail(client, 2);
                browser.Page<CalculatorApplicationForm>(hasFrame: false).Submit();

                //Get the offerkey off the view
                int offerKey = browser.Navigate<BuildingBlocks.Navigation.InternetComponents.WebsiteCalculators>(hasFrame: false).GetOfferKey();

                browser.Dispose();

                //Do Assertions.
                AssignmentAssertions.AssertOfferRoleRecordExists(offerKey, OfferRoleTypeEnum.CommissionableConsultant);
                AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(offerKey, OfferRoleTypeEnum.CommissionableConsultant);
                AssignmentAssertions.AssertOfferRoleRecordExists(offerKey, OfferRoleTypeEnum.BranchConsultantD);
                AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(offerKey, OfferRoleTypeEnum.BranchConsultantD);
                AssignmentAssertions.AssertThatAWorkFlowAssignmentRecordExists(offerKey, OfferRoleTypeEnum.BranchConsultantD);
                AssignmentAssertions.AssertThatTheWorkFlowAssignmentRecordIsActive(offerKey, OfferRoleTypeEnum.BranchConsultantD);
                X2Assertions.AssertCurrentAppCapX2State(offerKey, WorkflowStates.ApplicationCaptureWF.InternetLead);

                //Login as consultant
                browser = new TestBrowser(TestUsers.BranchConsultant, TestUsers.Password);

                browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Task();
                browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().WorkFlows(browser);
                browser.Page<WorkflowSuperSearch>().Search(offerKey);
                browser.ClickAction(WorkflowActivities.ApplicationCapture.ProcessInternetLead);
                browser.Page<WorkflowYesNo>().Confirm(true, true);

                //Do Assertions.
                AssignmentAssertions.AssertOfferRoleRecordExists(offerKey, OfferRoleTypeEnum.CommissionableConsultant);
                AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(offerKey, OfferRoleTypeEnum.CommissionableConsultant);
                AssignmentAssertions.AssertOfferRoleRecordExists(offerKey, OfferRoleTypeEnum.BranchConsultantD);
                AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(offerKey, OfferRoleTypeEnum.BranchConsultantD);
                AssignmentAssertions.AssertThatAWorkFlowAssignmentRecordExists(offerKey, OfferRoleTypeEnum.BranchConsultantD);
                AssignmentAssertions.AssertThatTheWorkFlowAssignmentRecordIsActive(offerKey, OfferRoleTypeEnum.BranchConsultantD);
                X2Assertions.AssertCurrentAppCapX2State(offerKey, WorkflowStates.ApplicationCaptureWF.ManageLead);
            }
            catch
            {
                throw;
            }
            finally
            {
                browser.Dispose();
                browser = null;
            }
        }

        /// <summary>
        /// This will test that a web application can be captured and that it can be picked up by a consultant and be moved to the application capture state.
        /// </summary>
        //[Ignore]
        [Test, Sequential, Description("Create a web application using the internet calculators and process it in the application capture workflow")]
        public void _048_CreateAndProcessWebApplication()
        {
            var browser = new TestBrowser(isSahomeloansWebsite: true);
            try
            {
                browser.Navigate<BuildingBlocks.Navigation.InternetComponents.WebsiteCalculators>(hasFrame: false).ClickCalculators();
                browser.Navigate<BuildingBlocks.Navigation.InternetComponents.WebsiteCalculators>(hasFrame: false).ClickNewPurchaseCalculator();

                //random employment from 2am
                var clientEmp = Service<ILegalEntityService>().GetRandomLegalEntityEmploymentRecord
                                            (
                                                Common.Enums.EmploymentStatusEnum.Current,
                                                Common.Enums.RemunerationTypeEnum.Salaried,
                                                Common.Enums.EmploymentTypeEnum.Salaried
                                            );
                //Change this so that we qualify
                clientEmp.MonthlyIncome = 200000.00f;
                clientEmp.Income01 = (clientEmp.MonthlyIncome / 2);
                clientEmp.Income02 = (clientEmp.MonthlyIncome / 2);

                //Random OfferInformationVariableLoanRecord
                var offerdetail = base.Service<IApplicationService>().GetRandomLatestOfferInformationMortgageLoanRecord(MortgageLoanPurposeEnum.Newpurchase, ProductEnum.NewVariableLoan, OfferStatusEnum.Open);

                //Capture employment and loan detail
                browser.Page<BuildingBlocks.Websites.NewPurchaseCalculator>(hasFrame: false).PopulateCalculator(clientEmp, offerdetail);
                browser.Page<BuildingBlocks.Websites.NewPurchaseCalculator>(hasFrame: false).Calculate();
                browser.Page<BuildingBlocks.Websites.NewPurchaseCalculator>(hasFrame: false).Apply();

                //Capture client info
                var client = Service<ILegalEntityService>().GetRandomLegalEntityRecord(LegalEntityTypeEnum.NaturalPerson);
                browser.Page<NewPurchaseApplicationForm>(hasFrame: false).PopulateApplicantDetail(client, 2);
                browser.Page<NewPurchaseApplicationForm>(hasFrame: false).Submit();

                //Get the offerkey off the view
                int offerKey = browser.Page<NewPurchaseApplicationForm>(hasFrame: false).GetReferenceNumber();
                browser.Dispose();

                //Do Assertions.
                AssignmentAssertions.AssertOfferRoleRecordExists(offerKey, OfferRoleTypeEnum.CommissionableConsultant);
                AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(offerKey, OfferRoleTypeEnum.CommissionableConsultant);
                AssignmentAssertions.AssertOfferRoleRecordExists(offerKey, OfferRoleTypeEnum.BranchConsultantD);
                AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(offerKey, OfferRoleTypeEnum.BranchConsultantD);
                AssignmentAssertions.AssertThatAWorkFlowAssignmentRecordExists(offerKey, OfferRoleTypeEnum.BranchConsultantD);
                AssignmentAssertions.AssertThatTheWorkFlowAssignmentRecordIsActive(offerKey, OfferRoleTypeEnum.BranchConsultantD);
                X2Assertions.AssertCurrentAppCapX2State(offerKey, WorkflowStates.ApplicationCaptureWF.InternetApplication);

                //Login as consultant
                browser = new TestBrowser(TestUsers.BranchConsultant, TestUsers.Password);

                browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Task();
                browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().WorkFlows(browser);
                browser.Page<WorkflowSuperSearch>().Search(offerKey);
                browser.ClickAction(WorkflowActivities.ApplicationCapture.ProcessInternetApplication);
                browser.Page<WorkflowYesNo>().Confirm(true, true);

                //Do Assertions.
                AssignmentAssertions.AssertOfferRoleRecordExists(offerKey, OfferRoleTypeEnum.CommissionableConsultant);
                AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(offerKey, OfferRoleTypeEnum.CommissionableConsultant);
                AssignmentAssertions.AssertOfferRoleRecordExists(offerKey, OfferRoleTypeEnum.BranchConsultantD);
                AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(offerKey, OfferRoleTypeEnum.BranchConsultantD);
                AssignmentAssertions.AssertThatAWorkFlowAssignmentRecordExists(offerKey, OfferRoleTypeEnum.BranchConsultantD);
                AssignmentAssertions.AssertThatTheWorkFlowAssignmentRecordIsActive(offerKey, OfferRoleTypeEnum.BranchConsultantD);
                X2Assertions.AssertCurrentAppCapX2State(offerKey, WorkflowStates.ApplicationCaptureWF.ApplicationCapture);
            }
            catch
            {
                throw;
            }
            finally
            {
                browser.Dispose();
                browser = null;
            }
        }
    }
}