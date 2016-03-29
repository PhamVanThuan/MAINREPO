using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxClasses;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.Data;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.DataTransferObjects;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.Views.PersonalLoan.Interfaces;

namespace SAHL.Web.Views.PersonalLoan
{
    public partial class PersonalLoanCalculator : SAHLCommonBaseView, IPersonalLoanCalculator
    {
        public event EventHandler OnCalculateButtonClicked;

        public event EventHandler OnCancelButtonClicked;

        public event EventHandler<PersonalLoanOptionSelectedEventArgs> OnCreateApplicationButtonClicked;

        public int SelectedPersonalLoanOptionID { get; set; }

        public double LoanAmount
        {
            get
            {
                return tbLoanAmount.Text.Length > 0 ? Convert.ToDouble(tbLoanAmount.Text) : 0D;
            }
            private set
            {
                tbLoanAmount.Text = value.ToString();
            }
        }

        public int? Term
        {
            get
            {
                return tbLoanTerm.Text.Length > 0 ? Convert.ToInt32(tbLoanTerm.Text) : (int?)null;
            }
            private set
            {
                tbLoanTerm.Text = value.ToString();
            }
        }

        public double LifePremium
        {
            set
            {
                lblLifeCreditpremium.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat);
            }
        }

        public bool CreditLifePolicy
        {
            get { return chkLifePolicy.Checked; }
            set { chkLifePolicy.Checked = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!ShouldRunPage)
                return;

            // btnCreateApplication.Enabled = false;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!ShouldRunPage) return;
        }

        public void BindResult(IResult result)
        {
            lblInitiationFee.Text = result.InitiationFee.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblMonthlyFee.Text = result.MonthlyFee.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblLifeCreditpremium.Text = result.CreditLifePremium.ToString(SAHL.Common.Constants.CurrencyFormat);
            grdLoanOptions.DataSource = result.CalculatedItems;
            grdLoanOptions.DataBind();
            grdLoanOptions.Visible = true;
            pnlTermSummary.Visible = true;
        }

        protected void OnCreateApplicationClick(object sender, EventArgs e)
        {
            OnCreateApplicationButtonClicked(sender, new PersonalLoanOptionSelectedEventArgs(int.Parse(grdLoanOptions.SelectedKeyValue)));
        }

        protected void OnCalculateClick(object sender, EventArgs e)
        {
            OnCalculateButtonClicked(sender, e);
        }

        public void BindApplication(IApplicationInformationPersonalLoan applicationInformationPersonalLoan)
        {
            if (applicationInformationPersonalLoan != null)
            {
                LoanAmount = applicationInformationPersonalLoan.LoanAmount;
                Term = applicationInformationPersonalLoan.Term;
                CreditLifePolicy = applicationInformationPersonalLoan.LifePremium > 0;
                LifePremium = applicationInformationPersonalLoan.LifePremium;
            }
        }

        protected void OnCancelClick(object sender, EventArgs e)
        {
            OnCancelButtonClicked(sender, e);
            pnlTermSummary.Visible = false;
            tbLoanAmount.Text = "";
            tbLoanTerm.Text = "";
        }

        public string SetTextOnCreateApplicationButton
        {
            set { btnCreateApplication.Text = value; }
        }
    }
}