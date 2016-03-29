using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class AttorneyInvoiceControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_txtInvNumber")]
        protected TextField InvoiceNumber { get; set; }

        [FindBy(Id = "ctl00_Main_currAmount_txtRands")]
        protected TextField InvoiceAmountRands { get; set; }

        [FindBy(Id = "ctl00_Main_currAmount_txtCents")]
        protected TextField InvoiceAmountCents { get; set; }

        [FindBy(Id = "ctl00_Main_currVatAmount_txtRands")]
        protected TextField VatAmtRands { get; set; }

        [FindBy(Id = "ctl00_Main_txtComments")]
        protected TextField Comments { get; set; }

        [FindBy(Id = "ctl00_Main_btnAdd")]
        protected Button btnAdd { get; set; }

        [FindBy(Id = "ctl00_Main_btnCancel")]
        protected Button btnCancel { get; set; }

        [FindBy(Id = "ctl00_Main_ddlAttorney")]
        protected SelectList AttorneyList { get; set; }

        [FindBy(Id = "ctl00_Main_txtAccountKey")]
        protected TextField AccountNumber { get; set; }

        [FindBy(Class = "SAHLValidationSummaryBody")]
        protected Div SAHLValidationSummaryBody { get; set; }

        [FindBy(Class = "SAHLAutoComplete_DefaultItem")]
        protected Div SAHLAutoComplete_DefaultItem { get; set; }

        [FindBy(Id = "ctl00_Main_grdInvoice_DXMainTable")]
        protected Table InvoiceGrid { get; set; }

        [FindBy(Id = "ctl00_Main_btnDelete")]
        protected Button btnDelete { get; set; }

        [FindBy(Id = "ctl00_Main_dteInvoiceDate")]
        protected TextField InvoiceDate { get; set; }

        /// <summary>
        /// Selects the first cell it finds with the matching invoice number
        /// </summary>
        /// <param name="invoiceNumber"></param>
        /// <returns></returns>
        protected TableRow gridCellsSelected(string invoiceNumber)
        {
            TableCellCollection cells = InvoiceGrid.TableCells;
            var cell = Find.ByText(invoiceNumber);
            var filter = cells.Filter(cell);
            return filter.Count > 0 ? filter[0].ContainingTableRow : null;
        }
    }
}