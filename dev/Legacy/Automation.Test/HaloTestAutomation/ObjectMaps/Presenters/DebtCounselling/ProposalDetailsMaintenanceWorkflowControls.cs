using System;
using WatiN.Core;

namespace ObjectMaps.Pages
{
    public class ProposalDetailsMaintenanceWorkflowControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_ProposalTab_ProposalInformation_ddlHOCIncl")]
        protected SelectList HOCInclusive { get; set; }

        [FindBy(Id = "ctl00_Main_ProposalTab_ProposalInformation_ddlLifeIncl")]
        protected SelectList LifeInclusive { get; set; }

        [FindBy(Id = "ctl00_Main_ProposalTab_ProposalInformation_apDetailGrid_content_gvProposalItems_DXMainTable")]
        protected Table ProposalItems { get; set; }

        public TableRowCollection ProposalItemsTableRows
        {
            get
            {
                return ProposalItems.OwnTableRows;
            }
        }

        public TableRow ProposalItemsTableRow(DateTime startDate, DateTime endDate)
        {
            TableRowCollection rows = ProposalItemsTableRows;

            foreach (TableRow row in rows)
            {
                if (row.TableCell(Find.ByText(startDate.ToString(@"dd/MM/yyyy"))).Exists && row.TableCell(Find.ByText(endDate.ToString(@"dd/MM/yyyy"))).Exists)
                    return row;
            }
            throw new WatiN.Core.Exceptions.WatiNException("");
        }

        public TableRow SelectProposalItemsByRowNumber(int rowNumber)
        {
            return ProposalItemsTableRows[rowNumber];
        }

        public TableCellCollection ctl00MaingvProposalItemsDXMainTableCells(int rowNumber)
        {
            return SelectProposalItemsByRowNumber(rowNumber).TableCells;
        }

        public TableCell ctl00MaingvProposalItemsDXMainTableCell(int rowNumber, int cellNumber)
        {
            return SelectProposalItemsByRowNumber(rowNumber).TableCells[cellNumber];
        }

        [FindBy(Id = "ctl00_Main_ProposalTab_ProposalInformation_dteStartDate")]
        protected TextField StartDate { get; set; }

        [FindBy(Id = "ctl00_Main_ProposalTab_ProposalInformation_dteEndDate")]
        protected TextField EndDate { get; set; }

        [FindBy(Id = "ctl00_Main_ProposalTab_ProposalInformation_ddlMarketRate")]
        protected SelectList MarketRate { get; set; }

        [FindBy(Id = "ctl00_Main_ProposalTab_ProposalInformation_txtInterestRate_txtRands")]
        protected TextField InterestRateRands { get; set; }

        [FindBy(Id = "ctl00_Main_ProposalTab_ProposalInformation_txtInterestRate_txtCents")]
        protected TextField InterestRateCents { get; set; }

        [FindBy(Id = "ctl00_Main_ProposalTab_ProposalInformation_txtAmount_txtRands")]
        protected TextField AmountRands { get; set; }

        [FindBy(Id = "ctl00_Main_ProposalTab_ProposalInformation_txtAmount_txtCents")]
        protected TextField AmountCents { get; set; }

        [FindBy(Id = "ctl00_Main_ProposalTab_ProposalInformation_txtAdditionalAmount_txtRands")]
        protected TextField AdditionalAmountRands { get; set; }

        [FindBy(Id = "ctl00_Main_ProposalTab_ProposalInformation_txtAdditionalAmount_txtCents")]
        protected TextField AdditionalAmountCents { get; set; }

        [FindBy(Id = "ctl00_Main_ProposalTab_ProposalInformation_txtStartPeriod")]
        protected TextField StartPeriod { get; set; }

        [FindBy(Id = "ctl00_Main_ProposalTab_ProposalInformation_txtEndPeriod")]
        protected TextField EndPeriod { get; set; }

        [FindBy(Id = "ctl00_Main_ProposalTab_ProposalInformation_btnAdd")]
        protected Button Add { get; set; }

        [FindBy(Id = "ctl00_Main_ProposalTab_ProposalInformation_btnRemove")]
        protected Button Remove { get; set; }

        [FindBy(Id = "ctl00_Main_ProposalTab_ProposalInformation_btnCancel")]
        protected Button Cancel { get; set; }

        [FindBy(Id = "ctl00_Main_ProposalTab_ProposalInformation_chkServiceFee")]
        protected CheckBox ServiceFee { get; set; }
    }
}