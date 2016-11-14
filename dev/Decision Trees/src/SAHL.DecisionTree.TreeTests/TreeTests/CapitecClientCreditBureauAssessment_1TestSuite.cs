using System.Collections.Generic;
using SAHL.Tools.DecisionTree.TestRunner.Interfaces;
using SAHL.DecisionTree.TreeTests.Models;

namespace SAHL.DecisionTree.TreeTests
{
    public class CapitecClientCreditBureauAssessment_1TestSuite : ITestSuite
    {
        public IEnumerable<ITestCase> TestCases { get; set; }
    
        public CapitecClientCreditBureauAssessment_1TestSuite()
        {
            this.TestCases = new List<ITestCase> 
            {
                new TestCase("when no credit bureau match is found", 
                    new List<ITestInput> { 

                        new TestInput ("Applicant Empirica", "int", "0"),
                        new TestInput ("Number of Judgments within Last 3 Years", "int", "0"),
                        new TestInput ("Aggregated Judgment Value within Last 3 Years", "float", "0"),
                        new TestInput ("NonSettled Aggregated Judgment Value within Last 3 Years", "float", "0"),
                        new TestInput ("Number of Unsettled Defaults within Last 2 Years", "int", "0"),
                        new TestInput ("Sequestration Notice", "bool", "False"),
                        new TestInput ("Administration Order Notice", "bool", "False"),
                        new TestInput ("Debt Counselling Notice", "bool", "False"),
                        new TestInput ("Debt Review Notice", "bool", "False"),
                        new TestInput ("Consumer Deceased Notification", "bool", "False"),
                        new TestInput ("Credit Card Revoked", "bool", "False"),
                        new TestInput ("Consumer Absconded", "bool", "False"),
                        new TestInput ("Paid Out on Deceased Claim", "bool", "False"),
                        new TestInput ("Credit Bureau Match", "bool", "False")
                    },
                    new List<IScenario> {
                        new Scenario("given credit bureau match is false",
                            new List<ITestInput> { 
                                new TestInput("Credit Bureau Match", "bool", "False")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Borrower", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("No credit bureau match found.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when there is a notice of sequestration", 
                    new List<ITestInput> { 

                        new TestInput ("Applicant Empirica", "int", "600"),
                        new TestInput ("Number of Judgments within Last 3 Years", "int", "0"),
                        new TestInput ("Aggregated Judgment Value within Last 3 Years", "float", "0"),
                        new TestInput ("NonSettled Aggregated Judgment Value within Last 3 Years", "float", "0"),
                        new TestInput ("Number of Unsettled Defaults within Last 2 Years", "int", "0"),
                        new TestInput ("Sequestration Notice", "bool", "True"),
                        new TestInput ("Administration Order Notice", "bool", "False"),
                        new TestInput ("Debt Counselling Notice", "bool", "False"),
                        new TestInput ("Debt Review Notice", "bool", "False"),
                        new TestInput ("Consumer Deceased Notification", "bool", "False"),
                        new TestInput ("Credit Card Revoked", "bool", "False"),
                        new TestInput ("Consumer Absconded", "bool", "False"),
                        new TestInput ("Paid Out on Deceased Claim", "bool", "False"),
                        new TestInput ("Credit Bureau Match", "bool", "True")
                    },
                    new List<IScenario> {
                        new Scenario("given notice of sequestration is true",
                            new List<ITestInput> { 
                                new TestInput("Sequestration Notice", "bool", "True")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Borrower", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("There is a record of Sequestration.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when there is a notice of an administration order", 
                    new List<ITestInput> { 

                        new TestInput ("Applicant Empirica", "int", "600"),
                        new TestInput ("Number of Judgments within Last 3 Years", "int", "0"),
                        new TestInput ("Aggregated Judgment Value within Last 3 Years", "float", "0"),
                        new TestInput ("NonSettled Aggregated Judgment Value within Last 3 Years", "float", "0"),
                        new TestInput ("Number of Unsettled Defaults within Last 2 Years", "int", "0"),
                        new TestInput ("Sequestration Notice", "bool", "False"),
                        new TestInput ("Administration Order Notice", "bool", "True"),
                        new TestInput ("Debt Counselling Notice", "bool", "False"),
                        new TestInput ("Debt Review Notice", "bool", "False"),
                        new TestInput ("Consumer Deceased Notification", "bool", "False"),
                        new TestInput ("Credit Card Revoked", "bool", "False"),
                        new TestInput ("Consumer Absconded", "bool", "False"),
                        new TestInput ("Paid Out on Deceased Claim", "bool", "False"),
                        new TestInput ("Credit Bureau Match", "bool", "True")
                    },
                    new List<IScenario> {
                        new Scenario("given administration order notice is true",
                            new List<ITestInput> { 
                                new TestInput("Administration Order Notice", "bool", "True")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Borrower", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("There is a record of an Administration Order.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when there is a notice of debt counseling", 
                    new List<ITestInput> { 

                        new TestInput ("Applicant Empirica", "int", "600"),
                        new TestInput ("Number of Judgments within Last 3 Years", "int", "0"),
                        new TestInput ("Aggregated Judgment Value within Last 3 Years", "float", "0"),
                        new TestInput ("NonSettled Aggregated Judgment Value within Last 3 Years", "float", "0"),
                        new TestInput ("Number of Unsettled Defaults within Last 2 Years", "int", "0"),
                        new TestInput ("Sequestration Notice", "bool", "False"),
                        new TestInput ("Administration Order Notice", "bool", "False"),
                        new TestInput ("Debt Counselling Notice", "bool", "True"),
                        new TestInput ("Debt Review Notice", "bool", "False"),
                        new TestInput ("Consumer Deceased Notification", "bool", "False"),
                        new TestInput ("Credit Card Revoked", "bool", "False"),
                        new TestInput ("Consumer Absconded", "bool", "False"),
                        new TestInput ("Paid Out on Deceased Claim", "bool", "False"),
                        new TestInput ("Credit Bureau Match", "bool", "True")
                    },
                    new List<IScenario> {
                        new Scenario("given notice of debt counseling is true",
                            new List<ITestInput> { 
                                new TestInput("Debt Counselling Notice", "bool", "True")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Borrower", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("There is a record of Debt Counselling.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when there is a notice of debt review", 
                    new List<ITestInput> { 

                        new TestInput ("Applicant Empirica", "int", "600"),
                        new TestInput ("Number of Judgments within Last 3 Years", "int", "0"),
                        new TestInput ("Aggregated Judgment Value within Last 3 Years", "float", "0"),
                        new TestInput ("NonSettled Aggregated Judgment Value within Last 3 Years", "float", "0"),
                        new TestInput ("Number of Unsettled Defaults within Last 2 Years", "int", "0"),
                        new TestInput ("Sequestration Notice", "bool", "False"),
                        new TestInput ("Administration Order Notice", "bool", "False"),
                        new TestInput ("Debt Counselling Notice", "bool", "False"),
                        new TestInput ("Debt Review Notice", "bool", "False"),
                        new TestInput ("Consumer Deceased Notification", "bool", "False"),
                        new TestInput ("Credit Card Revoked", "bool", "False"),
                        new TestInput ("Consumer Absconded", "bool", "False"),
                        new TestInput ("Paid Out on Deceased Claim", "bool", "False"),
                        new TestInput ("Credit Bureau Match", "bool", "True")
                    },
                    new List<IScenario> {
                        new Scenario("given notice of debt review is true",
                            new List<ITestInput> { 
                                new TestInput("Debt Review Notice", "bool", "True")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Borrower", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("There is a record of Debt Review.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when there is record that the consumer is deceased", 
                    new List<ITestInput> { 

                        new TestInput ("Applicant Empirica", "int", "600"),
                        new TestInput ("Number of Judgments within Last 3 Years", "int", "0"),
                        new TestInput ("Aggregated Judgment Value within Last 3 Years", "float", "0"),
                        new TestInput ("NonSettled Aggregated Judgment Value within Last 3 Years", "float", "0"),
                        new TestInput ("Number of Unsettled Defaults within Last 2 Years", "int", "0"),
                        new TestInput ("Sequestration Notice", "bool", "False"),
                        new TestInput ("Administration Order Notice", "bool", "False"),
                        new TestInput ("Debt Counselling Notice", "bool", "False"),
                        new TestInput ("Debt Review Notice", "bool", "False"),
                        new TestInput ("Consumer Deceased Notification", "bool", "False"),
                        new TestInput ("Credit Card Revoked", "bool", "False"),
                        new TestInput ("Consumer Absconded", "bool", "False"),
                        new TestInput ("Paid Out on Deceased Claim", "bool", "False"),
                        new TestInput ("Credit Bureau Match", "bool", "True")
                    },
                    new List<IScenario> {
                        new Scenario("given consumer is deceased is true",
                            new List<ITestInput> { 
                                new TestInput("Consumer Deceased Notification", "bool", "True")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Borrower", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("There is record that the consumer is deceased.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when there is record that the consumer has absconded", 
                    new List<ITestInput> { 

                        new TestInput ("Applicant Empirica", "int", "600"),
                        new TestInput ("Number of Judgments within Last 3 Years", "int", "0"),
                        new TestInput ("Aggregated Judgment Value within Last 3 Years", "float", "0"),
                        new TestInput ("NonSettled Aggregated Judgment Value within Last 3 Years", "float", "0"),
                        new TestInput ("Number of Unsettled Defaults within Last 2 Years", "int", "0"),
                        new TestInput ("Sequestration Notice", "bool", "False"),
                        new TestInput ("Administration Order Notice", "bool", "False"),
                        new TestInput ("Debt Counselling Notice", "bool", "False"),
                        new TestInput ("Debt Review Notice", "bool", "False"),
                        new TestInput ("Consumer Deceased Notification", "bool", "False"),
                        new TestInput ("Credit Card Revoked", "bool", "False"),
                        new TestInput ("Consumer Absconded", "bool", "False"),
                        new TestInput ("Paid Out on Deceased Claim", "bool", "False"),
                        new TestInput ("Credit Bureau Match", "bool", "True")
                    },
                    new List<IScenario> {
                        new Scenario("given consumer has absconded is true",
                            new List<ITestInput> { 
                                new TestInput("Consumer Absconded", "bool", "True")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Borrower", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("There is record that the applicant has absconded.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when there is record that a deceased claim has been paid out", 
                    new List<ITestInput> { 

                        new TestInput ("Applicant Empirica", "int", "600"),
                        new TestInput ("Number of Judgments within Last 3 Years", "int", "0"),
                        new TestInput ("Aggregated Judgment Value within Last 3 Years", "float", "0"),
                        new TestInput ("NonSettled Aggregated Judgment Value within Last 3 Years", "float", "0"),
                        new TestInput ("Number of Unsettled Defaults within Last 2 Years", "int", "0"),
                        new TestInput ("Sequestration Notice", "bool", "False"),
                        new TestInput ("Administration Order Notice", "bool", "False"),
                        new TestInput ("Debt Counselling Notice", "bool", "False"),
                        new TestInput ("Debt Review Notice", "bool", "False"),
                        new TestInput ("Consumer Deceased Notification", "bool", "False"),
                        new TestInput ("Credit Card Revoked", "bool", "False"),
                        new TestInput ("Consumer Absconded", "bool", "False"),
                        new TestInput ("Paid Out on Deceased Claim", "bool", "False"),
                        new TestInput ("Credit Bureau Match", "bool", "True")
                    },
                    new List<IScenario> {
                        new Scenario("given notice of deceased pay out is true",
                            new List<ITestInput> { 
                                new TestInput("Paid Out on Deceased Claim", "bool", "True")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Borrower", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("There is record that a deceased claim has been paid out.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when there is a record of judgements in the last 3 years", 
                    new List<ITestInput> { 

                        new TestInput ("Applicant Empirica", "int", "600"),
                        new TestInput ("Number of Judgments within Last 3 Years", "int", "0"),
                        new TestInput ("Aggregated Judgment Value within Last 3 Years", "float", "0"),
                        new TestInput ("NonSettled Aggregated Judgment Value within Last 3 Years", "float", "0"),
                        new TestInput ("Number of Unsettled Defaults within Last 2 Years", "int", "0"),
                        new TestInput ("Sequestration Notice", "bool", "False"),
                        new TestInput ("Administration Order Notice", "bool", "False"),
                        new TestInput ("Debt Counselling Notice", "bool", "False"),
                        new TestInput ("Debt Review Notice", "bool", "False"),
                        new TestInput ("Consumer Deceased Notification", "bool", "False"),
                        new TestInput ("Credit Card Revoked", "bool", "False"),
                        new TestInput ("Consumer Absconded", "bool", "False"),
                        new TestInput ("Paid Out on Deceased Claim", "bool", "False"),
                        new TestInput ("Credit Bureau Match", "bool", "True")
                    },
                    new List<IScenario> {
                        new Scenario("given the number of judgements is greater than 4",
                            new List<ITestInput> { 
                                new TestInput("Number of Judgments within Last 3 Years", "int", "5")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Borrower", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("There is record of multiple recent unpaid judgements in the last 3 years.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given the number of judgements is equal to 4",
                            new List<ITestInput> { 
                                new TestInput("Number of Judgments within Last 3 Years", "int", "4")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Borrower", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("There is record of multiple recent unpaid judgements in the last 3 years.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given the number of judgements is less than 4",
                            new List<ITestInput> { 
                                new TestInput("Number of Judgments within Last 3 Years", "int", "3")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Borrower", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Info Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when the number of judgements in the last 3 years is greater than 2", 
                    new List<ITestInput> { 

                        new TestInput ("Applicant Empirica", "int", "600"),
                        new TestInput ("Number of Judgments within Last 3 Years", "int", "3"),
                        new TestInput ("Aggregated Judgment Value within Last 3 Years", "float", "0"),
                        new TestInput ("NonSettled Aggregated Judgment Value within Last 3 Years", "float", "0"),
                        new TestInput ("Number of Unsettled Defaults within Last 2 Years", "int", "0"),
                        new TestInput ("Sequestration Notice", "bool", "False"),
                        new TestInput ("Administration Order Notice", "bool", "False"),
                        new TestInput ("Debt Counselling Notice", "bool", "False"),
                        new TestInput ("Debt Review Notice", "bool", "False"),
                        new TestInput ("Consumer Deceased Notification", "bool", "False"),
                        new TestInput ("Credit Card Revoked", "bool", "False"),
                        new TestInput ("Consumer Absconded", "bool", "False"),
                        new TestInput ("Paid Out on Deceased Claim", "bool", "False"),
                        new TestInput ("Credit Bureau Match", "bool", "True")
                    },
                    new List<IScenario> {
                        new Scenario("given aggregate judgement value is greater than R 10,000.00",
                            new List<ITestInput> { 
                                new TestInput("Aggregated Judgment Value within Last 3 Years", "float", "10000.01")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Borrower", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("There is record of unpaid judgements with a material aggregated rand value.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given aggregate judgement value is equal to R 10,000.00",
                            new List<ITestInput> { 
                                new TestInput("Aggregated Judgment Value within Last 3 Years", "float", "10000.00")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Borrower", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("There is record of unpaid judgements with a material aggregated rand value.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given aggregate judgement value is less than R 10,000.00",
                            new List<ITestInput> { 
                                new TestInput("Aggregated Judgment Value within Last 3 Years", "float", "9999.99")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Borrower", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("There is record of unpaid judgements with a material aggregated rand value", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when there is an aggregate value for judgements not settled for 13 to 36 months", 
                    new List<ITestInput> { 

                        new TestInput ("Applicant Empirica", "int", "600"),
                        new TestInput ("Number of Judgments within Last 3 Years", "int", "2"),
                        new TestInput ("Aggregated Judgment Value within Last 3 Years", "float", "0"),
                        new TestInput ("NonSettled Aggregated Judgment Value within Last 3 Years", "float", "0"),
                        new TestInput ("Number of Unsettled Defaults within Last 2 Years", "int", "0"),
                        new TestInput ("Sequestration Notice", "bool", "False"),
                        new TestInput ("Administration Order Notice", "bool", "False"),
                        new TestInput ("Debt Counselling Notice", "bool", "False"),
                        new TestInput ("Debt Review Notice", "bool", "False"),
                        new TestInput ("Consumer Deceased Notification", "bool", "False"),
                        new TestInput ("Credit Card Revoked", "bool", "False"),
                        new TestInput ("Consumer Absconded", "bool", "False"),
                        new TestInput ("Paid Out on Deceased Claim", "bool", "False"),
                        new TestInput ("Credit Bureau Match", "bool", "True")
                    },
                    new List<IScenario> {
                        new Scenario("given the aggregated judgement value is equal to R 15,000.00",
                            new List<ITestInput> { 
                                new TestInput("NonSettled Aggregated Judgment Value within Last 3 Years", "float", "15000.00")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Borrower", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("There is record of an outstanding aggregated unpaid judgement of material value.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given the aggregated judgement value is greater than R 15,000.00",
                            new List<ITestInput> { 
                                new TestInput("NonSettled Aggregated Judgment Value within Last 3 Years", "float", "15000.01")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Borrower", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("There is record of an outstanding aggregated unpaid judgement of material value.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given the aggregated judgement value is less than R 15,000.00",
                            new List<ITestInput> { 
                                new TestInput("NonSettled Aggregated Judgment Value within Last 3 Years", "float", "14999.99")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Borrower", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Info Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when there is record that a credit card has been revoked", 
                    new List<ITestInput> { 

                        new TestInput ("Applicant Empirica", "int", "600"),
                        new TestInput ("Number of Judgments within Last 3 Years", "int", "0"),
                        new TestInput ("Aggregated Judgment Value within Last 3 Years", "float", "0"),
                        new TestInput ("NonSettled Aggregated Judgment Value within Last 3 Years", "float", "0"),
                        new TestInput ("Number of Unsettled Defaults within Last 2 Years", "int", "0"),
                        new TestInput ("Sequestration Notice", "bool", "False"),
                        new TestInput ("Administration Order Notice", "bool", "False"),
                        new TestInput ("Debt Counselling Notice", "bool", "False"),
                        new TestInput ("Debt Review Notice", "bool", "True"),
                        new TestInput ("Consumer Deceased Notification", "bool", "False"),
                        new TestInput ("Credit Card Revoked", "bool", "True"),
                        new TestInput ("Consumer Absconded", "bool", "False"),
                        new TestInput ("Paid Out on Deceased Claim", "bool", "False"),
                        new TestInput ("Credit Bureau Match", "bool", "True")
                    },
                    new List<IScenario> {
                        new Scenario("given that that the credit card has been revoked",
                            new List<ITestInput> { 

                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Borrower", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("There is record of a revoked credit card.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when checking the empirica score&nbsp;", 
                    new List<ITestInput> { 

                        new TestInput ("Applicant Empirica", "int", "573"),
                        new TestInput ("Number of Judgments within Last 3 Years", "int", "0"),
                        new TestInput ("Aggregated Judgment Value within Last 3 Years", "float", "0"),
                        new TestInput ("NonSettled Aggregated Judgment Value within Last 3 Years", "float", "0"),
                        new TestInput ("Number of Unsettled Defaults within Last 2 Years", "int", "0"),
                        new TestInput ("Sequestration Notice", "bool", "False"),
                        new TestInput ("Administration Order Notice", "bool", "False"),
                        new TestInput ("Debt Counselling Notice", "bool", "False"),
                        new TestInput ("Debt Review Notice", "bool", "False"),
                        new TestInput ("Consumer Deceased Notification", "bool", "False"),
                        new TestInput ("Credit Card Revoked", "bool", "False"),
                        new TestInput ("Consumer Absconded", "bool", "False"),
                        new TestInput ("Paid Out on Deceased Claim", "bool", "False"),
                        new TestInput ("Credit Bureau Match", "bool", "True")
                    },
                    new List<IScenario> {
                        new Scenario("given a consumer empirica score of 573 where client exclusion rules have not failed",
                            new List<ITestInput> { 

                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Borrower", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Your credit score is below the SA Home Loans credit policy limit in terms of the loan details supplied.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a consumer empirica score of 573 where client exclusion rules have failed",
                            new List<ITestInput> { 
                                new TestInput("Applicant Empirica", "int", "573"),
                                new TestInput("Debt Counselling Notice", "bool", "True")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Borrower", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Your credit score is below the SA Home Loans credit policy limit in terms of the loan details supplied.", "should contain", "Warning Messages"),
                                new OutputMessage("There is a record of Debt Counselling.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a consumer empirica score of 574",
                            new List<ITestInput> { 
                                new TestInput("Applicant Empirica", "int", "574")
                            },
                            new List<ITestOutput> {

                            },
                            new List<IOutputMessage> {
                                new OutputMessage("The Credit Bureau score is below required minimum.", "should not contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given a consumer empirica score greater than 574",
                            new List<ITestInput> { 
                                new TestInput("Applicant Empirica", "int", "575")
                            },
                            new List<ITestOutput> {

                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Info Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when there is a record of defaults in the past 2 years", 
                    new List<ITestInput> { 

                        new TestInput ("Applicant Empirica", "int", "600"),
                        new TestInput ("Number of Judgments within Last 3 Years", "int", "0"),
                        new TestInput ("Aggregated Judgment Value within Last 3 Years", "float", "0"),
                        new TestInput ("NonSettled Aggregated Judgment Value within Last 3 Years", "float", "0"),
                        new TestInput ("Number of Unsettled Defaults within Last 2 Years", "int", "0"),
                        new TestInput ("Sequestration Notice", "bool", "False"),
                        new TestInput ("Administration Order Notice", "bool", "False"),
                        new TestInput ("Debt Counselling Notice", "bool", "False"),
                        new TestInput ("Debt Review Notice", "bool", "False"),
                        new TestInput ("Consumer Deceased Notification", "bool", "False"),
                        new TestInput ("Credit Card Revoked", "bool", "False"),
                        new TestInput ("Consumer Absconded", "bool", "False"),
                        new TestInput ("Paid Out on Deceased Claim", "bool", "False"),
                        new TestInput ("Credit Bureau Match", "bool", "True")
                    },
                    new List<IScenario> {
                        new Scenario("given the number of defaults is greater than 4",
                            new List<ITestInput> { 
                                new TestInput("Number of Unsettled Defaults within Last 2 Years", "int", "5")
                            },
                            new List<ITestOutput> {

                            },
                            new List<IOutputMessage> {
                                new OutputMessage("There is record of numerous unsettled defaults within the past 2 years.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given the number of defaults is equal to 4",
                            new List<ITestInput> { 
                                new TestInput("Number of Unsettled Defaults within Last 2 Years", "int", "4")
                            },
                            new List<ITestOutput> {

                            },
                            new List<IOutputMessage> {
                                new OutputMessage("There is record of numerous unsettled defaults within the past 2 years.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given the number of defaults is less than 4",
                            new List<ITestInput> { 
                                new TestInput("Number of Unsettled Defaults within Last 2 Years", "int", "3")
                            },
                            new List<ITestOutput> {

                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Info Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when there is a record of judgements in the past 3 years and an aggregated judgement value", 
                    new List<ITestInput> { 

                        new TestInput ("Applicant Empirica", "int", "600"),
                        new TestInput ("Number of Judgments within Last 3 Years", "int", "4"),
                        new TestInput ("Aggregated Judgment Value within Last 3 Years", "float", "15000.00"),
                        new TestInput ("NonSettled Aggregated Judgment Value within Last 3 Years", "float", "0"),
                        new TestInput ("Number of Unsettled Defaults within Last 2 Years", "int", "0"),
                        new TestInput ("Sequestration Notice", "bool", "False"),
                        new TestInput ("Administration Order Notice", "bool", "False"),
                        new TestInput ("Debt Counselling Notice", "bool", "False"),
                        new TestInput ("Debt Review Notice", "bool", "False"),
                        new TestInput ("Consumer Deceased Notification", "bool", "False"),
                        new TestInput ("Credit Card Revoked", "bool", "False"),
                        new TestInput ("Consumer Absconded", "bool", "False"),
                        new TestInput ("Paid Out on Deceased Claim", "bool", "False"),
                        new TestInput ("Credit Bureau Match", "bool", "True")
                    },
                    new List<IScenario> {
                        new Scenario("given the number of judgements is greater than 4 with an aggregated judgement value over R 10,000",
                            new List<ITestInput> { 

                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Borrower", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("There is record of multiple recent unpaid judgements in the last 3 years.", "should contain", "Warning Messages"),
                                new OutputMessage("There is record of unpaid judgements with a material aggregated rand value.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when there are multiple notices against a borrower", 
                    new List<ITestInput> { 

                        new TestInput ("Applicant Empirica", "int", "600"),
                        new TestInput ("Number of Judgments within Last 3 Years", "int", "0"),
                        new TestInput ("Aggregated Judgment Value within Last 3 Years", "float", "0"),
                        new TestInput ("NonSettled Aggregated Judgment Value within Last 3 Years", "float", "0"),
                        new TestInput ("Number of Unsettled Defaults within Last 2 Years", "int", "0"),
                        new TestInput ("Sequestration Notice", "bool", "True"),
                        new TestInput ("Administration Order Notice", "bool", "True"),
                        new TestInput ("Debt Counselling Notice", "bool", "True"),
                        new TestInput ("Debt Review Notice", "bool", "True"),
                        new TestInput ("Consumer Deceased Notification", "bool", "True"),
                        new TestInput ("Credit Card Revoked", "bool", "True"),
                        new TestInput ("Consumer Absconded", "bool", "True"),
                        new TestInput ("Paid Out on Deceased Claim", "bool", "True"),
                        new TestInput ("Credit Bureau Match", "bool", "True")
                    },
                    new List<IScenario> {
                        new Scenario("given multiple notices",
                            new List<ITestInput> { 

                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Borrower", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("There is record of a revoked credit card.", "should contain", "Warning Messages"),
                                new OutputMessage("There is a record of Sequestration.", "should contain", "Warning Messages"),
                                new OutputMessage("There is a record of an Administration Order.", "should contain", "Warning Messages"),
                                new OutputMessage("There is a record of Debt Counselling.", "should contain", "Warning Messages"),
                                new OutputMessage("There is a record of Debt Review.", "should contain", "Warning Messages"),
                                new OutputMessage("There is record that the consumer is deceased.", "should contain", "Warning Messages"),
                                new OutputMessage("There is record that the applicant has absconded.", "should contain", "Warning Messages"),
                                new OutputMessage("There is record that a deceased claim has been paid out.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when the borrower passes all credit bureau assessment checks", 
                    new List<ITestInput> { 

                        new TestInput ("Applicant Empirica", "int", "575"),
                        new TestInput ("Number of Judgments within Last 3 Years", "int", "0"),
                        new TestInput ("Aggregated Judgment Value within Last 3 Years", "float", "0"),
                        new TestInput ("NonSettled Aggregated Judgment Value within Last 3 Years", "float", "0"),
                        new TestInput ("Number of Unsettled Defaults within Last 2 Years", "int", "0"),
                        new TestInput ("Sequestration Notice", "bool", "False"),
                        new TestInput ("Administration Order Notice", "bool", "False"),
                        new TestInput ("Debt Counselling Notice", "bool", "False"),
                        new TestInput ("Debt Review Notice", "bool", "False"),
                        new TestInput ("Consumer Deceased Notification", "bool", "False"),
                        new TestInput ("Credit Card Revoked", "bool", "False"),
                        new TestInput ("Consumer Absconded", "bool", "False"),
                        new TestInput ("Paid Out on Deceased Claim", "bool", "False"),
                        new TestInput ("Credit Bureau Match", "bool", "True")
                    },
                    new List<IScenario> {
                        new Scenario("given a borrower with no notices, judgements or defaults and an empirica equal to 575",
                            new List<ITestInput> { 

                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Borrower", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages"),
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    }),
                new TestCase("when no empirica score is available", 
                    new List<ITestInput> { 

                        new TestInput ("Applicant Empirica", "int", "-999"),
                        new TestInput ("Number of Judgments within Last 3 Years", "int", "0"),
                        new TestInput ("Aggregated Judgment Value within Last 3 Years", "float", "0"),
                        new TestInput ("NonSettled Aggregated Judgment Value within Last 3 Years", "float", "0"),
                        new TestInput ("Number of Unsettled Defaults within Last 2 Years", "int", "0"),
                        new TestInput ("Sequestration Notice", "bool", "False"),
                        new TestInput ("Administration Order Notice", "bool", "False"),
                        new TestInput ("Debt Counselling Notice", "bool", "False"),
                        new TestInput ("Debt Review Notice", "bool", "False"),
                        new TestInput ("Consumer Deceased Notification", "bool", "False"),
                        new TestInput ("Credit Card Revoked", "bool", "False"),
                        new TestInput ("Consumer Absconded", "bool", "False"),
                        new TestInput ("Paid Out on Deceased Claim", "bool", "False"),
                        new TestInput ("Credit Bureau Match", "bool", "True")
                    },
                    new List<IScenario> {
                        new Scenario("given an empirica score of -999",
                            new List<ITestInput> { 

                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Eligible Borrower", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Your credit score is below the SA Home Loans credit policy limit in terms of the loan details supplied.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    })
            };
        }
    }
}


 