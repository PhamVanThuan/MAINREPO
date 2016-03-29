using BuildingBlocks;
using BuildingBlocks.Navigation;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.Origination;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System.Linq;

namespace ApplicationCaptureTests.Rules
{
    [RequiresSTA]
    public class AlphaHousing : ApplicationCaptureTests.TestBase<ApplicationLoanDetailsUpdate>
    {
        [Test]
        public void when_changing_alpha_housing_loan_product_to_NON_standard_variable_should_get_error()
        {
            base.Browser = new TestBrowser(TestUsers.BranchConsultant10, TestUsers.Password);
            var results = Service<IX2WorkflowService>().GetOfferKeysAtStateByType(WorkflowStates.ApplicationCaptureWF.ApplicationCapture, Workflows.ApplicationCapture, OfferTypeEnum.NewPurchase, string.Empty);
            var offerkey = results.Rows(0).Column("offerkey").GetValueAs<int>();
            //This will put it in alpha housing
            Service<IApplicationService>().UpdateNewPurchaseVariableLoanOffer(offerkey, householdIncome: 15000f,
                                                                                      loanAgreementAmount: 468000f,
                                                                                      feesTotal: 11928.25f,
                                                                                      cashDeposit: 32000f,
                                                                                      propertyValuation: 480000f,
                                                                                      instalment: 1145.71f,
                                                                                      bondToRegister: 500000f,
                                                                                      linkRate: 0.039f,
                                                                                      term: 240,
                                                                                      marketRateKey: MarketRateEnum._3MonthJibar_Rounded,
                                                                                      rateConfigurationKey: 252,
                                                                                      employmentType: EmploymentTypeEnum.Salaried,
                                                                                      purchasePrice: 500000f);
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerkey, WorkflowStates.ApplicationCaptureWF.ApplicationCapture);
            base.Browser.ClickAction(WorkflowActivities.ApplicationCapture.UpdateLoanDetails);
            base.View.RecalcAndSave(false);

            base.Browser.ClickAction(WorkflowActivities.ApplicationCapture.UpdateLoanDetails);
            var products = from p in base.View.GetProductsFromDropDownList()
                           where !p.Contains("Variable")
                           select p;
            foreach (var product in products)
            {
                //change product
                base.Browser.ClickAction(WorkflowActivities.ApplicationCapture.UpdateLoanDetails);
                base.View.ReworkProduct(product);
                base.View.RecalcAndSave(false);
                Assert.True(base.Browser.Page<BasePage>().CheckForerrorMessages("Only New Variable Loan products qualify for Alpha Housing."), "Expected rule to stop user from changing alpha housing product.");
            }
        }

        [Test]
        public void when_capitalising_initiation_fees_and_ltv_exceeds_100_percent_should_throw_error()
        {
            int offerKey = base.Service<IApplicationService>().GetAlphaOffersAtAppCapeWith100PercentLTV().FirstOrDefault().OfferKey;
            base.Browser = new TestBrowser(TestUsers.BranchConsultant, TestUsers.Password);
            base.Browser.Navigate<NavigationHelper>().CloseLoanNodesFLOBO(Browser);
            base.Browser.Navigate<NavigationHelper>().Task();
            base.Browser.Page<WorkflowSuperSearch>().Search(offerKey);
            base.Browser.ClickAction(WorkflowActivities.ApplicationCapture.UpdateLoanDetails);
            base.View.CapitaliseInitiationFees(true);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("LTV can not be greater than 100% when the initiation fee is capitalised.");
        }
    }
}