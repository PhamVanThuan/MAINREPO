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
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using System.Collections.Generic;
using SAHL.Common.Collections.Interfaces;
using Castle.ActiveRecord;

namespace SAHL.Web.Views.Administration.Presenters
{
    public class Admin_RuleSetRuleMap: Admin_RuleSetRuleBase<IViewRuleExclusionSetRule>
    {
        public Admin_RuleSetRuleMap(IViewRuleExclusionSetRule View, SAHLCommonBaseController Controller)
            : base(View, Controller)
        {
        }
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            _view.OnRuleSetSelected += new EventHandler(_view_OnRuleSetSelected);
            _view.OnRuleAdded += new EventHandler(_view_OnRuleAdded);
            _view.OnRuleRemoved += new EventHandler(_view_OnRuleRemoved);
            _view.OnSubmiClick += new EventHandler(_view_OnSubmiClick);
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

        protected void _view_OnRuleSetSelected(object sender, EventArgs e)
        {
            base._view_OnRuleSetSelected(sender, e);
            _view.VisibleSubmit = true;
        }

        void _view_OnSubmiClick(object sender, EventArgs e)
        {
            IRuleRepository repo = RepositoryFactory.GetRepository<IRuleRepository>();
            IRuleExclusionSet rs = PrivateCacheData["RULESET"] as IRuleExclusionSet;
            // rebind with new lists
            using (new TransactionScope())
            {
                repo.SaveRuleExclusionSet(rs);
            }
            if (_view.Messages.Count > 0)
            {
#warning messages control here
            }
            List<RuleItemBind> InRules = new List<RuleItemBind>();
            List<RuleItemBind> OutRules = new List<RuleItemBind>();
            CalculateInOutRules(ref InRules, ref OutRules, rs, repo);
            BindInOut();
        }

        void _view_OnRuleRemoved(object sender, EventArgs e)
        {
            IRuleRepository repo = RepositoryFactory.GetRepository<IRuleRepository>();
            int Key = Convert.ToInt32(((KeyChangedEventArgs)e).Key);
            // find this one in the cache
            IRuleExclusionSet rs = PrivateCacheData["RULESET"] as IRuleExclusionSet;

            // TODO: Uncomment rsr - removed for FxCop
            IRuleExclusionSetRule rsr = null;
            foreach (IRuleExclusionSetRule tmpri in rs.RuleExclusionSetRules)
            {
                if (tmpri.RuleItem.Key == Key)
                {
                     rsr = tmpri;
                    break;
                }
            }

            // remove from this ruleset
            rs.RuleExclusionSetRules.Remove(_view.Messages, rsr);

            // refresh to the cache
            PrivateCacheData.Remove("RULESET");
            PrivateCacheData.Add("RULESET", rs);

            // rebind with new lists
            List<RuleItemBind> InRules = new List<RuleItemBind>();
            List<RuleItemBind> OutRules = new List<RuleItemBind>();
            CalculateInOutRules(ref InRules, ref OutRules, rs, repo);
            BindInOut();
            using (new TransactionScope())
            {
                repo.SaveRuleExclusionSet(rs);
            }
            //repo.RemoveRuleSetRuleFromRuleSet(_rsr);
            _view.VisibleSubmit = true;
        }

        void _view_OnRuleAdded(object sender, EventArgs e)
        {
            int Key = Convert.ToInt32(((KeyChangedEventArgs)e).Key);
            // find this one in the cache
            IEventList<IRuleItem> AllRules = PrivateCacheData["ALLRULES"] as IEventList<IRuleItem>;
            IRuleItem ri = null;
            foreach (IRuleItem tmpri in AllRules)
            {
                if (tmpri.Key == Key)
                {
                    ri = tmpri;
                    break;
                }
            }

            // create an empty rulesetrule to add to the contained rules
            IRuleRepository repo = RepositoryFactory.GetRepository<IRuleRepository>();
            IRuleExclusionSetRule rsr = repo.CreateEmptyRuleExclusionSetRule();
            // get the ruleset we are working with from the cache
            IRuleExclusionSet rs = PrivateCacheData["RULESET"] as IRuleExclusionSet;
            rsr.RuleExclusionSet = rs;
            rsr.RuleItem = ri;
            rsr.EnForceRule = _view.Enforce;
            rsr.DisableRule = _view.Disable;
            
            // add to this ruleset
            rs.RuleExclusionSetRules.Add(_view.Messages, rsr);

            // refresh to the cache
            PrivateCacheData.Remove("RULESET");
            PrivateCacheData.Add("RULESET", rs);

            // rebind with new lists
            List<RuleItemBind> InRules = new List<RuleItemBind>();
            List<RuleItemBind> OutRules = new List<RuleItemBind>();
            CalculateInOutRules(ref InRules, ref OutRules, rs, repo);
            BindInOut();
            using (new TransactionScope())
            {
                repo.SaveRuleExclusionSetRule(rsr);
            }
            _view.VisibleSubmit = true;
        }
    }
}
