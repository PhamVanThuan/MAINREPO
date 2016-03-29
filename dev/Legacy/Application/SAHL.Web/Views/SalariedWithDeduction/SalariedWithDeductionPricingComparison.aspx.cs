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
using SAHL.Web.Views.SalariedWithDeduction.Interfaces;
using SAHL.Web.Views.SalariedWithDeduction.Models;

namespace SAHL.Web.Views.SalariedWithDeduction
{
    public partial class SalariedWithDeductionPricingComparison : SAHLCommonBaseView, ISalariedWithDeductionPricingComparison
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!this.ShouldRunPage)
                return;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.ShouldRunPage)
                return;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            OnCancelButtonClicked(sender, e);
        }

        public event EventHandler OnCancelButtonClicked;

        public void DisplayCurrentApplicationPricingDetails(ApplicationPricingDetailModel currentPricing)
        {
            this.lblTotalLoanRequirement_Current.Text = currentPricing.TotalLoanRequirement.HasValue ? currentPricing.TotalLoanRequirement.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
            this.lblLTV_Current.Text = currentPricing.LTV.HasValue ? currentPricing.LTV.Value.ToString(SAHL.Common.Constants.RateFormat) : "-";
            this.lblPTI_Current.Text = currentPricing.PTI.HasValue ? currentPricing.PTI.Value.ToString(SAHL.Common.Constants.RateFormat) : "-";
            this.lblMarketRate_Current.Text = currentPricing.MarketRate.HasValue ? currentPricing.MarketRate.Value.ToString(SAHL.Common.Constants.RateFormat) : "-";
            this.lblLinkRate_Current.Text = currentPricing.LinkRate.HasValue ? currentPricing.LinkRate.Value.ToString(SAHL.Common.Constants.RateFormat) : "-";
            this.lblDiscountOnRate_Current.Text = currentPricing.DiscountOnRate.HasValue ? currentPricing.DiscountOnRate.Value.ToString(SAHL.Common.Constants.RateFormat) : "-";
            this.lblPricingAdjustment_Current.Text = currentPricing.PricingAdjustment.HasValue ? currentPricing.PricingAdjustment.Value.ToString(SAHL.Common.Constants.RateFormat) : "-";
            this.lblEffectiveRate_Current.Text = currentPricing.EffectiveRate.HasValue ? currentPricing.EffectiveRate.Value.ToString(SAHL.Common.Constants.RateFormat) : "-";
            this.lblInstalment_Current.Text = currentPricing.Instalment.HasValue ? currentPricing.Instalment.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
            this.lblInterest_Current.Text = currentPricing.Interest.HasValue ? currentPricing.Interest.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
            this.lblRegistrationFee_Current.Text = currentPricing.RegistrationFee.HasValue ? currentPricing.RegistrationFee.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
            this.lblInitiationFee_Current.Text = currentPricing.InitiationFee.HasValue ? currentPricing.InitiationFee.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
            this.lblTotalFees_Current.Text = currentPricing.TotalFees.HasValue ? currentPricing.TotalFees.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
        }

        public void DisplayNewApplicationPricingDetails(ApplicationPricingDetailModel newPricing)
        {
            this.lblTotalLoanRequirement_New.Text = newPricing.TotalLoanRequirement.HasValue ? newPricing.TotalLoanRequirement.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
            this.lblLTV_New.Text = newPricing.LTV.HasValue ? newPricing.LTV.Value.ToString(SAHL.Common.Constants.RateFormat) : "-";
            this.lblPTI_New.Text = newPricing.PTI.HasValue ? newPricing.PTI.Value.ToString(SAHL.Common.Constants.RateFormat) : "-";
            this.lblMarketRate_New.Text = newPricing.MarketRate.HasValue ? newPricing.MarketRate.Value.ToString(SAHL.Common.Constants.RateFormat) : "-";
            this.lblLinkRate_New.Text = newPricing.LinkRate.HasValue ? newPricing.LinkRate.Value.ToString(SAHL.Common.Constants.RateFormat) : "-";
            this.lblDiscountOnRate_New.Text = newPricing.DiscountOnRate.HasValue ? newPricing.DiscountOnRate.Value.ToString(SAHL.Common.Constants.RateFormat) : "-";
            this.lblPricingAdjustment_New.Text = newPricing.PricingAdjustment.HasValue ? newPricing.PricingAdjustment.Value.ToString(SAHL.Common.Constants.RateFormat) : "-";
            this.lblEffectiveRate_New.Text = newPricing.EffectiveRate.HasValue ? newPricing.EffectiveRate.Value.ToString(SAHL.Common.Constants.RateFormat) : "-";
            this.lblInstalment_New.Text = newPricing.Instalment.HasValue ? newPricing.Instalment.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
            this.lblInterest_New.Text = newPricing.Interest.HasValue ? newPricing.Interest.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
            this.lblRegistrationFee_New.Text = newPricing.RegistrationFee.HasValue ? newPricing.RegistrationFee.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
            this.lblInitiationFee_New.Text = newPricing.InitiationFee.HasValue ? newPricing.InitiationFee.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
            this.lblTotalFees_New.Text = newPricing.TotalFees.HasValue ? newPricing.TotalFees.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
        }
    }
}