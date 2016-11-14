using System.Collections.Generic;
using SAHL.Tools.DecisionTree.TestRunner.Interfaces;
using SAHL.DecisionTree.TreeTests.Models;

namespace SAHL.DecisionTree.TreeTests
{
    public class SAHLOriginationCreditPricing_1TestSuite : ITestSuite
    {
        public IEnumerable<ITestCase> TestCases { get; set; }
    
        public SAHLOriginationCreditPricing_1TestSuite()
        {
            this.TestCases = new List<ITestCase> 
            {
                new TestCase("when the applicants fail the credit policy assessment", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried"),
                        new TestInput ("Household Income", "float", "30000"),
                        new TestInput ("Property Purchase Price", "float", "0"),
                        new TestInput ("Deposit Amount", "float", "0"),
                        new TestInput ("Cash Amount Required", "float", "100000"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "150000"),
                        new TestInput ("Estimated Market Value of Property", "float", "650000"),
                        new TestInput ("Eldest Applicant Age", "int", "35"),
                        new TestInput ("Youngest Applicant Age", "int", "34"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("First Income Contributor Applicant Empirica", "int", "550"),
                        new TestInput ("First Income Contributor Applicant Income", "float", "22500"),
                        new TestInput ("Second Income Contributor Applicant Empirica", "int", "610"),
                        new TestInput ("Second Income Contributor Applicant Income", "float", "7500"),
                        new TestInput ("Eligible Borrower", "bool", "True"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "True")
                    },
                    new List<IScenario> {
                        new Scenario("given an invalid empirica decision is reached",
                            new List<ITestInput> { 
                                new TestInput("First Income Contributor Applicant Income", "float", "22500"),
                                new TestInput("Second Income Contributor Applicant Income", "float", "7500")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Empirica", "to equal", "bool", "False"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an invalid borrower age decision is reached",
                            new List<ITestInput> { 
                                new TestInput("Youngest Applicant Age", "int", "17")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Borrower Age", "to equal", "bool", "True"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory0")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when using the tree to determine affordability", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried"),
                        new TestInput ("Household Income", "float", "20000"),
                        new TestInput ("Property Purchase Price", "float", "0"),
                        new TestInput ("Deposit Amount", "float", "0"),
                        new TestInput ("Cash Amount Required", "float", "250000"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "250000"),
                        new TestInput ("Estimated Market Value of Property", "float", "1000000"),
                        new TestInput ("Eldest Applicant Age", "int", "35"),
                        new TestInput ("Youngest Applicant Age", "int", "34"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("First Income Contributor Applicant Empirica", "int", "400"),
                        new TestInput ("First Income Contributor Applicant Income", "float", "17500"),
                        new TestInput ("Second Income Contributor Applicant Empirica", "int", "650"),
                        new TestInput ("Second Income Contributor Applicant Income", "float", "2500"),
                        new TestInput ("Eligible Borrower", "bool", "True"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "True")
                    },
                    new List<IScenario> {
                        new Scenario("given an application without applicant empirica or applicant ages or applicant income",
                            new List<ITestInput> { 
                                new TestInput("Eldest Applicant Age", "int", "-1"),
                                new TestInput("Youngest Applicant Age", "int", "-1"),
                                new TestInput("First Income Contributor Applicant Empirica", "int", "-999"),
                                new TestInput("Second Income Contributor Applicant Empirica", "int", "-999"),
                                new TestInput("First Income Contributor Applicant Income", "float", "-1.0"),
                                new TestInput("Second Income Contributor Applicant Income", "float", "-1.0"),
                                new TestInput("Household Income", "float", "40000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when checking the overall LTV limits for an investment property application", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.InvestmentProperty"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried"),
                        new TestInput ("Household Income", "float", "50000"),
                        new TestInput ("Property Purchase Price", "float", "375000"),
                        new TestInput ("Deposit Amount", "float", "75000"),
                        new TestInput ("Cash Amount Required", "float", "0"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "0"),
                        new TestInput ("Estimated Market Value of Property", "float", "0"),
                        new TestInput ("Eldest Applicant Age", "int", "35"),
                        new TestInput ("Youngest Applicant Age", "int", "34"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("First Income Contributor Applicant Empirica", "int", "650"),
                        new TestInput ("First Income Contributor Applicant Income", "float", "20000"),
                        new TestInput ("Second Income Contributor Applicant Empirica", "int", "650"),
                        new TestInput ("Second Income Contributor Applicant Income", "float", "20000"),
                        new TestInput ("Eligible Borrower", "bool", "True"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "True")
                    },
                    new List<IScenario> {
                        new Scenario("given an investment property with an LTV of 80%",
                            new List<ITestInput> { 

                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Loan to Value", "to equal", "float", "0.8")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an investment property with an LTV of 79.9%",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "375649.33"),
                                new TestInput("Deposit Amount", "float", "75459.33")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Loan to Value", "to equal", "float", "0.799")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase investment property with an LTV of 80.1%",
                            new List<ITestInput> { 
                                new TestInput("Deposit Amount", "float", "74531.83"),
                                new TestInput("Property Purchase Price", "float", "374531.83")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Loan to Value", "to equal", "float", "0.801")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your requested loan amount as a percentage of property value (LTV) for an investment property would be 80.10 % which equals or exceeds the maximum of 80%. In order to reduce your LTV, the loan amount would need to be lowered by increasing the deposit, by R376 or more.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a switch investment property with an LTV of 80.1%",
                            new List<ITestInput> { 
                                new TestInput("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch"),
                                new TestInput("Current Mortgage Loan Balance", "float", "701000"),
                                new TestInput("Cash Amount Required", "float", "100000"),
                                new TestInput("Estimated Market Value of Property", "float", "1000000")
                            },
                            new List<ITestOutput> {

                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your requested loan amount as a percentage of property value (LTV) for an investment property would be 80.10 % which equals or exceeds the maximum of 80%. In order to reduce your LTV, the loan amount would need to be lowered by decreasing the cash required by R1001 or more.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when an application fits into both alpha housing and SAHL credit pricing criteria", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried"),
                        new TestInput ("Household Income", "float", "13500"),
                        new TestInput ("Property Purchase Price", "float", "300000"),
                        new TestInput ("Deposit Amount", "float", "95000"),
                        new TestInput ("Cash Amount Required", "float", "0"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "0"),
                        new TestInput ("Estimated Market Value of Property", "float", "0"),
                        new TestInput ("Eldest Applicant Age", "int", "35"),
                        new TestInput ("Youngest Applicant Age", "int", "34"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("First Income Contributor Applicant Empirica", "int", "610"),
                        new TestInput ("First Income Contributor Applicant Income", "float", "10000"),
                        new TestInput ("Second Income Contributor Applicant Empirica", "int", "610"),
                        new TestInput ("Second Income Contributor Applicant Income", "float", "3500"),
                        new TestInput ("Eligible Borrower", "bool", "True"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "True")
                    },
                    new List<IScenario> {
                        new Scenario("given a salaried application with an LTV less than 70% and an income of R 13,500",
                            new List<ITestInput> { 

                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory0"),
 
                                new TestOutput("Alpha", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {
                                new SubtreeExpectation("Capitec Alpha Credit Pricing", "should not have been called")
                            }),
                        new Scenario("given a salary deduction application with an LTV less than 70% and an income of R 13,500",
                            new List<ITestInput> { 
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory0"),
 
                                new TestOutput("Alpha", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {
                                new SubtreeExpectation("Capitec Alpha Credit Pricing", "should not have been called")
                            }),
                        new Scenario("given a salaried application with an LTV between 70% and 80% with an income of R 18,000",
                            new List<ITestInput> { 
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried"),
                                new TestInput("Household Income", "float", "18000"),
                                new TestInput("Deposit Amount", "float", "65000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory1"),
 
                                new TestOutput("Alpha", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {
                                new SubtreeExpectation("Capitec Alpha Credit Pricing", "should not have been called")
                            }),
                        new Scenario("given a salary deduction application with an LTV between 70% and 80% with an income of R 18,000",
                            new List<ITestInput> { 
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction"),
                                new TestInput("Household Income", "float", "18000"),
                                new TestInput("Deposit Amount", "float", "65000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory1"),
 
                                new TestOutput("Alpha", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {
                                new SubtreeExpectation("Capitec Alpha Credit Pricing", "should not have been called")
                            })
                    }),
                new TestCase("when an application qualifies for a pricing for risk adjustment", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried"),
                        new TestInput ("Household Income", "float", "15000"),
                        new TestInput ("Property Purchase Price", "float", "500000"),
                        new TestInput ("Deposit Amount", "float", "200000"),
                        new TestInput ("Cash Amount Required", "float", "0"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "0"),
                        new TestInput ("Estimated Market Value of Property", "float", "0"),
                        new TestInput ("Eldest Applicant Age", "int", "35"),
                        new TestInput ("Youngest Applicant Age", "int", "25"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("First Income Contributor Applicant Empirica", "int", "580"),
                        new TestInput ("First Income Contributor Applicant Income", "float", "7500"),
                        new TestInput ("Second Income Contributor Applicant Empirica", "int", "580"),
                        new TestInput ("Second Income Contributor Applicant Income", "float", "7500"),
                        new TestInput ("Eligible Borrower", "bool", "True"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "True")
                    },
                    new List<IScenario> {
                        new Scenario("given a salaried application with a max empirica of 585 in category 0",
                            new List<ITestInput> { 

                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory0"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.028"),
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.004"),
 
                                new TestOutput("Interest Rate", "to equal", "float", "0.093")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when a salaried application does not qualify for pricing", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried"),
                        new TestInput ("Household Income", "float", "15000"),
                        new TestInput ("Property Purchase Price", "float", "0"),
                        new TestInput ("Deposit Amount", "float", "0"),
                        new TestInput ("Cash Amount Required", "float", "150000"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "150000"),
                        new TestInput ("Estimated Market Value of Property", "float", "310000"),
                        new TestInput ("Eldest Applicant Age", "int", "35"),
                        new TestInput ("Youngest Applicant Age", "int", "34"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("First Income Contributor Applicant Empirica", "int", "650"),
                        new TestInput ("First Income Contributor Applicant Income", "float", "8000"),
                        new TestInput ("Second Income Contributor Applicant Empirica", "int", "650"),
                        new TestInput ("Second Income Contributor Applicant Income", "float", "7000"),
                        new TestInput ("Eligible Borrower", "bool", "True"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "False")
                    },
                    new List<IScenario> {
                        new Scenario("given the household income falls outside the alpha housing limits",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "20700")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {
                                new SubtreeExpectation("Capitec Alpha Credit Pricing", "should not have been called")
                            }),
                        new Scenario("given the household income falls within the alpha housing limits",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "18000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory9")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {
                                new SubtreeExpectation("SAHL Alpha Credit Pricing", "should have been called")
                            })
                    }),
                new TestCase("when determining which pricing trees should be called for an application", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried"),
                        new TestInput ("Household Income", "float", "35000"),
                        new TestInput ("Property Purchase Price", "float", "650000"),
                        new TestInput ("Deposit Amount", "float", "150000"),
                        new TestInput ("Cash Amount Required", "float", "0"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "0"),
                        new TestInput ("Estimated Market Value of Property", "float", "0"),
                        new TestInput ("Eldest Applicant Age", "int", "35"),
                        new TestInput ("Youngest Applicant Age", "int", "34"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("First Income Contributor Applicant Empirica", "int", "650"),
                        new TestInput ("First Income Contributor Applicant Income", "float", "17500"),
                        new TestInput ("Second Income Contributor Applicant Empirica", "int", "650"),
                        new TestInput ("Second Income Contributor Applicant Income", "float", "17500"),
                        new TestInput ("Eligible Borrower", "bool", "True"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "False")
                    },
                    new List<IScenario> {
                        new Scenario("given an unemployed application",
                            new List<ITestInput> { 
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Unemployed")
                            },
                            new List<ITestOutput> {

                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {
                                new SubtreeExpectation("Capitec Self-Employed Credit Pricing", "should not have been called"),
                                new SubtreeExpectation("Capitec Alpha Credit Pricing", "should not have been called"),
                                new SubtreeExpectation("Capitec Salaried Credit Pricing", "should not have been called"),
                                new SubtreeExpectation("Capitec Salaried with Deduction Credit Pricing", "should not have been called")
                            }),
                        new Scenario("given a salaried application",
                            new List<ITestInput> { 
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried")
                            },
                            new List<ITestOutput> {

                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {
                                new SubtreeExpectation("SAHL Salaried Credit Pricing", "should have been called"),
                                new SubtreeExpectation("SAHL Self-Employed Credit Pricing", "should not have been called"),
                                new SubtreeExpectation("SAHL Salaried with Deduction Credit Pricing", "should not have been called"),
                                new SubtreeExpectation("SAHL Origination Pricing for Risk", "should have been called")
                            }),
                        new Scenario("given a self employed application",
                            new List<ITestInput> { 
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SelfEmployed")
                            },
                            new List<ITestOutput> {

                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {
                                new SubtreeExpectation("SAHL Self-Employed Credit Pricing", "should have been called"),
                                new SubtreeExpectation("SAHL Salaried Credit Pricing", "should not have been called"),
                                new SubtreeExpectation("SAHL Salaried with Deduction Credit Pricing", "should not have been called"),
                                new SubtreeExpectation("SAHL Origination Pricing for Risk", "should not have been called")
                            }),
                        new Scenario("given a salary deduction application",
                            new List<ITestInput> { 
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction")
                            },
                            new List<ITestOutput> {

                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {
                                new SubtreeExpectation("SAHL Salaried with Deduction Credit Pricing", "should have been called"),
                                new SubtreeExpectation("SAHL Salaried Credit Pricing", "should not have been called"),
                                new SubtreeExpectation("SAHL Self-Employed Credit Pricing", "should not have been called"),
                                new SubtreeExpectation("SAHL Origination Pricing for Risk", "should have been called")
                            })
                    }),
                new TestCase("when a salary with deduction application does not qualify for pricing", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction"),
                        new TestInput ("Household Income", "float", "15000"),
                        new TestInput ("Property Purchase Price", "float", "0"),
                        new TestInput ("Deposit Amount", "float", "0"),
                        new TestInput ("Cash Amount Required", "float", "150000"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "155000"),
                        new TestInput ("Estimated Market Value of Property", "float", "300000"),
                        new TestInput ("Eldest Applicant Age", "int", "35"),
                        new TestInput ("Youngest Applicant Age", "int", "34"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("First Income Contributor Applicant Empirica", "int", "650"),
                        new TestInput ("First Income Contributor Applicant Income", "float", "7500"),
                        new TestInput ("Second Income Contributor Applicant Empirica", "int", "650"),
                        new TestInput ("Second Income Contributor Applicant Income", "float", "7500"),
                        new TestInput ("Eligible Borrower", "bool", "True"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "True")
                    },
                    new List<IScenario> {
                        new Scenario("given the household income falls within the alpha housing limits",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "18599")
                            },
                            new List<ITestOutput> {

                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {
                                new SubtreeExpectation("SAHL Alpha Credit Pricing", "should have been called")
                            }),
                        new Scenario("given the household income falls outside the alpha housing limits",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "20700")
                            },
                            new List<ITestOutput> {

                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {
                                new SubtreeExpectation("SAHL Alpha Credit Pricing", "should not have been called")
                            })
                    }),
                new TestCase("when there are no eligible borrowers on an application", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried"),
                        new TestInput ("Household Income", "float", "35000"),
                        new TestInput ("Property Purchase Price", "float", "600000"),
                        new TestInput ("Deposit Amount", "float", "150000"),
                        new TestInput ("Cash Amount Required", "float", "0"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "0"),
                        new TestInput ("Estimated Market Value of Property", "float", "0"),
                        new TestInput ("Eldest Applicant Age", "int", "35"),
                        new TestInput ("Youngest Applicant Age", "int", "34"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("First Income Contributor Applicant Empirica", "int", "650"),
                        new TestInput ("First Income Contributor Applicant Income", "float", "17500"),
                        new TestInput ("Second Income Contributor Applicant Empirica", "int", "650"),
                        new TestInput ("Second Income Contributor Applicant Income", "float", "17500"),
                        new TestInput ("Eligible Borrower", "bool", "False"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "True")
                    },
                    new List<IScenario> {
                        new Scenario("given qualifying application details the output variables should still be set",
                            new List<ITestInput> { 

                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory1"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.032"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.118"),
 
                                new TestOutput("Loan to Value", "to equal", "float", "0.75")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when validation messages are returned from the salaried and alpha pricing trees", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried"),
                        new TestInput ("Household Income", "float", "15000"),
                        new TestInput ("Property Purchase Price", "float", "500000"),
                        new TestInput ("Deposit Amount", "float", "0"),
                        new TestInput ("Cash Amount Required", "float", "0"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "0"),
                        new TestInput ("Estimated Market Value of Property", "float", "0"),
                        new TestInput ("Eldest Applicant Age", "int", "35"),
                        new TestInput ("Youngest Applicant Age", "int", "34"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("First Income Contributor Applicant Empirica", "int", "650"),
                        new TestInput ("First Income Contributor Applicant Income", "float", "7500"),
                        new TestInput ("Second Income Contributor Applicant Empirica", "int", "650"),
                        new TestInput ("Second Income Contributor Applicant Income", "float", "7500"),
                        new TestInput ("Eligible Borrower", "bool", "True"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "True")
                    },
                    new List<IScenario> {
                        new Scenario("given a salaried application that does not qualify for salaried or alpha pricing",
                            new List<ITestInput> { 
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your loan amount as a percentage of purchase price (LTV) would be 100.00 %, which is greater than or equal to the maximum of 100.0%. In order to reduce your LTV, the loan amount would need to be lowered by increasing the deposit by R1 or more.", "should contain", "Warning Messages"),
                                new OutputMessage("Your loan amount to purchase price (LTV) would be 100.00 %, which is greater than the maximum of 95.0%. In order to reduce your LTV, the loan amount would need to be lowered by increasing the deposit by R25001 or more.", "should not contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {
                                new SubtreeExpectation("SAHL Alpha Credit Pricing", "should have been called"),
                                new SubtreeExpectation("SAHL Salaried Credit Pricing", "should have been called")
                            })
                    }),
                new TestCase("when calculating the loan amount for an application", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried"),
                        new TestInput ("Household Income", "float", "30000"),
                        new TestInput ("Property Purchase Price", "float", "0"),
                        new TestInput ("Deposit Amount", "float", "0"),
                        new TestInput ("Cash Amount Required", "float", "150000"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "150000"),
                        new TestInput ("Estimated Market Value of Property", "float", "600000"),
                        new TestInput ("Eldest Applicant Age", "int", "35"),
                        new TestInput ("Youngest Applicant Age", "int", "34"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("First Income Contributor Applicant Empirica", "int", "650"),
                        new TestInput ("First Income Contributor Applicant Income", "float", "15000"),
                        new TestInput ("Second Income Contributor Applicant Empirica", "int", "650"),
                        new TestInput ("Second Income Contributor Applicant Income", "float", "15000"),
                        new TestInput ("Eligible Borrower", "bool", "True"),
                        new TestInput ("Fees", "float", "5000"),
                        new TestInput ("Interim Interest", "float", "2500"),
                        new TestInput ("Capitalise Fees", "bool", "True")
                    },
                    new List<IScenario> {
                        new Scenario("given a switch application with capitalise fees set to true",
                            new List<ITestInput> { 

                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Loan Amount", "to equal", "float", "307500")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a switch application with capitalise fees set to false",
                            new List<ITestInput> { 
                                new TestInput("Capitalise Fees", "bool", "False")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Loan Amount", "to equal", "float", "300000")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with capitalise fees set true",
                            new List<ITestInput> { 
                                new TestInput("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase"),
                                new TestInput("Property Purchase Price", "float", "500000"),
                                new TestInput("Deposit Amount", "float", "250000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Loan Amount", "to equal", "float", "250000")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with capitalise fees set false",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "500000"),
                                new TestInput("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase"),
                                new TestInput("Deposit Amount", "float", "250000")
                            },
                            new List<ITestOutput> {

                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when the ITC service is not available but valid applicant ages are provided", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SelfEmployed"),
                        new TestInput ("Household Income", "float", "65000"),
                        new TestInput ("Property Purchase Price", "float", "985985"),
                        new TestInput ("Deposit Amount", "float", "500000"),
                        new TestInput ("Cash Amount Required", "float", "0"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "0"),
                        new TestInput ("Estimated Market Value of Property", "float", "0"),
                        new TestInput ("Eldest Applicant Age", "int", "68"),
                        new TestInput ("Youngest Applicant Age", "int", "68"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("First Income Contributor Applicant Empirica", "int", "-999"),
                        new TestInput ("First Income Contributor Applicant Income", "float", "-1"),
                        new TestInput ("Second Income Contributor Applicant Empirica", "int", "-999"),
                        new TestInput ("Second Income Contributor Applicant Income", "float", "-1"),
                        new TestInput ("Eligible Borrower", "bool", "True"),
                        new TestInput ("Fees", "float", "13851.01"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "True")
                    },
                    new List<IScenario> {
                        new Scenario("given an applicant older than 65",
                            new List<ITestInput> { 
                                new TestInput("Eldest Applicant Age", "int", "68")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Eligible Borrower Age", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {
                                new SubtreeExpectation("Capitec Application Credit Policy", "should not have been called")
                            }),
                        new Scenario("given an applicant with a valid age",
                            new List<ITestInput> { 
                                new TestInput("Eldest Applicant Age", "int", "35"),
                                new TestInput("Youngest Applicant Age", "int", "35")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Eligible Borrower Age", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {
                                new SubtreeExpectation("Capitec Application Credit Policy", "should not have been called")
                            })
                    }),
                new TestCase("when only a single applicant empirica score is available", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried"),
                        new TestInput ("Household Income", "float", "50000"),
                        new TestInput ("Property Purchase Price", "float", "0"),
                        new TestInput ("Deposit Amount", "float", "0"),
                        new TestInput ("Cash Amount Required", "float", "150000"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "150000"),
                        new TestInput ("Estimated Market Value of Property", "float", "1000000"),
                        new TestInput ("Eldest Applicant Age", "int", "35"),
                        new TestInput ("Youngest Applicant Age", "int", "34"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("First Income Contributor Applicant Empirica", "int", "585"),
                        new TestInput ("First Income Contributor Applicant Income", "float", "35000"),
                        new TestInput ("Second Income Contributor Applicant Empirica", "int", "-999"),
                        new TestInput ("Second Income Contributor Applicant Income", "float", "15000"),
                        new TestInput ("Eligible Borrower", "bool", "True"),
                        new TestInput ("Fees", "float", "5000"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "True")
                    },
                    new List<IScenario> {
                        new Scenario("given the second applicant has no empirica score",
                            new List<ITestInput> { 
                                new TestInput("Second Income Contributor Applicant Empirica", "int", "-999")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Application Empirica", "to equal", "int", "585"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given the first applicant has no empirica score",
                            new List<ITestInput> { 
                                new TestInput("First Income Contributor Applicant Empirica", "int", "-999"),
                                new TestInput("Second Income Contributor Applicant Empirica", "int", "585")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Application Empirica", "to equal", "int", "585"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    })
            };
        }
    }
}


 