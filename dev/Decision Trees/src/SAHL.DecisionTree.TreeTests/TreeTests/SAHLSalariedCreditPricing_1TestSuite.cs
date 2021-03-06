using System.Collections.Generic;
using SAHL.Tools.DecisionTree.TestRunner.Interfaces;
using SAHL.DecisionTree.TreeTests.Models;

namespace SAHL.DecisionTree.TreeTests
{
    public class SAHLSalariedCreditPricing_1TestSuite : ITestSuite
    {
        public IEnumerable<ITestCase> TestCases { get; set; }
    
        public SAHLSalariedCreditPricing_1TestSuite()
        {
            this.TestCases = new List<ITestCase> 
            {
                new TestCase("when an application does not meet the minimum salaried requirements", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried"),
                        new TestInput ("Household Income", "float", "150000"),
                        new TestInput ("Property Purchase Price", "float", "1000000"),
                        new TestInput ("Deposit Amount", "float", "700000"),
                        new TestInput ("Cash Amount Required", "float", "0"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "0"),
                        new TestInput ("Estimated Market Value of Property", "float", "0"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "640"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "False")
                    },
                    new List<IScenario> {
                        new Scenario("given an application with a household income of R 13,000",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "13000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Total income is below the required minimum for salaried applicants.", "should be empty", "Warning Messages"),
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with a household income of R 12,999",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "12999")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Total income is below the required minimum for salaried applicants.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with a household income of R 13,001",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "13001")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Warning Messages"),
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with a loan value of R150,000",
                            new List<ITestInput> { 
                                new TestInput("Deposit Amount", "float", "850000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Loan amount requested is below the product minimum.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with a loan value of R150,001",
                            new List<ITestInput> { 
                                new TestInput("Deposit Amount", "float", "849999")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Warning Messages"),
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with a loan value of R149,999",
                            new List<ITestInput> { 
                                new TestInput("Deposit Amount", "float", "850001")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Loan amount requested is below the product minimum.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with a loan value of R 4,999,999",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "8000000"),
                                new TestInput("Deposit Amount", "float", "3000001"),
                                new TestInput("Household Income", "float", "200000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with a loan value of R 5,000,000",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "8000000"),
                                new TestInput("Deposit Amount", "float", "3000000"),
                                new TestInput("Household Income", "float", "200000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Loan amount requested is above the product maximum.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with a loan value of R 5,000,001",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "8000000"),
                                new TestInput("Deposit Amount", "float", "2999999"),
                                new TestInput("Household Income", "float", "200000")
                            },
                            new List<ITestOutput> {

                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Loan amount requested is above the product maximum.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when calculating the loan amount", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried"),
                        new TestInput ("Household Income", "float", "50000"),
                        new TestInput ("Property Purchase Price", "float", "2000000"),
                        new TestInput ("Deposit Amount", "float", "200000"),
                        new TestInput ("Cash Amount Required", "float", "1000000"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "500000"),
                        new TestInput ("Estimated Market Value of Property", "float", "2000000"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "600"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "False")
                    },
                    new List<IScenario> {
                        new Scenario("given a switch application",
                            new List<ITestInput> { 
                                new TestInput("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch"),
                                new TestInput("Estimated Market Value of Property", "float", "2000000"),
                                new TestInput("Current Mortgage Loan Balance", "float", "500000"),
                                new TestInput("Cash Amount Required", "float", "1000000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Loan Amount", "to equal", "float", "1500000")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Info Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "2000000"),
                                new TestInput("Deposit Amount", "float", "200000"),
                                new TestInput("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Loan Amount", "to equal", "float", "1800000")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Info Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when calculating the PTI ratio", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried"),
                        new TestInput ("Household Income", "float", "0"),
                        new TestInput ("Property Purchase Price", "float", "0"),
                        new TestInput ("Deposit Amount", "float", "0"),
                        new TestInput ("Cash Amount Required", "float", "0"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "0"),
                        new TestInput ("Estimated Market Value of Property", "float", "0"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "640"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "False")
                    },
                    new List<IScenario> {
                        new Scenario("given an application in category 0",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "2000000"),
                                new TestInput("Deposit Amount", "float", "720000"),
                                new TestInput("Household Income", "float", "50000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Interest Rate", "to equal", "float", "0.088"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "22.7%"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.227"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory0"),
 
                                new TestOutput("Interest Rate as Percent", "to equal", "string", "8.80%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given&nbsp;an application in category 1",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "2370000"),
                                new TestInput("Deposit Amount", "float", "600000"),
                                new TestInput("Household Income", "float", "100000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "16.1%"),
 
                                new TestOutput("Interest Rate", "to equal", "float", "0.092"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.161"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory1"),
 
                                new TestOutput("Interest Rate as Percent", "to equal", "string", "9.20%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application in category 3",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "2000000"),
                                new TestInput("Deposit Amount", "float", "375000"),
                                new TestInput("Household Income", "float", "100000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory3"),
 
                                new TestOutput("Interest Rate", "to equal", "float", "0.097"),
 
                                new TestOutput("Interest Rate as Percent", "to equal", "string", "9.70%"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.153"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "15.3%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application in category 4",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "3100000"),
                                new TestInput("Deposit Amount", "float", "430000"),
                                new TestInput("Household Income", "float", "100000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory4"),
 
                                new TestOutput("Interest Rate", "to equal", "float", "0.099"),
 
                                new TestOutput("Interest Rate as Percent", "to equal", "string", "9.90%"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.255"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "25.5%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application in category 5",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "1800000"),
                                new TestInput("Deposit Amount", "float", "125000"),
                                new TestInput("Household Income", "float", "100000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory5"),
 
                                new TestOutput("Interest Rate", "to equal", "float", "0.099"),
 
                                new TestOutput("Interest Rate as Percent", "to equal", "string", "9.90%"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.16"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "16.0%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when calculating the LTV ratio", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried"),
                        new TestInput ("Household Income", "float", "100000"),
                        new TestInput ("Property Purchase Price", "float", "2000000"),
                        new TestInput ("Deposit Amount", "float", "500000"),
                        new TestInput ("Cash Amount Required", "float", "1000000"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "500000"),
                        new TestInput ("Estimated Market Value of Property", "float", "2000000"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "620"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "False")
                    },
                    new List<IScenario> {
                        new Scenario("given a switch application",
                            new List<ITestInput> { 

                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Loan to Value", "to equal", "float", "0.75"),
 
                                new TestOutput("Loan to Value as Percent", "to equal", "string", "75.00 %")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application",
                            new List<ITestInput> { 
                                new TestInput("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase"),
                                new TestInput("Property Purchase Price", "float", "2000000"),
                                new TestInput("Deposit Amount", "float", "500000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Loan to Value", "to equal", "float", "0.75"),
 
                                new TestOutput("Loan to Value as Percent", "to equal", "string", "75.00 %")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase with LTV > 95%",
                            new List<ITestInput> { 
                                new TestInput("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase"),
                                new TestInput("Household Income", "float", "100000"),
                                new TestInput("Property Purchase Price", "float", "1800000"),
                                new TestInput("Deposit Amount", "float", "50000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Loan to Value", "to equal", "float", "0.972")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when calculating the instalment", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried"),
                        new TestInput ("Household Income", "float", "100000"),
                        new TestInput ("Property Purchase Price", "float", "0"),
                        new TestInput ("Deposit Amount", "float", "0"),
                        new TestInput ("Cash Amount Required", "float", "0"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "0"),
                        new TestInput ("Estimated Market Value of Property", "float", "0"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "640"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "False")
                    },
                    new List<IScenario> {
                        new Scenario("given&nbsp;an application in category 0",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "2000000"),
                                new TestInput("Deposit Amount", "float", "700000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Installment in Rands", "to equal", "string", "11529.74"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory0"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given&nbsp;an application in category 1",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "2400000"),
                                new TestInput("Deposit Amount", "float", "600000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Installment in Rands", "to equal", "string", "16427.31"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory1"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application in category 3",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "2000000"),
                                new TestInput("Deposit Amount", "float", "350000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Installment in Rands", "to equal", "string", "15596.29"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory3"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application in category 4",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "2000000"),
                                new TestInput("Deposit Amount", "float", "250000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Installment in Rands", "to equal", "string", "16772.09"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory4"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application in category 5",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "1800000"),
                                new TestInput("Deposit Amount", "float", "100000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Installment in Rands", "to equal", "string", "16292.89"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory5"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when calculating the property value", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried"),
                        new TestInput ("Household Income", "float", "50000"),
                        new TestInput ("Property Purchase Price", "float", "0"),
                        new TestInput ("Deposit Amount", "float", "0"),
                        new TestInput ("Cash Amount Required", "float", "1000000"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "500000"),
                        new TestInput ("Estimated Market Value of Property", "float", "2000000"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "620"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "False")
                    },
                    new List<IScenario> {
                        new Scenario("given a switch application",
                            new List<ITestInput> { 

                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Property Value", "to equal", "float", "2000000")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application",
                            new List<ITestInput> { 
                                new TestInput("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase"),
                                new TestInput("Property Purchase Price", "float", "2000000"),
                                new TestInput("Deposit Amount", "float", "500000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Property Value", "to equal", "float", "2000000")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when an application falls into category 0", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried"),
                        new TestInput ("Household Income", "float", "34447.153"),
                        new TestInput ("Property Purchase Price", "float", "2000000"),
                        new TestInput ("Deposit Amount", "float", "700000"),
                        new TestInput ("Cash Amount Required", "float", "0"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "0"),
                        new TestInput ("Estimated Market Value of Property", "float", "0"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "576"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "False")
                    },
                    new List<IScenario> {
                        new Scenario("given an application with PTI ratio of 32.90% and a 10% variance on the 30% limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "35000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory0"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.329"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "32.9%"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.028")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with PTI ratio of 33.00% and a 10% variance on the 30% limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "34900")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory0"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.33"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "33.0%"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with PTI ratio of 33.10% and a 10% variance on the 30% limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "34800")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory0"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.331"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "33.1%"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with PTI ratio of 27.40% and a 10% variance on the 25% limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "93800"),
                                new TestInput("Property Purchase Price", "float", "4200000"),
                                new TestInput("Deposit Amount", "float", "1300000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory0"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.274"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.4%"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.028")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with PTI ratio of 27.50% and a 10% variance on the 25% limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "93500"),
                                new TestInput("Property Purchase Price", "float", "4200000"),
                                new TestInput("Deposit Amount", "float", "1300000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory0"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.275"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.5%"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with PTI ratio of 27.60% and a 10% variance on the 25% limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "93000"),
                                new TestInput("Property Purchase Price", "float", "4200000"),
                                new TestInput("Deposit Amount", "float", "1300000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory0"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.276"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.6%"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount > 1 800 000 and PTI < 30%",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "60000"),
                                new TestInput("Property Purchase Price", "float", "3000000"),
                                new TestInput("Deposit Amount", "float", "1000000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory0"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.295"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "29.5%"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount > 1 800 000 and PTI > 30%",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "52000"),
                                new TestInput("Property Purchase Price", "float", "3000000"),
                                new TestInput("Deposit Amount", "float", "1000000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory0"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.341")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 34.1% and is greater than or equal to the maximum of 33.0%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1934820 or alternatively additional income so that total income is at least R53753.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with an income = 12999",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "12999")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Total income is below the required minimum for salaried applicants.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with an income = 13000",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "13000"),
                                new TestInput("Deposit Amount", "float", "1600000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory0")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with an income = 13001",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "13001"),
                                new TestInput("Deposit Amount", "float", "1600000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory0")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when an application falls into category 1", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried"),
                        new TestInput ("Household Income", "float", "49225"),
                        new TestInput ("Property Purchase Price", "float", "2400000"),
                        new TestInput ("Deposit Amount", "float", "600000"),
                        new TestInput ("Cash Amount Required", "float", "0"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "0"),
                        new TestInput ("Estimated Market Value of Property", "float", "0"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "601"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "False")
                    },
                    new List<IScenario> {
                        new Scenario("given an application with PTI ratio of 32.90% and a 10% variance on the 30% limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "49800")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory1"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.329"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "32.9%"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.032")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with PTI ratio of 33.00% and a 10% variance on the 30% limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "49750")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory1"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.33"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "33.0%"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with PTI ratio of 33.10% and a 10% variance on the 30% limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "49500")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory1"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.331"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "33.1%"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with PTI ratio of 27.40% and a 10% variance on the 25% limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "89800"),
                                new TestInput("Property Purchase Price", "float", "3500000"),
                                new TestInput("Deposit Amount", "float", "800000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory1"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.274"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.4%"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.032")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with PTI ratio of 27.50% and a 10% variance on the 25% limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "89500"),
                                new TestInput("Property Purchase Price", "float", "3500000"),
                                new TestInput("Deposit Amount", "float", "800000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory1"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.275"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.5%"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with PTI ratio of 27.60% and a 10% variance on the 25% limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "89200"),
                                new TestInput("Property Purchase Price", "float", "3500000"),
                                new TestInput("Deposit Amount", "float", "800000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory1"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.276"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.6%"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount  > 2 750 000",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "5000000"),
                                new TestInput("Deposit Amount", "float", "1200000"),
                                new TestInput("Household Income", "float", "100000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory1")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Loan amount requested is above the product maximum.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with an income = 12999",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "12999")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Total income is below the required minimum for salaried applicants.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with an income = 13000",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "13000"),
                                new TestInput("Deposit Amount", "float", "150000"),
                                new TestInput("Property Purchase Price", "float", "550000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory1")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with an income = 13001",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "13001"),
                                new TestInput("Deposit Amount", "float", "150000"),
                                new TestInput("Property Purchase Price", "float", "550000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory1")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when an application falls into category 3", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried"),
                        new TestInput ("Household Income", "float", "46748"),
                        new TestInput ("Property Purchase Price", "float", "2000000"),
                        new TestInput ("Deposit Amount", "float", "350000"),
                        new TestInput ("Cash Amount Required", "float", "0"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "0"),
                        new TestInput ("Estimated Market Value of Property", "float", "0"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "611"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "False")
                    },
                    new List<IScenario> {
                        new Scenario("given an application with PTI ratio of 32.90% and a 10% variance on the 30% limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "47300")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory3"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.329"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "32.9%"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.037")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with PTI ratio of 33.00% and a 10% variance on the 30% limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "47250")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory3"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.33"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "33.0%"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with PTI ratio of 33.10% and a 10% variance on the 30% limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "47100")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory3"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.331"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "33.1%"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with PTI ratio of 27.40% and a 10% variance on the 25% limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "92900"),
                                new TestInput("Property Purchase Price", "float", "3300000"),
                                new TestInput("Deposit Amount", "float", "600000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory3"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.274"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.4%"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.037")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with PTI ratio of 27.50% and a 10% variance on the 25% limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "92500"),
                                new TestInput("Property Purchase Price", "float", "3300000"),
                                new TestInput("Deposit Amount", "float", "600000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory3"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.275"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.5%"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with PTI ratio of 27.60% and a 10% variance on the 25% limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "92250"),
                                new TestInput("Property Purchase Price", "float", "3300000"),
                                new TestInput("Deposit Amount", "float", "600000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory3"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.276"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.6%"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount > 2 750 000",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "3600000"),
                                new TestInput("Deposit Amount", "float", "600000"),
                                new TestInput("Household Income", "float", "150000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory3")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Loan amount requested is above the product maximum.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with an income = 18599",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "18599")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Total income is below the required minimum for salaried applicants.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with an income = 18600",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "18600"),
                                new TestInput("Deposit Amount", "float", "100000"),
                                new TestInput("Property Purchase Price", "float", "650000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory3")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with an income = 18601",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "18601"),
                                new TestInput("Deposit Amount", "float", "100000"),
                                new TestInput("Property Purchase Price", "float", "650000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory3")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when an application falls into category 4", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried"),
                        new TestInput ("Household Income", "float", "50141.0338278081000000"),
                        new TestInput ("Property Purchase Price", "float", "2000000"),
                        new TestInput ("Deposit Amount", "float", "250000"),
                        new TestInput ("Cash Amount Required", "float", "0"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "0"),
                        new TestInput ("Estimated Market Value of Property", "float", "0"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "626"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "False")
                    },
                    new List<IScenario> {
                        new Scenario("given an application with PTI ratio of 32.90% and a 10% variance on the 30% limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "50850")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory4"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.329"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "32.9%"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.039")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with PTI ratio of 33.00% and a 10% variance on the 30% limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "50750")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory4"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.33"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "33.0%"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with PTI ratio of 33.10% and a 10% variance on the 30% limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "50550")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory4"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.331"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "33.1%"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with PTI ratio of 27.40% and a 10% variance on the 25% limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "94250"),
                                new TestInput("Property Purchase Price", "float", "3100000"),
                                new TestInput("Deposit Amount", "float", "400000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory4"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.274"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.4%"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.039")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with PTI ratio of 27.50% and a 10% variance on the 25% limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "94000"),
                                new TestInput("Property Purchase Price", "float", "3100000"),
                                new TestInput("Deposit Amount", "float", "400000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory4"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.275"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.5%"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with PTI ratio of 27.60% and a 10% variance on the 25% limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "93750"),
                                new TestInput("Property Purchase Price", "float", "3100000"),
                                new TestInput("Deposit Amount", "float", "400000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory4"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.276"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.6%"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount > 2 750 000",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "3600000"),
                                new TestInput("Deposit Amount", "float", "450000"),
                                new TestInput("Household Income", "float", "150000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory4")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Loan amount requested is above the product maximum.", "should contain", "Warning Messages"),
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with an income = 18599",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "18599")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Total income is below the required minimum for salaried applicants.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with an income = 18600",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "18600"),
                                new TestInput("Deposit Amount", "float", "80000"),
                                new TestInput("Property Purchase Price", "float", "600000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory4")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with an income = 18601",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "18601"),
                                new TestInput("Deposit Amount", "float", "80000"),
                                new TestInput("Property Purchase Price", "float", "600000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory4")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when an application falls into category 5", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried"),
                        new TestInput ("Household Income", "float", "0"),
                        new TestInput ("Property Purchase Price", "float", "1800000"),
                        new TestInput ("Deposit Amount", "float", "100000"),
                        new TestInput ("Cash Amount Required", "float", "0"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "0"),
                        new TestInput ("Estimated Market Value of Property", "float", "0"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "641"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "False")
                    },
                    new List<IScenario> {
                        new Scenario("given an application with PTI ratio of 27.40% and a 10% variance on the 25% limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "59250")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory5"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.274"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.4%"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.039")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with PTI ratio of 27.50% and a 10% variance on the 25% limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "59100")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory5"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.275"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.5%"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with PTI ratio of 27.60% and a 10% variance on the 25% limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "59000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory5"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.276"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.6%"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with loan amount > 1 800 000",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "2000000"),
                                new TestInput("Deposit Amount", "float", "150000"),
                                new TestInput("Household Income", "float", "100000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory5")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Loan amount requested is above the product maximum.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with an income = 18599",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "18599")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Total income is below the required minimum for salaried applicants.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with an income = 18600",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "18600"),
                                new TestInput("Deposit Amount", "float", "40000"),
                                new TestInput("Property Purchase Price", "float", "500000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory5")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with an income = 18601",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "18601"),
                                new TestInput("Deposit Amount", "float", "40000"),
                                new TestInput("Property Purchase Price", "float", "500000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory5")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when checking the minimum empirica requirement&nbsp;for category 0", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried"),
                        new TestInput ("Household Income", "float", "50000"),
                        new TestInput ("Property Purchase Price", "float", "2000000"),
                        new TestInput ("Deposit Amount", "float", "800000"),
                        new TestInput ("Cash Amount Required", "float", "0"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "0"),
                        new TestInput ("Estimated Market Value of Property", "float", "0"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "574"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "False")
                    },
                    new List<IScenario> {
                        new Scenario("given an application with an empirica score of 574",
                            new List<ITestInput> { 

                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("The best Credit Bureau score applicable for this application is below the minimum requirement for salaried applicants.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with an empirica score of 575",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "575")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.028"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory0")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with an empirica score of 576",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "576")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.028"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory0")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with an empirica score of -999",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "-999")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.028"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory0")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when checking the minimum empirica requirement&nbsp;for category 1", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried"),
                        new TestInput ("Household Income", "float", "100000"),
                        new TestInput ("Property Purchase Price", "float", "2400000"),
                        new TestInput ("Deposit Amount", "float", "600000"),
                        new TestInput ("Cash Amount Required", "float", "0"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "0"),
                        new TestInput ("Estimated Market Value of Property", "float", "0"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "574"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "False")
                    },
                    new List<IScenario> {
                        new Scenario("given an application with an empirica score of 574",
                            new List<ITestInput> { 

                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("The best Credit Bureau score applicable for this application is below the minimum requirement for salaried applicants.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with an empirica score of 575",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "575")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.032"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory1")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with an empirica score of 576",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "576")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.032"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory1")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with empirica score of -999",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "-999")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.032"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory1")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when checking the minimum empirica requirement&nbsp;for category 3", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried"),
                        new TestInput ("Household Income", "float", "100000"),
                        new TestInput ("Property Purchase Price", "float", "2000000"),
                        new TestInput ("Deposit Amount", "float", "350000"),
                        new TestInput ("Cash Amount Required", "float", "0"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "0"),
                        new TestInput ("Estimated Market Value of Property", "float", "0"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "599"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "False")
                    },
                    new List<IScenario> {
                        new Scenario("given an application with an empirica score of 599",
                            new List<ITestInput> { 

                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("The best Credit Bureau score applicable for this application is below the minimum requirement for salaried applicants.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with an empirica score of 600",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "600")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.037"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory3")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Warning Messages"),
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with an empirica score of 601",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "601")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.037"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory3")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with empirica score of -999",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "-999")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.037"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory3")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when checking the minimum empirica requirement&nbsp;for category 4", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried"),
                        new TestInput ("Household Income", "float", "100000"),
                        new TestInput ("Property Purchase Price", "float", "2000000"),
                        new TestInput ("Deposit Amount", "float", "250000"),
                        new TestInput ("Cash Amount Required", "float", "0"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "0"),
                        new TestInput ("Estimated Market Value of Property", "float", "0"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "609"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "False")
                    },
                    new List<IScenario> {
                        new Scenario("given an application with an empirica score of 609",
                            new List<ITestInput> { 

                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("The best Credit Bureau score applicable for this application is below the minimum requirement for salaried applicants.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with an empirica score of 610",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "610")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.039"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory4")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with an empirica score of 611",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "611")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.039"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory4")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with empirica score of -999",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "-999")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.039"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory4")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when checking the minimum empirica requirement&nbsp;for category 5", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried"),
                        new TestInput ("Household Income", "float", "100000"),
                        new TestInput ("Property Purchase Price", "float", "1800000"),
                        new TestInput ("Deposit Amount", "float", "100000"),
                        new TestInput ("Cash Amount Required", "float", "0"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "0"),
                        new TestInput ("Estimated Market Value of Property", "float", "0"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "629"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "False")
                    },
                    new List<IScenario> {
                        new Scenario("given an application with an empirica score of 629",
                            new List<ITestInput> { 

                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("The best Credit Bureau score applicable for this application is below the minimum requirement for salaried applicants.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with an empirica score of 630",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "630")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.039"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory5")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with an empirica score of 631",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "631")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.039"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory5")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Warning Messages"),
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with empirica score of -999",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "-999")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.039"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory5")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    })
            };
        }
    }
}


 