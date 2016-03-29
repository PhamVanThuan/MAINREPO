using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using ObjectMaps.Pages;
using System;
using WatiN.Core.Logging;

namespace BuildingBlocks
{
    public static partial class Views
    {
        public class LoanCalculator : LoanCalculatorLeadControls
        {
            public void LoanCalculatorLead_Switch(string product, string marketValueOfYourHome, string currentLoanAmount,
                string cashOut, string employmentType, string term, bool CapitaliseFees,
                string grossMonthlyHousholdIncome, ButtonTypeEnum ButtonPress)
            {
                //Select the first option in the Marketing Source dropdown.
                base.ddlMarketingSource.Options[1].Select();
                base.selectLoanPurpose.Option("Switch loan").Select();
                WatiN.Core.Logging.Logger.LogAction("Selecting 'Switch loan' Option from 'Loan Purpose' SelectList");
                base.selectProduct.Option(product).Select();
                WatiN.Core.Logging.Logger.LogAction("Selecting '{0}' Option from 'Product' SelectList", product);
                base.textfieldMarketValueOfYourHome.Value = (marketValueOfYourHome);
                base.textfieldCurrentLoanAmount.Value = (currentLoanAmount);
                base.textfieldCashOut.Value = (cashOut);
                base.selectEmploymentType.Option(employmentType).Select();
                if ((product == "New Variable Loan" || product == "VariFix Loan") && !string.IsNullOrEmpty(term)) base.textfieldTermOfLoan.TypeText(term);
                if (!base.checkboxCapitaliseFees.Checked && CapitaliseFees) base.checkboxCapitaliseFees.Click();
                else if (base.checkboxCapitaliseFees.Checked && !CapitaliseFees) base.checkboxCapitaliseFees.Click();
                base.textfieldGrossMonthlyHousholdIncome.Value = (grossMonthlyHousholdIncome);

                switch (ButtonPress)
                {
                    case ButtonTypeEnum.CreateApplication:
                        base.btnCalculate.Click();
                        base.btnCreateApplication.Click();
                        break;

                    case ButtonTypeEnum.Calculate:
                        base.btnCalculate.Click();
                        break;

                    case ButtonTypeEnum.Cancel:
                        base.btnCancel.Click();
                        break;
                }
            }

            public void LoanCalculatorLead_NewPurchase(string product, string purchasePrice, string cashDeposit, string employmentType, string term,
                string grossMonthlyHousholdIncome, ButtonTypeEnum ButtonPress)
            {
                //Select the first option in the Marketing Source dropdown.  This is is a quick fix to get tests running with this new functionality that was introduced in v2.20.0.x
                base.ddlMarketingSource.Options[1].Select();
                base.selectLoanPurpose.Option("New purchase").Select();
                WatiN.Core.Logging.Logger.LogAction("Selecting 'New purchase' Option from 'Loan Purpose' SelectList");
                base.selectProduct.Option(product).Select();
                WatiN.Core.Logging.Logger.LogAction("Selecting '{0}' Option from 'Product' SelectList", product);
                base.textfieldPurchasePrice.Value = (purchasePrice);
                base.textfieldCashDeposit.Value = (cashDeposit);
                base.selectEmploymentType.Select(employmentType);
                WatiN.Core.Logging.Logger.LogAction("Selecting '{0}' Option from 'Employment Type' SelectList", employmentType);
                if ((product == "New Variable Loan" || product == "VariFix Loan") && !string.IsNullOrEmpty(term)) base.textfieldTermOfLoan.TypeText(term);
                base.textfieldGrossMonthlyHousholdIncome.Value = (grossMonthlyHousholdIncome);

                switch (ButtonPress)
                {
                    case ButtonTypeEnum.CreateApplication:
                        base.btnCalculate.Click();
                        base.btnCreateApplication.Click();
                        break;

                    case ButtonTypeEnum.Calculate:
                        base.btnCalculate.Click();
                        break;

                    case ButtonTypeEnum.Cancel:
                        base.btnCancel.Click();
                        break;
                }
            }

