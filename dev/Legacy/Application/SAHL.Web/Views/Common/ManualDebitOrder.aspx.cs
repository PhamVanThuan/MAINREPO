using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Microsoft.ApplicationBlocks.UIProcess;

using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Authentication;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.BusinessModel.Interfaces;
using System.Text;
using SAHL.Common.Collections;
using System.Drawing;
using System.Collections.Generic;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel;

namespace SAHL.Web.Views.Common
{
    public partial class ManualDebitOrder : SAHLCommonBaseView, IManualDebitOrder
    {
        #region Private Variables

        private bool _setEffectiveDateToCurrentDate;
        private bool _setControlsToFirstGridItem;

        private bool _showButtons;
        private bool _arrearBalanceRowVisible;  
        private GridPostBackType _gridPostBackType;
        private string _submitButtonText;
        private string _submitButtonAccessKey;
        private bool _displayControls;
        private bool _showLabels;
        private int _selectedBankAccountKey;
        private double _arrearBalance;
        private DateTime _effectiveDate;
        private double _amount;
        private string _reference;
        private string _note;

        private IEventList<IFinancialServiceRecurringTransaction> _lstRecurringTransactions;

        private IEventList<ILegalEntityBankAccount> _legalEntityBankAccounts;
        

        #endregion


        

        #region Protected Functions Section
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            
          

            BindOrdersToGrid(_lstRecurringTransactions);

            if (!IsPostBack && RecordsGrid.Rows.Count > 0 && RecordsGrid.SelectedIndex > -1)
            {
                BindControls();
            }
            BindBankAccountControl();

          
                                
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        protected void RecordsGrid_SelectedIndexChanged(Object s, EventArgs e)
        {

                 BindControls();
 
        
        }

        protected void Page_Load(object sender, EventArgs e)
        {
         }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            OnCancelButtonClicked(sender, e);          
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitButton_Click(object sender, EventArgs e)        
        {
            if (_displayControls)
            {
                _selectedBankAccountKey = int.Parse(ddlBankUpdate.SelectedValue);


                _effectiveDate = dateEffectiveDateUpdate.Date;
                _amount = double.Parse(txtAmountUpdate.Text.Substring(1));
                _reference = txtReferenceUpdate.Text;
                _note = txtNoteUpdate.Text;
            }

            KeyChangedEventArgs args = null;
            if (RecordsGrid.SelectedIndex > -1)
            {
                args = new KeyChangedEventArgs(RecordsGrid.Rows[RecordsGrid.SelectedIndex].Cells[0].Text);
            }
            OnSubmitButtonClicked(sender, args);
                     
        }

        protected void RecordsGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                IFinancialServiceRecurringTransaction rt = e.Row.DataItem as IFinancialServiceRecurringTransaction;
                if (rt != null)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(rt.BankAccount.ACBBranch.ACBBank.ACBBankDescription);
                    sb.Append(" - " + rt.BankAccount.ACBBranch.Key.ToString());
                    sb.Append(" " + rt.BankAccount.ACBBranch.ACBBranchDescription); 
                    sb.Append(" - " + rt.BankAccount.ACBType.ACBTypeDescription);
                    e.Row.Cells[5].Text = sb.ToString();

                    e.Row.Cells[1].Text = rt.BankAccount.Key.ToString();

                    if (rt.TransactionType != null)
                    {
                        if (rt.TransactionType.Key == (int)SAHL.Common.Globals.TransactionTypes.MonthlyServiceFee)
                        {
                            e.Row.Cells[7].Text = "Service Fee";
                        }
                        else
                        {
                            e.Row.Cells[7].Text = "Debit Order";
                        }
                    }
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
            CancelButton.Visible = _showButtons;
            SubmitButton.Visible = _showButtons;    

            ArrearBalanceRow.Visible = _arrearBalanceRowVisible;


            dateEffectiveDateUpdate.Visible = !_showLabels;
            lblEffectiveDate.Visible = _showLabels;

            txtNoteUpdate.Visible = !_showLabels;
            NotePanel.Visible = _showLabels;

