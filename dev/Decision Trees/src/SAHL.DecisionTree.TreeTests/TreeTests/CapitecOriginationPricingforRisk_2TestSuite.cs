using System.Collections.Generic;
using SAHL.Tools.DecisionTree.TestRunner.Interfaces;
using SAHL.DecisionTree.TreeTests.Models;

namespace SAHL.DecisionTree.TreeTests
{
    public class CapitecOriginationPricingforRisk_2TestSuite : ITestSuite
    {
        public IEnumerable<ITestCase> TestCases { get; set; }
    
        public CapitecOriginationPricingforRisk_2TestSuite()
        {
            this.TestCases = new List<ITestCase> 
            {
                new TestCase("when salaried application is category 0", 
                    new List<ITestInput> { 

                        new TestInput ("Application Empirica", "int", "594"),
                        new TestInput ("Credit Matrix Category", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory0"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried")
                    },
                    new List<IScenario> {
                        new Scenario("given empirica is&nbsp;between 575 and 600 inclusive",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "599")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.004")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given empirica is above 601",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "601")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given empirica equals 600",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "600")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.004")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given empirica is equal to 575",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "575")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.004")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when salaried application is category 1", 
                    new List<ITestInput> { 

                        new TestInput ("Application Empirica", "int", "600"),
                        new TestInput ("Credit Matrix Category", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory1"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried")
                    },
                    new List<IScenario> {
                        new Scenario("given empirica is between 600 and 610",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "609")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.004")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given empirica is above 610",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "611")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given empirica is below 600",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "599")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given empirica is equal to 600",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "600")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.004")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Warning Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given empirica is equal to 610",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "610"),
                                new TestInput("Credit Matrix Category", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory1"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.004")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when salaried application is in category 3, category 4 or category 5", 
                    new List<ITestInput> { 

                        new TestInput ("Application Empirica", "int", "600"),
                        new TestInput ("Credit Matrix Category", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory3"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried")
                    },
                    new List<IScenario> {
                        new Scenario("given Salaried Category 3",
                            new List<ITestInput> { 
                                new TestInput("Credit Matrix Category", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory3")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given Salaried Category 4",
                            new List<ITestInput> { 
                                new TestInput("Credit Matrix Category", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory4")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given Salaried Category 5",
                            new List<ITestInput> { 
                                new TestInput("Credit Matrix Category", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory5")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when salaried with deduction application is in category 0, category 1 or category 3", 
                    new List<ITestInput> { 

                        new TestInput ("Application Empirica", "int", "600"),
                        new TestInput ("Credit Matrix Category", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory0"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction")
                    },
                    new List<IScenario> {
                        new Scenario("given category 0",
                            new List<ITestInput> { 
                                new TestInput("Credit Matrix Category", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory0")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given category 1",
                            new List<ITestInput> { 
                                new TestInput("Credit Matrix Category", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory1")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given category 3",
                            new List<ITestInput> { 
                                new TestInput("Credit Matrix Category", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory3")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Info Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when salaried with deduction application is in category 4", 
                    new List<ITestInput> { 

                        new TestInput ("Application Empirica", "int", "585"),
                        new TestInput ("Credit Matrix Category", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory4"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction")
                    },
                    new List<IScenario> {
                        new Scenario("given empirica is between 575 and 600",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "585")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.003")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given empirica is below 575",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "574")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given empirica is above 600",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "601")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given empirica is equal to 575",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "575")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.003")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given empirica is equal to 600",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "600")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.003")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when salaried with deduction application is in category 5", 
                    new List<ITestInput> { 

                        new TestInput ("Application Empirica", "int", "600"),
                        new TestInput ("Credit Matrix Category", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory5"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction")
                    },
                    new List<IScenario> {
                        new Scenario("given empirica is between 575 and 600",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "585")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.003")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given empirica is below 575",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "574")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given empirica is above 600",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "601"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given empirica score is equal to 575",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "575")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.003")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given empirica score is equal to 600",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "575")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.003")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when salaried with deduction application is in category 10", 
                    new List<ITestInput> { 

                        new TestInput ("Application Empirica", "int", "600"),
                        new TestInput ("Credit Matrix Category", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedwithDeductionCategory10"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction")
                    },
                    new List<IScenario> {
                        new Scenario("given salaried with deduction application is in category 10",
                            new List<ITestInput> { 

                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.006")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when self-Employed application is in category 0, category 1, category 3", 
                    new List<ITestInput> { 

                        new TestInput ("Application Empirica", "int", "600"),
                        new TestInput ("Credit Matrix Category", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SelfEmployedCategory0"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SelfEmployed")
                    },
                    new List<IScenario> {
                        new Scenario("given category 0",
                            new List<ITestInput> { 
                                new TestInput("Credit Matrix Category", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SelfEmployedCategory0")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages"),
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given category 1",
                            new List<ITestInput> { 
                                new TestInput("Credit Matrix Category", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SelfEmployedCategory1")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given category 3",
                            new List<ITestInput> { 
                                new TestInput("Credit Matrix Category", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.SelfEmployedCategory3")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when alpha application is in Category 6", 
                    new List<ITestInput> { 

                        new TestInput ("Application Empirica", "int", "650"),
                        new TestInput ("Credit Matrix Category", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory6"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction")
                    },
                    new List<IScenario> {
                        new Scenario("given salaried application empirica between 600 and 609 inclusive",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "605"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.012")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried application empirica between 610 and 629 inclusive",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "615"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.008")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried application empirica between 630 and 649 inclusive",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "635"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.004")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried application empirica above 649",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "650"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried application empirica below 575",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "574"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried with deduction application empirica between 575 and 594 inclusive",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "580"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.010")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Info Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried with deduction application empirica between 595 and 629 inclusive",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "605"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.008")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried with deduction application empirica between 630 and 649 inclusive",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "635"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.004")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried with deduction application empirica above 649",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "650"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried with deduction application empirica below 575",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "574"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given self-employed application",
                            new List<ITestInput> { 
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SelfEmployed")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried application with empirica equal to 600",
                            new List<ITestInput> { 
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried"),
                                new TestInput("Application Empirica", "int", "600")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.012")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried application with empirica equal to 609",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "609"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.012")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried application with empirica equal to 610",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "610")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.008")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried application with empirica equal to 629",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "629")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.008")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried application with empirica equal to 630",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "630")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.004")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried application with empirica equal to 649",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "649")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.004")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried with deduction application empirica 575",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "575"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.010")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried with deduction application with empirica equal to 594",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "594"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.010")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried with deduction application with empirica equal to 595",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "595"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.008")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried with deduction application with empirica equal to 629",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "629"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.008")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried with deduction application with empirica equal to 630",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "630"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.004")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried with deduction application with empirica equal to 649",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "649"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.004")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when alpha application is in category 7", 
                    new List<ITestInput> { 

                        new TestInput ("Application Empirica", "int", "600"),
                        new TestInput ("Credit Matrix Category", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory7"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried")
                    },
                    new List<IScenario> {
                        new Scenario("given salaried application empirica between 600 and 609 inclusive",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "605"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.012")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried application empirica between 610 and 629 inclusive",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "620"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.008")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried application empirica between 630 and 649 inclusive",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "635"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.004")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried application empirica above 649",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "650"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried application empirica below 600",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "599"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried with deduction application empirica between 575 and 594 inclusive",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "580"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.010")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried with deduction application empirica between 595 and 629 inclusive",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "615"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.008")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried with deduction application empirica between 630 and 649 inclusive",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "635"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.004")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried with deduction application empirica above 649",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "650"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried with deduction application empirica below 575",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "547"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given self-employed application",
                            new List<ITestInput> { 
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SelfEmployed")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried application with empirica equal to 600",
                            new List<ITestInput> { 

                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.012")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried application with empirica equal to 609",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "609")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.012")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried application with empirica equal to 610",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "610")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.008")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried application with empirica equal to 629",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "629")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.008")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried application with empirica equal to 630",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "630")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.004")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried application with empirica equal to 649",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "649")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.004")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried with deduction application empirica equal to 575",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "575"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.010")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried with deduction application empirica equal to 594",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "594"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.010")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried with deduction application empirica equal to 595",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "595"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.008")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried with deduction application empirica equal to 630",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "629"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.008")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried with deduction application empirica equal to 630",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "630"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.004")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried with deduction application empirica equal to 649",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "649"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.004")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when alpha application is in category 8", 
                    new List<ITestInput> { 

                        new TestInput ("Application Empirica", "int", "650"),
                        new TestInput ("Credit Matrix Category", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory8"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction")
                    },
                    new List<IScenario> {
                        new Scenario("given salaried application empirica between 610 and 629 inclusive",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "620"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.008")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried application empirica between 630 and 649 inclusive",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "630"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.004")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried application empirica above 649",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "650"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried application empirica below 610",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "609"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried with deduction application empirica between 575 and 594 inclusive",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "580"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.010")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried with deduction application empirica between 595 and 629 inclusive",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "620"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.008")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried with deduction application empirica between 630 and 649 inclusive",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "635"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.004")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried with deduction application empirica above 649",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "650"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried with deduction application empirica below 575",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "574"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given self-employed application",
                            new List<ITestInput> { 
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SelfEmployed")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried application with empirica equal to 610",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "610"),
                                new TestInput("Credit Matrix Category", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory8"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.008")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried application with empirica equal to 629",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "629"),
                                new TestInput("Credit Matrix Category", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory8"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.008")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried application with empirica equal to 630",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "630"),
                                new TestInput("Credit Matrix Category", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory8"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.004")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried application with empirica equal to 649",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "649"),
                                new TestInput("Credit Matrix Category", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory8"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.004")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried with deduction application with empirica equal to 575",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "575"),
                                new TestInput("Credit Matrix Category", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory8"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.010")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried with deduction application with empirica equal to 594",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "594"),
                                new TestInput("Credit Matrix Category", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory8"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.010")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried with deduction application with empirica equal to 595",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "595"),
                                new TestInput("Credit Matrix Category", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory8"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.008")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried with deduction application with empirica equal to 629",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "629"),
                                new TestInput("Credit Matrix Category", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory8"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.008")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried with deduction application with empirica equal to 630",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "630"),
                                new TestInput("Credit Matrix Category", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory8"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.004")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried with deduction application with empirica equal to 649",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "649"),
                                new TestInput("Credit Matrix Category", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory8"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.004")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when alpha application is in category 9", 
                    new List<ITestInput> { 

                        new TestInput ("Application Empirica", "int", "620"),
                        new TestInput ("Credit Matrix Category", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory9"),
                        new TestInput ("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried")
                    },
                    new List<IScenario> {
                        new Scenario("given salaried application empirica between 610 and 629",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "620"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.008")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried application empirica between 630 and 649",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "640"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.004")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried application empirica above 649",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "650")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried application empirica below 610",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "609"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried with deduction application empirica between 575 and 594",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "580"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.010")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried with deduction application empirica between 595 and 629",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "620"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.008")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried with deduction application empirica between 630 and 649",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "635"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.004")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried with deduction application empirica above 649",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "650"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried with deduction application empirica below 575",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "574"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given self-employed application",
                            new List<ITestInput> { 
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SelfEmployed")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Info Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried application with empirica equal to 629",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "629"),
                                new TestInput("Credit Matrix Category", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory9"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.008")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried application with empirica equal to 610",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "610"),
                                new TestInput("Credit Matrix Category", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory9"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.008")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried application with empirica equal to 630",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "630"),
                                new TestInput("Credit Matrix Category", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory9"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.004")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried application with empirica equal to 649",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "649"),
                                new TestInput("Credit Matrix Category", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory9"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.004")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried with deduction application with empirica equal to 575",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "575"),
                                new TestInput("Credit Matrix Category", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory9"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.010")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried with deduction application with empirica equal to 594",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "594"),
                                new TestInput("Credit Matrix Category", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory9"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.010")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried with deduction application with empirica equal to 595",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "595"),
                                new TestInput("Credit Matrix Category", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory9"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.008")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried with deduction application with empirica equal to 629",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "629"),
                                new TestInput("Credit Matrix Category", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory9"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.008")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried with deduction application with empirica equal to 630",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "630"),
                                new TestInput("Credit Matrix Category", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory9"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.004")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given salaried with deduction application with empirica equal to 649",
                            new List<ITestInput> { 
                                new TestInput("Application Empirica", "int", "649"),
                                new TestInput("Credit Matrix Category", "SAHomeLoans::Credit::CreditMatrixCategory", "Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory9"),
                                new TestInput("Household Income Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Link Rate Adjustment", "to equal", "float", "0.004")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            })
                    })
            };
        }
    }
}


 