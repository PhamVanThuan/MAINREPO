using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation.InternetComponents;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.InternetComponents.Calculators;
using BuildingBlocks.Services.Contracts;
using BuildingBlocks.Websites;
using Common;
using Common.Enums;
using NUnit.Framework;

namespace InternetComponentTests
{
    [RequiresSTA]
    public sealed class CalculatorTests : TestBase<BasePage>
    {
        [Test]
        public void LeadCalculatorFieldValidationTests()
        {
            var browser = new TestBrowser(true);
            try
            {
                browser.BypassSSLCertificateWarning();
                browser.Navigate<WebsiteCalculators>().ClickAffordabilityCalculator();

                //random employment from 2am
                var clientEmp = Service<ILegalEntityService>().GetRandomLegalEntityEmploymentRecord
                                            (
                                                Common.Enums.EmploymentStatusEnum.Current,
                                                Common.Enums.RemunerationTypeEnum.Salaried,
                                                Common.Enums.EmploymentTypeEnum.Salaried
                                            );
                //Random OfferInformationVariableLoanRecord
                var offerdetail = base.Service<IApplicationService>().GetRandomLatestOfferInformationMortgageLoanRecord(MortgageLoanPurposeEnum.Newpurchase, ProductEnum.NewVariableLoan, OfferStatusEnum.Open);

                //Assign to local var to set back offerdetail and clientEmp later in the test
                float origMonthlyInstallment = offerdetail.MonthlyInstalment;
                double origInterestRate = offerdetail.InterestRate;
                int origTermYear = offerdetail.TermYears;
                float origMonthlyIncome = clientEmp.MonthlyIncome;

                //change some values
                offerdetail.TermYears = 0;
                offerdetail.MonthlyInstalment = 0;
                offerdetail.InterestRate = 0.0;
                clientEmp.MonthlyIncome = 0;
                clientEmp.Income01 = 0;
                clientEmp.Income02 = 0;

                //Capture employment and loan detail
                browser.Page<AffordabilityCalculator>().PopulateCalculator(clientEmp, offerdetail);
                browser.Page<AffordabilityCalculator>().Calculate();

                //Test that valdiation messages show.
                float maxInstallment = base.Service<ICalculationsService>().CalculateAffordabilityInstallment(clientEmp.MonthlyIncome);
                WebsiteCalculatorAssertions.AssertCalculatorZeroAmountFieldsContainsValidationMessages(MortgageLoanPurposeEnum.Unknown, ProductEnum.NewVariableLoan, affordabilityMaxInstalmentAmount: maxInstallment);

                //change values to blank and do assertion
                this.ClearCalculatorValues(browser, MortgageLoanPurposeEnum.Unknown);
                browser.Page<AffordabilityCalculator>().Calculate();
                WebsiteCalculatorAssertions.AssertCalculatorBlankFieldsContainsValidationMessages(MortgageLoanPurposeEnum.Unknown, ProductEnum.NewVariableLoan);

                //change values back
                offerdetail.TermYears = origTermYear;
                offerdetail.InterestRate = origInterestRate;
                clientEmp.MonthlyIncome = origMonthlyIncome;
                offerdetail.MonthlyInstalment = origMonthlyInstallment;
                maxInstallment = base.Service<ICalculationsService>().CalculateAffordabilityInstallment(clientEmp.MonthlyIncome);
                if (offerdetail.MonthlyInstalment >= maxInstallment)
                    offerdetail.MonthlyInstalment = maxInstallment;

                clientEmp.Income01 = (clientEmp.MonthlyIncome / 2);
                clientEmp.Income02 = (clientEmp.MonthlyIncome / 2);

                //Capture employment and loan detail
                browser.Page<AffordabilityCalculator>().PopulateCalculator(clientEmp, offerdetail);
                browser.Page<AffordabilityCalculator>().Calculate();

                //Test that validation messages do not show.
                WebsiteCalculatorAssertions.AssertCalculatorNotContainsValidationMessages(browser, offerdetail.LoanPurpose);
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

        [Test]
        public void ApplicationCalculatorNewPurchaseNewVariableValidationTests
            (
                [Values(EmploymentTypeEnum.Salaried, EmploymentTypeEnum.SelfEmployed)]
                EmploymentTypeEnum employmentType
            )
        {
            var browser = new TestBrowser(true);
            try
            {
                this.TestApplicationCalculatorValidations(browser, MortgageLoanPurposeEnum.Newpurchase, employmentType, ProductEnum.NewVariableLoan);
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

        [Test]
        public void ApplicationCalculatorNewPurchaseVariFixValidationTests
            (
                [Values(EmploymentTypeEnum.Salaried, EmploymentTypeEnum.SelfEmployed)]
                EmploymentTypeEnum employmentType
            )
        {
            var browser = new TestBrowser(true);
            try
            {
                this.TestApplicationCalculatorValidations(browser, MortgageLoanPurposeEnum.Newpurchase, employmentType, ProductEnum.VariFixLoan);
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

        [Test]
        public void ApplicationCalculatorSwitchNewVariableValidationTests
            (
                [Values(EmploymentTypeEnum.Salaried, EmploymentTypeEnum.SelfEmployed)]
                EmploymentTypeEnum employmentType
            )
        {
            var browser = new TestBrowser(true);
            try
            {
                this.TestApplicationCalculatorValidations(browser, MortgageLoanPurposeEnum.Switchloan, employmentType, ProductEnum.NewVariableLoan);
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

        [Test]
        public void ApplicationCalculatorSwitchVariFixValidationTests
            (
                [Values(EmploymentTypeEnum.Salaried, EmploymentTypeEnum.SelfEmployed)]
                EmploymentTypeEnum employmentType
            )
        {
            var browser = new TestBrowser(true);
            try
            {
                this.TestApplicationCalculatorValidations(browser, MortgageLoanPurposeEnum.Switchloan, employmentType, ProductEnum.VariFixLoan);
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

        #region Helpers

        public void TestApplicationCalculatorValidations(TestBrowser browser, MortgageLoanPurposeEnum loanPurpose, EmploymentTypeEnum employmentType, ProductEnum product)
        {
            browser.BypassSSLCertificateWarning();
            browser.GoTo(string.Format("{0}/Calculators.aspx", GlobalConfiguration.TestWebsite));

            //random employment from 2am
            var clientEmp = Service<ILegalEntityService>().GetRandomLegalEntityEmploymentRecord
                                        (
                                            Common.Enums.EmploymentStatusEnum.Current,
                                            Common.Enums.RemunerationTypeEnum.Salaried,
                                            employType: employmentType
                                        );
            if (clientEmp.MonthlyIncome <= 5000.00f)
                clientEmp.MonthlyIncome = 6000.00f;

            //random offer, offerinformation and offerinformationloan detail from 2am
            var offerdetail = base.Service<IApplicationService>().GetRandomLatestOfferInformationMortgageLoanRecord(loanPurpose, product, OfferStatusEnum.Open);
            offerdetail.IsCapitaliseFees = true;
            offerdetail.FixedPercentage = 50.0;

            //Assign to local var to set back offerdetail and clientEmp later in the test
            float origPropertyClientEstimate = offerdetail.ClientEstimatePropertyValuation;
            float origPurchasePrice = offerdetail.PurchasePrice;
            float origCashOut = offerdetail.CashOut;
            float origCashDeposit = offerdetail.CashDeposit;
            int origTermYear = offerdetail.TermYears;
            float origExistingLoan = offerdetail.ExistingLoan;
            float origMonthlyIncome = clientEmp.MonthlyIncome;
            double origInterestRate = offerdetail.InterestRate;
            double origFixedPerc = offerdetail.FixedPercentage;

            //change values to zero and do assertion
            offerdetail.ClientEstimatePropertyValuation = 0;
            offerdetail.PurchasePrice = 0;
            offerdetail.CashOut = 0;
            offerdetail.CashDeposit = 0;
            offerdetail.FixedPercentage = 0;
            offerdetail.TermYears = 0;
            offerdetail.ExistingLoan = 0;
            offerdetail.InterestRate = 0.0;
            clientEmp.MonthlyIncome = 0;
            this.CaptureCalculatorValues(browser, loanPurpose, clientEmp, offerdetail);
            browser.Page<BaseCalculator>().Calculate();
            WebsiteCalculatorAssertions.AssertCalculatorZeroAmountFieldsContainsValidationMessages(loanPurpose, product);

            //change values to blank and do assertion
            this.ClearCalculatorValues(browser, loanPurpose);
            browser.Page<BaseCalculator>().Calculate();
            WebsiteCalculatorAssertions.AssertCalculatorBlankFieldsContainsValidationMessages(loanPurpose, product);

            //set values back to orignal
            offerdetail.ClientEstimatePropertyValuation = origPropertyClientEstimate;
            offerdetail.PurchasePrice = origPurchasePrice;
            offerdetail.ExistingLoan = origExistingLoan;
            offerdetail.CashOut = origCashOut;
            offerdetail.TermYears = origTermYear;
            offerdetail.CashDeposit = origCashDeposit;
            offerdetail.InterestRate = origInterestRate;
            offerdetail.FixedPercentage = origFixedPerc;
            clientEmp.MonthlyIncome = origMonthlyIncome;

            //the pruchase price not to be the same as the cach deposit.
            if (offerdetail.CashDeposit >= offerdetail.PurchasePrice)
                offerdetail.CashDeposit = (offerdetail.CashDeposit / 2);

            switch (loanPurpose)
            {
                case MortgageLoanPurposeEnum.Newpurchase:
                    //Capture a new purchase loan
                    browser.Navigate<WebsiteCalculators>().ClickNewPurchaseCalculator();
                    browser.Page<NewPurchaseCalculator>().PopulateCalculator(clientEmp, offerdetail);
                    break;

                case MortgageLoanPurposeEnum.Switchloan:
                    //Capture a switch loan
                    browser.Navigate<WebsiteCalculators>().ClickSwitchCalculator();
                    browser.Page<SwitchCalculator>().PopulateCalculator(clientEmp, offerdetail);
                    break;
            }
            browser.Page<BaseCalculator>().Calculate();

            //Test that there are no validation messages
            WebsiteCalculatorAssertions.AssertCalculatorNotContainsValidationMessages(browser, loanPurpose);
        }

        private void CaptureCalculatorValues(TestBrowser browser, MortgageLoanPurposeEnum loanPurpose, Automation.DataModels.LegalEntityEmployment clientEmp, Automation.DataModels.Offer offerDetail)
        {
            switch (loanPurpose)
            {
                case MortgageLoanPurposeEnum.Newpurchase:
                    //Capture a new purchase loan
                    browser.Navigate<WebsiteCalculators>().ClickNewPurchaseCalculator();
                    browser.Page<NewPurchaseCalculator>().PopulateCalculator(clientEmp, offerDetail);
                    break;

                case MortgageLoanPurposeEnum.Switchloan:
                    //Capture a switch loan
                    browser.Navigate<WebsiteCalculators>().ClickSwitchCalculator();
                    browser.Page<SwitchCalculator>().PopulateCalculator(clientEmp, offerDetail);
                    break;
            }
        }

        private void ClearCalculatorValues(TestBrowser browser, MortgageLoanPurposeEnum loanPurpose)
        {
            switch (loanPurpose)
            {
                case MortgageLoanPurposeEnum.Newpurchase:
                    browser.Navigate<WebsiteCalculators>().ClickNewPurchaseCalculator();
                    break;

                case MortgageLoanPurposeEnum.Switchloan:
                    browser.Navigate<WebsiteCalculators>().ClickSwitchCalculator();
                    break;

                case MortgageLoanPurposeEnum.Unknown:
                    browser.Navigate<WebsiteCalculators>().ClickAffordabilityCalculator();
                    break;
            }
        }

        #endregion Helpers
    }
}