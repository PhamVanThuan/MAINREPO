using BuildingBlocks;
using BuildingBlocks.Navigation;
using BuildingBlocks.Navigation.FLOBO.Common;
using BuildingBlocks.Presenters.LegalEntity;
using BuildingBlocks.Presenters.Origination;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;

using Navigation = BuildingBlocks.Navigation;

namespace ApplicationCaptureTests
{
    public static class Helper
    {
        private static readonly IApplicationService applicationService;
        private static readonly ICommonService commonService;
        private static readonly ILegalEntityService legalEntityService;

        static Helper()
        {
            applicationService = ServiceLocator.Instance.GetService<IApplicationService>();
            commonService = ServiceLocator.Instance.GetService<ICommonService>();
            legalEntityService = ServiceLocator.Instance.GetService<ILegalEntityService>();
        }

        /// <summary>
        /// Creates an application through the Application Calculator
        /// </summary>
        /// <param name="browser">IE TestBrowser</param>
        /// <param name="consultant">the consultant to use to create the case</param>
        public static int CreateApplication(TestBrowser browser, string consultant)
        {
            browser = new TestBrowser(consultant, TestUsers.Password);
            //Clear FloBo
            browser.Navigate<Navigation.NavigationHelper>().Task();
            browser.Navigate<Navigation.NavigationHelper>().CloseLoanNodesFLOBO(browser);
            //application calculator
            browser.Navigate<Navigation.NavigationHelper>().Menu(browser);
            browser.Navigate<Navigation.CalculatorsNode>().Calculators(browser, CalculatorNodeTypeEnum.ApplicationCalculator);
            //complete the calculator
            browser.Page<BuildingBlocks.Views.LoanCalculator>().LoanCalculatorLead_Refinance("New Variable Loan", "978500", "450000", "Salaried", null, false, "28500",
                ButtonTypeEnum.CreateApplication);
            //add the LE
            browser.Page<LegalEntityDetailsLeadApplicantAdd>().AddLegalEntity(OfferRoleTypes.LeadMainApplicant, true, null, "Miss", "auto", "TestName", "TestSurname", null, Gender.Female,
                MaritalStatus.Single, "Unknown", "Unknown", CitizenType.SACitizen, null, null, null, "Unknown", Language.English, null, null, null, null, null, null,
                null, "0761948851", null, false, false, false, true, false, ButtonTypeEnum.Next);
            int offerKey = 0;
            offerKey = browser.Page<ApplicationSummaryBase>().GetOfferKey();
            //assert offer created
            Assert.That(offerKey > 0, "Offer not created");
            browser.Dispose();
            return offerKey;
        }

        /// <summary>
        /// Creates an application through the Application Calculator without disposing the browser (ref it back instead for use)
        /// </summary>
        /// <param name="browser">reference IE TestBrowser</param>
        /// <param name="consultant">consultant</param>
        /// <param name="_offerKey">out offerkey</param>
        /// <param name="idNumber">out IdNumber</param>
        public static void CreateApplication(ref TestBrowser browser, string consultant, OfferTypeEnum appType, out int _offerKey, out string idNumber)
        {
            browser = new TestBrowser(consultant);
            //Clear FloBo
            browser.Navigate<Navigation.NavigationHelper>().Task();
            browser.Navigate<Navigation.NavigationHelper>().CloseLoanNodesFLOBO(browser);
            //application calculator
            browser.Navigate<Navigation.NavigationHelper>().Menu(browser);
            browser.Navigate<Navigation.CalculatorsNode>().Calculators(browser, CalculatorNodeTypeEnum.ApplicationCalculator);
            //complete the calculator
            switch (appType)
            {
                case OfferTypeEnum.SwitchLoan:
                    browser.Page<BuildingBlocks.Views.LoanCalculator>().LoanCalculatorLead_Switch(Products.NewVariableLoan, "1500000", "250000", "75000", EmploymentType.Salaried,  null,
                        true, "50000", ButtonTypeEnum.CreateApplication);
                    break;

                case OfferTypeEnum.Refinance:
                    browser.Page<BuildingBlocks.Views.LoanCalculator>().LoanCalculatorLead_Refinance(Products.NewVariableLoan, "978500", "450000", EmploymentType.Salaried, null, false,
                        "28500", ButtonTypeEnum.CreateApplication);
                    break;

                case OfferTypeEnum.NewPurchase:
                    browser.Page<BuildingBlocks.Views.LoanCalculator>().LoanCalculatorLead_NewPurchase(Products.NewVariableLoan, "1500000", "325000", EmploymentType.Salaried, null,
                        "50000", ButtonTypeEnum.CreateApplication);
                    break;

                default:
                    break;
            }
            //add the LE
            idNumber = IDNumbers.GetNextIDNumber();
            string dateOfBirth = commonService.GetDateOfBirthFromIDNumber(idNumber);
            browser.Page<LegalEntityDetailsLeadApplicantAdd>().AddLegalEntity(OfferRoleTypes.LeadMainApplicant, true, idNumber, SalutationType.Miss, "T", "Test", "Case", null,
                Gender.Female, MaritalStatus.MarriedAnteNuptualContract, "Unknown", "Unknown", CitizenType.SACitizen, dateOfBirth, null,
                null, "Unknown", Language.English, null, null, null, null, null, null, null, "0761948851", null, false, false, false, true, false,
                ButtonTypeEnum.Next);
            int offerKey = 0;
            offerKey = browser.Page<ApplicationSummaryBase>().GetOfferKey();
            //assert offer created
            Assert.That(offerKey > 0, "Offer not created");
            _offerKey = offerKey;
            applicationService.CleanupNewBusinessOffer(offerKey);
        }

