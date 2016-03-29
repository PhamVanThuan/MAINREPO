using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.Origination;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;

namespace ApplicationCaptureTests.Views.LegalEntityFloBo
{
    /// <summary>
    /// Tests for legal entity Application Declarations
    /// </summary>
    [TestFixture, RequiresSTA]
    public class ApplicationDeclarationsTests : TestBase<ApplicationDeclarations>
    {
        private int offerKey;
        private int _legalEntityKey;

        #region Setup/Teardown

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = Helper.CreateApplicationWithBrowser(TestUsers.BranchConsultant10, out offerKey);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Task();
            base.Browser.Page<WorkflowSuperSearch>().Search(offerKey);
            _legalEntityKey = base.Service<IApplicationService>().GetFirstApplicantLegalEntityKeyOnOffer(offerKey);
        }

        protected override void OnTestStart()
        {
            base.OnTestStart();
            base.Browser.Navigate<LegalEntityNode>().LegalEntity(offerKey);
            base.Browser.Navigate<LegalEntityNode>().ApplicationDeclarations(NodeTypeEnum.Update);
        }

        #endregion Setup/Teardown

        /// <summary>
        /// Tests that all the application declaration questions are required to be answered. Checks that if all questions are not answered
        /// a validation error message is displayed and the answers don't save.
        /// </summary>
        [Test, Description("Tests that all application declaration answers are mandatory")]
        public void ApplicationDeclarationMandatory()
        {
            var appDeclarations = new Automation.DataModels.ApplicationDeclaration
                                      {
                                          InsolvencyAnswer = "-select-",
                                          DateRehabilitatedAnswer = null,
                                          AdministrationOrderAnswer = "-select-",
                                          DateRescindedAnswer = null,
                                          CurrentUnderDebtCounsellingAnswer = "-select-",
                                          CurrentDebtRearrangementAnswer = "-select-",
                                          ConductCreditCheckAnswer = "-select-"
                                      };
            base.View.ApplicationDeclarationUpdate(appDeclarations);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Application Declaration Answer is a mandatory field");
            ApplicationDeclarationsAssertions.AssertApplicationDeclarationsDoNotExist(offerKey, _legalEntityKey);
        }

        /// <summary>
        /// Tests that if a legal entity has ever been declared insolvent, the rehabilitation date is required to be answered. Checks that
        /// if the rehabilitation date is not answered a validation error message is displayed and the answers don't save.
        /// </summary>
        [Test, Description("Tests that the rehabilitation date is mandatory if insolvency has been declared")]
        public void ApplicationDeclarationRehabilitationDateMandatory()
        {
            var app = new Automation.DataModels.ApplicationDeclaration
            {
                InsolvencyAnswer = "Yes",
                DateRehabilitatedAnswer = null,
                AdministrationOrderAnswer = "-select-",
                DateRescindedAnswer = null,
                CurrentUnderDebtCounsellingAnswer = "-select-",
                CurrentDebtRearrangementAnswer = "-select-",
                ConductCreditCheckAnswer = "-select-"
            };
            base.View.ApplicationDeclarationUpdate(app);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("'Date rehabilitated from insolvency' must be entered.");
            ApplicationDeclarationsAssertions.AssertApplicationDeclarationsDoNotExist(offerKey, _legalEntityKey);
        }

        /// <summary>
        /// Tests that the rehabilitation date is not required to be answered if the legal entity has not declared insolvency. Checks that if the
        /// rehabilitation date has been incorrectly answered a validation error message is displayed and the answers don't save.
        /// </summary>
        [Test, Description("Tests that the rehabilitation date is not mandatory if insolvency has not been declared")]
        public void ApplicationDeclarationRehabilitationDateNotRequired()
        {
            var app = new Automation.DataModels.ApplicationDeclaration
                          {
                              InsolvencyAnswer = "No",
                              DateRehabilitatedAnswer = "01/01/2011",
                              AdministrationOrderAnswer = "-select-",
                              DateRescindedAnswer = null,
                              CurrentUnderDebtCounsellingAnswer = "-select-",
                              CurrentDebtRearrangementAnswer = "-select-",
                              ConductCreditCheckAnswer = "-select-"
                          };
            base.View.ApplicationDeclarationUpdate(app);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("'Date rehabilitated from insolvency' must not be entered.");
            ApplicationDeclarationsAssertions.AssertApplicationDeclarationsDoNotExist(offerKey, _legalEntityKey);
        }

        /// <summary>
        /// Tests that if a legal entity has ever been under an administration order, the date that the order was rescinded is required to be answered.
        /// Checks that if the date rescinded is not answered a validation error message is displayed and the answers don't save.
        /// </summary>
        [Test, Description("Tests that the date rescinded is mandatory if ever been under administration")]
        public void ApplicationDeclarationDateRescindedMandatory()
        {
            var app = new Automation.DataModels.ApplicationDeclaration
                          {
                              InsolvencyAnswer = "-select-",
                              DateRehabilitatedAnswer = null,
                              AdministrationOrderAnswer = "Yes",
                              DateRescindedAnswer = null,
                              CurrentUnderDebtCounsellingAnswer = "-select-",
                              CurrentDebtRearrangementAnswer = "-select-",
                              ConductCreditCheckAnswer = "-select-"
                          };
            base.View.ApplicationDeclarationUpdate(app);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("'Date administration order rescinded' must be entered.");
            ApplicationDeclarationsAssertions.AssertApplicationDeclarationsDoNotExist(offerKey, _legalEntityKey);
        }

        /// <summary>
        /// Tests that the date rescinded is not required to be answered if a legal entity has never been under an administration order. Checks that if
        /// the date rescinded has been incorrectly answered a validation error message is displayed and the answers don't save.
        /// </summary>
        [Test, Description("Tests that the date rescinded is not mandatory if never been under administration")]
        public void ApplicationDeclarationAdministrationDateNotRequired()
        {
            var app = new Automation.DataModels.ApplicationDeclaration
                          {
                              InsolvencyAnswer = "-select-",
                              DateRehabilitatedAnswer = null,
                              AdministrationOrderAnswer = "No",
                              DateRescindedAnswer = "01/01/2011",
                              CurrentUnderDebtCounsellingAnswer = "-select-",
                              CurrentDebtRearrangementAnswer = "-select-",
                              ConductCreditCheckAnswer = "-select-"
                          };
            base.View.ApplicationDeclarationUpdate(app);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("'Date administration order rescinded' must not be entered.");
            ApplicationDeclarationsAssertions.AssertApplicationDeclarationsDoNotExist(offerKey, _legalEntityKey);
        }

        /// <summary>
        /// Tests that all 7 application declaration questions are answered and saved correctly.
        /// </summary>
        [Test, Description("Tests that all 7 application declaration questions are answered and saved correctly.")]
        public void ApplicationDeclarationsAnswered()
        {
            var app = new Automation.DataModels.ApplicationDeclaration
                          {
                              InsolvencyAnswer = "No",
                              DateRehabilitatedAnswer = string.Empty,
                              AdministrationOrderAnswer = "No",
                              DateRescindedAnswer = string.Empty,
                              CurrentUnderDebtCounsellingAnswer = "No",
                              CurrentDebtRearrangementAnswer = "No",
                              ConductCreditCheckAnswer = "Yes"
                          };
            base.View.ApplicationDeclarationUpdate(app);
            ApplicationDeclarationsAssertions.AssertDeclarationsExist(app, offerKey, _legalEntityKey);
        }
    }
}