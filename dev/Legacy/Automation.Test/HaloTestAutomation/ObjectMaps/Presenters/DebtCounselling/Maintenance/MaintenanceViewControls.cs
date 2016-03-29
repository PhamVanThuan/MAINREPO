using ObjectMaps.Pages;
using WatiN.Core;

namespace ObjectMaps
{
    public abstract class LegalEntityOrganisationStructureMaintenanceViewControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_btnAdd")]
        public Button btnAdd;

        [FindBy(Id = "ctl00_Main_btnRemove")]
        protected Button btnRemove;

        [FindBy(Id = "ctl00_Main_btnUpdate")]
        protected Button btnUpdate;

        [FindBy(Id = "ctl00_Main_btnView")]
        protected Button btnView;

        [FindBy(Id = "ctl00_Main_btnSelect")]
        protected Button btnSelect;

        [FindBy(Id = "ctl00_Main_btnAddToCBO")]
        protected Button btnAddToMenu;

        protected Span Description(string labelDescription)
        {
            return base.Document.Span(Find.ByText(labelDescription));
        }

        /// <summary>
        /// The encapsulating table for the 'Payment Distribution Agencies / Agents' tree structure
        /// </summary>
        [FindBy(Id = "ctl00_Main_tlOrgStructure_D")]
        protected Table tblOrgStructure;

        /// <summary>
        /// The span element encapsulating the label in the 'Description' column of the 'Payment Distribution Agencies / Agents' table
        /// </summary>
        /// <param name="description">Description</param>
        /// <returns>The span element encapsulating the label in the 'Description' column of the 'Payment Distribution Agencies / Agents' table</returns>
        protected Span spanDescription(string description)
        {
            return tblOrgStructure.Span(Find.ByText(description));
        }

        /// <summary>
        /// Identifies a cell in the 'Payment Distribution Agencies / Agents' table that matches a given string.  Expect this to be a cell in the 'Display Name' column
        /// </summary>
        /// <param name="description">DisplayName</param>
        /// <returns>A TableCell in the 'Payment Distribution Agencies / Agents'</returns>
        protected TableCell DisplayNameCell(string description)
        {
            return tblOrgStructure.TableCell(Find.ByText(description));
        }

        /// <summary>
        /// The encapsulating TableCell of the span element identified by spanDescription
        /// </summary>
        /// <param name="description">description</param>
        /// <returns>The encapsulating TableCell of the span element identified by spanDescription</returns>
        protected TableCell DescriptionCell(string description)
        {
            return spanDescription(description).Parent as TableCell;
        }

        /// <summary>
        /// Gets the encapsulating TableCell of a TableCell element
        /// </summary>
        /// <param name="tierCell">DisplayNameCell(string description), DescriptionCell(string description)</param>
        /// <returns>The encapsulating TableCell of a TableCell element for a tier</returns>
        protected TableRow TierRow(TableCell tierCell)
        {
            return tierCell.ContainingTableRow;
        }

        /// <summary>
        /// Uses TierRow(TableCell tierCell) to identify the TableRow from a TableCell and then identifies the ExpandButton Image in that row
        /// </summary>
        /// <param name="tierCell">TierRow(TableCell tierCell)</param>
        /// <returns>The ExpandButton Image in a tier</returns>
        protected Image ExpandButton(TableCell tierCell)
        {
            return TierRow(tierCell).Image(Find.ByClass(new System.Text.RegularExpressions.Regex(@"dxTreeList_CollapsedButton_SoftOrange dxtl__Expand")));
        }

        /// <summary>
        /// Uses TierRow(TableCell tierCell) to identify the TableRow from a TableCell and then identifies the ColapseButton Image in that row
        /// </summary>
        /// <param name="tierCell">TierRow(TableCell tierCell)</param>
        /// <returns>The ExpandButton Image in a tier</returns>
        protected Image ColapseButton(TableCell tierCell)
        {
            return TierRow(tierCell).Image(Find.ByClass(new System.Text.RegularExpressions.Regex(@"dxTreeList_ExpandedButton_SoftOrange dxtl__Collapse")));
        }

        /// <summary>
        /// Search Field for the DebtCounsellor view
        /// </summary>
        [FindBy(Id = "ctl00_Main_txtSearchCriteria")]
        protected TextField SearchCriteria;
    }
}