            public void LoanCalculatorLead_Refinance(string product, string marketValueOfYourHome, string cashRequired,
                string employmentType, string term, bool CapitaliseFees, string grossMonthlyHousholdIncome,
                ButtonTypeEnum ButtonPress)
            {
                //Select the first option in the Marketing Source dropdown.
                base.ddlMarketingSource.Options[1].Select();
                base.selectLoanPurpose.Option("Refinance").Select();
                WatiN.Core.Logging.Logger.LogAction("Selecting 'Refinance' Option from 'Loan Purpose' SelectList");
                base.selectProduct.Option(product).Select();
                WatiN.Core.Logging.Logger.LogAction("Selecting '{0}' Option from 'Product' SelectList", product);
                base.textfieldMarketValueOfYourHome.Value = (marketValueOfYourHome);
                base.textfieldCashRequired.Value = (cashRequired);
                base.selectEmploymentType.Option(employmentType).Select();
                if ((product == "New Variable Loan" || product == "VariFix Loan") && !string.IsNullOrEmpty(term)) base.textfieldTermOfLoan.TypeText(term);
                if (!base.checkboxCapitaliseFees.Checked && CapitaliseFees) base.checkboxCapitaliseFees.Click();
                else if (base.checkboxCapitaliseFees.Checked && !CapitaliseFees) base.checkboxCapitaliseFees.Click();
                base.textfieldGrossMonthlyHousholdIncome.Value = (grossMonthlyHousholdIncome);

                switch (ButtonPress)
                {
                    case ButtonTypeEnum.CreateApplication:
                        base.btnCalculate.Click();
                        base.btnCreateApplication.Click();
                        break;

                    case ButtonTypeEnum.Calculate:
                        base.btnCalculate.Click();
                        break;

                    case ButtonTypeEnum.Cancel:
                        base.btnCancel.Click();
                        break;
                }
            }

            public void ApplicationWizardCalculator_Switch(string product, string marketValueOfYourHome, string currentLoanAmount,
                string cashOut, string employmentType, string term, bool CapitaliseFees,
                string grossMonthlyHousholdIncome, string needsIdentification, ButtonTypeEnum ButtonPress)
            {
                base.selectLoanPurpose.Option("Switch loan").Select();
                WatiN.Core.Logging.Logger.LogAction("Selecting 'Switch loan' Option from 'Loan Purpose' SelectList");
                base.ddlNeedsIdentification.Option(needsIdentification).Select();
                WatiN.Core.Logging.Logger.LogAction("Selecting {0} Option from 'Needs Identification' SelectList", needsIdentification);
                base.selectProduct.Option(product).Select();
                WatiN.Core.Logging.Logger.LogAction("Selecting '{0}' Option from 'Product' SelectList", product);
                base.textfieldMarketValueOfYourHome.Value = (marketValueOfYourHome);
                base.textfieldCurrentLoanAmount.Value = (currentLoanAmount);
                base.textfieldCashOut.Value = (cashOut);
                base.selectEmploymentType.Option(employmentType).Select();
                if ((product == "New Variable Loan" || product == "VariFix Loan") && !string.IsNullOrEmpty(term)) base.textfieldTermOfLoan.TypeText(term);
                if (!base.checkboxCapitaliseFees.Checked && CapitaliseFees) base.checkboxCapitaliseFees.Click();
                else if (base.checkboxCapitaliseFees.Checked && !CapitaliseFees) base.checkboxCapitaliseFees.Click();
                base.textfieldGrossMonthlyHousholdIncome.Value = (grossMonthlyHousholdIncome);

                switch (ButtonPress)
                {
                    case ButtonTypeEnum.CreateApplication:
                        base.btnCalculate.Click();
                        base.btnCreateApplication.Click();
                        break;

                    case ButtonTypeEnum.Calculate:
                        base.btnCalculate.Click();
                        break;

                    case ButtonTypeEnum.Cancel:
                        base.btnCancel.Click();
                        break;
                }
            }

