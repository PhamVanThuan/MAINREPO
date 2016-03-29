using BuildingBlocks.Services.Contracts;
using Common.Constants;
using ObjectMaps.Pages;
using System;
using System.Collections.Generic;
using WatiN.Core;

namespace BuildingBlocks.Presenters.Admin
{
    public class AttorneyInvoice : AttorneyInvoiceControls
    {
        private readonly IWatiNService watinService;

        public AttorneyInvoice()
        {
            watinService = ServiceLocator.Instance.GetService<IWatiNService>();
        }

        /// <summary>
        /// Populates the Attorney Invoice View
        /// </summary>
        /// <param name="accountkey"></param>
        /// <param name="invoicenumber"></param>
        /// <param name="attorney"></param>
        /// <param name="amount"></param>
        /// <param name="comments"></param>
        /// <param name="VATamount"></param>
        /// <param name="invoiceDate"></param>
        public void PopulateFields
            (
                int accountkey = 0,
                string invoicenumber = "",
                string attorney = "",
                decimal amount = 0,
                string comments = "",
                decimal VATamount = 0,
                DateTime? invoiceDate = null
            )
        {
            string date = invoiceDate == null ? DateTime.Now.ToString(Formats.DateFormat) : Convert.ToDateTime(invoiceDate).ToString(Formats.DateFormat);
            if (accountkey != 0)
            {
                base.AccountNumber.TypeText(accountkey.ToString());
                base.SAHLAutoComplete_DefaultItem.WaitUntilExists(10);
                base.SAHLAutoComplete_DefaultItem.MouseDown();
            }

            if (!string.IsNullOrEmpty(invoicenumber))
                base.InvoiceNumber.Value = invoicenumber;

            if (!string.IsNullOrEmpty(attorney))
                base.AttorneyList.Option(attorney).Select();

            if (amount != 0)
                base.InvoiceAmountRands.Value = amount.ToString();

            if (!string.IsNullOrEmpty(comments))
                base.Comments.Value = comments.ToString();

            if (VATamount >= 0)
                base.VatAmtRands.Value = VATamount.ToString();

            if (!string.IsNullOrEmpty(date))
                base.InvoiceDate.Clear();
            base.InvoiceDate.Value = date;
        }

        /// <summary>
        /// Click Add
        /// </summary>
        public void Add()
        {
            base.btnAdd.Click();
        }

        /// <summary>
        /// Click Cancel
        /// </summary>
        public void ClickCancel()
        {
            base.btnCancel.Click();
        }

        /// <summary>
        /// Click Delete
        /// </summary>
        public void ClickDelete()
        {
            //confirm
            watinService.HandleConfirmationPopup(base.btnDelete);
        }

        /// <summary>
        /// Get the list of domain validation messages
        /// </summary>
        /// <returns></returns>
        public string GetValidationMessages()
        {
            List<string> messages = new List<string>();
            ElementCollection elementCollection = base.SAHLValidationSummaryBody.ElementsWithTag("ol");
            if (elementCollection.Count == 0)
                return string.Empty;
            Element messageelement = elementCollection[0];
            return messageelement.Text;
        }

        /// <summary>
        /// Gets the list of attorneys
        /// </summary>
        /// <returns></returns>
        public List<string> GetAttorneyList()
        {
            List<string> attorneys = new List<string>();
            foreach (Option option in base.AttorneyList.Options)
                attorneys.Add(option.Text);
            return attorneys;
        }

        /// <summary>
        /// Select an invoice record from the grid
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="columnIndex"></param>
        public void SelectInvoiceRecord(string expression)
        {
            TableRow row = base.gridCellsSelected(expression);
            row.Click();
        }
    }
}