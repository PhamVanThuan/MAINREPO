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
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;

namespace SAHL.Web.Views.Administration.Presenters
{
    public class WorkflowRuleSetMaint : SAHLCommonBasePresenter<SAHL.Web.Views.Administration.Interfaces.IWorkflowRuleSetMaint>
    {
        private ILookupRepository _lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();
        IRuleRepository rRepo = RepositoryFactory.GetRepository<IRuleRepository>();
        private IWorkflowRuleSet WFRuleSet;

        public WorkflowRuleSetMaint(SAHL.Web.Views.Administration.Interfaces.IWorkflowRuleSetMaint view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            View.OnRuleSetChanged += new EventHandler(View_OnRuleSetChanged);
            View.OnSubmitClick += new EventHandler(View_OnSubmitClick);
            View.BindRuleSetList(_lookupRepo.WorkflowRuleSetSorted);
            View.BindRuleList(_lookupRepo.RuleItemList);
        }

        protected override void OnViewLoaded(object sender, EventArgs e)
        {
            base.OnViewLoaded(sender, e);
            LoadAndPopulate();
        }

        void LoadAndPopulate()
        {
            int SetKey = View.GetSelectedRuleSet;
            if (SetKey > 0)
            {
                // go load the WorkflowRuleSet
                WFRuleSet = rRepo.GetRuleSetForKey(SetKey);
            }
        }



        void View_OnSubmitClick(object sender, EventArgs e)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    // Go get list of selected Keys
                    ListItemCollection Items = View.CheckListBoxItems;
                    List<int> Keys = new List<int>();
                    for (int i = 0; i < Items.Count; i++)
                    {
                        if (Items[i].Selected)
                        {
                            Keys.Add(Convert.ToInt32(Items[i].Value));
                        }
                    }

                    // Get a list of rules that are no longer selected.
                    List<IRuleItem> RulesToRemove = new List<IRuleItem>();
                    foreach (IRuleItem ri in WFRuleSet.Rules)
                    {
                        bool found = false;
                        for (int i = 0; i < Keys.Count; i++)
                        {
                            if (Keys[i] == ri.Key)
                            {
                                found = true;
                                break;
                            }
                        }
                        if (!found)
                            RulesToRemove.Add(ri);
                    }

                    // get a list of rules that have been added to this ruleset
                    List<IRuleItem> RulesToAdd = new List<IRuleItem>();
                    for (int i = 0; i < Keys.Count; i++)
                    {
                        // look in the list of exsting rules for the currently selected
                        bool found = false;
                        for (int j = 0; j < WFRuleSet.Rules.Count; j++)
                        {
                            if (WFRuleSet.Rules[j].Key == Keys[i])
                            {
                                found = true;
                                break;
                            }
                        }
                        if (!found)
                        {
                            // We havnt found this in the list of existing rules so add it
                            RulesToAdd.Add(rRepo.FindRuleByKey(Keys[i]));
                        }
                    }
                    IDomainMessageCollection dmc = new DomainMessageCollection();
                    // remove any rules
                    foreach (IRuleItem ri in RulesToRemove)
                    {
                        WFRuleSet.Rules.Remove(dmc, ri);
                    }

                    // add in new rules
                    foreach (IRuleItem ri in RulesToAdd)
                    {
                        WFRuleSet.Rules.Add(dmc, ri);
                    }

                    // save
                    rRepo.SaveRuleSet(WFRuleSet);
                    ts.VoteCommit();
                }
                catch (Exception)
                {
                    ts.VoteRollBack();
                    if (_view.IsValid)
                        throw;
                }
            }
        }

        void View_OnRuleSetChanged(object sender, EventArgs e)
        {
            LoadAndPopulate();
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (null != WFRuleSet)
            {
                View.DoSelection(WFRuleSet.RuleKeys);
            }
        }
    }
}
