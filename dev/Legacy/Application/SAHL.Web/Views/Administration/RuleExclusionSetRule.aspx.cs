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
    public partial class RuleExclusionSetRule : SAHLCommonBaseView, IViewRuleExclusionSetRule
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        #region IViewRuleSetRule Members

        public bool VisibleRuleSetGrid
        {
            set
            {
                tblRuleSets.Visible = value;
            }
        }

        public bool VisibleMap
        {
            set { tblMapping.Visible = value; ; }
        }

        public bool VisibleSubmit
        {
            set
            {
                //btnSubmit.Visible = value;
                btnAdd.Visible = value;
                btnREmove.Visible = value;
                chkEnforce.Visible = value;
                chkDIsable.Visible = value;
            }
        }

        public bool Enforce { get { return chkEnforce.Checked; } }
        public bool Disable { get { return chkDIsable.Checked; } }

        public event EventHandler OnSearchClicked;

        public event EventHandler OnSubmiClick;

        public event EventHandler OnRuleSetSelected;

        public event EventHandler OnRuleAdded;

        public event EventHandler OnRuleRemoved;

        public void BindMapping(List<RuleItemBind> InRuleSet, List<RuleItemBind> NotInRuleSet)
        {
            liInRuleSet.DataSource = InRuleSet;
            liInRuleSet.DataTextField = "Name";
            liInRuleSet.DataValueField = "Key";
            liInRuleSet.DataBind();

            liNotInRuleSet.DataSource = NotInRuleSet;
            liNotInRuleSet.DataTextField = "Name";
            liNotInRuleSet.DataValueField = "Key";
            liNotInRuleSet.DataBind();
        }

        #endregion

        protected void btnSearch_OnClick(object sender, EventArgs e)
        {
            if (null != OnSearchClicked)
            {
                OnSearchClicked(null, new KeyChangedEventArgs(txtRuleName.Text));
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            // get selected item.
            if (null != liNotInRuleSet.SelectedItem)
            {
                if (null != OnRuleAdded)
                {
                    OnRuleAdded(null, new KeyChangedEventArgs(liNotInRuleSet.SelectedItem.Value));
                }
            }
        }

        protected void btnREmove_Click(object sender, EventArgs e)
        {
            // get selected item.
            if (null != liInRuleSet.SelectedItem)
            {

                if (null != OnRuleRemoved)
                {
                    OnRuleRemoved(null, new KeyChangedEventArgs(liInRuleSet.SelectedItem.Value));
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
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
    }
}
