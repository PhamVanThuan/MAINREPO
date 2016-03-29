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
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;

namespace SAHL.Web.Views.Administration.Presenters
{
    public abstract class Admin_BaseRule<T> : SAHLCommonBasePresenter<SAHL.Web.Views.Administration.Interfaces.IRuleDetails>
    {
        protected IEventList<IRuleParameter> RuleParameters = null;
        public Admin_BaseRule(SAHL.Web.Views.Administration.Interfaces.IRuleDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            
            if (!_view.ShouldRunPage) return;

            _view.OnSearchButtonClicked += new EventHandler(_view_OnSearchButtonClicked);
            BindLookups();
        }

        protected override void OnViewLoaded(object sender, EventArgs e)
        {
            base.OnViewLoaded(sender, e);
            if (!_view.ShouldRunPage) return;
            if (!String.IsNullOrEmpty(_view.RuleName))
                BindRulesGrid(_view.RuleName);
            BindRuleParams();
        }

        //protected override void OnViewPreRender(object sender, EventArgs e)
        //{
        //    base.OnViewPreRender(sender, e);
        //    if (!_view.ShouldRunPage) return;
        //}

        protected void _view_OnSearchButtonClicked(object sender, EventArgs e)
        {

            string PartialRuleName = ((KeyChangedEventArgs)e).Key.ToString();
            BindRulesGrid(PartialRuleName);
        }

        protected void BindRulesGrid(string RuleName)
        {
            IRuleRepository RulesRepo = RepositoryFactory.GetRepository<IRuleRepository>();
            //IDomainMessageCollection Messages = new DomainMessageCollection();
            IEventList<IRuleItem> Rules = RulesRepo.FindRulesByPartialName(RuleName);
            if (!View.IsValid)
            {
                // TODO: Validation
                //#warning what todo here.
                //string WhatToDoHere = "Good Question";
            }
            _view.BindRulesGrid(Rules);
            _view.RulesTableVisible = true;
        }

        protected virtual void _view_OnRuleSelected(object sender, EventArgs e)
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
        }

        protected void BindRuleParams()
        {
            if (PrivateCacheData.ContainsKey("PARAMS"))
            {
                IEventList<IRuleParameter> RuleParams = PrivateCacheData["PARAMS"] as IEventList<IRuleParameter>;
                if (null != RuleParams)
                {
                    _view.BindRuleParameters(RuleParams);
                    _view.RulesParamTableVisible = true;
                }
            }
        }

        protected void _view_OnRuleParamSelected(object sender, EventArgs e)
        {
            GridRowSelectEventArgs args = e as GridRowSelectEventArgs;
            if (null == args) return;
            int RuleParameterKey = Convert.ToInt32(args.Index);
            BindLookups();
            IRuleParameter prm = RepositoryFactory.GetRepository<IRuleRepository>().FindParameterByKey(RuleParameterKey);
            _view.BindRuleParam(prm);
            _view.RulesParamTableVisible = true;
            _view.ParamMaintVisible = true;
            HookRuleParamSelected();
        }

        protected void BindLookups()
        {
            IEventList<IParameterType> paramTypes = null;
            if (!PrivateCacheData.ContainsKey("ParamTypes"))
            {
                paramTypes = RepositoryFactory.GetRepository<ILookupRepository>().ParameterTypes;
                PrivateCacheData["ParamTypes"] = paramTypes;
            }
            paramTypes = PrivateCacheData["ParamTypes"] as IEventList<IParameterType>;
			_view.PopulateParamTypeDropDown(paramTypes);
        }
        protected abstract void HookRuleParamSelected();
    }
}
