using System.Collections.Generic;
using SAHL.Tools.DecisionTree.TestRunner.Interfaces;
using SAHL.DecisionTree.TreeTests.Models;

namespace SAHL.DecisionTree.TreeTests
{
    public class ThirtyYearMortgageLoanEligibility_1TestSuite : ITestSuite
    {
        public IEnumerable<ITestCase> TestCases { get; set; }
    
        public ThirtyYearMortgageLoanEligibility_1TestSuite()
        {
            this.TestCases = new List<ITestCase> 
            {
                new TestCase("when Qualifying for 30 Year Loan Term", 
                    new List<ITestInput> { 

                        new TestInput ("Application Blocked By Credit", "bool", "False"),
                        new TestInput ("Product", "SAHomeLoans::MortgageLoanProductType", "Enumerations::sAHomeLoans::mortgageLoanProductType.NewVariable"),
                        new TestInput ("Highest Income Contributor Salary Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.Salaried"),
                        new TestInput ("Highest Income Contributor Age", "int", "36"),
                        new TestInput ("Household Income", "float", "60000"),
                        new TestInput ("Highest Income Contributor Credit Score", "int", "600"),
                        new TestInput ("Current LTV", "float", "0.60"),
                        new TestInput ("Current PTI", "float", "0.25"),
                        new TestInput ("Mortgage Loan Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase"),
                        new TestInput ("Loan Amount", "double", "875000"),
                        new TestInput ("Property Value", "double", "1200000"),
                        new TestInput ("Interest Rate", "double", "0.093"),
                        new TestInput ("Highest Income Contributor Name", "string", "Mr. Test Client"),
                        new TestInput ("Highest Income Contributor Identity", "string", "7707075071089"),
                        new TestInput ("Is Alpha", "bool", "False"),
                        new TestInput ("Interest Only", "bool", "False")
                    },
                    new List<IScenario> {
                        new Scenario("given Credit Disqualifies Application",
                            new List<ITestInput> { 
                                new TestInput("Application Blocked By Credit", "bool", "True")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Qualifies For Thirty Year Loan Term", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Credit declined 30 year term.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given Applicant Passes All Checks",
                            new List<ITestInput> { 
                                new TestInput("Application Blocked By Credit", "bool", "False"),
                                new TestInput("Household Income", "float", "60000"),
                                new TestInput("Loan Amount", "double", "875000"),
                                new TestInput("Property Value", "double", "1200000"),
                                new TestInput("Interest Rate", "double", "0.093")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Qualifies For Thirty Year Loan Term", "to equal", "bool", "True"),
 
                                new TestOutput("Term Thirty Year", "to equal", "int", "360"),
 
                                new TestOutput("Instalment Thirty Year", "to equal", "double", "7421.39"),
 
                                new TestOutput("Loan to Value Thirty Year", "to equal", "double", "0.7291"),
 
                                new TestOutput("Payment to Income Thirty Year", "to equal", "double", "0.1236"),
 
                                new TestOutput("Pricing Adjustment Thirty Year", "to equal", "double", "0.003")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given Loan Purpose Is Not New Purchase",
                            new List<ITestInput> { 
                                new TestInput("Mortgage Loan Application Type", "SAHomeLoans::MortgageLoanApplicationType", "Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Qualifies For Thirty Year Loan Term", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Only New Purchase applications allowed.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given Product is Not New Variable Loan",
                            new List<ITestInput> { 
                                new TestInput("Product", "SAHomeLoans::MortgageLoanProductType", "Enumerations::sAHomeLoans::mortgageLoanProductType.Edge")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Qualifies For Thirty Year Loan Term", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Only New Variable applications allowed.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given Salary Type is Self Employed",
                            new List<ITestInput> { 
                                new TestInput("Highest Income Contributor Salary Type", "SAHomeLoans::HouseholdIncomeType", "Enumerations::sAHomeLoans::householdIncomeType.SelfEmployed")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Qualifies For Thirty Year Loan Term", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Not for Self Employed applications.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given Highest Income Contributor is 45",
                            new List<ITestInput> { 
                                new TestInput("Highest Income Contributor Age", "int", "45")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Qualifies For Thirty Year Loan Term", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given Highest Income Contributor is 46",
                            new List<ITestInput> { 
                                new TestInput("Highest Income Contributor Age", "int", "46"),
                                new TestInput("Highest Income Contributor Name", "string", "Rosie Test"),
                                new TestInput("Highest Income Contributor Identity", "string", "12345678")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Qualifies For Thirty Year Loan Term", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Highest income contributor to be below the age of 45 years.", "should contain", "Warning Messages"),
                                new OutputMessage("Highest Income Contributor is Rosie Test (12345678).", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given Highest Income Contributor is 44 and Income Less than R40,000",
                            new List<ITestInput> { 
                                new TestInput("Highest Income Contributor Age", "int", "44"),
                                new TestInput("Household Income", "float", "39999"),
                                new TestInput("Highest Income Contributor Name", "string", "Rosie Test"),
                                new TestInput("Highest Income Contributor Identity", "string", "12345678")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Qualifies For Thirty Year Loan Term", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Highest income contributor is above the age of 40 years and the household income is below R40 000.00.", "should contain", "Warning Messages"),
                                new OutputMessage("Highest Income Contributor is Rosie Test (12345678).", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given Highest Income Contributor is 44 and Income Equal to R40,000",
                            new List<ITestInput> { 
                                new TestInput("Highest Income Contributor Age", "int", "44"),
                                new TestInput("Household Income", "float", "40000")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Qualifies For Thirty Year Loan Term", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given Highest Income Contributor is 44 and Income Greater than R40,000",
                            new List<ITestInput> { 
                                new TestInput("Highest Income Contributor Age", "int", "44"),
                                new TestInput("Household Income", "float", "40001")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Qualifies For Thirty Year Loan Term", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Error Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given Highest Income Contributor Credit Score Equal to 595",
                            new List<ITestInput> { 
                                new TestInput("Highest Income Contributor Credit Score", "int", "595")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Qualifies For Thirty Year Loan Term", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given Highest Income Contributor Greater than 595",
                            new List<ITestInput> { 
                                new TestInput("Highest Income Contributor Credit Score", "int", "596")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Qualifies For Thirty Year Loan Term", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given Highest Income Contributor Less than 595",
                            new List<ITestInput> { 
                                new TestInput("Highest Income Contributor Credit Score", "int", "594"),
                                new TestInput("Highest Income Contributor Name", "string", "Rosie Test"),
                                new TestInput("Highest Income Contributor Identity", "string", "12345678")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Qualifies For Thirty Year Loan Term", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Highest income contributor credit score is below 595.", "should contain", "Warning Messages"),
                                new OutputMessage("Highest Income Contributor is Rosie Test (12345678).", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given LTV Equal to 95",
                            new List<ITestInput> { 
                                new TestInput("Current LTV", "float", "0.95")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Qualifies For Thirty Year Loan Term", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given LTV Greater than 95",
                            new List<ITestInput> { 
                                new TestInput("Current LTV", "float", "0.96")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Qualifies For Thirty Year Loan Term", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Only available for LTV less than or equal to 95%.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given LTV Less Than 95",
                            new List<ITestInput> { 
                                new TestInput("Current LTV", "float", "0.94")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Qualifies For Thirty Year Loan Term", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given PTI Less Than 30",
                            new List<ITestInput> { 
                                new TestInput("Current PTI", "float", "0.29")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Qualifies For Thirty Year Loan Term", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given PTI Equals 30",
                            new List<ITestInput> { 
                                new TestInput("Current PTI", "float", "0.30")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Qualifies For Thirty Year Loan Term", "to equal", "bool", "True")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("", "should be empty", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given PTI Greater than 30",
                            new List<ITestInput> { 
                                new TestInput("Current PTI", "float", "0.31")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Qualifies For Thirty Year Loan Term", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Current PTI is above 30% for 30 Year term.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given Application is Interest Only",
                            new List<ITestInput> { 
                                new TestInput("Interest Only", "bool", "True")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Qualifies For Thirty Year Loan Term", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("30 year term not available for Interest Only Applications.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given Application is Alpha",
                            new List<ITestInput> { 
                                new TestInput("Is Alpha", "bool", "True")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Qualifies For Thirty Year Loan Term", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("30 year term not available for Alpha Applications.", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given Highest Contributor Age Less Than 18",
                            new List<ITestInput> { 
                                new TestInput("Highest Income Contributor Age", "int", "17"),
                                new TestInput("Highest Income Contributor Name", "string", "Rosie Test"),
                                new TestInput("Highest Income Contributor Identity", "string", "12345678")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Qualifies For Thirty Year Loan Term", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Applicant must be 18 years or older.", "should contain", "Warning Messages"),
                                new OutputMessage("Highest Income Contributor is Rosie Test (12345678).", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given Highest Income Contributor Age Zero",
                            new List<ITestInput> { 
                                new TestInput("Highest Income Contributor Age", "int", "0"),
                                new TestInput("Highest Income Contributor Name", "string", "Rosie Test"),
                                new TestInput("Highest Income Contributor Identity", "string", "12345678")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Qualifies For Thirty Year Loan Term", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Applicant must have valid date of birth captured.", "should contain", "Warning Messages"),
                                new OutputMessage("Highest Income Contributor is Rosie Test (12345678).", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            }),
                        new Scenario("given Highest Income Contributor Age Less Than Zero",
                            new List<ITestInput> { 
                                new TestInput("Highest Income Contributor Age", "int", "-1"),
                                new TestInput("Highest Income Contributor Name", "string", "Rosie Test"),
                                new TestInput("Highest Income Contributor Identity", "string", "12345678")
                            },
                            new List<ITestOutput> {
 
                                new TestOutput("Qualifies For Thirty Year Loan Term", "to equal", "bool", "False")
                            },
                            new List<IOutputMessage> {
                                new OutputMessage("Applicant must have valid date of birth captured.", "should contain", "Warning Messages"),
                                new OutputMessage("Highest Income Contributor is Rosie Test (12345678).", "should contain", "Warning Messages")
                            },
                            new List<ISubtreeExpectation> {

                            })
                    })
            };
        }
    }
}


 