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
using System.Collections.Generic;
using SAHL.Common.Globals;

namespace SAHL.Web.Views.Administration.Presenters
{
    public class Admin_RuleParamEdit : Admin_BaseRule<SAHL.Web.Views.Administration.Interfaces.IRuleDetails>
  {

	protected ILookupRepository lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();

    public Admin_RuleParamEdit(SAHL.Web.Views.Administration.Interfaces.IRuleDetails view, SAHLCommonBaseController controller)
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
      _view.OnRuleParamSelected += new EventHandler(_view_OnRuleParamSelected);
      _view.OnRuleParamSubmitClicked += new EventHandler(_view_OnRuleParamSubmitClicked);
	  _view.OnRuleSubmitClicked += new EventHandler(_view_OnRuleSubmitClicked);

	  _view.PopulateStatusDropDown(new List<IGeneralStatus>(lookupRepo.GeneralStatuses.Values), (int)GeneralStatuses.Inactive);
	  _view.PopulateEnforceRuleDropDown(false);
    }

    void _view_OnRuleParamSubmitClicked(object sender, EventArgs e)
    {
      int Key = Convert.ToInt32(_view.PMaintParamKey);
      IRuleParameter prm = RepositoryFactory.GetRepository<IRuleRepository>().FindParameterByKey(Key);
      prm.Name = _view.PMaintName;
      prm.Value = _view.PMaintVal;
      string prmType = _view.PMaintType;
      IEventList<IParameterType> types = PrivateCacheData["ParamTypes"] as IEventList<IParameterType>;
      foreach (IParameterType t in types)
      {
        if (t.Description == prmType)
        {
          prm.RuleParameterType = t;
        }
      }
      // save
      IRuleRepository repo = RepositoryFactory.GetRepository<IRuleRepository>();
      using (new TransactionScope())
      {
          repo.SaveRuleParameter(prm);
      }
      if (_view.Messages.Count > 0)
      {
#warning errors control
      }
      else
      {
          _view.Navigator.Navigate("Admin_RuleView");
      }
      _view.RulesParamTableVisible = false;
      _view.RulesTableVisible = false;
    }

		void _view_OnRuleSubmitClicked(object sender, EventArgs e)
		{
			int ruleKey = Convert.ToInt32(PrivateCacheData["RULE"]);
			IRuleRepository RulesRepo = RepositoryFactory.GetRepository<IRuleRepository>();
			IRuleItem rule = RulesRepo.FindRuleByKey(ruleKey);

			rule.EnforceRule = _view.Get_ddlEnforce;
			ICommonRepository commRepo = RepositoryFactory.GetRepository<ICommonRepository>();
			rule.GeneralStatus = commRepo.GetByKey<IGeneralStatus>(_view.Get_ddlStatus);
			rule.GeneralStatusReasonDescription = _view.GetSet_txtRuleStatusReason;
			// save
			IRuleRepository repo = RepositoryFactory.GetRepository<IRuleRepository>();
			using (new TransactionScope())
			{
				repo.SaveRuleItem(rule);
			}
			if (_view.Messages.Count > 0)
			{
#warning errors control
			}
			else
			{
				_view.Navigator.Navigate("Admin_RuleView");
			}
			_view.RulesParamTableVisible = false;
			_view.RulesTableVisible = false;
		}

    /// <summary>
    /// Set the relevant properties for displaying controls within the view
    /// </summary>
    protected override void OnViewPreRender(object sender, EventArgs e)
    {
        if (!_view.ShouldRunPage) return; 
        
        base.OnViewPreRender(sender, e);
    }

    /// <summary>
    /// 
    /// </summary>
    protected override void OnViewLoaded(object sender, EventArgs e)
    {
        if (!_view.ShouldRunPage) return; 
        
        base.OnViewLoaded(sender, e);
    }

        protected override void HookRuleParamSelected()
        {
            _view.RuleMaintSubmitText = "Edit";
            _view.VisibleParamSubmitVisible = true;
        }

		protected override void _view_OnRuleSelected(object sender, EventArgs e)
		{
			GridRowSelectEventArgs args = e as GridRowSelectEventArgs;
			if (null == args) return;
			int RuleItemKey = Convert.ToInt32(args.Index);
			IRuleRepository RulesRepo = RepositoryFactory.GetRepository<IRuleRepository>();
			IRuleItem rule = RulesRepo.FindRuleByKey(RuleItemKey);
			PrivateCacheData["RULE"] = rule.Key;
			IEventList<IRuleParameter> RuleParams = rule.RuleParameters;
			if (!_view.IsValid)
			{
				// TODO: Validation
				//#warning what todo here.
				//string WhatToDoHere = "Good Question";
			}
			PrivateCacheData["PARAMS"] = RuleParams;
			BindRuleParams();

			_view.RulesMaintVisible = true;
			_view.PopulateStatusDropDown(new List<IGeneralStatus>(lookupRepo.GeneralStatuses.Values), rule.GeneralStatus.Key);
			_view.PopulateEnforceRuleDropDown(rule.EnforceRule);
			_view.GetSet_txtRuleStatusReason = rule.GeneralStatusReasonDescription;
		}
  }
}
