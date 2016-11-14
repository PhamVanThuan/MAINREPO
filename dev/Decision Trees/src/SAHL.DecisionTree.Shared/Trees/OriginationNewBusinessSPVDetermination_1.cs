
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
    public class OriginationNewBusinessSPVDetermination_1 : IDecisionTree
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

        public OriginationNewBusinessSPVDetermination_1(ISystemMessageCollection messages)
        {
            this.NodeLinks = new List<Link>() { new Link(0,1,-2,LinkType.Standard), new Link(1,-2,-3,LinkType.DecisionYes), new Link(2,-3,-4,LinkType.DecisionYes), new Link(3,-4,-6,LinkType.DecisionYes), new Link(4,-4,-7,LinkType.DecisionNo), new Link(5,-3,-5,LinkType.DecisionNo), new Link(6,-5,-9,LinkType.DecisionYes), new Link(7,-5,-10,LinkType.DecisionNo), new Link(8,-2,-11,LinkType.DecisionNo), new Link(9,-11,-13,LinkType.DecisionYes), new Link(10,-11,-12,LinkType.DecisionNo), new Link(11,-12,-1,LinkType.DecisionYes), new Link(12,-13,-15,LinkType.DecisionYes), new Link(13,-13,-14,LinkType.DecisionNo), new Link(14,-12,-20,LinkType.DecisionNo), new Link(15,-1,-21,LinkType.Standard), new Link(16,-21,-16,LinkType.DecisionYes), new Link(17,-21,-19,LinkType.DecisionNo), new Link(18,-14,-22,LinkType.Standard), new Link(19,-22,-18,LinkType.DecisionYes), new Link(20,-22,-17,LinkType.DecisionNo)};
            this.Nodes = new Dictionary<int, Node>() {{1, new Node(1,"Start",NodeType.Start,@"")},{-2, new Node(-2,"Is Capitec?",NodeType.Decision,@"")},{-3, new Node(-3,"Is Alpha?",NodeType.Decision,@"")},{-4, new Node(-4,"Is Developer?",NodeType.Decision,@"")},{-5, new Node(-5,"LTV > 80%?",NodeType.Decision,@"")},{-6, new Node(-6,"Old Mutual Developer",NodeType.End,@"")},{-7, new Node(-7,"Old Mutual Alpha",NodeType.End,@"")},{-9, new Node(-9,"Calibre",NodeType.End,@"")},{-10, new Node(-10,"Main Street 65",NodeType.End,@"")},{-11, new Node(-11,"Is Alpha?",NodeType.Decision,@"")},{-12, new Node(-12,"LTV > 80%?",NodeType.Decision,@"")},{-13, new Node(-13,"Is Developer?",NodeType.Decision,@"")},{-1, new Node(-1,"Calibre_BB_Allocation",NodeType.Process,@"")},{-14, new Node(-14,"OM_BB_Allocation",NodeType.Process,@"")},{-15, new Node(-15,"Old Mutual Developer",NodeType.End,@"")},{-16, new Node(-16,"Blue Banner",NodeType.End,@"")},{-17, new Node(-17,"Old Mutual Alpha",NodeType.End,@"")},{-18, new Node(-18,"Blue Banner Alpha",NodeType.End,@"")},{-19, new Node(-19,"Calibre",NodeType.End,@"")},{-20, new Node(-20,"Main Street 65",NodeType.End,@"")},{-21, new Node(-21,"Blue Banner?",NodeType.Decision,@"")},{-22, new Node(-22,"Blue Banner?",NodeType.Decision,@"")}};
            this.systemMessageCollection = messages;
            SubtreeMessagesDictionary = new Dictionary<string, ISystemMessageCollection>();
            SubtreeMessagesToClear = new List<string>();
        }		
    }
}