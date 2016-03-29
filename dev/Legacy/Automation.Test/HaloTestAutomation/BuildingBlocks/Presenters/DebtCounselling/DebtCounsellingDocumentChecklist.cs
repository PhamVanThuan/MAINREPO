using Common.Constants;
using NUnit.Framework;
using ObjectMaps.Pages;
using System;
using WatiN.Core;
using WatiN.Core.Logging;

namespace BuildingBlocks.Presenters.DebtCounselling
{
    /// <summary>
    /// DebtCounsellingDocumentChecklist screen
    /// </summary>
    [Page]
    public class DebtCounsellingDocumentChecklist : BasePageControls
    {
        /// <summary>
        /// Captures a new date
        /// </summary>
        /// <param name="newDate">New date value</param>
        /// <param name="comment">Comment field value</param>
        /// <param name="dateType">The date type to capture</param>
        public void CaptureDate(DateTime newDate, string comment, string dateType)
        {
            MainTableRow(dateType).Click();
            Date.Value = newDate.ToString(Formats.DateFormat);
            Comment.TypeText(comment);
            btnSave.Click();
        }

        /// <summary>
        /// Asserts that a row exists in the grid
        /// </summary>
        /// <param name="columnValue"></param>
        public void AssertRowExists(string columnValue)
        {
            Logger.LogAction(@"Asserting that the DebtCounsellingDocumentChecklist view contains a grid row with a column value of {0}", columnValue);
            Assert.That(this.MainTableRowExists(columnValue), "Row with column name was not found.");
        }

        /// <summary>
        /// The new date value
        /// </summary>
        [FindBy(Id = "ctl00_Main_dteNewDate")]
        public TextField Date;

        /// <summary>
        /// The comments field
        /// </summary>
        [FindBy(Id = "ctl00_Main_txtComments")]
        public TextField Comment;

        /// <summary>
        /// The Save button
        /// </summary>
        [FindBy(Id = "ctl00_Main_btnSave")]
        public Button btnSave;

        /// <summary>
        /// The cancel button
        /// </summary>
        [FindBy(Id = "ctl00_Main_btnCancel")]
        public Button btnCancel;

        /// <summary>
        /// The table that contains the dates
        /// </summary>
        [FindBy(Id = "ctl00_Main_grdDate")]
        public Table MainTable;

        /// <summary>
        /// Looks in the main grid table for a row that has a column value matching the string provided.
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns>TableRow</returns>
        public TableRow MainTableRow(string columnName)
        {
            TableCellCollection cellCollection = MainTable.TableCells;
            return cellCollection.Filter(Find.ByText(columnName))[0].ContainingTableRow;
        }

        /// <summary>
        /// Looks in the main grid table for a row that has a column value matching the string provided. Returns true if it finds it, false if it does not.
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns>bool, TRUE = row exists, FALSE = row does not exist</returns>
        public bool MainTableRowExists(string columnName)
        {
            return MainTableRow(columnName).Exists;
        }
    }
}