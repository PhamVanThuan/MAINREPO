using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation;
using BuildingBlocks.Presenters.PersonalLoans;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;

namespace PersonalLoansTests.Views
{
    /// <summary>
    /// Tests for Personal Loans Applicaton Declarations
    /// </summary>
    [TestFixture, RequiresSTA]
    public class PersonalLoanApplicationDeclarationTests : PersonalLoansWorkflowTestBase<PersonalLoanApplicationDeclarations>
    {
        private Automation.DataModels.LegalEntity legalEntityModel;

        #region Setup and Teardown

        /// <summary>
        /// Setup the test base
        /// </summary>
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            // Find case at state 'Manage Application' and then Get the IDNumber from the offer key
            base.FindCaseAtState(WorkflowStates.PersonalLoansWF.ManageApplication, WorkflowRoleTypeEnum.PLConsultantD);
            // Get the Legal Entity model from the Id Number
            legalEntityModel = base.Service<ILegalEntityService>().GetLegalEntity(base.Service<IAccountService>().GetIDNumberforExternalRoleOnOffer(base.GenericKey));
        }

        /// <summary>
        /// Start the test base
        /// </summary>
        protected override void OnTestStart()
        {
            base.OnTestStart();

            // Delete any previous declarations
            base.Service<IApplicationService>().DeleteExternalRoleDeclarations(base.GenericKey);

            // Locate the Legal Entity and click the application declarations node
            base.Browser.Navigate<LegalEntityNode>().LegalEntity_ByLegalEntityKey(legalEntityModel.LegalEntityKey);
            base.Browser.Navigate<LegalEntityNode>().ApplicationDeclarations(NodeTypeEnum.Update);
        }

        #endregion Setup and Teardown

        #region Tests

        /// <summary>
        /// Tests that all required application declarations are mandatory.
        /// </summary>
        [Test, Description("Tests that all required application declarations are mandatory.")]
        public void when_capturing_declarations_all_mandatory_application_declarations_must_be_answered()
        {
            // First setup some 'un-selected' declarations for use later
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

            // Update the application declarations
            base.View.ApplicationDeclarationUpdate(appDeclarations);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Application Declaration Answer is a mandatory field");
            ApplicationDeclarationsAssertions.AssertExternalApplicationDeclarationsDoNotExist(base.GenericKey, legalEntityModel.LegalEntityKey);
        }

        /// <summary>
        /// If insolvency has been declared, this method tests that the rehabilitation date is mandatory.
        /// If the date is not provided, a validation message is displayed and the answers should not be saved.
        /// </summary>
        [Test, Description("If insolvency has been declared, this method tests that the rehabilitation date is mandatory.")]
        public void when_the_client_has_been_declared_insolvent_a_rehabilitation_date_is_required()
        {
            var appDeclarations = new Automation.DataModels.ApplicationDeclaration
            {
                InsolvencyAnswer = "Yes",
                DateRehabilitatedAnswer = null,
                AdministrationOrderAnswer = "-select-",
                DateRescindedAnswer = null,
                CurrentUnderDebtCounsellingAnswer = "-select-",
                CurrentDebtRearrangementAnswer = "-select-",
                ConductCreditCheckAnswer = "-select-"
            };
            base.View.ApplicationDeclarationUpdate(appDeclarations);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("'Date rehabilitated from insolvency' must be entered.");
            ApplicationDeclarationsAssertions.AssertExternalApplicationDeclarationsDoNotExist(base.GenericKey, legalEntityModel.LegalEntityKey);
        }