        /// <summary>
        /// Add an natural person applicant to an application.
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="offerKey"></param>
        /// <param name="legalentityIdNumber"></param>
        /// <param name="marriedCop"></param>
        public static void AddNaturalPersonApplicantToOffer(TestBrowser browser, int offerKey, out string legalentityIdNumber, bool marriedCop)
        {
            string maritalStatus = marriedCop ? MaritalStatus.MarriedCommunityofProperty : MaritalStatus.MarriedAnteNuptualContract;
            legalentityIdNumber = IDNumbers.GetNextIDNumber();
            //go to the add applicants node
            browser.Navigate<ApplicantsNode>().Applicants(NodeTypeEnum.Add);
            browser.Page<LegalEntityDetails>().AddLegalEntity
                (
                    false, legalentityIdNumber, OfferRoleTypes.LeadMainApplicant, true,
                    "Mrs", "A", "AssetLiability", "Associate", "AssetLiability", Gender.Female, maritalStatus,
                    PopulationGroup.Unknown, Education.Unknown, CitizenType.SACitizen, string.Empty, string.Empty,
                    Language.English, Language.English, "Alive", "012", "1234567", string.Empty, string.Empty, string.Empty,
                    string.Empty, string.Empty, string.Empty, true, true, true, true, true, string.Empty,
                    commonService.GetDateOfBirthFromIDNumber(legalentityIdNumber)
                );
        }

        /// <summary>
        /// Create a spouse relationship between selected legalentity in flobo and given id number.
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="legalentityIdNumber"></param>
        /// <param name="relationshipType"></param>
        public static void SetupSelectedLegalEntityRelationship(TestBrowser browser, string legalentityIdNumber, string relationshipType)
        {
            browser.Navigate<LegalEntityNode>().LegalEntityRelationships(NodeTypeEnum.Add);
            browser.Page<LegalEntityRelationshipAdd>().AddNewLegalEntity();
            browser.Page<LegalEntityDetails>().AddExisting(legalentityIdNumber);
            browser.Page<LegalEntityRelationshipsRelate>().AddRelationship(relationshipType);
        }

        /// <summary>
        /// Creates an application through the Application Calculator
        /// </summary>
        /// <param name="offerKey">OfferKey</param>
        /// <param name="consultant">the consultant to use to create the case</param>
        public static TestBrowser CreateApplicationWithBrowser(string consultant, out int offerKey)
        {
            var browser = new TestBrowser(consultant, TestUsers.Password);
            //Clear FloBo

            browser.Navigate<Navigation.NavigationHelper>().Task();
            browser.Navigate<Navigation.NavigationHelper>().CloseLoanNodesFLOBO(browser);
            //application calculator
            browser.Navigate<Navigation.NavigationHelper>().Menu(browser);

            browser.Navigate<Navigation.CalculatorsNode>().Calculators(browser, CalculatorNodeTypeEnum.ApplicationCalculator);
            //complete the calculator
            browser.Page<BuildingBlocks.Views.LoanCalculator>().LoanCalculatorLead_Refinance("New Variable Loan", "978500", "450000", "Salaried", null, false, "28500",
                ButtonTypeEnum.CreateApplication);
            //add the LE
            browser.Page<LegalEntityDetailsLeadApplicantAdd>().AddLegalEntity(OfferRoleTypes.LeadMainApplicant, true, null, "Miss", "auto", "TestName", "TestSurname", null, Gender.Female,
                MaritalStatus.Single, "Unknown", "Unknown", CitizenType.SACitizen, null, null, null, "Unknown", Language.English, null, null, null, null, null, null,
                null, "0761948851", null, false, false, false, true, false, ButtonTypeEnum.Next);
            offerKey = browser.Page<ApplicationSummaryBase>().GetOfferKey();
            //assert offer created
            Assert.That(offerKey > 0, "Offer not created");
            return browser;
        }

        /// <summary>
        /// Gets the legalentitykey
        /// </summary>
        /// <param name="offerKey">offerKey</param>
        /// <returns>legalEntityKey</returns>
        public static int GetLegalEntityKey(int offerKey)
        {
            var results = legalEntityService.GetLegalEntityLegalNamesForOffer(offerKey);
            return results.Rows(0).Column("LegalEntityKey").GetValueAs<int>();
        }
    }
}