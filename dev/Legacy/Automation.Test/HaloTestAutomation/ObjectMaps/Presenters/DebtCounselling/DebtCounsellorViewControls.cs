using WatiN.Core;

namespace ObjectMaps
{
    public class DebtCounsellorViewControls
    {
        private Browser _window;
        private Frame _frame;

        public DebtCounsellorViewControls(Browser window)
        {
            _window = window;
            _frame = _window.Frame(Find.ByIndex(0));
        }

        public Button btnAdd
        {
            get
            {
                return _frame.Button(Find.ById("ctl00_Main_btnAdd"));
            }
        }

        public Button btnRemove
        {
            get
            {
                return _frame.Button(Find.ById("ctl00_Main_btnRemove"));
            }
        }

        public Button btnUpdate
        {
            get
            {
                return _frame.Button(Find.ById("ctl00_Main_btnUpdate"));
            }
        }

        public Button btnView
        {
            get
            {
                return _frame.Button(Find.ById("ctl00_Main_btnView"));
            }
        }

        public Button btnSelect
        {
            get
            {
                return _frame.Button(Find.ById("ctl00_Main_btnSelect"));
            }
        }

        public Button btnAddToMenu
        {
            get
            {
                return _frame.Button(Find.ById("ctl00_Main_btnAddToCBO"));
            }
        }

        public Span Description(string Description)
        {
            return _frame.Span(Find.ByText(Description));
        }

        /// <summary>
        /// The encapsulating table for the 'Payment Distribution Agencies / Agents' tree structure
        /// </summary>
        public Table tblOrgStructure
        {
            get
            {
                return _frame.Table(Find.ById("ctl00_Main_tlOrgStructure_D"));
            }
        }

        /// <summary>
        /// The span element encapsulating the label in the 'Description' column of the 'Payment Distribution Agencies / Agents' table
        /// </summary>
        /// <param name="description">Description</param>
        /// <returns>The span element encapsulating the label in the 'Description' column of the 'Payment Distribution Agencies / Agents' table</returns>
        public Span spanDescription(string description)
        {
            return tblOrgStructure.Span(Find.ByText(description));
        }

        /// <summary>
        /// Identifies a cell in the 'Payment Distribution Agencies / Agents' table that matches a given string.  Expect this to be a cell in the 'Display Name' column
        /// </summary>
        /// <param name="description">DisplayName</param>
        /// <returns>A TableCell in the 'Payment Distribution Agencies / Agents'</returns>
        public TableCell DisplayNameCell(string description)
        {
            return tblOrgStructure.TableCell(Find.ByText(description));
        }

        /// <summary>
        /// The encapsulating TableCell of the span element identified by spanDescription
        /// </summary>
        /// <param name="description">description</param>
        /// <returns>The encapsulating TableCell of the span element identified by spanDescription</returns>
        public TableCell DescriptionCell(string description)
        {
            return spanDescription(description).Parent as TableCell;
        }

        /// <summary>
        /// Gets the encapsulating TableCell of a TableCell element
        /// </summary>
        /// <param name="tierCell">DisplayNameCell(string description), DescriptionCell(string description)</param>
        /// <returns>The encapsulating TableCell of a TableCell element for a tier</returns>
        public TableRow TierRow(TableCell tierCell)
        {
            return tierCell.ContainingTableRow;
        }

        /// <summary>
        /// Uses TierRow(TableCell tierCell) to identify the TableRow from a TableCell and then identifies the ExpandButton Image in that row
        /// </summary>
        /// <param name="tierCell">TierRow(TableCell tierCell)</param>
        /// <returns>The ExpandButton Image in a tier</returns>
        public Image ExpandButton(TableCell tierCell)
        {
            return TierRow(tierCell).Image(Find.ByClass(new System.Text.RegularExpressions.Regex(@"dxTreeList_CollapsedButton_SoftOrange dxtl__Expand")));
        }

        /// <summary>
        /// Uses TierRow(TableCell tierCell) to identify the TableRow from a TableCell and then identifies the ColapseButton Image in that row
        /// </summary>
        /// <param name="tierCell">TierRow(TableCell tierCell)</param>
        /// <returns>The ExpandButton Image in a tier</returns>
        public Image ColapseButton(TableCell tierCell)
        {
            return TierRow(tierCell).Image(Find.ByClass(new System.Text.RegularExpressions.Regex(@"dxTreeList_ExpandedButton_SoftOrange dxtl__Collapse")));
        }

        /// <summary>
        /// Search Field for the DebtCounsellor view
        /// </summary>
        public TextField SearchCriteria
        {
            get
            {
                return _frame.TextField("ctl00$Main$txtSearchCriteria");
            }
        }
    }
}