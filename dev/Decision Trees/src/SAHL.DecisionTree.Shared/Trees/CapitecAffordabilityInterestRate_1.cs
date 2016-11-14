
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
    public class CapitecAffordabilityInterestRate_1 : IDecisionTree
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

        public CapitecAffordabilityInterestRate_1(ISystemMessageCollection messages)
        {
            this.NodeLinks = new List<Link>() { new Link(0,1,-9,LinkType.Standard), new Link(1,-2,-1,LinkType.DecisionYes), new Link(2,-2,-4,LinkType.DecisionNo), new Link(3,-9,-2,LinkType.DecisionYes), new Link(4,-9,-13,LinkType.DecisionNo), new Link(5,-11,-5,LinkType.DecisionYes), new Link(6,-11,-12,LinkType.DecisionNo), new Link(7,-4,-12,LinkType.Standard), new Link(8,-1,-12,LinkType.Standard), new Link(9,-5,-10,LinkType.Standard), new Link(10,-13,-14,LinkType.DecisionYes), new Link(11,-13,-3,LinkType.DecisionNo), new Link(12,-14,-11,LinkType.DecisionYes), new Link(13,-14,-3,LinkType.DecisionNo)};
            this.Nodes = new Dictionary<int, Node>() {{1, new Node(1,"Start",NodeType.Start,@"")},{-2, new Node(-2,"Household Income < Alpha Maximum Household Income",NodeType.Decision,@"if (Variables::inputs.HouseholdIncome <= Variables::capitec::credit::alpha.MaximumHouseholdIncome) then_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Variables::outputs.NodeResult = false_newline_end _newline_  ")},{-1, new Node(-1,"Set Interest Rate = Alpha Category 7  + JIBAR",NodeType.Process,@"Variables::outputs.InterestRate = (Variables::capitec::credit::alpha::category7.CategoryLinkRate).to_f + (Variables::sAHomeLoans::rates.JIBAR3MonthRounded).to_f")},{-4, new Node(-4,"Set Interest Rate = Salaried Category 4  + JIBAR",NodeType.Process,@"Variables::outputs.InterestRate = (Variables::capitec::credit::salaried::category4.CategoryLinkRate).to_f + (Variables::sAHomeLoans::rates.JIBAR3MonthRounded).to_f_newline__newline_")},{-5, new Node(-5,"Calculate Loan Amount",NodeType.Process,@"Variables::outputs.TermInMonths = 240_newline_Variables::outputs.AmountQualifiedFor = 0_newline_Variables::outputs.InterestRate = Variables::inputs.CalcRate_newline_Variables::outputs.Instalment = (Variables::capitec.AffordabilityMaximumPTI * Variables::inputs.HouseholdIncome).to_f_newline_Variables::outputs.AmountQualifiedFor = (((Variables::outputs.Instalment).to_f * (  ((((Variables::inputs.CalcRate / 12.0).to_f + 1.0) ** (Variables::outputs.TermInMonths).to_f) - 1.0) / ((Variables::inputs.CalcRate / 12.0).to_f * ((1.0 + (Variables::inputs.CalcRate / 12.0).to_f) ** (Variables::outputs.TermInMonths).to_f )) )) - 1).round_newline_Variables::outputs.PaymentToIncome = (Variables::outputs.Instalment / Variables::inputs.HouseholdIncome).truncate_to(3)_newline_Variables::outputs.PropertyPriceQualifiedFor = Variables::outputs.AmountQualifiedFor + Variables::inputs.Deposit_newline_")},{-9, new Node(-9,"Interest Rate Query == true",NodeType.Decision,@"if Variables::inputs.InterestRateQuery == true  then_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Variables::outputs.NodeResult = false_newline_end _newline_  ")},{-11, new Node(-11,"Interest Rate > 0",NodeType.Decision,@"if Variables::inputs.CalcRate > 0.0_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Messages.AddWarning(_sgl_quote_Interest rate must be greater than zero._sgl_quote_)_newline_  Variables::outputs.NodeResult = false_newline_end ")},{-12, new Node(-12,"End",NodeType.End,@"")},{-10, new Node(-10,"End",NodeType.End,@"")},{-13, new Node(-13,"Household Income > Affordability Minimum Household Income",NodeType.Decision,@"if (Variables::inputs.HouseholdIncome >= Variables::capitec.AffordabilityMinimumHouseholdIncome) then_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Messages.AddWarning(Messages::capitec.AffordabilityMinimumHouseholdIncome)_newline_  Variables::outputs.NodeResult = false_newline_end ")},{-14, new Node(-14,"Household Income < Affordability Maximum Household Income",NodeType.Decision,@"if (Variables::inputs.HouseholdIncome <= Variables::capitec.AffordabilityMaximumHouseholdIncome) then_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Messages.AddWarning(Messages::capitec.AffordabilityMaximumHouseholdIncome)_newline_  Variables::outputs.NodeResult = false_newline_end ")},{-3, new Node(-3,"End",NodeType.End,@"")}};
            this.systemMessageCollection = messages;
            SubtreeMessagesDictionary = new Dictionary<string, ISystemMessageCollection>();
            SubtreeMessagesToClear = new List<string>();
        }		
    }
}