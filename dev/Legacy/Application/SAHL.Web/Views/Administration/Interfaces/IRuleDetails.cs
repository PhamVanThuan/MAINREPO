using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI;
using System.Collections.Generic;

namespace SAHL.Web.Views.Administration.Interfaces
{
  public interface IRuleDetails : IViewBase
  {
    /// <summary>
    /// Binds a list of rules to a grid.
    /// </summary>
    /// <param name="Rules">Collection of IRuleItems.</param>
    void BindRulesGrid(IEventList<IRuleItem> Rules);

    /// <summary>
    /// Binds a single rule
    /// </summary>
    /// <param name="Rule"></param>
    void BindRule(IRuleItem Rule);
    /// <summary>
    /// Binds a list of rule parametes for a selected rule.
    /// </summary>
    /// <param name="RuleParams"></param>
    void BindRuleParameters(IEventList<IRuleParameter> RuleParams);
    /// <summary>
    /// Binds a single rule parameter item to be updated.
    /// </summary>
    /// <param name="Param"></param>
    void BindRuleParam(IRuleParameter Param);
	/// <summary>
	/// 
	/// </summary>
	/// <param name="ParamTypes"></param>
	void PopulateParamTypeDropDown(IEventList<IParameterType> ParamTypes);
	/// <summary>
	/// 
	/// </summary>
	/// <param name="GeneralStatus"></param>
	/// <param name="GeneralStatusKey"></param>
	void PopulateStatusDropDown(IList<IGeneralStatus> GeneralStatus, int GeneralStatusKey);
	/// <summary>
	/// 
	/// </summary>
	void PopulateEnforceRuleDropDown(bool EnforceRule);
	/// <summary>
	/// 
	/// </summary>
	int Get_ddlStatus{ get; }
	/// <summary>
	/// 
	/// </summary>
	bool Get_ddlEnforce{ get; }
	/// <summary>
	/// 
	/// </summary>
	string GetSet_txtRuleStatusReason { get; set;}
    /// <summary>
    /// Fires when the search button is clicked
    /// </summary>
    event EventHandler OnSearchButtonClicked;
    /// <summary>
    /// Fires when a rule is selected in the rules grid.
    /// </summary>
    event EventHandler OnRuleSelected;
    /// <summary>
    /// Fires when a ruleparameter is selected in the ruleparameter grid.
    /// </summary>
    event EventHandler OnRuleParamSelected;
    /// <summary>
    /// Fires when the add rule button is clicked.
    /// </summary>
    event EventHandler OnNewRuleClick;
    /// <summary>
    /// Fires when the update / add RuleItem button is clicked.
    /// </summary>
    event EventHandler OnRuleSubmitButtonClicked;
    /// <summary>
    /// Fires when the submit (Add / update) button for parameters is clicked.
    /// </summary>
    event EventHandler OnRuleParamSubmitClicked;

	event EventHandler OnRuleSubmitClicked;

    string RuleMaintSubmitText { set; }
    bool RulesTableVisible { set; }
    bool RulesParamTableVisible { set; }
    bool RulesMaintVisible { set; }
    bool RulesSearchVisible { set; }
    bool ParamMaintVisible { set; }
    string RuleName { get; }

    int MaintRuleKey { get; }
	//string MaintRuleName { get;}
	//string MaintRuleDesc { get;}
	//string MaintAsmName { get;}
	//string MaintTypeName { get;}
    string SearchValue { get; }

    int PMaintParamKey { get; }
    string PMaintName { get; }
    string PMaintVal { get; }
    string PMaintType { get; }
      bool VisibleParamSubmitVisible { set; }
  }
}
