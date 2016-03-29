using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Life.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;
using System.Linq;
using SAHL.Services.Interfaces.LifeDomain.Models;

namespace SAHL.Web.Views.Life
{
    public partial class DisabilityClaimApprove : SAHLCommonBaseView, IDisabilityClaimApprove
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

        public int LoanDebitOrderDay
        {
            set { lblDebitOrderDay.Text = value.ToString(); }
        }

        public DateTime DateLastWorked
        {
            set { lblDateLastWorked.Text = value.ToString(SAHL.Common.Constants.DateFormat); }
        }

        public DateTime DisbilityPaymentStartDate
        {
            set { lblDisbilityPaymentStartDate.Text = value.ToString(SAHL.Common.Constants.DateFormat);}
        }

        public DateTime? DisbilityPaymentEndDate
        {
            get
            {
                DateTime endDate;
                if (DateTime.TryParseExact(hidDisbilityPaymentEndDate.Value, SAHL.Common.Constants.DateFormat, System.Globalization.CultureInfo.CurrentCulture, DateTimeStyles.None, out endDate))
                    return endDate;
                else
                    return null;
            }
        }

        public int? NoOfInstalmentsAuthorised
        {
            get
            {
                int noOfInstalmentsAuthorised;
                if (int.TryParse(txtInstalmentsAuthorised.Text, out noOfInstalmentsAuthorised))
                    return noOfInstalmentsAuthorised;

                return null;
            }
        }


        public void BindFurtherLendingExclusions(IEnumerable<DisabilityClaimFurtherLendingExclusionModel> furtherLendingExclusions)
        {
            if (furtherLendingExclusions == null || furtherLendingExclusions.Count() <= 0)
                pnlExclusions.Visible = false;
            else
            {
                pnlExclusions.Visible = true;

                gridFurtherLendingExclusions.AutoGenerateColumns = false;
                gridFurtherLendingExclusions.AddGridBoundColumn("Description", "Description", Unit.Percentage(20), HorizontalAlign.Left, true);
                gridFurtherLendingExclusions.AddGridBoundColumn("Date", "Disbursement Date", Unit.Percentage(20), HorizontalAlign.Center, true);
                gridFurtherLendingExclusions.AddGridBoundColumn("Amount", "Amount (incl fees)", Unit.Percentage(20), HorizontalAlign.Right, true);

                gridFurtherLendingExclusions.DataSource = furtherLendingExclusions;
                gridFurtherLendingExclusions.DataBind();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridFurtherLendingExclusions_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            TableCellCollection cells = e.Row.Cells;

            DisabilityClaimFurtherLendingExclusionModel furtherLendingExclusion = e.Row.DataItem as DisabilityClaimFurtherLendingExclusionModel;
            if (e.Row.DataItem != null)
            {
                cells[0].Text = furtherLendingExclusion.Description;
                cells[1].Text = furtherLendingExclusion.Date.ToString(SAHL.Common.Constants.DateFormat);
                cells[2].Text = furtherLendingExclusion.Amount.ToString(SAHL.Common.Constants.CurrencyFormat);
            }
        }
    }
}