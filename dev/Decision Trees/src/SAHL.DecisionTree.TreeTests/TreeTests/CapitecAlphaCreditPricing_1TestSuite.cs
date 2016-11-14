using System.Collections.Generic;
using SAHL.Tools.DecisionTree.TestRunner.Interfaces;
using SAHL.DecisionTree.TreeTests.Models;

namespace SAHL.DecisionTree.TreeTests
{
    public class CapitecAlphaCreditPricing_1TestSuite : ITestSuite
    {
        public IEnumerable<ITestCase> TestCases { get; set; }
    
        public CapitecAlphaCreditPricing_1TestSuite()
        {
            this.TestCases = new List<ITestCase> 
            {
                new TestCase("when alpha housing minimum requirements are not met", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried"),
                        new TestInput ("Household Income", "float", "18599"),
                        new TestInput ("Property Purchase Price", "float", "199999.99"),
                        new TestInput ("Deposit Amount", "float", "20000"),
                        new TestInput ("Cash Amount Required", "float", "40000.00"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "59999.99"),
                        new TestInput ("Estimated Market Value of Property", "float", "200000"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "600"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "True")
                    },
                    new List<IScenario> {
                        new Scenario("given a household income of R 7,999.99",
                            new List<ITestInput> { 

                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a household income of R 18,600.00",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "20000.00")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Total income is above the maximum for the product.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a property purchase price of R 199,999.99",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "199999.99"),
                                new TestInput("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase"),
                                new TestInput("Deposit Amount", "float", "20000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("The property value is below the minimum for the product.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a switch application with an est property value of R 199,999.99",
                            new List<ITestInput> { 
                                new TestInput("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch"),
                                new TestInput("Estimated Market Value of Property", "float", "199999.99")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("The property value is below the minimum for the product.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a switch application with a loan amount of R 99,999.99",
                            new List<ITestInput> { 
                                new TestInput("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch"),
                                new TestInput("Estimated Market Value of Property", "float", "200000"),
                                new TestInput("Current Mortgage Loan Balance", "float", "59999.99"),
                                new TestInput("Cash Amount Required", "float", "40000.00")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Loan amount requested is below the product minimum.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 99,999.99",
                            new List<ITestInput> { 
                                new TestInput("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase"),
                                new TestInput("Property Purchase Price", "float", "200000.00"),
                                new TestInput("Deposit Amount", "float", "100000.01")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Loan amount requested is below the product minimum.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when calculating the loan value", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried"),
                        new TestInput ("Household Income", "float", "15000"),
                        new TestInput ("Property Purchase Price", "float", "350000"),
                        new TestInput ("Deposit Amount", "float", "70000"),
                        new TestInput ("Cash Amount Required", "float", "0"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "0"),
                        new TestInput ("Estimated Market Value of Property", "float", "0"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "600"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "True")
                    },
                    new List<IScenario> {
                        new Scenario("given a new purchase application",
                            new List<ITestInput> { 

                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Loan Amount", "to equal", "float", "280000")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a switch application",
                            new List<ITestInput> { 
                                new TestInput("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch"),
                                new TestInput("Cash Amount Required", "float", "200000"),
                                new TestInput("Current Mortgage Loan Balance", "float", "150000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Loan Amount", "to equal", "float", "350000")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when calculating the LTV ratio", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried"),
                        new TestInput ("Household Income", "float", "15000"),
                        new TestInput ("Property Purchase Price", "float", "0"),
                        new TestInput ("Deposit Amount", "float", "0"),
                        new TestInput ("Cash Amount Required", "float", "200000"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "150000"),
                        new TestInput ("Estimated Market Value of Property", "float", "700000"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "600"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "True")
                    },
                    new List<IScenario> {
                        new Scenario("given a switch application",
                            new List<ITestInput> { 

                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Loan to Value", "to equal", "float", "0.5"),
 
                                new TestOutput("Loan to Value as Percent", "to equal", "string", "50.00 %")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application",
                            new List<ITestInput> { 
                                new TestInput("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase"),
                                new TestInput("Property Purchase Price", "float", "400000"),
                                new TestInput("Deposit Amount", "float", "120000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Loan to Value", "to equal", "float", "0.7"),
 
                                new TestOutput("Loan to Value as Percent", "to equal", "string", "70.00 %")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when calculating the PTI ratio", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried"),
                        new TestInput ("Household Income", "float", "15000"),
                        new TestInput ("Property Purchase Price", "float", "0"),
                        new TestInput ("Deposit Amount", "float", "0"),
                        new TestInput ("Cash Amount Required", "float", "150000"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "150000"),
                        new TestInput ("Estimated Market Value of Property", "float", "600000"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "600"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "True")
                    },
                    new List<IScenario> {
                        new Scenario("given a switch application",
                            new List<ITestInput> { 

                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Interest Rate", "to equal", "float", "0.1"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "19.3%"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.193")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application",
                            new List<ITestInput> { 
                                new TestInput("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase"),
                                new TestInput("Property Purchase Price", "float", "450000"),
                                new TestInput("Deposit Amount", "float", "150000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.193"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "19.3%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when calculating the instalment", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried"),
                        new TestInput ("Household Income", "float", "15000"),
                        new TestInput ("Property Purchase Price", "float", "0"),
                        new TestInput ("Deposit Amount", "float", "0"),
                        new TestInput ("Cash Amount Required", "float", "150000"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "150000"),
                        new TestInput ("Estimated Market Value of Property", "float", "600000"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "600"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "True")
                    },
                    new List<IScenario> {
                        new Scenario("given a switch application",
                            new List<ITestInput> { 

                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Installment in Rands", "to equal", "string", "2895.06")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application",
                            new List<ITestInput> { 
                                new TestInput("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase"),
                                new TestInput("Property Purchase Price", "float", "600000"),
                                new TestInput("Deposit Amount", "float", "300000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Installment in Rands", "to equal", "string", "2895.06")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when checking the minimum empirica requirement for category 6", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried"),
                        new TestInput ("Household Income", "float", "15000"),
                        new TestInput ("Property Purchase Price", "float", "0"),
                        new TestInput ("Deposit Amount", "float", "0"),
                        new TestInput ("Cash Amount Required", "float", "150000"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "150000"),
                        new TestInput ("Estimated Market Value of Property", "float", "600000"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "590"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "True")
                    },
                    new List<IScenario> {
                        new Scenario("given a salaried application with an empirica score of 591 and a 1.5% variance on the 600 limit",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "591")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory6")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given&nbsp;a salaried application with&nbsp;an empirica score of 592 and a&nbsp;1.5% variance on the 600 limit",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "592")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory6")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given&nbsp;a salaried application with&nbsp;an empirica score of 590 and a&nbsp;1.5% variance on the 600 limit",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "590")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Alpha", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("The best Credit Bureau score applicable for this application is below the minimum requirement for the product.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a salary with deduction application with an empirica score of 566 and a 1.5% variance on the 575 limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction"),
                                new TestInput("Application Empirica", "int", "566")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a salary with deduction application with an empirica score of 567 and a 1.5% variance on the 575 limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction"),
                                new TestInput("Application Empirica", "int", "567")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a salary with deduction application with an empirica score of 565 and a 1.5% variance on the 575 limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction"),
                                new TestInput("Application Empirica", "int", "565")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("The best Credit Bureau score applicable for this application is below the minimum requirement for the product.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a qualifying salaried application with an unknown empirica score",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "-999")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a&nbsp;qualifying&nbsp;salary with deduction application with an unknown empirica score",
                            new List<ITestInput> { 
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction"),
                                new TestInput("Application Empirica", "int", "-999")
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
                new TestCase("when checking the minimum empirica requirement for category 7", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried"),
                        new TestInput ("Household Income", "float", "15000"),
                        new TestInput ("Property Purchase Price", "float", "0"),
                        new TestInput ("Deposit Amount", "float", "0"),
                        new TestInput ("Cash Amount Required", "float", "150000"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "150000"),
                        new TestInput ("Estimated Market Value of Property", "float", "350000"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "592"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "True")
                    },
                    new List<IScenario> {
                        new Scenario("given an application empirica of 591 and a 1.5% variance on the 600 limit&nbsp;",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "591")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory7"),
 
                                new TestOutput("Alpha", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an empirica score of 592 and a 1.5% variance on the 600 limit",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "592")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory7"),
 
                                new TestOutput("Alpha", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an empirica score of 590 and a 1.5% variance on the 600 limit",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "590")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Alpha", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("The best Credit Bureau score applicable for this application is below the minimum requirement for the product.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a salary with deduction application with an empirica score of 565 and a 1.5% variance on the 575 limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction"),
                                new TestInput("Application Empirica", "int", "565")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("The best Credit Bureau score applicable for this application is below the minimum requirement for the product.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a salary with deduction application with an empirica score of 566 and a 1.5% variance on the 575 limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction"),
                                new TestInput("Application Empirica", "int", "566")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a salary with deduction application with an empirica score of 567 and a 1.5% variance on the 575 limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction"),
                                new TestInput("Application Empirica", "int", "567")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a&nbsp;qualifying&nbsp;salaried application with an unknown empirica score",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "-999"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory7")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a&nbsp;qualifying&nbsp;salary with deduction application with an unknown empirica score",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "-999"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory7")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when checking the minimum empirica requirement for category 8", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried"),
                        new TestInput ("Household Income", "float", "12000"),
                        new TestInput ("Property Purchase Price", "float", "0"),
                        new TestInput ("Deposit Amount", "float", "0"),
                        new TestInput ("Cash Amount Required", "float", "150000"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "150000"),
                        new TestInput ("Estimated Market Value of Property", "float", "320000"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "601"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "True")
                    },
                    new List<IScenario> {
                        new Scenario("given an application empirica of 601 and a 1.5% variance on the 610 limit",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "601")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory8"),
 
                                new TestOutput("Alpha", "to equal", "bool", "True"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application empirica of 600 and a 1.5% variance on the 610 limit",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "600")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Alpha", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("The best Credit Bureau score applicable for this application is below the minimum requirement for the product.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application empirica of 602 and a 1.5% variance on the 610 limit",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "602")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory8"),
 
                                new TestOutput("Alpha", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a salary with deduction application with an empirica score of 565 and a 1.5% variance on the 575 limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction"),
                                new TestInput("Application Empirica", "int", "565")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("The best Credit Bureau score applicable for this application is below the minimum requirement for the product.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a salary with deduction application with an empirica score of 566 and a 1.5% variance on the 575 limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction"),
                                new TestInput("Application Empirica", "int", "566")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a salary with deduction application with an empirica score of 567 and a 1.5% variance on the 575 limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction"),
                                new TestInput("Application Empirica", "int", "567")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a&nbsp;qualifying&nbsp;salaried application with an unknown empirica score",
                            new List<ITestInput> { 
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried"),
                                new TestInput("Application Empirica", "int", "-999")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory8")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a&nbsp;qualifying&nbsp;salary with deduction application with an unknown empirica score",
                            new List<ITestInput> { 
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction"),
                                new TestInput("Application Empirica", "int", "-999")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory8")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when checking the minimum empirica requirement for category 9", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried"),
                        new TestInput ("Household Income", "float", "12000"),
                        new TestInput ("Property Purchase Price", "float", "0"),
                        new TestInput ("Deposit Amount", "float", "0"),
                        new TestInput ("Cash Amount Required", "float", "150000"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "150000"),
                        new TestInput ("Estimated Market Value of Property", "float", "310000"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "610"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "True")
                    },
                    new List<IScenario> {
                        new Scenario("given an application empirica of 600 and a 1.5% variance on the 610 limit",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "600")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Alpha", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("The best Credit Bureau score applicable for this application is below the minimum requirement for the product.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application empirica of 601 and a 1.5% variance on the 610 limit",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "601")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory9"),
 
                                new TestOutput("Alpha", "to equal", "bool", "True"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application empirica of 602 and a 1.5% variance on the 610 limit",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "602")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory9"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Alpha", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a salary with deduction application with an empirica score of 565 and a 1.5% variance on the 575 limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction"),
                                new TestInput("Application Empirica", "int", "565")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("The best Credit Bureau score applicable for this application is below the minimum requirement for the product.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a salary with deduction application with an empirica score of 566 and a 1.5% variance on the 575 limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction"),
                                new TestInput("Application Empirica", "int", "566")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a salary with deduction application with an empirica score of 567 and a 1.5% variance on the 575 limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction"),
                                new TestInput("Application Empirica", "int", "567")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a&nbsp;qualifying&nbsp;salaried application with an unknown empirica score",
                            new List<ITestInput> { 
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried"),
                                new TestInput("Application Empirica", "int", "-999")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory9")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a&nbsp;qualifying&nbsp;salary with deduction application with an unknown empirica score",
                            new List<ITestInput> { 
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction"),
                                new TestInput("Application Empirica", "int", "-999")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory9")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when an application falls into category 6", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried"),
                        new TestInput ("Household Income", "float", "8752"),
                        new TestInput ("Property Purchase Price", "float", "400000"),
                        new TestInput ("Deposit Amount", "float", "100000"),
                        new TestInput ("Cash Amount Required", "float", "0"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "0"),
                        new TestInput ("Estimated Market Value of Property", "float", "0"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "600"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "False")
                    },
                    new List<IScenario> {
                        new Scenario("given an application with PTI ratio of 33.0% and a 10% variance on the 30% limit",
                            new List<ITestInput> { 

                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "33.0%"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.33"),
 
                                new TestOutput("Instalment", "to equal", "float", "2895.06")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 33.0% and is greater than or equal to the maximum of 33.0%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R299283 or alternatively additional income so that total income is at least R8774.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with PTI ratio of 32.9% and a 10% variance on the 30% limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "8789")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "32.9%"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.329"),
 
                                new TestOutput("Alpha", "to equal", "bool", "True"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory6")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with PTI ratio of 33.1% and a 10% variance on the 30% limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "8736")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "33.1%"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.331"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Alpha", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 33.1% and is greater than or equal to the maximum of 33.0%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R298736 or alternatively additional income so that total income is at least R8774.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a qualifying salary with deduction application",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "12000"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory6"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.039")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a qualifying salaried application",
                            new List<ITestInput> { 
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried"),
                                new TestInput("Household Income", "float", "12000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory6"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.039"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when an application falls into category 7", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried"),
                        new TestInput ("Household Income", "float", "12000"),
                        new TestInput ("Property Purchase Price", "float", "0"),
                        new TestInput ("Deposit Amount", "float", "0"),
                        new TestInput ("Cash Amount Required", "float", "150000"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "150000"),
                        new TestInput ("Estimated Market Value of Property", "float", "350000"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "610"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "True")
                    },
                    new List<IScenario> {
                        new Scenario("given an application with PTI ratio of 33.0% and a 10% variance on the 30% limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "9000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "33.0%"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Alpha", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.33")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 33.0% and is greater than or equal to the maximum of 33.0%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R299493 or alternatively additional income so that total income is at least R9016.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with PTI ratio of 33.1% and a 10% variance on the 30% limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "8970")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.331"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "33.1%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 33.1% and is greater than or equal to the maximum of 33.0%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R298495 or alternatively additional income so that total income is at least R9016.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with PTI ratio of 32.9% and a 10% variance on the 30% limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "9040")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "32.9%"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.329"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Alpha", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory7")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a qualifying salaried application",
                            new List<ITestInput> { 
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried"),
                                new TestInput("Household Income", "float", "12000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory7"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.043")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a qualifying salary with deduction application",
                            new List<ITestInput> { 
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction"),
                                new TestInput("Household Income", "float", "12000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory7"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.043")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when an application falls into category 8", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried"),
                        new TestInput ("Household Income", "float", "12000"),
                        new TestInput ("Property Purchase Price", "float", "0"),
                        new TestInput ("Deposit Amount", "float", "0"),
                        new TestInput ("Cash Amount Required", "float", "150000"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "150000"),
                        new TestInput ("Estimated Market Value of Property", "float", "325000"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "610"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "True")
                    },
                    new List<IScenario> {
                        new Scenario("given an application with PTI ratio of 32.9% and a 10% variance on the 30% limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "9220")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.329"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "32.9%"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory8"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with PTI ratio of 33.00% and a 10% variance on the 30% limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "9180")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "33.0%"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.33")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 33.0% and is greater than or equal to the maximum of 33.0%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R299391 or alternatively additional income so that total income is at least R9200.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with PTI ratio of 33.1% and a 10% variance on the 30% limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "9160")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.331"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "33.1%"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 33.1% and is greater than or equal to the maximum of 33.0%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R298739 or alternatively additional income so that total income is at least R9200.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a qualifying salaried application",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "12000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate", "to equal", "float", "0.046")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a qualifying salaried with deduction application",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "12000"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate", "to equal", "float", "0.046")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when an application falls into category 9", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried"),
                        new TestInput ("Household Income", "float", "12121.2121212121"),
                        new TestInput ("Property Purchase Price", "float", "400000"),
                        new TestInput ("Deposit Amount", "float", "0"),
                        new TestInput ("Cash Amount Required", "float", "100000"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "290094.900619586"),
                        new TestInput ("Estimated Market Value of Property", "float", "400000"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "610"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "True")
                    },
                    new List<IScenario> {
                        new Scenario("given an application with PTI ratio of 32.9% and a 1% variance on the 30% limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "12070"),
                                new TestInput("Cash Amount Required", "float", "100000"),
                                new TestInput("Current Mortgage Loan Balance", "float", "290094.900619586"),
                                new TestInput("Estimated Market Value of Property", "float", "400000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.329"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "32.9%")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with PTI ratio of 33.0% and a 1% variance on the 30% limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "12040")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "33.0%"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.33"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 33.0% and is greater than or equal to the maximum of 33.0%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R390059 or alternatively additional income so that total income is at least R12042.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with PTI ratio of 33.1% and a 1% variance on the 30% limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "12000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "33.1%"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.331"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 33.1% and is greater than or equal to the maximum of 33.0%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R388763 or alternatively additional income so that total income is at least R12042.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a qualifying salaried application",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "16000"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory9"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.047")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a qualifying salary with deduction application",
                            new List<ITestInput> { 
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction"),
                                new TestInput("Household Income", "float", "16000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.047"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory9")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when ensuring an application does not exceed the alpha housing category LTV limits", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried"),
                        new TestInput ("Household Income", "float", "12500"),
                        new TestInput ("Property Purchase Price", "float", "352941"),
                        new TestInput ("Deposit Amount", "float", "0"),
                        new TestInput ("Cash Amount Required", "float", "0"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "0"),
                        new TestInput ("Estimated Market Value of Property", "float", "0"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "605"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "False")
                    },
                    new List<IScenario> {
                        new Scenario("given an application with an LTV of 100%",
                            new List<ITestInput> { 

                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Alpha", "to equal", "bool", "False"),
 
                                new TestOutput("Loan to Value", "to equal", "float", "1")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your loan amount as a percentage of purchase price (LTV) would be 100.00 %, which is greater than or equal to the maximum of 100.0%. In order to reduce your LTV, the loan amount would need to be lowered by increasing the deposit by R1 or more.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a category 9 application with an LTV of 99.9%",
                            new List<ITestInput> { 
                                new TestInput("Deposit Amount", "float", "300.30"),
                                new TestInput("Property Purchase Price", "float", "300300.30")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Alpha", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory9"),
 
                                new TestOutput("Loan to Value", "to equal", "float", "0.999")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a category 9 application with an LTV of 96%",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "312500"),
                                new TestInput("Deposit Amount", "float", "12500")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Loan to Value", "to equal", "float", "0.96"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory9"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Alpha", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a category 9 application with an LTV of 96.1%",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "312174.81"),
                                new TestInput("Deposit Amount", "float", "12174.81")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory9"),
 
                                new TestOutput("Loan to Value", "to equal", "float", "0.961"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Alpha", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a category 8 application with an LTV of 92.1%",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "325732.89"),
                                new TestInput("Deposit Amount", "float", "25732.89")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Loan to Value", "to equal", "float", "0.921"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory8")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a category 8 application with an LTV of 92%",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "326086.95"),
                                new TestInput("Deposit Amount", "float", "26086.95")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Loan to Value", "to equal", "float", "0.92"),
 
                                new TestOutput("Alpha", "to equal", "bool", "True"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory8")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a category 7 application with an LTV of 91.9%",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "326441.78"),
                                new TestInput("Deposit Amount", "float", "26441.78")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Loan to Value", "to equal", "float", "0.919"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory7"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Alpha", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a category 7 application with an LTV of 85.1%",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "352526.43"),
                                new TestInput("Deposit Amount", "float", "52526.43")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Loan to Value", "to equal", "float", "0.851"),
 
                                new TestOutput("Alpha", "to equal", "bool", "True"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory7")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a category 8 application with an LTV of 95.9%",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "312825.86"),
                                new TestInput("Deposit Amount", "float", "12825.86")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Loan to Value", "to equal", "float", "0.959"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory8"),
 
                                new TestOutput("Alpha", "to equal", "bool", "True"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a category 7 application with an LTV of 85%",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "352941.17"),
                                new TestInput("Deposit Amount", "float", "52941.17")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Loan to Value", "to equal", "float", "0.85"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory7"),
 
                                new TestOutput("Alpha", "to equal", "bool", "True"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a category 6 application with an LTV of 84.9%",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "353356.89"),
                                new TestInput("Deposit Amount", "float", "53356.89")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Loan to Value", "to equal", "float", "0.849"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory6"),
 
                                new TestOutput("Alpha", "to equal", "bool", "True"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with a valuation of 100.1%",
                            new List<ITestInput> { 
                                new TestInput("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch"),
                                new TestInput("Estimated Market Value of Property", "float", "299700.29"),
                                new TestInput("Cash Amount Required", "float", "150000"),
                                new TestInput("Current Mortgage Loan Balance", "float", "150000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Alpha", "to equal", "bool", "False"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Loan to Value", "to equal", "float", "1.001")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your loan amount as a percentage of estimated property value (LTV) would be 100.10 %, which is greater than or equal to the maximum of 100.0%. In order to reduce your LTV, the loan amount would need to be lowered by decreasing the cash required by R301 or more.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when checking the minimum income requirements for category 6", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried"),
                        new TestInput ("Household Income", "float", "8000.00"),
                        new TestInput ("Property Purchase Price", "float", "0"),
                        new TestInput ("Deposit Amount", "float", "0"),
                        new TestInput ("Cash Amount Required", "float", "165000"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "100000"),
                        new TestInput ("Estimated Market Value of Property", "float", "400000"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "605"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "True")
                    },
                    new List<IScenario> {
                        new Scenario("given a qualifying application with a household income of R 8,000.00",
                            new List<ITestInput> { 

                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory6")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Total income is below the required minimum for the product.", "should not contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a qualifying application with a household income of R 7,999.99",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "7999.99")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Total income is below the required minimum for the product.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a qualifying application with a household income of R 8,000.01",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "8000.01")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Total income is below the required minimum for the product.", "should not contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when checking the minimum income requirements for category 7", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried"),
                        new TestInput ("Household Income", "float", "8000.00"),
                        new TestInput ("Property Purchase Price", "float", "0"),
                        new TestInput ("Deposit Amount", "float", "0"),
                        new TestInput ("Cash Amount Required", "float", "100000"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "155542"),
                        new TestInput ("Estimated Market Value of Property", "float", "285000"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "605"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "True")
                    },
                    new List<IScenario> {
                        new Scenario("given an application with a household income of R 8,000.00",
                            new List<ITestInput> { 

                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory7")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Total income is below the required minimum for the product.", "should not contain", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with a household income of R 7,999.99",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "7999.99")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Total income is below the required minimum for the product.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with a household income of R 8,000.01",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "8000.01")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Total income is below the required minimum for the product.", "should not contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when checking the minimum income requirements for category 8", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried"),
                        new TestInput ("Household Income", "float", "8000.00"),
                        new TestInput ("Property Purchase Price", "float", "262000"),
                        new TestInput ("Deposit Amount", "float", "13100"),
                        new TestInput ("Cash Amount Required", "float", "0"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "0"),
                        new TestInput ("Estimated Market Value of Property", "float", "0"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "605"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "False")
                    },
                    new List<IScenario> {
                        new Scenario("given an application with a household income of R 8,000.00",
                            new List<ITestInput> { 

                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory8")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Total income is below the required minimum for the product.", "should not contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with a household income of R 7,999.99",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "7999.99")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Total income is below the required minimum for the product.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with a household income of R 8,000.01",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "8000.01")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Total income is below the required minimum for the product.", "should not contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when checking the minimum income requirements for category 9", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried"),
                        new TestInput ("Household Income", "float", "10000.00"),
                        new TestInput ("Property Purchase Price", "float", "300000"),
                        new TestInput ("Deposit Amount", "float", "6000"),
                        new TestInput ("Cash Amount Required", "float", "0"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "0"),
                        new TestInput ("Estimated Market Value of Property", "float", "0"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "605"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "True")
                    },
                    new List<IScenario> {
                        new Scenario("given an application with a household income of R 10,000.00",
                            new List<ITestInput> { 

                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory9")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Total income is below the required minimum for the product.", "should not contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with a household income of R 10,000.01",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "10000.01")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory9")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Total income is below the required minimum for the product.", "should not contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with a household income of R 9,999.99",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "9999.99")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Total income is below the required minimum for the product.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    })
            };
        }
    }
}


 