using System;
using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;

using SAHL.Tools.DecisionTree.TestRunner.Lib.TestResult;
using SAHL.Tools.DecisionTree.TestRunner.Interfaces;
using SAHL.Tools.DecisionTree.TestRunner.Lib.Specs.Fakes;

namespace SAHL.Tools.DecisionTree.TestRunner.Lib.Specs
{
    public class when_processing_tree_output_results_given_a_failing_assertion : WithFakes
    {
        private static TestProcessorFactory factory;
        private static TestProcessor processor;
        private static List<ITestOutput> testOutputs;
        private static List<ITestResult> result;
        private static FakeDecisionTreeVariables treeVariables;

        private Establish context = () =>
        {
            factory = new TestProcessorFactory();
            processor = factory.Processor;

            testOutputs = new List<ITestOutput>
            {
                new TestOutput("Credit Matrix Category", "to equal", "SAHomeLoans::Credit::CreditMatrixCategory","Enumerations::sAHomeLoans::credit::creditMatrixCategory.SalariedCategory0"),
                new TestOutput("Eligible Application", "to equal", "bool", "true"),
                new TestOutput("Instalment", "to equal", "double", "9000")
            };

            treeVariables = new FakeDecisionTreeVariables();
            treeVariables.outputs.CreditMatrixCategory = "Globals.Enumerations.sAHomeLoans.credit.creditMatrixCategory.SalariedCategory0";
            treeVariables.outputs.EligibleApplication = true;
            treeVariables.outputs.Instalment = 10000;
        };

        private Because of = () =>
        {
            result = processor.ProcessDecisionTreeOutputResults(treeVariables, testOutputs, new FakeEnumerations());
        };

        private It should_call_the_assertion_manager_with_the_parsed_enum_for_the_first_test = () =>
        {
            factory.assertionManager.WasToldTo(x => x.AssertOutputTestPassed(testOutputs[0],
                "Globals.Enumerations.sAHomeLoans.credit.creditMatrixCategory.SalariedCategory0",
                "Globals.Enumerations.sAHomeLoans.credit.creditMatrixCategory.SalariedCategory0", typeof(String)));
        };

        private It should_call_the_assertion_manager_for_the_other_two_tests = () =>
        {
            factory.assertionManager.WasToldTo(x => x.AssertOutputTestPassed(testOutputs[1], "true", "True", typeof(bool)));
            factory.assertionManager.WasToldTo(x => x.AssertOutputTestPassed(testOutputs[2], "9000", "10000", typeof(double)));
        };

        private It should_return_the_test_results = () =>
        {
            result.ShouldNotBeNull();
        };

        private It should_return_a_result_for_each_expected_output = () =>
        {
            result.Count.ShouldEqual(3);
        };
    }

    public class FakeDecisionTreeVariables
    {
        public FakeDecisionTreeVariables()
        {
            this.outputs = new FakeDecisionTreeVariablesOutputs();
        }
        public class FakeDecisionTreeVariablesOutputs
        {
            public string CreditMatrixCategory { get;set; }
            public bool EligibleApplication { get;set; }
            public double Instalment { get;set; }
        }
        public FakeDecisionTreeVariablesOutputs outputs { get;set; }
    }
    public class FakeEnumerations
    {
        public class SAHomeLoans
        {
            public class Credit
            {
                public class CreditMatrixCategory
                {
                    public string SalariedCategory0 { get { return "Globals.Enumerations.sAHomeLoans.credit.creditMatrixCategory.SalariedCategory0"; } }
                }
                public CreditMatrixCategory creditMatrixCategory { get;set; }
                public Credit()
                {
                    this.creditMatrixCategory = new CreditMatrixCategory();
                }
            }
            public Credit credit { get;set; }
            public SAHomeLoans()
            {
                this.credit = new Credit();
            }
        }
        public SAHomeLoans sAHomeLoans { get; set; }

        public FakeEnumerations()
        {
            this.sAHomeLoans = new SAHomeLoans();
        }
    }
}