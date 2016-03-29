using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.Views.PersonalLoan.Interfaces;
using System;

namespace SAHL.Web.Views.PersonalLoan
{
    public partial class PersonalLoanMaintainExternalLifePolicy : SAHLCommonBaseView, IPersonalLoanMaintainLifePolicy
    {
        public event EventHandler OnCancelButtonClicked
        {
            add { this.btnCancel.Click += value; }
            remove { this.btnCancel.Click -= value; }
        }

        public event EventHandler OnSubmitButtonClicked
        {
            add { this.btnSave.Click += value; }
            remove { this.btnSave.Click -= value; }
        }

        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (OnPolicyStatusSelectedIndexChanged != null)
            {
                OnPolicyStatusSelectedIndexChanged(sender, new KeyChangedEventArgs(ddlStatus.SelectedValue));
            }
        }


        public event KeyChangedEventHandler OnPolicyStatusSelectedIndexChanged;

        public void SetupSummaryView()
        {
            lblClosedate.Visible = true;
            lblCommencementDate.Visible = true;
            lblInsurer.Visible = true;
            lblPolicyNumber.Visible = true;
            lblStatus.Visible = true;
            lblSumInsured.Visible = true;

            ddlInsurer.Visible = false;
            txtPolicyNumber.Visible = false;
            txtSumInsured.Visible = false;
            dtClosedate.Visible = false;
            dtCommencementDate.Visible = false;
            ddlStatus.Visible = false;
            chkPolicyCeded.Enabled = false;
            btnCancel.Visible = false;
            btnSave.Visible = false;
        }

        public void ResetCloseDate()
        {
            this.dtClosedate.Date = null;
        }

        public void ResetCeded()
        {
            this.chkPolicyCeded.Checked = false;
        }

        public void BindStatus(System.Collections.Generic.IDictionary<string, string> lifeStatuses)
        {
            ddlStatus.DataSource = lifeStatuses;
            ddlStatus.DataValueField = "Key";
            ddlStatus.DataTextField = "Description";
            ddlStatus.DataBind();
        }

        public void BindInsurers(System.Collections.Generic.IDictionary<string, string> insurers)
        {
            ddlInsurer.DataSource = insurers;
            ddlInsurer.DataValueField = "Key";
            ddlInsurer.DataTextField = "Description";
            ddlInsurer.DataBind();
        }

        public void BindMaintainLifePolicyForReadOnly(IExternalLifePolicy externalLifePolicy)
        {
            lblClosedate.Text = externalLifePolicy.CloseDate.HasValue ? externalLifePolicy.CloseDate.Value.ToShortDateString() : null;
            lblCommencementDate.Text = externalLifePolicy.CommencementDate.ToShortDateString();
            lblInsurer.Text = externalLifePolicy.Insurer.Description;
            lblPolicyNumber.Text = externalLifePolicy.PolicyNumber;
            lblStatus.Text = externalLifePolicy.LifePolicyStatus.Description;
            lblSumInsured.Text = externalLifePolicy.SumInsured.ToString(SAHL.Common.Constants.CurrencyFormat);
            chkPolicyCeded.Checked = externalLifePolicy.PolicyCeded;
        }

        public void BindMaintainLifePolicyForReadWrite(IExternalLifePolicy externalLifePolicy)
        {
            dtClosedate.Date = externalLifePolicy.CloseDate;
            dtCommencementDate.Date = externalLifePolicy.CommencementDate;
            ddlInsurer.SelectedValue = externalLifePolicy.Insurer.Key.ToString();
            txtPolicyNumber.Text = externalLifePolicy.PolicyNumber;
            ddlStatus.SelectedValue = externalLifePolicy.LifePolicyStatus.Key.ToString();
            txtSumInsured.Text = externalLifePolicy.SumInsured.ToString();
            chkPolicyCeded.Checked = externalLifePolicy.PolicyCeded;
        }

        public string Insurer
        {
            get { return GetSelectedValueFrom(ddlInsurer); }
        }

        public string PolicyNumber { get { return this.txtPolicyNumber.Text; } }

        public DateTime? CommencementDate { get { return this.dtCommencementDate.Date; } }

        public string Status
        {
            get { return GetSelectedValueFrom(ddlStatus); }
        }

        private string GetSelectedValueFrom(SAHLDropDownList dropDownList)
        {
            if (!string.IsNullOrEmpty(Request.Form[dropDownList.UniqueID]) && Request.Form[dropDownList.UniqueID] != "-select-")
                return (Convert.ToInt32(Request.Form[dropDownList.UniqueID])).ToString();
            else if (!string.IsNullOrEmpty(dropDownList.SelectedValue) && dropDownList.SelectedValue != "-select-")
                return (Convert.ToString(dropDownList.SelectedValue));
            else
                return Convert.ToString(-1);
        }

        public DateTime? CloseDate { get { return this.dtClosedate.Date; } }

        public double SumInsured { get { return this.txtSumInsured.Text.Length > 0 ? Convert.ToDouble(this.txtSumInsured.Text) : 0D; } }

        public bool PolicyCeded { get { return this.chkPolicyCeded.Checked; } }

    }
}