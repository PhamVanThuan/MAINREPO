
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
    public class CapitecSelfEmployedCreditPricing_1 : IDecisionTree
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

        public CapitecSelfEmployedCreditPricing_1(ISystemMessageCollection messages)
        {
            this.NodeLinks = new List<Link>() { new Link(0,-19,-338,LinkType.DecisionYes), new Link(1,-19,-41,LinkType.DecisionNo), new Link(2,-27,-50,LinkType.DecisionNo), new Link(3,-73,-74,LinkType.DecisionNo), new Link(4,-74,-53,LinkType.DecisionNo), new Link(5,-81,-165,LinkType.DecisionNo), new Link(6,-322,-206,LinkType.DecisionNo), new Link(7,-338,-27,LinkType.DecisionYes), new Link(8,-338,-339,LinkType.DecisionNo), new Link(9,-327,-348,LinkType.DecisionNo), new Link(10,-73,-46,LinkType.DecisionYes), new Link(11,-27,-73,LinkType.DecisionYes), new Link(12,-74,-49,LinkType.DecisionYes), new Link(13,-349,-266,LinkType.DecisionYes), new Link(14,-266,-81,LinkType.Standard), new Link(15,-154,-2,LinkType.Standard), new Link(16,-156,-155,LinkType.DecisionNo), new Link(17,-203,-154,LinkType.DecisionYes), new Link(18,-2,-156,LinkType.DecisionYes), new Link(19,-2,-169,LinkType.DecisionNo), new Link(20,-170,-171,LinkType.DecisionNo), new Link(21,-169,-170,LinkType.DecisionYes), new Link(22,-169,-3,LinkType.DecisionNo), new Link(23,-203,-322,LinkType.DecisionNo), new Link(24,-322,-154,LinkType.DecisionYes), new Link(25,-349,-327,LinkType.DecisionNo), new Link(26,-327,-266,LinkType.DecisionYes), new Link(27,-81,-44,LinkType.DecisionYes), new Link(28,-156,-42,LinkType.DecisionYes), new Link(29,-170,-45,LinkType.DecisionYes), new Link(30,1,-1,LinkType.Standard), new Link(31,-1,-19,LinkType.Standard), new Link(32,-46,-349,LinkType.DecisionYes), new Link(33,-46,-47,LinkType.DecisionNo), new Link(34,-49,-48,LinkType.DecisionNo), new Link(35,-49,-203,LinkType.DecisionYes)};
            this.Nodes = new Dictionary<int, Node>() {{1, new Node(1,"Start",NodeType.Start,@"")},{-19, new Node(-19,"Loan Amount >= Self-Employed Minimum Loan Amount",NodeType.Decision,@"if (Variables::outputs.LoanAmount).truncate_to(2) >= (Variables::capitec::credit::selfEmployed.MinimumLoanAmount).truncate_to(2) then_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Messages.AddWarning(Messages::capitec::credit::selfEmployed.LoanAmountBelowMinimum)_newline_  Variables::outputs.EligibleApplication = false_newline_  Variables::outputs.NodeResult = false_newline_end_newline__newline_ ")},{-27, new Node(-27,"Household Income >= Self -Employed Minimum Household Income",NodeType.Decision,@"if (Variables::inputs.HouseholdIncome).truncate_to(2) >= (Variables::capitec::credit::selfEmployed.MinimumHouseholdIncome).truncate_to(2) then_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Messages.AddWarning(Messages::capitec::credit::selfEmployed.HouseholdIncomeBelowMinimum)_newline_  Variables::outputs.EligibleApplication = false_newline_  Variables::outputs.NodeResult = false_newline_end")},{-41, new Node(-41,"End",NodeType.End,@"")},{-50, new Node(-50,"End",NodeType.End,@"")},{-73, new Node(-73,"LTV < Self-Employed Category 0 Maximum LTV",NodeType.Decision,@"if (Variables::outputs.LoantoValue).truncate_to(3) < (Variables::capitec::credit::selfEmployed::category0.MaximumLoanToValue).truncate_to(3) then_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Variables::outputs.NodeResult = false_newline_end")},{-74, new Node(-74,"LTV < Self-Employed Switch Category 1 Maximum LTV",NodeType.Decision,@"if (Variables::outputs.LoantoValue).truncate_to(3) < (Variables::capitec::credit::selfEmployed::category1.MaximumLoanToValue).truncate_to(3) then_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  _newline_    _newline_   maximumloanamount = (Variables::outputs.PropertyValue).to_f * (Variables::capitec::credit::selfEmployed::category1.MaximumLoanToValue).to_f_newline_   requiredamounttolowerloanamountby = (((Variables::outputs.LoanAmount).to_f - (maximumloanamount).to_f ) + 1.0).round_newline_   maximumloantovalue = (Variables::capitec::credit::selfEmployed::category1.MaximumLoanToValue * 100).truncate_to(1)_newline_    _newline_   if Variables::inputs.ApplicationType == Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase then_newline_     Messages.AddWarning(Messages::capitec::credit.NewPurchaseLTVaboveMaximum)_newline_   else_newline_     if Variables::inputs.ApplicationType == Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch then_newline_       Messages.AddWarning(Messages::capitec::credit.SwitchLTVaboveMaximum)_newline_     else_newline_       Messages.AddWarning(Messages::capitec::credit.LoantoValueAboveCreditMaximum)_newline_  _tab_ end _newline_  end_newline__newline_  Variables::outputs.EligibleApplication = false_newline_  Variables::outputs.NodeResult = false_newline_end")},{-81, new Node(-81,"PTI < Self-Employed Category 0 Maximum PTI + Variance Percentage",NodeType.Decision,@"if (Variables::outputs.PaymenttoIncome).truncate_to(3) < ((Variables::capitec::credit::selfEmployed::category0.MaximumPaymentToIncome * (Variables::capitec::credit.PercentVarianceonPaymentToIncomeRatio + 1.0))).truncate_to(3) then_newline_  Variables::outputs.EligibleApplication = true_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  _newline_  maxpti = (((Variables::capitec::credit::selfEmployed::category0.MaximumPaymentToIncome * (Variables::capitec::credit.PercentVarianceonPaymentToIncomeRatio + 1.0))).truncate_to(3)).to_f_newline_  maximumpti = (maxpti * 100).truncate_to(1)  _newline_  requiredpayment = (maxpti * Variables::inputs.HouseholdIncome).to_f_newline_  maximumloanamount = (((requiredpayment).to_f * (  ((((Variables::outputs.InterestRate / 12.0).to_f + 1.0) ** (Variables::inputs.TermInMonth).to_f) - 1.0) / ((Variables::outputs.InterestRate / 12.0).to_f * ((1.0 + (Variables::outputs.InterestRate / 12.0).to_f) ** (Variables::inputs.TermInMonth).to_f )) )) - 1).round_newline_  requiredhouseholdincome = (((Variables::outputs.Instalment).to_f / maxpti.to_f).to_f + 1.0).round_newline_  _newline_  Messages.AddWarning(Messages::capitec::credit.PTIaboveMaximum)_newline__newline_  Variables::outputs.EligibleApplication = false_newline_  Variables::outputs.NodeResult = false_newline_end")},{-165, new Node(-165,"End",NodeType.End,@"")},{-203, new Node(-203,"Application Empirica >= Self-Employed Switch Category 1 Minimum Empirica",NodeType.Decision,@"if  Variables::inputs.ApplicationEmpirica >= Variables::capitec::credit::selfEmployed::category1.MinimumApplicationEmpirica_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Variables::outputs.NodeResult = false_newline_end")},{-206, new Node(-206,"End",NodeType.End,@"")},{-266, new Node(-266,"Self-Employed Category 0",NodeType.Process,@"Variables::outputs.LinkRate = Variables::capitec::credit::selfEmployed::category0.CategoryLinkRate_newline_Variables::outputs.InterestRate = Variables::capitec::credit::selfEmployed::category0.CategoryLinkRate + Variables::sAHomeLoans::rates.JIBAR3MonthRounded_newline_Variables::outputs.CreditMatrixCategory = Enumerations::sAHomeLoans::credit::creditMatrixCategory.SelfEmployedCategory0 _newline_Variables::outputs.Alpha = false_newline__newline_Variables::outputs.Instalment = (((Variables::outputs.InterestRate / 12.0) + ((Variables::outputs.InterestRate / 12.0) / ((((Variables::outputs.InterestRate / 12.0) + 1.0) ** Variables::inputs.TermInMonth) - 1))) * Variables::outputs.LoanAmount).truncate_to(2)_newline_Variables::outputs.PaymenttoIncome = (Variables::outputs.Instalment / Variables::inputs.HouseholdIncome).truncate_to(3)_newline__newline_Variables::outputs.InterestRateasPercent =  _sgl_quote_%.2f_sgl_quote_ % ((Variables::outputs.InterestRate).truncate_to(4) * 100.0) + _sgl_quote_%_sgl_quote__newline_Variables::outputs.PaymenttoIncomeasPercent = _sgl_quote_%.1f_sgl_quote_ % (Variables::outputs.PaymenttoIncome * 100.0).truncate_to(1) + _sgl_quote_%_sgl_quote__newline_Variables::outputs.InstallmentinRands = _sgl_quote_%.2f_sgl_quote_ % (Variables::outputs.Instalment).truncate_to(2)_newline_")},{-338, new Node(-338,"Loan Amount <= Self-Employed Maximum Loan Amount",NodeType.Decision,@"if (Variables::outputs.LoanAmount).truncate_to(2)<= (Variables::capitec::credit::selfEmployed.MaximumLoanAmount).truncate_to(2) then_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Messages.AddWarning(Messages::capitec::credit::selfEmployed.LoanAmountAboveMaximum)_newline_  Variables::outputs.EligibleApplication = false_newline_  Variables::outputs.NodeResult = false_newline_end_newline__newline_ ")},{-339, new Node(-339,"End",NodeType.End,@"")},{-348, new Node(-348,"End",NodeType.End,@"")},{-349, new Node(-349,"Application Empirica >= Self-Employed Category 0 Minimum Empirica",NodeType.Decision,@"if  Variables::inputs.ApplicationEmpirica >= Variables::capitec::credit::selfEmployed::category0.MinimumApplicationEmpirica_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Variables::outputs.NodeResult = false_newline_end")},{-154, new Node(-154,"Self-Employed Category 1",NodeType.Process,@"Variables::outputs.LinkRate = Variables::capitec::credit::selfEmployed::category1.CategoryLinkRate_newline_Variables::outputs.InterestRate = Variables::capitec::credit::selfEmployed::category1.CategoryLinkRate + Variables::sAHomeLoans::rates.JIBAR3MonthRounded_newline_Variables::outputs.CreditMatrixCategory = Enumerations::sAHomeLoans::credit::creditMatrixCategory.SelfEmployedCategory1 _newline_Variables::outputs.Alpha = false_newline__newline_Variables::outputs.Instalment = (((Variables::outputs.InterestRate / 12.0) + ((Variables::outputs.InterestRate / 12.0) / ((((Variables::outputs.InterestRate / 12.0) + 1.0) ** Variables::inputs.TermInMonth) - 1))) * Variables::outputs.LoanAmount).truncate_to(2)_newline_Variables::outputs.PaymenttoIncome = (Variables::outputs.Instalment / Variables::inputs.HouseholdIncome).truncate_to(3)_newline__newline_Variables::outputs.InterestRateasPercent =  _sgl_quote_%.2f_sgl_quote_ % ((Variables::outputs.InterestRate).truncate_to(4) * 100.0) + _sgl_quote_%_sgl_quote__newline_Variables::outputs.PaymenttoIncomeasPercent = _sgl_quote_%.1f_sgl_quote_ % (Variables::outputs.PaymenttoIncome * 100.0).truncate_to(1) + _sgl_quote_%_sgl_quote__newline_Variables::outputs.InstallmentinRands = _sgl_quote_%.2f_sgl_quote_ % (Variables::outputs.Instalment).truncate_to(2)_newline__newline_   ")},{-155, new Node(-155,"End",NodeType.End,@"")},{-156, new Node(-156,"PTI < Self-Employed Category 1 Maximum PTI + Variance Percentage",NodeType.Decision,@"if (Variables::outputs.PaymenttoIncome).truncate_to(3) < ((Variables::capitec::credit::selfEmployed::category1::loanSizeRange1.MaximumPaymentToIncome  * (Variables::capitec::credit.PercentVarianceonPaymentToIncomeRatio + 1.0))).truncate_to(3) then _newline_  Variables::outputs.EligibleApplication = true_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  _newline_  maxpti = (((Variables::capitec::credit::selfEmployed::category1::loanSizeRange1.MaximumPaymentToIncome * (Variables::capitec::credit.PercentVarianceonPaymentToIncomeRatio + 1.0))).truncate_to(3)).to_f_newline_  maximumpti = (maxpti * 100).truncate_to(1)  _newline_  requiredpayment = (maxpti * Variables::inputs.HouseholdIncome).to_f_newline_  maximumloanamount = (((requiredpayment).to_f * (  ((((Variables::outputs.InterestRate / 12.0).to_f + 1.0) ** (Variables::inputs.TermInMonth).to_f) - 1.0) / ((Variables::outputs.InterestRate / 12.0).to_f * ((1.0 + (Variables::outputs.InterestRate / 12.0).to_f) ** (Variables::inputs.TermInMonth).to_f )) )) - 1).round_newline_  requiredhouseholdincome = (((Variables::outputs.Instalment).to_f / maxpti.to_f).to_f + 1.0).round_newline__newline_  Messages.AddWarning(Messages::capitec::credit.PTIaboveMaximum)_newline__newline_  Variables::outputs.EligibleApplication = false_newline_  Variables::outputs.NodeResult = false_newline_end")},{-2, new Node(-2,"Loan Amount <= Maximum Loan Amount Loan Size Range 1",NodeType.Decision,@"if (Variables::outputs.LoanAmount).truncate_to(2) <= (Variables::capitec::credit::selfEmployed.MaximumLoanAmountLoanSizeRange1).truncate_to(2) then_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Variables::outputs.NodeResult = false_newline_end")},{-169, new Node(-169,"Loan Amount <= Maximum Loan Amount Loan Size Range 2",NodeType.Decision,@"if (Variables::outputs.LoanAmount).truncate_to(2) <= (Variables::capitec::credit::selfEmployed.MaximumLoanAmountLoanSizeRange2).truncate_to(2) then_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Messages.AddWarning(Messages::capitec::credit::selfEmployed.LoanAmountAboveMaximum)_newline_  Variables::outputs.EligibleApplication = false_newline_  Variables::outputs.NodeResult = false_newline_end")},{-3, new Node(-3,"End",NodeType.End,@"")},{-170, new Node(-170,"PTI < Self-Employed Category 1 Maximum PTI + Variance Percentage",NodeType.Decision,@"if (Variables::outputs.PaymenttoIncome).truncate_to(3) < ((Variables::capitec::credit::selfEmployed::category1::loanSizeRange2.MaximumPaymentToIncome  * (Variables::capitec::credit.PercentVarianceonPaymentToIncomeRatio + 1.0))).truncate_to(3) then _newline_  Variables::outputs.EligibleApplication = true_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  maxpti = (((Variables::capitec::credit::selfEmployed::category1::loanSizeRange2.MaximumPaymentToIncome * (Variables::capitec::credit.PercentVarianceonPaymentToIncomeRatio + 1.0))).truncate_to(3)).to_f_newline_  maximumpti = (maxpti * 100).truncate_to(1)  _newline_  requiredpayment = (maxpti * Variables::inputs.HouseholdIncome).to_f_newline_  maximumloanamount = (((requiredpayment).to_f * (  ((((Variables::outputs.InterestRate / 12.0).to_f + 1.0) ** (Variables::inputs.TermInMonth).to_f) - 1.0) / ((Variables::outputs.InterestRate / 12.0).to_f * ((1.0 + (Variables::outputs.InterestRate / 12.0).to_f) ** (Variables::inputs.TermInMonth).to_f )) )) - 1).round_newline_  requiredhouseholdincome = (((Variables::outputs.Instalment).to_f / maxpti.to_f).to_f + 1.0).round_newline__newline_  Messages.AddWarning(Messages::capitec::credit.PTIaboveMaximum)_newline__newline_  Variables::outputs.EligibleApplication = false_newline_  Variables::outputs.NodeResult = false_newline_end")},{-171, new Node(-171,"End",NodeType.End,@"")},{-322, new Node(-322,"Application Empirica = -999",NodeType.Decision,@"if Variables::inputs.ApplicationEmpirica == -999_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Messages.AddWarning(Messages::capitec::credit::selfEmployed.CreditScoreBelowMinimum)_newline_  Variables::outputs.EligibleApplication = false_newline_  Variables::outputs.NodeResult = false_newline_end")},{-327, new Node(-327,"Application Empirica = -999",NodeType.Decision,@"if Variables::inputs.ApplicationEmpirica == -999_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Messages.AddWarning(Messages::capitec::credit::selfEmployed.CreditScoreBelowMinimum)_newline_  Variables::outputs.EligibleApplication = false_newline_  Variables::outputs.NodeResult = false_newline_end")},{-42, new Node(-42,"End",NodeType.End,@"")},{-44, new Node(-44,"End",NodeType.End,@"")},{-45, new Node(-45,"End",NodeType.End,@"")},{-1, new Node(-1,"Calculate Loan Amount, Property Value and Loan to Value",NodeType.Process,@"Variables::outputs.EligibleApplication = false_newline_Variables::outputs.InterestRate = 0.00_newline_Variables::outputs.Alpha = false _newline_Variables::outputs.Instalment = 0.00_newline_Variables::outputs.PaymenttoIncome = 0.00_newline__newline_if Variables::inputs.CapitaliseFees == true then_newline_  fees = Variables::inputs.Fees_newline_  interiminterest = Variables::inputs.InterimInterest_newline_else_newline_  fees = 0.0_newline_  interiminterest = 0.0_newline_end_newline__newline_if Variables::inputs.ApplicationType == Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch then_newline_      Variables::outputs.LoanAmount = Variables::inputs.CurrentMortgageLoanBalance + Variables::inputs.CashAmountRequired + fees + interiminterest_newline_      Variables::outputs.PropertyValue = Variables::inputs.EstimatedMarketValueofProperty_newline_end _newline_  _newline_if Variables::inputs.ApplicationType == Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase then_newline_      Variables::outputs.LoanAmount = Variables::inputs.PropertyPurchasePrice - Variables::inputs.DepositAmount_newline_      Variables::outputs.PropertyValue = Variables::inputs.PropertyPurchasePrice_newline_end _newline__newline_if Variables::outputs.PropertyValue == 0 then_newline_    Variables::outputs.LoantoValue = 9999_newline_    Messages.AddError(_sgl_quote_Property Value cannot be zero._sgl_quote_)_newline_else  _newline_    Variables::outputs.LoantoValue =  (Variables::outputs.LoanAmount / Variables::outputs.PropertyValue).truncate_to(3)_newline_end_newline_  _newline_  Variables::outputs.LoantoValueasPercent = _sgl_quote_%.2f_sgl_quote_ % (Variables::outputs.LoantoValue * 100) + _sgl_quote_ %_sgl_quote_ ")},{-46, new Node(-46,"Household Income >= Self -Employed Category 0 Minimum Household Income",NodeType.Decision,@"if (Variables::inputs.HouseholdIncome).truncate_to(2) >= (Variables::capitec::credit::selfEmployed::category0.MinimumHouseholdIncome).truncate_to(2) then_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Messages.AddWarning(Messages::capitec::credit::selfEmployed.HouseholdIncomeBelowMinimum)_newline_  Variables::outputs.EligibleApplication = false_newline_  Variables::outputs.NodeResult = false_newline_end")},{-47, new Node(-47,"End",NodeType.End,@"")},{-48, new Node(-48,"End",NodeType.End,@"")},{-49, new Node(-49,"Household Income >= Self -Employed Category 1 Minimum Household Income",NodeType.Decision,@"if (Variables::inputs.HouseholdIncome).truncate_to(2) >= (Variables::capitec::credit::selfEmployed::category1.MinimumHouseholdIncome).truncate_to(2) then_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Messages.AddWarning(Messages::capitec::credit::selfEmployed.HouseholdIncomeBelowMinimum)_newline_  Variables::outputs.EligibleApplication = false_newline_  Variables::outputs.NodeResult = false_newline_end")},{-53, new Node(-53,"End",NodeType.End,@"")}};
            this.systemMessageCollection = messages;
            SubtreeMessagesDictionary = new Dictionary<string, ISystemMessageCollection>();
            SubtreeMessagesToClear = new List<string>();
        }		
    }
}