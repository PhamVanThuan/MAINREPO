using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation.CBO.LoanServicing;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.Origination.Valuations;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Linq;

namespace LoanServicingTests.Views.Account
{
    [RequiresSTA]
    public sealed class ValuationTests : BuildingBlocks.TestBase<ManualValuationAdd>
    {
        private Automation.DataModels.Property property;
        private Random randomizer;

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            this.randomizer = new Random();
            base.Browser = new TestBrowser(TestUsers.HaloUser);
            //Get any legalentity that plays an active role.
            var naturalLegalentity = Service<ILegalEntityService>().GetRandomLegalEntityRecord
                (
                    legalEntityType: LegalEntityTypeEnum.NaturalPerson,
                    documentLanguageKey: LanguageEnum.English
                );
            //Search navigate and load the legal entity and navigate the the udpate screen screen
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();

            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);
            base.Browser.Page<ClientSuperSearch>().PopulateSearch(naturalLegalentity);
            base.Browser.Page<ClientSuperSearch>().PerformSearch();
            base.Browser.Page<ClientSuperSearch>().SelectByLegalEntityKey(naturalLegalentity.LegalEntityKey);

            var account = Service<ILegalEntityService>().GetLegalEntityLoanAccount(naturalLegalentity.LegalEntityKey);
            base.Browser.Navigate<LoanServicingCBO>().LoanAccountNode(account.AccountKey);
            property = Service<IPropertyService>().GetProperty(accountkey: account.AccountKey);
        }

        #region AddTests

        [Test]
        public void ManualValuationAddTest()
        {
            var valuation = this.GetValuationTestData();
            valuation.IsActive = true;

            CaptureManualValuation(valuation, NodeTypeEnum.Add, captureMainBuilding: true, captureImprovement: true,
                                 captureOutbuilding: true, captureValuation: true);
            PropertyValuationAssertions.AssertProperty(property, expectedValuation: valuation);
        }

        [Test]
        public void MainDwellingAddControlTest()
        {
            var valuation = this.GetValuationTestData();

            //Change some values
            valuation.ValuationClassificationDescription = "- Please Select -";
            valuation.ValuationMainBuilding.ValuationRoofTypeDescription = "- Please Select -";
            valuation.ValuationMainBuilding.Extent = 0;
            valuation.ValuationMainBuilding.Rate = 0;

            CaptureManualValuation(valuation, NodeTypeEnum.Add, captureMainBuilding: true, assertMainBuildingValidationMessages: true);

            base.Browser.Page<ManualValuationsMainDwellingDetailsAdd>().AssertMainDwellingControls();
        }

        [Test]
        public void MainDwellingExtendedControlTest()
        {
            var valuation = this.GetValuationTestData();

            //Change some values
            valuation.ValuationImprovement.ImprovementValue = 0;
            valuation.ValuationImprovement.ValuationImprovementTypeDescription = "- Please Select -";
            valuation.ValuationImprovement.ImprovementDate = null;
            valuation.ValuationOutbuilding.ValuationRoofTypeDescription = "- Please Select -";
            valuation.ValuationOutbuilding.Extent = 0;
            valuation.ValuationOutbuilding.Rate = 0;

            CaptureManualValuation(valuation, NodeTypeEnum.Add, captureMainBuilding: true, captureImprovement: true, assertImprovementValidationMessages: true);
            base.Browser.Page<ManualValuationsMainDwellingExtendedDetailsAdd>().AssertMainDwellingExtendedImprovementControls();

            CaptureManualValuation(valuation, NodeTypeEnum.Add, captureMainBuilding: true, captureOutbuilding: true, assertOutbuildingValidationMessages: true);
            base.Browser.Page<ManualValuationsMainDwellingExtendedDetailsAdd>().AssertMainDwellingExtendedOutbuildingControls();
        }

        [Test]
        public void MainDwellingValuationControlTest()
        {
            var valuation = this.GetValuationTestData();

            valuation.Valuator.LegalEntity.RegisteredName = "- Please Select -";
            valuation.ValuationAmount = 0;
            valuation.ValuationMunicipal = 0;
            valuation.ValuationDate = null;
            valuation.HOCRoofDescription = "- Please Select -";
            valuation.HOCConventionalAmount = 0;
            valuation.HOCThatchAmount = 0;

            CaptureManualValuation(valuation, NodeTypeEnum.Add, captureMainBuilding: true, captureValuation: true, assertValuationValidationMessages: true);

            base.Browser.Page<ManualValuationAdd>().AssertManualValuationControls();
        }

        #endregion AddTests

        #region Helper

        private void CaptureManualValuation
            (
                Automation.DataModels.Valuation valuation,
                NodeTypeEnum nodeType,
                bool captureMainBuilding = false,
                bool captureOutbuilding = false,
                bool captureImprovement = false,
                bool captureValuation = false,
                bool assertMainBuildingValidationMessages = false,
                bool assertOutbuildingValidationMessages = false,
                bool assertImprovementValidationMessages = false,
                bool assertValuationValidationMessages = false
            )
        {
            base.Browser.Navigate<LoanServicingCBO>().ClickValuationDetails(nodeType);

            if (captureMainBuilding)
            {
                //Add Main Building
                base.Browser.Page<ManualValuationsMainDwellingDetailsAdd>().PopulateMainBuilding(valuation);
                base.Browser.Page<ManualValuationsMainDwellingDetailsAdd>().PopulateCottage(valuation.ValuationCottage);
                base.Browser.Page<ManualValuationsMainDwellingDetailsAdd>().Next();

                if (assertMainBuildingValidationMessages)
                {
                    base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(
                           "Valuation Classification is required",
                           "MainBuilding Roof Type is required",
                           "MainBuilding Extent (sq meters) is required",
                           "MainBuilding Rate (R /sq meter) is required"
                       );
                }
            }
            if (captureOutbuilding)
            {
                //Add outbuilding
                base.Browser.Page<ManualValuationsMainDwellingExtendedDetailsAdd>().PopulateOutbuilding(valuation.ValuationOutbuilding);
                base.Browser.Page<ManualValuationsMainDwellingExtendedDetailsAdd>().Add();

                if (assertOutbuildingValidationMessages)
                {
                    base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(
                         "Outbuilding Roof Type is required",
                         "Outbuilding Extent (sq meters) is required",
                         "Outbuilding Rate (R /sq meter) is required"
             );
                }
            }
            if (captureImprovement)
            {
                //Add improvement
                base.Browser.Page<ManualValuationsMainDwellingExtendedDetailsAdd>().PopulateImprovement(valuation.ValuationImprovement);
                base.Browser.Page<ManualValuationsMainDwellingExtendedDetailsAdd>().Add();

                if (assertImprovementValidationMessages)
                {
                    base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(
                         "Improvement Value is required",
                         "Valuation Improvement Type is a mandatory field"
                     );
                }
            }

            if (
                    (
                        (captureOutbuilding || captureImprovement)
                        && !assertImprovementValidationMessages && !assertOutbuildingValidationMessages
                    )
                    ||
                    (
                        captureValuation
                    )
               )
            {
                base.Browser.Page<ManualValuationsMainDwellingExtendedDetailsAdd>().Next();
            }

            if (captureValuation)
            {
                //Just go next on the summary
                base.Browser.Page<ManualValuationsMainDwellingDetails>().Next();

                //Add Valuation
                base.Browser.Page<ManualValuationAdd>().PopulateValuationDetail(valuation);
                base.Browser.Page<ManualValuationAdd>().Add();

                if (assertValuationValidationMessages)
                {
                    base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(
                            "Valuer is required.",
                            "Valuation Date must be entered.",
                            "Valuation Amount must be greater than R 100 000.00.",
                            "Valuation HOC Roof is required.",
                            "Valuation HOC Amount must be captured."
                        );
                }
            }
            base.Browser.Page<BasePage>().DomainWarningClickYesHandlingPopUp();
        }

        #endregion Helper

        private Automation.DataModels.Valuation GetValuationTestData()
        {
            var valuation = new Automation.DataModels.Valuation();

            valuation.ValuationDate = DateTime.Now;
            valuation.ValuationAmount = 760000;
            valuation.ChangeDate = DateTime.Now;
            valuation.PropertyKey = property.PropertyKey;
            valuation.HOCConventionalAmount = 300000;
            valuation.HOCThatchAmount = 300000;
            valuation.HOCRoofKey = HOCRoofEnum.Partial;
            valuation.HOCRoofDescription = HOCRoof.Partial;
            valuation.ValuationStatusKey = ValuationStatusEnum.Complete;

            //SAHL MANUAL VALUATION
            valuation.ValuationDataProviderDataServiceKey = 1;

            if (valuation.ValuationMainBuilding == null)
                valuation.ValuationMainBuilding = new Automation.DataModels.ValuationMainBuilding();

            if (valuation.ValuationCottage == null)
                valuation.ValuationCottage = new Automation.DataModels.ValuationCottage();

            if (valuation.ValuationOutbuilding == null)
                valuation.ValuationOutbuilding = new Automation.DataModels.ValuationOutbuilding();

            if (valuation.ValuationImprovement == null)
                valuation.ValuationImprovement = new Automation.DataModels.ValuationImprovement();

            if (valuation.Valuator == null)
            {
                var valuator = Service<IValuationService>().GetActiveValuators().FirstOrDefault();
                valuation.Valuator = new Automation.DataModels.Valuer();
                valuation.ValuatorKey = valuator.ValuatorKey;
                valuation.Valuator.LegalEntity = Service<ILegalEntityService>().GetLegalEntity(legalentitykey: valuator.LegalEntityKey);
            }

            valuation.ValuationMainBuilding.ValuationRoofTypeDescription = ValuationRoofType.Conventional;
            valuation.ValuationMainBuilding.Extent = randomizer.Next(100, 200);
            valuation.ValuationMainBuilding.Rate = randomizer.Next(6000, 7000);

            valuation.ValuationClassificationDescription = ValuationClassification.Budgetstandard;
            valuation.ValuationCottage.ValuationRoofTypeDescription = ValuationRoofType.Conventional;
            valuation.ValuationCottage.Extent = randomizer.Next(50, 100);
            valuation.ValuationCottage.Rate = randomizer.Next(6000, 7000);

            valuation.ValuationOutbuilding.ValuationRoofTypeDescription = ValuationRoofType.Conventional;
            valuation.ValuationOutbuilding.Extent = randomizer.Next(50, 100);
            valuation.ValuationOutbuilding.Rate = randomizer.Next(6000, 7000);

            valuation.ValuationImprovement.ImprovementDate = DateTime.Now;
            valuation.ValuationImprovement.ValuationImprovementTypeDescription = ValuationImprovementType.Walls;
            valuation.ValuationImprovement.ImprovementValue = randomizer.Next(15000, 60000);

            if (valuation != null && String.IsNullOrEmpty(valuation.HOCRoofDescription))
                valuation.HOCRoofDescription = "Conventional";

            return valuation;
        }
    }
}