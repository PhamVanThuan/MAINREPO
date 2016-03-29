using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using Castle.ActiveRecord;

namespace SAHL.Web.Views.Administration.Presenters
{
  public class Admin_RuleParamAdd : Admin_BaseRule<SAHL.Web.Views.Administration.Interfaces.IRuleDetails>
  {
    public Admin_RuleParamAdd(SAHL.Web.Views.Administration.Interfaces.IRuleDetails view, SAHLCommonBaseController controller)
      : base(view, controller)
    {
    }
    /// <summary>
    /// Hook the events fired by the view and call relevant methods to bind control data
    /// </summary>
    protected override void OnViewInitialised(object sender, EventArgs e)
    {
      base.OnViewInitialised(sender, e);
      
        if (!_view.ShouldRunPage) return;

      _view.OnRuleSelected += new EventHandler(_view_OnRuleSelected);
      _view.OnRuleParamSubmitClicked += new EventHandler(_view_OnRuleParamSubmitClicked);
    }

    void _view_OnRuleParamSubmitClicked(object sender, EventArgs e)
    {
      IRuleRepository repo = RepositoryFactory.GetRepository<IRuleRepository>();
      IRuleParameter prm = repo.CreateEmptyRuleParameter();
      int ruleKey = Convert.ToInt32(PrivateCacheData["RULE"]);
      IRuleItem rule = RepositoryFactory.GetRepository<IRuleRepository>().FindRuleByKey(ruleKey);
      prm.RuleItem = rule;
      prm.Name = _view.PMaintName;
      prm.Value = _view.PMaintVal;
      string val = _view.PMaintType;
      IEventList<IParameterType> types = PrivateCacheData["ParamTypes"] as IEventList<IParameterType>;
      foreach (IParameterType t in types)
      {
        if (t.CSharpDataType == val)
        {
          prm.RuleParameterType = t; break;
        }
      }
      rule.RuleParameters.Add(_view.Messages, prm);
      if (_view.Messages.Count > 0)
      {
#warning error control
      }
      using (new TransactionScope())
      {
          repo.SaveRuleItem(rule);
      }
      if (_view.Messages.Count > 0)
      {
#warning error control
      }
      else
      {
          _view.Navigator.Navigate("Admin_RuleView");
      }
    }

      protected override void _view_OnRuleSelected(object sender, EventArgs e)
      {
          GridRowSelectEventArgs args = e as GridRowSelectEventArgs;
          if (null == args) return;
          int RuleItemKey = Convert.ToInt32(args.Index);
          IRuleRepository RulesRepo = RepositoryFactory.GetRepository<IRuleRepository>();
          IRuleItem rule = RulesRepo.FindRuleByKey(RuleItemKey);
          PrivateCacheData["RULE"] = rule.Key;
          _view.ParamMaintVisible = true;
          _view.VisibleParamSubmitVisible = true;
      }

      protected override void HookRuleParamSelected()
      {
          _view.RuleMaintSubmitText = "Add";
          _view.VisibleParamSubmitVisible = true;
      }
  }
}
