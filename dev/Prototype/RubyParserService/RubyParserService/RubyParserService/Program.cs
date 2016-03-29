using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using SAHL.DecisionTree.Shared;
using SAHL.DecisionTree;
using SAHL.Core.SystemMessages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RubyParserService
{
    public class Program
    {

        public static void CreateDecisionTreeFromJSON(string jsonTree, Variables variables, SystemMessageCollection systemMessagesCollection, int startNodeId)
        {

            #region create ruby runtime and setup varriables

            ScriptRuntime runtime = IronRuby.Ruby.CreateRuntime();
            var scriptScope = runtime.CreateScope("Ruby");

            var messages = new Messages(systemMessagesCollection);
            var treeNodes = new Dictionary<int, Node>();

            scriptScope.Engine.Runtime.Globals.SetVariable("Variables", variables);
            scriptScope.Engine.Runtime.Globals.SetVariable("Messages", messages);

            #endregion create ruby runtime and setup varriables

            var results = JsonConvert.DeserializeObject<dynamic>(jsonTree);

            foreach (var node in results.decision_tree.tree.nodes)
            {
                treeNodes.Add(Convert.ToInt32(node.id),new Node(Convert.ToInt32(node.id),(NodeType)Enum.Parse(typeof(NodeType), Convert.ToString(node.type)),Convert.ToString(node.code)));
            }

            var links = new List<Link>();

            foreach (var link in results.decision_tree.tree.links)
            { 
                links.Add(new Link(Convert.ToInt32(link.id), Convert.ToInt32(link.fromNodeId), Convert.ToInt32(link.toNodeId), (LinkType)Enum.Parse(typeof(LinkType),Convert.ToString(link.type))));
            }

            var creditDecisionTree = new DecisionTree(treeNodes, links, startNodeId, scriptScope, systemMessagesCollection);
            creditDecisionTree.Step();
        }

        private static void Main(string[] args)
        {
            #region json
            string jsonTree = @"{
	            'decision_tree': {
		            'id': '1',
		            'name': 'Credit Category Decision',
		            'tree': {
			            'variables': [{
				            'id': '1',
				            'type': 'float',
				            'usage': 'input',
				            'name': 'HouseHold Income'
			            },
			            {
				            'id': '2',
				            'type': 'float',
				            'usage': 'input',
				            'name': 'Loan Purpose'
			            },
			            {
				            'id': '3',
				            'usage': 'input',
				            'type': 'enumeration',
				            'type_enumeration_values': ['Salaried',
				            'Salaried With Deduction',
				            'Self Employed'],
				            'name': 'Household Income Type'
			            },
			            {
				            'id': '4',
				            'usage': 'input',
				            'type': 'float',
				            'name': 'Loan Amount'
			            },
			            {
				            'id': '5',
				            'usage': 'input',
				            'type': 'float',
				            'name': 'Property Value'
			            },
			            {
				            'id': '6',
				            'usage': 'input',
				            'type': 'float',
				            'name': 'LTV'
			            },
			            {
				            'id': '7',
				            'usage': 'output',
				            'type': 'string',
				            'name': 'CreditCategory'
			            }],
			            'nodes': [{
				            'id': '1',
				            'name': 'Start',
				            'type': 'Start',
				            'required_variables': [],
				            'output_variables': [],
				            'code': ''
			            },
			            {
				            'id': '2',
				            'type': 'Decision',
				            'name': 'Is Salaried',
				            'required_variables': ['3'],
				            'output_variables': [],
				            'code': ''
			            },
			            {
				            'id': '3',
				            'type': 'Decision',
				            'name': 'Is New Purchase',
				            'required_variables': ['2'],
				            'output_variables': [],
				            'code': ''
			            },
			            {
				            'id': '4',
				            'type': 'Decision',
				            'name': 'Is Salaried With Deduction',
				            'required_variables': ['2'],
				            'output_variables': [],
				            'code': ''
			            },
			            {
				            'id': '5',
				            'type': 'Process',
				            'name': 'Check Loan Amount',
				            'required_variables': ['4'],
				            'output_variables': [],
				            'code': ' if Variables.Input.LoanAmount >  Variables.Credit.MaximumSalariedNewPurchaseLoanAmount && Variables.Input.LoanAmount < Variables.Credit.MinimumSalariedNewPurchaseLoanAmount then NodeMessages.AddError(Messages.Credit.SalariedNewPurchaseLoanAmountOutsideLimits) end'
			            },
			            {
				            'id': '6',
				            'type': 'Process',
				            'name': 'Check Household Income',
				            'required_variables': ['4'],
				            'output_variables': [],
				            'code': ' if Variables.Input.HouseholdIncome >  Variables.Credit.MaximumSalariedNewPurchaseHouseholdIncome && Variables.Input.HouseholdIncome < Variables.Credit.MinimumSalariedNewPurchaseHouseholdIncome then NodeMessages.AddError(Messages.Credit.SalariedNewPurchaseHouseholdIncometOutsideLimits) end'
			            },
			            {
				            'id': '7',
				            'type': 'Process',
				            'name': 'Establish Credit Category',
				            'required_variables': ['4',
				            '6'],
				            'output_variables': ['7'],
				            'code': ' if Variables.Input.LTV <  Variables.Credit.Category0SalariedNewPurchaseMaxLTV then Variables.Outputs.CreditCategory = Variables.Credit.Categories.Category0 end'
			            }],
			            'links': [{
				            'id': '1',
				            'type': 'DecisionYes',
				            'fromNodeId': '1',
				            'toNodeId': '2'
			            },
			            {
				            'id': '2',
				            'type': 'DecisionYes',
				            'fromNodeId': '2',
				            'toNodeId': '3'
			            },
			            {
				            'id': '3',
				            'type': 'DecisionNo',
				            'fromNodeId': '2',
				            'toNodeId': '4'
			            },
			            {
				            'id': '4',
				            'type': 'DecisionYes',
				            'fromNodeId': '3',
				            'toNodeId': '5'
			            },
			            {
				            'id': '5',
				            'type': 'Standard',
				            'fromNodeId': '5',
				            'toNodeId': '6'
			            }]
		            },
		            'layout': {
			            'nodes': [{
				            'id': '1',
				            'loc': '0 90'
			            }],
			            'links': [{
				            'linkId': '1',
				            'fromPort': 'L',
				            'toPort': 'R',
				            'visible': true,
				            'points': [460.5,
				            447,
				            470.5,
				            447,
				            470.5,
				            500,
				            367,
				            500,
				            263.5,
				            500,
				            253.5,
				            500],
				            'text': 'False'
			            }]
		            }
	            }
            }";
            #endregion json

            Variables variables = new Variables();
            var systemMessagesCollection = new SystemMessageCollection();

            //parse tree inputs
            variables.inputs.DepositAmount = 120000.0f;
            CreateDecisionTreeFromJSON(jsonTree, variables, systemMessagesCollection,1);
        }
    }
}