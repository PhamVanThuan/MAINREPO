using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.BusinessModel;

namespace SAHL.Common.BusinessModel.Rules.Trade
{

    #region TradeCompanyMandatory
    [RuleDBTag("TradeCompanyMandatory",
    "TradeCAPMandatory",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Trade.TradeCompanyMandatory")]
    [RuleInfo]
    public class TradeCompanyMandatory : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)
            if (Parameters.Length == 0)
                throw new ArgumentException("The TradeCAPMandatory rule expects a Domain object to be passed.");

            if (!(Parameters[0] is ITrade))
                throw new ArgumentException("The TradeCAPMandatory rule expects the following objects to be passed: ITrade.");

            #endregion

            ITrade trade = Parameters[0] as ITrade;
            if (string.IsNullOrEmpty(trade.Company) || (trade.Company.Trim()).Length == 0)
            {
                string errorMessage = "A Company is Mandatory for a Trade";
                AddMessage(errorMessage, errorMessage, Messages);
                return 0;
            }
            return 1;
        }
    }
    #endregion

    #region TradePremiumMandatory
    [RuleDBTag("TradePremiumMandatory",
    "TradeCAPMandatory",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Trade.TradePremiumMandatory")]
    [RuleInfo]
    public class TradePremiumMandatory : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)
            if (Parameters.Length == 0)
                throw new ArgumentException("The TradeCAPMandatory rule expects a Domain object to be passed.");

            if (!(Parameters[0] is ITrade))
                throw new ArgumentException("The TradeCAPMandatory rule expects the following objects to be passed: ITrade.");

            #endregion

            ITrade trade = Parameters[0] as ITrade;
            if (trade.TradeType == "C" && !trade.Premium.HasValue)
            {
                string errorMessage = "A Premium is Mandatory for a Trade";
                AddMessage(errorMessage, errorMessage, Messages);
                return 0;
            }
            return 1;
        }
    }
    #endregion

    #region TradeCAPMandatory
    [RuleDBTag("TradeCAPMandatory",
          "TradeCAPMandatory",
            "SAHL.Rules.DLL",
          "SAHL.Common.BusinessModel.Rules.Trade.TradeCAPMandatory")]
    [RuleInfo]
    public class TradeCAPMandatory : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)
            if (Parameters.Length == 0)
                throw new ArgumentException("The TradeCAPMandatory rule expects a Domain object to be passed.");

            if (!(Parameters[0] is ITrade))
                throw new ArgumentException("The TradeCAPMandatory rule expects the following objects to be passed: ITrade.");

            #endregion

            ITrade trade = Parameters[0] as ITrade;
            if (trade.TradeType == "C" && trade.CapType == null)
            {
                string errorMessage = "A Cap Type is Mandatory for a Trade Type of CAP";
                AddMessage(errorMessage, errorMessage, Messages);
                return 0;
            }
            return 1;
        }
    }
    #endregion
    
    #region TradeStrikeRateMax
    [RuleDBTag("TradeStrikeRateMax",
         "TradeStrikeRateMax",
            "SAHL.Rules.DLL",
         "SAHL.Common.BusinessModel.Rules.Trade.TradeStrikeRateMax")]
    [RuleInfo]
    public class TradeStrikeRateMax : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)
            if (Parameters.Length == 0)
                throw new ArgumentException("The TradeStrikeRateMax rule expects a Domain object to be passed.");

            if (!(Parameters[0] is ITrade))
                throw new ArgumentException("The TradeStrikeRateMax rule expects the following objects to be passed: ITrade.");

            #endregion

            ITrade trade = Parameters[0] as ITrade;
            if (trade.StrikeRate >= 1) //strike rate stored as a percentage
            {
                string errorMessage = "A Strike Rate for a Trade has to be less than 100%";
                AddMessage(errorMessage, errorMessage, Messages);
            }
            return 0;
        }
    }
    #endregion
    
    #region TradeStrikeRateMin
    [RuleDBTag("TradeStrikeRateMin",
        "TradeStrikeRateMin",
            "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.Trade.TradeStrikeRateMin")]
    [RuleInfo]
    public class TradeStrikeRateMin : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)
            if (Parameters.Length == 0)
                throw new ArgumentException("The TradeStrikeRateMin rule expects a Domain object to be passed.");

            if (!(Parameters[0] is ITrade))
                throw new ArgumentException("The TradeStrikeRateMin rule expects the following objects to be passed: ITrade.");

            #endregion

            ITrade trade = Parameters[0] as ITrade;
            if (trade.StrikeRate <= 0) //strike rate stored as a percentage
            {
                string errorMessage = "A Strike Rate for a Trade has to be greater than 0%";
                AddMessage(errorMessage, errorMessage, Messages);
            }
            return 0;
        }
    }
    #endregion

    #region TradeTermMin
    [RuleDBTag("TradeTermMin",
    "TradeTermMin",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Trade.TradeTermMin")]
    [RuleParameterTag(new string[] { "@TradeTermMin,0,9" })]
    [RuleInfo]
    public class TradeTermMin : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)
            if (Parameters.Length == 0)
                throw new ArgumentException("The TradeTermMin rule expects a Domain object to be passed.");

            if (!(Parameters[0] is ITrade))
                throw new ArgumentException("The TradeTermMin rule expects the following objects to be passed: ITrade.");

            #endregion

            ITrade trade = Parameters[0] as ITrade;

            if (trade.StartDate == new DateTime())
                return 1;

            int iStart = (trade.StartDate.Year * 12) + trade.StartDate.Month;
            int iEnd = (trade.EndDate.Year * 12) + trade.EndDate.Month;
            int term = iEnd - iStart;

            int tradeTermMin = Convert.ToInt32(RuleItem.RuleParameters[0].Value);

            if (term <= tradeTermMin)
            {
                string errorMessage = string.Format("The term of a Trade has to be greater than {0} months", tradeTermMin);
                AddMessage(errorMessage, errorMessage, Messages);
            }
            return 0;
        }
    }
    #endregion

    #region TradeCAPTermMax
    [RuleDBTag("TradeCAPTermMax",
    "TradeCAPTermMax",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Trade.TradeCAPTermMax")]
    [RuleParameterTag(new string[] { "@TradeTermMax,280,9" })]
    [RuleInfo]
    public class TradeCAPTermMax : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)
            if (Parameters.Length == 0)
                throw new ArgumentException("The TradeCAPTermMax rule expects a Domain object to be passed.");

            if (!(Parameters[0] is ITrade))
                throw new ArgumentException("The TradeCAPTermMax rule expects the following objects to be passed: ITrade.");

            #endregion

            ITrade trade = Parameters[0] as ITrade;

            if (trade.StartDate == new DateTime())
                return 1;

            int iStart = (trade.StartDate.Year * 12) + trade.StartDate.Month;
            int iEnd = (trade.EndDate.Year * 12) + trade.EndDate.Month;
            int term = iEnd - iStart;
            int tradeTermMax = Convert.ToInt32(RuleItem.RuleParameters[0].Value);

            if (term > tradeTermMax)
            {
                string errorMessage = string.Format("The term of a Trade cannot to be greater than {0} months", tradeTermMax);
                AddMessage(errorMessage, errorMessage, Messages);
            }
            return 0;
        }
    }
    #endregion

    #region TradeTradeBalanceMin
    [RuleDBTag("TradeTradeBalanceMin",
    "TradeTradeBalanceMin",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Trade.TradeTradeBalanceMin")]
    [RuleParameterTag(new string[] { "@TradeTradeBalanceMin,100000.00,7" })]
    [RuleInfo]
    public class TradeTradeBalanceMin : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)
            if (Parameters.Length == 0)
                throw new ArgumentException("The TradeTradeBalanceMin rule expects a Domain object to be passed.");

            if (!(Parameters[0] is ITrade))
                throw new ArgumentException("The TradeTradeBalanceMin rule expects the following objects to be passed: ITrade.");

            #endregion

            ITrade trade = Parameters[0] as ITrade;

            double tradeBalanceMin = Convert.ToDouble(RuleItem.RuleParameters[0].Value);

            if (trade.TradeBalance >= tradeBalanceMin)
                return 0;

            string errorMessage = string.Format("The Trade Balance of a Trade has to be greater than {0}", tradeBalanceMin);
            AddMessage(errorMessage, errorMessage, Messages);
            return 1;
        }
    }
    #endregion

    #region TradeTradePremiumMin
    [RuleDBTag("TradeTradePremiumMin",
    "TradeTradePremiumMin",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Trade.TradeTradePremiumMin")]
    [RuleParameterTag(new string[] { "@TradeTradePremiumMin,0.00,7" })]
    [RuleInfo]
    public class TradeTradePremiumMin : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)
            if (Parameters.Length == 0)
                throw new ArgumentException("The TradeTradePremiumMin rule expects a Domain object to be passed.");

            if (!(Parameters[0] is ITrade))
                throw new ArgumentException("The TradeTradePremiumMin rule expects the following objects to be passed: ITrade.");

            #endregion

            ITrade trade = Parameters[0] as ITrade;

            double tradePremiumMin = Convert.ToDouble(RuleItem.RuleParameters[0].Value);

            if (trade.Premium > tradePremiumMin)
                return 0;

            string errorMessage = string.Format("The Trade Premium of a Trade has to be greater than {0}",tradePremiumMin);
            AddMessage(errorMessage, errorMessage, Messages);
            return 1;
        }
    }
    #endregion
    
    #region TradeTradeDate
    [RuleDBTag("TradeTradeDate",
            "TradeTradeDate",
            "SAHL.Rules.DLL",
            "SAHL.Common.BusinessModel.Rules.Trade.TradeTradeDate")]
    [RuleInfo]
    public class TradeTradeDate : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)
            if (Parameters.Length == 0)
                throw new ArgumentException("The TradeTradeDate rule expects a Domain object to be passed.");

            if (!(Parameters[0] is ITrade))
                throw new ArgumentException("The TradeTradeDate rule expects the following objects to be passed: ITrade.");

            #endregion

            ITrade trade = Parameters[0] as ITrade;
            
                if (trade.TradeDate.Year > DateTime.Today.Year || trade.TradeDate.Month > DateTime.Today.Month)
                {
                    string errorMessage = "The Trade Date captured against a trade must not be in the future";
                    AddMessage(errorMessage, errorMessage, Messages);
                }
            
            return 0;
        }
    }
    #endregion
    
    #region TradeCAPCommencement
    [RuleDBTag("TradeCAPCommencement",
    "TradeCAPCommencement",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Trade.TradeCAPCommencement")]
    [RuleInfo]
    public class TradeCAPCommencement : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)
            if (Parameters.Length == 0)
                throw new ArgumentException("The TradeCAPCommencement rule expects a Domain object to be passed.");

            if (!(Parameters[0] is ITrade))
                throw new ArgumentException("The TradeCAPCommencement rule expects the following objects to be passed: ITrade.");

            #endregion

            ITrade trade = Parameters[0] as ITrade;
            /*
            if (trade.StartDate > DateTime.Now)  //the trade has commenced
            {
                string errorMessage = "A CAP Trade cannot be updated once the associated CAP has commenced";
                AddMessage(errorMessage, errorMessage, Messages);
            }
            */
            if (trade.StartDate == new DateTime())
                return 1;

            if (trade.StartDate < trade.ResetConfiguration.ActionDate)
            {
                string errorMessage = "The Effective Date captured against a trade cannot be set before the Reset Action Date";
                AddMessage(errorMessage, errorMessage, Messages);
                return 1;
            }

            ICalendar cal = Calendar.GetCalendarItemsByDate(trade.StartDate);
            if (cal != null)
            {
                if (cal.IsHoliday || cal.IsSaturday || cal.IsSunday)
                {
                    string errorMessage = "The Effective Date captured against a trade must be a valid business day";
                    AddMessage(errorMessage, errorMessage, Messages);
                    return 1;
                }
            }
            else
                throw new Exception("Effective Date can not be found on Calendar ");
            return 0;
        }
    }
    #endregion

    #region TradeCheckEffectiveDate
    [RuleDBTag("TradeCheckEffectiveDate",
    "TradeCAPCommencement",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Trade.TradeCheckEffectiveDate")]
    [RuleInfo]
    public class TradeCheckEffectiveDate : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)
            if (Parameters.Length == 0)
                throw new ArgumentException("The TradeCheckEffectiveDate rule expects a Domain object to be passed.");

            if (!(Parameters[0] is ITrade))
                throw new ArgumentException("The TradeCheckEffectiveDate rule expects the following objects to be passed: ITrade.");

            #endregion

            ITrade trade = Parameters[0] as ITrade;

            if (trade.StartDate == new DateTime())
                return 1;

            if (trade.StartDate < DateTime.Now)
            {
                string errorMessage = "The Effective Date captured against a trade cannot be in the past";
                AddMessage(errorMessage, errorMessage, Messages);
                return 1;
            }
            return 0;
        }
    }
    #endregion

    #region TradeDelete
    [RuleDBTag("TradeDelete",
          "TradeDelete",
            "SAHL.Rules.DLL",
          "SAHL.Common.BusinessModel.Rules.Trade.TradeDelete")]
    [RuleInfo]
    public class TradeDelete : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)
            if (Parameters.Length == 0)
                throw new ArgumentException("The TradeDelete rule expects a Domain object to be passed.");

            if (!(Parameters[0] is ITrade))
                throw new ArgumentException("The TradeDelete rule expects the following objects to be passed: ITrade.");

            #endregion

            ITrade trade = Parameters[0] as ITrade;
            if (trade.CapBalance > 0)  
            {
                string errorMessage = "A trade cannot be deleted if the CAP Balance on the trade is greater than R 0.00 ";
                AddMessage(errorMessage, errorMessage, Messages);
            }

            return 0;
        }
    }
    #endregion

    #region TradeValidateEffectiveDate
    [RuleDBTag("TradeValidateEffectiveDate",
    "TradeCAPCommencement",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Trade.TradeValidateEffectiveDate")]
    [RuleInfo]
    public class TradeValidateEffectiveDate : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)
            if (Parameters.Length == 0)
                throw new ArgumentException("The TradeValidateEffectiveDate rule expects a Domain object to be passed.");

            if (!(Parameters[0] is ITrade))
                throw new ArgumentException("The TradeValidateEffectiveDate rule expects the following objects to be passed: ITrade.");

            #endregion

            ITrade trade = Parameters[0] as ITrade;
            if (trade.StartDate  == new DateTime())
            {
                string errorMessage = "A valid Effective Date is mandatory for a Trade";
                AddMessage(errorMessage, errorMessage, Messages);
                return 1;
            }
            return 0;
        }
    }
    #endregion


    #region TradeCapBalanceValidation
    [RuleDBTag("TradeCapBalanceValidation",
    "TradeCAPCommencement",
    "SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.Trade.TradeCapBalanceValidation")]
    [RuleInfo]
    public class TradeCapBalanceValidation : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)
            if (Parameters.Length == 0)
                throw new ArgumentException("The TradeCapBalanceValidation rule expects a Domain object to be passed.");

            if (!(Parameters[0] is ITrade))
                throw new ArgumentException("The TradeCapBalanceValidation rule expects the following objects to be passed: ITrade.");

            #endregion

            ITrade trade = Parameters[0] as ITrade;
            if (trade.CapBalance != 0.00)
            {
                string errorMessage = "Can not delete Trade with Cap Balance";
                AddMessage(errorMessage, errorMessage, Messages);
                return 1;
            }
            return 0;
        }
    }
    #endregion
}
