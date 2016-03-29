using System;
using System.Web.UI.WebControls;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using System.Text;
using SAHL.Web.AJAX;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using System.Collections;


namespace SAHL.Web.Views.Common
{
    public partial class BankingDetails : SAHLCommonBaseView, IBankingDetails
    {
        #region Private Variables

        private string _branchCode = "";
        private string _bankName = "";
        private string _branchName;
        //  private string _accountType;
        private string _accountNumber;
        private string _accountName;
        private bool _controlsVisible;
        private bool _showButtons = true;
        private bool _showStatus = true;
        private string _submitButtonText = "";
        private bool _searchButtonVisible;
        private bool _setControlsToFirstAccount;
        private string _bankKey;
        private string _branchKey = "";
        private int _accountTypeKey;
        private IBankAccount _selectedBankAccount;
        private List<IBankAccount> _lstAccounts;
        private List<GridRowInfo> _lstGridItems;
        private bool _hideGridStatusColumn;
        private List<SelectedAccountInfo> _lstInfo = new List<SelectedAccountInfo>();
        private ILegalEntity _legalEntity;
        private bool _accountTypeBondOnly;
        private int _generalStatusKey;

        private string _bankValue;
        private string _branchCodeValue = "-1";
        private string _accountNumberValue;
        private string _accountTypeValue;
        private string _accountNameValue;
        private int _selectedLegalEntityBankAccountKey;
        private bool _isUpdate;
        private bool _setControlsToSearchValues;
        private bool _showReferenceRow;

        #endregion

        public bool IsUpdate
        {
            set
            {
                _isUpdate = value;
            }
        }

        /// <summary>
        /// GETS or SETS the ddlBank index for posting back
        /// </summary>
        public int GETSETddlBank
        {
            get { return ddlBank.SelectedIndex; }
            set { ddlBank.SelectedIndex = value; }
        }


        /// <summary>
        /// GETS or SETS the txtBranch index for posting back
        /// </summary>
        public string GETSETtxtBranch
        {
            get { return txtBranch.Text; }
            set
            {
                txtBranch.Text = value;
                _branchCodeValue = value;
            }
        }

        /// <summary>
        /// GETS or SETS the ddlAccountType index for posting back
        /// </summary>
        public int GETSETddlAccountType
        {
            get { return ddlAccountType.SelectedIndex; }
            set { ddlAccountType.SelectedIndex = value; }
        }

        /// <summary>
        /// GETS or SETS the ddlStatus index for posting back
        /// </summary>
        public int GETSETddlStatus
        {
            get { return ddlStatus.SelectedIndex; }
            set { ddlStatus.SelectedIndex = value; }
        }

        /// <summary>
        /// GETS or SETS the txtAccountNumber index for posting back
        /// </summary>
        public string GETSETtxtAccountNumber
        {
            get { return txtAccountNumber.Text; }
            set
            {
                txtAccountNumber.Text = value;
                _accountNumberValue = value;
            }
        }

        /// <summary>
        /// GETS or SETS the txtAccountName index for posting back
        /// </summary>
        public string GETSETtxtAccountName
        {
            get { return txtAccountName.Text; }
            set
            {
                txtAccountName.Text = value;
                _accountNameValue = value;
            }
        }


        /// <summary>
        /// GETS or SETS the txtReference index for posting back
        /// </summary>
        public string GETSETtxtReference
        {
            set { txtReference.Text = value; }
            get { return txtReference.Text; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!ShouldRunPage) return;


        }

        protected void BankDetailsGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!ShouldRunPage) return;



            RegisterWebService(ServiceConstants.Bank);
            if (BankDetailsGrid.Rows.Count > 0 && BankDetailsGrid.SelectedIndex > -1 && _setControlsToFirstAccount)
            {
                if (!Page.IsPostBack)
                {
                    txtAccountNumber.Text = BankDetailsGrid.Rows[BankDetailsGrid.SelectedIndex].Cells[4].Text;
                    txtAccountName.Text = BankDetailsGrid.Rows[BankDetailsGrid.SelectedIndex].Cells[5].Text;
                }

            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!ShouldRunPage) return;

