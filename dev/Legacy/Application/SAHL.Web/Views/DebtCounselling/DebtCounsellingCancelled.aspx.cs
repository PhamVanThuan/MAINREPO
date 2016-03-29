using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.DebtCounselling.Interfaces;

namespace SAHL.Web.Views.DebtCounselling
{
    public partial class DebtCounsellingCancelled : SAHLCommonBaseView, IDebtCounsellingCancelled
    {
        public event EventHandler OnSubmitButtonClicked
        {
            add { this.btnConfirm.Click += value; }
            remove { this.btnConfirm.Click -= value; }
        }

        public event EventHandler OnCancelButtonClicked
        {
            add { this.btnCancel.Click += value; }
            remove { this.btnCancel.Click -= value; }
        }

        private bool _cancelButtonVisible;

        public bool CancelButtonVisible
        {
            set { _cancelButtonVisible = value; }
        }

        public void BindReasons(Dictionary<int, string> dictreasons)
        {
            ddlReason.DataSource = dictreasons;
            ddlReason.DataValueField = "Key";
            ddlReason.DataTextField = "Description";
            ddlReason.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!ShouldRunPage) return;

            btnCancel.Visible = _cancelButtonVisible;
        }

        protected void gvLinkedAccounts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }

        public void BindLinkedAccountsGrid(DataTable dtLinkedAccounts)
        {
            gvLinkedAccounts.AddGridBoundColumn("AccountKey", "Account Key", Unit.Percentage(20), HorizontalAlign.Left, true);
            gvLinkedAccounts.AddGridBoundColumn("LegalEntityLegalName", "Legal Name", Unit.Percentage(40), HorizontalAlign.Left, true);
            gvLinkedAccounts.AddGridBoundColumn("IDNumber", "ID", Unit.Percentage(20), HorizontalAlign.Left, true);
            gvLinkedAccounts.AddGridBoundColumn("Role", "Role", Unit.Percentage(20), HorizontalAlign.Left, true);
            gvLinkedAccounts.DataSource = dtLinkedAccounts;
            gvLinkedAccounts.DataBind();
        }

        public int SelectedReasonDefinitionKey
        {
            get
            {
                if (ddlReason.SelectedValue == "-select-")
                    return 0;
                else
                    return Convert.ToInt32(ddlReason.SelectedValue);
            }
        }
    }
}