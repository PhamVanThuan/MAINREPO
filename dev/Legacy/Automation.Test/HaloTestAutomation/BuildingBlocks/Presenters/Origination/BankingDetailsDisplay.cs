using NUnit.Framework;
using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.Origination
{
    public class BankingDetailsDisplay : BankingDetailsDisplayControls
    {
        protected enum ColumnHeader
        {
            Bank = 1,
            Branch = 2,
            AccountType = 3,
            AccountNumber = 4,
            AccountName = 5,
            Status = 7
        }

        /// <summary>
        /// Loops through all rows in the Bank Details grid looking for Bank Details that match the expected Bank Details
        /// </summary>
        /// <param name="b"></param>
        /// <param name="expectedBank"></param>
        /// <param name="expectedAccountType"></param>
        /// <param name="expectedAccountNumber"></param>
        /// <param name="expectedAccountName"></param>
        /// <param name="expectedStatus"></param>
        public void ValidateBankingDetailsDisplayed(string expectedBank, string expectedAccountType, string expectedAccountNumber,
            string expectedAccountName, string expectedStatus)
        {
            bool result = false;

            for (int i = 1; i < base.tblRowsBankDetailsGrid.Count; i++)
            {
                string actualBank = base.tblCellBankDetailsGrid(i, (int)ColumnHeader.Bank).Text;
                string actualAccountType = base.tblCellBankDetailsGrid(i, (int)ColumnHeader.AccountType).Text;
                string actualAccountNumber = base.tblCellBankDetailsGrid(i, (int)ColumnHeader.AccountNumber).Text;
                string actualStatus = base.tblCellBankDetailsGrid(i, (int)ColumnHeader.Status).Text;
                if (actualBank == expectedBank && actualAccountType == expectedAccountType && actualAccountNumber == expectedAccountNumber && actualStatus == expectedStatus)
                {
                    result = true;
                    break;
                }
            }

            Assert.IsTrue(result,
                "Bank details of Bank: {0}, Account Type: {1}, Account Number: {2}, Account Name: {3}, Status: {4}, is not displayed in the Bank Details grid");
        }
    }
}