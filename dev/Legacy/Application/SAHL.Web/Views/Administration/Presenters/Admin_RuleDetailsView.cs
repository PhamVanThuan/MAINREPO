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
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Collections;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Administration.Presenters
{
  public class Admin_RuleDetailsView : Admin_BaseRule<SAHL.Web.Views.Administration.Interfaces.IRuleDetails>
  {
    public Admin_RuleDetailsView(SAHL.Web.Views.Administration.Interfaces.IRuleDetails view, SAHLCommonBaseController controller)
      : base(view, controller)
    {
    }

    /// <summary>
    /// Hook the events fired by the view and call relevant methods to bind control data
    /// </summary>
    protected override void OnViewInitialised(object sender, EventArgs e)
    {
        _view.OnRuleSelected += new EventHandler(_view_OnRuleSelected);
        _view.OnRuleParamSelected += new EventHandler(_view_OnRuleParamSelected);
      base.OnViewInitialised(sender, e);

      if (!_view.ShouldRunPage) return;
    }


    /// <summary>
    /// 
    /// </summary>
      protected override void OnViewLoaded(object sender, EventArgs e)
      {

          base.OnViewLoaded(sender, e);
          if (!_view.ShouldRunPage) return;
          if (PrivateCacheData.ContainsKey("PARAMS"))
          {
              BindRuleParams();
          }
      }

    protected void _view_OnRuleSelected(object sender, EventArgs e)
    {
      GridRowSelectEventArgs args = e as GridRowSelectEventArgs;
      if (null == args) return;
      int RuleItemKey = Convert.ToInt32(args.Index);
      IRuleRepository RulesRepo = RepositoryFactory.GetRepository<IRuleRepository>();
      IEventList<IRuleParameter> RuleParams = RulesRepo.FindRuleByKey(RuleItemKey).RuleParameters;
      if (_view.IsValid)
      {
          // TODO: Validation
        //#warning what todo here.
        //string WhatToDoHere = "Good Question";
      }
      PrivateCacheData["PARAMS"] = RuleParams;
      BindRuleParams();
    }

      protected void BindRuleParams()
      {
          IEventList<IRuleParameter> RuleParams = PrivateCacheData["PARAMS"] as IEventList<IRuleParameter>;
          _view.BindRuleParameters(RuleParams);
          _view.RulesParamTableVisible = true;
      }

      protected void _view_OnRuleParamSelected(object sender, EventArgs e)
      {
          GridRowSelectEventArgs args = e as GridRowSelectEventArgs;
          if (null == args) return;
          int RuleParameterKey = Convert.ToInt32(args.Index);

		  _view.PopulateParamTypeDropDown(RepositoryFactory.GetRepository<ILookupRepository>().ParameterTypes);
          IRuleParameter prm = RepositoryFactory.GetRepository<IRuleRepository>().FindParameterByKey(RuleParameterKey);
          _view.BindRuleParam(prm);
          _view.RulesParamTableVisible = true;
          _view.ParamMaintVisible = true;
      }

      protected override void HookRuleParamSelected()
      {
          
      }
  }
}
