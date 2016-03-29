using System;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;

namespace SAHL.Web.Views.Common.Interfaces
{
    
    public interface IBankingDetailsSearch : IViewBase
    {
        event KeyChangedEventHandler OnSearchGridSelectedIndexChanged;
        event EventHandler OnCancelButtonClicked;
        event KeyChangedEventHandler OnUseButtonClicked;

        IBankAccount BankAcccount { get;}

        /// <summary>
        /// 
        /// </summary>
        IBankAccount SelectedBankAccount { get;}


        /// <summary>
        /// 
        /// </summary>
        int BankDetailsSearchGridItemIndex { get; set;}


        /// <summary>
        /// The selected account type 
        /// </summary>
        //string AccountType { get;}

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
        string BankName { get;}

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
       // int BranchKey { get;}

        /// <summary>
        /// The Key for the selected account type
        /// </summary>
        int AccountTypeKey { get;}


        /// <summary>
        /// Binds the list of found bank accounts to the grid
        /// </summary>
        /// <param name="matchingBankAccounts"></param>
        void BindSearchGrid(List<BankingDetailsSearchGridRowItem> matchingBankAccounts);
    }


    /// <summary>
    /// 
    /// </summary>
    public class BankingDetailsSearchGridRowItem
    {
        IBankAccount _bankAccount;
        int _bankAccountKey;
        int _legalEntityKey;
        string _legalEntityName;
        string _legalEntityNumber;
        string _legalEntityStatus;
        string _ACBBranchKey;
        string _accountName;
        string _accountNumber;
        string _ACBBankDescription;
        string _ACBBranchDescription;

        /// <summary>
        /// 
        /// </summary>
        public IBankAccount BankAccount
        {
            get
            {
                return _bankAccount;
            }
            set
            {
                _bankAccount = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string AccountName
        {
            get
            {
                return _accountName;
            }
            set
            {
                _accountName = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string AccountNumber
        {
            get
            {
                return _accountNumber;
            }
            set
            {
                _accountNumber = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ACBBankDescription
        {
            get
            {
                return _ACBBankDescription;
            }
            set
            {
                _ACBBankDescription = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ACBBranchDescription
        {
            get
            {
                return _ACBBranchDescription;
            }
            set
            {
                _ACBBranchDescription = value;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public string ACBBranchKey
        {
            get
            {
                return _ACBBranchKey;
            }
            set
            {
                _ACBBranchKey = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int BankAccountKey
        {
            get
            {
                return _bankAccountKey;
            }
            set
            {
                _bankAccountKey = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int LegalEntityKey
        {
            get
            {
                return _legalEntityKey;
            }
            set
            {
                _legalEntityKey = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string LegalEntityName
        {
            get
            {
                return _legalEntityName;
            }
            set
            {
                _legalEntityName = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string LegalEntityNumber
        {
            get
            {
                return _legalEntityNumber;
            }
            set
            {
                _legalEntityNumber = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string LegalEntityStatus
        {
            get
            {
                return _legalEntityStatus;
            }
            set
            {
                _legalEntityStatus = value;
            }
        }


    }

}
