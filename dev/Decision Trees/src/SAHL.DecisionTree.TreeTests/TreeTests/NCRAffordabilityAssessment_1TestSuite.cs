using System.Collections.Generic;
using SAHL.Tools.DecisionTree.TestRunner.Interfaces;
using SAHL.DecisionTree.TreeTests.Models;

namespace SAHL.DecisionTree.TreeTests
{
    public class NCRAffordabilityAssessment_1TestSuite : ITestSuite
    {
        public IEnumerable<ITestCase> TestCases { get; set; }
    
        public NCRAffordabilityAssessment_1TestSuite()
        {
            this.TestCases = new List<ITestCase> 
            {
                new TestCase("when NCR Affordability", 
                    new List<ITestInput> { 

                        new TestInput ("Gross Client Monthly Income", "float", "500")
                    },
                    new List<IScenario> {
                        new Scenario("given Client Gross Monthly Income Less Than 0",
                            new List<ITestInput> { 
                                new TestInput("Gross Client Monthly Income", "float", "-1")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("MinMonthlyFixedExpenses", "to equal", "float", "0")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given Client Gross Monthly Income Equals 0",
                            new List<ITestInput> { 
                                new TestInput("Gross Client Monthly Income", "float", "0")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("MinMonthlyFixedExpenses", "to equal", "float", "0")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given Client Gross Monthly Income Equals R800",
                            new List<ITestInput> { 
                                new TestInput("Gross Client Monthly Income", "float", "800")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("MinMonthlyFixedExpenses", "to equal", "float", "0")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given Client Gross Monthly Income equals R800.01",
                            new List<ITestInput> { 
                                new TestInput("Gross Client Monthly Income", "float", "800.01")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("MinMonthlyFixedExpenses", "to equal", "float", "800")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given Client Gross Monthly Income equals R6250",
                            new List<ITestInput> { 
                                new TestInput("Gross Client Monthly Income", "float", "6250")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("MinMonthlyFixedExpenses", "to equal", "float", "1168")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given Client Gross Monthly Income equals R6250.01",
                            new List<ITestInput> { 
                                new TestInput("Gross Client Monthly Income", "float", "6250.01")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("MinMonthlyFixedExpenses", "to equal", "float", "1168")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given Client Gross Monthly Income equals R10,000",
                            new List<ITestInput> { 
                                new TestInput("Gross Client Monthly Income", "float", "10000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("MinMonthlyFixedExpenses", "to equal", "float", "1505")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given Client Gross Monthly Income R25,000",
                            new List<ITestInput> { 
                                new TestInput("Gross Client Monthly Income", "float", "25000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("MinMonthlyFixedExpenses", "to equal", "float", "2855")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given Client Gross Monthly Income equals R25,000.01",
                            new List<ITestInput> { 
                                new TestInput("Gross Client Monthly Income", "float", "25000.01")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("MinMonthlyFixedExpenses", "to equal", "float", "2855")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given Client Monthly Income equals R35,000",
                            new List<ITestInput> { 
                                new TestInput("Gross Client Monthly Income", "float", "35000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("MinMonthlyFixedExpenses", "to equal", "float", "3675")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given Client Gross Monthly Income equals R50,000",
                            new List<ITestInput> { 
                                new TestInput("Gross Client Monthly Income", "float", "50000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("MinMonthlyFixedExpenses", "to equal", "float", "4905")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given Client Gross Monthly Income > R50,000",
                            new List<ITestInput> { 
                                new TestInput("Gross Client Monthly Income", "float", "60000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("MinMonthlyFixedExpenses", "to equal", "float", "5580")
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


 