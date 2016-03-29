using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Administration.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using Castle.ActiveRecord;

namespace SAHL.Web.Views.Administration.Presenters
{
    public class Admin_RuleSetEdit : Admin_RuleSetBase<IViewRuleExclusionSet>
    {
        public Admin_RuleSetEdit(IViewRuleExclusionSet View, SAHLCommonBaseController Controller)
            : base(View, Controller)
        {
        }
        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            _view.OnRuleSetSelected += new EventHandler(_view_OnRuleSetSelected);
            _view.OnSubmiClick += new EventHandler(_view_OnSubmiClick);
            BindRuleSetGrid();
            BindRuleSet();
        }

        void _view_OnSubmiClick(object sender, EventArgs e)
        {
            IRuleExclusionSet ruleSet = PrivateCacheData["RULESET"] as IRuleExclusionSet;
            IRuleRepository repo = RepositoryFactory.GetRepository<IRuleRepository>();
            ruleSet.Description = _view.RuleSetDesc;
            ruleSet.Name = _view.RuleSetName;
            using (new SessionScope())
            {
                repo.SaveRuleExclusionSet(ruleSet);
            }
            PrivateCacheData["RULESET"] = ruleSet;
            if (_view.Messages.Count > 0)
            {
#warning errors control.
            }
            _view.Navigator.Navigate("RuleSetView");
        }

        protected void _view_OnRuleSetSelected(object sender, EventArgs e)
        {
            base._view_OnRuleSetSelected(sender, e);
            BindRuleSet();
            _view.VisibleRuleMaint = true;
            _view.VisibleSubmit = true;
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
    }
}
