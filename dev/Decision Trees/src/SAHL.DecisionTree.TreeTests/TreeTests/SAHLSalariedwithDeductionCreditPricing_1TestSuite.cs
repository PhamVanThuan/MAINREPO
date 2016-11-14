using System.Collections.Generic;
using SAHL.Tools.DecisionTree.TestRunner.Interfaces;
using SAHL.DecisionTree.TreeTests.Models;

namespace SAHL.DecisionTree.TreeTests
{
    public class SAHLSalariedwithDeductionCreditPricing_1TestSuite : ITestSuite
    {
        public IEnumerable<ITestCase> TestCases { get; set; }
    
        public SAHLSalariedwithDeductionCreditPricing_1TestSuite()
        {
            this.TestCases = new List<ITestCase> 
            {
                new TestCase("when checking the minimum empirica requirement for category 0", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction"),
                        new TestInput ("Household Income", "float", "25000"),
                        new TestInput ("Property Purchase Price", "float", "0"),
                        new TestInput ("Deposit Amount", "float", "0"),
                        new TestInput ("Cash Amount Required", "float", "150000"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "500000"),
                        new TestInput ("Estimated Market Value of Property", "float", "1000000"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "575"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "False")
                    },
                    new List<IScenario> {
                        new Scenario("given a salary deduction application with an empirica score of 574",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "574")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a salary deduction application with an empirica score of 576",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "576")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory0"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a qualifying salary deduction application with an unknown empirica score",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "-999")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory0"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a salary deduction application with an empirica score of 575",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "575")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory0")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when checking the minimum empirica requirement for category 1", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction"),
                        new TestInput ("Household Income", "float", "25000"),
                        new TestInput ("Property Purchase Price", "float", "0"),
                        new TestInput ("Deposit Amount", "float", "0"),
                        new TestInput ("Cash Amount Required", "float", "250000"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "500000"),
                        new TestInput ("Estimated Market Value of Property", "float", "1000000"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "575"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "False")
                    },
                    new List<IScenario> {
                        new Scenario("given a salary deduction application with an empirica score of 574",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "574")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a salary deduction application with an empirica score of 575",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "575")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory1")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a salary deduction application with an empirica score of 576",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "576")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory1")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a qualifying salary deduction application with an unknown empirica score",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "-999")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory1")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when checking the minimum empirica requirement for category 3", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction"),
                        new TestInput ("Household Income", "float", "30000"),
                        new TestInput ("Property Purchase Price", "float", "0"),
                        new TestInput ("Deposit Amount", "float", "0"),
                        new TestInput ("Cash Amount Required", "float", "325000"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "500000"),
                        new TestInput ("Estimated Market Value of Property", "float", "1000000"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "575"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "False")
                    },
                    new List<IScenario> {
                        new Scenario("given a salary deduction application with an empirica score of 574",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "574")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a salary deduction application with an empirica score of 575",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "575")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory3")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a salary deduction application with an empirica score of 576",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "576")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory3")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a qualifying salary deduction application with an unknown empirica score",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "-999")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory3")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when checking the minimum empirica requirement for category 4", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction"),
                        new TestInput ("Household Income", "float", "30000"),
                        new TestInput ("Property Purchase Price", "float", "0"),
                        new TestInput ("Deposit Amount", "float", "0"),
                        new TestInput ("Cash Amount Required", "float", "375000"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "500000"),
                        new TestInput ("Estimated Market Value of Property", "float", "1000000"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "575"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "False")
                    },
                    new List<IScenario> {
                        new Scenario("given a salary deduction application with an empirica score of 574",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "574")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a salary deduction application with an empirica score of 575",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "575")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory4")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a salary deduction application with an empirica score of 576",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "576")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory4")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a qualifying salary deduction application with an unknown empirica score",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "-999")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory4")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when checking the minimum empirica requirement for category 5", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction"),
                        new TestInput ("Household Income", "float", "35000"),
                        new TestInput ("Property Purchase Price", "float", "0"),
                        new TestInput ("Deposit Amount", "float", "0"),
                        new TestInput ("Cash Amount Required", "float", "425000"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "500000"),
                        new TestInput ("Estimated Market Value of Property", "float", "1000000"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "575"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "False")
                    },
                    new List<IScenario> {
                        new Scenario("given a salary deduction application with an empirica score of 574",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "574")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a salary deduction application with an empirica score of 575",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "575")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory5")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a salary deduction application with an empirica score of 576",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "576")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory5")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a qualifying salary deduction application with an unknown empirica score",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "-999")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory5")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when checking the minimum empirica requirement for category 10", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction"),
                        new TestInput ("Household Income", "float", "40000"),
                        new TestInput ("Property Purchase Price", "float", "1000000"),
                        new TestInput ("Deposit Amount", "float", "25000"),
                        new TestInput ("Cash Amount Required", "float", "0"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "0"),
                        new TestInput ("Estimated Market Value of Property", "float", "1000000"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "575"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "False")
                    },
                    new List<IScenario> {
                        new Scenario("given a salary deduction application with an empirica score of 574",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "574")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a salary deduction application with an empirica score of 575",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "575")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory10")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a salary deduction application with an empirica score of 576",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "576")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory10")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a qualifying salary deduction application with an unknown empirica score",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "-999")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory10")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when checking the minimum income requirements for category 0", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction"),
                        new TestInput ("Household Income", "float", "13000"),
                        new TestInput ("Property Purchase Price", "float", "0"),
                        new TestInput ("Deposit Amount", "float", "0"),
                        new TestInput ("Cash Amount Required", "float", "225000"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "100000"),
                        new TestInput ("Estimated Market Value of Property", "float", "500000"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "575"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "False")
                    },
                    new List<IScenario> {
                        new Scenario("given a qualifying application with a household income of R12999.99",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "12999.99")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Total income is below the required minimum for salaried with deduction applicants.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a qualifying application with a household income of R13000.00",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "13000.00")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory0")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a qualifying application with a household income of R13000.01",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "13000.01")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory0")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when checking the minimum income requirements for category 1", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction"),
                        new TestInput ("Household Income", "float", "13000"),
                        new TestInput ("Property Purchase Price", "float", "0"),
                        new TestInput ("Deposit Amount", "float", "0"),
                        new TestInput ("Cash Amount Required", "float", "275000"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "100000"),
                        new TestInput ("Estimated Market Value of Property", "float", "500000"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "575"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "False")
                    },
                    new List<IScenario> {
                        new Scenario("given a qualifying application with a household income of R12999.99",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "12999.99")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Total income is below the required minimum for salaried with deduction applicants.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a qualifying application with a household income of R13000.00",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "13000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory1")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a qualifying application with a household income of R13000.01",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "13000.01")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory1")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when checking the minimum income requirements for category 3", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction"),
                        new TestInput ("Household Income", "float", "18600"),
                        new TestInput ("Property Purchase Price", "float", "0"),
                        new TestInput ("Deposit Amount", "float", "0"),
                        new TestInput ("Cash Amount Required", "float", "315000"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "100000"),
                        new TestInput ("Estimated Market Value of Property", "float", "500000"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "600"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "False")
                    },
                    new List<IScenario> {
                        new Scenario("given a qualifying application with a household income of R18599.99",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "18599.99")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Total income is below the required minimum for salaried with deduction applicants.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a qualifying application with a household income of R18600.00",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "18600.00")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory3")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a qualifying application with a household income of R18600.01",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "18600.01")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory3")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when checking the minimum income requirements for category 4", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction"),
                        new TestInput ("Household Income", "float", "18600"),
                        new TestInput ("Property Purchase Price", "float", "0"),
                        new TestInput ("Deposit Amount", "float", "0"),
                        new TestInput ("Cash Amount Required", "float", "337500"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "100000"),
                        new TestInput ("Estimated Market Value of Property", "float", "500000"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "610"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "False")
                    },
                    new List<IScenario> {
                        new Scenario("given a qualifying application with a household income of R18599.99",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "18599.99")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Total income is below the required minimum for salaried with deduction applicants.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a qualifying application with a household income of R18600.00",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "18600.00")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory4")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a qualifying application with a household income of R18600.01",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "18600.01")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory4")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when checking the minimum income requirements for category 5", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction"),
                        new TestInput ("Household Income", "float", "18600"),
                        new TestInput ("Property Purchase Price", "float", "0"),
                        new TestInput ("Deposit Amount", "float", "0"),
                        new TestInput ("Cash Amount Required", "float", "362500"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "100000"),
                        new TestInput ("Estimated Market Value of Property", "float", "500000"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "620"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "False")
                    },
                    new List<IScenario> {
                        new Scenario("given a qualifying application with a household income of R18599.99",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "18599.99")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Total income is below the required minimum for salaried with deduction applicants.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a qualifying application with a household income of R18600.00",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "18600.00")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory5")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a qualifying application with a household income of R18600.01",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "18600.01")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory5")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when checking the minimum income requirements for category 10", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction"),
                        new TestInput ("Household Income", "float", "18600"),
                        new TestInput ("Property Purchase Price", "float", "500000"),
                        new TestInput ("Deposit Amount", "float", "0"),
                        new TestInput ("Cash Amount Required", "float", "0"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "0"),
                        new TestInput ("Estimated Market Value of Property", "float", "500000"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "620"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "False")
                    },
                    new List<IScenario> {
                        new Scenario("given a qualifying application with a household income of R18599.99",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "18599.99")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Total income is below the required minimum for salaried with deduction applicants.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a qualifying application with a household income of R18600.00",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "18600.00")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory10")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a qualifying application with a household income of R18600.01",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "18600.01")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory10")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when calculating the loan amount", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction"),
                        new TestInput ("Household Income", "float", "18600"),
                        new TestInput ("Property Purchase Price", "float", "500000"),
                        new TestInput ("Deposit Amount", "float", "100000"),
                        new TestInput ("Cash Amount Required", "float", "0"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "0"),
                        new TestInput ("Estimated Market Value of Property", "float", "0"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "575"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "False")
                    },
                    new List<IScenario> {
                        new Scenario("given a new purchase application",
                            new List<ITestInput> { 

                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Loan Amount", "to equal", "float", "400000")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a switch application",
                            new List<ITestInput> { 
                                new TestInput("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch"),
                                new TestInput("Cash Amount Required", "float", "175000"),
                                new TestInput("Current Mortgage Loan Balance", "float", "200000"),
                                new TestInput("Estimated Market Value of Property", "float", "500000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Loan Amount", "to equal", "float", "375000")
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
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction"),
                        new TestInput ("Household Income", "float", "18600"),
                        new TestInput ("Property Purchase Price", "float", "500000"),
                        new TestInput ("Deposit Amount", "float", "100000"),
                        new TestInput ("Cash Amount Required", "float", "0"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "0"),
                        new TestInput ("Estimated Market Value of Property", "float", "0"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "575"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "False")
                    },
                    new List<IScenario> {
                        new Scenario("given a new purchase application",
                            new List<ITestInput> { 

                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Loan to Value", "to equal", "float", "0.8"),
 
                                new TestOutput("Loan to Value as Percent", "to equal", "string", "80.00 %")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a switch application",
                            new List<ITestInput> { 
                                new TestInput("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch"),
                                new TestInput("Cash Amount Required", "float", "100000"),
                                new TestInput("Current Mortgage Loan Balance", "float", "300000"),
                                new TestInput("Estimated Market Value of Property", "float", "500000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Loan to Value", "to equal", "float", "0.8"),
 
                                new TestOutput("Loan to Value as Percent", "to equal", "string", "80.00 %")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when calculating the PTI ratio", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction"),
                        new TestInput ("Household Income", "float", "18904"),
                        new TestInput ("Property Purchase Price", "float", "500000"),
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
                        new Scenario("given a new purchase application",
                            new List<ITestInput> { 

                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.2"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "20.0%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a switch application",
                            new List<ITestInput> { 
                                new TestInput("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch"),
                                new TestInput("Cash Amount Required", "float", "250000"),
                                new TestInput("Current Mortgage Loan Balance", "float", "150000"),
                                new TestInput("Estimated Market Value of Property", "float", "500000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.2"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "20.0%")
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
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction"),
                        new TestInput ("Household Income", "float", "18600"),
                        new TestInput ("Property Purchase Price", "float", "500000"),
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
                        new Scenario("given a new purchase application",
                            new List<ITestInput> { 

                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Installment in Rands", "to equal", "string", "3780.92")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a switch application",
                            new List<ITestInput> { 
                                new TestInput("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch"),
                                new TestInput("Cash Amount Required", "float", "300000"),
                                new TestInput("Current Mortgage Loan Balance", "float", "100000"),
                                new TestInput("Estimated Market Value of Property", "float", "500000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Installment in Rands", "to equal", "string", "3780.92")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when salary deduction loan amount requirements are not met", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction"),
                        new TestInput ("Household Income", "float", "18600"),
                        new TestInput ("Property Purchase Price", "float", "0"),
                        new TestInput ("Deposit Amount", "float", "0"),
                        new TestInput ("Cash Amount Required", "float", "50000"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "100000"),
                        new TestInput ("Estimated Market Value of Property", "float", "350000"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "575"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "False")
                    },
                    new List<IScenario> {
                        new Scenario("given a switch application with a loan amount of R 149,999.99",
                            new List<ITestInput> { 
                                new TestInput("Cash Amount Required", "float", "49999.99")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Loan amount requested is below the product minimum.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 149,999.99",
                            new List<ITestInput> { 
                                new TestInput("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase"),
                                new TestInput("Property Purchase Price", "float", "149999.99")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Loan amount requested is below the product minimum.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a switch application with a loan amount of R 5,000,000.01",
                            new List<ITestInput> { 
                                new TestInput("Cash Amount Required", "float", "2500000.01"),
                                new TestInput("Current Mortgage Loan Balance", "float", "2500000.00")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Loan amount requested is above the product maximum.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 5,000,000.01",
                            new List<ITestInput> { 
                                new TestInput("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase"),
                                new TestInput("Property Purchase Price", "float", "5000000.01")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Loan amount requested is above the product maximum.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when ensuring an application does not exceed the salary deduction LTV category limits", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction"),
                        new TestInput ("Household Income", "float", "13000"),
                        new TestInput ("Property Purchase Price", "float", "500000"),
                        new TestInput ("Deposit Amount", "float", "150000.01"),
                        new TestInput ("Cash Amount Required", "float", "0"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "0"),
                        new TestInput ("Estimated Market Value of Property", "float", "500000"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "620"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "False")
                    },
                    new List<IScenario> {
                        new Scenario("given a category 0 application with an LTV of 69.90 %",
                            new List<ITestInput> { 

                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Loan to Value", "to equal", "float", "0.699"),
 
                                new TestOutput("Loan to Value as Percent", "to equal", "string", "69.90 %"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory0")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a category 1 application with an LTV of 70.00 %",
                            new List<ITestInput> { 
                                new TestInput("Deposit Amount", "float", "150000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Loan to Value", "to equal", "float", "0.7"),
 
                                new TestOutput("Loan to Value as Percent", "to equal", "string", "70.00 %"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory1")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a category 1 application with an LTV of 70.10 %",
                            new List<ITestInput> { 
                                new TestInput("Deposit Amount", "float", "149500")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Loan to Value", "to equal", "float", "0.701"),
 
                                new TestOutput("Loan to Value as Percent", "to equal", "string", "70.10 %"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory1")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a category 1 application with an LTV of 79.90 %",
                            new List<ITestInput> { 
                                new TestInput("Deposit Amount", "float", "100001")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Loan to Value", "to equal", "float", "0.799"),
 
                                new TestOutput("Loan to Value as Percent", "to equal", "string", "79.90 %"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory1")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a category 3 application with an LTV of 80.00 %",
                            new List<ITestInput> { 
                                new TestInput("Deposit Amount", "float", "100000"),
                                new TestInput("Household Income", "float", "18600")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Loan to Value", "to equal", "float", "0.8"),
 
                                new TestOutput("Loan to Value as Percent", "to equal", "string", "80.00 %"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory3")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a category 3 application with an LTV of 80.10 %",
                            new List<ITestInput> { 
                                new TestInput("Deposit Amount", "float", "99500"),
                                new TestInput("Household Income", "float", "18600")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Loan to Value", "to equal", "float", "0.801"),
 
                                new TestOutput("Loan to Value as Percent", "to equal", "string", "80.10 %"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory3")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a category 3 application with an LTV of 84.90 %",
                            new List<ITestInput> { 
                                new TestInput("Deposit Amount", "float", "75001"),
                                new TestInput("Household Income", "float", "18600")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Loan to Value", "to equal", "float", "0.849"),
 
                                new TestOutput("Loan to Value as Percent", "to equal", "string", "84.90 %"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory3")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a category 4 application with an LTV of 85.00 %",
                            new List<ITestInput> { 
                                new TestInput("Deposit Amount", "float", "75000"),
                                new TestInput("Household Income", "float", "18600")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Loan to Value", "to equal", "float", "0.85"),
 
                                new TestOutput("Loan to Value as Percent", "to equal", "string", "85.00 %"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory4")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a category 4 application with an LTV of 85.10 %",
                            new List<ITestInput> { 
                                new TestInput("Deposit Amount", "float", "74500"),
                                new TestInput("Household Income", "float", "18600")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Loan to Value", "to equal", "float", "0.851"),
 
                                new TestOutput("Loan to Value as Percent", "to equal", "string", "85.10 %"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory4")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a category 4 application with an LTV of 89.90 %",
                            new List<ITestInput> { 
                                new TestInput("Deposit Amount", "float", "50001"),
                                new TestInput("Household Income", "float", "18600")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Loan to Value", "to equal", "float", "0.899"),
 
                                new TestOutput("Loan to Value as Percent", "to equal", "string", "89.90 %"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory4")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a category 5 application with an LTV of 90.00 %",
                            new List<ITestInput> { 
                                new TestInput("Deposit Amount", "float", "50000"),
                                new TestInput("Household Income", "float", "18600")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Loan to Value", "to equal", "float", "0.9"),
 
                                new TestOutput("Loan to Value as Percent", "to equal", "string", "90.00 %"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory5")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a category 5 application with an LTV of 90.10 %",
                            new List<ITestInput> { 
                                new TestInput("Deposit Amount", "float", "49500"),
                                new TestInput("Household Income", "float", "18600")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Loan to Value", "to equal", "float", "0.901"),
 
                                new TestOutput("Loan to Value as Percent", "to equal", "string", "90.10 %"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory5")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a category 5 application with an LTV of 94.90 %",
                            new List<ITestInput> { 
                                new TestInput("Deposit Amount", "float", "25001"),
                                new TestInput("Household Income", "float", "18600")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Loan to Value", "to equal", "float", "0.949"),
 
                                new TestOutput("Loan to Value as Percent", "to equal", "string", "94.90 %"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory5")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a category 10 application with an LTV of 95.00 %",
                            new List<ITestInput> { 
                                new TestInput("Deposit Amount", "float", "25000"),
                                new TestInput("Household Income", "float", "18600")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Loan to Value", "to equal", "float", "0.95"),
 
                                new TestOutput("Loan to Value as Percent", "to equal", "string", "95.00 %"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory10")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a category 10 application with an LTV of 95.10 %",
                            new List<ITestInput> { 
                                new TestInput("Deposit Amount", "float", "24500"),
                                new TestInput("Household Income", "float", "18600")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Loan to Value", "to equal", "float", "0.951"),
 
                                new TestOutput("Loan to Value as Percent", "to equal", "string", "95.10 %"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory10")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a category 10 new purchase application with an LTV of 99.90 %",
                            new List<ITestInput> { 
                                new TestInput("Deposit Amount", "float", "500"),
                                new TestInput("Household Income", "float", "18600")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Loan to Value", "to equal", "float", "0.999"),
 
                                new TestOutput("Loan to Value as Percent", "to equal", "string", "99.90 %"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory10")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a category 10 switch application with an LTV of 99.90 %",
                            new List<ITestInput> { 
                                new TestInput("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch"),
                                new TestInput("Current Mortgage Loan Balance", "float", "400000"),
                                new TestInput("Cash Amount Required", "float", "99500"),
                                new TestInput("Household Income", "float", "18600")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your loan amount as a percentage of estimated property value (LTV) would be 99.90 %, which is greater than or equal to the maximum of 95.0%. In order to reduce your LTV, the loan amount would need to be lowered by decreasing the cash required by R24501 or more.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a category 10 new purchase application with an LTV of 100.00 %",
                            new List<ITestInput> { 
                                new TestInput("Deposit Amount", "float", "0"),
                                new TestInput("Household Income", "float", "18600")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Loan to Value", "to equal", "float", "1"),
 
                                new TestOutput("Loan to Value as Percent", "to equal", "string", "100.00 %")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a category 10 switch application with an LTV of 100.00 %",
                            new List<ITestInput> { 
                                new TestInput("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch"),
                                new TestInput("Current Mortgage Loan Balance", "float", "400000"),
                                new TestInput("Cash Amount Required", "float", "100000"),
                                new TestInput("Household Income", "float", "18600")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your loan amount as a percentage of estimated property value (LTV) would be 100.00 %, which is greater than or equal to the maximum of 95.0%. In order to reduce your LTV, the loan amount would need to be lowered by decreasing the cash required by R25001 or more.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an application with an LTV of 100.10 %",
                            new List<ITestInput> { 
                                new TestInput("Deposit Amount", "float", "-501"),
                                new TestInput("Household Income", "float", "18600")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Loan to Value", "to equal", "float", "1.001"),
 
                                new TestOutput("Loan to Value as Percent", "to equal", "string", "100.10 %")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your loan amount as a percentage of purchase price (LTV) would be 100.10 %, which is greater than or equal to the maximum of 100.0%. In order to reduce your LTV, the loan amount would need to be lowered by increasing the deposit by R502 or more.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when a new purchase application falls into category 0", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction"),
                        new TestInput ("Household Income", "float", "48522.11"),
                        new TestInput ("Property Purchase Price", "float", "2650000"),
                        new TestInput ("Deposit Amount", "float", "850000.01"),
                        new TestInput ("Cash Amount Required", "float", "0"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "0"),
                        new TestInput ("Estimated Market Value of Property", "float", "0"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "575"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "False")
                    },
                    new List<IScenario> {
                        new Scenario("given a new purchase application with a loan amount of R 1,799,999.99 and PTI ratio of 32.9% because of a 10.0% variance on the 30.0% limit",
                            new List<ITestInput> { 

                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory0"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.329"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "32.9%"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.028")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 1,799,999.99 and PTI ratio of 33.0% because of a 10.0% variance on the 30.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "48376.54")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.33"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "33.0%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 33.0% and is greater than or equal to the maximum of 33.0%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1799998 or alternatively additional income so that total income is at least R48378.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 1,799,999.99 and PTI ratio of 33.1% because of a 10.0% variance on the 30.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "48230.39")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.331"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "33.1%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 33.1% and is greater than or equal to the maximum of 33.0%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1794560 or alternatively additional income so that total income is at least R48378.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 1,800,000.00 and PTI ratio of 32.9% because of a 10.0% variance on the 30.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Deposit Amount", "float", "850000"),
                                new TestInput("Household Income", "float", "48522.11")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory0"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.329"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "32.9%"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.028")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 1,800,000.00 and PTI ratio of 33.0% because of a 10.0% variance on the 30.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Deposit Amount", "float", "850000"),
                                new TestInput("Household Income", "float", "48376.54")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.33"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "33.0%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 33.0% and is greater than or equal to the maximum of 33.0%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1799998 or alternatively additional income so that total income is at least R48378.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 1,800,000.00 and PTI ratio of 33.1% because of a 10.0% variance on the 30.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Deposit Amount", "float", "850000"),
                                new TestInput("Household Income", "float", "48230.39")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.331"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "33.1%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 33.1% and is greater than or equal to the maximum of 33.0%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1794560 or alternatively additional income so that total income is at least R48378.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 1,800,000.01 and PTI ratio of 32.9% because of a 10.0% variance on the 30.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Deposit Amount", "float", "849999.99"),
                                new TestInput("Household Income", "float", "48522.11")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory0"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.329"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "32.9%"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.028")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 1,800,000.01 and PTI ratio of 33.0% because of a 10.0% variance on the 30.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Deposit Amount", "float", "849999.99"),
                                new TestInput("Household Income", "float", "48376.54")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.33"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "33.0%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 33.0% and is greater than or equal to the maximum of 33.0%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1799998 or alternatively additional income so that total income is at least R48378.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 1,800,000.01 and PTI ratio of 33.1% because of a 10.0% variance on the 30.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Deposit Amount", "float", "849999.99"),
                                new TestInput("Household Income", "float", "48230.39")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.331"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "33.1%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 33.1% and is greater than or equal to the maximum of 33.0%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1794560 or alternatively additional income so that total income is at least R48378.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 2,749,999.99 and PTI ratio of 32.9% because of a 10.0% variance on the 30.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "4000000"),
                                new TestInput("Deposit Amount", "float", "1250000.01"),
                                new TestInput("Household Income", "float", "74131.02")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory0"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.329"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "32.9%"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.028")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 2,749,999.99 and PTI ratio of 33.0% because of a 10.0% variance on the 30.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "4000000"),
                                new TestInput("Deposit Amount", "float", "1250000.01"),
                                new TestInput("Household Income", "float", "73906.39")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.33"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "33.0%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 33.0% and is greater than or equal to the maximum of 33.0%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R2749916 or alternatively additional income so that total income is at least R73910.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 2,749,999.99 and PTI ratio of 33.1% because of a 10.0% variance on the 30.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "4000000"),
                                new TestInput("Deposit Amount", "float", "1250000.01"),
                                new TestInput("Household Income", "float", "73683.11")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.331"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "33.1%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 33.1% and is greater than or equal to the maximum of 33.0%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R2741608 or alternatively additional income so that total income is at least R73910.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 2,750,000.00 and PTI ratio of 32.9% because of a 10.0% variance on the 30.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "4000000"),
                                new TestInput("Deposit Amount", "float", "1250000"),
                                new TestInput("Household Income", "float", "74131.02")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory0"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.329"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "32.9%"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.028")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 2,750,000.00 and PTI ratio of 33.0% because of a 10.0% variance on the 30.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "4000000"),
                                new TestInput("Deposit Amount", "float", "1250000"),
                                new TestInput("Household Income", "float", "73906.39")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.33"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "33.0%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 33.0% and is greater than or equal to the maximum of 33.0%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R2749916 or alternatively additional income so that total income is at least R73910.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 2,750,000.00 and PTI ratio of 33.1% because of a 10.0% variance on the 30.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "4000000"),
                                new TestInput("Deposit Amount", "float", "1250000"),
                                new TestInput("Household Income", "float", "73683.11")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.331"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "33.1%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 33.1% and is greater than or equal to the maximum of 33.0%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R2741608 or alternatively additional income so that total income is at least R73910.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 2,750,000.01 and PTI ratio of 27.4% because of a 10.0% variance on the 25.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "4000000"),
                                new TestInput("Deposit Amount", "float", "1249999.99"),
                                new TestInput("Household Income", "float", "89010.79")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.274"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.4%"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory0"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.028")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 2,750,000.01 and PTI ratio of 27.5% because of a 10.0% variance on the 25.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "4000000"),
                                new TestInput("Deposit Amount", "float", "1249999.99"),
                                new TestInput("Household Income", "float", "88687.13")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.275"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.5%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.5% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R2749899 or alternatively additional income so that total income is at least R88691.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 2,750,000.01 and PTI ratio of 27.6% because of a 10.0% variance on the 25.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "4000000"),
                                new TestInput("Deposit Amount", "float", "1249999.99"),
                                new TestInput("Household Income", "float", "88365.81")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.276"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.6%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.6% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R2739936 or alternatively additional income so that total income is at least R88691.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 4,999,999.99 and PTI ratio of 27.4% because of a 10.0% variance on the 25.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "7500000"),
                                new TestInput("Deposit Amount", "float", "2500000.01"),
                                new TestInput("Household Income", "float", "161837.81")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory0"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.274"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.4%"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.028")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 4,999,999.99 and PTI ratio of 27.5% because of a 10.0% variance on the 25.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "7500000"),
                                new TestInput("Deposit Amount", "float", "2500000.01"),
                                new TestInput("Household Income", "float", "161249.33")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.275"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.5%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.5% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R4999817 or alternatively additional income so that total income is at least R161256.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 4,999,999.99 and PTI ratio of 27.6% because of a 10.0% variance on the 25.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "7500000"),
                                new TestInput("Deposit Amount", "float", "2500000.01"),
                                new TestInput("Household Income", "float", "160665.12")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.276"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.6%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.6% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R4981703 or alternatively additional income so that total income is at least R161256.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 5,000,000.00 and PTI ratio of 27.4% because of a 10.0% variance on the 25.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "7500000"),
                                new TestInput("Deposit Amount", "float", "2500000"),
                                new TestInput("Household Income", "float", "161837.81")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory0"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.274"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.4%"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.028")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 5,000,000.00 and PTI ratio of 27.5% because of a 10.0% variance on the 25.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "7500000"),
                                new TestInput("Deposit Amount", "float", "2500000"),
                                new TestInput("Household Income", "float", "161249.33")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.275"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.5%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.5% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R4999817 or alternatively additional income so that total income is at least R161256.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 5,000,000.00 and PTI ratio of 27.6% because of a 10.0% variance on the 25.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "7500000"),
                                new TestInput("Deposit Amount", "float", "2500000"),
                                new TestInput("Household Income", "float", "160665.12")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.276"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.6%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.6% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R4981703 or alternatively additional income so that total income is at least R161256.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when a new purchase application falls into category 1", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction"),
                        new TestInput ("Household Income", "float", "49929.53"),
                        new TestInput ("Property Purchase Price", "float", "2450000"),
                        new TestInput ("Deposit Amount", "float", "650000.01"),
                        new TestInput ("Cash Amount Required", "float", "0"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "0"),
                        new TestInput ("Estimated Market Value of Property", "float", "0"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "575"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "False")
                    },
                    new List<IScenario> {
                        new Scenario("given a new purchase application with a loan amount of R 1,799,999.99 and PTI ratio of 32.9% because of a 10.0% variance on the 30.0% limit",
                            new List<ITestInput> { 

                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory1"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.329"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "32.9%"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.032")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 1,799,999.99 and PTI ratio of 33.0% because of a 10.0% variance on the 30.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "49778.23")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.33"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "33.0%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 33.0% and is greater than or equal to the maximum of 33.0%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1799944 or alternatively additional income so that total income is at least R49781.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 1,799,999.99 and PTI ratio of 33.1% because of a 10.0% variance on the 30.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "49627.85")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.331"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "33.1%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 33.1% and is greater than or equal to the maximum of 33.0%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1794506 or alternatively additional income so that total income is at least R49781.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 1,800,000.00 and PTI ratio of 32.9% because of a 10.0% variance on the 30.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Deposit Amount", "float", "650000"),
                                new TestInput("Household Income", "float", "49929.53")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory1"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.329"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "32.9%"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.032")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 1,800,000.00 and PTI ratio of 33.0% because of a 10.0% variance on the 30.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Deposit Amount", "float", "650000"),
                                new TestInput("Household Income", "float", "49778.23")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.33"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "33.0%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 33.0% and is greater than or equal to the maximum of 33.0%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1799944 or alternatively additional income so that total income is at least R49781.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 1,800,000.00 and PTI ratio of 33.1% because of a 10.0% variance on the 30.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Deposit Amount", "float", "650000"),
                                new TestInput("Household Income", "float", "49627.85")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.331"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "33.1%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 33.1% and is greater than or equal to the maximum of 33.0%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1794506 or alternatively additional income so that total income is at least R49781.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 1,800,000.01 and PTI ratio of 27.4% because of a 10.0% variance on the 25.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Deposit Amount", "float", "649999.99"),
                                new TestInput("Household Income", "float", "59951.52")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory1"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.274"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.4%"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.032")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 1,800,000.01 and PTI ratio of 27.5% because of a 10.0% variance on the 25.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Deposit Amount", "float", "649999.99"),
                                new TestInput("Household Income", "float", "59733.52")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.275"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.5%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.5% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1799933 or alternatively additional income so that total income is at least R59737.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 1,800,000.01 and PTI ratio of 27.6% because of a 10.0% variance on the 25.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Deposit Amount", "float", "649999.99"),
                                new TestInput("Household Income", "float", "59517.10")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.276"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.6%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.6% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1793412 or alternatively additional income so that total income is at least R59737.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 2,749,999.99 and PTI ratio of 27.4% because of a 10.0% variance on the 25.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "3500000"),
                                new TestInput("Deposit Amount", "float", "750000.01"),
                                new TestInput("Household Income", "float", "91592.60")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory1"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.274"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.4%"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.032")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 2,749,999.99 and PTI ratio of 27.5% because of a 10.0% variance on the 25.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "3500000"),
                                new TestInput("Deposit Amount", "float", "750000.01"),
                                new TestInput("Household Income", "float", "91259.55")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.275"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.5%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.5% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R2749899 or alternatively additional income so that total income is at least R91264.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 2,749,999.99 and PTI ratio of 27.6% because of a 10.0% variance on the 25.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "3500000"),
                                new TestInput("Deposit Amount", "float", "750000.01"),
                                new TestInput("Household Income", "float", "90928.91")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.276"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.6%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.6% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R2739936 or alternatively additional income so that total income is at least R91264.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 2,750,000.00 and PTI ratio of 27.4% because of a 10.0% variance on the 25.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "3500000"),
                                new TestInput("Deposit Amount", "float", "750000"),
                                new TestInput("Household Income", "float", "91592.60")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory1"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.274"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.4%"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.032")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 2,750,000.00 and PTI ratio of 27.5% because of a 10.0% variance on the 25.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "3500000"),
                                new TestInput("Deposit Amount", "float", "750000"),
                                new TestInput("Household Income", "float", "91259.55")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.275"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.5%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.5% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R2749899 or alternatively additional income so that total income is at least R91264.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 2,750,000.00 and PTI ratio of 27.6% because of a 10.0% variance on the 25.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "3500000"),
                                new TestInput("Deposit Amount", "float", "750000"),
                                new TestInput("Household Income", "float", "90928.91")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.276"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.6%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.6% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R2739936 or alternatively additional income so that total income is at least R91264.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 2,750,000.01 and PTI ratio of 27.4% because of a 10.0% variance on the 25.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "3500000"),
                                new TestInput("Deposit Amount", "float", "749999.99"),
                                new TestInput("Household Income", "float", "90300.94")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Loan Amount", "to equal", "float", "2750000.01")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when a new purchase application falls into category 3", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction"),
                        new TestInput ("Household Income", "float", "51714"),
                        new TestInput ("Property Purchase Price", "float", "2150000"),
                        new TestInput ("Deposit Amount", "float", "350000.01"),
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
                        new Scenario("given a new purchase application with a loan amount of R 1,799,999.99 and PTI ratio of 32.9% because of a 10.0% variance on the 30.0% limit",
                            new List<ITestInput> { 

                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory3"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.329"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "32.9%"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.037")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 1,799,999.99 and PTI ratio of 33.0% because of a 10.0% variance on the 30.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "51558")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.33"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "33.0%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 33.0% and is greater than or equal to the maximum of 33.0%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1799999 or alternatively additional income so that total income is at least R51559.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 1,799,999.99 and PTI ratio of 33.1% because of a 10.0% variance on the 30.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "51402")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.331"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "33.1%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 33.1% and is greater than or equal to the maximum of 33.0%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1794552 or alternatively additional income so that total income is at least R51559.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 1,800,000.00 and PTI ratio of 32.9% because of a 10.0% variance on the 30.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Deposit Amount", "float", "350000"),
                                new TestInput("Household Income", "float", "51714")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory3"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.329"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "32.9%"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.037")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 1,800,000.00 and PTI ratio of 33.0% because of a 10.0% variance on the 30.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Deposit Amount", "float", "350000"),
                                new TestInput("Household Income", "float", "51558")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.33"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "33.0%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 33.0% and is greater than or equal to the maximum of 33.0%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1799999 or alternatively additional income so that total income is at least R51559.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 1,800,000.00 and PTI ratio of 33.1% because of a 10.0% variance on the 30.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Deposit Amount", "float", "350000"),
                                new TestInput("Household Income", "float", "51402")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.331"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "33.1%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 33.1% and is greater than or equal to the maximum of 33.0%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1794552 or alternatively additional income so that total income is at least R51559.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 1,800,000.01 and PTI ratio of 27.4% because of a 10.0% variance on the 25.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Deposit Amount", "float", "349999.99"),
                                new TestInput("Household Income", "float", "62095")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory3"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.274"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.4%"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.037")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 1,800,000.01 and PTI ratio of 27.5% because of a 10.0% variance on the 25.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Deposit Amount", "float", "349999.99"),
                                new TestInput("Household Income", "float", "61869")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.275"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.5%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.5% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1799981 or alternatively additional income so that total income is at least R61871.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 1,800,000.01 and PTI ratio of 27.6% because of a 10.0% variance on the 25.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Deposit Amount", "float", "349999.99"),
                                new TestInput("Household Income", "float", "61645")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.276"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.6%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.6% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1793464 or alternatively additional income so that total income is at least R61871.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 2,749,999.99 and PTI ratio of 27.4% because of a 10.0% variance on the 25.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "3250000"),
                                new TestInput("Deposit Amount", "float", "500000.01"),
                                new TestInput("Household Income", "float", "94867")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory3"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.274"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.4%"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.037")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 2,749,999.99 and PTI ratio of 27.5% because of a 10.0% variance on the 25.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "3250000"),
                                new TestInput("Deposit Amount", "float", "500000.01"),
                                new TestInput("Household Income", "float", "94522")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.275"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.5%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.5% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R2749970 or alternatively additional income so that total income is at least R94524.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 2,749,999.99 and PTI ratio of 27.6% because of a 10.0% variance on the 25.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "3250000"),
                                new TestInput("Deposit Amount", "float", "500000.01"),
                                new TestInput("Household Income", "float", "94180")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.276"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.6%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.6% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R2740020 or alternatively additional income so that total income is at least R94524.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 2,750,000.00 and PTI ratio of 27.4% because of a 10.0% variance on the 25.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "3250000"),
                                new TestInput("Deposit Amount", "float", "500000"),
                                new TestInput("Household Income", "float", "94867")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory3"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.274"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.4%"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.037")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R&nbsp;2,750,000.00&nbsp;and PTI ratio of 27.5% because of a 10.0% variance on the 25.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "3250000"),
                                new TestInput("Deposit Amount", "float", "500000"),
                                new TestInput("Household Income", "float", "94522")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.275"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.5%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.5% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R2749970 or alternatively additional income so that total income is at least R94524.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R&nbsp;2,750,000.00&nbsp;and PTI ratio of 27.6% because of a 10.0% variance on the 25.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "3250000"),
                                new TestInput("Deposit Amount", "float", "500000"),
                                new TestInput("Household Income", "float", "94180")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.276"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.6%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.6% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R2740020 or alternatively additional income so that total income is at least R94524.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 2,750,000.01 and PTI ratio of 27.4% because of a 10.0% variance on the 25.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "3250000"),
                                new TestInput("Deposit Amount", "float", "499999.99"),
                                new TestInput("Household Income", "float", "94867")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Loan Amount", "to equal", "float", "2750000.01")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Loan amount requested is above the product maximum.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when a new purchase application falls into category 4", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction"),
                        new TestInput ("Household Income", "float", "52435"),
                        new TestInput ("Property Purchase Price", "float", "2050000"),
                        new TestInput ("Deposit Amount", "float", "250000.01"),
                        new TestInput ("Cash Amount Required", "float", "0"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "0"),
                        new TestInput ("Estimated Market Value of Property", "float", "0"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "610"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "False")
                    },
                    new List<IScenario> {
                        new Scenario("given a new purchase application with a loan amount of R 1,799,999.99 and PTI ratio of 32.9% because of a 10.0% variance on the 30.0% limit",
                            new List<ITestInput> { 

                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory4"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.329"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "32.9%"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.039")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 1,799,999.99 and PTI ratio of 33.0% because of a 10.0% variance on the 30.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "52276")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.33"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "33.0%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 33.0% and is greater than or equal to the maximum of 33.0%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1799976 or alternatively additional income so that total income is at least R52278.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 1,799,999.99 and PTI ratio of 33.1% because of a 10.0% variance on the 30.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "52118")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.331"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "33.1%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 33.1% and is greater than or equal to the maximum of 33.0%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1794536 or alternatively additional income so that total income is at least R52278.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 1,800,000.00 and PTI ratio of 32.9% because of a 10.0% variance on the 30.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Deposit Amount", "float", "250000"),
                                new TestInput("Household Income", "float", "52435")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory4"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.329"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "32.9%"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.039")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 1,800,000.00 and PTI ratio of 33.0% because of a 10.0% variance on the 30.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Deposit Amount", "float", "250000"),
                                new TestInput("Household Income", "float", "52276")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.33"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "33.0%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 33.0% and is greater than or equal to the maximum of 33.0%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1799976 or alternatively additional income so that total income is at least R52278.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 1,800,000.00 and PTI ratio of 33.1% because of a 10.0% variance on the 30.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Deposit Amount", "float", "250000"),
                                new TestInput("Household Income", "float", "52118")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.331"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "33.1%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 33.1% and is greater than or equal to the maximum of 33.0%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1794536 or alternatively additional income so that total income is at least R52278.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 1,800,000.01 and PTI ratio of 27.4% because of a 10.0% variance on the 25.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Deposit Amount", "float", "249999.99"),
                                new TestInput("Household Income", "float", "62960")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory4"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.274"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.4%"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.039")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 1,800,000.01 and PTI ratio of 27.5% because of a 10.0% variance on the 25.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Deposit Amount", "float", "249999.99"),
                                new TestInput("Household Income", "float", "62731")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.275"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.5%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.5% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1799970 or alternatively additional income so that total income is at least R62733.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 1,800,000.01 and PTI ratio of 27.6% because of a 10.0% variance on the 25.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Deposit Amount", "float", "249999.99"),
                                new TestInput("Household Income", "float", "62504")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.276"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.6%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.6% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1793457 or alternatively additional income so that total income is at least R62733.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 2,749,999.99 and PTI ratio of 27.4% because of a 10.0% variance on the 25.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "3150000"),
                                new TestInput("Deposit Amount", "float", "400000.01"),
                                new TestInput("Household Income", "float", "96190")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory4"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.274"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.4%"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.039")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 2,749,999.99 and PTI ratio of 27.5% because of a 10.0% variance on the 25.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "3150000"),
                                new TestInput("Deposit Amount", "float", "400000.01"),
                                new TestInput("Household Income", "float", "95840")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.275"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.5%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.5% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R2749983 or alternatively additional income so that total income is at least R95842.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 2,749,999.99 and PTI ratio of 27.6% because of a 10.0% variance on the 25.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "3150000"),
                                new TestInput("Deposit Amount", "float", "400000.01"),
                                new TestInput("Household Income", "float", "95493")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.276"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.6%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.6% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R2740026 or alternatively additional income so that total income is at least R95842.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 2,750,000.00 and PTI ratio of 27.4% because of a 10.0% variance on the 25.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "3150000"),
                                new TestInput("Deposit Amount", "float", "400000"),
                                new TestInput("Household Income", "float", "96190")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.274"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.4%"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory4"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.039")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 2,750,000.00 and PTI ratio of 27.5% because of a 10.0% variance on the 25.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "3150000"),
                                new TestInput("Deposit Amount", "float", "400000"),
                                new TestInput("Household Income", "float", "95840")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.275"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.5%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.5% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R2749983 or alternatively additional income so that total income is at least R95842.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 2,750,000.00 and PTI ratio of 27.6% because of a 10.0% variance on the 25.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "3150000"),
                                new TestInput("Deposit Amount", "float", "400000"),
                                new TestInput("Household Income", "float", "95493")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.276"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.6%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.6% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R2740026 or alternatively additional income so that total income is at least R95842.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 2,750,000.01 and PTI ratio of 27.4% because of a 10.0% variance on the 25.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Property Purchase Price", "float", "3150000"),
                                new TestInput("Deposit Amount", "float", "399999.99"),
                                new TestInput("Household Income", "float", "96190")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Loan Amount", "to equal", "float", "2750000.01")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Loan amount requested is above the product maximum.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when a new purchase application falls into category 5", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction"),
                        new TestInput ("Household Income", "float", "62960"),
                        new TestInput ("Property Purchase Price", "float", "1950000"),
                        new TestInput ("Deposit Amount", "float", "150000.01"),
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
                        new Scenario("given a new purchase application with a loan amount of R 1,799,999.99 and PTI ratio of 27.4% because of a 10.0% variance on the 25.0% limit",
                            new List<ITestInput> { 

                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory5"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.274"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.4%"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.039")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 1,799,999.99 and PTI ratio of 27.5% because of a 10.0% variance on the 25.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "62731")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.275"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.5%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.5% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1799970 or alternatively additional income so that total income is at least R62733.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 1,799,999.99 and PTI ratio of 27.6% because of a 10.0% variance on the 25.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "62504")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.276"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.6%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.6% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1793457 or alternatively additional income so that total income is at least R62733.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 1,800,000.00 and PTI ratio of 27.4% because of a 10.0% variance on the 25.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Deposit Amount", "float", "150000"),
                                new TestInput("Household Income", "float", "62960")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory5"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.274"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.4%"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.039")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 1,800,000.00 and PTI ratio of 27.5% because of a 10.0% variance on the 25.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Deposit Amount", "float", "150000"),
                                new TestInput("Household Income", "float", "62731")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.275"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.5%"),
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.5% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1799970 or alternatively additional income so that total income is at least R62733.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 1,800,000.00 and PTI ratio of 27.6% because of a 10.0% variance on the 25.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Deposit Amount", "float", "150000"),
                                new TestInput("Household Income", "float", "62504")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.276"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.6%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.6% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1793457 or alternatively additional income so that total income is at least R62733.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 1,800,000.01 and PTI ratio of 27.4% because of a 10.0% variance on the 25.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Deposit Amount", "float", "149999.99"),
                                new TestInput("Household Income", "float", "62960")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Loan Amount", "to equal", "float", "1800000.01")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Loan amount requested is above the product maximum.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when a new purchase application falls into category 10", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction"),
                        new TestInput ("Household Income", "float", "64268"),
                        new TestInput ("Property Purchase Price", "float", "1850000"),
                        new TestInput ("Deposit Amount", "float", "50000.01"),
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
                        new Scenario("given a new purchase application with a loan amount of R 1,799,999.99 and PTI ratio of 27.4% because of a 10.0% variance on the 25.0% limit",
                            new List<ITestInput> { 

                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory10"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.274"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.4%"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.042")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 1,799,999.99 and PTI ratio of 27.5% because of a 10.0% variance on the 25.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "64034")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.275"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.5%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.5% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1799976 or alternatively additional income so that total income is at least R64036.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 1,799,999.99 and PTI ratio of 27.6% because of a 10.0% variance on the 25.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "63802")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.276"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.6%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.6% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1793454 or alternatively additional income so that total income is at least R64036.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 1,800,000.00 and PTI ratio of 27.4% because of a 10.0% variance on the 25.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Deposit Amount", "float", "50000"),
                                new TestInput("Household Income", "float", "64268")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory10"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.274"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.4%"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.042")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 1,800,000.00 and PTI ratio of 27.5% because of a 10.0% variance on the 25.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Deposit Amount", "float", "50000"),
                                new TestInput("Household Income", "float", "64034")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.275"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.5%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.5% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1799976 or alternatively additional income so that total income is at least R64036.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 1,800,000.00 and PTI ratio of 27.6% because of a 10.0% variance on the 25.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Deposit Amount", "float", "50000"),
                                new TestInput("Household Income", "float", "63802")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.276"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.6%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.6% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1793454 or alternatively additional income so that total income is at least R64036.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a new purchase application with a loan amount of R 1,800,000.01 and PTI ratio of 27.4% because of a 10.0% variance on the 25.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Deposit Amount", "float", "49999.99"),
                                new TestInput("Household Income", "float", "64268")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Loan Amount", "to equal", "float", "1800000.01")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Loan amount requested is above the product maximum.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when a switch application falls into category 0", 
                    new List<ITestInput> { 

                        new TestInput ("Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch"),
                        new TestInput ("Property Occupancy Type", "SAHomeLoans::PropertyOccupancyType", "Enumerations::sAHomeLoans::propertyOccupancyType.OwnerOccupied"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction"),
                        new TestInput ("Household Income", "float", "48523"),
                        new TestInput ("Property Purchase Price", "float", "0"),
                        new TestInput ("Deposit Amount", "float", "0"),
                        new TestInput ("Cash Amount Required", "float", "799999.99"),
                        new TestInput ("Current Mortgage Loan Balance", "float", "1000000"),
                        new TestInput ("Estimated Market Value of Property", "float", "2650000"),
                        new TestInput ("Term In Month", "int", "240"),
                        new TestInput ("Application Empirica", "int", "575"),
                        new TestInput ("Fees", "float", "0"),
                        new TestInput ("Interim Interest", "float", "0"),
                        new TestInput ("Capitalise Fees", "bool", "False")
                    },
                    new List<IScenario> {
                        new Scenario("given a switch application with a loan amount of R 1,799,999.99 and PTI ratio of 32.9% because of a 10.0% variance on the 33.0% limit",
                            new List<ITestInput> { 

                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory0"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.329"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "32.9%"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.028")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a switch application with a loan amount of R 1,799,999.99 and PTI ratio of 33.0% because of a 10.0% variance on the 33.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "48376")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.33"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "33.0%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 33.0% and is greater than or equal to the maximum of 33.0%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1799978 or alternatively additional income so that total income is at least R48378.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a switch application with a loan amount of R 1,799,999.99 and PTI ratio of 33.1% because of a 10.0% variance on the 33.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "48230")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.331"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "33.1%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 33.1% and is greater than or equal to the maximum of 33.0%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1794546 or alternatively additional income so that total income is at least R48378.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a switch application with a loan amount of R 1,800,000.00 and PTI ratio of 32.9% because of a 10.0% variance on the 33.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Cash Amount Required", "float", "800000"),
                                new TestInput("Household Income", "float", "48523")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory0"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.329"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "32.9%"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.028")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a switch application with a loan amount of R 1,800,000.00 and PTI ratio of 33.0% because of a 10.0% variance on the 33.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Cash Amount Required", "float", "800000"),
                                new TestInput("Household Income", "float", "48376")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.33"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "33.0%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 33.0% and is greater than or equal to the maximum of 33.0%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1799978 or alternatively additional income so that total income is at least R48378.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a switch application with a loan amount of R 1,800,000.00 and PTI ratio of 33.1% because of a 10.0% variance on the 33.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Cash Amount Required", "float", "800000"),
                                new TestInput("Household Income", "float", "48230")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.331"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "33.1%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 33.1% and is greater than or equal to the maximum of 33.0%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1794546 or alternatively additional income so that total income is at least R48378.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a switch application with a loan amount of R 1,800,000.01 and PTI ratio of 32.9% because of a 10.0% variance on the 33.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Cash Amount Required", "float", "800000.01"),
                                new TestInput("Household Income", "float", "48523")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory0"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.329"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "32.9%"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.028")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a switch application with a loan amount of R 1,800,000.01 and PTI ratio of 33.0% because of a 10.0% variance on the 33.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Cash Amount Required", "float", "800000.01"),
                                new TestInput("Household Income", "float", "48376")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.33"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "33.0%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 33.0% and is greater than or equal to the maximum of 33.0%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1799978 or alternatively additional income so that total income is at least R48378.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a switch application with a loan amount of R 1,800,000.01 and PTI ratio of 33.1% because of a 10.0% variance on the 33.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Cash Amount Required", "float", "800000.01"),
                                new TestInput("Household Income", "float", "48230")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.331"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "33.1%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 33.1% and is greater than or equal to the maximum of 33.0%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R1794546 or alternatively additional income so that total income is at least R48378.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a switch application with a loan amount of R 2,749,999.99 and PTI ratio of 32.9% because of a 10.0% variance on the 33.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Estimated Market Value of Property", "float", "4230000"),
                                new TestInput("Cash Amount Required", "float", "1749999.99"),
                                new TestInput("Household Income", "float", "74133")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory0"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.329"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "32.9%"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.028")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a switch application with a loan amount of R 2,749,999.99 and PTI ratio of 33.0% because of a 10.0% variance on the 33.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Estimated Market Value of Property", "float", "4230000"),
                                new TestInput("Cash Amount Required", "float", "1749999.99"),
                                new TestInput("Household Income", "float", "73907")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.33"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "33.0%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 33.0% and is greater than or equal to the maximum of 33.0%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R2749938 or alternatively additional income so that total income is at least R73910.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a switch application with a loan amount of R 2,749,999.99 and PTI ratio of 33.1% because of a 10.0% variance on the 33.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Estimated Market Value of Property", "float", "4230000"),
                                new TestInput("Cash Amount Required", "float", "1749999.99"),
                                new TestInput("Household Income", "float", "73685")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.331"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "33.1%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 33.1% and is greater than or equal to the maximum of 33.0%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R2741678 or alternatively additional income so that total income is at least R73910.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a switch application with a loan amount of R 2,750,000.00 and PTI ratio of 32.9% because of a 10.0% variance on the 33.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Estimated Market Value of Property", "float", "4230000"),
                                new TestInput("Cash Amount Required", "float", "1750000"),
                                new TestInput("Household Income", "float", "74133")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory0"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.329"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "32.9%"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.028")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a switch application with a loan amount of R 2,750,000.00 and PTI ratio of 33.0% because of a 10.0% variance on the 33.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Estimated Market Value of Property", "float", "4230000"),
                                new TestInput("Cash Amount Required", "float", "1750000"),
                                new TestInput("Household Income", "float", "73908")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.33"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "33.0%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 33.0% and is greater than or equal to the maximum of 33.0%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R2749975 or alternatively additional income so that total income is at least R73910.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a switch application with a loan amount of R 2,750,000.00 and PTI ratio of 33.1% because of a 10.0% variance on the 33.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Estimated Market Value of Property", "float", "4230000"),
                                new TestInput("Cash Amount Required", "float", "1750000"),
                                new TestInput("Household Income", "float", "73685")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.331"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "33.1%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 33.1% and is greater than or equal to the maximum of 33.0%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R2741678 or alternatively additional income so that total income is at least R73910.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a switch application with a loan amount of R 2,750,000.01 and PTI ratio of 27.4% because of a 10.0% variance on the 25.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Estimated Market Value of Property", "float", "4230000"),
                                new TestInput("Cash Amount Required", "float", "1750000.01"),
                                new TestInput("Household Income", "float", "89014")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "True"),
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory0"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.274"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.4%"),
 
                                new TestOutput("Link Rate", "to equal", "float", "0.028")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a switch application with a loan amount of R 2,750,000.01 and PTI ratio of 27.5% because of a 10.0% variance on the 25.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Estimated Market Value of Property", "float", "4230000"),
                                new TestInput("Cash Amount Required", "float", "1750000.01"),
                                new TestInput("Household Income", "float", "88690")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.275"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.5%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.5% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R2749988 or alternatively additional income so that total income is at least R88691.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a switch application with a loan amount of R 2,750,000.01 and PTI ratio of 27.6% because of a 10.0% variance on the 25.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Estimated Market Value of Property", "float", "4230000"),
                                new TestInput("Cash Amount Required", "float", "1750000.01"),
                                new TestInput("Household Income", "float", "88368")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Application", "to equal", "bool", "False"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.276"),
 
                                new TestOutput("Payment to Income as Percent", "to equal", "string", "27.6%")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.6% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R2740004 or alternatively additional income so that total income is at least R88691.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a switch application with a loan amount of R 4,999,999.99 and PTI ratio of 27.4% because of a 10.0% variance on the 25.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Estimated Market Value of Property", "float", "7690000"),
                                new TestInput("Cash Amount Required", "float", "3999999.99"),
                                new TestInput("Household Income", "float", "161843")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory0"),
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.274")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a switch application with a loan amount of R 4,999,999.99 and PTI ratio of 27.5% because of a 10.0% variance on the 25.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Estimated Market Value of Property", "float", "7690000"),
                                new TestInput("Cash Amount Required", "float", "3999999.99"),
                                new TestInput("Household Income", "float", "161255")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.275")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.5% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R4999993 or alternatively additional income so that total income is at least R161256.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a switch application with a loan amount of R 4,999,999.99 and PTI ratio of 27.6% because of a 10.0% variance on the 25.0% limit",
                            new List<ITestInput> { 
                                new TestInput("Estimated Market Value of Property", "float", "7690000"),
                                new TestInput("Cash Amount Required", "float", "3999999.99"),
                                new TestInput("Household Income", "float", "160670")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Payment to Income", "to equal", "float", "0.276")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Your repayment as a percentage of household income (PTI) would be 27.6% and is greater than or equal to the maximum of 27.5%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R4981854 or alternatively additional income so that total income is at least R161256.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    })
            };
        }
    }
}


 