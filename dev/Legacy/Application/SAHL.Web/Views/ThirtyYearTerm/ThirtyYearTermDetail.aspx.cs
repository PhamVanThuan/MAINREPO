using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.ThirtyYearTerm.Interfaces;
using SAHL.V3.Framework.Model;

namespace SAHL.Web.Views.ThirtyYearTerm
{
    /// <summary>
    ///
    /// </summary>
    public partial class ThirtyYearTermDetail : SAHLCommonBaseView, IThirtyYearTermDetail
    {
        private bool applicationQualifiesFor30Year;

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

            pnlDetail.Visible = applicationQualifiesFor30Year;
            pnlDecisionTreeMessages.Visible = !applicationQualifiesFor30Year;
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

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            OnSubmitButtonClicked(sender, e);
        }


        #region I30YearTermDetail Members

        public event EventHandler OnCancelButtonClicked;

        public event EventHandler OnSubmitButtonClicked;

        public bool ShowCancelButton
        {
            set { btnCancel.Visible = value; }
        }

        public bool ShowSubmitButton
        {
            set { btnSubmit.Visible = value; }
        }

        public string SubmitButtonText
        {
            set { btnSubmit.Text = value; }
        }


        public bool ApplicationQualifiesFor30Year
        {
            get
            {
                return applicationQualifiesFor30Year;
            }
            set
            {
                applicationQualifiesFor30Year = value;
            }
        }

        public IList<string> DecisionTreeMessages
        {
            set
            {
                lstDecisionTreeMessages.Items.Clear();
                foreach (var msg in value)
                {
                    lstDecisionTreeMessages.Items.Add(msg); 
                }
            }
        }

        #endregion

        public void DisplayCurrentTermDetails(ApplicationLoanDetail termDetail)
        {
            lblTerm_20.Text = termDetail.Term.HasValue ? termDetail.Term.Value.ToString() + " months" : "-";
            lblLoanAgreementAmt_20.Text = termDetail.LoanAgreementAmount.HasValue ? termDetail.LoanAgreementAmount.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
            lblLTV_20.Text = termDetail.LTV.HasValue ? termDetail.LTV.Value.ToString(SAHL.Common.Constants.RateFormat) : "-";
            lblPTI_20.Text = termDetail.PTI.HasValue ? termDetail.PTI.Value.ToString(SAHL.Common.Constants.RateFormat) : "-";
            lblMarketRate_20.Text = termDetail.MarketRate.HasValue ? termDetail.MarketRate.Value.ToString(SAHL.Common.Constants.RateFormat) : "-";
            lblLinkRate_20.Text = termDetail.LinkRate.HasValue ? termDetail.LinkRate.Value.ToString(SAHL.Common.Constants.RateFormat) : "-";
            lblPricingFoRiskAdj_20.Text = termDetail.PricingForRiskAdjustment.HasValue ? termDetail.PricingForRiskAdjustment.Value.ToString(SAHL.Common.Constants.RateFormat) : "-";
            lblEffectiveRate_20.Text = termDetail.EffectiveRate.HasValue ? termDetail.EffectiveRate.Value.ToString(SAHL.Common.Constants.RateFormat) : "-";
            lblInstalment_20.Text = termDetail.Instalment.HasValue ? termDetail.Instalment.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
            lblInterest_20.Text = termDetail.Interest.HasValue ? termDetail.Interest.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
        }

        public void Display30YearTermDetails(ApplicationLoanDetail termDetail)
        {
            lblTerm_30.Text = termDetail.Term.HasValue ? termDetail.Term.Value.ToString() + " months" : "-";
            lblLoanAgreementAmt_30.Text = termDetail.LoanAgreementAmount.HasValue ? termDetail.LoanAgreementAmount.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
            lblLTV_30.Text = termDetail.LTV.HasValue ? termDetail.LTV.Value.ToString(SAHL.Common.Constants.RateFormat) : "-";
            lblPTI_30.Text = termDetail.PTI.HasValue ? termDetail.PTI.Value.ToString(SAHL.Common.Constants.RateFormat) : "-";
            lblMarketRate_30.Text = termDetail.MarketRate.HasValue ? termDetail.MarketRate.Value.ToString(SAHL.Common.Constants.RateFormat) : "-";
            lblLinkRate_30.Text = termDetail.LinkRate.HasValue ? termDetail.LinkRate.Value.ToString(SAHL.Common.Constants.RateFormat) : "-";
            lblPricingFoRiskAdj_30.Text = termDetail.PricingForRiskAdjustment.HasValue ? termDetail.PricingForRiskAdjustment.Value.ToString(SAHL.Common.Constants.RateFormat) : "-";
            lblEffectiveRate_30.Text = termDetail.EffectiveRate.HasValue ? termDetail.EffectiveRate.Value.ToString(SAHL.Common.Constants.RateFormat) : "-";
            lblInstalment_30.Text = termDetail.Instalment.HasValue ? termDetail.Instalment.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
            lblInterest_30.Text = termDetail.Interest.HasValue ? termDetail.Interest.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
        }
    }
}