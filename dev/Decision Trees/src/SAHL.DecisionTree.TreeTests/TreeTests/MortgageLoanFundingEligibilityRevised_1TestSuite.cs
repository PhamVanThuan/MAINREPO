using System.Collections.Generic;
using SAHL.Tools.DecisionTree.TestRunner.Interfaces;
using SAHL.DecisionTree.TreeTests.Models;

namespace SAHL.DecisionTree.TreeTests
{
    public class MortgageLoanFundingEligibilityRevised_1TestSuite : ITestSuite
    {
        public IEnumerable<ITestCase> TestCases { get; set; }
    
        public MortgageLoanFundingEligibilityRevised_1TestSuite()
        {
            this.TestCases = new List<ITestCase> 
            {
                new TestCase("when debugging the funding tree", 
                    new List<ITestInput> { 

                        new TestInput ("Loan Agreement Amount", "double", "600000"),
                        new TestInput ("Property Value", "double", "1000000"),
                        new TestInput ("Mortgage Loan Product", "SAHomeLoans::MortgageLoanProductType", "Enumerations::sAHomeLoans::mortgageLoanProductType.NewVariable"),
                        new TestInput ("Channel", "string", "Capitec"),
                        new TestInput ("Current Parent SPV", "int", "0"),
                        new TestInput ("is Further Lending", "bool", "True"),
                        new TestInput ("Application Household Income", "double", "50000"),
                        new TestInput ("Application Empirica", "int", "595"),
                        new TestInput ("is Government Employed", "bool", "False"),
                        new TestInput ("is Credit Score Exception", "bool", "False"),
                        new TestInput ("is Policy Excpetion", "bool", "False")
                    },
                    new List<IScenario> {
                        new Scenario("given all passes",
                            new List<ITestInput> { 

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


 