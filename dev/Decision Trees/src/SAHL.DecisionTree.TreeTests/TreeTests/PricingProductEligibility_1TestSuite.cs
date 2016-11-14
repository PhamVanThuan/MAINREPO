using System.Collections.Generic;
using SAHL.Tools.DecisionTree.TestRunner.Interfaces;
using SAHL.DecisionTree.TreeTests.Models;

namespace SAHL.DecisionTree.TreeTests
{
    public class PricingProductEligibility_1TestSuite : ITestSuite
    {
        public IEnumerable<ITestCase> TestCases { get; set; }
    
        public PricingProductEligibility_1TestSuite()
        {
            this.TestCases = new List<ITestCase> 
            {
                new TestCase("when Debugging", 
                    new List<ITestInput> { 

                        new TestInput ("Channel", "string", "GEPF"),
                        new TestInput ("Application Employment Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried"),
                        new TestInput ("Application Income", "double", "50000"),
                        new TestInput ("Loan Agreement Amount", "double", "2500000"),
                        new TestInput ("Application Credit Score", "int", "595"),
                        new TestInput ("Area Classification", "int", "4"),
                        new TestInput ("Property Value", "double", "3000000"),
                        new TestInput ("Application Payment Type", "string", "Subsidy Payment")
                    },
                    new List<IScenario> {
                        new Scenario("given testing",
                            new List<ITestInput> { 
                                new TestInput("Channel", "string", "Capitec")
                            },
                            new List<ITestOutput> {

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


 