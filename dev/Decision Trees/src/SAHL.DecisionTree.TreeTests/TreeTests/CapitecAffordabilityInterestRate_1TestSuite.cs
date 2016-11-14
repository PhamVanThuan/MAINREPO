using System.Collections.Generic;
using SAHL.Tools.DecisionTree.TestRunner.Interfaces;
using SAHL.DecisionTree.TreeTests.Models;

namespace SAHL.DecisionTree.TreeTests
{
    public class CapitecAffordabilityInterestRate_1TestSuite : ITestSuite
    {
        public IEnumerable<ITestCase> TestCases { get; set; }
    
        public CapitecAffordabilityInterestRate_1TestSuite()
        {
            this.TestCases = new List<ITestCase> 
            {
                new TestCase("when querying the tree for an interest rate only", 
                    new List<ITestInput> { 

                        new TestInput ("Household Income", "float", "15000"),
                        new TestInput ("Deposit", "float", "0"),
                        new TestInput ("Calc Rate", "float", "0"),
                        new TestInput ("Interest Rate Query", "bool", "True")
                    },
                    new List<IScenario> {
                        new Scenario("given household income of R 20,000",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "20000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Interest Rate", "to equal", "float", "0.1")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given household income of R 18,599",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "19999")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Interest Rate", "to equal", "float", "0.104")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given household income of R 18,601",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "20001")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Interest Rate", "to equal", "float", "0.1")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when calculating what the client can afford", 
                    new List<ITestInput> { 

                        new TestInput ("Household Income", "float", "15000"),
                        new TestInput ("Deposit", "float", "200000"),
                        new TestInput ("Calc Rate", "float", "0.102"),
                        new TestInput ("Interest Rate Query", "bool", "False")
                    },
                    new List<IScenario> {
                        new Scenario("given interest rate query set to false and valid inputs are provided",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "15000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Property Price Qualified For", "to equal", "float", "659976"),
 
                                new TestOutput("Amount Qualified For", "to equal", "float", "459976"),
 
                                new TestOutput("Instalment", "to equal", "float", "4500"),
 
                                new TestOutput("Term In Months", "to equal", "int", "240"),
 
                                new TestOutput("Interest Rate", "to equal", "float", "0.102")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given interest rate query set to false and the rate provided is 0",
                            new List<ITestInput> { 
                                new TestInput("Calc Rate", "float", "0")
                            },
                            new List<ITestOutput> {

                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("Interest rate must be greater than zero.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given household income is less than minimum affordability income",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "5999")
                            },
                            new List<ITestOutput> {

                            },
                            new List<IOutputMessage> {
                                new OutputMessage("The minimum Household Income is :R 6000.0", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given the household income is greater than the maximum affordability income",
                            new List<ITestInput> { 
                                new TestInput("Household Income", "float", "250001")
                            },
                            new List<ITestOutput> {

                            },
                            new List<IOutputMessage> {
                                new OutputMessage("The maximum Household Income is :R 250000.0", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    })
            };
        }
    }
}


 