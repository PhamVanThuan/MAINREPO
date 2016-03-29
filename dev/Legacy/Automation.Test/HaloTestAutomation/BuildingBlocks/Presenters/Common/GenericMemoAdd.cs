using ObjectMaps.Pages;
using System.Collections.Generic;
using System.Threading;
using WatiN.Core;

namespace BuildingBlocks.Presenters.CommonPresenters
{
    /// <summary>
    /// Contains building blocks for the adding and updating of Memo records.
    /// </summary>
    public class GenericMemoAdd : GenericMemoControls
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="status"></param>
        /// <param name="memoText"></param>
        public void AddMemoRecord(string status, string memoText)
        {
            AddMemoRecord(status, memoText, false);
        }

        /// <summary>
        /// Adds a memo record using the currently navigated to memo screen
        /// </summary>
        /// <param name="status">Status of the memo to be added</param>
        /// <param name="memoText">Memo text of the memo to be added</param>
        /// <param name="continueWithWarnings">IE TestBrowser</param>
        public void AddMemoRecord(string status, string memoText, bool continueWithWarnings)
        {
            base.MemoText.Value = memoText;
            base.ddlMemoStatus.Option(status).Select();
            base.btnAdd.Click();
            Thread.Sleep(1500);
            if (continueWithWarnings)
                base.Document.Page<BasePage>().DomainWarningClickYes();
        }

        /// <summary>
        /// This building block will add a memo record without providing a memo text
        /// </summary>
        /// <param name="status">Memo Status</param>
        public void AddMemoWithoutMemoText(string status)
        {
            base.MemoText.Clear();
            base.ddlMemoStatus.Option(status).Select();
            base.btnAdd.Click();
        }

        /// <summary>
        /// This building block will add a memo record without providing a reminder date
        /// </summary>
        /// <param name="status">Memo Status</param>
        public void AddMemoWithoutReminderDate(string status)
        {
            base.MemoText.Value = "Memo No Reminder Date";
            base.ddlMemoStatus.Option(status).Select();
            base.ReminderDate.Clear();
            base.btnAdd.Click();
        }

        /// <summary>
        /// This building block will add a memo record without providing an expiry date
        /// </summary>
        /// <param name="status">Memo Status</param>
        public void AddMemoWithoutExpiryDate(string status)
        {
            base.MemoText.Value = "Memo No Expiry Date";
            base.ddlMemoStatus.Option(status).Select();
            base.ExpiryDate.Clear();
            base.btnAdd.Click();
        }

        /// <summary>
        /// Updates a memo record by selecting a record from the grid by using the columnValue variable which is a
        /// description of the column used to find a grid row.
        /// </summary>
        /// <param name="columnValue">value to find row by</param>
        /// <param name="status">MemoStatus</param>
        /// <param name="newMemoText">New Memo Description to update to</param>
        public void UpdateMemo(string columnValue, string status, string newMemoText)
        {
            //find the grid record
            base.gridSelectMemo(columnValue);
            base.MemoText.Clear();
            base.MemoText.Value = newMemoText;
            base.ddlMemoStatus.Option(status).Select();
            base.btnAdd.Click();
        }

        /// <summary>
        /// Updates a memo record by selecting a record from the grid by using the columnValue variable which is a
        /// description of the column used to find a grid row.
        /// </summary>
        /// <param name="status">MemoStatus</param>
        /// <param name="newMemoText">New Memo Description to update to</param>
        public void UpdateMemo(int memoKey, string status, string newMemoText)
        {
            //find the grid record
            base.gridSelectMemo(memoKey.ToString());
            base.MemoText.Clear();
            base.MemoText.Value = newMemoText;
            base.ddlMemoStatus.Option(status).Select();
            base.btnAdd.Click();
        }

        /// <summary>
        /// Returns a boolean value to check if the update button exists
        /// </summary>
        /// <returns></returns>
        public bool UpdateButtonExists()
        {
            return base.btnAdd.Exists;
        }

        /// <summary>
        /// Selects a memo record by its memo key
        /// </summary>
        /// <param name="memoKey"></param>
        public void SelectMemoRecord(int memoKey)
        {
            base.gridSelectMemo(memoKey.ToString());
        }

        /// <summary>
        /// Asserts that the add fields exist and are enabled.
        /// </summary>
        private List<Element> EnabledAddFields
        {
            get
            {
                var list = new List<Element>() {
                    base.MemoText, base.ddlMemoStatus, base.ReminderDate, base.btnAdd,
                    base.btnCancel, base.ExpiryDate
                };
                return list;
            }
        }

        public void AssertMemoStatusList(List<string> expectedContents)
        {
            Assertions.WatiNAssertions.AssertSelectListContents(base.ddlMemoStatus, expectedContents);
        }

        public void AssertAccountStatusList(List<string> expectedContents)
        {
            Assertions.WatiNAssertions.AssertSelectListContents(base.AccountStatus, expectedContents);
        }

        public void AssertAddFieldsExistsAndEnabled()
        {
            Assertions.WatiNAssertions.AssertFieldsExist(this.EnabledAddFields);
            Assertions.WatiNAssertions.AssertFieldsAreEnabled(this.EnabledAddFields);
        }
    }
}