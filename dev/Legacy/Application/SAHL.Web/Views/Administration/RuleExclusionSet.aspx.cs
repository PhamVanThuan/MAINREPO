using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Administration.Interfaces;
using SAHL.Web.Views.Administration.Presenters;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Administration
{
    public partial class RuleExclusionSet : SAHLCommonBaseView, IViewRuleExclusionSet
    {

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSearch_OnClick(object sender, EventArgs e)
        {
            if (null != OnSearchClicked)
            {
                OnSearchClicked(null, new KeyChangedEventArgs(txtRuleName.Text));
            }
        }

        protected void BtnSubmitRuleSetClick(object sender, EventArgs e)
        {
            if (null != OnSubmiClick)
            {
                OnSubmiClick(null, null);
            }
        }

        protected void RulesGrid_GridDoubleClick(object sender, SAHL.Common.Web.UI.Controls.GridSelectEventArgs e)
        {
            // get the rule and then get the rule params and bind those.
            int SelectedRowIndex = e.m_RowNum;

            int RuleSetKey = Convert.ToInt32(((GridViewRow)RuleSetGrid.Rows[e.m_RowNum]).Cells[0].Text);
            if (SelectedRowIndex != -1)
            {
                if (null != OnRuleSetSelected)
                {
                    OnRuleSetSelected(sender, new GridRowSelectEventArgs(RuleSetKey));
                }
            }
        }

        #region IViewRuleExclusionSet Members

        public bool VisibleSubmit { set { btnMaint.Visible = value; } }

        public bool VisibleSearch
        {
            set { tblSearch.Visible = value; }
        }

        public bool VisibleRuleSetGrid
        {
            set { tblRuleSets.Visible = value; }
        }

        public bool VisibleRuleMaint
        {
            set { tblMaintRuleSet.Visible = value; }
        }

        public int RuleSetKey { get { return Convert.ToInt32(txtMaintRuleSetKey.Value); } }
        public string RuleSetName { get { return txtMaintRuleSetName.Text; } }
        public string RuleSetDesc { get { return txtMaintRuleSetDesc.Text; } }


        public event EventHandler OnSearchClicked;

        public event EventHandler OnSubmiClick;

        public event EventHandler OnRuleSetSelected;

        //public void BindRuleSetList(IEventList<IRuleExclusionSet> RuleSets)
        //{
        //    List<BindRuleExclusionSet> Bind = new List<BindRuleExclusionSet>();
        //    foreach (IRuleExclusionSet r in RuleSets)
        //    {
        //        Bind.Add(new BindRuleExclusionSet(r));
        //    }
        //    RuleSetGrid.Columns.Clear();
        //    RuleSetGrid.AddGridBoundColumn("Key", "", Unit.Percentage(0), HorizontalAlign.Left, false);
        //    RuleSetGrid.AddGridBoundColumn("Name", "RuleSet Name", Unit.Percentage(20), HorizontalAlign.Left, true);
        //    RuleSetGrid.AddGridBoundColumn("Desc", "Description", Unit.Percentage(40), HorizontalAlign.Left, true);
        //    RuleSetGrid.DataSource = Bind;
        //    RuleSetGrid.DataBind();
        //}

        //public void BindRuleMaint(IRuleExclusionSet RuleSet)
        //{
        //    txtMaintRuleSetKey.Value = RuleSet.Key.ToString();
        //    txtMaintRuleSetName.Text = RuleSet.Name;
        //    txtMaintRuleSetDesc.Text = RuleSet.Description;
        //}

        #endregion
    }
}
