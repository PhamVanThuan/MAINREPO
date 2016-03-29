using BuildingBlocks.Assertions;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using ObjectMaps.Pages;
using System.Collections.Generic;
using System.Threading;
using WatiN.Core;
using WatiN.Core.Logging;

namespace BuildingBlocks.Presenters.Origination
{
    public class BankingDetails : BankingDetailsAddControls
    {
        private readonly IWatiNService watinService;
        private readonly IBankingDetailsService bankingDetailsService;

        public BankingDetails()
        {
            watinService = ServiceLocator.Instance.GetService<IWatiNService>();
            bankingDetailsService = ServiceLocator.Instance.GetService<IBankingDetailsService>();
        }

        /// <summary>
        /// Add bank details.  The user needs to provide the relevant bank details
        /// </summary>
        /// <param name="bankAccount"></param>
        /// <param name="Button"></param>
        public void AddBankingDetails(Automation.DataModels.BankAccount bankAccount, ButtonTypeEnum Button)
        {
            if (!string.IsNullOrEmpty(bankAccount.ACBBankDescription))
                base.ddlBank.Option(bankAccount.ACBBankDescription).Select();
            if (!string.IsNullOrEmpty(bankAccount.ACBBranchCode))
            {
                base.txtBranch.TypeText(bankAccount.ACBBranchCode);
                base.SAHLAutoCompleteDiv_iframe.WaitUntilExists();
                base.SAHLAutoComplete_DefaultItem(bankAccount.ACBBranchCode).MouseDown();
            }
            if (!string.IsNullOrEmpty(bankAccount.ACBTypeDescription))
                base.ddlAccountType.Option(bankAccount.ACBTypeDescription).Select();

            if (!string.IsNullOrEmpty(bankAccount.AccountNumber))
                base.txtAccountNumber.TypeText(bankAccount.AccountNumber);

            if (!string.IsNullOrEmpty(bankAccount.AccountName))
                base.txtAccountName.TypeText(bankAccount.AccountName);

            if (!string.IsNullOrEmpty(bankAccount.ACBTypeDescription))
                base.ddlAccountType.Option(bankAccount.ACBTypeDescription).Select();

            switch (Button)
            {
                case ButtonTypeEnum.Add:
                    base.SubmitButton.Click();
                    break;

                case ButtonTypeEnum.Cancel:
                    base.CancelButton.Click();
                    break;

                case ButtonTypeEnum.Search:
                    base.SearchButton.Click();
                    break;
            }
            Thread.Sleep(2500);
        }

        /// <summary>
        /// Add bank details.  Automatically fetches valid unused bank details and populates the relevant fields
        /// </summary>
        /// <param name="accountName"></param>
        /// <param name="button"></param>
        public void AddBankingDetails(string accountName, ButtonTypeEnum button)
        {
            var bankAcc = bankingDetailsService.GetNextUnusedBankAccountDetails();
            bankAcc.AccountName = accountName;
            AddBankingDetails(bankAcc, button);
        }

        /// <summary>
        /// Add settelment bank details. The user needs to provide the relevant bank details
        /// </summary>
        /// <param name="bank"></param>
        /// <param name="branch"></param>
        /// <param name="accountType"></param>
        /// <param name="accountNumber"></param>
        /// <param name="accountName"></param>
        /// <param name="reference"></param>
        /// <param name="Button"></param>
        public void AddSettlementBankingDetails(Automation.DataModels.BankAccount bankAccount, string reference, ButtonTypeEnum Button)
        {
            base.ddlBank.Option(bankAccount.ACBBankDescription).Select();
            base.txtBranch.TypeText(bankAccount.ACBBranchCode);
            base.SAHLAutoCompleteDiv_iframe.WaitUntilExists();
            base.SAHLAutoComplete_DefaultItem(bankAccount.ACBBranchCode).MouseDown();
            base.ddlAccountType.Option(bankAccount.ACBTypeDescription).Select();
            base.txtAccountNumber.TypeText(bankAccount.AccountNumber);
            base.txtAccountName.TypeText(bankAccount.AccountName);
            base.txtReference.TypeText(reference);

            switch (Button)
            {
                case ButtonTypeEnum.Add:
                    base.SubmitButton.Click();
                    break;

                case ButtonTypeEnum.Cancel:
                    base.CancelButton.Click();
                    break;
            }
        }

        /// <summary>
        /// Add settlement bank details.  Automatically fetches valid unused bank details and populates the relevant fields
        /// </summary>
        /// <param name="accountName"></param>
        /// <param name="reference"></param>
        /// <param name="button"></param>
        public void AddSettlementBankingDetails(Automation.DataModels.BankAccount bankAccount, string accountName, string reference, ButtonTypeEnum button)
        {
            var bankAcc = bankingDetailsService.GetNextUnusedBankAccountDetails();
            bankAcc.AccountName = accountName;
            this.AddSettlementBankingDetails(bankAcc, reference, button);
        }