            public void ApplicationWizardCalculator_NewPurchase(string product, string purchasePrice, string cashDeposit,
                string employmentType, string term, string grossMonthlyHousholdIncome, string needsIdentification,
                ButtonTypeEnum ButtonPress)
            {
                base.selectLoanPurpose.Option("New purchase").Select();
                WatiN.Core.Logging.Logger.LogAction("Selecting 'New purchase' Option from 'Loan Purpose' SelectList");
                base.ddlNeedsIdentification.Option(needsIdentification).Select();
                WatiN.Core.Logging.Logger.LogAction("Selecting {0} Option from 'Needs Identification' SelectList", needsIdentification);
                base.selectProduct.Option(product).Select();
                WatiN.Core.Logging.Logger.LogAction("Selecting '{0}' Option from 'Product' SelectList", product);
                base.textfieldPurchasePrice.Value = (purchasePrice);
                base.textfieldCashDeposit.Value = (cashDeposit);
                base.selectEmploymentType.Select(employmentType);
                WatiN.Core.Logging.Logger.LogAction("Selecting '{0}' Option from 'Employment Type' SelectList", employmentType);
                if ((product == "New Variable Loan" || product == "VariFix Loan") && !string.IsNullOrEmpty(term)) base.textfieldTermOfLoan.TypeText(term);
                base.textfieldGrossMonthlyHousholdIncome.Value = (grossMonthlyHousholdIncome);

                switch (ButtonPress)
                {
                    case ButtonTypeEnum.CreateApplication:
                        base.btnCalculate.Click();
                        base.btnCreateApplication.Click();
                        break;

                    case ButtonTypeEnum.Calculate:
                        base.btnCalculate.Click();
                        break;

                    case ButtonTypeEnum.Cancel:
                        base.btnCancel.Click();
                        break;
                }
            }

            public void ApplicationWizardCalculator_Refinance(string product, string marketValueOfYourHome, string cashRequired,
                string employmentType, string term, bool CapitaliseFees, string grossMonthlyHousholdIncome,
                string needsIdentification, ButtonTypeEnum ButtonPress)
            {
                base.selectLoanPurpose.Option("Refinance").Select();
                WatiN.Core.Logging.Logger.LogAction("Selecting 'Refinance' Option from 'Loan Purpose' SelectList");
                base.ddlNeedsIdentification.Option(needsIdentification).Select();
                WatiN.Core.Logging.Logger.LogAction("Selecting {0} Option from 'Needs Identification' SelectList", needsIdentification);
                base.selectProduct.Option(product).Select();
                WatiN.Core.Logging.Logger.LogAction("Selecting '{0}' Option from 'Product' SelectList", product);
                base.textfieldMarketValueOfYourHome.Value = (marketValueOfYourHome);
                base.textfieldCashRequired.Value = (cashRequired);
                base.selectEmploymentType.Option(employmentType).Select();
                if ((product == "New Variable Loan" || product == "VariFix Loan") && !string.IsNullOrEmpty(term)) base.textfieldTermOfLoan.TypeText(term);
                if (!base.checkboxCapitaliseFees.Checked && CapitaliseFees) base.checkboxCapitaliseFees.Click();
                else if (base.checkboxCapitaliseFees.Checked && !CapitaliseFees) base.checkboxCapitaliseFees.Click();
                base.textfieldGrossMonthlyHousholdIncome.Value = (grossMonthlyHousholdIncome);

                switch (ButtonPress)
                {
                    case ButtonTypeEnum.CreateApplication:
                        base.btnCalculate.Click();
                        base.btnCreateApplication.Click();
                        break;

                    case ButtonTypeEnum.Calculate:
                        base.btnCalculate.Click();
                        break;

                    case ButtonTypeEnum.Cancel:
                        base.btnCancel.Click();
                        break;
                }
            }

            /// <summary>
            /// Selects the Loan Purepose from the dropdown on the Application Calculator screen
            /// </summary>
            /// <param name="b">IE TestBrowser</param>
            /// <param name="loanPurpose">Loan Purpose</param>
            public void SelectLoanPurpose(string loanPurpose)
            {
                //select a loan purpose
                base.selectLoanPurpose.Option(loanPurpose).Select();
            }

