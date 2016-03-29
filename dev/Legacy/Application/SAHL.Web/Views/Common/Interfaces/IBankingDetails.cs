using System;
using SAHL.Common.Web.UI;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Web.Views.Common.Interfaces
{

    public interface IBankingDetails : IViewBase
    {
        #region Events


        /// <summary>
        /// Raised when the submit button is clicked
        /// </summary>
        event EventHandler OnSubmitButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnBankDetailsGrid_SelectedIndexChanged;

        /// <summary>
        /// Raised when the search button is clicked, pass in the list of search criteria.
        /// </summary>
        event EventHandler OnSearchBankAccountClicked;
             

        /// <summary>
        /// Raised when the cancel button is clicked.
        /// </summary>
        event EventHandler OnCancelClicked;

        #endregion

        #region Properties

        #region Set Properties

        /// <summary>
        /// 
        /// </summary>
        bool IsUpdate { set;}

        /// <summary>
        /// GETS or SETS the ddlBank index for posting back
        /// </summary>
        int GETSETddlBank { set; get;}

        /// <summary>
        /// GETS or SETS the txtBranch index for posting back
        /// </summary>
        string GETSETtxtBranch { set; get;}

        /// <summary>
        /// GETS or SETS the ddlAccountType index for posting back
        /// </summary>
        int GETSETddlAccountType { set; get;}

        /// <summary>
        /// GETS or SETS the ddlStatus index for posting back
        /// </summary>
        int GETSETddlStatus { set; get;}

        /// <summary>
        /// GETS or SETS the txtAccountNumber index for posting back
        /// </summary>
        string GETSETtxtAccountNumber { set; get;}

        /// <summary>
        /// GETS or SETS the txtAccountName index for posting back
        /// </summary>
         string GETSETtxtAccountName { set; get;}

        /// <summary>
        /// Set whether the controls below the grid are visible
        /// </summary>
        bool ControlsVisible { set;}

        bool AccountTypeBondOnly { set;}

        /// <summary>
        /// Set whether the grid is enabled
        /// </summary>
        bool BankAccountGridEnabled { set;}

        /// <summary>
        /// Set whether the action buttons must be shown
        /// </summary>
        bool ShowButtons { set;}

        /// <summary>
        /// 
        /// </summary>
        ILegalEntity LegalEntity { set;}

        /// <summary>
        /// Set whether the controls values must be set for the selected index of the grid
        /// </summary>
        bool SetControlsToFirstAccount { set;}

        /// <summary>
        /// Set whether the search button must be visible
        /// </summary>
        bool SearchButtonVisible { set;}

        /// <summary>
        /// Set whether the bank account status control must be shown
        /// </summary>
        bool ShowStatus { set;}

        /// <summary>
        /// 
        /// </summary>
        bool SetControlsToSearchValues { set;}

        /// <summary>
        /// 
        /// </summary>
        string BankValue { set; get;}
        
        /// <summary>
        /// 
        /// </summary>
        string BranchCodeValue { set; get;}
        
        /// <summary>
        /// 
        /// </summary>
        string AccountTypeValue { set; get;}
        
        /// <summary>
        /// 
        /// </summary>
        string AccountNumberValue { set; get;}
        
        /// <summary>
        /// 
        /// </summary>
        string AccountNameValue { set; get;}

        /// <summary>
        /// 
        /// </summary>
        bool HideGridStatusColumn { set;}

        /// <summary>
        /// The text to be displayed on the submit button
        /// </summary>
        string SubmitButtonText { set;}

        /// <summary>
        /// 
        /// </summary>
        bool SubmitButtonEnabled { set;}

        /// <summary>
        /// 
        /// </summary>
        bool ShowReferenceRow { set;}

        /// <summary>
        /// 
        /// </summary>
        string GETSETtxtReference { set;get;}

        #endregion

        #region Get Properties

        /// <summary>
        /// The Bank Account  for the specified bank account
        /// </summary>
        IBankAccount SelectedBankAccount { get;}


        /// <summary>
        /// The legal entity bank account key that has been selected
        /// </summary>
        int SelectedLegalEntityBankAccountKey { get;}

        /// <summary>
        /// The selected account type 
        /// </summary>
        string AccountType { get;}

        /// <summary>
        /// The specified account name
        /// </summary>
        string AccountName { get;}

        /// <summary>
        /// The specified account number
        /// </summary>
        string AccountNumber { get;}       

        /// <summary>
        /// The specified bank name
        /// </summary>
        string BankName { get; set;}

        /// <summary>
        /// The selected branch code
        /// </summary>
        string BranchCode { get;}

        /// <summary>
        /// The selected branch name
        /// </summary>
        string BranchName { get;}
        
        /// <summary>
        /// The Key for the selected bank
        /// </summary>        
        string BankKey { get;}

        /// <summary>
        /// The Key for the selected branch
        /// </summary>
        string BranchKey { get;}

        /// <summary>
        /// The Key for the selected account type
        /// </summary>
        int AccountTypeKey { get;}

        /// <summary>
        /// The key for the selected general status
        /// </summary>
        int GeneralStatusKey { get;}

        #endregion

        #endregion

        #region methods
               

        /// <summary>
        /// Called to bind the list of bank accounts
        /// </summary>
        /// <param name="bankAccounts"></param>
        /// <param name="dictAppExp"></param>
        void BindGridForBankAccounts(List<IApplicationExpense> ApplicationExpenses);

        /// <summary>
        /// Called to bind the list of bank accounts
        /// </summary>
        /// <param name="bankAccounts"></param>
        void BindGridForLegalEntityBankAccounts(List<ILegalEntityBankAccount> bankAccounts);
      
        /// <summary>
        /// Binds the banking details data for the selected grid row to the controls
        /// </summary>
        /// <param name="banks"></param>
        /// <param name="bankAccountTypes"></param>
        /// <param name="generalStatuses"></param>
        void BindBankAccountControls(IEventList<IACBBank> banks, IEventList<IACBType> bankAccountTypes, ICollection<IGeneralStatus> generalStatuses);


        /// <summary>
        /// Gets or Sets the Bank Grid Index
        /// </summary>
        int BankDetailsGridIndex { get; set;}

        /// <summary>
        /// 
        /// </summary>
        void BindFromGrid(int BankGridKey);

        #endregion
    }

}
