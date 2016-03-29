using Automation.DataAccess;
using Common.Constants;
using NUnit.Framework;
using ObjectMaps.Pages;
using System;
using System.Linq;
using System.Threading;
using WatiN.Core;

namespace BuildingBlocks.Presenters.Admin
{
    /// <summary>
    /// Administration / User Status Maintenance
    /// </summary>
    public class AdminUserStatusMaintenance : AdminUserStatusMaintenanceControls
    {
        /// <summary>
        /// Select Role Type
        /// </summary>
        /// <param name="roleType"></param>
        public void SelectRoleType(string roleType)
        {
            try
            {
                base.RoleTypesDropDownList.Select(roleType);
            }
            catch (UnauthorizedAccessException)
            {
                //Attempt to gain access to the RoleTypesDropDownList again
                SelectRoleType(roleType);
            }
            Thread.Sleep(1000);
        }

        /// <summary>
        /// Set AD User Status
        /// </summary>
        /// <param name="adUsername"></param>
        /// <param name="status"></param>
        public void SetUserStatus(string adUsername, string status)
        {
            //Find the row with the user in it
            TableRow row = FindADUserRowInTable(adUsername);

            //Select the row to toggle the editing
            if (!row.Exists || row == null)
            {
                return;
            }

            row.Click();
            Thread.Sleep(1000);

            //Activate the drop down list for the User status
            base.UserStatusDropDownList.MouseDown();
            Thread.Sleep(1000);
            switch (status)
            {
                case ADUserStatus.Active:
                    base.ActiveUserStatusCell.MouseUp();
                    break;

                case ADUserStatus.Inactive:
                    base.InactiveUserStatusCell.MouseUp();
                    break;
            }
            Thread.Sleep(1000);
        }

        /// <summary>
        /// Set Round Robin Status
        /// </summary>
        /// <param name="adUsername"></param>
        /// <param name="status"></param>
        public void SetRoundRobinStatus(string adUsername, string status)
        {
            //Find the row with the user in it
            TableRow row = FindADUserRowInTable(adUsername);

            //Select the row to toggle the editing
            if (!row.Exists || row == null)
            {
                return;
            }

            row.Click();
            Thread.Sleep(1000);

            //Activate the drop down list for the User status
            base.RoundRobinDropDownList.MouseDown();
            Thread.Sleep(1000);
            switch (status)
            {
                case ADUserStatus.Active:
                    base.ActiveRoundRobinCell.MouseUp();
                    break;

                case ADUserStatus.Inactive:
                    base.InactiveRoundRobinCell.MouseUp();
                    break;
            }
            Thread.Sleep(1000);
        }

        /// <summary>
        /// Get User Rows In Page
        /// </summary>
        /// <returns></returns>
        public int GetUserRowCountInPage()
        {
            return base.UserStatusMaintenanceTable.TableRows.Filter((tableRow) => tableRow.Index >= 1).Count;
        }

        /// <summary>
        /// Check the items in the DDL match what we expect from the DB
        /// </summary>
        /// <param name="qr"></param>
        public void ConfirmRoleTypesDDLItems(QueryResults qr)
        {
            SelectList ddl = base.RoleTypesDropDownList;

            //assert - count is equal and each item in the QR is in the dll list
            Assert.AreEqual(ddl.Options.Count - 1, qr.RowList.Count);

            //check ddl items
            foreach (QueryResultsRow row in qr.RowList)
            {
                Assert.IsTrue(ddl.Option(row.Column(0).Value).Exists);
            }
        }

        /// <summary>
        /// Reset Navigation
        /// </summary>
        public void ResetPaging()
        {
            bool hasReset = PreviousPage();
            while (hasReset)
            {
                hasReset = PreviousPage();
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// Navigate to the Next Page
        /// </summary>
        /// <returns></returns>
        public bool NextPage()
        {
            if (base.NextButtonImage.Exists)
            {
                base.NextButtonImage.Click();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Navigate to the Previous Page
        /// </summary>
        /// <returns></returns>
        public bool PreviousPage()
        {
            if (base.PreviousButtonImage.Exists)
            {
                base.PreviousButtonImage.Click();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Click the Submit Button
        /// </summary>
        public void Submit()
        {
            base.SubmitButton.Click();
        }

        /// <summary>
        /// Click the Cancel Button
        /// </summary>
        public void Cancel()
        {
            base.CancelButton.Click();
        }

        /// <summary>
        /// Find AD User in Table
        /// </summary>
        /// <param name="adUsername"></param>
        /// <returns></returns>
        private TableRow FindADUserRowInTable(string adUsername)
        {
            TableRowCollection rows = base.UserStatusMaintenanceTable.TableRows.Filter((tableRow) => tableRow.Index >= 1);
            return rows.FirstOrDefault(row => row.Text.Contains(adUsername));
        }
    }
}