            /// <summary>
            /// Selects the Product from the dropdown on the Application Calculator screen
            /// </summary>
            /// <param name="b">IE TestBrowser</param>
            /// <param name="product">Product</param>
            public void SelectProduct(string product)
            {
                //select a product
                base.selectProduct.Option(product).Select();
            }

            /// <summary>
            /// Retrieve the value from the Term text field off of the Application Calculator screen
            /// </summary>
            /// <param name="b">IE TestBrowser</param>
            /// <returns>Term Value</returns>
            public int CurrentTerm()
            {
                int Term;
                string sTerm = base.textfieldTermOfLoan.Value;
                Term = Convert.ToInt32(sTerm);
                return Term;
            }

            public void SetEstateAgentApplication()
            {
                base.chkEstateAgent.Checked = true;
            }

            public void LoanCalculatorLead_Switch(string loanPurpose, string marketValueOfYourHome, string currentLoanAmount,
                string cashOut, string employmentType, string term, bool CapitaliseFees,
                string grossMonthlyHousholdIncome, ButtonTypeEnum ButtonPress, bool isEstateAgent)
            {
                //Select the first option in the Marketing Source dropdown.
                base.ddlMarketingSource.Options[1].Select();
                base.selectLoanPurpose.Option("Switch loan").Select();
                WatiN.Core.Logging.Logger.LogAction("Selecting 'Switch loan' Option from 'Loan Purpose' SelectList");
                base.selectProduct.Option(loanPurpose).Select();
                WatiN.Core.Logging.Logger.LogAction("Selecting '{0}' Option from 'Product' SelectList", loanPurpose);
                base.textfieldMarketValueOfYourHome.Value = (marketValueOfYourHome);
                base.textfieldCurrentLoanAmount.Value = (currentLoanAmount);
                base.textfieldCashOut.Value = (cashOut);
                base.selectEmploymentType.Option(employmentType).Select();
                if ((loanPurpose == "New Variable Loan" || loanPurpose == "VariFix Loan") && !string.IsNullOrEmpty(term)) base.textfieldTermOfLoan.TypeText(term);
                if (!base.checkboxCapitaliseFees.Checked && CapitaliseFees) base.checkboxCapitaliseFees.Click();
                else if (base.checkboxCapitaliseFees.Checked && !CapitaliseFees) base.checkboxCapitaliseFees.Click();
                base.textfieldGrossMonthlyHousholdIncome.Value = (grossMonthlyHousholdIncome);

                if (isEstateAgent)
                {
                    SetEstateAgentApplication();
                }

                switch (ButtonPress)
                {
                    case ButtonTypeEnum.CreateApplication:
                        base.btnCalculate.Click();
                        base.btnCreateApplication.Click();
                        break;

                    case ButtonTypeEnum.Calculate:
                        base.btnCalculate.Click();
                        break;

                    case ButtonTypeEnum.Cancel:
                        base.btnCancel.Click();
                        break;
                }
            }

            public void LoanCalculatorLead_NewPurchase(string loanPurpose, string purchasePrice, string cashDeposit, string employmentType,
                string term, string grossMonthlyHousholdIncome, ButtonTypeEnum buttonPress, bool isEstateAgent)
            {
                //Select the first option in the Marketing Source dropdown.
                base.ddlMarketingSource.Options[1].Select();
                base.selectLoanPurpose.Option("New purchase").Select();
                WatiN.Core.Logging.Logger.LogAction("Selecting 'New purchase' Option from 'Loan Purpose' SelectList");
                base.selectProduct.Option(loanPurpose).Select();
                WatiN.Core.Logging.Logger.LogAction("Selecting '{0}' Option from 'Product' SelectList", loanPurpose);
                base.textfieldPurchasePrice.Value = (purchasePrice);
                base.textfieldCashDeposit.Value = (cashDeposit);
                base.selectEmploymentType.Select(employmentType);
                WatiN.Core.Logging.Logger.LogAction("Selecting '{0}' Option from 'Employment Type' SelectList", employmentType);
                if ((loanPurpose == "New Variable Loan" || loanPurpose == "VariFix Loan") && !string.IsNullOrEmpty(term)) base.textfieldTermOfLoan.TypeText(term);
                base.textfieldGrossMonthlyHousholdIncome.Value = (grossMonthlyHousholdIncome);

                if (isEstateAgent)
                {
                    SetEstateAgentApplication();
                }

                switch (buttonPress)
                {
                    case ButtonTypeEnum.CreateApplication:
                        base.btnCalculate.Click();
                        base.btnCreateApplication.Click();
                        break;

                    case ButtonTypeEnum.Calculate:
                        base.btnCalculate.Click();
                        break;

                    case ButtonTypeEnum.Cancel:
                        base.btnCancel.Click();
                        break;
                }
            }

