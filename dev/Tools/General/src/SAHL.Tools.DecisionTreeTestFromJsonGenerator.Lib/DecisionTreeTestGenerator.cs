using System;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using SAHL.Tools.DecisionTreeTestFromJsonGenerator.Lib.Models;
using SAHL.Tools.DecisionTreeTestFromJsonGenerator.Lib.Templates;
using Microsoft.CSharp.RuntimeBinder;
using System.IO;

namespace SAHL.Tools.DecisionTreeTestFromJsonGenerator.Lib
{
    public class DecisionTreeTestGenerator
    {
        private List<TestSuiteVariable> testSuiteVariables;

        public void GenerateDecisonTreeTests(string connectionString, bool publishedOnly, List<string> excludedTrees, string outputPath, string nameSpace)
        {
            var decisionTrees = GetDecisionTreeJsonFromDB(connectionString, publishedOnly, excludedTrees);
            foreach (var decisionTree in decisionTrees)
            {
                string testSuiteName = String.Format("{0}_{1}TestSuite", Utilities.UppercaseFirstLetter(Utilities.StripBadChars(decisionTree.Name)), decisionTree.Version);
                var testSuiteJson = JsonConvert.DeserializeObject<dynamic>(decisionTree.Json);

                this.testSuiteVariables = ParseTestSuiteVariables(testSuiteJson);
                List<TestCase> testCases = ParseTestCases(testSuiteJson);
                DecisionTreeTestSuiteTemplate testSuiteTemplate = new DecisionTreeTestSuiteTemplate(nameSpace, Utilities.EscapeQuotes(testSuiteName), testCases);
                var testSuite = testSuiteTemplate.TransformText();

                WriteTestContentToOutputPath(testSuiteName, testSuite, outputPath);
            }
        }

        private List<TestSuiteVariable> ParseTestSuiteVariables(dynamic testSuiteJson)
        {
            var testSuiteVariables = new List<TestSuiteVariable>();
            if (!String.IsNullOrEmpty(Convert.ToString(testSuiteJson.testVariables)))
            {
                foreach(var testVariable in testSuiteJson.testVariables)
                {
                    testSuiteVariables.Add(new TestSuiteVariable(Convert.ToInt32(testVariable.id), Convert.ToString(testVariable.value)));
                }
            }
            return testSuiteVariables;
        }

        private void WriteTestContentToOutputPath(string testName, string testContent, string outputPath)
        {
            using (StreamWriter sw = new StreamWriter(outputPath + "\\" + testName + ".cs", false))
            {
                sw.Write(testContent);
                sw.Flush();
            }
        }

        private List<DecisionTreeDbModel> GetDecisionTreeJsonFromDB(string connectionString, bool publishedOnly, List<string> excludedTrees)
        {
            List<DecisionTreeDbModel> models = new List<DecisionTreeDbModel>();

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                StringBuilder exceptionListBuilder = new StringBuilder();
                foreach (var excludedTree in excludedTrees)
                {
                    exceptionListBuilder.Append(String.Format(", '{0}'", excludedTree));
                }
                var query = new DbQueries().GetDecisionTreeQuery(exceptionListBuilder.ToString(), publishedOnly);

                models = connection.Query<DecisionTreeDbModel>(query).ToList();
            }

            return models;
        }

        private List<TestCase> ParseTestCases(dynamic testSuite)
        {
            List<TestCase> testCases = new List<TestCase>();

            foreach (var testCaseJson in testSuite.testCases)
            {
                var testCaseInputs = ParseInputVariables(testCaseJson.input_values);
                var testCaseScenarios = new List<Scenario>();
                foreach (var scenarioJson in testCaseJson.scenarios)
                {
                    testCaseScenarios.Add(ParseScenario(scenarioJson));
                }
                var testCase = new TestCase(Utilities.EscapeQuotes(Convert.ToString(testCaseJson.name)), testCaseScenarios, testCaseInputs);
                testCases.Add(testCase);
            }

            return testCases;
        }

        private List<InputVariable> ParseInputVariables(dynamic inputVars)
        {
            var inputs = new List<InputVariable>();
            if (inputVars != null)
            {
                foreach (var inputVar in inputVars)
                {
                    var input = new InputVariable(Convert.ToString(inputVar.name), Convert.ToString(inputVar.type), Convert.ToString(inputVar.value));
                    inputs.Add(input);
                }
            }
            return inputs;
        }

        private List<OutputVariable> ParseOutputVariables(dynamic outputVars)
        {
            var outputs = new List<OutputVariable>();
            if (outputVars != null)
            {
                foreach (var outputVar in outputVars)
                {
                    string value = Convert.ToString(outputVar.value);
                    if(outputVar.expectationType == "test-variable")
                    {
                        var testVariableID = Convert.ToInt32(outputVar.value);
                        var testVariable = testSuiteVariables.FirstOrDefault(x => x.ID == testVariableID);
                        if (testVariable == null)
                        {
                            throw new InvalidOperationException(String.Format("Test Variable with ID {0} was not found.", testVariableID));
                        }
                        value = testVariable.Value;
                    }
                    var output = new OutputVariable(Convert.ToString(outputVar.name), Convert.ToString(outputVar.assertion), Convert.ToString(outputVar.type), Convert.ToString(value));
                    outputs.Add(output);
                }
            }
            return outputs;
        }

        private List<OutputMessage> ParseOutputMessages(dynamic messageVars)
        {
            var messages = new List<OutputMessage>();
            if (messageVars != null)
            {
                foreach (var messageVar in messageVars)
                {
                    var messageValue = String.Empty;
                    if (messageVar.message != null)
                    {
                        try
                        {
                            messageValue = Convert.ToString(messageVar.message.value);
                        }
                        catch(RuntimeBinderException ex)
                        { }
                    }
                    var message = new OutputMessage(Convert.ToString(messageVar.assertion), messageValue, Convert.ToString(messageVar.messageCollection));
                    messages.Add(message);
                }
            }
            return messages;

        }

        private List<ExpectedSubtree> ParseSubtrees(dynamic subtreeVars)
        {
            var subtrees = new List<ExpectedSubtree>();
            if (subtreeVars != null)
            {
                foreach (var subtreeVar in subtreeVars)
                {
                    var subtree = new ExpectedSubtree(Convert.ToString(subtreeVar.name), Convert.ToString(subtreeVar.assertion));
                    subtrees.Add(subtree);
                }
            }
            return subtrees;
        }

        private Scenario ParseScenario(dynamic scenarioJson)
        {
            var scenarioInputs = ParseInputVariables(scenarioJson.input_values);
            var scenarioOutputs = ParseOutputVariables(scenarioJson.output_values);
            var scenarioMessages = ParseOutputMessages(scenarioJson.output_messages);
            var scenarioSubtrees = ParseSubtrees(scenarioJson.subtrees);
            var scenario = new Scenario(Utilities.EscapeQuotes(Convert.ToString(scenarioJson.name)), scenarioInputs, scenarioOutputs, scenarioMessages, scenarioSubtrees);
            return scenario;
        }

        public IList<string> GetAllTreesNames(string connectionString)
        {
            IList<string> results = null;
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                results = connection.Query<string>(DbQueries.GetTreeNames()).ToList();
            }
            return results;
        }
    }
}
