using SAHL.Common;
using SAHL.Common.Web.UI;
using SAHL.Services.Interfaces.LifeDomain.Models;
using SAHL.Web.Views.Life.Interfaces;
using System;
using System.Collections.Generic;

namespace SAHL.Web.Views.Life
{
    public partial class DisabilityClaimDetails : SAHLCommonBaseView, IDisabilityClaimDetails
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!this.ShouldRunPage)
                return;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.ShouldRunPage)
                return;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!this.ShouldRunPage)
                return;
        }

        public event EventHandler OnSubmitButtonClicked;

        public event EventHandler OnCancelButtonClicked;

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            OnSubmitButtonClicked(sender, e);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            OnCancelButtonClicked(sender, e);
        }


        public bool DateOfDiagnosisEditable
        {
            set 
            { 
                lblDateOfDiagnosis.Visible = !value;
                dtDateOfDiagnosis.Visible = value;
            }
        }

        public bool DisabilityTypeEditable
        {
            set 
            {
                lblDisabilityType.Visible = !value;
                ddlDisabilityType.Visible = value;
            }
        }

        public bool AdditionalDisabilityDetailsEditable
        {
            set 
            {
                lblOtherDisabilityComments.Visible = !value;
                txtOtherDisabilityComments.Visible = value; 
            }
        }

        public bool OccupationEditable
        {
            set 
            {
                lblClaimantOccupation.Visible = !value;
                txtClaimantOccupation.Visible = value;
            }
        }

        public bool LastDateWorkedEditable
        {
            set 
            {
                lblLastDateWorked.Visible = !value;
                dtLastDateWorked.Visible = value; 
            }
        }

        public bool ExpectedReturnToWorkDateEditable
        {
            set 
            {
                lblExpectedReturnToWorkDate.Visible = !value;
                dtExpectedReturnToWorkDate.Visible = value;
            }
        }

        public bool UpdateButtonsVisible
        {
            set 
            { 
                pnlButtons.Visible = value; 
            }
        }

        public void BindDisabilityTypes(IDictionary<int, string> disabilityTypes)
        {
            ddlDisabilityType.DataSource = disabilityTypes;
            ddlDisabilityType.DataBind();
        }

        public void BindDisabilityClaim(DisabilityClaimDetailModel disabilityClaim) 
        {
            lblClaimant.Text = disabilityClaim.ClaimantLegalEntityDisplayName;
            lblDateClaimReceived.Text = disabilityClaim.DateClaimReceived.Date.ToString(SAHL.Common.Constants.DateFormat);
            lblClaimStatus.Text = disabilityClaim.DisabilityClaimStatus;

            // view mode stufff
            lblLastDateWorked.Text = disabilityClaim.LastDateWorked.HasValue ? disabilityClaim.LastDateWorked.Value.ToString(SAHL.Common.Constants.DateFormat) : "-";
            lblDateOfDiagnosis.Text = disabilityClaim.DateOfDiagnosis.HasValue ? disabilityClaim.DateOfDiagnosis.Value.ToString(SAHL.Common.Constants.DateFormat) : "-";
            lblClaimantOccupation.Text = !string.IsNullOrWhiteSpace(disabilityClaim.ClaimantOccupation) ? disabilityClaim.ClaimantOccupation : "-";
            lblDisabilityType.Text = !string.IsNullOrWhiteSpace(disabilityClaim.DisabilityType) ? disabilityClaim.DisabilityType : "-";
            lblOtherDisabilityComments.Text = !string.IsNullOrWhiteSpace(disabilityClaim.OtherDisabilityComments) ? disabilityClaim.OtherDisabilityComments : "-";
            lblExpectedReturnToWorkDate.Text = disabilityClaim.ExpectedReturnToWorkDate.HasValue ? disabilityClaim.ExpectedReturnToWorkDate.Value.ToString(SAHL.Common.Constants.DateFormat) : "-";

            // update mode stuff
            DateTime? date = null;
            dtLastDateWorked.Date = disabilityClaim.LastDateWorked.HasValue ? disabilityClaim.LastDateWorked.Value : date;
            dtDateOfDiagnosis.Date = disabilityClaim.DateOfDiagnosis.HasValue ? disabilityClaim.DateOfDiagnosis.Value : date;

            txtClaimantOccupation.Text = disabilityClaim.ClaimantOccupation;
            ddlDisabilityType.SelectedValue = disabilityClaim.DisabilityTypeKey.HasValue ? disabilityClaim.DisabilityTypeKey.ToString() : "";
            txtOtherDisabilityComments.Text = disabilityClaim.OtherDisabilityComments;
            dtExpectedReturnToWorkDate.Date = disabilityClaim.ExpectedReturnToWorkDate.HasValue ? disabilityClaim.ExpectedReturnToWorkDate.Value : date;

            // update approved stuff
            lblLastDateWorked.Text = disabilityClaim.LastDateWorked.HasValue ? disabilityClaim.LastDateWorked.Value.ToString(SAHL.Common.Constants.DateFormat) : "-";
            lblDateOfDiagnosis.Text = disabilityClaim.DateOfDiagnosis.HasValue ? disabilityClaim.DateOfDiagnosis.Value.ToString(SAHL.Common.Constants.DateFormat) : "-";
        }

        public void BindPaymentDetails(string approvedByUser, DateTime approvedDate, int numberOfPayments, DateTime paymentStartDate, DateTime paymentEndDate)
        {
            pnlPaymentDetails.Visible = true;
            lblApprovedBy.Text = approvedByUser;
            lblNumberOfPayments.Text = numberOfPayments.ToString();
            lblPaymentApprovedDate.Text = approvedDate.ToString(SAHL.Common.Constants.DateFormat);
            lblPaymentStartDate.Text = paymentStartDate.ToString(SAHL.Common.Constants.DateFormat);
            lblPaymentEndDate.Text = paymentEndDate.ToString(SAHL.Common.Constants.DateFormat);
        }

        public DateTime? LastDateWorked
        {
            get
            {
                if (dtLastDateWorked.DateIsValid)
                    return dtLastDateWorked.Date.Value;

                return null;
            }
        }

        public DateTime? DateOfDiagnosis
        {
            get
            {
                if (dtDateOfDiagnosis.DateIsValid)
                    return dtDateOfDiagnosis.Date.Value;

                return null;
            }
        }

        public string ClaimantOccupation
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(txtClaimantOccupation.Text))
                    return txtClaimantOccupation.Text;

                return null;
            }
        }

        public int? DisabilityTypeKey
        {
            get
            {
                int disabilityTypeKey;
                if (int.TryParse(ddlDisabilityType.SelectedValue, out disabilityTypeKey))
                    return disabilityTypeKey;

                return null;
            }
        }

        public string OtherDisabilityComments
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(txtOtherDisabilityComments.Text))
                    return txtOtherDisabilityComments.Text;

                return null;
            }
        }

        public DateTime? ExpectedReturnToWorkDate
        {
            get
            {
                if (dtExpectedReturnToWorkDate.DateIsValid)
                    return dtExpectedReturnToWorkDate.Date.Value;

                return null;
            }
        }
    }
}