            public void LoanCalculatorLead_Refinance(string loanPurpose, string marketValueOfYourHome, string cashRequired, string employmentType,
                string term, bool capitaliseFees, string grossMonthlyHousholdIncome, ButtonTypeEnum buttonPress, bool isEstateAgent)
            {
                //Select the first option in the Marketing Source dropdown.
                base.ddlMarketingSource.Options[1].Select();
                base.selectLoanPurpose.Option("Refinance").Select();
                WatiN.Core.Logging.Logger.LogAction("Selecting 'Refinance' Option from 'Loan Purpose' SelectList");
                base.selectProduct.Option(loanPurpose).Select();
                WatiN.Core.Logging.Logger.LogAction("Selecting '{0}' Option from 'Product' SelectList", loanPurpose);
                base.textfieldMarketValueOfYourHome.Value = (marketValueOfYourHome);
                base.textfieldCashRequired.Value = (cashRequired);
                base.selectEmploymentType.Option(employmentType).Select();
                if ((loanPurpose == "New Variable Loan" || loanPurpose == "VariFix Loan") && !string.IsNullOrEmpty(term)) base.textfieldTermOfLoan.TypeText(term);
                if (!base.checkboxCapitaliseFees.Checked && capitaliseFees) base.checkboxCapitaliseFees.Click();
                else if (base.checkboxCapitaliseFees.Checked && !capitaliseFees) base.checkboxCapitaliseFees.Click();
                base.textfieldGrossMonthlyHousholdIncome.Value = (grossMonthlyHousholdIncome);

                if (isEstateAgent)
                {
                    SetEstateAgentApplication();
                }

                switch (buttonPress)
                {
                    case ButtonTypeEnum.CreateApplication:
                        base.btnCalculate.Click();
                        base.btnCreateApplication.Click();
                        break;

                    case ButtonTypeEnum.Calculate:
                        base.btnCalculate.Click();
                        break;

                    case ButtonTypeEnum.Cancel:
                        base.btnCancel.Click();
                        break;
                }
            }

            /// <summary>
            /// Selects a Needs Indentification from the dropdown on the Application Wizard Calculator screen
            /// </summary>
            /// <param name="b">IE TestBrowser</param>
            /// <param name="needsIdentification">Needs Indentification</param>
            public void SelectNeedsIndentification(string needsIdentification)
            {
                //select a needs identification
                base.ddlNeedsIdentification.Option(needsIdentification).Select();
            }

