using System;
using System.Data;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI.Events;
using System.Collections.Generic;
using SAHL.Common.Web.UI.Controls;


namespace SAHL.Web.Views.Common.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICATSDisbursement : IViewBase
    {
        #region Properties




        /// <summary>
        /// 
        /// </summary>
        bool DisplayControlsVisible { set;}

        /// <summary>
        /// 
        /// </summary>
        bool DisbursementTypeEnabled { set;}

        /// <summary>
        /// 
        /// </summary>
        int DisbursementTypeSelectedValue { set; get;}

        /// <summary>
        /// 
        /// </summary>
        bool DisbursementGridVisible { set;}

        /// <summary>
        /// 
        /// </summary>
        bool AddControlsVisible { set;}

        /// <summary>
        /// 
        /// </summary>
        bool RollbackControlsVisible { set;}

        /// <summary>
        /// 
        /// </summary>
        bool CancelButtonVisible { set;}

        /// <summary>
        /// 
        /// </summary>
        bool SubmitButtonVisible { set;}

        /// <summary>
        /// 
        /// </summary>
        bool SaveButtonVisible { set;}

        /// <summary>
        /// 
        /// </summary>
        string SubmitButtonText { set;}

        /// <summary>
        /// 
        /// </summary>
        bool SubmitButtonEnabled { set;}

        /// <summary>
        /// 
        /// </summary>
        bool SaveButtonEnabled { set;}

        /// <summary>
        /// 
        /// </summary>
        bool DeleteButtonEnabled { set;}

        /// <summary>
        /// 
        /// </summary>
        bool DisbursementTypeLableVisible { set;}

        /// <summary>
        /// 
        /// </summary>
        GridPostBackType DisbursementGridPostBackType { set;}

        /// <summary>
        /// 
        /// </summary>
        double DisbursementAmount { get;set;}

        /// <summary>
        /// 
        /// </summary>
        double DisbursementTotalAmount { get;set; }

        /// <summary>
        /// 
        /// </summary>
        string DisbursementReference { get;}

        /// <summary>
        /// 
        /// </summary>
        int SelectedBankAccount { get; set;}

        /// <summary>
        /// 
        /// </summary>
        string DisbursementTypeLabelText { set;}

        /// <summary>
        /// 
        /// </summary>
        string TotalAmountLabelText { set;}

        ///// <summary>
        ///// 
        ///// </summary>
        //List<IBankAccount> BankAccountList { set;}

        /// <summary>
        /// 
        /// </summary>
        int LoanTransactionSelectedIndex { get;}

        /// <summary>
        /// 
        /// </summary>
        bool AddControlsEnabled { set;}

        bool AddButtonEnabled { set;}


        bool DisplayCancelConfirmationMessage { set;}
        bool DisplaySubmitConfirmationMessage { set;}
        bool DisplaySaveConfirmationMessage { set;}
        bool DisplayDeleteConfirmationMessage { set;}

        string CancelConfirmationMessage { get; set;}
        string SubmitConfirmationMessage { get; set;}
        string SaveConfirmationMessage { get; set;}
        string DeleteConfirmationMessage { get; set;}

        #endregion

        #region Event Handlers

        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler OnLoanTransactionGridSelectedIndexChanged;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnCancelButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnSubmitButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnAddDisbursementClicked;

        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler OnDeleteDisbursementClicked;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnSaveButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnDisbursementTypeSelectedIndexChanged;

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disbursementTypeList"></param>
        void BindDisbursementTypes(IReadOnlyEventList<IDisbursementTransactionType> disbursementTypeList);

        /// <summary>
        /// 
        /// </summary>
        void BindBankAccounts();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disbursements"></param>
        void BindGridDisbursements(IList<IDisbursement> disbursements);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loanTransactions"></param>
        void BindLoanTransactions(DataTable loanTransactions);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disbursementTransactions"></param>
        void BindDisbursementTransactions(DataTable disbursementTransactions);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BankAccountList"></param>
        void SetBankAccounts(List<IBankAccount> BankAccountList);

        #endregion

    }
}