            txtReferenceUpdate.Visible = !_showLabels;
            lblReference.Visible = _showLabels;


            txtAmountUpdate.Visible = !_showLabels;
            lblAmount.Visible = _showLabels;

            ddlBankUpdate.Visible = !_showLabels;
            lblBank.Visible = _showLabels;

            DisplayData.Visible = _displayControls;

            SubmitButton.Text = _submitButtonText;
            SubmitButton.AccessKey = _submitButtonAccessKey;

            

            if (_setEffectiveDateToCurrentDate)
            {
                DateTime current = DateTime.Now;
                if (current.TimeOfDay.Hours < 15)
                {
                    dateEffectiveDateUpdate.Date = DateTime.Today;
                }
                else
                {                    
                    dateEffectiveDateUpdate.Date = DateTime.Today.Add(new TimeSpan(24,0,0));
                }
            }

            if (_setControlsToFirstGridItem && RecordsGrid.Rows.Count > 0)
            {
                for (int x = 0; x < ddlBankUpdate.Items.Count; x++)
                {
                    if (ddlBankUpdate.Items[x].Value.ToString() == RecordsGrid.Rows[RecordsGrid.SelectedIndex].Cells[1].Text)
                    {
                        ddlBankUpdate.SelectedIndex = x;
                        break;
                    }
                }
            }

            if (RecordsGrid.Rows[RecordsGrid.SelectedIndex].Cells[7].Text == "Service Fee")
            {               
                if (SubmitButton.Text == "Update")
                {
                    SubmitButton.Enabled = false;
                }
            }
        }

        #endregion       
    
