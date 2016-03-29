using SAHL.Common.Web.UI;
using SAHL.Web.Views.PersonalLoan.Interfaces;
using System;
using System.Linq;

namespace SAHL.Web.Views.PersonalLoan
{
    public partial class PersonalLoanMaintainLifePolicy : SAHLCommonBaseView, IPersonalLoanMaintainLifePolicy
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

        public void SetupSummaryView()
        {
            lblClosedate.Visible = true;
            lblCommencementDate.Visible = true;
            lblInsurer.Visible = true;
            lblPolicyNumber.Visible = true;
            lblStatus.Visible = true;
            lblSumInsured.Visible = true;

            txtInsurer.Visible = false;
            txtPolicyNumber.Visible = false;
            txtSumInsured.Visible = false;
            dtClosedate.Visible = false;
            dtCommencementDate.Visible = false;
            ddlStatus.Visible = false;
            chkPolicyCeded.Enabled = false;
            btnCancel.Visible = false;
            btnSave.Visible = false;
        }

        public void BindStatus(System.Collections.Generic.IDictionary<string, string> lifeStatus)
        {
            ddlStatus.DataSource = lifeStatus;
            ddlStatus.DataValueField = "Key";
            ddlStatus.DataTextField = "Description";
            ddlStatus.DataBind();
        }

        public void BindMaintainLifePolicy()
        {
            throw new NotImplementedException();
        }

        public string Insurer { get { return this.txtInsurer.Text; } }

        public string PolicyNumber { get { return this.txtPolicyNumber.Text; } }

        public DateTime? CommencementDate { get { return this.dtCommencementDate.Date; } }

        public string Status
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.Form[ddlStatus.UniqueID]) && Request.Form[ddlStatus.UniqueID] != "-select-")
                    return (Convert.ToInt32(Request.Form[ddlStatus.UniqueID])).ToString();
                else if (!string.IsNullOrEmpty(ddlStatus.SelectedValue) && ddlStatus.SelectedValue != "-select-")
                    return (Convert.ToString(ddlStatus.SelectedValue));
                else
                    return Convert.ToString(-1);
            }
        }

        public DateTime? CloseDate { get { return this.dtClosedate.Date; } }

        public int SumInsured { get { return Convert.ToInt32(this.txtSumInsured.Text); } }

        public bool PolicyCeded { get { return this.chkPolicyCeded.Checked; } }
    }
}