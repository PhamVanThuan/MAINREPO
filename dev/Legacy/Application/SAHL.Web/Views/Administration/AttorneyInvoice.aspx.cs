using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Administration.Interfaces;
using SAHL.Web.AJAX;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Controls;
using DevExpress.Web.ASPxGridView;
using DevExpress.Data;

namespace SAHL.Web.Views.Administration
{
    public partial class AttorneyInvoice : SAHLCommonBaseView, IAttorneyInvoice
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!ShouldRunPage) return;

            if (!ReadOnly)
            {
                RegisterWebService(ServiceConstants.Account);
                acAccount.ServiceMethod = WebServiceUrls.SearchForAccountsByKey;

                
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!ShouldRunPage) return;

            dteInvoiceDate.Date = DateTime.Now;
        }

        /// <summary>
        /// On Search Result Item Selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnAccountItemSelected(object sender, KeyChangedEventArgs e)
        {
            AccountKey = Convert.ToInt32(e.Key.ToString());

            //get AccountAttorneyInvoices and Bind to grid
            if (OnAccountItemSelect != null)
                OnAccountItemSelect(sender, e);

            btnAdd.Enabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="invList"></param>
        public void BindGrid(IEventList<IAccountAttorneyInvoice> invList)
        {
            pnlInvoiceDetail.Visible = true;

            grdInvoice.SettingsText.Title = String.Format(@"Non Capitalised Invoice Detail: {0}", AccountKey.ToString());

            grdInvoice.Columns.Clear();

            grdInvoice.TotalSummary.Clear();
            ASPxSummaryItem totalHeading = new ASPxSummaryItem();
            totalHeading.FieldName = "InvoiceDate";
            totalHeading.ShowInColumn = "InvoiceDate";
            totalHeading.SummaryType = SummaryItemType.None;
            totalHeading.DisplayFormat = "Totals:";
            grdInvoice.TotalSummary.Add(totalHeading);

            ASPxSummaryItem totalAmount = new ASPxSummaryItem();
            totalAmount.FieldName = "Amount";
            totalAmount.ShowInColumn = "Amount";
            totalAmount.SummaryType = SummaryItemType.Sum;
            totalAmount.DisplayFormat = "{0:c2}";
            grdInvoice.TotalSummary.Add(totalAmount);

            ASPxSummaryItem totalVatAmount = new ASPxSummaryItem();
            totalVatAmount.FieldName = "VatAmount";
            totalVatAmount.ShowInColumn = "VatAmount";
            totalVatAmount.SummaryType = SummaryItemType.Sum;
            totalVatAmount.DisplayFormat = "{0:c2}";
            grdInvoice.TotalSummary.Add(totalVatAmount);

            ASPxSummaryItem totalTotalAmount = new ASPxSummaryItem();
            totalTotalAmount.FieldName = "TotalAmount";
            totalTotalAmount.ShowInColumn = "TotalAmount";
            totalTotalAmount.SummaryType = SummaryItemType.Sum;
            totalTotalAmount.DisplayFormat = "{0:c2}";
            grdInvoice.TotalSummary.Add(totalTotalAmount);

            grdInvoice.KeyFieldName = "Key";
            grdInvoice.AddGridColumn("Key", "Key", 0, GridFormatType.GridString, "", HorizontalAlign.Left, false, true);
            grdInvoice.AddGridColumn("AttorneyRegisteredName", "Attorney", 20, GridFormatType.GridString, "", HorizontalAlign.Left, true, true);
            grdInvoice.AddGridColumn("InvoiceNumber", "Invoice Number", 10, GridFormatType.GridString, "", HorizontalAlign.Left, true, true);
            grdInvoice.AddGridColumn("InvoiceDate", "Date", 8, GridFormatType.GridDate, SAHL.Common.Constants.DateFormat, HorizontalAlign.Center, true, true);
            grdInvoice.AddGridColumn("Amount", "Amount", 10, GridFormatType.GridCurrency, "", HorizontalAlign.Right, true, true);
            grdInvoice.AddGridColumn("VatAmount", "Vat", 8, GridFormatType.GridCurrency, "", HorizontalAlign.Right, true, true);
            grdInvoice.AddGridColumn("TotalAmount", "Total", 10, GridFormatType.GridCurrency, "", HorizontalAlign.Right, true, true);
            grdInvoice.AddGridColumn("Comment", "Comment",24, GridFormatType.GridString, "", HorizontalAlign.Left, true, true);
            grdInvoice.AddGridColumn("ChangeDate", "Created", 8, GridFormatType.GridDate, SAHL.Common.Constants.DateFormat, HorizontalAlign.Center, true, true);


            grdInvoice.DataSource = invList;
            grdInvoice.DataBind();

            btnDelete.Enabled = true;
            if (invList.Count < 1)
                btnDelete.Enabled = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attorneys"></param>
        public void BindAttorneys(Dictionary<int, string> attorneys)
        {
            ddlAttorney.DataSource = attorneys;
            ddlAttorney.DataBind();
        }

        public int AccountKey { get; set; }
        public int SelectedAccountAttorneyInvoiceKey { get; set; }

        private bool _readOnly;
        public bool ReadOnly
        {
            get
            {
                return _readOnly;
            }
            set
            {
                _readOnly = value;
                pnlAttorneySelect.Visible = !value;
                btnDelete.Visible = !value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void ResetInputs()
        {
            txtInvNumber.Text = "";
            dteInvoiceDate.Date = DateTime.Now;
            currAmount.Text = "";
            currVatAmount.Text = "";
            currTotalAmount.Text = "";
            txtComments.Text = "";
        }
        public void PopulateDetail(IAccountAttorneyInvoice accAttInv)
        {
            if (AccountKey > 0)
                accAttInv.AccountKey = AccountKey;

            if (ddlAttorney.SelectedValue != "-select-")
                accAttInv.AttorneyKey = Convert.ToInt32(ddlAttorney.SelectedValue);

            accAttInv.InvoiceNumber = txtInvNumber.Text;
            accAttInv.InvoiceDate = dteInvoiceDate.Date.HasValue ? dteInvoiceDate.Date.Value : DateTime.MinValue;

            if (!String.IsNullOrEmpty(currAmount.Text))
                accAttInv.Amount = Convert.ToDecimal(currAmount.Text);
            else
                accAttInv.Amount = 0;

            if (!String.IsNullOrEmpty(currVatAmount.Text))
                accAttInv.VatAmount = Convert.ToDecimal(currVatAmount.Text);
            else
                accAttInv.VatAmount = 0;

            accAttInv.TotalAmount = accAttInv.Amount + accAttInv.VatAmount;

            accAttInv.Comment = txtComments.Text;
            accAttInv.ChangeDate = DateTime.Now;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (OnAddClick != null)
                OnAddClick(sender, e);

            btnAdd.Enabled = true;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            SelectedAccountAttorneyInvoiceKey = Convert.ToInt32(grdInvoice.SelectedKeyValue);

            if (OnDeleteClick != null)
                OnDeleteClick(sender, e);

            btnAdd.Enabled = true;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (OnCancelClick != null)
                OnCancelClick(sender, e);
        }

        public event EventHandler OnAccountItemSelect;

        public event EventHandler OnAddClick;

        public event EventHandler OnCancelClick;

        public event EventHandler OnDeleteClick;
    }
}