        #region IManualDebitOrder Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recurringTransactions"></param>
        public void BindOrdersToGrid(SAHL.Common.Collections.Interfaces.IEventList<SAHL.Common.BusinessModel.Interfaces.IFinancialServiceRecurringTransaction> recurringTransactions)
        {
            _lstRecurringTransactions = recurringTransactions;

            RecordsGrid.PostBackType = _gridPostBackType;

            if (!DesignMode)
            {
                RecordsGrid.Columns.Clear();

                RecordsGrid.AddGridBoundColumn("Key", "", Unit.Percentage(0), HorizontalAlign.Left, false);
                RecordsGrid.AddGridBoundColumn("", "BankKey", Unit.Percentage(0), HorizontalAlign.Left, false);
                RecordsGrid.AddGridBoundColumn("InsertDate", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Insert Date", false, Unit.Percentage(7), HorizontalAlign.Center, true);
                RecordsGrid.AddGridBoundColumn("StartDate", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Effective Date", false, Unit.Percentage(7), HorizontalAlign.Center, true);
                RecordsGrid.AddGridBoundColumn("Amount", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Amount", false, Unit.Percentage(12), HorizontalAlign.Right, true);
                RecordsGrid.AddGridBoundColumn("", "Bank", Unit.Percentage(50), HorizontalAlign.Left, true);
                RecordsGrid.AddGridBoundColumn("UserName", "Captured By", Unit.Percentage(7), HorizontalAlign.Left, true);
                RecordsGrid.AddGridBoundColumn("","Transaction Type", Unit.Percentage(25), HorizontalAlign.Center, true);
            }

            RecordsGrid.DataSource = recurringTransactions;
            RecordsGrid.DataBind();

            
        }    

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnCancelButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        public event KeyChangedEventHandler OnSubmitButtonClicked;
     
        /// <summary>
        /// 
        /// </summary>
        public event KeyChangedEventHandler OnGridSelectedIndexChanged;
       
        /// <summary>
        /// 
        /// </summary>       
        public void BindControls()
        {
            if (RecordsGrid.Rows[RecordsGrid.SelectedIndex].Cells[7].Text == "Service Fee")
            {
                _showLabels = true;
            }

            for (int x = 0; x < _lstRecurringTransactions.Count; x++)
            {
                if (_lstRecurringTransactions[x].Key == int.Parse(RecordsGrid.SelectedRow.Cells[0].Text))
                {
                    if (!_showLabels)
                    {
                        txtNoteUpdate.Text = _lstRecurringTransactions[x].Notes;
                        if (_lstRecurringTransactions[x].StartDate != null)
                        {
                            dateEffectiveDateUpdate.Date = _lstRecurringTransactions[x].StartDate.Value;
                        }
                        txtAmountUpdate.Text = _lstRecurringTransactions[x].Amount.Value.ToString(SAHL.Common.Constants.CurrencyFormat);
                        txtReferenceUpdate.Text = _lstRecurringTransactions[x].Reference.ToString();
                        txtNoteUpdate.Text = _lstRecurringTransactions[x].Notes;
                    }
                    else
                    {
                        lblNote.Text = _lstRecurringTransactions[x].Notes;
                        lblEffectiveDate.Text = _lstRecurringTransactions[x].StartDate.Value.ToString(SAHL.Common.Constants.DateFormat);
                        lblAmount.Text = _lstRecurringTransactions[x].Amount.Value.ToString(SAHL.Common.Constants.CurrencyFormat);
                        lblReference.Text = _lstRecurringTransactions[x].Reference.ToString();

                        for (int y = 0; y < _legalEntityBankAccounts.Count; y++)
                        {
                            if (_legalEntityBankAccounts[y].BankAccount.Key == _lstRecurringTransactions[x].BankAccount.Key)
                            {
                                lblBank.Text = _legalEntityBankAccounts[y].BankAccount.GetDisplayName(BankAccountNameFormat.Full);
                            }
                        }
                    }
                }
            }
        }
     
        /// <summary>
        /// 
        /// </summary>
        public bool ShowButtons
        {
            set { _showButtons = value ; }
        }
       
        /// <summary>
        /// 
        /// </summary>
        public bool ArrearBalanceRowVisible
        {
            set { _arrearBalanceRowVisible = value; }
        }


       /// <summary>
       /// 
       /// </summary>
        public string SubmitButtonText
        {
            set { _submitButtonText = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SubmitButtonAccessKey
        {
            set { _submitButtonAccessKey = value; }
        }
       
      

      
        /// <summary>
        /// 
        /// </summary>
        public bool ControlsVisible
        {
            set { _displayControls = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public GridPostBackType GridPostbackType
        {
            set { _gridPostBackType = value; }
        }
   
        public bool ShowLabels
        {
            set { _showLabels = value; }
        }
              
        /// <summary>
        /// 
        /// </summary>
        public void BindBankAccountControl()
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();

            // StringBuilder sb;

            for (int x = 0; x < _legalEntityBankAccounts.Count; x++)
            {
                if (!dic.ContainsKey(_legalEntityBankAccounts[x].BankAccount.Key))
                {
                    dic.Add(_legalEntityBankAccounts[x].BankAccount.Key, _legalEntityBankAccounts[x].BankAccount.GetDisplayName(BankAccountNameFormat.Full));
                }
            }
            ddlBankUpdate.DataSource = dic;
            ddlBankUpdate.DataValueField = "Key";
            ddlBankUpdate.DataTextField = "Description";
            ddlBankUpdate.DataBind();
        }

       
       
        /// <summary>
        /// 
        /// </summary>
        public int SelectedBankAccountKey
        {
            get { return _selectedBankAccountKey; }
        }

        /// <summary>
        /// 
        /// </summary>
        public double ArrearBalance
        {
            get { return _arrearBalance; }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime EffectiveDate
        {
            get { return _effectiveDate; }
        }

        /// <summary>
        /// 
        /// </summary>
        public double Amount
        {
            get { return _amount; }
        }


        /// <summary>
        /// 
        /// </summary>
        public string Reference
        {
            get { return _reference; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Note
        {
            get { return _note; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public bool SetEffectiveDateToCurrentDate
        {
            set { _setEffectiveDateToCurrentDate = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool SetControlsToFirstGridItem
        {
            set { _setControlsToFirstGridItem = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public IEventList<ILegalEntityBankAccount> LegalEntityBankAccounts
        {
            set { _legalEntityBankAccounts = value ; }
        }

       

        #endregion

    }
}
