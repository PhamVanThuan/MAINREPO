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
    public interface ILoanSummary : IViewBase
    {
        #region Methods

        /// <summary>
        /// Binds Account Details to Loan Summary View
        /// </summary>
        /// <param name="account"></param>
        void BindAccountLoanSummaryData(IAccount account);

        /// <summary>
        /// Binds the total data to the Loan Summary View
        /// </summary>
        void BindTotalData();

        /// <summary>
        /// Binds the MortgageLoan data to the View
        /// </summary>
        void BindMorgageLoanData();

        /// <summary>
        /// Bind the Financial Service Data to the Loans Grid
        /// </summary>
        /// <param name="lstMortgageLoans"></param>
        /// <param name="gridtotalInstallment"></param>
        /// <param name="gridtotalArrearBalance"></param>
        /// <param name="gridtotalCurrentBalance"></param>
        void BindLoansGrid(IList<IMortgageLoan> lstMortgageLoans,
                                    double gridtotalInstallment,
                                    double gridtotalArrearBalance,
                                    double gridtotalCurrentBalance);

        /// <summary>
        /// Bind the Financial Service Data to the Short Term Loans Grid
        /// </summary>
        /// <param name="lstShortTermMortgageLoans"></param>
        /// <param name="gridtotalInstallment"></param>
        /// <param name="gridtotalArrearBalance"></param>
        /// <param name="gridtotalCurrentBalance"></param>
        void BindShortTermLoansGrid(IList<IMortgageLoan> lstShortTermMortgageLoans,
                                    double gridtotalInstallment,
                                    double gridtotalArrearBalance,
                                    double gridtotalCurrentBalance);

        #endregion


        #region Properties
        /// <summary>
        /// Sets the Interest Current to Date Value for the Variable loan
        /// </summary>
        double InterestCurrenttoDateVariable { set;}

        /// <summary>
        /// Sets the Interest Current to Date Value for the Variable loan
        /// </summary>
        double InterestTotalforMonthVariable { set;}

        /// <summary>
        /// Sets the Interest Previous Month Value for the Variable loan
        /// </summary>
        double InterestPreviousMonthVariable { set;}

        /// <summary>
        /// Sets the Interest Current to Date Value for the Fixed loan
        /// </summary>
        double InterestCurrenttoDateFixed { set;}

        /// <summary>
        /// Sets the Interest Current to Date Value for the Fixed loan
        /// </summary>
        double InterestTotalforMonthFixed { set;}

        /// <summary>
        /// Sets the Interest Previous Month Value for the Fixed loan
        /// </summary>
        double InterestPreviousMonthFixed { set;}

        /// <summary>
        /// Visibility of Fixed Loan Fields fields
        /// </summary>
        bool FixedLoanControlsVisible { set;}

        /// <summary>
        /// 
        /// </summary>
        bool TitleDeedOnFile {set;}

        /// <summary>
        /// Set the text of the maturity date label
        /// </summary>
        string MaturityDateTitleText { set;}

        bool ManualLifePolicyPaymentVisible { set; }

        #region Total Details
        /// <summary>
        /// 
        /// </summary>
        double LatestProperyValuationAmount { set;}
        /// <summary>
        /// 
        /// </summary>
        double LoanAgreementAmount { set;}
        /// <summary>
        /// 
        /// </summary>
        double TotalBondAmount { set;}
        /// <summary>
        /// 
        /// </summary>
        double CommittedLoanValue { set; }
        /// <summary>
        /// 
        /// </summary>
        double HouseholdIncome { set;}
        /// <summary>
        /// 
        /// </summary>
        double PTI { set;}
        /// <summary>
        /// 
        /// </summary>
        double LTV { set;}
        #endregion

        #region MortgageLoanDetails
        /// <summary>
        /// 
        /// </summary>
        string SpvDescription { set;}
        /// <summary>
        /// 
        /// </summary>
        DateTime ValuationDate { set;}
        /// <summary>
        /// 
        /// </summary>
        DateTime MaturityDate { set;}
        /// <summary>
        /// 
        /// </summary>
        int DebitOrderDay { set;}

        /// <summary>
        /// 
        /// </summary>
        bool NonPerformingLoan { set; }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        bool NaedoCompliant { set; }

        /// <summary>
        /// 
        /// </summary>
        bool IsGEPFFunded { set; get; }

        #endregion

    }
}