            tblControls.Visible = _controlsVisible;
            ddlStatus.Visible = _showStatus;
            lblStatus.Visible = _showStatus;
            CancelButton.Visible = _showButtons;
            SubmitButton.Visible = _showButtons;
            SearchButton.Visible = _searchButtonVisible;
            SubmitButton.Text = _submitButtonText;
            ReferenceRow.Visible = _showReferenceRow;
            ddlAccountType.Visible = true;
            lblAccountTypeBondOnly.Visible = false;

            if (_isUpdate)
            {
                ddlBank.Visible = false;
                lblBank.Visible = true;
                //
                BranchDynamic.Visible = false;
                BranchStatic.Visible = true;
                //
                ddlAccountType.Visible = true;
                lblAccountType.Visible = false;
                //
                txtAccountNumber.Visible = false;
                lblAccountNumber.Visible = true;
            }


            if (BankDetailsGrid.Rows.Count > 0)
            {
                if (BankDetailsGrid.SelectedIndex > 0)
                {
                    for (int x = 0; x < ddlBank.Items.Count; x++)
                    {
                        if (ddlBank.Items[x].Text == BankDetailsGrid.Rows[BankDetailsGrid.SelectedIndex].Cells[1].Text)
                        {
                            if (_isUpdate)
                                lblBank.Text = BankDetailsGrid.Rows[BankDetailsGrid.SelectedIndex].Cells[1].Text;
                            else
                                ddlBank.SelectedIndex = x;
                        }
                    }
                }
                else if (_setControlsToFirstAccount)
                {
                    for (int x = 0; x < ddlBank.Items.Count; x++)
                    {
                        if (ddlBank.Items[x].Text == BankDetailsGrid.Rows[0].Cells[1].Text)
                        {
                            if (_isUpdate)
                            {
                                lblBank.Text = BankDetailsGrid.Rows[0].Cells[1].Text;
                                break;
                            }
                            else
                            {
                                ddlBank.SelectedIndex = x;
                                break;
                            }
                        }
                    }
                }
            }

            if (!IsPostBack)
            {
                if (_setControlsToFirstAccount == false)
                {
                    ddlBank.SelectedIndex = -1;
                    txtAccountName.Text = "";
                    txtAccountNumber.Text = "";
                    ddlAccountType.SelectedIndex = -1;
                }
                else
                {
                    if (_lstAccounts != null && _lstAccounts.Count > 0)
                    {
                        if (_isUpdate)
                            lblAccountNumber.Text = _lstAccounts[0].AccountNumber;
                        else
                            txtAccountNumber.Text = _lstAccounts[0].AccountNumber;

                        if (_lstAccounts[0].AccountName != null)
                        {
                            txtAccountName.Text = _lstAccounts[0].AccountName;
                        }
                        for (int x = 0; x < ddlAccountType.Items.Count; x++)
                        {
                            if (ddlAccountType.Items[x].Text == _lstAccounts[0].ACBType.ACBTypeDescription)
                            {
                                //if (_isUpdate)
                                //{
                                //    //lblAccountType.Text = _lstAccounts[0].ACBType.ACBTypeDescription;
                                //    ddlAccountType.SelectedIndex = x;
                                //    break;
                                //}
                                //else
                                //{
                                ddlAccountType.SelectedIndex = x;
                                break;
                                //}
                            }
                        }
                        if (_isUpdate)
                        {
                            lblBranch.Text = _lstAccounts[0].ACBBranch.Key + " - " + _lstAccounts[0].ACBBranch.ACBBranchDescription;
                        }
                        else
                        {
                            txtBranch.Text = _lstAccounts[0].ACBBranch.Key + " - " + _lstAccounts[0].ACBBranch.ACBBranchDescription;
                            ddlBranch.SelectedValue = _lstAccounts[0].ACBBranch.Key;
                        }

                        for (int x = 0; x < ddlStatus.Items.Count; x++)
                        {
                            if (ddlStatus.Items[x].Text == _lstGridItems[0].Status)
                            {
                                ddlStatus.SelectedIndex = x;
                            }
                        }
                    }
                }
            }