        /// <summary>
        /// If insolvency has been NOT declared, this method tests that the rehabilitation date is NOT mandatory.
        /// If the date is incorrectly provided, a validation message is displayed and the answers should not be saved.
        /// </summary>
        [Test, Description("If insolvency has been NOT declared, this method tests that the rehabilitation date is NOT mandatory.")]
        public void when_the_client_has_not_been_declared_insolvent_no_rehabilitation_date_is_required()
        {
            var appDeclarations = new Automation.DataModels.ApplicationDeclaration
            {
                InsolvencyAnswer = "No",
                DateRehabilitatedAnswer = "01/01/2011",
                AdministrationOrderAnswer = "-select-",
                DateRescindedAnswer = null,
                CurrentUnderDebtCounsellingAnswer = "-select-",
                CurrentDebtRearrangementAnswer = "-select-",
                ConductCreditCheckAnswer = "-select-"
            };
            base.View.ApplicationDeclarationUpdate(appDeclarations);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("'Date rehabilitated from insolvency' must not be entered.");
            ApplicationDeclarationsAssertions.AssertExternalApplicationDeclarationsDoNotExist(base.GenericKey, legalEntityModel.LegalEntityKey);
        }

        /// <summary>
        /// If a legal entity has ever been under an administration order, this method tests that the date that the order was rescinded is required to be answered.
        /// If the date rescinded is not answered, a validation error message is displayed and the answers should not be saved.
        /// </summary>
        [Test, Description("If a legal entity has ever been under an administration order, this method tests that the date that the order was rescinded is required to be answered.")]
        public void when_the_client_has_been_under_an_administration_order_a_rescinded_date_is_required()
        {
            var appDeclarations = new Automation.DataModels.ApplicationDeclaration
            {
                InsolvencyAnswer = "-select-",
                DateRehabilitatedAnswer = null,
                AdministrationOrderAnswer = "Yes",
                DateRescindedAnswer = null,
                CurrentUnderDebtCounsellingAnswer = "-select-",
                CurrentDebtRearrangementAnswer = "-select-",
                ConductCreditCheckAnswer = "-select-"
            };
            base.View.ApplicationDeclarationUpdate(appDeclarations);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("'Date administration order rescinded' must be entered.");
            ApplicationDeclarationsAssertions.AssertExternalApplicationDeclarationsDoNotExist(base.GenericKey, legalEntityModel.LegalEntityKey);
        }

        /// <summary>
        /// Tests that the date rescinded is not required to be answered if a legal entity has never been under an administration order. Checks that if
        /// the date rescinded has been incorrectly answered a validation error message is displayed and the answers don't save.
        /// </summary>
        [Test, Description("Tests that the date rescinded is not mandatory if never been under administration")]
        public void when_the_client_has_not_been_under_an_administration_order_a_rescinded_date_is_not_required()
        {
            var appDeclarations = new Automation.DataModels.ApplicationDeclaration
            {
                InsolvencyAnswer = "-select-",
                DateRehabilitatedAnswer = null,
                AdministrationOrderAnswer = "No",
                DateRescindedAnswer = "01/01/2011",
                CurrentUnderDebtCounsellingAnswer = "-select-",
                CurrentDebtRearrangementAnswer = "-select-",
                ConductCreditCheckAnswer = "-select-"
            };
            base.View.ApplicationDeclarationUpdate(appDeclarations);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("'Date administration order rescinded' must not be entered.");
            ApplicationDeclarationsAssertions.AssertExternalApplicationDeclarationsDoNotExist(base.GenericKey, legalEntityModel.LegalEntityKey);
        }

        /// <summary>
        /// Tests that all required application declarations are answered and saved correctly.
        /// </summary>
        [Test, Description("Tests that all 7 required application declarations are answered and saved correctly.")]
        public void when_capturing_declarations_answers_to_all_questions_must_be_provided()
        {
            // First setup some 'un-answered' declarations for use later
            var appDeclarations = new Automation.DataModels.ApplicationDeclaration
            {
                InsolvencyAnswer = "No",
                DateRehabilitatedAnswer = null,
                AdministrationOrderAnswer = "No",
                DateRescindedAnswer = null,
                CurrentUnderDebtCounsellingAnswer = "No",
                CurrentDebtRearrangementAnswer = "No",
                ConductCreditCheckAnswer = "Yes"
            };

            base.View.ApplicationDeclarationUpdate(appDeclarations);
            ApplicationDeclarationsAssertions.AssertExternalDeclarationsExist(appDeclarations, base.GenericKey, legalEntityModel.LegalEntityKey);
        }

        #endregion Tests
    }
}