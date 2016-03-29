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
using System;
using System.Collections.Generic;

namespace SAHL.Web.Views.Administration.Presenters
{
    public class Admin_RuleSetRuleBase<T> : SAHLCommonBasePresenter<IViewRuleExclusionSetRule>
    {
        public Admin_RuleSetRuleBase(IViewRuleExclusionSetRule View, SAHLCommonBaseController Controller)
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
            _view.OnRuleSetSelected += new EventHandler(_view_OnRuleSetSelected);
            BindRuleSetGrid();
            BindInOut();
        }

        protected void _view_OnSearchClicked(object sender, EventArgs e)
        {
//            string RuleSetName = ((KeyChangedEventArgs)e).Key.ToString();
//            IRuleRepository repo = RepositoryFactory.GetRepository<IRuleRepository>();
//            IEventList<IRuleExclusionSet> RuleSets = repo.FindRuleExclusionSetsByPartialName(RuleSetName);
//            if (_view.Messages.Count > 0)
//            {
//#warning Messages Control here.
//            }
//            PrivateCacheData.Add("RULESETS", RuleSets);
//            BindRuleSetGrid();
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
            //int Key = Convert.ToInt32(((GridRowSelectEventArgs)e).Index);
            //IRuleRepository repo = RepositoryFactory.GetRepository<IRuleRepository>();
            //IRuleExclusionSet ruleSet = repo.FindRuleExclusionSetByKey(Key);
            //PrivateCacheData.Add("RULESET", ruleSet);
            //// get a list of all the rules and then build a subset of ones that are not contained in this
            //// ruleset
            //List<RuleItemBind> InRules = new List<RuleItemBind>();
            //List<RuleItemBind> OutRules = new List<RuleItemBind>();
            //CalculateInOutRules(ref InRules, ref OutRules, ruleSet, repo);

            //BindInOut();
        }

        protected void BindInOut()
        {
            if (PrivateCacheData.ContainsKey("IN") && PrivateCacheData.ContainsKey("OUT"))
            {
                List<RuleItemBind> InRules = PrivateCacheData["IN"] as List<RuleItemBind>;
                List<RuleItemBind> OutRules = PrivateCacheData["OUT"] as List<RuleItemBind>;
                _view.BindMapping(InRules, OutRules);
                _view.VisibleMap = true;
            }
        }

        //protected void CalculateInOutRules(ref List<RuleItemBind> InRules, ref List<RuleItemBind> OutRules, IRuleExclusionSet ruleSet, IRuleRepository repo)
        //{
        //    List<int> Keys = new List<int>();
        //    IEventList<IRuleItem> AllRules = null;
        //    foreach (IRuleExclusionSetRule rsr in ruleSet.RuleExclusionSetRules)
        //    {
        //        InRules.Add(new RuleItemBind(rsr.RuleItem));
        //        Keys.Add(rsr.RuleItem.Key);
        //    }
        //    if (!PrivateCacheData.ContainsKey("ALLRULES"))
        //    {
        //        AllRules = repo.GetAllRules();
        //        PrivateCacheData.Add("ALLRULES", AllRules);
        //    }
        //    else
        //    {
        //        AllRules = PrivateCacheData["ALLRULES"] as IEventList<IRuleItem>;
        //    }

        //    foreach (IRuleItem ri in AllRules)
        //    {
        //        RuleItemBind rib = new RuleItemBind(ri);
        //        if (!Keys.Contains(rib.Key))
        //            OutRules.Add(rib);
        //    }
        //    PrivateCacheData.Remove("IN");
        //    PrivateCacheData.Add("IN", InRules);
        //    PrivateCacheData.Remove("OUT");
        //    PrivateCacheData.Add("OUT", OutRules);
        //}

        protected void BindRuleSetGrid()
        {
            if (PrivateCacheData.ContainsKey("RULESETS"))
            {
                //IEventList<IRuleExclusionSet> RuleSets = PrivateCacheData["RULESETS"] as IEventList<IRuleExclusionSet>;
                //_view.BindRuleSets(RuleSets);
                //_view.VisibleRuleSetGrid = true;
            }
        }
    }
}

