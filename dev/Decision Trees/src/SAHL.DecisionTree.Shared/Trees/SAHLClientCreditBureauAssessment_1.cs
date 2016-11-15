
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
    public class SAHLClientCreditBureauAssessment_1 : IDecisionTree
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

        public SAHLClientCreditBureauAssessment_1(ISystemMessageCollection messages)
        {
            this.NodeLinks = new List<Link>() { new Link(0,-2,-31,LinkType.DecisionYes), new Link(1,-2,-31,LinkType.DecisionNo), new Link(2,-3,-5,LinkType.DecisionYes), new Link(3,-7,-8,LinkType.DecisionYes), new Link(4,-5,-8,LinkType.DecisionNo), new Link(5,-12,-14,LinkType.DecisionYes), new Link(6,-14,-15,LinkType.DecisionYes), new Link(7,-15,-16,LinkType.DecisionYes), new Link(8,-23,-21,LinkType.DecisionYes), new Link(9,-21,-22,LinkType.DecisionYes), new Link(10,-22,-24,LinkType.DecisionYes), new Link(11,-31,-3,LinkType.DecisionYes), new Link(12,-31,-32,LinkType.DecisionNo), new Link(13,-32,-10,LinkType.DecisionYes), new Link(14,-5,-7,LinkType.DecisionYes), new Link(15,-8,-32,LinkType.DecisionYes), new Link(16,-7,-8,LinkType.DecisionNo), new Link(17,-32,-19,LinkType.DecisionNo), new Link(18,-10,-19,LinkType.DecisionYes), new Link(19,-10,-19,LinkType.DecisionNo), new Link(20,-24,-2,LinkType.DecisionYes), new Link(21,-8,-32,LinkType.DecisionNo), new Link(22,-16,-23,LinkType.DecisionYes), new Link(23,-12,-14,LinkType.DecisionNo), new Link(24,-14,-15,LinkType.DecisionNo), new Link(25,-15,-16,LinkType.DecisionNo), new Link(26,-16,-23,LinkType.DecisionNo), new Link(27,-23,-21,LinkType.DecisionNo), new Link(28,-21,-22,LinkType.DecisionNo), new Link(29,-22,-24,LinkType.DecisionNo), new Link(30,-24,-2,LinkType.DecisionNo), new Link(31,-3,-5,LinkType.DecisionNo), new Link(32,1,-20,LinkType.Standard), new Link(33,-20,-12,LinkType.DecisionYes), new Link(34,-20,-19,LinkType.DecisionNo)};
            this.Nodes = new Dictionary<int, Node>() {{1, new Node(1,"Start",NodeType.Start,@"")},{-2, new Node(-2,"Applicant Empirica < 575",NodeType.Decision,@"if Variables::inputs.ApplicantEmpirica < 574 then_newline_  Messages.AddWarning(Messages::sAHomeLoans::credit.ApplicantMinimumEmpirica)_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Variables::outputs.NodeResult = false_newline_end_newline_")},{-3, new Node(-3,"No. of Judgments within Last 3 Years >= 4",NodeType.Decision,@"if Variables::inputs.NumberofJudgmentswithinLast3Years >= 4 then_newline_  Messages.AddWarning(Messages::sAHomeLoans::credit.ApplicantMaximumJudgementsinLast3Years)_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Variables::outputs.NodeResult = false_newline_end")},{-5, new Node(-5,"No. of Judgments within Last 3 Years > 2",NodeType.Decision,@"if Variables::inputs.NumberofJudgmentswithinLast3Years > 2 then_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Variables::outputs.NodeResult = false_newline_end")},{-7, new Node(-7,"Aggregated Judgment Value in Last 3 Years >= 10000",NodeType.Decision,@"if Variables::inputs.AggregatedJudgmentValuewithinLast3Years >= 10000 then_newline_  Messages.AddWarning(Messages::sAHomeLoans::credit.MaximumAggregateJudgementValuewith3JudgementsinLast3Years)_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Variables::outputs.NodeResult = false_newline_end")},{-8, new Node(-8,"Non-Settled Aggregated Judgment Value Between 13-36 Months >= 15000",NodeType.Decision,@"if Variables::inputs.NonSettledAggregatedJudgmentValuewithinLast3Years >=15000 then_newline_  Messages.AddWarning(Messages::sAHomeLoans::credit.MaximumAggregatedJudgementValueUnsettledForBetween13And36Months )_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Variables::outputs.NodeResult = false_newline_end")},{-10, new Node(-10,"No. of Unsettled Defaults within Last 2 Years >=4",NodeType.Decision,@"if Variables::inputs.NumberofUnsettledDefaultswithinLast2Years >=4 then_newline_  Messages.AddWarning(Messages::sAHomeLoans::credit.MaximumNumberOfUnsettledDefaultsWithinPast2Years)_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Variables::outputs.NodeResult = false_newline_end")},{-12, new Node(-12,"Notice: Sequestration = True",NodeType.Decision,@"if Variables::inputs.SequestrationNotice == true then_newline_  Variables::outputs.EligibleBorrower = false_newline_ Messages.AddWarning(Messages::sAHomeLoans::credit.NoticeOfSequestration)_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Variables::outputs.NodeResult = false_newline_end_newline_")},{-14, new Node(-14,"Notice: Admin Order = True",NodeType.Decision,@"if Variables::inputs.AdministrationOrderNotice == true then_newline_  Variables::outputs.EligibleBorrower = false_newline_  Messages.AddWarning(Messages::sAHomeLoans::credit.NoticeOfAdministrationOrder)_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Variables::outputs.NodeResult = false_newline_end")},{-15, new Node(-15,"Notice: Debt Counselling = True",NodeType.Decision,@"if Variables::inputs.DebtCounsellingNotice == true then_newline_  Variables::outputs.EligibleBorrower = false_newline_  Messages.AddWarning(Messages::sAHomeLoans::credit.NoticeOfDebtCounselling)  _newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Variables::outputs.NodeResult = false_newline_end")},{-16, new Node(-16,"Notice: Debt Review = True",NodeType.Decision,@"if Variables::inputs.DebtReviewNotice == true then_newline_  Variables::outputs.EligibleBorrower = false_newline_ Messages.AddWarning(Messages::sAHomeLoans::credit.NoticeOfDebtReview)_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Variables::outputs.NodeResult = false_newline_end")},{-21, new Node(-21,"Credit Card Revoked = True",NodeType.Decision,@"if Variables::inputs.CreditCardRevoked == true then_newline_  Variables::outputs.EligibleBorrower = false_newline_    Messages.AddWarning(Messages::sAHomeLoans::credit.NoticeOfCreditCardRevoked )_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Variables::outputs.NodeResult = false_newline_end")},{-22, new Node(-22,"Consumer Absconded = True",NodeType.Decision,@"if Variables::inputs.ConsumerAbsconded == true then_newline_  Variables::outputs.EligibleBorrower = false_newline_    Messages.AddWarning(Messages::sAHomeLoans::credit.NoticeOfAbsconded)_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Variables::outputs.NodeResult = false_newline_end")},{-23, new Node(-23,"Consumer Deceased Notification= True",NodeType.Decision,@"if Variables::inputs.ConsumerDeceasedNotification == true then_newline_   Variables::outputs.EligibleBorrower = false_newline_   Messages.AddWarning(Messages::sAHomeLoans::credit.NoticeOfConsumerIsDeceased)_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Variables::outputs.NodeResult = false_newline_end")},{-24, new Node(-24,"Deceased Claim Paid Out = True",NodeType.Decision,@"if Variables::inputs.PaidOutonDeceasedClaim == true then_newline_  Variables::outputs.EligibleBorrower = false_newline_  Messages.AddWarning(Messages::sAHomeLoans::credit.NoticeOfPaidOutOnDeceasedClaim)  _newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Variables::outputs.NodeResult = false_newline_end")},{-31, new Node(-31,"No. of Judgments within Last three years > 0",NodeType.Decision,@"if Variables::inputs.NumberofJudgmentswithinLast3Years > 0 then_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Variables::outputs.NodeResult = false_newline_end")},{-32, new Node(-32,"Number of Unsettled Defaults within Last 2 Years > 0",NodeType.Decision,@"if Variables::inputs.NumberofUnsettledDefaultswithinLast2Years > 0 then_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Variables::outputs.NodeResult = false_newline_end")},{-19, new Node(-19,"End",NodeType.End,@"")},{-20, new Node(-20,"Credit Bureau Match",NodeType.Decision,@"Variables::outputs.EligibleBorrower = true_newline__newline_if Variables::inputs.CreditBureauMatch == true then  _newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Variables::outputs.EligibleBorrower = false_newline_  Messages.AddWarning(Messages::sAHomeLoans::credit.NoCreditBureauMatchFound)  _newline_  Variables::outputs.NodeResult = false_newline_end_newline_")}};
            this.systemMessageCollection = messages;
            SubtreeMessagesDictionary = new Dictionary<string, ISystemMessageCollection>();
            SubtreeMessagesToClear = new List<string>();
        }		
    }
}