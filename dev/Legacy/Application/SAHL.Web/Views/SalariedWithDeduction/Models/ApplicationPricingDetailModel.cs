using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAHL.Web.Views.SalariedWithDeduction.Models
{
    public class ApplicationPricingDetailModel
    {
        public double? TotalLoanRequirement { get; set; }
        public double? LTV { get; set; }
        public double? PTI { get; set; }
        public double? MarketRate { get; set; }
        public double? LinkRate { get; set; }
        public double? DiscountOnRate { get; set; }
        public double? PricingAdjustment { get; set; }
        public double? EffectiveRate { get; set; }
        public double? Instalment { get; set; }
        public double? Interest { get; set; }
        public double? RegistrationFee { get; set; }
        public double? InitiationFee { get; set; }
        public double? TotalFees { get; set; }
        public double? Term { get; set; }

        public ApplicationPricingDetailModel Clone()
        {
            return new ApplicationPricingDetailModel
            {
                TotalLoanRequirement = TotalLoanRequirement,
                LTV = LTV,
                PTI = PTI,
                MarketRate = MarketRate,
                LinkRate = LinkRate,
                DiscountOnRate = DiscountOnRate,
                PricingAdjustment = PricingAdjustment,
                EffectiveRate = EffectiveRate,
                Instalment = Instalment,
                Interest = Interest,
                RegistrationFee = RegistrationFee,
                InitiationFee = InitiationFee,
                TotalFees = TotalFees,
                Term = Term
            };
        }
    }
}