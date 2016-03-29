using System;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Collections.Interfaces;
using System.Web.UI.WebControls;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Repositories;
using System.Diagnostics.CodeAnalysis;
using SAHL.Common.Factories;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using System.Globalization;
using SAHL.Common.Collections;
using SAHL.Common.Globals;
using SAHL.Common.DomainMessages;

namespace SAHL.Web.Views.Common
{
    public partial class DebitOrderDetails : SAHLCommonBaseView, IDebitOrderDetails
    {
        #region Private Variables

        private bool _viewLabels;
        private bool _showControls;
        // private IEventList<IDebitOrderDetails> _lstDebitOrderDetails;
        private bool _showButtons;
        // private GridPostBackType _gridPostBackType;
        private bool _bankAccountVisible;
        private bool _setEffectiveDateToCurrentDate;
        private bool _setControlsToGrid;
        private bool _forceShowBankAccountControl;
        private bool _hideEffectiveDate;
        private string _paymentTypeKey;
        private string _bankAccountKey;
        private string _debitOrderDay;
        private DateTime? _effectiveDate;
        private int _detailKey = -1;
        private int _gridSelectedIndex = -1;
        private bool _ignore;
        Dictionary<string, int> _gridOrderDict;

        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!ShouldRunPage) return;
        }

        private void AddGridColumns(bool isApplication)
        {
            gridOrder.AddGridBoundColumn("Key", "", Unit.Percentage(0), HorizontalAlign.Left, false);
            if (!_hideEffectiveDate)
            {
                gridOrder.AddGridBoundColumn("EffectiveDate", "Effective Date", Unit.Percentage(10), HorizontalAlign.Center, true);
            }
            else
            {
                gridOrder.AddGridBoundColumn("", "", Unit.Percentage(0), HorizontalAlign.Center, false);
            }
            gridOrder.AddGridBoundColumn("BankDetails", "Bank Details", Unit.Percentage(30), HorizontalAlign.Left, true);
            gridOrder.AddGridBoundColumn("DebitOrderDay", "Debit Order Day", Unit.Percentage(10), HorizontalAlign.Center, true);
            if (!isApplication)
            {
                gridOrder.AddGridBoundColumn("UserID", "Changed By", Unit.Percentage(15), HorizontalAlign.Left, true);
                gridOrder.AddGridBoundColumn("ChangeDate", "Date Changed", Unit.Percentage(10), HorizontalAlign.Left, true);
            }
            gridOrder.AddGridBoundColumn("PaymentType", "Payment Type", Unit.Percentage(15), HorizontalAlign.Left, true);
            gridOrder.AddGridBoundColumn("PaymentTypeKey", "Payment Type Key", Unit.Percentage(0), HorizontalAlign.Left, false);
            gridOrder.AddGridBoundColumn("BankAccountKey", "Bank Account Key", Unit.Percentage(0), HorizontalAlign.Left, false);
            gridOrder.AddGridBoundColumn("FutureDatedChangeKey", "Future Dated Change Key", Unit.Percentage(0), HorizontalAlign.Left, false);

            if (!isApplication)
            {
                gridOrder.AddGridBoundColumn("NaedoCollection", "Collecting by Naedo", Unit.Percentage(8), HorizontalAlign.Center, true);
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

            DOPaymentType.Visible = _viewLabels;
            DOPaymentTypeUpdate.Visible = !_viewLabels;

            BankAccount.Visible = _viewLabels;
            BankAccountUpdate.Visible = !_viewLabels;

            DebitOrderDay.Visible = _viewLabels;
            DebitOrderDayUpdate.Visible = !_viewLabels;

            if (!_ignore)
            {
                EffectiveDate.Visible = _viewLabels;
                EffectiveDateUpdate.Visible = !_viewLabels;
                //  ValEffectiveDateUpdate.Visible = !_viewLabels;
                ValEffectiveDateUpdateControl.Visible = false;
            }

            InfoTable.Visible = _showControls;

            ButtonRow.Visible = _showButtons;

            if (_hideEffectiveDate)
            {
                EffectiveDateUpdate.Visible = false;
                EffectiveDate.Visible = false;
                lblEffectiveDate.Visible = false;
            }
            else if (_setEffectiveDateToCurrentDate)
            {
                EffectiveDate.Text = DateTime.Today.ToLongDateString();
            }

            if (_showControls && _forceShowBankAccountControl)
            {
                lblBankAccount.Visible = true;
                BankAccountUpdate.Visible = true;
            }

            if (!IsPostBack)
                BindControlsToGrid();

            if (_showControls)
            {
                lblBankAccount.Visible = _bankAccountVisible;
                if (!_viewLabels)
                {
                    if (DOPaymentTypeUpdate.SelectedValue == ((int)FinancialServicePaymentTypes.DebitOrderPayment).ToString())
                    {
                        BankAccountUpdate.Visible = true;
                        lblBankAccount.Visible = true;
                    }
                    else
                    {
                        BankAccountUpdate.Visible = false;
                        lblBankAccount.Visible = false;
                    }
                }
                else
                {
                    BankAccount.Visible = _bankAccountVisible;
                }

            }
            if (lblBankAccount.Visible == false
                && BankAccountUpdate.Visible == false
                && BankAccount.Visible == false)
            {
                BnkAccRow.Visible = false;
            }

            SalaryPaymentDayRow.Visible = BnkAccRow.Visible;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OrderGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            KeyChangedEventArgs args = new KeyChangedEventArgs(gridOrder.SelectedIndex);
            _gridSelectedIndex = gridOrder.SelectedIndex;
            if (OnGridSelectedIndexChanged != null)
            {
                OnGridSelectedIndexChanged(sender, args);
            }


            BindControlsToGrid();

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            OnDeleteButtonClicked(sender, new KeyChangedEventArgs(gridOrder.Rows[gridOrder.SelectedIndex].Cells[0].Text));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (DOPaymentTypeUpdate.SelectedValue != "-select-")
            {

                KeyChangedEventArgs args = null;
                _paymentTypeKey = DOPaymentTypeUpdate.SelectedValue;
                _bankAccountKey = BankAccountUpdate.SelectedValue;
                if (DebitOrderDayUpdate.SelectedItem != null)
                {
                    _debitOrderDay = DebitOrderDayUpdate.SelectedItem.Text;
                }
                _effectiveDate = EffectiveDateUpdate.Date;

                if (gridOrder.Rows.Count > 0 && gridOrder.SelectedIndex > -1)
                {
                    int val = -1;
                    if (int.TryParse(gridOrder.Rows[gridOrder.SelectedIndex].Cells[0].Text, out val))
                    {
                        _detailKey = val;
                    }
                }

                if (gridOrder.Rows.Count > 0 && gridOrder.SelectedIndex > -1)
                {
                    args = new KeyChangedEventArgs(gridOrder.Rows[gridOrder.SelectedIndex].Cells[0].Text);
                }

                OnUpdateButtonClicked(sender, args);
            }
            else
            {
                this.Messages.Add(new Error("Please select a valid payment type.", "Please select a valid payment type."));
            }

            // Debit Order day must be set using the ffg method on the framework - this contains DO day
            // rules - speak to SS if more details are required 
            // FinancialServiceBankAccount.SetDebitOrderDay(effectivedate,DebitOrderDay).
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            _paymentTypeKey = DOPaymentTypeUpdate.SelectedValue;
            _bankAccountKey = BankAccountUpdate.SelectedValue;
            if (DebitOrderDayUpdate.SelectedItem != null)
            {
                _debitOrderDay = DebitOrderDayUpdate.SelectedItem.Text;
            }
            _effectiveDate = EffectiveDateUpdate.Date;

            if (gridOrder.Rows.Count > 0 && gridOrder.SelectedIndex > -1)
            {
                int val = -1;
                if (int.TryParse(gridOrder.Rows[gridOrder.SelectedIndex].Cells[0].Text, out val))
                {
                    _detailKey = val;
                }
            }

            OnAddButtonClicked(sender, e);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridOrder_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {

            }
        }


        #region IDebitOrderDetails Members

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnCancelButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnAddButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        public event KeyChangedEventHandler OnUpdateButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        public event KeyChangedEventHandler OnDeleteButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        public bool ShowLabels
        {
            set { _viewLabels = value; }
        }


        /// <summary>
        /// 
        /// </summary>
        public bool ShowControls
        {
            set { _showControls = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fs"></param>
        public void BindGrid(IFinancialService fs)
        {
            List<DebitOrderDetailGridItem> lstItems = new List<DebitOrderDetailGridItem>();

            IFinancialServiceRepository fsRepo = RepositoryFactory.GetRepository<IFinancialServiceRepository>();
            IFutureDatedChangeRepository FDCR = RepositoryFactory.GetRepository<IFutureDatedChangeRepository>();
            ILookupRepository lookUps = RepositoryFactory.GetRepository<ILookupRepository>();
            IBankAccountRepository BAR = RepositoryFactory.GetRepository<IBankAccountRepository>();

            DebitOrderDetailGridItem itm = null;

            // add the financial service bank accounts
            foreach (IFinancialServiceBankAccount fsBankAccount in fs.FinancialServiceBankAccounts)
            {
                if (fsBankAccount.GeneralStatus.Key == (int)SAHL.Common.Globals.GeneralStatuses.Active)
                {
                    itm = new DebitOrderDetailGridItem();
                    itm.Key = fsBankAccount.Key.ToString();
                    itm.EffectiveDate = "Current";
                    itm.FutureDatedChangeKey = "-1";
                    itm.DebitOrderDay = fsBankAccount.DebitOrderDay.ToString();

                    if (fsBankAccount.BankAccount != null)
                    {
                        itm.BankDetails = fsBankAccount.BankAccount.GetDisplayName(BankAccountNameFormat.Full);
                        itm.BankAccountKey = int.Parse(fsBankAccount.BankAccount.Key.ToString());
                    }
                    else
                    {
                        itm.BankDetails = fsBankAccount.FinancialServicePaymentType.Description;
                    }

                    itm.ChangedBy = fsBankAccount.UserID.ToString();
                    itm.ChangeDate = fsBankAccount.ChangeDate.ToString(SAHL.Common.Constants.DateFormat);
                    itm.UserID = fsBankAccount.UserID;
                    itm.PaymentType = fsBankAccount.FinancialServicePaymentType.Description;
                    itm.PaymentTypeKey = fsBankAccount.FinancialServicePaymentType.Key;
                    if (fsBankAccount.BankAccount != null)
                        itm.BankAccountKey = fsBankAccount.BankAccount.Key;

                    itm.NaedoCollection = fsBankAccount.ProviderKey.HasValue && fsBankAccount.ProviderKey.Value == (int)SAHL.Common.Globals.Providers.Naedo ? "Yes" : "No";

                    lstItems.Add(itm);
                }
            }

            // add the future dated changes
            IList<IFutureDatedChange> lstFutureChanges = FDCR.GetFutureDatedChangesByGenericKey(fs.Key, (int)SAHL.Common.Globals.FutureDatedChangeTypes.NormalDebitOrder);
            foreach (IFutureDatedChange futureDatedChange in lstFutureChanges)
            {

                if (futureDatedChange.EffectiveDate < DateTime.Today)
                    continue;

                DebitOrderDetailGridItem detailItem = null;

                foreach (IFutureDatedChangeDetail futureDatedChangeDetail in futureDatedChange.FutureDatedChangeDetails)
                {
                    if (futureDatedChangeDetail.TableName == "FinancialServiceBankAccount")
                    {
                        // check for null on the grid item - if it is then we're working with a new record and 
                        // we need to load up the IFinancialServiceBankAccount to set the default values
                        if (detailItem == null)
                        {
                            detailItem = new DebitOrderDetailGridItem();
                            IFinancialServiceBankAccount detailFsBankAccount = fsRepo.GetFinancialServiceBankAccountByKey(futureDatedChangeDetail.ReferenceKey);
                            if (detailFsBankAccount != null)
                            {
                                detailItem.DebitOrderDay = detailFsBankAccount.DebitOrderDay.ToString();
                                detailItem.ChangedBy = detailFsBankAccount.UserID;
                                detailItem.ChangeDate = detailFsBankAccount.ChangeDate.ToString(SAHL.Common.Constants.DateFormat);
                                detailItem.UserID = detailFsBankAccount.UserID;
                                detailItem.PaymentType = detailFsBankAccount.FinancialServicePaymentType.Description;
                                detailItem.PaymentTypeKey = detailFsBankAccount.FinancialServicePaymentType.Key;
                                if (detailFsBankAccount.BankAccount != null)
                                {
                                    detailItem.BankAccountKey = detailFsBankAccount.BankAccount.Key;
                                    detailItem.BankDetails = detailFsBankAccount.BankAccount.GetDisplayName(BankAccountNameFormat.Full);
                                }
                                detailItem.NaedoCollection = detailFsBankAccount.ProviderKey.HasValue && detailFsBankAccount.ProviderKey.Value == (int)SAHL.Common.Globals.Providers.Naedo ? "Yes" : "No";
                            }

                            // set detail items that only need to get set once but appear on all the detail items
                            detailItem.Key = futureDatedChangeDetail.Key.ToString();
                            detailItem.FutureDatedChangeKey = futureDatedChange.Key.ToString();
                            detailItem.EffectiveDate = futureDatedChange.EffectiveDate.ToString(SAHL.Common.Constants.DateFormat);
                            detailItem.ChangedBy = futureDatedChangeDetail.UserID;
                            detailItem.UserID = futureDatedChangeDetail.UserID;

                        }

                        // now we can check the unique detail item values and update anything else
                        switch (futureDatedChangeDetail.ColumnName)
                        {
                            case "DebitOrderDay":
                                {
                                    detailItem.DebitOrderDay = futureDatedChangeDetail.Value;
                                    break;
                                }
                            case "BankAccountKey":
                                {
                                    if (detailItem.PaymentTypeKey != 2 && detailItem.PaymentTypeKey != 3)
                                    {
                                        IBankAccount ba = BAR.GetBankAccountByKey(int.Parse(futureDatedChangeDetail.Value));
                                        detailItem.BankDetails = ba.GetDisplayName(BankAccountNameFormat.Full);
                                        detailItem.BankAccountKey = int.Parse(futureDatedChangeDetail.Value.ToString());
                                    }
                                    break;
                                }
                            case "FinancialServicePaymentTypeKey":
                                {
                                    detailItem.PaymentType = lookUps.FinancialServicePaymentTypes.ObjectDictionary[futureDatedChangeDetail.Value].Description;
                                    detailItem.PaymentTypeKey = int.Parse(futureDatedChangeDetail.Value);
                                    // detailItem.BankDetails = lookUps.FinancialServicePaymentTypes.ObjectDictionary[futureDatedChangeDetail.Value].Description;
                                    break;
                                }
                        }
                    }
                }

                if (!String.IsNullOrEmpty(detailItem.EffectiveDate))
                    lstItems.Add(detailItem);

            }
            AddGridColumns(false);
            gridOrder.DataSource = lstItems;
            gridOrder.DataBind();

        }

        public void BindPaymentTypes()
        {

            ILookupRepository LR = RepositoryFactory.GetRepository<ILookupRepository>();

            DOPaymentTypeUpdate.DataSource = LR.FinancialServicePaymentTypes;
            DOPaymentTypeUpdate.DataValueField = "Key";
            DOPaymentTypeUpdate.DataTextField = "Description";
            DOPaymentTypeUpdate.DataBind();

        }

        public void BindDebitOrderDays()
        {
            ILookupRepository LR = RepositoryFactory.GetRepository<ILookupRepository>();

            DebitOrderDayUpdate.DataSource = LR.DebitOrderDays;
            DebitOrderDayUpdate.DataValueField = "Key";
            DebitOrderDayUpdate.DataTextField = "Day";
            DebitOrderDayUpdate.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gridRowIndex"></param>
        public void BindLabels(int gridRowIndex)
        {
            if (gridRowIndex > -1)
            {
                if (_setControlsToGrid && gridOrder.Rows.Count > 0)
                {
                    DOPaymentType.Text = gridOrder.Rows[gridRowIndex].Cells[7].Text;
                    if (gridOrder.Rows[gridRowIndex].Cells[6].Text == "1")
                    {
                        _bankAccountVisible = true;
                    }
                    else
                    {
                        _bankAccountVisible = false;
                    }

                    BankAccount.Text = gridOrder.Rows[gridRowIndex].Cells[1].Text;
                    DebitOrderDay.Text = gridOrder.Rows[gridRowIndex].Cells[2].Text;

                    EffectiveDate.Text = gridOrder.Rows[gridRowIndex].Cells[0].Text;
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public bool ShowButtons
        {
            set { _showButtons = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public GridPostBackType gridPostBackType
        {
            set { gridOrder.PostBackType = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public event KeyChangedEventHandler OnGridSelectedIndexChanged;

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
        /// <param name="bankAccounts"></param>
        public void BindBankAccountControl(IEventList<IBankAccount> bankAccounts)
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();

            // StringBuilder sb;

            for (int x = 0; x < bankAccounts.Count; x++)
            {
                if (bankAccounts[x] != null && !dic.ContainsKey(bankAccounts[x].Key))
                {
                    dic.Add(bankAccounts[x].Key, bankAccounts[x].GetDisplayName(BankAccountNameFormat.Full));
                }
            }
            BankAccountUpdate.DataSource = dic;
            BankAccountUpdate.DataValueField = "Key";
            BankAccountUpdate.DataTextField = "Description";
            BankAccountUpdate.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        public bool SetControlsToGrid
        {
            set { _setControlsToGrid = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ForceShowBankAccountControl
        {
            set { _forceShowBankAccountControl = value; }
        }

        public void BindGridForApplication(IApplication application)
        {
            // gridOrder.PostBackType = _gridPostBackType;

            List<DebitOrderDetailGridItem> lstItems = new List<DebitOrderDetailGridItem>();

            IApplicationRepository AR = RepositoryFactory.GetRepository<IApplicationRepository>();
            IEventList<IApplicationDebitOrder> lstDO = AR.GetApplicationDebitOrdersByApplicationKey(application.Key);
            for (int x = 0; x < lstDO.Count; x++)
            {
                DebitOrderDetailGridItem itm = new DebitOrderDetailGridItem();
                itm.Key = lstDO[x].Key.ToString();
                itm.EffectiveDate = "Current";
                itm.DebitOrderDay = lstDO[x].DebitOrderDay.ToString();
                if (lstDO[x].FinancialServicePaymentType.Key == (int)SAHL.Common.Globals.FinancialServicePaymentTypes.DebitOrderPayment
                || lstDO[x].FinancialServicePaymentType.Key == (int)SAHL.Common.Globals.FinancialServicePaymentTypes.DirectPayment)
                {
                    if (lstDO[x].BankAccount != null)
                    {
                        itm.BankDetails = lstDO[x].BankAccount.GetDisplayName(BankAccountNameFormat.Full);
                        if (lstDO[x].BankAccount != null)
                        {
                            itm.BankAccountKey = int.Parse(lstDO[x].BankAccount.Key.ToString());
                        }
                    }
                }
                else
                {
                    if (lstDO[x].BankAccount != null)
                    {
                        itm.BankDetails = lstDO[x].BankAccount.GetDisplayName(BankAccountNameFormat.Full);
                    }
                }


                //    itm.ChangedBy = ?
                //    itm.ChangeDate = ?
                //    itm.UserID = ?
                itm.PaymentType = lstDO[x].FinancialServicePaymentType.Description;
                itm.PaymentTypeKey = lstDO[x].FinancialServicePaymentType.Key;
                lstItems.Add(itm);
            }

            AddGridColumns(true);
            gridOrder.DataSource = lstItems;
            gridOrder.DataBind();
        }

        protected void DOPaymentTypeUpdate_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public bool HideEffectiveDate
        {
            set { _hideEffectiveDate = value; }
        }


        public string BankAccountKey
        {
            get { return _bankAccountKey; }
        }

        public string PaymentTypeKey
        {
            get { return _paymentTypeKey; }
        }

        public int DetailKey
        {
            get { return _detailKey; }
        }

        #endregion

        #region IDebitOrderDetails Members


        public string DODay
        {
            get { return _debitOrderDay; }
        }

        public DateTime? EffectDate
        {
            get { return _effectiveDate; }
        }


        public int selectedGridIndex
        {
            get { return _gridSelectedIndex; }
        }


        public Dictionary<string, int> GridOrderDict
        {
            get
            {
                if (_gridOrderDict == null || _gridOrderDict.Count == 0)
                {
                    _gridOrderDict = new Dictionary<string, int>();
                    for (int i = 0; i < gridOrder.Columns.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(gridOrder.Columns[i].HeaderText) && !_gridOrderDict.ContainsKey(gridOrder.Columns[i].HeaderText))
                            _gridOrderDict.Add(gridOrder.Columns[i].HeaderText, i);
                    }
                }
                return _gridOrderDict;
            }
        }

        public void BindControlsToGrid()
        {
            int gridRowIndex = gridOrder.SelectedIndex;

            if (gridRowIndex > -1)
            {
                if (_setControlsToGrid && gridOrder.Rows.Count > 0)
                {
                    for (int x = 0; x < DebitOrderDayUpdate.Items.Count; x++)
                    {
                        //if (DebitOrderDayUpdate.Items[x].Value == gridOrder.Rows[gridRowIndex].Cells[2].Text)
                        if (DebitOrderDayUpdate.Items[x].Text == gridOrder.Rows[gridRowIndex].Cells[GridOrderDict["Debit Order Day"]].Text)
                        {
                            DebitOrderDayUpdate.SelectedIndex = x;
                            break;
                        }
                    }

                    for (int x = 0; x < DOPaymentTypeUpdate.Items.Count; x++)
                    {
                        //if (DOPaymentTypeUpdate.Items[x].Value == gridOrder.Rows[gridRowIndex].Cells[7].Text)
                        if (DOPaymentTypeUpdate.Items[x].Value == gridOrder.Rows[gridRowIndex].Cells[GridOrderDict["Payment Type Key"]].Text)
                        {
                            DOPaymentTypeUpdate.SelectedIndex = x;
                            break;
                        }
                    }

                    for (int x = 0; x < BankAccountUpdate.Items.Count; x++)
                    {
                        //if (BankAccountUpdate.Items[x].Value == gridOrder.Rows[gridRowIndex].Cells[8].Text)
                        if (BankAccountUpdate.Items[x].Value == gridOrder.Rows[gridRowIndex].Cells[GridOrderDict["Bank Account Key"]].Text)
                        {
                            BankAccountUpdate.SelectedIndex = x;
                            break;
                        }
                    }

                    // Duplicated Code - no idea why it has been duplicated !!!
                    //for (int x = 0; x < DebitOrderDayUpdate.Items.Count; x++)
                    //{
                    //    if (DebitOrderDayUpdate.Items[x].Text == gridOrder.Rows[gridRowIndex].Cells[3].Text)
                    //    {
                    //        DebitOrderDayUpdate.SelectedIndex = x;
                    //        break;
                    //    }
                    //}

                    if (_hideEffectiveDate == false)
                    {
                        if (gridOrder.Rows[gridRowIndex].Cells[1].Text != "Current")
                            EffectiveDateUpdate.Date = new DateTime?(DateTime.Parse(gridOrder.Rows[gridRowIndex].Cells[1].Text, new CultureInfo(SAHL.Common.Web.UI.Controls.Constants.CultureGb)));

                        EffectiveDateUpdate.Visible = true;
                        EffectiveDate.Visible = false;
                    }
                    else
                    {
                        lblEffectiveDate.Visible = false;
                        EffectiveDateUpdate.Visible = false;
                    }
                }
            }
        }

        public bool Ignore
        {
            set
            {
                _ignore = value;
            }
        }

        public int? FutureDatedChangeKey
        {
            get
            {
                int val = -1;
                if (int.TryParse(gridOrder.Rows[gridOrder.SelectedIndex].Cells[GridOrderDict["Future Dated Change Key"]].Text, out val))
                    return new int?(val);
                else
                    return new int?();
            }
        }

        /// <summary>
        /// Gets/sets the visibility of the Add button.
        /// </summary>
        public bool ButtonAddVisible
        {
            get
            {
                return btnAdd.Visible;
            }
            set
            {
                btnAdd.Visible = value;
            }
        }

        /// <summary>
        /// Gets/sets the visibility of the Update button.
        /// </summary>
        public bool ButtonUpdateVisible
        {
            get
            {
                return btnUpdate.Visible;
            }
            set
            {
                btnUpdate.Visible = value;
            }
        }

        /// <summary>
        /// Gets/sets the visibility of the Delete button.
        /// </summary>
        public bool ButtonDeleteVisible
        {
            get
            {
                return btnDelete.Visible;
            }
            set
            {
                btnDelete.Visible = value;
            }
        }

        #endregion


        public event KeyChangedEventHandler OnDebitOrderPaymentTypeChanged;


        public void BindSalaryPaymentDays(IDictionary<ILegalEntity, string> dicSalaryPaymentDates)
        {
            if (SalaryPaymentDayRow.Visible == true)
            {
                foreach (var item in dicSalaryPaymentDates)
                {
                    TableRow tr = new TableRow();
                    TableCell tcName = new TableCell();
                    TableCell tcDays = new TableCell();

                    tcName.Text = "&nbsp;" + item.Key.DisplayName;
                    tcDays.Text = " - " + item.Value;
                    tr.Cells.Add(tcName);
                    tr.Cells.Add(tcDays);
                    tblSalaryPaymentDays.Rows.Add(tr);

                    if (item.Value.Contains(","))
                        lblSalaryPaymentDays.Text = "Salary Payment Days";
                }

                if (dicSalaryPaymentDates.Count <= 0)
                    lblSalaryPaymentDays.Visible = false;
            }
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public class DebitOrderDetailGridItem
    {
        private string _key;
        private string _futureDatedChangeKey;
        private string _effectiveDate;
        private string _bankDetails;
        private string _debitOrderDay;
        private string _changedBy;
        private string _changeDate;
        private string _userID;
        private string _paymentType;
        private int _paymentTypeKey;
        private int _bankAccountKey;
        private string _naedoCollection;


        public string FutureDatedChangeKey
        {
            get
            {
                return _futureDatedChangeKey;
            }
            set
            {
                _futureDatedChangeKey = value;
            }
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

        /// <summary>
        /// 
        /// </summary>
        public string EffectiveDate
        {
            get
            {
                return _effectiveDate;
            }
            set
            {
                _effectiveDate = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string BankDetails
        {
            get
            {
                return _bankDetails;
            }
            set
            {
                _bankDetails = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string DebitOrderDay
        {
            get
            {
                return _debitOrderDay;
            }
            set
            {
                _debitOrderDay = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ChangedBy
        {
            get
            {
                return _changedBy;
            }
            set
            {
                _changedBy = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ChangeDate
        {
            get
            {
                return _changeDate;
            }
            set
            {
                _changeDate = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string PaymentType
        {
            get
            {
                return _paymentType;
            }
            set
            {
                _paymentType = value;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public int PaymentTypeKey
        {
            get
            {
                return _paymentTypeKey;
            }
            set
            {
                _paymentTypeKey = value;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public string UserID
        {
            get
            {
                return _userID;
            }
            set
            {
                _userID = value;
            }
        }

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

        public string NaedoCollection
        {
            get
            {
                return _naedoCollection;
            }
            set
            {
                _naedoCollection = value;
            }
        }

    }
}
