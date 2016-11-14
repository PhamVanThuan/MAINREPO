using System.Collections.Generic;
using SAHL.Tools.DecisionTree.TestRunner.Interfaces;
using SAHL.DecisionTree.TreeTests.Models;

namespace SAHL.DecisionTree.TreeTests
{
    public class CapitecSelfEmployedCreditPricing_1TestSuite : ITestSuite
    {
        public IEnumerable<ITestCase> TestCases { get; set; }
    
        public CapitecSelfEmployedCreditPricing_1TestSuite()
        {
            this.TestCases = new List<ITestCase> 
            {
                new TestCase("when an application does not meet the minimum self employed requirements", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SelfEmployed"),
                        new TestInput ("Household Income", "float", "12999"),
                        new TestInput ("Property Purchase Price", "float", "1000000"),
                        new TestInput ("Deposit Amount", "float", "0"),
                        new TestInput ("Cash Amount Required", "float", "0"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "0"),
                        new TestInput ("Estimated Market Value of Property", "float", "0"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "620"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "False")
                    },
                    new List<IScenario> {
                        new Scenario("given an application with a household income of R 13,000 that would fall into category 0",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "13000"),
                                new TestInput("Property Purchase Price", "float", "300000"),
                                new TestInput("Deposit Amount", "float", "100000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Total income is below the required minimum for self-employed applicants.", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with a household income of R 12,999.99 that would fall into category 0",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "12999.99"),
                                new TestInput("Property Purchase Price", "float", "300000"),
                                new TestInput("Deposit Amount", "float", "100000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Total income is below the required minimum for self-employed applicants.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with a household income of R 13,000.01 that would fall into category 0",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "13000.01"),
                                new TestInput("Property Purchase Price", "float", "300000"),
                                new TestInput("Deposit Amount", "float", "100000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Total income is below the required minimum for self-employed applicants.", "should not contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with a loan value of R 150,000",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "150000")
                            },
                            new List<ITestOutput> {

                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Loan amount is below the required minimum", "should not contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with a loan value of R 149,999",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "149999")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Loan amount requested is below the product minimum.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with a loan amount of R 150,001",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "150001")
                            },
                            new List<ITestOutput> {

                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Loan amount is below the required minimum", "should not contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan value of R 3,499,999",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "3499999")
                            },
                            new List<ITestOutput> {

                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Loan amount is above the maximum allowed", "should not contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 3,500,000",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "3500000")
                            },
                            new List<ITestOutput> {

                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Loan amount is above the maximum allowed", "should not contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan value of R 3,500,001",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "3500001")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Loan amount requested is above the product maximum.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with a household income of R 13,000 that would fall into category 1",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "13000"),
                                new TestInput("Property Purchase Price", "float", "300000"),
                                new TestInput("Deposit Amount", "float", "75000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with a household income of R 12,999.99 that would fall into category 1",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "12999.99"),
                                new TestInput("Property Purchase Price", "float", "300000"),
                                new TestInput("Deposit Amount", "float", "75000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Total income is below the required minimum for self-employed applicants.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with a household income of R 13,000.01 that would fall into category 1",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "13000.01"),
                                new TestInput("Property Purchase Price", "float", "300000"),
                                new TestInput("Deposit Amount", "float", "75000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an new purchase application with LTV ratio of 0.801",
                            new List<ITestInput> { 
                                new TestInput("Deposit Amount", "float", "199000"),
                                new TestInput("Household Income", "float", "50000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Loan to Value", "to equal", "float", "0.801"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your loan amount as a percentage of purchase price (LTV) would be 80.10 %, which is greater than or equal to the maximum of 80.0%. In order to reduce your LTV, the loan amount would need to be lowered by increasing the deposit by R1001 or more.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a switch application with LTV ratio of 0.801",
                            new List<ITestInput> { 
                                new TestInput("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch"),
                                new TestInput("Estimated Market Value of Property", "float", "1000000"),
                                new TestInput("Current Mortgage Loan Balance", "float", "801000"),
                                new TestInput("Household Income", "float", "50000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Loan to Value", "to equal", "float", "0.801"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your loan amount as a percentage of estimated property value (LTV) would be 80.10 %, which is greater than or equal to the maximum of 80.0%. In order to reduce your LTV, the loan amount would need to be lowered by decreasing the cash required by R1001 or more.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when calculating the loan amount", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SelfEmployed"),
                        new TestInput ("Household Income", "float", "0"),
                        new TestInput ("Property Purchase Price", "float", "0"),
                        new TestInput ("Deposit Amount", "float", "0"),
                        new TestInput ("Cash Amount Required", "float", "0"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "0"),
                        new TestInput ("Estimated Market Value of Property", "float", "0"),
                        new TestInput ("Term In Month", "int", "0"),
                        new TestInput ("Application Empirica", "int", "0"),
                        new TestInput ("Fees", "float", "1111"),
                        new TestInput ("Interim Interest", "float", "2222"),
                        new TestInput ("Capitalise Fees", "bool", "False")
                    },
                    new List<IScenario> {
                        new Scenario("given a switch application and capitalise fees false",
                            new List<ITestInput> { 
                                new TestInput("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch"),
                                new TestInput("Current Mortgage Loan Balance", "float", "500000"),
                                new TestInput("Cash Amount Required", "float", "100000"),
                                new TestInput("Capitalise Fees", "bool", "False")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Loan Amount", "to equal", "float", "600000")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application and capitalise fees false",
                            new List<ITestInput> { 
                                new TestInput("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase"),
                                new TestInput("Property Purchase Price", "float", "1000000"),
                                new TestInput("Deposit Amount", "float", "100000"),
                                new TestInput("Capitalise Fees", "bool", "False")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Loan Amount", "to equal", "float", "900000")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a switch application with capitalise fees true",
                            new List<ITestInput> { 
                                new TestInput("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch"),
                                new TestInput("Current Mortgage Loan Balance", "float", "500000"),
                                new TestInput("Cash Amount Required", "float", "100000"),
                                new TestInput("Capitalise Fees", "bool", "True")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Loan Amount", "to equal", "float", "603333")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with capitalise fees true",
                            new List<ITestInput> { 
                                new TestInput("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase"),
                                new TestInput("Property Purchase Price", "float", "1000000"),
                                new TestInput("Deposit Amount", "float", "100000"),
                                new TestInput("Capitalise Fees", "bool", "True")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Loan Amount", "to equal", "float", "900000")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when calculating the LTV ratio", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried"),
                        new TestInput ("Household Income", "float", "0"),
                        new TestInput ("Property Purchase Price", "float", "0"),
                        new TestInput ("Deposit Amount", "float", "0"),
                        new TestInput ("Cash Amount Required", "float", "0"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "0"),
                        new TestInput ("Estimated Market Value of Property", "float", "0"),
                        new TestInput ("Term In Month", "int", "0"),
                        new TestInput ("Application Empirica", "int", "0"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "False")
                    },
                    new List<IScenario> {
                        new Scenario("given a switch application",
                            new List<ITestInput> { 
                                new TestInput("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch"),
                                new TestInput("Estimated Market Value of Property", "float", "1000000"),
                                new TestInput("Current Mortgage Loan Balance", "float", "666666"),
                                new TestInput("Cash Amount Required", "float", "100000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Loan to Value", "to equal", "float", "0.766"),
 
                                new TestOutput("Loan to Value as Percent", "to equal", "string", "76.60 %")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application",
                            new List<ITestInput> { 
                                new TestInput("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase"),
                                new TestInput("Property Purchase Price", "float", "1000000"),
                                new TestInput("Deposit Amount", "float", "333333")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Loan to Value", "to equal", "float", "0.666"),
 
                                new TestOutput("Loan to Value as Percent", "to equal", "string", "66.60 %")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a switch application and property value of R 0",
                            new List<ITestInput> { 
                                new TestInput("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch"),
                                new TestInput("Estimated Market Value of Property", "float", "0")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Loan to Value", "to equal", "float", "9999")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Property Value cannot be zero.", "should contain", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application and property value of R 0",
                            new List<ITestInput> { 
                                new TestInput("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase"),
                                new TestInput("Property Purchase Price", "float", "0")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Loan to Value", "to equal", "float", "9999")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Property Value cannot be zero.", "should contain", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when calculating property value", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried"),
                        new TestInput ("Household Income", "float", "0"),
                        new TestInput ("Property Purchase Price", "float", "0"),
                        new TestInput ("Deposit Amount", "float", "0"),
                        new TestInput ("Cash Amount Required", "float", "0"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "0"),
                        new TestInput ("Estimated Market Value of Property", "float", "0"),
                        new TestInput ("Term In Month", "int", "0"),
                        new TestInput ("Application Empirica", "int", "0"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "False")
                    },
                    new List<IScenario> {
                        new Scenario("given a switch application",
                            new List<ITestInput> { 
                                new TestInput("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch"),
                                new TestInput("Estimated Market Value of Property", "float", "1000000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Property Value", "to equal", "float", "1000000")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application",
                            new List<ITestInput> { 
                                new TestInput("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase"),
                                new TestInput("Property Purchase Price", "float", "500000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Property Value", "to equal", "float", "500000")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when a switch application falls into category 0", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SelfEmployed"),
                        new TestInput ("Household Income", "float", "64780.2688212124"),
                        new TestInput ("Property Purchase Price", "float", "0"),
                        new TestInput ("Deposit Amount", "float", "0"),
                        new TestInput ("Cash Amount Required", "float", "0"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "1799999.99"),
                        new TestInput ("Estimated Market Value of Property", "float", "2769230"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "620"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "False")
                    },
                    new List<IScenario> {
                        new Scenario("given an application with loan amount  of R 1,800,000 and PTI of 27.5 % because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Estimated Market Value of Property", "float", "2769230"),
                                new TestInput("Current Mortgage Loan Balance", "float", "1800000"),
                                new TestInput("Household Income", "float", "60135")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.275"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.032"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.5% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1799253 or alternatively additional income so that total income is at least R60161.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 1,800,000 and PTI of 27.51 5 because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Estimated Market Value of Property", "float", "2769230"),
                                new TestInput("Current Mortgage Loan Balance", "float", "1800000"),
                                new TestInput("Household Income", "float", "59919")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.276"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.6% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1792791 or alternatively additional income so that total income is at least R60161.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 1,800,000 and PTI of 27.49 % because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Estimated Market Value of Property", "float", "2769230"),
                                new TestInput("Current Mortgage Loan Balance", "float", "1800000"),
                                new TestInput("Household Income", "float", "60253")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.274"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 1,799,999,99 and PTI of 27.5 % because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Estimated Market Value of Property", "float", "2769230"),
                                new TestInput("Current Mortgage Loan Balance", "float", "1799999.99"),
                                new TestInput("Household Income", "float", "60035")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.275"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.5% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1796261 or alternatively additional income so that total income is at least R60161.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 1,799,999.99 and PTI of 27.51 % because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Estimated Market Value of Property", "float", "2769230"),
                                new TestInput("Current Mortgage Loan Balance", "float", "1799999.99"),
                                new TestInput("Household Income", "float", "59919")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.276"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.6% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1792791 or alternatively additional income so that total income is at least R60161.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 1,799,999,99 and PTI of 27.49 % because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Estimated Market Value of Property", "float", "2769230"),
                                new TestInput("Current Mortgage Loan Balance", "float", "1799999.99"),
                                new TestInput("Household Income", "float", "60253")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.274"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 1,800,000.01 and PTI of 27.5 % because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Estimated Market Value of Property", "float", "2769230"),
                                new TestInput("Current Mortgage Loan Balance", "float", "1800000.01"),
                                new TestInput("Household Income", "float", "60135")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.275"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.5% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1799253 or alternatively additional income so that total income is at least R60161.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 1,800,000.01 and PTI of 27.51 % because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Estimated Market Value of Property", "float", "2769230"),
                                new TestInput("Current Mortgage Loan Balance", "float", "1800000.01"),
                                new TestInput("Household Income", "float", "59919")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.276"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.6% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1792791 or alternatively additional income so that total income is at least R60161.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 1,800,000.01 and PTI of 27.49 % because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Estimated Market Value of Property", "float", "2769230"),
                                new TestInput("Current Mortgage Loan Balance", "float", "1800000.01"),
                                new TestInput("Household Income", "float", "60253")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.274"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 2,749,999.99 and PTI of 27.5 % because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Estimated Market Value of Property", "float", "4230769"),
                                new TestInput("Current Mortgage Loan Balance", "float", "2749999.99"),
                                new TestInput("Household Income", "float", "91862")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.275"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.5% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R2748533 or alternatively additional income so that total income is at least R91912.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 2,749,999.99 and PTI of 27.51 % because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Estimated Market Value of Property", "float", "4230769"),
                                new TestInput("Current Mortgage Loan Balance", "float", "2749999.99"),
                                new TestInput("Household Income", "float", "91532")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.276"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.6% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R2738660 or alternatively additional income so that total income is at least R91912.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 2,749,999.99 and PTI of 27.49 % because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Estimated Market Value of Property", "float", "4230769"),
                                new TestInput("Current Mortgage Loan Balance", "float", "2749999.99"),
                                new TestInput("Household Income", "float", "92195")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.274"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 2,750,000 and PTI of 27.5 % because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Estimated Market Value of Property", "float", "4230769"),
                                new TestInput("Current Mortgage Loan Balance", "float", "2750000"),
                                new TestInput("Household Income", "float", "91862")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.275"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.5% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R2748533 or alternatively additional income so that total income is at least R91912.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 2,750,000 and PTI of 27.51 % because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Estimated Market Value of Property", "float", "4230769"),
                                new TestInput("Current Mortgage Loan Balance", "float", "2750000"),
                                new TestInput("Household Income", "float", "91532")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.276"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.6% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R2738660 or alternatively additional income so that total income is at least R91912.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 2,750,000 and PTI of 27.49 % because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Estimated Market Value of Property", "float", "4230769"),
                                new TestInput("Current Mortgage Loan Balance", "float", "2750000"),
                                new TestInput("Household Income", "float", "92195")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.274"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 2,750,000.01 and PTI of 27.5 % because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Estimated Market Value of Property", "float", "4230769"),
                                new TestInput("Current Mortgage Loan Balance", "float", "2750000.01"),
                                new TestInput("Household Income", "float", "91862")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.275"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.5% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R2748533 or alternatively additional income so that total income is at least R91912.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 2,750,000.01 and PTI of 27.51 % because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Estimated Market Value of Property", "float", "4230769"),
                                new TestInput("Current Mortgage Loan Balance", "float", "2750000.01"),
                                new TestInput("Household Income", "float", "91532")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.276"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.6% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R2738660 or alternatively additional income so that total income is at least R91912.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 2,750,000.01 and PTI of 27.49 % because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Estimated Market Value of Property", "float", "4230769"),
                                new TestInput("Current Mortgage Loan Balance", "float", "2750000.01"),
                                new TestInput("Household Income", "float", "92195")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.274"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 3,499,999.99 and PTI of 27.5 % because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Estimated Market Value of Property", "float", "5384615"),
                                new TestInput("Current Mortgage Loan Balance", "float", "3499999.99"),
                                new TestInput("Household Income", "float", "116952")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.275"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.5% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R3499232 or alternatively additional income so that total income is at least R116979.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 3,499,999.99 and PTI of 27.51 % because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Estimated Market Value of Property", "float", "5384615"),
                                new TestInput("Current Mortgage Loan Balance", "float", "3499999.99"),
                                new TestInput("Household Income", "float", "116231")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.276"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.6% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R3477660 or alternatively additional income so that total income is at least R116979.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 3,499,999.99 and PTI of 27.49 % because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Estimated Market Value of Property", "float", "5384615"),
                                new TestInput("Current Mortgage Loan Balance", "float", "3499999.99"),
                                new TestInput("Household Income", "float", "117076")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.274"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 3,500,000 and PTI of 27.5 % because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Estimated Market Value of Property", "float", "5384615"),
                                new TestInput("Current Mortgage Loan Balance", "float", "3500000"),
                                new TestInput("Household Income", "float", "116652")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.275"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.5% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R3490256 or alternatively additional income so that total income is at least R116979.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 3,500,000 and PTI of 27.51 % because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Estimated Market Value of Property", "float", "5384615"),
                                new TestInput("Current Mortgage Loan Balance", "float", "3500000"),
                                new TestInput("Household Income", "float", "116231")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.276"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.6% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R3477660 or alternatively additional income so that total income is at least R116979.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 3,500,000 and PTI of 27.49 % because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Estimated Market Value of Property", "float", "5384615"),
                                new TestInput("Current Mortgage Loan Balance", "float", "3500000"),
                                new TestInput("Household Income", "float", "117076")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.274"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when a switch application falls into category 1", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SelfEmployed"),
                        new TestInput ("Household Income", "float", "116229"),
                        new TestInput ("Property Purchase Price", "float", "0"),
                        new TestInput ("Deposit Amount", "float", "0"),
                        new TestInput ("Cash Amount Required", "float", "0"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "2750000.01"),
                        new TestInput ("Estimated Market Value of Property", "float", "3666666"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "620"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "False")
                    },
                    new List<IScenario> {
                        new Scenario("given an application with loan amount of R 1,799,999.99 and PTI of 27.5 % because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Estimated Market Value of Property", "float", "2399999"),
                                new TestInput("Current Mortgage Loan Balance", "float", "1799999.99"),
                                new TestInput("Household Income", "float", "61840")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.275"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.5% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1799138 or alternatively additional income so that total income is at least R61871.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 1,799,999.99 and PTI of 27.51 % because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Estimated Market Value of Property", "float", "2399999"),
                                new TestInput("Current Mortgage Loan Balance", "float", "1799999.99"),
                                new TestInput("Household Income", "float", "61617")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.276"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.6% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1792650 or alternatively additional income so that total income is at least R61871.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 1,799,999.99 and PTI of 27.49 % because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Estimated Market Value of Property", "float", "2399999"),
                                new TestInput("Current Mortgage Loan Balance", "float", "1799999.99"),
                                new TestInput("Household Income", "float", "62064")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.274"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 1,800,000 and PTI of 27.5 % because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Estimated Market Value of Property", "float", "2399999"),
                                new TestInput("Current Mortgage Loan Balance", "float", "1800000"),
                                new TestInput("Household Income", "float", "61840")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.275"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.5% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1799138 or alternatively additional income so that total income is at least R61871.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 1,800,000 and PTI of 27.51 % because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Estimated Market Value of Property", "float", "2399999"),
                                new TestInput("Current Mortgage Loan Balance", "float", "1800000"),
                                new TestInput("Household Income", "float", "61617")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.276"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.6% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1792650 or alternatively additional income so that total income is at least R61871.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 1,800,000 and PTI of 27.49 % because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Estimated Market Value of Property", "float", "2399999"),
                                new TestInput("Current Mortgage Loan Balance", "float", "1800000"),
                                new TestInput("Household Income", "float", "62064")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.274"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 1,800,000.01 and PTI of 22 % because of 10 % variance on 20 % limit",
                            new List<ITestInput> { 
                                new TestInput("Estimated Market Value of Property", "float", "2399999"),
                                new TestInput("Current Mortgage Loan Balance", "float", "1800000.01"),
                                new TestInput("Household Income", "float", "77200")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.22"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 22.0% and is greater than or equal to the maximum of 22.0%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1796810 or alternatively additional income so that total income is at least R77338.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 1,800,000.01 and PTI of 22.01 % because of 10 % variance on 20 % limit",
                            new List<ITestInput> { 
                                new TestInput("Estimated Market Value of Property", "float", "2399999"),
                                new TestInput("Current Mortgage Loan Balance", "float", "1800000.01"),
                                new TestInput("Household Income", "float", "76852")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.221"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 22.1% and is greater than or equal to the maximum of 22.0%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1788711 or alternatively additional income so that total income is at least R77338.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 1,800,000.01 and PTI of 21.99 % because of 10 % variance on 20 % limit",
                            new List<ITestInput> { 
                                new TestInput("Estimated Market Value of Property", "float", "2399999"),
                                new TestInput("Current Mortgage Loan Balance", "float", "1800000.01"),
                                new TestInput("Household Income", "float", "77551")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.219"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 2,749,999.99 and PTI of 22 % because of 10 % variance on 20 % limit",
                            new List<ITestInput> { 
                                new TestInput("Estimated Market Value of Property", "float", "3666666"),
                                new TestInput("Current Mortgage Loan Balance", "float", "2749999.99"),
                                new TestInput("Household Income", "float", "117933")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.22"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 22.0% and is greater than or equal to the maximum of 22.0%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R2744861 or alternatively additional income so that total income is at least R118155.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 2,749,999.99 and PTI of 22.01 % because of 10 % variance on 20 % limit",
                            new List<ITestInput> { 
                                new TestInput("Estimated Market Value of Property", "float", "3666666"),
                                new TestInput("Current Mortgage Loan Balance", "float", "2749999.99"),
                                new TestInput("Household Income", "float", "117402")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.221"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 22.1% and is greater than or equal to the maximum of 22.0%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R2732502 or alternatively additional income so that total income is at least R118155.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 2,749,999.99 and PTI of 21.99 % because of 10 % variance on 20 % limit",
                            new List<ITestInput> { 
                                new TestInput("Estimated Market Value of Property", "float", "3666666"),
                                new TestInput("Current Mortgage Loan Balance", "float", "2749999.99"),
                                new TestInput("Household Income", "float", "118469")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.219"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 2,750,000 and PTI of 22 % because of 10 % variance on 20 % limit",
                            new List<ITestInput> { 
                                new TestInput("Estimated Market Value of Property", "float", "3666666"),
                                new TestInput("Current Mortgage Loan Balance", "float", "2750000"),
                                new TestInput("Household Income", "float", "117933")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.22"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 22.0% and is greater than or equal to the maximum of 22.0%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R2744861 or alternatively additional income so that total income is at least R118155.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 2,750,000 and PTI of 22.01 % because of 10 % variance on 20 % limit",
                            new List<ITestInput> { 
                                new TestInput("Estimated Market Value of Property", "float", "3666666"),
                                new TestInput("Current Mortgage Loan Balance", "float", "2750000"),
                                new TestInput("Household Income", "float", "117402")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.221"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 22.1% and is greater than or equal to the maximum of 22.0%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R2732502 or alternatively additional income so that total income is at least R118155.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 2,750,000 and PTI of 21.99 % because of 10 % variance on 20 % limit",
                            new List<ITestInput> { 
                                new TestInput("Estimated Market Value of Property", "float", "3666666"),
                                new TestInput("Current Mortgage Loan Balance", "float", "2750000"),
                                new TestInput("Household Income", "float", "118469")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.219"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 2,750,000.01 and PTI of less than 22 % because of 10 % variance on 20 % limit",
                            new List<ITestInput> { 
                                new TestInput("Estimated Market Value of Property", "float", "3666666"),
                                new TestInput("Current Mortgage Loan Balance", "float", "2750000.01"),
                                new TestInput("Household Income", "float", "118469")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.219"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Loan amount requested is above the product maximum.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when a new purchase application falls into category 0", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SelfEmployed"),
                        new TestInput ("Household Income", "float", "0"),
                        new TestInput ("Property Purchase Price", "float", "0"),
                        new TestInput ("Deposit Amount", "float", "0"),
                        new TestInput ("Cash Amount Required", "float", "0"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "0"),
                        new TestInput ("Estimated Market Value of Property", "float", "0"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "620"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "False")
                    },
                    new List<IScenario> {
                        new Scenario("given an application with loan amount of R 1,799,999.99 and PTI of 27.5 % because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "2769231"),
                                new TestInput("Deposit Amount", "float", "969231.01"),
                                new TestInput("Household Income", "float", "60035")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.275"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.5% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1796261 or alternatively additional income so that total income is at least R60161.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 1,799,999.99 and PTI of 27.51 % because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "2769231"),
                                new TestInput("Deposit Amount", "float", "969231.01"),
                                new TestInput("Household Income", "float", "59819")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.276"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.6% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1789799 or alternatively additional income so that total income is at least R60161.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 1,799,999.99 and PTI of 27,49 % because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "2769231"),
                                new TestInput("Deposit Amount", "float", "969231.01"),
                                new TestInput("Household Income", "float", "60253")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.274"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 1,800,000 and PTI of 27.5 % because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "2769231"),
                                new TestInput("Deposit Amount", "float", "969231"),
                                new TestInput("Household Income", "float", "60035")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.275"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.5% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1796261 or alternatively additional income so that total income is at least R60161.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 1,800,000 and PTI of 27.51 % because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "2769231"),
                                new TestInput("Deposit Amount", "float", "969231"),
                                new TestInput("Household Income", "float", "59819")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.276"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.6% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1789799 or alternatively additional income so that total income is at least R60161.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 1,800,000 and PTI of 27,49 % because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "2769231"),
                                new TestInput("Deposit Amount", "float", "969231"),
                                new TestInput("Household Income", "float", "60253")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.274"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 1,800,000.01 and PTI of 27.5 % because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "2769231"),
                                new TestInput("Deposit Amount", "float", "969230.99"),
                                new TestInput("Household Income", "float", "60035")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.275"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.5% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1796261 or alternatively additional income so that total income is at least R60161.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 1,800,000.01 and PTI of 27.51 % because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "2769231"),
                                new TestInput("Deposit Amount", "float", "969230.99"),
                                new TestInput("Household Income", "float", "59819")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.276"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.6% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1789799 or alternatively additional income so that total income is at least R60161.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 1,800,000.01 and PTI of 27,49 % because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "2769231"),
                                new TestInput("Deposit Amount", "float", "969230.99"),
                                new TestInput("Household Income", "float", "60253")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.274"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 2,749,999.99 and PTI of 27.5 % because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "4230769"),
                                new TestInput("Deposit Amount", "float", "1480769.01"),
                                new TestInput("Household Income", "float", "91662")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.275"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.5% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R2742549 or alternatively additional income so that total income is at least R91912.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 2,749,999.99 and PTI of 27.51 % because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "4230769"),
                                new TestInput("Deposit Amount", "float", "1480769.01"),
                                new TestInput("Household Income", "float", "91332")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.276"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.6% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R2732675 or alternatively additional income so that total income is at least R91912.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 2,749,999.99 and PTI of 27,49 % because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "4230769"),
                                new TestInput("Deposit Amount", "float", "1480769.01"),
                                new TestInput("Household Income", "float", "91995")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.274"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 2,750,000 and PTI of 27.5 % because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "4230769"),
                                new TestInput("Deposit Amount", "float", "1480769"),
                                new TestInput("Household Income", "float", "91662")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.275"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.5% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R2742549 or alternatively additional income so that total income is at least R91912.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 2,750,000 and PTI of 27.51 % because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "4230769"),
                                new TestInput("Deposit Amount", "float", "1480769"),
                                new TestInput("Household Income", "float", "91332")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.276"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.6% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R2732675 or alternatively additional income so that total income is at least R91912.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 2,750,000 and PTI of 27,49 % because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "4230769"),
                                new TestInput("Deposit Amount", "float", "1480769"),
                                new TestInput("Household Income", "float", "91995")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.274"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 2,750,000.01 and PTI of 27.5 % because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "4230769"),
                                new TestInput("Deposit Amount", "float", "1480768.99"),
                                new TestInput("Household Income", "float", "91662")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.275"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.5% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R2742549 or alternatively additional income so that total income is at least R91912.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 2,750,000.01 and PTI of 27.51 % because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "4230769"),
                                new TestInput("Deposit Amount", "float", "1480768.99"),
                                new TestInput("Household Income", "float", "91332")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.276"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.6% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R2732675 or alternatively additional income so that total income is at least R91912.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 2,750,000.01 and PTI of 27,49 % because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "4230769"),
                                new TestInput("Deposit Amount", "float", "1480768.99"),
                                new TestInput("Household Income", "float", "91995")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.274"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 3,499,999.99 and PTI of 27.5 % because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "5384615"),
                                new TestInput("Deposit Amount", "float", "1884615.01"),
                                new TestInput("Household Income", "float", "116752")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.275"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.5% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R3493248 or alternatively additional income so that total income is at least R116979.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 3,499,999.99 and PTI of 27.51 % because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "5384615"),
                                new TestInput("Deposit Amount", "float", "1884615.01"),
                                new TestInput("Household Income", "float", "116331")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.276"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.6% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R3480652 or alternatively additional income so that total income is at least R116979.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 3,499,999.99 and PTI of 27,49 % because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "5384615"),
                                new TestInput("Deposit Amount", "float", "1884615.01"),
                                new TestInput("Household Income", "float", "117176")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.274"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 3,500,000 and PTI of 27.5 % because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "5384615"),
                                new TestInput("Deposit Amount", "float", "1884615"),
                                new TestInput("Household Income", "float", "116752")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.275"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.5% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R3493248 or alternatively additional income so that total income is at least R116979.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 3,500,000 and PTI of 27.51 % because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "5384615"),
                                new TestInput("Deposit Amount", "float", "1884615"),
                                new TestInput("Household Income", "float", "116331")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.276"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.6% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R3480652 or alternatively additional income so that total income is at least R116979.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 3,500,000 and PTI of 27,49 % because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "5384615"),
                                new TestInput("Deposit Amount", "float", "1884615"),
                                new TestInput("Household Income", "float", "117176")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.274"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when a new purchase application falls into category 1", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SelfEmployed"),
                        new TestInput ("Household Income", "float", "60585.4323961206000000"),
                        new TestInput ("Property Purchase Price", "float", "2400000"),
                        new TestInput ("Deposit Amount", "float", "599999.99"),
                        new TestInput ("Cash Amount Required", "float", "599999.99"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "0"),
                        new TestInput ("Estimated Market Value of Property", "float", "0"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "620"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "False")
                    },
                    new List<IScenario> {
                        new Scenario("given an application with loan amount of R 1,799,999.99 and PTI of 27.5 % because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "2400000"),
                                new TestInput("Deposit Amount", "float", "600000.01"),
                                new TestInput("Household Income", "float", "61840")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.275"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.5% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1799138 or alternatively additional income so that total income is at least R61871.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 1,799,999.99 and PTI of 27.51 % because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "2400000"),
                                new TestInput("Deposit Amount", "float", "600000.01"),
                                new TestInput("Household Income", "float", "61617")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.276"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.6% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1792650 or alternatively additional income so that total income is at least R61871.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 1,799,999.99 and PTI of 27,49 % because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "2400000"),
                                new TestInput("Deposit Amount", "float", "600000.01"),
                                new TestInput("Household Income", "float", "62064")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.274"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 1,800,000 and PTI of 27.5 % because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "2400000"),
                                new TestInput("Deposit Amount", "float", "600000"),
                                new TestInput("Household Income", "float", "61840")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.275"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.5% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1799138 or alternatively additional income so that total income is at least R61871.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 1,800,000 and PTI of 27.51 % because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "2400000"),
                                new TestInput("Deposit Amount", "float", "600000"),
                                new TestInput("Household Income", "float", "61617")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.276"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.6% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1792650 or alternatively additional income so that total income is at least R61871.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 1,800,000 and PTI of 27,49 % because of 10 % variance on 25 % limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "2400000"),
                                new TestInput("Deposit Amount", "float", "600000"),
                                new TestInput("Household Income", "float", "62064")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.274"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 1,800,00.01 and PTI of 22 % because of 10 % variance on 20 % limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "2400000"),
                                new TestInput("Cash Amount Required", "float", "599999.99"),
                                new TestInput("Household Income", "float", "77200")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.22"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 22.0% and is greater than or equal to the maximum of 22.0%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1796810 or alternatively additional income so that total income is at least R77338.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 1,800,00.01 and PTI of 22.01 % because of 10 % variance on 20 % limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "2400000"),
                                new TestInput("Deposit Amount", "float", "599999.99"),
                                new TestInput("Household Income", "float", "76852")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.221"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 22.1% and is greater than or equal to the maximum of 22.0%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1788711 or alternatively additional income so that total income is at least R77338.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 1,800,00.01 and PTI of 21.99 % because of 10 % variance on 20 % limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "2400000"),
                                new TestInput("Deposit Amount", "float", "599999.99"),
                                new TestInput("Household Income", "float", "77551")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.219"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 2,749,999.99 and PTI of 22 % because of 10 % variance on 20 % limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "3666667"),
                                new TestInput("Deposit Amount", "float", "916667.01"),
                                new TestInput("Household Income", "float", "117933")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.22"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 22.0% and is greater than or equal to the maximum of 22.0%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R2744861 or alternatively additional income so that total income is at least R118155.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 2,749,999.99 and PTI of 22.01 % because of 10 % variance on 20 % limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "3666667"),
                                new TestInput("Deposit Amount", "float", "916667.01"),
                                new TestInput("Household Income", "float", "117202")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.221"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 22.1% and is greater than or equal to the maximum of 22.0%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R2727847 or alternatively additional income so that total income is at least R118155.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 2,749,999.99 and PTI of 21.99 % because of 10 % variance on 20 % limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "3666667"),
                                new TestInput("Deposit Amount", "float", "916667.01"),
                                new TestInput("Household Income", "float", "118469")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.219"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 2,750,000 and PTI of 22 % because of 10 % variance on 20 % limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "3666667"),
                                new TestInput("Deposit Amount", "float", "916667"),
                                new TestInput("Household Income", "float", "117933")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.22"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 22.0% and is greater than or equal to the maximum of 22.0%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R2744861 or alternatively additional income so that total income is at least R118155.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 2,750,000 and PTI of 22.01 % because of 10 % variance on 20 % limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "3666667"),
                                new TestInput("Deposit Amount", "float", "916667"),
                                new TestInput("Household Income", "float", "117402")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.221"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 22.1% and is greater than or equal to the maximum of 22.0%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R2732502 or alternatively additional income so that total income is at least R118155.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 2,750,000 and PTI of 21.99 % because of 10 % variance on 20 % limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "3666667"),
                                new TestInput("Deposit Amount", "float", "916667"),
                                new TestInput("Household Income", "float", "118469")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.219"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount of R 2,750,000.01 and PTI of less than 22 % because of 10 % variance on 20 % limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "3666667"),
                                new TestInput("Deposit Amount", "float", "916666.99"),
                                new TestInput("Household Income", "float", "118469")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.219"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Loan amount requested is above the product maximum.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when checking the minimum empirica requirement", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SelfEmployed"),
                        new TestInput ("Household Income", "float", "50000"),
                        new TestInput ("Property Purchase Price", "float", "0"),
                        new TestInput ("Deposit Amount", "float", "0"),
                        new TestInput ("Cash Amount Required", "float", "0"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "825000"),
                        new TestInput ("Estimated Market Value of Property", "float", "1000000"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "609"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "False")
                    },
                    new List<IScenario> {
                        new Scenario("given an application that falls into category 0 with empirica score of 609",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "609"),
                                new TestInput("Current Mortgage Loan Balance", "float", "650000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application that falls into category 0 with empirica score of 610",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "610"),
                                new TestInput("Current Mortgage Loan Balance", "float", "650000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SelfEmployedCategory0"),
 
                                new TestOutput("Alpha", "to equal", "bool", "False"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application that falls into category 0 with empirica score of 611",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "611"),
                                new TestInput("Current Mortgage Loan Balance", "float", "650000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SelfEmployedCategory0"),
 
                                new TestOutput("Alpha", "to equal", "bool", "False"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application that falls into category 1 with empirica score of 609",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "609"),
                                new TestInput("Current Mortgage Loan Balance", "float", "750000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application that falls into category 1 with empirica score of 610",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "610"),
                                new TestInput("Current Mortgage Loan Balance", "float", "750000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SelfEmployedCategory1"),
 
                                new TestOutput("Alpha", "to equal", "bool", "False"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application that falls into category 1 with empirica score of 611",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "611"),
                                new TestInput("Current Mortgage Loan Balance", "float", "750000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SelfEmployedCategory1"),
 
                                new TestOutput("Alpha", "to equal", "bool", "False"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application that falls into category 0 with empirica score of -999",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "-999"),
                                new TestInput("Current Mortgage Loan Balance", "float", "650000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SelfEmployedCategory0"),
 
                                new TestOutput("Alpha", "to equal", "bool", "False"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application that falls into category 1 with empirica score of -999",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "-999"),
                                new TestInput("Current Mortgage Loan Balance", "float", "750000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SelfEmployedCategory1"),
 
                                new TestOutput("Alpha", "to equal", "bool", "False"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when calculating the instalment value", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SelfEmployed"),
                        new TestInput ("Household Income", "float", "50000"),
                        new TestInput ("Property Purchase Price", "float", "0"),
                        new TestInput ("Deposit Amount", "float", "0"),
                        new TestInput ("Cash Amount Required", "float", "0"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "0"),
                        new TestInput ("Estimated Market Value of Property", "float", "1000000"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "620"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "False")
                    },
                    new List<IScenario> {
                        new Scenario("given an application that falls into category 0",
                            new List<ITestInput> { 
                                new TestInput("Current Mortgage Loan Balance", "float", "650000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Instalment", "to equal", "float", "5974.21"),
 
                                new TestOutput("Installment in Rands", "to equal", "string", "5974.21")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application that falls into category 1",
                            new List<ITestInput> { 
                                new TestInput("Current Mortgage Loan Balance", "float", "750000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Instalment", "to equal", "float", "7089.22"),
 
                                new TestOutput("Installment in Rands", "to equal", "string", "7089.22")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when calculating PTI ratio", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SelfEmployed"),
                        new TestInput ("Household Income", "float", "50000"),
                        new TestInput ("Property Purchase Price", "float", "0"),
                        new TestInput ("Deposit Amount", "float", "0"),
                        new TestInput ("Cash Amount Required", "float", "0"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "0"),
                        new TestInput ("Estimated Market Value of Property", "float", "1000000"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "620"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "False")
                    },
                    new List<IScenario> {
                        new Scenario("given an application that falls into category 0",
                            new List<ITestInput> { 
                                new TestInput("Current Mortgage Loan Balance", "float", "654000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.12")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application that falls into category 1",
                            new List<ITestInput> { 
                                new TestInput("Current Mortgage Loan Balance", "float", "750000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.141"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "14.0%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when calculating interest rate", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SelfEmployed"),
                        new TestInput ("Household Income", "float", "50000"),
                        new TestInput ("Property Purchase Price", "float", "0"),
                        new TestInput ("Deposit Amount", "float", "0"),
                        new TestInput ("Cash Amount Required", "float", "0"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "0"),
                        new TestInput ("Estimated Market Value of Property", "float", "1000000"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "620"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "False")
                    },
                    new List<IScenario> {
                        new Scenario("given an application that falls into category 0",
                            new List<ITestInput> { 
                                new TestInput("Current Mortgage Loan Balance", "float", "650000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate", "to equal", "float", "0.032"),
 
                                new TestOutput("Interest Rate", "to equal", "float", "0.093"),
 
                                new TestOutput("Interest Rate as Percent", "to equal", "string", "9.30%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application that falls into category 1",
                            new List<ITestInput> { 
                                new TestInput("Current Mortgage Loan Balance", "float", "750000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate", "to equal", "float", "0.036"),
 
                                new TestOutput("Interest Rate", "to equal", "float", "0.097"),
 
                                new TestOutput("Interest Rate as Percent", "to equal", "string", "9.70%")
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


 