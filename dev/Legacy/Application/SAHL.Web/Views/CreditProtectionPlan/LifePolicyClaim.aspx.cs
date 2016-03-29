using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.CreditProtectionPlan
{
    public partial class LifePolicyClaim : SAHLCommonBaseView, SAHL.Web.Views.CreditProtectionPlan.Interfaces.ILifePolicyClaim
    {
        public event KeyChangedEventHandler OnLifePolicyClaimGridSelectedIndexChanged;

        public event EventHandler OnCancelButtonClicked;

        public event EventHandler OnSubmitButtonClicked;


        protected void LifePolicyClaimGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (OnLifePolicyClaimGridSelectedIndexChanged != null)
                if (LifePolicyClaimGrid.SelectedIndex >= 0)
                    OnLifePolicyClaimGridSelectedIndexChanged(sender, new KeyChangedEventArgs(LifePolicyClaimGrid.SelectedIndex));
        }

        protected void LifePolicyClaimGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            TableCellCollection cells = e.Row.Cells;

            ILifePolicyClaim lifePolicyClaim = e.Row.DataItem as ILifePolicyClaim;

            if (e.Row.DataItem != null)
            {
                cells[0].Text = lifePolicyClaim.ClaimType == null ? " " : lifePolicyClaim.ClaimType.Description;
                cells[1].Text = lifePolicyClaim.ClaimStatus == null ? " " : lifePolicyClaim.ClaimStatus.Description;
                cells[2].Text = lifePolicyClaim.ClaimDate == null ? " " : lifePolicyClaim.ClaimDate.ToShortDateString();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (OnCancelButtonClicked != null)
                OnCancelButtonClicked(sender, e);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (OnSubmitButtonClicked != null)
                OnSubmitButtonClicked(sender, e);
        }

        public void BindLifePolicyClaimGrid(IList<ILifePolicyClaim> lifePolicyClaims)
        {
            LifePolicyClaimGrid.PostBackType = _lifePolicyClaimGrid_PostBackType;

            LifePolicyClaimGrid.AddGridBoundColumn("", "Claim Type", Unit.Percentage(40), HorizontalAlign.Left, true);
            LifePolicyClaimGrid.AddGridBoundColumn("", "Claim Status", Unit.Percentage(40), HorizontalAlign.Left, true);
            LifePolicyClaimGrid.AddGridBoundColumn("", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Claim Date", false, Unit.Percentage(20), HorizontalAlign.Left, true);

            LifePolicyClaimGrid.DataSource = lifePolicyClaims;
            LifePolicyClaimGrid.DataBind();
        }

        public void BindLifePolicyClaimFields(ILifePolicyClaim lifePolicyClaim, bool display, bool adding)
        {
            if (display)
            {
                hiLifePolicyClaimKey.Value = "0";
                lblClaimStatus.Text = lifePolicyClaim.ClaimStatus.Description;
                lblClaimType.Text = lifePolicyClaim.ClaimType.Description;
                lblClaimStatusDate.Text = lifePolicyClaim.ClaimDate.ToShortDateString();
            }
            else
            {
                if (adding)
                {
                    hiLifePolicyClaimKey.Value = "0";
                    ddlClaimStatus.SelectedIndex = 0;
                    ddlClaimType.SelectedIndex = 0;
                    dtClaimStatusDate.Date = DateTime.Now;
                }
                else
                {
                    hiLifePolicyClaimKey.Value = lifePolicyClaim.Key.ToString();
                    ddlClaimStatus.SelectedValue = lifePolicyClaim.ClaimStatus.Key.ToString();
                    lblClaimType.Text = lifePolicyClaim.ClaimType.Description;
                    dtClaimStatusDate.Date = lifePolicyClaim.ClaimDate;
                }
            }
        }

        public void SetupControls(bool display, bool adding = false)
        {
            lblClaimType.Visible = display || !adding;
            ddlClaimType.Visible = !display && adding;

            lblClaimStatus.Visible = display;
            ddlClaimStatus.Visible = !display;

            lblClaimStatusDate.Visible = display;
            dtClaimStatusDate.Visible = !display;

            if (!display)
                SubmitButton.Text = adding ? "Add" : "Update";
        }

        public void BindClaimTypes(IDictionary<int, string> claimTypes)
        {
            ddlClaimType.DataSource = claimTypes;
            ddlClaimType.DataBind();
        }

        public void BindClaimStatuses(IDictionary<int, string> claimStatuses)
        {
            ddlClaimStatus.DataSource = claimStatuses;
            ddlClaimStatus.DataBind();
        }

        private GridPostBackType _lifePolicyClaimGrid_PostBackType;
        public GridPostBackType LifePolicyClaimGrid_PostBackType
        {
            set 
            {
                _lifePolicyClaimGrid_PostBackType = value;
            }
        }

        public bool ButtonRow_visability
        {
            set 
            {
                ButtonRow.Visible = value; 
            }
        }

        public int LifePolicyClaimKey
        {
            get
            {
                string key = hiLifePolicyClaimKey.Value;
                int result = -1;
                if (Int32.TryParse(key, out result))
                    return result;

                return result;
            }
        }

        public int ClaimTypeKey
        {
            get
            {
                string key = ddlClaimType.SelectedValue;
                int result = -1;
                if (Int32.TryParse(key, out result))
                    return result;

                return result;
            }
        }

        public int ClaimStatusKey
        {
            get
            {
                string key = ddlClaimStatus.SelectedValue;
                int result = -1;
                if (Int32.TryParse(key, out result) == false)
                    return result;

                return result;
            }
        }

        public DateTime? ClaimDate
        {
            get
            {
                if (dtClaimStatusDate.DateIsValid)
                    return dtClaimStatusDate.Date.Value;

                return null;
            }
        }
    }
}