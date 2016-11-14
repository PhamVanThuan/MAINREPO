
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
    public class CapitecAlphaCreditPricing_1 : IDecisionTree
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

        public CapitecAlphaCreditPricing_1(ISystemMessageCollection messages)
        {
            this.NodeLinks = new List<Link>() { new Link(0,-29,-56,LinkType.DecisionYes), new Link(1,-29,-28,LinkType.DecisionNo), new Link(2,-56,-30,LinkType.DecisionNo), new Link(3,-54,-61,LinkType.DecisionNo), new Link(4,-61,-63,LinkType.DecisionNo), new Link(5,-63,-65,LinkType.DecisionNo), new Link(6,-55,-54,LinkType.DecisionYes), new Link(7,-55,-31,LinkType.DecisionNo), new Link(8,-56,-55,LinkType.DecisionYes), new Link(9,-65,-32,LinkType.DecisionNo), new Link(10,-317,-34,LinkType.DecisionNo), new Link(11,-316,-35,LinkType.DecisionNo), new Link(12,-315,-36,LinkType.DecisionNo), new Link(13,-306,-33,LinkType.DecisionNo), new Link(14,-54,-2,LinkType.DecisionYes), new Link(15,-214,-67,LinkType.DecisionYes), new Link(16,-277,-145,LinkType.Standard), new Link(17,-215,-69,LinkType.DecisionYes), new Link(18,-279,-146,LinkType.Standard), new Link(19,-216,-70,LinkType.DecisionYes), new Link(20,-281,-147,LinkType.Standard), new Link(21,-217,-71,LinkType.DecisionYes), new Link(22,-283,-148,LinkType.Standard), new Link(23,-145,-40,LinkType.DecisionNo), new Link(24,-146,-41,LinkType.DecisionNo), new Link(25,-147,-38,LinkType.DecisionNo), new Link(26,-148,-37,LinkType.DecisionNo), new Link(27,-217,-306,LinkType.DecisionNo), new Link(28,-306,-71,LinkType.DecisionYes), new Link(29,-216,-315,LinkType.DecisionNo), new Link(30,-315,-70,LinkType.DecisionYes), new Link(31,-215,-316,LinkType.DecisionNo), new Link(32,-316,-69,LinkType.DecisionYes), new Link(33,-214,-317,LinkType.DecisionNo), new Link(34,-317,-67,LinkType.DecisionYes), new Link(35,1,-1,LinkType.Standard), new Link(36,-145,-39,LinkType.DecisionYes), new Link(37,-146,-42,LinkType.DecisionYes), new Link(38,-147,-43,LinkType.DecisionYes), new Link(39,-148,-44,LinkType.DecisionYes), new Link(40,-1,-29,LinkType.Standard), new Link(41,-2,-45,LinkType.DecisionYes), new Link(42,-45,-50,LinkType.DecisionNo), new Link(43,-45,-67,LinkType.DecisionYes), new Link(44,-2,-214,LinkType.DecisionNo), new Link(45,-50,-51,LinkType.DecisionNo), new Link(46,-50,-67,LinkType.DecisionYes), new Link(47,-48,-49,LinkType.DecisionYes), new Link(48,-49,-52,LinkType.DecisionNo), new Link(49,-52,-53,LinkType.DecisionNo), new Link(50,-61,-48,LinkType.DecisionYes), new Link(51,-49,-69,LinkType.DecisionYes), new Link(52,-52,-69,LinkType.DecisionYes), new Link(53,-48,-215,LinkType.DecisionNo), new Link(54,-57,-58,LinkType.DecisionYes), new Link(55,-58,-59,LinkType.DecisionNo), new Link(56,-59,-60,LinkType.DecisionNo), new Link(57,-63,-57,LinkType.DecisionYes), new Link(58,-57,-216,LinkType.DecisionNo), new Link(59,-58,-70,LinkType.DecisionYes), new Link(60,-59,-70,LinkType.DecisionYes), new Link(61,-62,-66,LinkType.DecisionYes), new Link(62,-66,-64,LinkType.DecisionNo), new Link(63,-64,-68,LinkType.DecisionNo), new Link(64,-65,-62,LinkType.DecisionYes), new Link(65,-64,-71,LinkType.DecisionYes), new Link(66,-66,-71,LinkType.DecisionYes), new Link(67,-62,-217,LinkType.DecisionNo), new Link(68,-67,-277,LinkType.DecisionYes), new Link(69,-67,-3,LinkType.DecisionNo), new Link(70,-69,-279,LinkType.DecisionYes), new Link(71,-69,-72,LinkType.DecisionNo), new Link(72,-70,-281,LinkType.DecisionYes), new Link(73,-70,-73,LinkType.DecisionNo), new Link(74,-71,-283,LinkType.DecisionYes), new Link(75,-71,-74,LinkType.DecisionNo)};
            this.Nodes = new Dictionary<int, Node>() {{1, new Node(1,"Start",NodeType.Start,@"")},{-29, new Node(-29,"Household Income <= Alpha Maximum Household Income",NodeType.Decision,@"if (Variables::inputs.HouseholdIncome).truncate_to(2) <= (Variables::capitec::credit::alpha.MaximumHouseholdIncome).truncate_to(2) then_newline_  Variables::outputs.NodeResult = true_newline_else  _newline_  Variables::outputs.EligibleApplication = false_newline_  Messages.AddWarning(Messages::capitec::credit::alpha.HouseholdIncomeAboveMaximum)_newline_  Variables::outputs.NodeResult = false_newline_end")},{-54, new Node(-54,"LTV < Alpha Category 6 Maximum LTV",NodeType.Decision,@"if  (Variables::outputs.LoantoValue).truncate_to(3) < (Variables::capitec::credit::alpha::category6.MaximumLoanToValue).truncate_to(3) then_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Variables::outputs.NodeResult = false_newline_end")},{-55, new Node(-55,"Loan Amount >= Alpha Minimum Loan Amount",NodeType.Decision,@"if (Variables::outputs.LoanAmount).truncate_to(2) >= (Variables::capitec::credit::alpha.MinimumLoanAmount).truncate_to(2) then_newline_  Variables::outputs.Alpha = true_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Variables::outputs.EligibleApplication = false_newline_  Messages.AddWarning(Messages::capitec::credit::alpha.LoanAmountBelowMinimum)_newline_  Variables::outputs.NodeResult = false_newline_end")},{-56, new Node(-56,"Property Value >= Alpha Minimum Property Value",NodeType.Decision,@"if  (Variables::outputs.PropertyValue).truncate_to(2) >= (Variables::capitec::credit::alpha.MinimumPropertyValue).truncate_to(2) then  _newline_  Variables::outputs.NodeResult = true_newline_else_newline_    Variables::outputs.EligibleApplication = false_newline_    Messages.AddWarning(Messages::capitec::credit::alpha.PropertyValueBelowMinimum)_newline_  Variables::outputs.NodeResult = false_newline_end")},{-61, new Node(-61,"LTV < Alpha Category 7 Maximum LTV",NodeType.Decision,@"if (Variables::outputs.LoantoValue).truncate_to(3) < (Variables::capitec::credit::alpha::category7.MaximumLoanToValue).truncate_to(3) then_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Variables::outputs.NodeResult = false_newline_end")},{-63, new Node(-63,"LTV < Alpha Category 8 Maximum LTV",NodeType.Decision,@"if (Variables::outputs.LoantoValue).truncate_to(3) < (Variables::capitec::credit::alpha::category8.MaximumLoanToValue).truncate_to(3) then_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Variables::outputs.NodeResult = false_newline_end")},{-65, new Node(-65,"LTV <= Alpha Category 9 Maximum LTV",NodeType.Decision,@"if (Variables::outputs.LoantoValue).truncate_to(3) < (Variables::capitec::credit::alpha::category9.MaximumLoanToValue).truncate_to(3) then_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  _newline_      _newline_   maximumloanamount = (Variables::outputs.PropertyValue).to_f * (Variables::capitec::credit::alpha::category9.MaximumLoanToValue).to_f_newline_   requiredamounttolowerloanamountby = (((Variables::outputs.LoanAmount).to_f - (maximumloanamount).to_f ) + 1.0).round_newline_   maximumloantovalue = (Variables::capitec::credit::alpha::category9.MaximumLoanToValue * 100).truncate_to(1)_newline_    _newline_   if Variables::inputs.ApplicationType == Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase then_newline_     Messages.AddWarning(Messages::capitec::credit.NewPurchaseLTVaboveMaximum)_newline_   else_newline_     if Variables::inputs.ApplicationType == Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch then_newline_       Messages.AddWarning(Messages::capitec::credit.SwitchLTVaboveMaximum)_newline_     else_newline_       Messages.AddWarning(Messages::capitec::credit.LoantoValueAboveCreditMaximum)_newline_  _tab_ end _newline_  end_newline__newline_  Variables::outputs.EligibleApplication = false_newline_  Variables::outputs.Alpha = false_newline_  Variables::outputs.NodeResult = false_newline_end")},{-67, new Node(-67,"Household Income >= Alpha Minium Household Income",NodeType.Decision,@"if (Variables::inputs.HouseholdIncome).truncate_to(2) >= (Variables::capitec::credit::alpha.MinimumHouseholdIncome).truncate_to(2) then_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Variables::outputs.Alpha = false_newline_  Variables::outputs.EligibleApplication = false_newline_  Messages.AddWarning(Messages::capitec::credit::alpha.HouseholdIncomeBelowMinimum)_newline_  Variables::outputs.NodeResult = false_newline_end")},{-145, new Node(-145,"PTI < Alpha Category 6 Maximum PTI",NodeType.Decision,@"if (Variables::outputs.PaymenttoIncome).truncate_to(3) < ((Variables::capitec::credit::alpha::category6.MaximumPaymentToIncome  * (Variables::capitec::credit.PercentVarianceonPaymentToIncomeRatio + 1.0))).truncate_to(3) then _newline_  Variables::outputs.EligibleApplication = true_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  _newline_  maxpti = (((Variables::capitec::credit::alpha::category6.MaximumPaymentToIncome * (Variables::capitec::credit.PercentVarianceonPaymentToIncomeRatio + 1.0))).truncate_to(3)).to_f_newline_  maximumpti = (maxpti * 100).truncate_to(1)  _newline_  requiredpayment = (maxpti * Variables::inputs.HouseholdIncome).to_f_newline_  maximumloanamount = (((requiredpayment).to_f * (  ((((Variables::outputs.InterestRate / 12.0).to_f + 1.0) ** (Variables::inputs.TermInMonth).to_f) - 1.0) / ((Variables::outputs.InterestRate / 12.0).to_f * ((1.0 + (Variables::outputs.InterestRate / 12.0).to_f) ** (Variables::inputs.TermInMonth).to_f )) )) - 1).round_newline_ requiredhouseholdincome = (((Variables::outputs.Instalment).to_f / maxpti.to_f).to_f + 1.0).round_newline__newline_  Messages.AddWarning(Messages::capitec::credit.PTIaboveMaximum)_newline__newline_  Variables::outputs.Alpha = false_newline_  Variables::outputs.NodeResult = false_newline_end")},{-146, new Node(-146,"PTI < Alpha Category 7 Maximum PTI",NodeType.Decision,@"if (Variables::outputs.PaymenttoIncome).truncate_to(3) < ((Variables::capitec::credit::alpha::category7.MaximumPaymentToIncome  * (Variables::capitec::credit.PercentVarianceonPaymentToIncomeRatio + 1.0))).truncate_to(3) then _newline_  Variables::outputs.EligibleApplication = true_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  _newline_  maxpti = (((Variables::capitec::credit::alpha::category7.MaximumPaymentToIncome * (Variables::capitec::credit.PercentVarianceonPaymentToIncomeRatio + 1.0))).truncate_to(3)).to_f_newline_  maximumpti = (maxpti * 100).truncate_to(1)  _newline_  requiredpayment = (maxpti * Variables::inputs.HouseholdIncome).to_f_newline_  maximumloanamount = (((requiredpayment).to_f * (  ((((Variables::outputs.InterestRate / 12.0).to_f + 1.0) ** (Variables::inputs.TermInMonth).to_f) - 1.0) / ((Variables::outputs.InterestRate / 12.0).to_f * ((1.0 + (Variables::outputs.InterestRate / 12.0).to_f) ** (Variables::inputs.TermInMonth).to_f )) )) - 1).round_newline_  requiredhouseholdincome = (((Variables::outputs.Instalment).to_f / maxpti.to_f).to_f + 1.0).round_newline__newline_  Messages.AddWarning(Messages::capitec::credit.PTIaboveMaximum)_newline__newline_  Variables::outputs.Alpha = false_newline_  Variables::outputs.NodeResult = false_newline_end")},{-147, new Node(-147,"PTI < Alpha Category 8 Maximum PTI",NodeType.Decision,@"if (Variables::outputs.PaymenttoIncome).truncate_to(3) < ((Variables::capitec::credit::alpha::category8.MaximumPaymentToIncome * (Variables::capitec::credit.PercentVarianceonPaymentToIncomeRatio + 1.0))).truncate_to(3) then _newline_  Variables::outputs.EligibleApplication = true_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  _newline_  maxpti = (((Variables::capitec::credit::alpha::category8.MaximumPaymentToIncome * (Variables::capitec::credit.PercentVarianceonPaymentToIncomeRatio + 1.0))).truncate_to(3)).to_f_newline_  maximumpti = (maxpti * 100).truncate_to(1)  _newline_  requiredpayment = (maxpti * Variables::inputs.HouseholdIncome).to_f_newline_  maximumloanamount = (((requiredpayment).to_f * (  ((((Variables::outputs.InterestRate / 12.0).to_f + 1.0) ** (Variables::inputs.TermInMonth).to_f) - 1.0) / ((Variables::outputs.InterestRate / 12.0).to_f * ((1.0 + (Variables::outputs.InterestRate / 12.0).to_f) ** (Variables::inputs.TermInMonth).to_f )) )) - 1).round_newline_  requiredhouseholdincome = (((Variables::outputs.Instalment).to_f / maxpti.to_f).to_f + 1.0).round_newline__newline_  Messages.AddWarning(Messages::capitec::credit.PTIaboveMaximum)_newline__newline_  Variables::outputs.Alpha = false_newline_  Variables::outputs.NodeResult = false_newline_end")},{-148, new Node(-148,"PTI < Alpha Category 9 Maximum PTI",NodeType.Decision,@"if (Variables::outputs.PaymenttoIncome).truncate_to(3) < ((Variables::capitec::credit::alpha::category9.MaximumPaymentToIncome  * (Variables::capitec::credit.PercentVarianceonPaymentToIncomeRatio + 1.0))).truncate_to(3) then _newline_  Variables::outputs.EligibleApplication = true_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  _newline_  maxpti = (((Variables::capitec::credit::alpha::category9.MaximumPaymentToIncome * (Variables::capitec::credit.PercentVarianceonPaymentToIncomeRatio + 1.0))).truncate_to(3)).to_f_newline_  maximumpti = (maxpti * 100).truncate_to(1)  _newline_  requiredpayment = (maxpti * Variables::inputs.HouseholdIncome).to_f_newline_  maximumloanamount = (((requiredpayment).to_f * (  ((((Variables::outputs.InterestRate / 12.0).to_f + 1.0) ** (Variables::inputs.TermInMonth).to_f) - 1.0) / ((Variables::outputs.InterestRate / 12.0).to_f * ((1.0 + (Variables::outputs.InterestRate / 12.0).to_f) ** (Variables::inputs.TermInMonth).to_f )) )) - 1).round_newline_  requiredhouseholdincome = (((Variables::outputs.Instalment).to_f / maxpti.to_f).to_f + 1.0).round_newline_  _newline_  Messages.AddWarning(Messages::capitec::credit.PTIaboveMaximum)_newline__newline_  Variables::outputs.Alpha = false_newline_  Variables::outputs.NodeResult = false_newline_end_newline_  Messages.AddInfo((Variables::capitec::credit::alpha::category9.MaximumPaymentToIncome  * (Variables::capitec::credit.PercentVarianceonPaymentToIncomeRatio + 1.0)).to_s)")},{-214, new Node(-214,"Application Empirica >= (Alpha Category 6 Minimum Empirica less variance %)",NodeType.Decision,@"if Variables::inputs.ApplicationEmpirica >= (Variables::capitec::credit::alpha::category6.MinimumApplicationEmpirica - (Variables::capitec::credit::alpha::category6.MinimumApplicationEmpirica * Variables::capitec::credit.PercentVarianceonCategoryEmpirica)).round_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Variables::outputs.NodeResult = false_newline_end")},{-215, new Node(-215,"Application Empirica >= (Alpha Category 7 Minimum Empirica less variance %)",NodeType.Decision,@"Messages.AddInfo(((Variables::capitec::credit::alpha::category7.MinimumApplicationEmpirica - (Variables::capitec::credit::alpha::category7.MinimumApplicationEmpirica * Variables::capitec::credit.PercentVarianceonCategoryEmpirica)).round).to_s)_newline_if  Variables::inputs.ApplicationEmpirica >= (Variables::capitec::credit::alpha::category7.MinimumApplicationEmpirica - (Variables::capitec::credit::alpha::category7.MinimumApplicationEmpirica * Variables::capitec::credit.PercentVarianceonCategoryEmpirica)).round_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Variables::outputs.NodeResult = false_newline_end")},{-216, new Node(-216,"Application Empirica >= (Alpha Category 8 Minimum Empirica less variance %)",NodeType.Decision,@"if  Variables::inputs.ApplicationEmpirica >= (Variables::capitec::credit::alpha::category8.MinimumApplicationEmpirica - (Variables::capitec::credit::alpha::category8.MinimumApplicationEmpirica * Variables::capitec::credit.PercentVarianceonCategoryEmpirica)).round_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Variables::outputs.NodeResult = false_newline_end")},{-217, new Node(-217,"Application Empirica >= (Alpha Category 9 Minimum  Empirica less variance %)",NodeType.Decision,@"if  Variables::inputs.ApplicationEmpirica >= (Variables::capitec::credit::alpha::category9.MinimumApplicationEmpirica - (Variables::capitec::credit::alpha::category9.MinimumApplicationEmpirica * Variables::capitec::credit.PercentVarianceonCategoryEmpirica)).round_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Variables::outputs.NodeResult = false_newline_end")},{-277, new Node(-277,"Alpha Category 6",NodeType.Process,@"Variables::outputs.LinkRate = Variables::capitec::credit::alpha::category6.CategoryLinkRate_newline_Variables::outputs.InterestRate = Variables::capitec::credit::alpha::category6.CategoryLinkRate + Variables::sAHomeLoans::rates.JIBAR3MonthRounded_newline_Variables::outputs.CreditMatrixCategory = Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory6_newline_Variables::outputs.Alpha = true_newline__newline_Variables::outputs.Instalment = (((Variables::outputs.InterestRate / 12.0) + ((Variables::outputs.InterestRate / 12.0) / ((((Variables::outputs.InterestRate / 12.0) + 1.0) ** Variables::inputs.TermInMonth) - 1))) * Variables::outputs.LoanAmount).truncate_to(2)_newline_Variables::outputs.PaymenttoIncome = (Variables::outputs.Instalment / Variables::inputs.HouseholdIncome).truncate_to(3)_newline__newline_Variables::outputs.InterestRateasPercent =  _sgl_quote_%.2f_sgl_quote_ % ((Variables::outputs.InterestRate).truncate_to(4) * 100.0) + _sgl_quote_%_sgl_quote__newline_Variables::outputs.PaymenttoIncomeasPercent = _sgl_quote_%.1f_sgl_quote_ % (Variables::outputs.PaymenttoIncome * 100.0).truncate_to(1) + _sgl_quote_%_sgl_quote__newline_Variables::outputs.InstallmentinRands = _sgl_quote_%.2f_sgl_quote_ % (Variables::outputs.Instalment).truncate_to(2)_newline__newline_   ")},{-279, new Node(-279,"Alpha Category 7",NodeType.Process,@"Variables::outputs.LinkRate = Variables::capitec::credit::alpha::category7.CategoryLinkRate_newline_Variables::outputs.InterestRate = Variables::capitec::credit::alpha::category7.CategoryLinkRate + Variables::sAHomeLoans::rates.JIBAR3MonthRounded_newline_Variables::outputs.CreditMatrixCategory = Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory7 _newline_Variables::outputs.Alpha = true _newline__newline_Variables::outputs.Instalment = (((Variables::outputs.InterestRate / 12.0) + ((Variables::outputs.InterestRate / 12.0) / ((((Variables::outputs.InterestRate / 12.0) + 1.0) ** Variables::inputs.TermInMonth) - 1))) * Variables::outputs.LoanAmount).truncate_to(2)_newline_Variables::outputs.PaymenttoIncome = (Variables::outputs.Instalment / Variables::inputs.HouseholdIncome).truncate_to(3)_newline__newline_Variables::outputs.InterestRateasPercent =  _sgl_quote_%.2f_sgl_quote_ % ((Variables::outputs.InterestRate).truncate_to(4) * 100.0) + _sgl_quote_%_sgl_quote__newline_Variables::outputs.PaymenttoIncomeasPercent = _sgl_quote_%.1f_sgl_quote_ % (Variables::outputs.PaymenttoIncome * 100.0).truncate_to(1) + _sgl_quote_%_sgl_quote__newline_Variables::outputs.InstallmentinRands = _sgl_quote_%.2f_sgl_quote_ % (Variables::outputs.Instalment).truncate_to(2)")},{-281, new Node(-281,"Alpha Category 8",NodeType.Process,@"Variables::outputs.LinkRate = Variables::capitec::credit::alpha::category8.CategoryLinkRate_newline_Variables::outputs.InterestRate = Variables::capitec::credit::alpha::category8.CategoryLinkRate + Variables::sAHomeLoans::rates.JIBAR3MonthRounded _newline_Variables::outputs.CreditMatrixCategory = Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory8 _newline_Variables::outputs.Alpha = true _newline__newline_Variables::outputs.Instalment = (((Variables::outputs.InterestRate / 12.0) + ((Variables::outputs.InterestRate / 12.0) / ((((Variables::outputs.InterestRate / 12.0) + 1.0) ** Variables::inputs.TermInMonth) - 1))) * Variables::outputs.LoanAmount).truncate_to(2)_newline_Variables::outputs.PaymenttoIncome = (Variables::outputs.Instalment / Variables::inputs.HouseholdIncome).truncate_to(3)_newline__newline_Variables::outputs.InterestRateasPercent =  _sgl_quote_%.2f_sgl_quote_ % ((Variables::outputs.InterestRate).truncate_to(4) * 100.0) + _sgl_quote_%_sgl_quote__newline_Variables::outputs.PaymenttoIncomeasPercent = _sgl_quote_%.1f_sgl_quote_ % (Variables::outputs.PaymenttoIncome * 100.0).truncate_to(1) + _sgl_quote_%_sgl_quote__newline_Variables::outputs.InstallmentinRands = _sgl_quote_%.2f_sgl_quote_ % (Variables::outputs.Instalment).truncate_to(2)_newline_")},{-283, new Node(-283,"Alpha Category 9",NodeType.Process,@"Variables::outputs.LinkRate = Variables::capitec::credit::alpha::category9.CategoryLinkRate_newline_Variables::outputs.InterestRate = Variables::capitec::credit::alpha::category9.CategoryLinkRate + Variables::sAHomeLoans::rates.JIBAR3MonthRounded _newline_Variables::outputs.CreditMatrixCategory = Enumerations::sAHomeLoans::credit::creditMatrixCategory.AlphaCategory9 _newline_Variables::outputs.Alpha = true _newline__newline_Variables::outputs.Instalment = (((Variables::outputs.InterestRate / 12.0) + ((Variables::outputs.InterestRate / 12.0) / ((((Variables::outputs.InterestRate / 12.0) + 1.0) ** Variables::inputs.TermInMonth) - 1))) * Variables::outputs.LoanAmount).truncate_to(2)_newline_Variables::outputs.PaymenttoIncome = (Variables::outputs.Instalment / Variables::inputs.HouseholdIncome).truncate_to(3)_newline__newline_Variables::outputs.InterestRateasPercent =  _sgl_quote_%.2f_sgl_quote_ % ((Variables::outputs.InterestRate).truncate_to(4) * 100.0) + _sgl_quote_%_sgl_quote__newline_Variables::outputs.PaymenttoIncomeasPercent = _sgl_quote_%.1f_sgl_quote_ % (Variables::outputs.PaymenttoIncome * 100.0).truncate_to(1) + _sgl_quote_%_sgl_quote__newline_Variables::outputs.InstallmentinRands = _sgl_quote_%.2f_sgl_quote_ % (Variables::outputs.Instalment).truncate_to(2)")},{-306, new Node(-306,"Application Empirica = -999",NodeType.Decision,@"if Variables::inputs.ApplicationEmpirica == -999_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Variables::outputs.EligibleApplication = false_newline_  Messages.AddWarning(Messages::capitec::credit::alpha.CreditScoreBelowMinimum)_newline_  Variables::outputs.Alpha = false_newline_  Variables::outputs.NodeResult = false_newline_end")},{-315, new Node(-315,"Application Empirica = -999",NodeType.Decision,@"if Variables::inputs.ApplicationEmpirica == -999_newline_  Variables::outputs.NodeResult = true_newline_else_newline_    Variables::outputs.EligibleApplication = false_newline_    Messages.AddWarning(Messages::capitec::credit::alpha.CreditScoreBelowMinimum)_newline__tab_Variables::outputs.Alpha = false_newline_ _tab_Variables::outputs.NodeResult = false_newline_end")},{-316, new Node(-316,"Application Empirica = -999",NodeType.Decision,@"if Variables::inputs.ApplicationEmpirica == -999_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Variables::outputs.EligibleApplication = false_newline_  Messages.AddWarning(Messages::capitec::credit::alpha.CreditScoreBelowMinimum)_newline_  Variables::outputs.Alpha = false_newline_  Variables::outputs.NodeResult = false_newline_end")},{-317, new Node(-317,"Application Empirica = -999",NodeType.Decision,@"if Variables::inputs.ApplicationEmpirica == -999_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Variables::outputs.EligibleApplication = false_newline_  Messages.AddWarning(Messages::capitec::credit::alpha.CreditScoreBelowMinimum)  _newline_  Variables::outputs.Alpha = false_newline_  Variables::outputs.NodeResult = false_newline_end")},{-28, new Node(-28,"End",NodeType.End,@"")},{-30, new Node(-30,"End",NodeType.End,@"")},{-31, new Node(-31,"End",NodeType.End,@"")},{-32, new Node(-32,"End",NodeType.End,@"")},{-33, new Node(-33,"End",NodeType.End,@"")},{-34, new Node(-34,"End",NodeType.End,@"")},{-35, new Node(-35,"End",NodeType.End,@"")},{-36, new Node(-36,"End",NodeType.End,@"")},{-37, new Node(-37,"End",NodeType.End,@"")},{-38, new Node(-38,"End",NodeType.End,@"")},{-39, new Node(-39,"End",NodeType.End,@"")},{-40, new Node(-40,"End",NodeType.End,@"")},{-41, new Node(-41,"End",NodeType.End,@"")},{-42, new Node(-42,"End",NodeType.End,@"")},{-43, new Node(-43,"End",NodeType.End,@"")},{-44, new Node(-44,"End",NodeType.End,@"")},{-1, new Node(-1,"Calculate Loan Amount, Property Value and Loan to Value",NodeType.Process,@"Variables::outputs.EligibleApplication = false_newline_Variables::outputs.InterestRate = 0.00_newline_Variables::outputs.Alpha = false _newline_Variables::outputs.Instalment = 0.00_newline_Variables::outputs.PaymenttoIncome = 0.00_newline__newline_if (Variables::inputs.CapitaliseFees == true) then_newline_  fees = Variables::inputs.Fees_newline_  interiminterest = Variables::inputs.InterimInterest_newline_else_newline_  fees = 0.0_newline_  interiminterest = 0.0_newline_end_newline__newline_if Variables::inputs.ApplicationType == Enumerations::sAHomeLoans::mortgageLoanApplicationType.Switch then_newline_      Variables::outputs.LoanAmount = Variables::inputs.CurrentMortgageLoanBalance + Variables::inputs.CashAmountRequired + fees + interiminterest_newline_      Variables::outputs.PropertyValue = Variables::inputs.EstimatedMarketValueofProperty_newline_end _newline_  _newline_if Variables::inputs.ApplicationType == Enumerations::sAHomeLoans::mortgageLoanApplicationType.NewPurchase then_newline_      Variables::outputs.LoanAmount = Variables::inputs.PropertyPurchasePrice - Variables::inputs.DepositAmount_newline_      Variables::outputs.PropertyValue = Variables::inputs.PropertyPurchasePrice_newline_end _newline__newline_if Variables::outputs.PropertyValue == 0 then_newline_    Variables::outputs.LoantoValue = 9999_newline_    Messages.AddInfo(_sgl_quote_Property Value cannot be zero._sgl_quote_)_newline_else  _newline_    Variables::outputs.LoantoValue =  (Variables::outputs.LoanAmount / Variables::outputs.PropertyValue).truncate_to(3) _newline_end_newline_  _newline_  Variables::outputs.LoantoValueasPercent = _sgl_quote_%.2f_sgl_quote_ % (Variables::outputs.LoantoValue * 100) + _sgl_quote_ %_sgl_quote_ _newline_  _newline__newline__newline_  Variables::outputs.Alpha = false_newline__newline_Variables::outputs.Instalment = 0.0_newline_Variables::outputs.PaymenttoIncome = 0.0_newline__newline_Variables::outputs.InterestRateasPercent =_sgl_quote__sgl_quote__newline_Variables::outputs.PaymenttoIncomeasPercent =_sgl_quote__sgl_quote__newline_Variables::outputs.InstallmentinRands = _sgl_quote__sgl_quote__newline_")},{-2, new Node(-2,"Household Income Type = Salaried with Deduction",NodeType.Decision,@"if Variables::inputs.HouseholdIncomeType == Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Variables::outputs.NodeResult = false_newline_end")},{-45, new Node(-45,"Application Empirica >= (Salary with Deduction  Minimum Empirica less variance %)",NodeType.Decision,@"if Variables::inputs.ApplicationEmpirica >= Variables::capitec::credit::salariedwithDeduction.MinimumApplicationEmpirica - (Variables::capitec::credit::salariedwithDeduction.MinimumApplicationEmpirica * Variables::capitec::credit.PercentVarianceonCategoryEmpirica).round_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Variables::outputs.Alpha = false_newline_  Variables::outputs.NodeResult = false_newline_end")},{-50, new Node(-50,"Application Empirica = -999",NodeType.Decision,@"if Variables::inputs.ApplicationEmpirica == -999_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Variables::outputs.EligibleApplication = false_newline_  Messages.AddWarning(Messages::capitec::credit::alpha.CreditScoreBelowMinimum)_newline_  Variables::outputs.Alpha = false_newline_  Variables::outputs.NodeResult = false_newline_end")},{-51, new Node(-51,"End",NodeType.End,@"")},{-48, new Node(-48,"Household Income Type = Salaried with Deduction",NodeType.Decision,@"if Variables::inputs.HouseholdIncomeType == Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Variables::outputs.NodeResult = false_newline_end")},{-49, new Node(-49,"Application Empirica >= (Salary with Deduction  Minimum Empirica less variance %)",NodeType.Decision,@"if Variables::inputs.ApplicationEmpirica >= Variables::capitec::credit::salariedwithDeduction.MinimumApplicationEmpirica - (Variables::capitec::credit::salariedwithDeduction.MinimumApplicationEmpirica * Variables::capitec::credit.PercentVarianceonCategoryEmpirica).round_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Variables::outputs.Alpha = false_newline_  Variables::outputs.NodeResult = false_newline_end")},{-52, new Node(-52,"Application Empirica = -999",NodeType.Decision,@"if Variables::inputs.ApplicationEmpirica == -999_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Variables::outputs.EligibleApplication = false_newline_  Messages.AddWarning(Messages::capitec::credit::alpha.CreditScoreBelowMinimum)_newline_  Variables::outputs.Alpha = false_newline_  Variables::outputs.NodeResult = false_newline_end")},{-53, new Node(-53,"End",NodeType.End,@"")},{-57, new Node(-57,"Household Income Type = Salaried with Deduction",NodeType.Decision,@"if Variables::inputs.HouseholdIncomeType == Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Variables::outputs.NodeResult = false_newline_end")},{-58, new Node(-58,"Application Empirica >= (Salary with Deduction  Minimum Empirica less variance %)",NodeType.Decision,@"if Variables::inputs.ApplicationEmpirica >= Variables::capitec::credit::salariedwithDeduction.MinimumApplicationEmpirica - (Variables::capitec::credit::salariedwithDeduction.MinimumApplicationEmpirica * Variables::capitec::credit.PercentVarianceonCategoryEmpirica).round_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Variables::outputs.Alpha = false_newline_  Variables::outputs.NodeResult = false_newline_end")},{-59, new Node(-59,"Application Empirica = -999",NodeType.Decision,@"if Variables::inputs.ApplicationEmpirica == -999_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Variables::outputs.EligibleApplication = false_newline_  Messages.AddWarning(Messages::capitec::credit::alpha.CreditScoreBelowMinimum)_newline_  Variables::outputs.Alpha = false_newline_  Variables::outputs.NodeResult = false_newline_end")},{-60, new Node(-60,"End",NodeType.End,@"")},{-62, new Node(-62,"Household Income Type = Salaried with Deduction",NodeType.Decision,@"if Variables::inputs.HouseholdIncomeType == Enumerations::sAHomeLoans::householdIncomeType.SalariedwithDeduction_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Variables::outputs.NodeResult = false_newline_end")},{-64, new Node(-64,"Application Empirica = -999",NodeType.Decision,@"if Variables::inputs.ApplicationEmpirica == -999_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Variables::outputs.EligibleApplication = false_newline_  Messages.AddWarning(Messages::capitec::credit::alpha.CreditScoreBelowMinimum)_newline_  Variables::outputs.Alpha = false_newline_  Variables::outputs.NodeResult = false_newline_end")},{-66, new Node(-66,"Application Empirica >= (Salary with Deduction  Minimum Empirica less variance %)",NodeType.Decision,@"if Variables::inputs.ApplicationEmpirica >= Variables::capitec::credit::salariedwithDeduction.MinimumApplicationEmpirica - (Variables::capitec::credit::salariedwithDeduction.MinimumApplicationEmpirica * Variables::capitec::credit.PercentVarianceonCategoryEmpirica).round_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Variables::outputs.Alpha = false_newline_  Variables::outputs.NodeResult = false_newline_end")},{-68, new Node(-68,"End",NodeType.End,@"")},{-69, new Node(-69,"Household Income >= Alpha Minium Household Income",NodeType.Decision,@"if (Variables::inputs.HouseholdIncome).truncate_to(2) >= (Variables::capitec::credit::alpha.MinimumHouseholdIncome).truncate_to(2) then_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Variables::outputs.Alpha = false_newline_  Variables::outputs.EligibleApplication = false_newline_  Messages.AddWarning(Messages::capitec::credit::alpha.HouseholdIncomeBelowMinimum)_newline_  Variables::outputs.NodeResult = false_newline_end")},{-70, new Node(-70,"Household Income >= Alpha Minium Household Income",NodeType.Decision,@"if (Variables::inputs.HouseholdIncome).truncate_to(2) >= (Variables::capitec::credit::alpha.MinimumHouseholdIncome).truncate_to(2) then_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Variables::outputs.Alpha = false_newline_  Variables::outputs.EligibleApplication = false_newline_  Messages.AddWarning(Messages::capitec::credit::alpha.HouseholdIncomeBelowMinimum)_newline_  Variables::outputs.NodeResult = false_newline_end")},{-71, new Node(-71,"Household Income >= Alpha Category 9 Minium Household Income",NodeType.Decision,@"if (Variables::inputs.HouseholdIncome).truncate_to(2) >= (Variables::capitec::credit::alpha::category9.MinimumHouseholdIncome).truncate_to(2) then_newline_  Variables::outputs.NodeResult = true_newline_else_newline_  Variables::outputs.Alpha = false_newline_  Variables::outputs.EligibleApplication = false_newline_  Messages.AddWarning(Messages::capitec::credit::alpha.HouseholdIncomeBelowMinimum)  _newline_  Variables::outputs.NodeResult = false_newline_end")},{-3, new Node(-3,"End",NodeType.End,@"")},{-72, new Node(-72,"End",NodeType.End,@"")},{-73, new Node(-73,"End",NodeType.End,@"")},{-74, new Node(-74,"End",NodeType.End,@"")}};
            this.systemMessageCollection = messages;
            SubtreeMessagesDictionary = new Dictionary<string, ISystemMessageCollection>();
            SubtreeMessagesToClear = new List<string>();
        }		
    }
}