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
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Administration.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;

namespace SAHL.Web.Views.Administration
{

	public class RuleItemBind2
	{
		public int _Key;
		public int Key { get { return _Key; } }
		public string _Name;
		public string Name { get { return _Name; } }
		public RuleItemBind2(IRuleItem ri)
		{
			_Key = ri.Key;
			_Name = ri.Key + "-" + ri.Name;
		}
	}

    public partial class WorkflowRuleSetMaint : SAHLCommonBaseView, IWorkflowRuleSetMaint
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region IWorkflowRuleSetMaint Members

        public event EventHandler OnSubmitClick;

        public event EventHandler OnRuleSetChanged;

        public void BindRuleSetList(IList<IWorkflowRuleSet> Sets)
        {
            ddlRuleSets.DataSource = Sets;
            ddlRuleSets.DataTextField = "Name";
            ddlRuleSets.DataValueField = "Key";
            ddlRuleSets.DataBind();
        }

	public void BindRuleList(IList<IRuleItem> AllRules)
	{
		List<RuleItemBind2> BindRules = new List<RuleItemBind2>();
		foreach (IRuleItem r in AllRules)
		{
			BindRules.Add(new RuleItemBind2(r));
		}

		clb.DataSource = BindRules;
		clb.DataTextField = "Name";
		clb.DataValueField = "Key";
		clb.DataBind();
	}

        public void DoSelection(IList<int> Keys)
        {
            for (int i = 0; i < clb.Items.Count; i++)
            {
                object o = clb.Items[i].Value;
                int Key = Convert.ToInt32(o);
                for(int j = 0; j < Keys.Count; j++)
                {
                    if (Keys[j] == Key)
                    {
                        clb.Items[i].Selected = true;
                        break;
                    }
                    else
                    {
                        clb.Items[i].Selected = false;
                    }
                }
            }
        }

        public ListItemCollection CheckListBoxItems
        {
            get
            {
                return clb.Items;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", Justification="New rule added to FxCop, avoiding refactor")]
        public int GetSelectedRuleSet
        {
            get
            {
                int val = -1;
                int.TryParse(ddlRuleSets.SelectedValue, out val);
                return val;
            }
        }

        #endregion

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            OnSubmitClick(null, null);
        }

        protected void ddlRuleSets_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnRuleSetChanged(sender, e);
        }
    }
}