            public void Populate
                (
                    bool isCapitaliseFees = false,
                    bool isInterestOnlyFees = false,
                    bool isEstateAgent = false,
                    ProductEnum product = ProductEnum.NewVariableLoan,
                    EmploymentTypeEnum employmentType = EmploymentTypeEnum.Salaried,
                    MortgageLoanPurposeEnum purpose = MortgageLoanPurposeEnum.Newpurchase,
                    int term = 240,
                    float marketValue = 800000,
                    float currentloanAmount = 250000,
                    float cashOut = 200000,
                    float purchasePrice = 800000,
                    float cashDeposit = 150000,
                    float cashRequired = 150000,
                    float percentageToFix = 50,
                    float income = 60000
                )
            {
                base.checkboxCapitaliseFees.Checked = isCapitaliseFees;
                base.chkEstateAgent.Checked = isEstateAgent;
                base.textfieldMarketValueOfYourHome.Value = Int32.Parse(marketValue.ToString()).ToString();
                base.textfieldGrossMonthlyHousholdIncome.Value = Int32.Parse(income.ToString()).ToString();
                base.ddlMarketingSource.Options[1].Select();

                //Select LoanPurpose

                #region LoanPurpose

                switch (purpose)
                {
                    case MortgageLoanPurposeEnum.Newpurchase:
                        {
                            base.selectLoanPurpose.Option(LoanPurposeType.NewPurchase).Select();
                            base.textfieldPurchasePrice.Value = Int32.Parse(purchasePrice.ToString()).ToString();
                            base.textfieldCashDeposit.Value = Int32.Parse(cashDeposit.ToString()).ToString();
                            break;
                        }
                    case MortgageLoanPurposeEnum.Refinance:
                        {
                            base.selectLoanPurpose.Option(LoanPurposeType.Refinance).Select();
                            base.textfieldCashRequired.Value = Int32.Parse(cashRequired.ToString()).ToString();
                            break;
                        }
                    case MortgageLoanPurposeEnum.Switchloan:
                        {
                            base.selectLoanPurpose.Option(LoanPurposeType.SwitchLoan).Select();
                            base.textfieldCurrentLoanAmount.Value = Int32.Parse(currentloanAmount.ToString()).ToString();
                            base.textfieldCashOut.Value = Int32.Parse(cashOut.ToString()).ToString();
                            break;
                        }
                }

                #endregion LoanPurpose

                //Select Employment

                #region EmploymentType

                switch (employmentType)
                {
                    case EmploymentTypeEnum.Salaried:
                        {
                            base.selectEmploymentType.Option(EmploymentType.Salaried).Select();
                            break;
                        }
                    case EmploymentTypeEnum.SelfEmployed:
                        {
                            base.selectEmploymentType.Option(EmploymentType.SelfEmployed).Select();
                            break;
                        }
                    case EmploymentTypeEnum.SalariedWithDeductions:
                        {
                            base.selectEmploymentType.Option(EmploymentType.SalariedWithDeductions).Select();
                            break;
                        }
                    case EmploymentTypeEnum.Unemployed:
                        {
                            base.selectEmploymentType.Option(EmploymentType.Unemployed).Select();
                            break;
                        }
                }

                #endregion EmploymentType

                //Select Product

                #region Product

                switch (product)
                {
                    case ProductEnum.VariFixLoan:
                        {
                            base.selectProduct.Option(Common.Constants.Products.VariFixLoan).Select();
                            base.textfieldTermOfLoan.Value = term.ToString();                            
                            break;
                        }
                    case ProductEnum.NewVariableLoan:
                        {
                            base.textfieldTermOfLoan.Value = term.ToString();
                            base.selectProduct.Option(Products.NewVariableLoan).Select();
                            break;
                        }
                    case ProductEnum.Edge:
                        {
                            base.selectProduct.Option(Products.Edge).Select();
                            break;
                        }
                }

                #endregion Product
            }

            public void ClickCreateApplication()
            {
                base.btnCreateApplication.Click();
            }

            public void ClickCalculate()
            {
                base.btnCalculate.Click();
            }

            /// <summary>
            /// Checks that the ExpectedTerm value is equal to the actual value in the Term text field on the Application Loan Calculator screen
            /// </summary>
            /// <param name="b">IE TestBrowser</param>
            /// <param name="ExpectedTerm">New Expected Term</param>
            public void AssertTermValue(int ExpectedTerm)
            {
                int ActualTerm = CurrentTerm();
                Logger.LogAction(String.Format("Asserting that the term value is the expected value"));
                Assert.AreEqual(ExpectedTerm, ActualTerm, "Term value is not the expected value of:" + ExpectedTerm);
            }
        }
    }
}