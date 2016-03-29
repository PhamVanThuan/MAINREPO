using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Web.UI;
using SAHL.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using Castle.ActiveRecord;

namespace SAHL.Web.Views.Administration.Presenters
{
  public class Admin_RuleItemEdit: Admin_BaseRule<SAHL.Web.Views.Administration.Interfaces.IRuleDetails>
  {
    public Admin_RuleItemEdit(SAHL.Web.Views.Administration.Interfaces.IRuleDetails View, SAHLCommonBaseController Controller)
      : base(View, Controller)
    {
    }
    /// <summary>
    /// Hook the events fired by the view and call relevant methods to bind control data
    /// </summary>
    protected override void OnViewInitialised(object sender, EventArgs e)
    {
      base.OnViewInitialised(sender, e);
      _view.OnSearchButtonClicked += new EventHandler(_view_OnSearchButtonClicked);
      _view.OnRuleSelected += new EventHandler(_view_OnRuleSelected);
      _view.OnRuleSubmitButtonClicked += new EventHandler(_view_OnRuleSubmitButtonClicked);
    }

    /// <summary>
    /// 
    /// </summary>
    protected override void OnViewLoaded(object sender, EventArgs e)
    {
      base.OnViewLoaded(sender, e);
    }
    /// <summary>
    /// Set the relevant properties for displaying controls within the view
    /// </summary>
    protected override void OnViewPreRender(object sender, EventArgs e)
    {
      base.OnViewPreRender(sender, e);
      _view.RulesSearchVisible = true;
      _view.RuleMaintSubmitText = "Edit Rule";
    }

    void _view_OnRuleSubmitButtonClicked(object sender, EventArgs e)
    {
        /*
      using (new TransactionScope())
      {
        int RuleKey = Convert.ToInt32(((KeyChangedEventArgs)e).Key);
        IRuleRepository repo = RepositoryFactory.GetRepository<IRuleRepository>();
        IRuleItem rule = repo.FindRuleByKey(_view.Messages, RuleKey);
        if (_view.Messages.Count > 0)
        {
          // string s = "wtf";
          return;
        }
        rule.AssemblyName = _view.MaintAsmName;
        rule.Description = _view.MaintRuleDesc;
        rule.Name = _view.MaintRuleName;
        rule.TypeName = _view.MaintTypeName;
        using (new TransactionScope())
        {
            repo.SaveRuleItem(_view.Messages, rule);
        }
        if (_view.Messages.Count > 0)
        {
          // string s = "wtf";
        }
      }
      // rebind the grid
      string SearchValue = _view.SearchValue;
      base._view_OnSearchButtonClicked(sender, new KeyChangedEventArgs(SearchValue));
      _view.RulesTableVisible = true;
        */
    }

    protected void _view_OnSearchButtonClicked(object sender, EventArgs e)
    {
      base._view_OnSearchButtonClicked(sender, e);
      _view.RulesTableVisible = true;
      //string PartialRuleName = ((KeyChangedEventArgs)e).Key.ToString();
      //BindRulesGrid(PartialRuleName);
    }

    void _view_OnRuleSelected(object sender, EventArgs e)
    {
        /*
      GridRowSelectEventArgs args = e as GridRowSelectEventArgs;
      if (null == args) return;
      int RuleItemKey = Convert.ToInt32(args.Index);
      IRuleRepository RulesRepo = RepositoryFactory.GetRepository<IRuleRepository>();
      IRuleItem rule = RulesRepo.FindRuleByKey(_view.Messages, RuleItemKey);
      if (_view.Messages.HasErrorMessages || _view.Messages.HasWarningMessages || _view.Messages.HasInfoMessages)
      {
        // TODO: What to do here!
        // string WhatToDoHere = "Good Question";
      }
      _view.BindRule(rule);
      _view.RulesMaintVisible = true;
        */
    }
  }
}
