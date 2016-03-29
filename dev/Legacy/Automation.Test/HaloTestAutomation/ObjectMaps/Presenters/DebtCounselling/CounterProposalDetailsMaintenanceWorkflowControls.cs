using System;
using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class CounterProposalDetailsMaintenanceWorkflowControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_ProposalTab_ProposalInformation_ddlHOCIncl")]
        protected SelectList HOC { get; set; }

        [FindBy(Id = "ctl00_Main_ProposalTab_ProposalInformation_ddlLifeIncl")]
        protected SelectList Life { get; set; }

        [FindBy(Id = "ctl00_Main_ProposalTab_ProposalInformation_dteReviewDate")]
        protected TextField ReviewDate { get; set; }

        protected Link linkCounterProposalNotes
        {
            get
            {
                return base.Document.Div("ctl00_Main_ProposalTab_ProposalInformation_apReason_header").Link(Find.ByText("Counter Proposal Notes"));
            }
        }

        [FindBy(Id = "ctl00_Main_ProposalTab_ProposalInformation_apReason_content_txtCounterReason")]
        protected TextField CounterProposalNote { get; set; }

        protected Link linkAccountSummary
        {
            get
            {
                return base.Document.Div("ctl00_Main_ProposalTab_ProposalInformation_apAccountSummary_header").Link(Find.ByText("Account Summary"));
            }
        }

        protected Link linkDetails
        {
            get
            {
                return base.Document.Div("ctl00_Main_ProposalTab_ProposalInformation_apDetailGrid_header").Link(Find.ByText("Details"));
            }
        }

        [FindBy(Id = "ctl00_Main_ProposalTab_ProposalInformation_ctl00_Main_lblProposalStatus")]
        protected Span lblProposalStatus { get; set; }

        [FindBy(Id = "ctl00_Main_ProposalTab_ProposalInformation_apDetailGrid_content_gvProposalItems_DXMainTable")]
        protected Table ProposalItems { get; set; }

        protected TableRowCollection ProposalItemsRows
        {
            get
            {
                return ProposalItems.OwnTableRows;
            }
        }

        protected TableRow ProposalItemRow(DateTime startDate, DateTime endDate)
        {
            TableRowCollection rows = ProposalItemsRows;

            foreach (TableRow row in rows)
            {
                if (row.TableCell(Find.ByText(startDate.ToString(@"dd/MM/yyyy"))).Exists && row.TableCell(Find.ByText(endDate.ToString(@"dd/MM/yyyy"))).Exists)
                    return row;
            }
            throw new WatiN.Core.Exceptions.WatiNException("");
        }

        protected TableRow ctl00MaingvProposalItemsDXMainTableRow(int rowNumber)
        {
            return ProposalItemsRows[rowNumber];
        }

        protected TableCellCollection ProposalItemTableRowCellCollection(int rowNumber)
        {
            return ctl00MaingvProposalItemsDXMainTableRow(rowNumber).TableCells;
        }

        protected TableCell ProposalItemTableRowCell(int rowNumber, int cellNumber)
        {
            return ctl00MaingvProposalItemsDXMainTableRow(rowNumber).TableCells[cellNumber];
        }

        [FindBy(Id = "ctl00_Main_ProposalTab_ProposalInformation_dteStartDate")]
        protected TextField StartDate { get; set; }

        [FindBy(Id = "ctl00_Main_ProposalTab_ProposalInformation_dteEndDate")]
        protected TextField EndDate { get; set; }

        [FindBy(Id = "ctl00_Main_ProposalTab_ProposalInformation_txtInterestRate_txtRands")]
        protected TextField InterestRate { get; set; }

        [FindBy(Id = "ctl00_Main_ProposalTab_ProposalInformation_txtInterestRate_txtCents")]
        protected TextField InterestRateDecimals { get; set; }

        [FindBy(Id = "ctl00_Main_ProposalTab_ProposalInformation_txtInstalPercentDisplay_txtRands")]
        protected TextField InstalmentPercentage { get; set; }

        [FindBy(Id = "ctl00_Main_ProposalTab_ProposalInformation_txtInstalPercentDisplay_txtCents")]
        protected TextField InstalmentPercentageDecimal { get; set; }

        [FindBy(Id = "ctl00_Main_ProposalTab_ProposalInformation_txtInstalment_txtRands")]
        protected TextField InstalmentRands { get; set; }

        [FindBy(Id = "ctl00_Main_ProposalTab_ProposalInformation_txtInstalment_txtCents")]
        protected TextField InstalmentCents { get; set; }

        [FindBy(Id = "ctl00_Main_ProposalTab_ProposalInformation_txtAnnualEscalation_txtRands")]
        protected TextField AnnualEscalation { get; set; }

        [FindBy(Id = "ctl00_Main_ProposalTab_ProposalInformation_txtAnnualEscalation_txtCents")]
        protected TextField AnnualEscalationDecimal { get; set; }

        [FindBy(Id = "ctl00_Main_ProposalTab_ProposalInformation_btnAdd")]
        protected Button btnAdd { get; set; }

        [FindBy(Id = "ctl00_Main_ProposalTab_ProposalInformation_btnRemove")]
        protected Button btnRemove { get; set; }

        [FindBy(Id = "ctl00_Main_ProposalTab_ProposalInformation_btnCancel")]
        protected Button btnCancel { get; set; }

        [FindBy(Id = "ctl00_Main_ProposalTab_ProposalInformation_txtStartPeriod")]
        protected TextField StartPeriod { get; set; }

        [FindBy(Id = "ctl00_Main_ProposalTab_ProposalInformation_txtEndPeriod")]
        protected TextField EndPeriod { get; set; }

        [FindBy(Id = "ctl00_Main_ProposalTab_ProposalInformation_chkServiceFee")]
        protected CheckBox ServiceFee { get; set; }
    }
}