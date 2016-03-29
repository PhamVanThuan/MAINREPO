using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.V3.Framework.Model
{
    public class ApplicationLoanDetail
    {
        public ApplicationLoanDetail(int? term, double? loanAgreementAmount, double? ltv, double? pti, double? marketRate, double? linkRate, double? pricingForRiskAdjustment, double? effectiveRate, double? instalment, double? interest)
        {
            this.Term = term;
            this.LoanAgreementAmount = loanAgreementAmount;
            this.LTV = ltv;
            this.PTI = pti;
            this.MarketRate = marketRate;
            this.LinkRate = linkRate;
            this.PricingForRiskAdjustment = pricingForRiskAdjustment;
            this.EffectiveRate = effectiveRate;
            this.Instalment = instalment;
            this.Interest = interest;
        }

        public int? Term { get; protected set; }

        public double? LoanAgreementAmount { get; protected set; }

        public double? LTV { get; protected set; }

        public double? PTI { get; protected set; }

        public double? MarketRate { get; protected set; }

        public double? LinkRate { get; protected set; }

        public double? PricingForRiskAdjustment { get; protected set; }

        public double? EffectiveRate { get; protected set; }

        public double? Instalment { get; protected set; }

        public double? Interest { get; protected set; }
    }
}
