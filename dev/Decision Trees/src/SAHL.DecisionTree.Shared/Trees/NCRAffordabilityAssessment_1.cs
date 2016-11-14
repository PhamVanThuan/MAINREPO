
using Microsoft.Scripting.Hosting;
using SAHL.Core.SystemMessages;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Reflection;
using System.Dynamic;
using SAHL.DecisionTree.Shared.Interfaces;
using SAHL.DecisionTree.Shared.Core;
using SAHL.DecisionTree.Shared.Helpers;

namespace SAHL.DecisionTree.Shared.Trees
{
    public class NCRAffordabilityAssessment_1 : IDecisionTree
    {
        private int currentNodeId;
        private bool currentResult;
        private ScriptScope scope;
        private bool nodeExecutionResultedInError;
        private dynamic variablesCollection;
        private ISystemMessageCollection systemMessageCollection;

        private Dictionary<string, ISystemMessageCollection> SubtreeMessagesDictionary { get; set; }
        private List<string> SubtreeMessagesToClear { get; set; }
        
        public List<Link> NodeLinks {get; private set;}
        public Dictionary<int, Node> Nodes {get; private set;}
        public QueryGlobalsVersion GlobalsVersion { get; protected set; }

        public NCRAffordabilityAssessment_1(ISystemMessageCollection messages)
        {
            this.NodeLinks = new List<Link>() { new Link(0,-2,-1,LinkType.DecisionYes), new Link(1,1,-11,LinkType.Standard), new Link(2,-2,-4,LinkType.DecisionNo), new Link(3,-4,-5,LinkType.DecisionYes), new Link(4,-4,-6,LinkType.DecisionNo), new Link(5,-6,-7,LinkType.DecisionYes), new Link(6,-6,-8,LinkType.DecisionNo), new Link(7,-8,-9,LinkType.DecisionYes), new Link(8,-8,-10,LinkType.DecisionNo), new Link(9,-11,-2,LinkType.Standard), new Link(10,-1,-3,LinkType.Standard), new Link(11,-5,-13,LinkType.Standard), new Link(12,-7,-14,LinkType.Standard), new Link(13,-9,-15,LinkType.Standard), new Link(14,-10,-16,LinkType.Standard)};
            this.Nodes = new Dictionary<int, Node>() {{1, new Node(1,"Start",NodeType.Start,@"")},{-2, new Node(-2,"If Income <= R800",NodeType.Decision,@"if Variables::inputs.GrossClientMonthlyIncome <= 800 then_newline_ Variables::outputs.NodeResult = true_newline_else_newline_ Variables::outputs.NodeResult = false_newline_end")},{-1, new Node(-1,"Process",NodeType.Process,@"Variables::outputs.MinMonthlyFixedExpenses = 0")},{-4, new Node(-4,"If Income > R800 and <= R6250",NodeType.Decision,@"if Variables::inputs.GrossClientMonthlyIncome > 800 and_newline_   Variables::inputs.GrossClientMonthlyIncome <= 6250  then_newline_   Variables::outputs.NodeResult = true_newline_else_newline_   Variables::outputs.NodeResult = false_newline_end")},{-5, new Node(-5,"Process",NodeType.Process,@"Variables::outputs.MinMonthlyFixedExpenses = 800 + (0.0675* (Variables::inputs.GrossClientMonthlyIncome - 800.01))_newline_Variables::outputs.MinMonthlyFixedExpenses = (Variables::outputs.MinMonthlyFixedExpenses)_newline_Variables::outputs.MinMonthlyFixedExpenses = _quote_%.2f_quote_ % Variables::outputs.MinMonthlyFixedExpenses_newline_Variables::outputs.MinMonthlyFixedExpenses = _quote_%.0f_quote_ % Variables::outputs.MinMonthlyFixedExpenses_newline_")},{-6, new Node(-6,"If Income > R6250 and <= R25000",NodeType.Decision,@"if Variables::inputs.GrossClientMonthlyIncome > 6250 and_newline_   Variables::inputs.GrossClientMonthlyIncome <= 25000  then_newline_   Variables::outputs.NodeResult = true_newline_else_newline_   Variables::outputs.NodeResult = false_newline_end")},{-7, new Node(-7,"Process",NodeType.Process,@"Variables::inputs.GrossClientMonthlyIncome = (Variables::inputs.GrossClientMonthlyIncome - 6250.01)_newline_Variables::outputs.MinMonthlyFixedExpenses = 1167.88 + (0.09*  Variables::inputs.GrossClientMonthlyIncome)_newline_Variables::outputs.MinMonthlyFixedExpenses = _quote_%.2f_quote_ % Variables::outputs.MinMonthlyFixedExpenses_newline_Variables::outputs.MinMonthlyFixedExpenses = _quote_%.0f_quote_ % Variables::outputs.MinMonthlyFixedExpenses_newline__newline__newline_")},{-8, new Node(-8," If Income > R25000 and <= R50000",NodeType.Decision,@"if Variables::inputs.GrossClientMonthlyIncome > 25000 and_newline_   Variables::inputs.GrossClientMonthlyIncome <= 50000  then_newline_   Variables::outputs.NodeResult = true_newline_else_newline_   Variables::outputs.NodeResult = false_newline_end")},{-9, new Node(-9,"Process",NodeType.Process,@"Variables::inputs.GrossClientMonthlyIncome = (Variables::inputs.GrossClientMonthlyIncome - 25000.01)_newline_Variables::outputs.MinMonthlyFixedExpenses = 2855.38 + (0.082*  Variables::inputs.GrossClientMonthlyIncome)_newline_Variables::outputs.MinMonthlyFixedExpenses = _quote_%.2f_quote_ % Variables::outputs.MinMonthlyFixedExpenses_newline_Variables::outputs.MinMonthlyFixedExpenses = _quote_%.0f_quote_ % Variables::outputs.MinMonthlyFixedExpenses")},{-10, new Node(-10,"Process",NodeType.Process,@"Variables::inputs.GrossClientMonthlyIncome = (Variables::inputs.GrossClientMonthlyIncome - 50000.01)_newline_Variables::outputs.MinMonthlyFixedExpenses = 4905.38 + (0.0675*  Variables::inputs.GrossClientMonthlyIncome)_newline_Variables::outputs.MinMonthlyFixedExpenses = _quote_%.2f_quote_ % Variables::outputs.MinMonthlyFixedExpenses_newline_Variables::outputs.MinMonthlyFixedExpenses = _quote_%.0f_quote_ % Variables::outputs.MinMonthlyFixedExpenses")},{-11, new Node(-11,"Process",NodeType.Process,@"Variables::outputs.MinMonthlyFixedExpenses = 0_newline_")},{-3, new Node(-3,"End",NodeType.End,@"")},{-13, new Node(-13,"End",NodeType.End,@"")},{-14, new Node(-14,"End",NodeType.End,@"")},{-15, new Node(-15,"End",NodeType.End,@"")},{-16, new Node(-16,"End",NodeType.End,@"")}};
            this.systemMessageCollection = messages;
            SubtreeMessagesDictionary = new Dictionary<string, ISystemMessageCollection>();
            SubtreeMessagesToClear = new List<string>();
        }		
    }
}