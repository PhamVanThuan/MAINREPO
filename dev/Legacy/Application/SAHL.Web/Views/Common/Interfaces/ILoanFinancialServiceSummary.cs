using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI.Events;
using System.Collections.Generic;

namespace SAHL.Web.Views.Common.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILoanFinancialServiceSummary : IViewBase
    {
        #region Methods
        /// <summary>
        /// Binds the MortgageLoan to Financial Service Summary View
        /// </summary>
        /// <param name="MortgageLoan"></param>
        void BindFinancialServiceSummaryData(IMortgageLoan MortgageLoan);

        /// <summary>
        /// Binds the FinancialAdjustment Data to the Rate Override Grid
        /// </summary>
        /// <param name="lstFinancialAdjustments"></param>
        void BindFinancialAdjustmentGrid(IList<IFinancialAdjustment> lstFinancialAdjustments);
        #endregion


        #region Properties

        /// <summary>
        /// Sets the Interest Current to Date Value
        /// </summary>
        double InterestCurrenttoDate { set;}

        /// <summary>
        /// Sets the Interest Total for Month Value
        /// </summary>
        double InterestTotalforMonth { set;}

        /// <summary>
        /// Sets the Interest Previous Month Value
        /// </summary>
        double InterestPreviousMonth { set;}

        /// <summary>
        /// Sets the Amortisation Amount Value and makes the control visible
        /// </summary>
        double AmortisationInstalment { set;}

        /// <summary>
        /// Visibility of AmortisingInstallment fields
        /// </summary>
        bool AmortisingInstallmentVisible { set;}


        /// <summary>
        /// Visibility of Loyalty Button
        /// </summary>
        bool LoyaltyButtonVisible { set;}
        #endregion


        /// <summary>
        /// Event handler for Loyalty Button Clicked
        /// </summary>
        event EventHandler LoyaltyButtonClicked;
    }
}
