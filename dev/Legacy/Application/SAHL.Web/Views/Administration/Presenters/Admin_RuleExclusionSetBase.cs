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
using SAHL.Web.Views.Administration.Interfaces;
using SAHL.Common.Web.UI.Events;
using SAHL.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Web.Views.Administration.Presenters
{
    public class Admin_RuleSetBase<T> : SAHLCommonBasePresenter<IViewRuleExclusionSet>
    {
        public Admin_RuleSetBase(IViewRuleExclusionSet View, SAHLCommonBaseController Controller)
            : base(View, Controller)
        {
        }
        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            _view.OnSearchClicked += new EventHandler(_view_OnSearchClicked);
        }


        /// <summary>
        /// Set the relevant properties for displaying controls within the view
        /// </summary>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            _view.VisibleSearch = true;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnViewLoaded(object sender, EventArgs e)
        {
            base.OnViewLoaded(sender, e);
        }

        protected void _view_OnRuleSetSelected(object sender, EventArgs e)
        {
            int Key = Convert.ToInt32(((GridRowSelectEventArgs)e).Index);
            IRuleRepository repo = RepositoryFactory.GetRepository<IRuleRepository>();
            IRuleExclusionSet ruleSet = repo.FindRuleExclusionSetByKey(Key);
            PrivateCacheData.Add("RULESET", ruleSet);
            BindRuleSet();
        }

        protected void BindRuleSet()
        {
            if (PrivateCacheData.ContainsKey("RULESET"))
            {
                IRuleExclusionSet ruleSet = PrivateCacheData["RULESET"] as IRuleExclusionSet;
                _view.BindRuleMaint(ruleSet);
                _view.VisibleRuleMaint = true;
            }
        }

        protected void _view_OnSearchClicked(object sender, EventArgs e)
        {
            string RuleSetName = ((KeyChangedEventArgs)e).Key.ToString();
            IRuleRepository repo = RepositoryFactory.GetRepository<IRuleRepository>();
            IEventList<IRuleExclusionSet> RuleSets = repo.FindRuleExclusionSetsByPartialName(RuleSetName);
            if (_view.Messages.Count > 0)
            {
#warning Messages Control here.
            }
            PrivateCacheData.Add("RULESETS", RuleSets);
            BindRuleSetGrid();
        }

        protected void BindRuleSetGrid()
        {
            if (PrivateCacheData.ContainsKey("RULESETS"))
            {
                IEventList<IRuleExclusionSet> RuleSets = PrivateCacheData["RULESETS"] as IEventList<IRuleExclusionSet>;
                _view.BindRuleSetList(RuleSets);
                _view.VisibleRuleSetGrid = true;
            }
        }

    }
}
