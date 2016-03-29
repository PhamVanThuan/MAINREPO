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
using System.Collections.Generic;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Cap.Interfaces
{
    public interface ICapOfferSales : IViewBase
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        double AccruedInterestMTD { set;}
        /// <summary>
        /// 
        /// </summary>
        double CommittedLoanValue { set;}
        /// <summary>
        /// 
        /// </summary>
        double LoanAgreementAmount { set;}
        /// <summary>
        /// 
        /// </summary>
        double LinkRate { set;}
        /// <summary>
        /// 
        /// </summary>
        bool ReasonDropDownVisible { set;}
        /// <summary>
        /// 
        /// </summary>
        string NTUReasonLabelText { set;}
        /// <summary>
        /// 
        /// </summary>
        string PaymentOptionText { set;}
        /// <summary>
        /// 
        /// </summary>
        bool OfferStatusListVisible { set;}
        /// <summary>
        /// 
        /// </summary>
        bool BalanceRowVisible { set;}
        /// <summary>
        /// 
        /// </summary>
        bool PromotionCheckBoxEnabled { set;}
        /// <summary>
        /// 
        /// </summary>
        bool PromotionCheckBoxChecked { set;}
        /// <summary>
        /// 
        /// </summary>
        string LegalEntityNameText { set;}
        /// <summary>
        /// 
        /// </summary>
        bool PaymentOptionDropDownVisible { set;get;}
        /// <summary>
        /// 
        /// </summary>
        string AccountNumberText { set;}
        /// <summary>
        /// 
        /// </summary>
        string SalesConsultantText { set;}
        /// <summary>
        /// 
        /// </summary>
        string NextResetDateText { set;}
        /// <summary>
        /// 
        /// </summary>
        string CapEffectiveDateText { set;}
        /// <summary>
        /// 
        /// </summary>
        string OfferStartDateText { set;}
        /// <summary>
        /// 
        /// </summary>
        string OfferEndDateText { set;}
        /// <summary>
        /// 
        /// </summary>
        double TotalBondAmount { set;}
        /// <summary>
        /// 
        /// </summary>
        string OfferStatusText { set;}
        /// <summary>
        /// 
        /// </summary>
        string BalanceToCap { set;}
        /// <summary>
        /// 
        /// </summary>
        bool ButtonRowVisible { set;}
        /// <summary>
        /// /
        /// </summary>
        string SubmitButtonText { set;}
        /// <summary>
        /// 
        /// </summary>
        bool SubmitButtonEnabled { set;}
        /// <summary>
        /// 
        /// </summary>
        bool CancelButtonVisible { set;}
        /// <summary>
        /// 
        /// </summary>
        string ConfirmMessageText { set;}
        /// <summary>
        /// 
        /// </summary>
        int GridSelectedIndex { get;}
        /// <summary>
        /// 
        /// </summary>
        GridPostBackType CapGridPostBackType { set;}
        /// <summary>
        /// 
        /// </summary>
        int CapReasonSelectedValue { get;}
        /// <summary>
        /// 
        /// </summary>
        int CapPaymentOptionSelectedValue { get; set;}
        /// <summary>
        /// 
        /// </summary>
        bool CapPageVisible { set;}
        /// <summary>
        /// 
        /// </summary>
        bool CapReportVisible { set;}
        /// <summary>
        /// 
        /// </summary>
        double HouseholdIncome { set;}
        /// <summary>
        /// 
        /// </summary>
        double LatestValuation { set;}

        /// <summary>
        /// 
        /// </summary>
        double VariableInstallment { set;}

        #endregion

        #region Event Handlers
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnCancelButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnSubmitButtonClicked;
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="capDetailList"></param>
        void BindCapGrid(IList<ICapApplicationDetail> capDetailList);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="installmentData"></param>
        /// <param name="installmentHeaderValue"></param>
        void BindInstalmentGrid(DataTable installmentData, string installmentHeaderValue);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="capReasons"></param>
        void BindReasonDropdown(IList<ICapNTUReason> capReasons);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AccountKey"></param>
        void LoadCapLetter(int AccountKey);
        /// <summary>
        /// Binds the Cap Payment Options list to the drop down
        /// </summary>
        /// <param name="capPaymentOptions"></param>
        void BindPaymentOptionDropDown(IList<ICapPaymentOption> capPaymentOptions);

    }
}
