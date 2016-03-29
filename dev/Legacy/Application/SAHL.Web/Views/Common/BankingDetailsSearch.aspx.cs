using System;
using System.Web.UI.WebControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI.Events;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Web.Views.Common
{
    public partial class BankingDetailsSearch : SAHLCommonBaseView, IBankingDetailsSearch
    {

        #region Private Variables

        IBankAccount _selectedBankAccount;
        List<BankingDetailsSearchGridRowItem> _matchingBankAccounts;
        private string _branchCode  ;
        private string _bankName ;
        private string _branchName;
        //private string _accountType;
        private string _accountNumber;
        private string _accountName;
        private string _bankKey;
        //private int _branchKey;
        private int _accountTypeKey;

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!ShouldRunPage) return;

            UseButton.Enabled = BankDetailsSearchGrid.Rows.Count > 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BankDetailsSearchGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BankDetailsSearchGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnSearchGridSelectedIndexChanged(sender, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SelectButton_Click(object sender, EventArgs e)
        {

            if (BankDetailsSearchGrid.Rows.Count > 0 && BankDetailsSearchGrid.SelectedIndex > -1)
            {
                int key = int.Parse(BankDetailsSearchGrid.Rows[BankDetailsSearchGrid.SelectedIndex].Cells[1].Text);               
                for (int x = 0; x < _matchingBankAccounts.Count; x++)
                {
                    if (_matchingBankAccounts[x].BankAccount.Key == key)
                    {
                        _selectedBankAccount = _matchingBankAccounts[x].BankAccount;
                        _accountName = _matchingBankAccounts[x].BankAccount.AccountName;
                        _accountNumber = _matchingBankAccounts[x].BankAccount.AccountNumber;
                        _accountTypeKey = _matchingBankAccounts[x].BankAccount.ACBType.Key;
                        _bankKey = _matchingBankAccounts[x].BankAccount.Key.ToString();
                        _bankName = _matchingBankAccounts[x].BankAccount.ACBBranch.ACBBank.ACBBankDescription;
                        _branchCode = _matchingBankAccounts[x].BankAccount.ACBBranch.Key;
                        _branchName = _matchingBankAccounts[x].BankAccount.ACBBranch.ACBBranchDescription;
                        break;
                    }
                }
                KeyChangedEventArgs args = new KeyChangedEventArgs(key);
                OnUseButtonClicked(sender,args);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CancelButton_Click(object sender, EventArgs e)
        {
            OnCancelButtonClicked(sender, e);
        }

        #region IBankingDetailsSearch Members

        /// <summary>
        /// implements <see cref="IBankingDetailsSearch.OnSearchGridSelectedIndexChanged"/>
        /// </summary>
        public event KeyChangedEventHandler OnSearchGridSelectedIndexChanged;

        /// <summary>
        /// implements <see cref="IBankingDetailsSearch.OnCancelButtonClicked"/>
        /// </summary>
        public event EventHandler OnCancelButtonClicked;

        /// <summary>
        /// implements <see cref="IBankingDetailsSearch.OnUseButtonClicked"/>
        /// </summary>
        public event KeyChangedEventHandler OnUseButtonClicked;

        /// <summary>
        /// implements <see cref="IBankingDetailsSearch.BindSearchGrid"/>
        /// </summary>
        /// <param name="matchingBankAccounts"></param>
        public void BindSearchGrid(List<BankingDetailsSearchGridRowItem> matchingBankAccounts)
        {
            _matchingBankAccounts = matchingBankAccounts;           
            BankDetailsSearchGrid.AddGridBoundColumn("LegalEntityKey", "", Unit.Percentage(0), HorizontalAlign.Left, false);
            BankDetailsSearchGrid.AddGridBoundColumn("BankAccountKey", "", Unit.Percentage(0), HorizontalAlign.Left, false); //BankAccountKey
            BankDetailsSearchGrid.AddGridBoundColumn("LegalEntityName", "Legal Entity Name", Unit.Percentage(55), HorizontalAlign.Left, true);
            BankDetailsSearchGrid.AddGridBoundColumn("LegalEntityNumber", "ID / Company Number", Unit.Percentage(30), HorizontalAlign.Left, true);
            BankDetailsSearchGrid.AddGridBoundColumn("LegalEntityStatus", "Status", Unit.Percentage(15), HorizontalAlign.Left, true);            
            
            BankDetailsSearchGrid.DataSource = matchingBankAccounts;
            BankDetailsSearchGrid.DataBind();
        }        

        /// <summary>
        /// 
        /// </summary>
        public IBankAccount BankAcccount
        {
            get { return _selectedBankAccount; }
        }

        #endregion

        #region IBankingDetailsSearch Members


        //public string AccountType
        //{
        //    get { return _accountType; }
        //}

        public string AccountName
        {
            get { return _accountName; }
        }

        public string AccountNumber
        {
            get { return _accountNumber; }
        }

        public string BankName
        {
            get { return _bankName; }
        }

        public string BranchCode
        {
            get { return _branchCode; }
        }

        public string BranchName
        {
            get { return _branchName; }
        }

        public string BankKey
        {
            get { return _bankKey; }
        }

        //public int BranchKey
        //{
        //    get { return _branchKey; }
        //}

        public int AccountTypeKey
        {
            get { return _accountTypeKey; }
        }

      
        public IBankAccount SelectedBankAccount
        {
            get { return _selectedBankAccount; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int BankDetailsSearchGridItemIndex
        {
            set { BankDetailsSearchGrid.SelectedIndex = value; }
            get { return BankDetailsSearchGrid.SelectedIndex; }
        }

        #endregion

       
    }

  }