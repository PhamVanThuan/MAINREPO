using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.LegalEntity;
using BuildingBlocks.Presenters.Origination;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Linq;
using Navigation = BuildingBlocks.Navigation;

namespace ApplicationCaptureTests.Fees
{
    [RequiresSTA]
    public class FeeTests : ApplicationCaptureTests.TestBase<BuildingBlocks.Views.LoanCalculator>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.BranchConsultant1);
        }

        [Test]
        public void when_create_application_should_have_initiation_fee_equal_to_control_table_amount()
        {
            base.Browser.Navigate<Navigation.NavigationHelper>().Menu(base.Browser);
            base.Browser.Navigate<Navigation.CalculatorsNode>().Calculators(base.Browser, CalculatorNodeTypeEnum.ApplicationCalculator);
            base.Browser.Page<BuildingBlocks.Views.LoanCalculator>().Populate();
            base.Browser.Page<BuildingBlocks.Views.LoanCalculator>().ClickCalculate();
            base.Browser.Page<BuildingBlocks.Views.LoanCalculator>().ClickCreateApplication();
            var legalentity = Service<ILegalEntityService>().GetLegalEntityIDNumberNotLinkedToOffer(0);
            base.Browser.Page<LegalEntityDetails>().AddExisting(legalentity.Column("idnumber").Value);
            int offerkey = base.Browser.Page<ApplicationSummaryBase>().GetOfferKey();
            OfferAssertions.AssertOfferExpense(offerkey, ControlNumeric.InitiationFee, false, ExpenseTypeEnum.InitiationFeeBondPreparationFee);
        }

        [Test]
        public void when_rework_application_should_have_a_discounted_initiation_fee()
        {
            var legalEntityReturningDiscountQualifyingData = Service<ILegalEntityService>().GetLegalEntityReturningDiscountQualifyingDataWithValidIDNumber();
            var legalEntity = (from le in legalEntityReturningDiscountQualifyingData
                               where le.MainApplicantOpenAccountsCount == 1 && le.ReturningClientDiscountOpenAccountsCount == 0
                               select le).FirstOrDefault();
            var offerKey = base.Service<IApplicationService>().GetRandomOfferWithOfferExpense(Workflows.ApplicationCapture, WorkflowStates.ApplicationCaptureWF.ApplicationCapture,
                OfferTypeEnum.NewPurchase, ControlNumeric.InitiationFee, ExpenseTypeEnum.InitiationFeeBondPreparationFee);
            Service<IApplicationService>().CreateOfferRole(legalEntity.LegalEntityKey, offerKey, OfferRoleTypeEnum.MainApplicant, GeneralStatusEnum.Active);
            base.Browser.Navigate<Navigation.NavigationHelper>().Task();
            base.Browser.Navigate<Navigation.NavigationHelper>().CloseLoanNodesFLOBO(base.Browser);
            base.Service<IApplicationService>().CleanupNewBusinessOffer(offerKey);
            base.Browser.Page<WorkflowSuperSearch>().Search(offerKey);
            base.Browser.ClickAction(WorkflowActivities.ApplicationCapture.UpdateLoanDetails);
            base.Browser.Page<ApplicationLoanDetailsUpdate>().RecalcAndSave(false);
            var data = base.Service<IApplicationService>().GetOfferData(offerKey);
            var offerStartDate = data.Rows(0).Column("OfferStartDate").GetValueAs<DateTime>();
            if (offerStartDate > Convert.ToDateTime(ControlNumeric.DiscountedInitiationFeeDateSwitch))
            {
                OfferAssertions.AssertOfferExpense(offerKey, (ControlNumeric.InitiationFee - (ControlNumeric.InitiationFee / 4)), false, ExpenseTypeEnum.InitiationFeeBondPreparationFee);
            }
            else
            {
                OfferAssertions.AssertOfferExpense(offerKey, ControlNumeric.InitiationFee * ControlNumeric.ReturningMainApplicantInitiationFeeDiscount, false, ExpenseTypeEnum.InitiationFeeBondPreparationFee);
            }
        }

        [Test]
        public void when_recalc_staff_application_should_have_initiation_fee_of_zero()
        {
            var offerKey = base.Service<IApplicationService>().GetRandomOfferWithOfferExpense(Workflows.ApplicationCapture, WorkflowStates.ApplicationCaptureWF.ApplicationCapture,
                OfferTypeEnum.NewPurchase, ControlNumeric.InitiationFee, ExpenseTypeEnum.InitiationFeeBondPreparationFee);
            base.Browser.Navigate<Navigation.NavigationHelper>().Task();
            base.Browser.Navigate<Navigation.NavigationHelper>().CloseLoanNodesFLOBO(base.Browser);
            base.Browser.Page<WorkflowSuperSearch>().Search(offerKey);
            base.Browser.ClickAction(WorkflowActivities.ApplicationCapture.UpdateLoanDetails);
            base.Browser.Page<ApplicationLoanDetailsUpdate>().Populate(isStaffLoan: true);
            base.Browser.Page<ApplicationLoanDetailsUpdate>().RecalcAndSave(false);
            OfferAssertions.AssertOfferExpense(offerKey, 0.00f, false, ExpenseTypeEnum.InitiationFeeBondPreparationFee);
        }
    }
}