        /// <summary>
        /// Updates banking details. passing a null value for either the accountName, accountType or bankAccountStatus will not update
        /// that value.
        /// </summary>
        /// <param name="accountName">New Account Name</param>
        /// <param name="accountType">Account Type</param>
        /// <param name="bankAccountStatus">Bank Account Status</param>
        public void UpdateBankingDetails(string accountName, string accountType, string bankAccountStatus)
        {
            if (accountName != null) //want to be able to update the string to an empty string for validation testing therefore have not used string.IsNullOrEmpty()
            {
                base.txtAccountName.Clear();
                base.txtAccountName.Value = accountName;
            }
            if (!string.IsNullOrEmpty(accountType))
            {
                base.ddlAccountType.Option(accountType).Select();
            }
            if (!string.IsNullOrEmpty(bankAccountStatus))
            {
                base.ddlStatus.Option(bankAccountStatus).Select();
            }
            base.SubmitButton.Click();
            Thread.Sleep(2500);
        }

        /// <summary>
        /// Updates banking details. passing a null value for either the accountName or bankAccountStatus will not update
        /// that value, passing -1 for accountType will not update that value.
        /// </summary>
        /// <param name="b">TestBrowser</param>
        /// <param name="accountName">New Account Name</param>
        /// <param name="accountType">Account Type</param>
        /// <param name="bankAccountStatus">Bank Account Status</param>
        public void UpdateBankingDetails(string accountName, int accountType, string bankAccountStatus)
        {
            Assert.AreEqual(base.ViewName.Text, "BankingDetailsUpdate");
            if (accountName != null) //want to be able to update the string to an empty string for validation testing therefore have not used string.IsNullOrEmpty()
            {
                base.txtAccountName.Clear();
                base.txtAccountName.Value = accountName;
            }
            if (accountType != -1)
            {
                base.ddlAccountType.Options[accountType].Select();
            }
            if (!string.IsNullOrEmpty(bankAccountStatus))
            {
                base.ddlStatus.Option(bankAccountStatus).Select();
            }
            base.SubmitButton.Click();
            Thread.Sleep(2500);
        }

        /// <summary>
        /// Deletes a bank account record
        /// </summary>
        public void DeleteBankAccount()
        {
            watinService.HandleConfirmationPopup(base.SubmitButton);
        }

        #region Assertions

        /// <summary>
        /// This assertion makes sure that the Bank, Branch and Account Number fields are not available in update mode.
        /// </summary>
        /// <param name="b"></param>
        public void AssertUpdateFieldsDisabled()
        {
            var updateControls = new List<Element>() {
                    base.ddlBank,
                    base.txtBranch,
                    base.txtAccountNumber
                };

            WatiNAssertions.AssertFieldsDoNotExist(updateControls);
        }

        /// <summary>
        /// Assert that the Bank Acount identified by Account Number is displayed in the Bank Details grid
        /// </summary>
        /// <param name="accountNumbers">list of Account Numbers</param>
        public void AssertBankingDetailsDisplayed(List<string> accountNumbers)
        {
            foreach (string accountnumber in accountNumbers)
            {
                Logger.LogAction(string.Format(@"Asserting that Account Number: {0} exists in the 'Banking Details' grid", accountnumber));
                Assert.True(base.BankDetailsGrid_Row(accountnumber).Exists,
                    string.Format(@"The Bank Details identified by Account Number: {0} are not displayed in the 'Banking Details' grid",
                    accountnumber));
            }
        }

        /// <summary>
        /// Assert the expected controls exist on the BankDetailsAdd view
        /// </summary>
        public void AssertBankingDetailsAddControlsExist()
        {
            var addControls = new List<Element>() {
                    base.ddlBank,
                    base.txtBranch,
                    base.ddlAccountType,
                    base.txtAccountNumber,
                    base.txtAccountName,
                    base.CancelButton,
                    base.SubmitButton,
                    base.SearchButton
                };

            WatiNAssertions.AssertFieldsExist(addControls);
            WatiNAssertions.AssertFieldsAreEnabled(addControls);
        }

        /// <summary>
        /// Assert the expected controls exist on the BankDetailsDelete view
        /// </summary>
        public void AssertBankingDetailsDeleteControlsExist()
        {
            var deleteControls = new List<Element>() {
                    base.CancelButton,
                    base.SubmitButton,
                };

            WatiNAssertions.AssertFieldsExist(deleteControls);
            WatiNAssertions.AssertFieldsAreEnabled(deleteControls);
        }

        /// <summary>
        /// Assert the expected options exist in the Account Type dropdown list
        /// </summary>
        public void AssertAccountTypeOptions()
        {
            List<string> accountTypes = new List<string>() {
                    ACBType.Current,
                    ACBType.Savings,
                    ACBType.Bond,
                    ACBType.CreditCard
                };
            WatiNAssertions.AssertSelectListContents(base.ddlAccountType, accountTypes);
        }

        /// <summary>
        /// Assert the expected options exist in the Status dropdown list
        /// </summary>
        public void AssertStatusOptions()
        {
            List<string> status = new List<string>() {
                    GeneralStatusEnum.Active.ToString(),
                    GeneralStatusEnum.Inactive.ToString()
                };
            WatiNAssertions.AssertSelectListContents(base.ddlStatus, status);
        }

        #endregion Assertions
    }
}