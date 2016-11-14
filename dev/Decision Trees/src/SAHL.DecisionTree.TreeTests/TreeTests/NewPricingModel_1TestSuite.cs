using System.Collections.Generic;
using SAHL.Tools.DecisionTree.TestRunner.Interfaces;
using SAHL.DecisionTree.TreeTests.Models;

namespace SAHL.DecisionTree.TreeTests
{
    public class NewPricingModel_1TestSuite : ITestSuite
    {
        public IEnumerable<ITestCase> TestCases { get; set; }
    
        public NewPricingModel_1TestSuite()
        {
            this.TestCases = new List<ITestCase> 
            {
                new TestCase("when Pricing model executes", 
                    new List<ITestInput> { 

                        new TestInput ("Funding Source", "string", "Low LTV"),
                        new TestInput ("Channel", "string", "Contact Centre"),
                        new TestInput ("LTV Range", "string", "Low Range"),
                        new TestInput ("Lending Purpose", "string", "New Purchase"),
                        new TestInput ("Term", "int", "240"),
                        new TestInput ("Loan Size", "string", "Medium"),
                        new TestInput ("Product Type", "string", "Variable"),
                        new TestInput ("Title Type", "string", "Freehold"),
                        new TestInput ("Applicants", "int", "2"),
                        new TestInput ("PTI", "double", "22"),
                        new TestInput ("Household Income", "double", "22000"),
                        new TestInput ("Application Employment Type", "string", "Salaried"),
                        new TestInput ("Credit Score", "int", "623"),
                        new TestInput ("Defending Rates", "string", "Key Focus"),
                        new TestInput ("Customer Pricing Appetite Type", "string", "Elastic")
                    },
                    new List<IScenario> {
                        new Scenario("given defaults",
                            new List<ITestInput> { 
                                new TestInput("Funding Source", "string", "Low LTV")
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


 