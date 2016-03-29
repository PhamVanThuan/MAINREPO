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
using SAHL.Common.Web.UI.Events;
using SAHL.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Collections;
using Castle.ActiveRecord;

namespace SAHL.Web.Views.Administration.Presenters
{
  public class Admin_RuleView : Admin_BaseRule<SAHL.Web.Views.Administration.Interfaces.IRuleDetails>
  {
      public Admin_RuleView(SAHL.Web.Views.Administration.Interfaces.IRuleDetails View, SAHLCommonBaseController Controller)
      : base(View, Controller)
    {
    }
    /// <summary>
    /// Hook the events fired by the view and call relevant methods to bind control data
    /// </summary>
    protected override void OnViewInitialised(object sender, EventArgs e)
    {
      base.OnViewInitialised(sender, e);
    }

    /// <summary>
    /// Set the relevant properties for displaying controls within the view
    /// </summary>
    protected override void OnViewPreRender(object sender, EventArgs e)
    {
      base.OnViewPreRender(sender, e);
    }

    /// <summary>
    /// 
    /// </summary>
    protected override void OnViewLoaded(object sender, EventArgs e)
    {
      base.OnViewLoaded(sender, e);
    }

      protected override void HookRuleParamSelected()
      {
          
      }
  }
}
