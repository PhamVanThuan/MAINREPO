using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI;
using System;
using System.Linq;

namespace SAHL.Web.Views.PersonalLoan.Interfaces
{
    public interface IPersonalLoanChangeTerm : IViewBase
    {
        /// <summary>
        /// Enable/Disable Calculate button
        /// </summary>
        bool EnableCalculateButton { set; }

        /// <summary>
        /// Enable/Disable submit button
        /// </summary>
        bool EnableSubmitButton { set; }

        /// <summary>
        /// Bind Grid for Personal Loans
        /// </summary>
        /// <param name="lstFinancialServices"></param>
        void BindPersonalLoansGrid(IEventList<IFinancialService> lstFinancialServices);

        /// <summary>
        ///
        /// </summary>
        string MemoComments { get; }

        /// <summary>
        /// Get Term Captured on screen
        /// </summary>
        int CapturedTerm { get; set; }

        /// <summary>
        /// The new calculated instalment
        /// </summary>
        double NewInstalment { get; set; }

        /// <summary>
        ///
        /// </summary>
        double CreditLifePremium { get; set; }

        /// <summary>
        ///
        /// </summary>
        double MonthlyServiceFee { get; set; }

        /// <summary>
        /// Calculate button Clicked
        /// </summary>
        event EventHandler CalculateButtonClicked;

        /// <summary>
        /// Submit button clicked
        /// </summary>
        event EventHandler SubmitButtonClicked;

        /// <summary>
        /// Cancel button Clicked
        /// </summary>
        event EventHandler CancelButtonClicked;
    }
}