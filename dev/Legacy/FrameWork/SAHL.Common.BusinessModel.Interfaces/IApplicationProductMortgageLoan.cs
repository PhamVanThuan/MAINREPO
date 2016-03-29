using System;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Interfaces
{
    public interface IApplicationProductMortgageLoan : IApplicationProduct
    {
        void RecalculateMortgageLoanDetails();

        /// <summary>
        ///
        /// </summary>
        Int32? Term
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        Double? LoanAgreementAmount
        {
            get;
            //set;
        }

        /// <summary>
        ///
        /// </summary>
        Double? LoanAmountNoFees
        {
            get;
            set;
        }

        //commented out to get the build green, this needs to be looked at
        //void SetManualDiscount(SAHL.Common.Collections.Interfaces.IDomainMessageCollection Messages, double? discount, RateOverrideTypes roType);

        Double? ManualDiscount
        {
            get;
        }

        Double? DiscountedLinkRate
        {
            get;
        }

        Double? MarketRate
        {
            get;
        }

        Double? EffectiveRate
        {
            get;
        }

        void SetManualDiscount(SAHL.Common.Collections.Interfaces.IDomainMessageCollection Messages, double? discount, FinancialAdjustmentTypeSources roType);
    }
}