            if (_setControlsToSearchValues)
            {
                for (int x = 0; x < ddlBank.Items.Count; x++)
                {
                    if (ddlBank.Items[x].Value == _bankValue)
                    {
                        ddlBank.SelectedIndex = x;
                        break;
                    }
                }
                txtAccountNumber.Text = _accountNumberValue;
                txtBranch.Text = _branchCodeValue;
                txtAccountName.Text = _accountNameValue;

                for (int x = 0; x < ddlAccountType.Items.Count; x++)
                {
                    if (ddlAccountType.Items[x].Value == _accountTypeValue)
                    {
                        ddlAccountType.SelectedIndex = x;
                        break;
                    }
                }

            }

            //if (_accountTypeBondOnly)
            //{
            //    ddlAccountType.Visible = false;
            //    lblAccountTypeBondOnly.Visible = true;
            //}



        }

        protected override void OnPreRenderComplete(EventArgs e)
        {
            base.OnPreRenderComplete(e);
            RegisterClientScripts();
            ddlBank.Attributes["onChange"] = "ClearBranchText();" + ddlBank.Attributes["onChange"];


        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CancelButton_Click(object sender, EventArgs e)
        {
            OnCancelClicked(sender, e);
        }

        private void SetGetVariables()
        {
            if (BankDetailsGrid.Rows.Count > 0 && BankDetailsGrid.SelectedIndex > -1)
            {
                if (BankDetailsGrid.Rows[BankDetailsGrid.SelectedIndex].Cells[0].Text.Length > 0)
                {
                    _selectedLegalEntityBankAccountKey = int.Parse(BankDetailsGrid.Rows[BankDetailsGrid.SelectedIndex].Cells[0].Text);
                }


                for (int x = 0; x < _lstAccounts.Count; x++)
                {
                    if (_lstAccounts[x].Key == int.Parse(BankDetailsGrid.Rows[BankDetailsGrid.SelectedIndex].Cells[0].Text))
                    {
                        _selectedBankAccount = _lstAccounts[x];

                        break;
                    }
                }
            }
            else
            {
                _selectedLegalEntityBankAccountKey = -1;
            }

            if (ddlBank.Visible)
            {
                _bankKey = ddlBank.SelectedValue;
                string branch = Request.Form[txtBranch.UniqueID];
                if (!string.IsNullOrEmpty(branch))
                {
                    if (branch.IndexOf('-') != -1)
                    {
                        _branchKey = branch.Substring(0, branch.IndexOf('-') - 1);
                        _branchCode = branch;
                    }
                    else
                    {
                        IBankAccountRepository BAR = RepositoryFactory.GetRepository<IBankAccountRepository>();

                        int key = int.Parse(branch);
                        if (key != -1 && key != 0)
                        {
                            _branchKey = branch;
                            IACBBranch b = BAR.GetACBBranchByKey(_branchKey);
                            if (b != null)
                            {
                                _bankKey = b.ACBBank.Key.ToString();
                                _branchCode = b.Key;
                            }
                        }
                        else
                        {
                            _branchName = branch;
                        }
                    }
                }
                if (_accountTypeBondOnly)
                {
                    //ILookupRepository LR = RepositoryFactory.GetRepository<ILookupRepository>();
                    //foreach (IACBType bat in LR.BankAccountTypes)
                    //{
                    //    if (bat.Key == (int)SAHL.Common.Globals.ACBTypes.Bond)
                    //    {
                    //        _accountTypeKey = bat.Key;
                    //        break;
                    //    }
                    //}
                }
                else
                {

                    if (ddlAccountType.SelectedValue == "-select-")
                        _accountTypeKey = -1;
                    else
                    {
                        int res;
                        if (int.TryParse(ddlAccountType.SelectedValue, out res))
                        {
                            _accountTypeKey = res;
                        }
                    }
                }

                if (_showStatus)
                {
                    int gsKey;
                    if (ddlStatus.SelectedIndex > -1 && ddlStatus.Items.Count > ddlStatus.SelectedIndex && int.TryParse(ddlStatus.Items[ddlStatus.SelectedIndex].Value, out gsKey))
                    {
                        _generalStatusKey = gsKey;
                    }
                }
                _accountName = Request.Form[txtAccountName.UniqueID];
                _accountNumber = Request.Form[txtAccountNumber.UniqueID];

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            SetGetVariables();
            OnSubmitButtonClicked(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SearchButton_Click(object sender, EventArgs e)
        {
            SetGetVariables();
            OnSearchBankAccountClicked(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        /// <summary>
        /// 
        /// </summary>
        private void RegisterClientScripts()
        {
            StringBuilder mBuilder = new StringBuilder();

            if (BankDetailsGrid.Rows.Count > 0 && _setControlsToFirstAccount)
            {
                mBuilder.AppendLine("var _branches;");
                mBuilder.AppendLine("var accountType = '" + _lstInfo[0].AccountType + "';");
                mBuilder.AppendLine("var accountStatus = '" + _lstInfo[0].Status + "';");
                mBuilder.AppendLine("if (window.addEventListener)");
                mBuilder.AppendLine("window.addEventListener('load', Startup, false);");
                mBuilder.AppendLine("else if (window.attachEvent)");
                mBuilder.AppendLine("window.attachEvent('onload', Startup);");

                mBuilder.AppendLine("function Startup()");
                mBuilder.AppendLine("{");
                if (_showStatus)
                {
                    mBuilder.AppendLine("var cbxAccountStatus = document.getElementById('" + ddlStatus.ClientID + "');");
                    mBuilder.AppendLine("for(var g=0;g<cbxAccountStatus.options.length;g++)");
                    mBuilder.AppendLine("{");
                    mBuilder.AppendLine("if(cbxAccountStatus.options[g].text == accountStatus)");
                    mBuilder.AppendLine("{");
                    mBuilder.AppendLine("cbxAccountStatus.selectedIndex = g;");
                    mBuilder.AppendLine("break;");
                    mBuilder.AppendLine("}");
                    mBuilder.AppendLine("}");
                }
                mBuilder.AppendLine("}");
            }
            mBuilder.AppendLine("function ClearBranchText()");
            mBuilder.AppendLine("{");
            mBuilder.AppendLine("var cbxBranch = document.getElementById('" + txtBranch.ClientID + "');");
            mBuilder.AppendLine("cbxBranch.value = '';");
            mBuilder.AppendLine("}");


            if (!Page.ClientScript.IsClientScriptBlockRegistered(GetType(), "lstScripts"))
            {
                Page.ClientScript.RegisterClientScriptBlock(GetType(), "lstScripts", mBuilder.ToString(), true);
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BankDetailsGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (OnBankDetailsGrid_SelectedIndexChanged != null)
            {
                OnBankDetailsGrid_SelectedIndexChanged(sender, e);
            };

        }

        /// <summary>
        /// Gets or Sets the Banks details Grid index
        /// </summary>
        public int BankDetailsGridIndex
        {
            set { BankDetailsGrid.SelectedIndex = value; }
            get { return BankDetailsGrid.SelectedIndex; }
        }

        /// <summary>
        /// 
        /// </summary>
        public void BindFromGrid(int BankGridKey)
        {
            if (BankDetailsGrid.SelectedIndex > -1)
            {
                ILookupRepository lr = RepositoryFactory.GetRepository<ILookupRepository>();

                if (_isUpdate)
                    lblAccountNumber.Text = _lstGridItems[BankGridKey].AccountNumber;
                else
                    txtAccountNumber.Text = _lstGridItems[BankGridKey].AccountNumber;


                //if (_isUpdate)
                //{
                //    lblAccountType.Text = _lstGridItems[BankGridKey].AccountType;
                //}
                //else
                //{
                for (int x = 0; x < ddlAccountType.Items.Count; x++)
                {
                    if (ddlAccountType.Items[x].Text == _lstGridItems[BankGridKey].AccountType)
                    {
                        ddlAccountType.SelectedIndex = x;
                        break;
                    }
                }
                //}
                //txtBranch.Text = _branchCodeValue;
                for (int x = 0; x < lr.BankBranches.Count; x++)
                {
                    if (lr.BankBranches[x].Key == _lstGridItems[BankGridKey].BranchKey)
                    {
                        IACBBranch b = lr.BankBranches[x];
                        txtBranch.Text = b.Key + " - " + b.ACBBranchDescription;
                        lblBranch.Text = b.Key + " - " + b.ACBBranchDescription;
                        break;
                    }
                }

                if (Messages.Count == 0)
                {
                    if (_showStatus)
                    {
                        for (int x = 0; x < ddlStatus.Items.Count; x++)
                        {
                            if (ddlStatus.Items[x].Text == _lstGridItems[BankGridKey].Status)
                            {
                                ddlStatus.SelectedIndex = x;
                                break;
                            }

                        }
                    }
                    txtAccountName.Text = _lstGridItems[BankGridKey].AccountName;
                }


            }
        }

        #region IBankingDetails Members

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnSubmitButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnBankDetailsGrid_SelectedIndexChanged;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ApplicationExpenses"></param>
        public void BindGridForBankAccounts(List<IApplicationExpense> ApplicationExpenses)
        {
            _lstAccounts = new List<IBankAccount>();
            _lstGridItems = new List<GridRowInfo>();
            for (int x = 0; x < ApplicationExpenses.Count; x++)
            {
                _lstAccounts.Add(ApplicationExpenses[x].ApplicationDebtSettlements[0].BankAccount);
                GridRowInfo gridItem = new GridRowInfo();
                SelectedAccountInfo infoItem = new SelectedAccountInfo();
                gridItem.AccountType = ApplicationExpenses[x].ApplicationDebtSettlements[0].BankAccount.ACBType.ACBTypeDescription;
                gridItem.AccountName = ApplicationExpenses[x].ApplicationDebtSettlements[0].BankAccount.AccountName;
                gridItem.AccountNumber = ApplicationExpenses[x].ApplicationDebtSettlements[0].BankAccount.AccountNumber;
                gridItem.AccountType = ApplicationExpenses[x].ApplicationDebtSettlements[0].BankAccount.ACBType.ACBTypeDescription;
                gridItem.Reference = ApplicationExpenses[x].ExpenseReference != null ? ApplicationExpenses[x].ExpenseReference : "";
                gridItem.Bank = ApplicationExpenses[x].ApplicationDebtSettlements[0].BankAccount.ACBBranch.ACBBank.ACBBankDescription;
                gridItem.Branch = ApplicationExpenses[x].ApplicationDebtSettlements[0].BankAccount.ACBBranch.Key + " - " + ApplicationExpenses[x].ApplicationDebtSettlements[0].BankAccount.ACBBranch.ACBBranchDescription;
                gridItem.BranchKey = ApplicationExpenses[x].ApplicationDebtSettlements[0].BankAccount.ACBBranch.Key;
                gridItem.Key = ApplicationExpenses[x].ApplicationDebtSettlements[0].BankAccount.Key.ToString();
                if (_legalEntity != null)
                {
                    for (int z = 0; z < _legalEntity.LegalEntityBankAccounts.Count; z++)
                    {
                        if (_legalEntity.LegalEntityBankAccounts[z].BankAccount.Key == ApplicationExpenses[x].Key)
                        {
                            gridItem.Status = _legalEntity.LegalEntityBankAccounts[x].GeneralStatus.Description;
                            infoItem.Status = _legalEntity.LegalEntityBankAccounts[x].GeneralStatus.Description;

                            break;
                        }
                    }
                }
                _lstGridItems.Add(gridItem);

                infoItem.AccountType = ApplicationExpenses[x].ApplicationDebtSettlements[0].BankAccount.ACBType.ACBTypeDescription;
                infoItem.BranchCode = ApplicationExpenses[x].ApplicationDebtSettlements[0].BankAccount.ACBBranch.Key;
                _lstInfo.Add(infoItem);
            }
            BankDetailsGrid.AutoGenerateColumns = false;
            BankDetailsGrid.AddGridBoundColumn("Key", "", Unit.Percentage(0), HorizontalAlign.Left, false);
            BankDetailsGrid.AddGridBoundColumn("Bank", "Bank", Unit.Percentage(18), HorizontalAlign.Left, true);
            BankDetailsGrid.AddGridBoundColumn("Branch", "Branch", Unit.Percentage(23), HorizontalAlign.Left, true);
            BankDetailsGrid.AddGridBoundColumn("AccountType", "Account Type", Unit.Percentage(13), HorizontalAlign.Left, true);
            BankDetailsGrid.AddGridBoundColumn("AccountNumber", "Account Number", Unit.Percentage(13), HorizontalAlign.Left, true);
            BankDetailsGrid.AddGridBoundColumn("AccountName", "Account Name", Unit.Percentage(23), HorizontalAlign.Left, true);
            BankDetailsGrid.AddGridBoundColumn("Reference", "Reference", Unit.Percentage(23), HorizontalAlign.Left, true);
            BankDetailsGrid.AddGridBoundColumn("BranchKey", "", Unit.Percentage(0), HorizontalAlign.Left, false);
            if (_hideGridStatusColumn == false)
            {
                BankDetailsGrid.AddGridBoundColumn("Status", "Status", Unit.Percentage(10), HorizontalAlign.Left, true);
            }
            BankDetailsGrid.DataSource = _lstGridItems;
            BankDetailsGrid.DataBind();

            if (ApplicationExpenses.Count > 0)
            {
                BankDetailsGrid.SelectFirstRow = true;
            }
            else
            {
                BankDetailsGrid.SelectFirstRow = false;
                BankDetailsGrid.PostBackType = GridPostBackType.None;
                //BankDetailsGrid.Enabled = false;

            }

        }

        public void BindGridForLegalEntityBankAccounts(List<ILegalEntityBankAccount> bankAccounts)
        {


            _lstAccounts = new List<IBankAccount>();
            _lstGridItems = new List<GridRowInfo>();
            for (int x = 0; x < bankAccounts.Count; x++)
            {
                _lstAccounts.Add(bankAccounts[x].BankAccount);
                GridRowInfo gridItem = new GridRowInfo();
                SelectedAccountInfo infoItem = new SelectedAccountInfo();
                gridItem.AccountType = bankAccounts[x].BankAccount.ACBType.ACBTypeDescription;
                gridItem.AccountName = bankAccounts[x].BankAccount.AccountName;
                gridItem.AccountNumber = bankAccounts[x].BankAccount.AccountNumber;
                gridItem.AccountType = bankAccounts[x].BankAccount.ACBType.ACBTypeDescription;
                gridItem.Bank = bankAccounts[x].BankAccount.ACBBranch.ACBBank.ACBBankDescription;
                gridItem.Branch = bankAccounts[x].BankAccount.ACBBranch.Key + " - " + bankAccounts[x].BankAccount.ACBBranch.ACBBranchDescription;
                gridItem.Key = bankAccounts[x].Key.ToString();
                gridItem.Status = bankAccounts[x].GeneralStatus.Description;
                gridItem.BranchKey = bankAccounts[x].BankAccount.ACBBranch.Key;
                if (_legalEntity != null)
                {
                    for (int z = 0; z < _legalEntity.LegalEntityBankAccounts.Count; z++)
                    {
                        if (_legalEntity.LegalEntityBankAccounts[z].BankAccount.Key == bankAccounts[x].Key)
                        {
                            gridItem.Status = _legalEntity.LegalEntityBankAccounts[x].GeneralStatus.Description;
                            infoItem.Status = _legalEntity.LegalEntityBankAccounts[x].GeneralStatus.Description;

                            break;
                        }
                    }
                }
                _lstGridItems.Add(gridItem);

                infoItem.AccountType = bankAccounts[x].BankAccount.ACBType.ACBTypeDescription;
                infoItem.BranchCode = bankAccounts[x].BankAccount.ACBBranch.Key;
                _lstInfo.Add(infoItem);
            }
            BankDetailsGrid.AutoGenerateColumns = false;
            BankDetailsGrid.AddGridBoundColumn("Key", "", Unit.Percentage(0), HorizontalAlign.Left, false);
            BankDetailsGrid.AddGridBoundColumn("Bank", "Bank", Unit.Percentage(18), HorizontalAlign.Left, true);
            BankDetailsGrid.AddGridBoundColumn("Branch", "Branch", Unit.Percentage(23), HorizontalAlign.Left, true);
            BankDetailsGrid.AddGridBoundColumn("AccountType", "Account Type", Unit.Percentage(13), HorizontalAlign.Left, true);
            BankDetailsGrid.AddGridBoundColumn("AccountNumber", "Account Number", Unit.Percentage(13), HorizontalAlign.Left, true);
            BankDetailsGrid.AddGridBoundColumn("AccountName", "Account Name", Unit.Percentage(23), HorizontalAlign.Left, true);
            BankDetailsGrid.AddGridBoundColumn("BranchKey", "", Unit.Percentage(0), HorizontalAlign.Left, false);
            if (_hideGridStatusColumn == false)
            {
                BankDetailsGrid.AddGridBoundColumn("Status", "Status", Unit.Percentage(10), HorizontalAlign.Left, true);
            }
            BankDetailsGrid.DataSource = _lstGridItems;
            BankDetailsGrid.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="banks"></param>
        /// <param name="bankAccountTypes"></param>
        /// <param name="generalStatuses"></param>
        public void BindBankAccountControls(IEventList<IACBBank> banks, IEventList<IACBType> bankAccountTypes, ICollection<IGeneralStatus> generalStatuses)
        {
            ddlBank.DataSource = banks;
            ddlBank.DataValueField = "Key";
            ddlBank.DataTextField = "ACBBankDescription";
            ddlBank.DataBind();

            ddlAccountType.DataSource = bankAccountTypes;
            ddlAccountType.DataValueField = "Key";
            ddlAccountType.DataTextField = "ACBTypeDescription";
            ddlAccountType.DataBind();

            //Don't allow account type of unknown
            //NB! this must be removed from the dropdown list, not the Lookup
            //If it is removed from the bankAccountTypes lookup this will affect the entire application
            for (int i = 0; i < ddlAccountType.Items.Count; i++)
            {
                if (ddlAccountType.Items[i].Text == "Unknown")
                    ddlAccountType.Items.Remove(ddlAccountType.Items[i]);
            }

            ddlStatus.DataSource = generalStatuses;
            ddlStatus.DataValueField = "Key";
            ddlStatus.DataTextField = "Description";
            ddlStatus.DataBind();

        }

        /// <summary>
        /// 
        /// </summary>
        public bool ShowReferenceRow
        {
            set
            {
                _showReferenceRow = value;
            }
        }

        /// <summary>
        /// implements <see cref = "IBankingDetails.BankName"/>
        /// </summary>
        public string BankName
        {
            get { return _bankName; }
            set { _bankName = value; }
        }

        /// <summary>
        /// implements <see cref = "IBankingDetails.BranchCode"/>
        /// </summary>
        public string BranchCode
        {
            get { return _branchCode; }
        }

        /// <summary>
        /// implements <see cref = "IBankingDetails.BranchName"/>
        /// </summary>
        public string BranchName
        {
            get { return _branchName; }
        }

        /// <summary>
        /// implements <see cref = "IBankingDetails.AccountType"/>
        /// </summary>
        public string AccountType
        {
            get { return _accountTypeKey.ToString(); }
        }

        /// <summary>
        /// implements <see cref = "IBankingDetails.AccountNumber"/>
        /// </summary>
        public string AccountNumber
        {
            get { return _accountNumber; }
        }

        /// <summary>
        /// implements <see cref = "IBankingDetails.AccountName"/>
        /// </summary>
        public string AccountName
        {
            get { return _accountName; }
        }


        /// <summary>
        /// implements <see cref = "IBankingDetails.ControlsVisible"/>
        /// </summary>
        public bool ControlsVisible
        {
            set { _controlsVisible = value; }
        }

        /// <summary>
        /// implements <see cref = "IBankingDetails.ShowButtons"/>
        /// </summary>
        public bool ShowButtons
        {
            set { _showButtons = value; }
        }

        /// <summary>
        /// implements <see cref = "IBankingDetails.ShowStatus"/>
        /// </summary>
        public bool ShowStatus
        {
            set { _showStatus = value; }
        }

        /// <summary>
        /// implements <see cref = "IBankingDetails.SubmitButtonText"/>
        /// </summary>
        public string SubmitButtonText
        {
            set { _submitButtonText = value; }
        }

        /// <summary>
        /// implements <see cref = "IBankingDetails.SearchButtonVisible"/>
        /// </summary>
        public bool SearchButtonVisible
        {
            set { _searchButtonVisible = value; }
        }

        /// <summary>
        /// implements <see cref = "IBankingDetails.BankAccountGridEnabled"/>
        /// </summary>
        public bool BankAccountGridEnabled
        {
            set
            {
                if (value)
                {
                    BankDetailsGrid.PostBackType = GridPostBackType.SingleClick;
                }
            }
        }

        /// <summary>
        /// implements <see cref = "IBankingDetails.SetControlsToFirstAccount"/>
        /// </summary>
        public bool SetControlsToFirstAccount
        {
            set { _setControlsToFirstAccount = value; }
        }

        /// <summary>
        /// implements <see cref = "IBankingDetails.BankKey"/>
        /// </summary>
        public string BankKey
        {
            get { return _bankKey; }
        }

        /// <summary>
        /// implements <see cref = "IBankingDetails.BranchKey"/>
        /// </summary>
        public string BranchKey
        {
            get { return _branchKey; }
        }

        /// <summary>
        /// implements <see cref = "IBankingDetails.AccountTypeKey"/>
        /// </summary>
        public int AccountTypeKey
        {
            get { return _accountTypeKey; }
        }

        /// <summary>
        /// implements <see cref = "IBankingDetails.OnSearchBankAccountClicked"/>
        /// </summary>
        public event EventHandler OnSearchBankAccountClicked;

        /// <summary>
        /// implements <see cref = "IBankingDetails.OnCancelClicked"/>
        /// </summary>
        public event EventHandler OnCancelClicked;



        public bool HideGridStatusColumn
        {
            set { _hideGridStatusColumn = value; }
        }

        public ILegalEntity LegalEntity
        {
            set { _legalEntity = value; }
        }

        public bool AccountTypeBondOnly
        {
            set { _accountTypeBondOnly = value; }
        }

        public string BankValue
        {
            set { _bankValue = value; }
            get { return _bankValue; }
        }

        public string BranchCodeValue
        {
            set { _branchCodeValue = value; }
            get { return _branchCodeValue; }
        }

        public string AccountTypeValue
        {
            set
            {
                _accountTypeValue = value;
                ddlAccountType.SelectedIndex = Convert.ToInt32(value);
            }

            get
            {
                return Convert.ToString(Request.Form[ddlAccountType.UniqueID]);
                //return ddlAccountType.SelectedValue;
            }
        }

        public string AccountNumberValue
        {
            set { _accountNumberValue = value; }
            get { return _accountNumberValue; }
        }

        public string AccountNameValue
        {
            set { _accountNameValue = value; }
            get { return _accountNameValue; }
        }


        public bool SetControlsToSearchValues
        {
            set { _setControlsToSearchValues = value; }
        }


        public IBankAccount SelectedBankAccount
        {
            get { return _selectedBankAccount; }
        }


        public int SelectedLegalEntityBankAccountKey
        {
            get { return _selectedLegalEntityBankAccountKey; }
        }

        public int GeneralStatusKey
        {
            get { return _generalStatusKey; }
        }

        public bool SubmitButtonEnabled
        {
            set { SubmitButton.Enabled = value; }
        }

        #endregion
    }

    public class GridRowInfo
    {
        private string _key;
        private string _bank;
        private string _branch;
        private string _accountType;
        private string _accountNumber;
        private string _accountName;
        private string _status;
        private string _branchKey;
        private string _reference;

        public string Reference
        {
            get { return _reference; }
            set { _reference = value; }
        }

        public string Key
        {
            get
            {
                return _key;
            }
            set
            {
                _key = value;
            }
        }

        public string Bank
        {
            get
            {
                return _bank;
            }
            set
            {
                _bank = value;
            }
        }

        public string Branch
        {
            get
            {
                return _branch;
            }
            set
            {
                _branch = value;
            }
        }

        public string AccountType
        {
            get
            {
                return _accountType;
            }
            set
            {
                _accountType = value;
            }
        }

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

        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
            }
        }
        public string BranchKey
        {
            get
            {
                return _branchKey;
            }
            set
            {
                _branchKey = value;
            }
        }
    }

    /// <summary>
    /// Custom class for selected account information
    /// </summary>
    public class SelectedAccountInfo
    {
        private string _branchCode;
        private string _accountType;
        private string _status;

        /// <summary>
        /// 
        /// </summary>
        public string BranchCode
        {
            get
            {
                return _branchCode;
            }
            set
            {
                _branchCode = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string AccountType
        {
            get
            {
                return _accountType;
            }
            set
            {
                _accountType = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
            }
        }



    }
}
