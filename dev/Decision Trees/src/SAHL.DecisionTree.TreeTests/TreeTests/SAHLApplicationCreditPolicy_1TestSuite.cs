using System.Collections.Generic;
using SAHL.Tools.DecisionTree.TestRunner.Interfaces;
using SAHL.DecisionTree.TreeTests.Models;

namespace SAHL.DecisionTree.TreeTests
{
    public class SAHLApplicationCreditPolicy_1TestSuite : ITestSuite
    {
        public IEnumerable<ITestCase> TestCases { get; set; }
    
        public SAHLApplicationCreditPolicy_1TestSuite()
        {
            this.TestCases = new List<ITestCase> 
            {
                new TestCase("when ensuring all applicants are between the ages of 18 and 65", 
                    new List<ITestInput> { 

                        new TestInput ("First Income Contributor Applicant Empirica", "int", "600"),
                        new TestInput ("First Income Contributor Applicant Income", "float", "25000"),
                        new TestInput ("Second Income Contributor Applicant Empirica", "int", "600"),
                        new TestInput ("Second Income Contributor Applicant Income", "float", "25000"),
                        new TestInput ("Household Income", "float", "50000"),
                        new TestInput ("Youngest Applicant Age in Years", "int", "18"),
                        new TestInput ("Eldest Applicant Age in Years", "int", "65")
                    },
                    new List<IScenario> {
                        new Scenario("given a youngest applicant younger than 18 years of age",
                            new List<ITestInput> { 
                                new TestInput("Youngest Applicant Age in Years", "int", "17")
                            },
                            new List<ITestOutput> {

                            },
                            new List<IOutputMessage> {
                                new OutputMessage("All applicants must be between the ages of 18 and 65 years.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a youngest applicant of 18 years of age",
                            new List<ITestInput> { 
                                new TestInput("Youngest Applicant Age in Years", "int", "18")
                            },
                            new List<ITestOutput> {

                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Applicants must be between the ages of 18 and 65 years", "should not contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an oldest applicant older than 65 years of age",
                            new List<ITestInput> { 
                                new TestInput("Eldest Applicant Age in Years", "int", "66")
                            },
                            new List<ITestOutput> {

                            },
                            new List<IOutputMessage> {
                                new OutputMessage("All applicants must be between the ages of 18 and 65 years.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an oldest applicant of 65 years of age",
                            new List<ITestInput> { 
                                new TestInput("Eldest Applicant Age in Years", "int", "65")
                            },
                            new List<ITestOutput> {

                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Applicants must be between the ages of 18 and 65 years", "should not contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a youngest applicant older than 18 years of age",
                            new List<ITestInput> { 
                                new TestInput("Youngest Applicant Age in Years", "int", "19")
                            },
                            new List<ITestOutput> {

                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Applicants must be between the ages of 18 and 65 years", "should not contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an oldest applicant younger than 65 years of age",
                            new List<ITestInput> { 
                                new TestInput("Eldest Applicant Age in Years", "int", "64")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Borrower Age", "to equal", "bool", "True"),
 
                                new TestOutput("Eligible Empirica", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should not contain", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when the 1st applicant contributes at least 75% towards household income", 
                    new List<ITestInput> { 

                        new TestInput ("First Income Contributor Applicant Empirica", "int", "575"),
                        new TestInput ("First Income Contributor Applicant Income", "float", "22500"),
                        new TestInput ("Second Income Contributor Applicant Empirica", "int", "575"),
                        new TestInput ("Second Income Contributor Applicant Income", "float", "7500"),
                        new TestInput ("Household Income", "float", "30000"),
                        new TestInput ("Youngest Applicant Age in Years", "int", "35"),
                        new TestInput ("Eldest Applicant Age in Years", "int", "36")
                    },
                    new List<IScenario> {
                        new Scenario("given an empirica score of 575 for the first applicant",
                            new List<ITestInput> { 

                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Empirica", "to equal", "bool", "True"),
 
                                new TestOutput("Application Empirica", "to equal", "int", "575")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an empirica score less than 575 for the first applicant",
                            new List<ITestInput> { 
                                new TestInput("First Income Contributor Applicant Empirica", "int", "574")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Empirica", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an empirica score greater than 575 for the first applicant",
                            new List<ITestInput> { 
                                new TestInput("First Income Contributor Applicant Empirica", "int", "576")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Empirica", "to equal", "bool", "True"),
 
                                new TestOutput("Application Empirica", "to equal", "int", "576")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a first applicant that contributes more than 75% and an empirica greater than 575",
                            new List<ITestInput> { 
                                new TestInput("First Income Contributor Applicant Empirica", "int", "575"),
                                new TestInput("First Income Contributor Applicant Income", "float", "22503")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Application Empirica", "to equal", "int", "575"),
 
                                new TestOutput("Eligible Empirica", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when the 2nd applicant contributes at least 75% towards household income", 
                    new List<ITestInput> { 

                        new TestInput ("First Income Contributor Applicant Empirica", "int", "560"),
                        new TestInput ("First Income Contributor Applicant Income", "float", "7500"),
                        new TestInput ("Second Income Contributor Applicant Empirica", "int", "575"),
                        new TestInput ("Second Income Contributor Applicant Income", "float", "22500"),
                        new TestInput ("Household Income", "float", "30000"),
                        new TestInput ("Youngest Applicant Age in Years", "int", "34"),
                        new TestInput ("Eldest Applicant Age in Years", "int", "35")
                    },
                    new List<IScenario> {
                        new Scenario("given an empirica score of 575 for the second applicant",
                            new List<ITestInput> { 

                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Application Empirica", "to equal", "int", "575"),
 
                                new TestOutput("Eligible Empirica", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an empirica score less than 575 for the second applicant",
                            new List<ITestInput> { 
                                new TestInput("Second Income Contributor Applicant Empirica", "int", "574")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Empirica", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given an empirica score greater than 575 for the second applicant",
                            new List<ITestInput> { 
                                new TestInput("Second Income Contributor Applicant Empirica", "int", "576")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Empirica", "to equal", "bool", "True"),
 
                                new TestOutput("Application Empirica", "to equal", "int", "576")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given the 2nd applicant contributes more than 75% and an empirica greater than 575",
                            new List<ITestInput> { 
                                new TestInput("First Income Contributor Applicant Empirica", "int", "575"),
                                new TestInput("First Income Contributor Applicant Income", "float", "22503")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Application Empirica", "to equal", "int", "575"),
 
                                new TestOutput("Eligible Empirica", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when neither applicant contributes at least 75% towards household income", 
                    new List<ITestInput> { 

                        new TestInput ("First Income Contributor Applicant Empirica", "int", "575"),
                        new TestInput ("First Income Contributor Applicant Income", "float", "15000"),
                        new TestInput ("Second Income Contributor Applicant Empirica", "int", "575"),
                        new TestInput ("Second Income Contributor Applicant Income", "float", "15000"),
                        new TestInput ("Household Income", "float", "30000"),
                        new TestInput ("Youngest Applicant Age in Years", "int", "34"),
                        new TestInput ("Eldest Applicant Age in Years", "int", "35")
                    },
                    new List<IScenario> {
                        new Scenario("given both applicants have an empirica less than 561",
                            new List<ITestInput> { 
                                new TestInput("First Income Contributor Applicant Empirica", "int", "560"),
                                new TestInput("Second Income Contributor Applicant Empirica", "int", "560")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Empirica", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given the 1st applicant has an empirica less than 561",
                            new List<ITestInput> { 
                                new TestInput("First Income Contributor Applicant Empirica", "int", "560")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Empirica", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given the 2nd applicant has an empirica less than 561",
                            new List<ITestInput> { 
                                new TestInput("Second Income Contributor Applicant Empirica", "int", "560")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Empirica", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given the 1st applicant has an empirica of 561 and the 2nd applicant empirica equal to 649",
                            new List<ITestInput> { 
                                new TestInput("First Income Contributor Applicant Empirica", "int", "561"),
                                new TestInput("Second Income Contributor Applicant Empirica", "int", "649")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Empirica", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given the 1st applicant has an empirica of 561 and the 2nd applicant empirica equal to 650",
                            new List<ITestInput> { 
                                new TestInput("First Income Contributor Applicant Empirica", "int", "561"),
                                new TestInput("Second Income Contributor Applicant Empirica", "int", "650")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Application Empirica", "to equal", "int", "650"),
 
                                new TestOutput("Eligible Empirica", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given the 1st applicant has an empirica of 566 and the 2nd applicant empirica equal to 649",
                            new List<ITestInput> { 
                                new TestInput("First Income Contributor Applicant Empirica", "int", "566"),
                                new TestInput("Second Income Contributor Applicant Empirica", "int", "649")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Empirica", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given the 1st applicant has an empirica of 566 and the 2nd applicant empirica equal to 650",
                            new List<ITestInput> { 
                                new TestInput("First Income Contributor Applicant Empirica", "int", "566"),
                                new TestInput("Second Income Contributor Applicant Empirica", "int", "650")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Application Empirica", "to equal", "int", "650"),
 
                                new TestOutput("Eligible Empirica", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given the 1st applicant has an empirica of 562 and the 2nd applicant empirica equal to 650",
                            new List<ITestInput> { 
                                new TestInput("First Income Contributor Applicant Empirica", "int", "562"),
                                new TestInput("Second Income Contributor Applicant Empirica", "int", "650")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Application Empirica", "to equal", "int", "650"),
 
                                new TestOutput("Eligible Empirica", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given the 1st applicant has an empirica of 565 and the 2nd applicant empirica equal to 650",
                            new List<ITestInput> { 
                                new TestInput("First Income Contributor Applicant Empirica", "int", "565"),
                                new TestInput("Second Income Contributor Applicant Empirica", "int", "650")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Application Empirica", "to equal", "int", "650"),
 
                                new TestOutput("Eligible Empirica", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given the 1st applicant has an empirica of 568 and the 2nd applicant equal to 594",
                            new List<ITestInput> { 
                                new TestInput("First Income Contributor Applicant Empirica", "int", "568"),
                                new TestInput("Second Income Contributor Applicant Empirica", "int", "594")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Empirica", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given the 1st applicant has an empirica of 568 and the 2nd applicant greater than 594",
                            new List<ITestInput> { 
                                new TestInput("First Income Contributor Applicant Empirica", "int", "568"),
                                new TestInput("Second Income Contributor Applicant Empirica", "int", "595")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Application Empirica", "to equal", "int", "595"),
 
                                new TestOutput("Eligible Empirica", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given the 1st applicant has an empirica of 575 and has the higher empirica of the 2 applicants",
                            new List<ITestInput> { 
                                new TestInput("First Income Contributor Applicant Empirica", "int", "575"),
                                new TestInput("Second Income Contributor Applicant Empirica", "int", "575")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Application Empirica", "to equal", "int", "575"),
 
                                new TestOutput("Eligible Empirica", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given the 1st applicant has an empirica of 575 and but the lower empirica of the 2 applicants",
                            new List<ITestInput> { 
                                new TestInput("First Income Contributor Applicant Empirica", "int", "575"),
                                new TestInput("Second Income Contributor Applicant Empirica", "int", "580")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Application Empirica", "to equal", "int", "580"),
 
                                new TestOutput("Eligible Empirica", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given the 1st applicant has an empirica of 569&nbsp;<span style=\"background-color: rgb(96, 169, 23);\">and the 2nd applicant equal to 595</span>",
                            new List<ITestInput> { 
                                new TestInput("First Income Contributor Applicant Empirica", "int", "569"),
                                new TestInput("Second Income Contributor Applicant Empirica", "int", "595")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Application Empirica", "to equal", "int", "595"),
 
                                new TestOutput("Eligible Empirica", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given the 1st applicant has an empirica of 573&nbsp;<span style=\"background-color: rgb(96, 169, 23);\">and the 2nd applicant equal to 595</span>",
                            new List<ITestInput> { 
                                new TestInput("First Income Contributor Applicant Empirica", "int", "573"),
                                new TestInput("Second Income Contributor Applicant Empirica", "int", "595")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Application Empirica", "to equal", "int", "595"),
 
                                new TestOutput("Eligible Empirica", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given the 1st applicant has an empirica of 575 and both applicants have equal empirica scores",
                            new List<ITestInput> { 
                                new TestInput("First Income Contributor Applicant Empirica", "int", "575"),
                                new TestInput("Second Income Contributor Applicant Empirica", "int", "575")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Application Empirica", "to equal", "int", "575"),
 
                                new TestOutput("Eligible Empirica", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when empirica scores are not available for an applicant", 
                    new List<ITestInput> { 

                        new TestInput ("First Income Contributor Applicant Empirica", "int", "-999"),
                        new TestInput ("First Income Contributor Applicant Income", "float", "15000"),
                        new TestInput ("Second Income Contributor Applicant Empirica", "int", "575"),
                        new TestInput ("Second Income Contributor Applicant Income", "float", "15000"),
                        new TestInput ("Household Income", "float", "30000"),
                        new TestInput ("Youngest Applicant Age in Years", "int", "35"),
                        new TestInput ("Eldest Applicant Age in Years", "int", "36")
                    },
                    new List<IScenario> {
                        new Scenario("given the 1st applicant has no available empirica score and neither applicant contributes 75% to household income",
                            new List<ITestInput> { 
                                new TestInput("First Income Contributor Applicant Empirica", "int", "-999"),
                                new TestInput("Second Income Contributor Applicant Empirica", "int", "575")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Application Empirica", "to equal", "int", "575"),
 
                                new TestOutput("Eligible Empirica", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given the 2nd applicant has no available empirica score and neither applicant contributes 75% to household income",
                            new List<ITestInput> { 
                                new TestInput("First Income Contributor Applicant Empirica", "int", "575"),
                                new TestInput("Second Income Contributor Applicant Empirica", "int", "-999")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Application Empirica", "to equal", "int", "575"),
 
                                new TestOutput("Eligible Empirica", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given the 1st applicant has no available empirica and the 2nd applicant contributes 75% to household income",
                            new List<ITestInput> { 
                                new TestInput("First Income Contributor Applicant Empirica", "int", "-999"),
                                new TestInput("Second Income Contributor Applicant Empirica", "int", "575"),
                                new TestInput("First Income Contributor Applicant Income", "float", "7500"),
                                new TestInput("Second Income Contributor Applicant Income", "float", "22500")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Application Empirica", "to equal", "int", "575"),
 
                                new TestOutput("Eligible Empirica", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given the 2nd applicant has no available empirica and the 1st applicant contributes 75% to household income",
                            new List<ITestInput> { 
                                new TestInput("First Income Contributor Applicant Empirica", "int", "575"),
                                new TestInput("First Income Contributor Applicant Income", "float", "22500"),
                                new TestInput("Second Income Contributor Applicant Empirica", "int", "-999"),
                                new TestInput("Second Income Contributor Applicant Income", "float", "7500")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Application Empirica", "to equal", "int", "575"),
 
                                new TestOutput("Eligible Empirica", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given first applicant has no available empirica score and second applicant score 574",
                            new List<ITestInput> { 
                                new TestInput("First Income Contributor Applicant Empirica", "int", "-999"),
                                new TestInput("Second Income Contributor Applicant Empirica", "int", "574")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Application Empirica", "to equal", "int", "-999"),
 
                                new TestOutput("Eligible Empirica", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given first applicant score 574 and the 2nd applicant has no available empirica score",
                            new List<ITestInput> { 
                                new TestInput("First Income Contributor Applicant Empirica", "int", "574"),
                                new TestInput("Second Income Contributor Applicant Empirica", "int", "-999")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Application Empirica", "to equal", "int", "-999"),
 
                                new TestOutput("Eligible Empirica", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when empirica scores are not available for either applicant", 
                    new List<ITestInput> { 

                        new TestInput ("First Income Contributor Applicant Empirica", "int", "-999"),
                        new TestInput ("First Income Contributor Applicant Income", "float", "15000"),
                        new TestInput ("Second Income Contributor Applicant Empirica", "int", "-999"),
                        new TestInput ("Second Income Contributor Applicant Income", "float", "15000"),
                        new TestInput ("Household Income", "float", "30000"),
                        new TestInput ("Youngest Applicant Age in Years", "int", "34"),
                        new TestInput ("Eldest Applicant Age in Years", "int", "35")
                    },
                    new List<IScenario> {
                        new Scenario("given both applicants have no available empirica scores",
                            new List<ITestInput> { 

                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Application Empirica", "to equal", "int", "-999"),
 
                                new TestOutput("Eligible Empirica", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {

                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when app score < 561", 
                    new List<ITestInput> { 

                        new TestInput ("First Income Contributor Applicant Empirica", "int", "0"),
                        new TestInput ("First Income Contributor Applicant Income", "float", "50000"),
                        new TestInput ("Second Income Contributor Applicant Empirica", "int", "0"),
                        new TestInput ("Second Income Contributor Applicant Income", "float", "50000"),
                        new TestInput ("Household Income", "float", "100000"),
                        new TestInput ("Youngest Applicant Age in Years", "int", "30"),
                        new TestInput ("Eldest Applicant Age in Years", "int", "30")
                    },
                    new List<IScenario> {
                        new Scenario("given first applicant score &lt; 561 should fail",
                            new List<ITestInput> { 
                                new TestInput("First Income Contributor Applicant Empirica", "int", "560"),
                                new TestInput("Second Income Contributor Applicant Empirica", "int", "650")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Empirica", "to equal", "bool", "False"),
 
                                new TestOutput("Application Empirica", "to equal", "int", "-999")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given second applicant score &lt; 561 should fail",
                            new List<ITestInput> { 
                                new TestInput("First Income Contributor Applicant Empirica", "int", "650"),
                                new TestInput("Second Income Contributor Applicant Empirica", "int", "560")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Empirica", "to equal", "bool", "False"),
 
                                new TestOutput("Application Empirica", "to equal", "int", "-999")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when low score &gt; 560 high score must be &gt; 649", 
                    new List<ITestInput> { 

                        new TestInput ("First Income Contributor Applicant Empirica", "int", "0"),
                        new TestInput ("First Income Contributor Applicant Income", "float", "50000"),
                        new TestInput ("Second Income Contributor Applicant Empirica", "int", "0"),
                        new TestInput ("Second Income Contributor Applicant Income", "float", "50000"),
                        new TestInput ("Household Income", "float", "100000"),
                        new TestInput ("Youngest Applicant Age in Years", "int", "30"),
                        new TestInput ("Eldest Applicant Age in Years", "int", "30")
                    },
                    new List<IScenario> {
                        new Scenario("given first applicant score 561 and second applicant score 650 should pass",
                            new List<ITestInput> { 
                                new TestInput("First Income Contributor Applicant Empirica", "int", "561"),
                                new TestInput("Second Income Contributor Applicant Empirica", "int", "650")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Empirica", "to equal", "bool", "True"),
 
                                new TestOutput("Application Empirica", "to equal", "int", "650")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given first applicant score 561 and second applicant score 649 should fail",
                            new List<ITestInput> { 
                                new TestInput("First Income Contributor Applicant Empirica", "int", "561"),
                                new TestInput("Second Income Contributor Applicant Empirica", "int", "549")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Empirica", "to equal", "bool", "False"),
 
                                new TestOutput("Application Empirica", "to equal", "int", "-999")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given first applicant score 650 and second applicant score 561 should pass",
                            new List<ITestInput> { 
                                new TestInput("First Income Contributor Applicant Empirica", "int", "650"),
                                new TestInput("Second Income Contributor Applicant Empirica", "int", "561")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Empirica", "to equal", "bool", "True"),
 
                                new TestOutput("Application Empirica", "to equal", "int", "650")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given first applicant score 649 and second applicant score 561 should fail",
                            new List<ITestInput> { 
                                new TestInput("First Income Contributor Applicant Empirica", "int", "649"),
                                new TestInput("Second Income Contributor Applicant Empirica", "int", "561")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Empirica", "to equal", "bool", "False"),
 
                                new TestOutput("Application Empirica", "to equal", "int", "-999")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when low score &gt; 567 and &lt; 575 high score must be &gt; 595", 
                    new List<ITestInput> { 

                        new TestInput ("First Income Contributor Applicant Empirica", "int", "0"),
                        new TestInput ("First Income Contributor Applicant Income", "float", "50000"),
                        new TestInput ("Second Income Contributor Applicant Empirica", "int", "0"),
                        new TestInput ("Second Income Contributor Applicant Income", "float", "50000"),
                        new TestInput ("Household Income", "float", "100000"),
                        new TestInput ("Youngest Applicant Age in Years", "int", "30"),
                        new TestInput ("Eldest Applicant Age in Years", "int", "30")
                    },
                    new List<IScenario> {
                        new Scenario("given first applicant score 568 and second applicant score 595 should pass",
                            new List<ITestInput> { 
                                new TestInput("First Income Contributor Applicant Empirica", "int", "568"),
                                new TestInput("Second Income Contributor Applicant Empirica", "int", "595")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Empirica", "to equal", "bool", "True"),
 
                                new TestOutput("Application Empirica", "to equal", "int", "595")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given first applicant score 595 and second applicant score 568 should pass",
                            new List<ITestInput> { 
                                new TestInput("First Income Contributor Applicant Empirica", "int", "595"),
                                new TestInput("Second Income Contributor Applicant Empirica", "int", "568")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Empirica", "to equal", "bool", "True"),
 
                                new TestOutput("Application Empirica", "to equal", "int", "595")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given first applicant score 567 and second applicant score 595 should fail",
                            new List<ITestInput> { 
                                new TestInput("First Income Contributor Applicant Empirica", "int", "567"),
                                new TestInput("Second Income Contributor Applicant Empirica", "int", "595")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Empirica", "to equal", "bool", "False"),
 
                                new TestOutput("Application Empirica", "to equal", "int", "-999")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given first applicant score 595 and second applicant score 567 should fail",
                            new List<ITestInput> { 
                                new TestInput("First Income Contributor Applicant Empirica", "int", "595"),
                                new TestInput("Second Income Contributor Applicant Empirica", "int", "567")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Empirica", "to equal", "bool", "False"),
 
                                new TestOutput("Application Empirica", "to equal", "int", "-999")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given first applicant score 574 and second applicant score 595 should pass",
                            new List<ITestInput> { 
                                new TestInput("First Income Contributor Applicant Empirica", "int", "574"),
                                new TestInput("Second Income Contributor Applicant Empirica", "int", "595")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Empirica", "to equal", "bool", "True"),
 
                                new TestOutput("Application Empirica", "to equal", "int", "595")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given first applicant score 595 and second applicant score 574 should pass",
                            new List<ITestInput> { 
                                new TestInput("First Income Contributor Applicant Empirica", "int", "595"),
                                new TestInput("Second Income Contributor Applicant Empirica", "int", "574")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Empirica", "to equal", "bool", "True"),
 
                                new TestOutput("Application Empirica", "to equal", "int", "595")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given first applicant score 574 and second applicant score 594 should fail",
                            new List<ITestInput> { 
                                new TestInput("First Income Contributor Applicant Empirica", "int", "574"),
                                new TestInput("Second Income Contributor Applicant Empirica", "int", "594")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Empirica", "to equal", "bool", "False"),
 
                                new TestOutput("Application Empirica", "to equal", "int", "-999")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given first applicant score 594 and second applicant score 574 should fail",
                            new List<ITestInput> { 
                                new TestInput("First Income Contributor Applicant Empirica", "int", "574"),
                                new TestInput("Second Income Contributor Applicant Empirica", "int", "594")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Empirica", "to equal", "bool", "False"),
 
                                new TestOutput("Application Empirica", "to equal", "int", "-999")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given first applicant score 575 and second applicant score 594 should pass",
                            new List<ITestInput> { 
                                new TestInput("First Income Contributor Applicant Empirica", "int", "575"),
                                new TestInput("Second Income Contributor Applicant Empirica", "int", "594")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Empirica", "to equal", "bool", "True"),
 
                                new TestOutput("Application Empirica", "to equal", "int", "594")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given first applicant score 594 and second applicant score 575 should pass",
                            new List<ITestInput> { 
                                new TestInput("First Income Contributor Applicant Empirica", "int", "594"),
                                new TestInput("Second Income Contributor Applicant Empirica", "int", "575")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Empirica", "to equal", "bool", "True"),
 
                                new TestOutput("Application Empirica", "to equal", "int", "594")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when application type is salary with deduction and low score &gt; 560 high score must be &gt; 595", 
                    new List<ITestInput> { 

                        new TestInput ("First Income Contributor Applicant Empirica", "int", "0"),
                        new TestInput ("First Income Contributor Applicant Income", "float", "50000"),
                        new TestInput ("Second Income Contributor Applicant Empirica", "int", "0"),
                        new TestInput ("Second Income Contributor Applicant Income", "float", "50000"),
                        new TestInput ("Household Income", "float", "100000"),
                        new TestInput ("Youngest Applicant Age in Years", "int", "30"),
                        new TestInput ("Eldest Applicant Age in Years", "int", "30")
                    },
                    new List<IScenario> {
                        new Scenario("given first applicant score 561 and second applicant score 595 should fail",
                            new List<ITestInput> { 
                                new TestInput("First Income Contributor Applicant Empirica", "int", "561"),
                                new TestInput("Second Income Contributor Applicant Empirica", "int", "595")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Empirica", "to equal", "bool", "False"),
 
                                new TestOutput("Application Empirica", "to equal", "int", "-999")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given first applicant score 595 and second applicant score 561 should fail",
                            new List<ITestInput> { 
                                new TestInput("First Income Contributor Applicant Empirica", "int", "595"),
                                new TestInput("Second Income Contributor Applicant Empirica", "int", "561")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Empirica", "to equal", "bool", "False"),
 
                                new TestOutput("Application Empirica", "to equal", "int", "-999")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given first applicant score 561 and second applicant score 650 should pass",
                            new List<ITestInput> { 
                                new TestInput("First Income Contributor Applicant Empirica", "int", "561"),
                                new TestInput("Second Income Contributor Applicant Empirica", "int", "650")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Empirica", "to equal", "bool", "True"),
 
                                new TestOutput("Application Empirica", "to equal", "int", "650")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given first applicant score 650 and second applicant score 561 should pass",
                            new List<ITestInput> { 
                                new TestInput("First Income Contributor Applicant Empirica", "int", "650"),
                                new TestInput("Second Income Contributor Applicant Empirica", "int", "561")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Empirica", "to equal", "bool", "True"),
 
                                new TestOutput("Application Empirica", "to equal", "int", "650")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when vetting only one applicant that applicants score must be &gt;=575", 
                    new List<ITestInput> { 

                        new TestInput ("First Income Contributor Applicant Empirica", "int", "0"),
                        new TestInput ("First Income Contributor Applicant Income", "float", "0"),
                        new TestInput ("Second Income Contributor Applicant Empirica", "int", "0"),
                        new TestInput ("Second Income Contributor Applicant Income", "float", "0"),
                        new TestInput ("Household Income", "float", "0"),
                        new TestInput ("Youngest Applicant Age in Years", "int", "30"),
                        new TestInput ("Eldest Applicant Age in Years", "int", "30")
                    },
                    new List<IScenario> {
                        new Scenario("given first applicant score 574 and second applicant is non-income contributor should fail",
                            new List<ITestInput> { 
                                new TestInput("First Income Contributor Applicant Empirica", "int", "574"),
                                new TestInput("Second Income Contributor Applicant Empirica", "int", "-999"),
                                new TestInput("First Income Contributor Applicant Income", "float", "50000"),
                                new TestInput("Second Income Contributor Applicant Income", "float", "-1"),
                                new TestInput("Household Income", "float", "50000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Empirica", "to equal", "bool", "False"),
 
                                new TestOutput("Application Empirica", "to equal", "int", "-999")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given first applicant 575 and second applicant&nbsp;<span style=\"background-color: rgb(96, 169, 23);\">is non-income contributor</span>&nbsp;should pass",
                            new List<ITestInput> { 
                                new TestInput("First Income Contributor Applicant Empirica", "int", "575"),
                                new TestInput("Second Income Contributor Applicant Empirica", "int", "-999"),
                                new TestInput("First Income Contributor Applicant Income", "float", "50000"),
                                new TestInput("Second Income Contributor Applicant Income", "float", "-1"),
                                new TestInput("Household Income", "float", "50000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Empirica", "to equal", "bool", "True"),
 
                                new TestOutput("Application Empirica", "to equal", "int", "575")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given first applicant score 756 and second applicant score&nbsp;<span style=\"background-color: rgb(96, 169, 23);\">is non-income contributor</span>&nbsp;should pass",
                            new List<ITestInput> { 
                                new TestInput("First Income Contributor Applicant Empirica", "int", "576"),
                                new TestInput("Second Income Contributor Applicant Empirica", "int", "-999"),
                                new TestInput("First Income Contributor Applicant Income", "float", "50000"),
                                new TestInput("Second Income Contributor Applicant Income", "float", "-1"),
                                new TestInput("Household Income", "float", "50000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Empirica", "to equal", "bool", "True"),
 
                                new TestOutput("Application Empirica", "to equal", "int", "576")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given first applicant score&nbsp;is non-income contributor&nbsp;and second applicant score 574 should fail",
                            new List<ITestInput> { 
                                new TestInput("First Income Contributor Applicant Empirica", "int", "-999"),
                                new TestInput("Second Income Contributor Applicant Empirica", "int", "574"),
                                new TestInput("First Income Contributor Applicant Income", "float", "-1"),
                                new TestInput("Second Income Contributor Applicant Income", "float", "50000"),
                                new TestInput("Household Income", "float", "50000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Empirica", "to equal", "bool", "False"),
 
                                new TestOutput("Application Empirica", "to equal", "int", "-999")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given first applicant score&nbsp;<span style=\"background-color: rgb(96, 169, 23);\">is non-income contributor</span>&nbsp;and second applicant score 575 should pass",
                            new List<ITestInput> { 
                                new TestInput("First Income Contributor Applicant Empirica", "int", "-999"),
                                new TestInput("Second Income Contributor Applicant Empirica", "int", "575"),
                                new TestInput("First Income Contributor Applicant Income", "float", "-1"),
                                new TestInput("Second Income Contributor Applicant Income", "float", "50000"),
                                new TestInput("Household Income", "float", "50000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Empirica", "to equal", "bool", "True"),
 
                                new TestOutput("Application Empirica", "to equal", "int", "575")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given first applicant score&nbsp;<span style=\"background-color: rgb(96, 169, 23);\">is non-income contributor</span>&nbsp;and second applicant score 576 should pass",
                            new List<ITestInput> { 
                                new TestInput("First Income Contributor Applicant Empirica", "int", "-999"),
                                new TestInput("Second Income Contributor Applicant Empirica", "int", "576"),
                                new TestInput("First Income Contributor Applicant Income", "float", "-1"),
                                new TestInput("Second Income Contributor Applicant Income", "float", "50000"),
                                new TestInput("Household Income", "float", "50000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Empirica", "to equal", "bool", "True"),
 
                                new TestOutput("Application Empirica", "to equal", "int", "576")
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


 